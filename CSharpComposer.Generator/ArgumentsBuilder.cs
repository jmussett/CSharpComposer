using Humanizer;
using SyntaxBuilder;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator.Models;

namespace CSharpComposer.Generator;

internal class ArgumentsBuilder
{
    private readonly Tree _tree;
    private readonly EnumStore _enumStore;

    public ArgumentsBuilder(Tree tree, EnumStore enumStore)
    {
        _tree = tree;
        _enumStore = enumStore;
    }

    public List<string> WithArguments(IBlockBuilder blockBuilder, Node node, bool createArguments, string? prefix = null)
    {
        var arguments = new List<string>();

        if (node.Kinds.Count > 1 && !NodeValidator.IsTokenized(node))
        {
            var typeName = NameFactory.CreateTypeName(node.Name);

            var enumName = $"{typeName}Kind";
            _enumStore.TryAddEnum(enumName, node);

            var kindArgument = prefix is null ? "kind" : $"{prefix}Kind";

            if (createArguments)
            {
                var variableName = $"syntaxKind";

                // TODO: Build with builder
                // TODO: Not supported message??
                blockBuilder.WithStatement($$"""
                        var {{variableName}} = {{kindArgument}} switch
                            {
                                {{string.Join("\r\n", node.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct().Select(x =>
                                    $"{enumName}.{x} => SyntaxKind.{x},"))}}
                                _ => throw new NotSupportedException()
                            };
                        """);

                arguments.Add(variableName);
            }
            else
            {
                arguments.Add(kindArgument);
            }
        }

        foreach (var child in node.Children)
        {
            if (createArguments)
            {
                if (child is Choice choice)
                {
                    foreach (var choiceChild in choice.Children)
                    {
                        if (choiceChild is Sequence sequence)
                        {
                            foreach (var sequenceChild in sequence.Children)
                            {
                                if (sequenceChild is Choice sequenceChoice)
                                {
                                    foreach (var sequenceChoiceChild in sequenceChoice.Children)
                                    {
                                        arguments.Add($"null");
                                    }
                                }
                                else if (sequenceChild is Field sequenceField && (sequenceField.IsToken || NodeValidator.IsAnyList(sequenceField.Type)))
                                {
                                    arguments.Add($"default({sequenceField.Type})");
                                }
                                else
                                {
                                    arguments.Add($"null");
                                }

                                continue;
                            }
                        }
                        else if (choiceChild is Field choideField && (choideField.IsToken || NodeValidator.IsAnyList(choideField.Type)))
                        {
                            arguments.Add($"default({choideField.Type})");
                        }
                        else
                        {
                            arguments.Add($"null");
                        }

                        continue;
                    }
                }

                if (child is Field childField)
                {
                    if (createArguments && 
                        !childField.IsOptional && 
                        childField.Kinds.Count > 1 &&
                        childField.Kinds.Count == node.Kinds.Count && 
                        childField.IsToken && 
                        ((childField.Name?.EndsWith("Keyword") ?? false) || childField.Name == "OperatorToken")
                    )
                    {
                        var typeName = NameFactory.CreateTypeName(node.Name);

                        var enumName = $"{typeName}Kind";

                        var kindArgument = prefix is null ? "kind" : $"{prefix}Kind";

                        var variableName = $"{childField.Name.Camelize()}Token";

                        // TODO: Build with builder
                        // TODO: Not supported message??
                        blockBuilder.WithStatement(
                            $$"""
                            var {{variableName}} = {{kindArgument}} switch
                                {
                                    {{string.Join("\r\n", node.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct().Select((x, i) =>
                                                $"{enumName}.{x} => SyntaxFactory.Token(SyntaxKind.{childField.Kinds[i].Name}),"))}}
                                    _ => throw new NotSupportedException()
                                };
                            """
                        );

                        arguments.Add(variableName);
                    }

                    //if (!childField.IsOptional && childField.Kinds.Count > 1 && childField.IsToken && childField.Name == "Token")
                    //{
                    //    // TODO:
                    //    //arguments.Add("null");
                    //}

                    if (childField.IsOptional || NodeValidator.IsAnyNodeList(childField.Type))
                    {
                        var fieldType = childField.Type;

                        if (fieldType == "SyntaxList<SyntaxToken>")
                        {
                            fieldType = "SyntaxTokenList";
                        }

                        arguments.Add($"default({fieldType})");
                    }
                    else if (!childField.IsOptional && NodeValidator.IsConcreteList(childField.Type))
                    {
                        var variableName = $"{childField.Name.Camelize()}Syntax";

                        var listTypeName = NameFactory.CreateTypeName(childField.Type);

                        blockBuilder.WithStatement($"var {variableName} = SyntaxFactory.{listTypeName}();");

                        arguments.Add(variableName);
                    }
                }
            }

            if (child is not Field field)
            {
                continue;
            }

            if (!field.IsOptional && field.IsToken &&
                (
                    field.Name == "Identifier" ||
                    (field.Kinds.Count == 1 && field.Kinds.FirstOrDefault()?.Name == "IdentifierToken")
                )
            )
            {
                var identifierName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                if (createArguments && node.Name != "IdentifierNameSyntax")
                {
                    var identifierArgument = $"{identifierName}Token";

                    blockBuilder.WithStatement($"var {identifierArgument} = SyntaxFactory.Identifier({NameFactory.CreateSafeIdentifier(identifierName)});");

                    arguments.Add(identifierArgument);
                }
                else
                {
                    arguments.Add(NameFactory.CreateSafeIdentifier(identifierName));
                }
            }
            else if (!field.IsOptional && field.Kinds.Count > 1 && field.IsToken &&
                // Exclude keywords if node has multiple kinds
                (node.Kinds.Count <= 1 || (!(field.Name?.EndsWith("Keyword") ?? false) && field.Name != "OperatorToken" && field.Name != "Token")))
            {
                var typeName = NameFactory.CreateTypeName(node.Name);

                var enumName = $"{typeName}{field.Name}";

                var enumArgument = prefix is null
                    ? enumName.Camelize()
                    : $"{prefix}{enumName}";

                if (createArguments)
                {
                    var variableName = $"{field.Name.Camelize()}Token";

                    // TODO: Build with builder
                    // TODO: Not supported message??
                    blockBuilder.WithStatement($$"""
                        var {{variableName}} = {{NameFactory.CreateSafeIdentifier(enumArgument)}} switch
                            {
                                {{string.Join("\r\n", field.Kinds.Where(x => x.Name != "IdentifierToken").Select(x => x.Name).Distinct().Select(x =>
                                        $"{enumName}.{x} => SyntaxFactory.Token(SyntaxKind.{x}),"))}}
                                _ => throw new NotSupportedException()
                            };
                        """);

                    arguments.Add(variableName);
                }
                else
                {
                    arguments.Add(NameFactory.CreateSafeIdentifier(enumArgument));
                }
            }
            else if (!field.IsOptional && field.Kinds.Count == 1 && (field.Kinds.FirstOrDefault()?.Name.EndsWith("LiteralToken") ?? false))
            {
                var literalName = prefix is null
                    ? $"{field.Name.Camelize()}"
                    : $"{prefix}{field.Name}";

                if (createArguments)
                {
                    blockBuilder.WithStatement($"var {literalName}Token = SyntaxFactory.Literal({NameFactory.CreateSafeIdentifier(literalName)});");

                    arguments.Add($"{literalName}Token");
                }
                else
                {
                    arguments.Add(literalName);
                }
            }
            else if (!field.IsOptional && createArguments && field.IsToken &&
                (field.Kinds.Count == 1 || field.IsOverride || field.Name == "ColonToken" || field.Name == "CommaToken"))
            {
                var tokenArgument = prefix is null
                    ? $"{field.Name.Camelize()}"
                    : $"{prefix}{field.Name}";

                var tokenName = field.Kinds.FirstOrDefault()?.Name ?? field.Name;

                blockBuilder.WithStatement($"var {tokenArgument}Token = SyntaxFactory.Token(SyntaxKind.{tokenName});");

                arguments.Add($"{tokenArgument}Token");
            }

            if (field.IsOptional) continue;

            if (NodeValidator.IsSyntaxNode(field.Type) &&
                !NodeValidator.IsAnyList(field.Type))
            {

                var referenceType = _tree.Types.FirstOrDefault(x => x.Name == field.Type);

                var builderName = NameFactory.CreateBuilderName(field.Type);

                var variableName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                // If we are a regular node and do not have an interface, use parameters from the node's CreateSyntax method.
                if (referenceType is Node referenceTypeNode && NodeValidator.HasMandatoryChildren(referenceTypeNode))
                {
                    var newPrefix = prefix is null ? field.Name.Camelize() : $"{field.Name.Camelize()}{prefix.Pascalize()}";

                    var referencedArguments = WithArguments(blockBuilder, referenceTypeNode, false, newPrefix);

                    if (createArguments)
                    {
                        variableName = $"{variableName}Syntax";

                        blockBuilder.WithStatement($"var {variableName} = {builderName}.CreateSyntax({string.Join(", ", referencedArguments)});");

                        arguments.Add(variableName);
                    }
                    else
                    {
                        arguments.AddRange(referencedArguments);
                    }

                    continue;
                }

                if (createArguments)
                {
                    variableName = $"{variableName}Syntax";

                    blockBuilder.WithStatement($"var {variableName} = {builderName}.CreateSyntax({field.Name.Camelize()}Callback);");

                    arguments.Add(variableName);
                }
                else
                {
                    arguments.Add($"{variableName}Callback");
                }
            }

            if (field.Type == "bool")
            {
                var variableName = prefix is null
                     ? $"{field.Name.Camelize()}"
                     : $"{prefix}{field.Name}";

                arguments.Add(variableName);
            }
        }

        // If we arent creating SyntaxFactory arguments,
        // we need the callback for the builder when forwarding arguments to CreateSyntax methods.
        if (!createArguments && (NodeValidator.HasOptionalChildren(node) || NodeValidator.IsTokenized(node)))
        {
            var typeName = NameFactory.CreateTypeName(node.Name);

            var calbackName = prefix is null
                    ? $"{typeName.Camelize()}Callback"
                    : $"{prefix}{typeName}Callback";

            arguments.Add(calbackName);
        }

        return arguments;
    }

}
