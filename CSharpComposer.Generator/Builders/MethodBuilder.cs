using Humanizer;
using CSharpComposer.Generator.Models;
using Microsoft.CodeAnalysis.CSharp;
using CSharpComposer.Generator.Utility;
using CSharpComposer.Generator.Registries;

namespace CSharpComposer.Generator.Builders;

internal class MethodBuilder
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly EnumRegistry _enumRegistry;
    private readonly ParametersBuilder _parametersBuilder;
    private readonly ArgumentsBuilder _argumentsBuilder;

    public MethodBuilder(CSharpRegistry csharpRegistry, EnumRegistry enumRegistry, ParametersBuilder parametersBuilder, ArgumentsBuilder argumentsBuilder)
    {
        _csharpRegistry = csharpRegistry;
        _enumRegistry = enumRegistry;
        _parametersBuilder = parametersBuilder;
        _argumentsBuilder = argumentsBuilder;
    }

    public void WithMethods(IClassDeclarationBuilder builder, bool isImplementation, TreeType type)
    {
        var builderName = $"I{NameFactory.CreateBuilderName(type.Name)}";

        if (type is AbstractNode || NodeValidator.IsTokenized(type))
        {
            WithFromSyntaxMethod(builder, isImplementation, type);
            WithCastMethods(builder, isImplementation, type);
        }
        else
        {
            WithChildMethods(builder, isImplementation, builderName, type, type.Children);
        }
    }

    public void WithChildMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, TreeType type, IEnumerable<TreeTypeChild> children, bool isChoice = false, bool isSequence = false)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        foreach (var choice in children.OfType<Choice>())
        {
            WithChildMethods(builder, isImplementation, returnType, type, choice.Children, true);
        }

        foreach (var sequence in children.OfType<Sequence>())
        {
            WithChildMethods(builder, isImplementation, returnType, type, sequence.Children, isChoice, true);
        }

        WithFieldMethods(builder, isImplementation, returnType, type, children.OfType<Field>(), isChoice, isSequence);
    }

    public void WithCastMethods<TBuilder>(TBuilder builder, bool isImplementation, TreeType type)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        if (type is Node node && NodeValidator.IsTokenized(type))
        {
            foreach (var kind in node.Kinds)
            {
                WithTokenizedCastMethod(builder, isImplementation, node, kind);
            }

            return;
        }

        var derivedTypes = _csharpRegistry.Tree.Types.Where(x => x.Base == type.Name);

        foreach (var derivedType in derivedTypes)
        {
            if (derivedType is Node derivedNode)
            {
                WithCastMethod(builder, isImplementation, derivedNode);
            }
            else
            {
                // We do not want cast methods for abstract nodes,
                // so we recursively find all the derived types for each abstract node
                WithCastMethods(builder, isImplementation, derivedType);
            }
        }
    }

    public void WithFromSyntaxMethod<TBuilder>(TBuilder builder, bool isImplementation, TreeType type)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.AddMethodDeclaration(
            x => x.AsPredefinedType(PredefinedTypeKeyword.VoidKeyword),
            $"FromSyntax",
            x =>
            {
                x.AddParameter("syntax", x => x.WithType(type.Name));

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        x.AddStatement($"Syntax = syntax;");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );
    }

    private void WithTokenizedCastMethod<TBuilder>(TBuilder builder, bool isImplementation, Node node, Kind kind)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        var typeName = NameFactory.CreateTypeName(node.Name);

        // We need a shortened name for the method and the prefix to match the token kind.
        var shortenedName = NameFactory.CreateNameWithoutSuffix(kind.Name);
        var prefix = NameFactory.CreateNameWithoutSuffix(shortenedName);

        var tokenField = node.Children.OfType<Field>().First(x => x.Name == "Token");
        var tokenKind = tokenField.Kinds.First(x => x.Name.StartsWith(prefix));

        if (!NodeValidator.IsValidLiteral(tokenKind))
        {
            return;
        }

        builder.AddMethodDeclaration(
            x => x.AsPredefinedType(PredefinedTypeKeyword.VoidKeyword),
            $"As{shortenedName}",
            x =>
            {
                _parametersBuilder.WithLiteralParameter(x, shortenedName.Camelize(), tokenKind);

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        if (tokenKind.Name.EndsWith("Token"))
                        {
                            x.AddStatement($"var {tokenKind.Name.Camelize()} = SyntaxFactory.Literal({shortenedName.Camelize()});");
                            x.AddStatement($"Syntax = SyntaxFactory.{typeName}(SyntaxKind.{kind.Name}, {tokenKind.Name.Camelize()});");
                        }
                        else
                        {
                            x.AddStatement($"Syntax = SyntaxFactory.{typeName}(SyntaxKind.{kind.Name});");
                        }
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );
    }

    private void WithCastMethod<TBuilder>(TBuilder builder, bool isImplementation, Node derivedNode)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.AddMethodDeclaration(
            x => x.AsPredefinedType(PredefinedTypeKeyword.VoidKeyword),
            $"As{derivedNode.Name[..^"Syntax".Length]}",
            x =>
            {
                _parametersBuilder.WithParameters(x, derivedNode);

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        var builderName = NameFactory.CreateBuilderName(derivedNode.Name);
                        var arguments = _argumentsBuilder.WithArguments(x, derivedNode, false);

                        x.AddStatement($"Syntax = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );
    }

    private void WithFieldMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, TreeType type, IEnumerable<Field> fields, bool isChoice = false, bool isSequence = false)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        foreach (var field in fields)
        {
            if (!_csharpRegistry.Tree.IsValidFieldMethod(type, field, isImplementation, isChoice))
            {
                continue;
            }

            // Add method-specific interfaces instead of using regular methods if the field names match the types and if the child field is unique.
            if (!isImplementation
                && !NodeValidator.IsSyntaxToken(field.Type)
                // Only use method builders with unique type, otherwise we get conflicting methods.
                && type.Children.GetNestedChildren().Count(x => x.Field.Type == field.Type) == 1)
            {
                // Only use interfaces where the field names match the types
                if (!NodeValidator.IsAnyList(field.Type) && field.Name == NameFactory.CreateTypeName(field.Type))
                {
                    // Even if we are an override, base fields can be excluded if any derived types are non-optional.
                    // So include the method interface if we find any non optional derived base fields. 
                    if (field.IsOverride && _csharpRegistry.Tree.TryGetBaseField(type, field, out var baseType, out var baseField, out _, out _))
                    {
                        var derivedFields = _csharpRegistry.Tree.GetDerivedFields(baseType, baseField);

                        if (derivedFields.Any(x => !x.IsOptional))
                        {
                            builder.AddSimpleBaseType(x => x.AsGenericName($"IWith{NameFactory.CreateTypeName(field.Type)}", x => x.AddType(returnType)));
                        }
                    }
                    else if (!field.IsOverride)
                    {
                        builder.AddSimpleBaseType(x => x.AsGenericName($"IWith{NameFactory.CreateTypeName(field.Type)}", x => x.AddType(returnType)));
                    }

                    continue;
                }

                var listTypeName = NameFactory.ExtractReferenceTypeFromListType(_csharpRegistry.Tree, field.Type);

                // TODO: modifiers?
                if (listTypeName is not null && !NodeValidator.IsSyntaxToken(listTypeName) &&
                    (field.Name == NameFactory.CreateTypeName(field.Type) || NameFactory.CreateSingularName(field) == NameFactory.CreateTypeName(listTypeName)))
                {
                    if (!field.IsOverride)
                    {
                        builder.AddSimpleBaseType(x => x.AsGenericName($"IAdd{NameFactory.CreateTypeName(listTypeName)}", x => x.AddType(returnType)));
                    }


                    continue;
                }
            }

            // Remove overriden field methods from interfaces
            // TODO: don't remove if overriding field is optional?
            if (!isImplementation && !isChoice && field.IsOverride)
            {
                if (_csharpRegistry.Tree.TryGetBaseField(type, field, out var baseType, out var baseField, out _, out _))
                {
                    if (!baseField.IsOptional)
                    {
                        continue;
                    }
                    else if (baseField.IsOptional)
                    {
                        // Are any derived fields mandatory? If so, do not skip.

                        var derivedFields = _csharpRegistry.Tree.GetDerivedFields(baseType, field);

                        if (!derivedFields.Any() || !derivedFields.Any(x => !x.IsOptional))
                        {
                            continue;
                        }
                    }
                }

            }

            if (NodeValidator.IsAnyList(field.Type))
            {
                WithListTypeMethods(builder, isImplementation, returnType, field);
            }
            else if (NodeValidator.IsSyntaxToken(field.Type))
            {
                // Ignore syntax tokens, like semicolons, if we are in a sequence

                if (isSequence) return;

                WithTokenMethod(builder, isImplementation, returnType, field);
            }
            else
            {
                WithFieldMethods(builder, isImplementation, returnType, field, fields.Count(x => x.Type == field.Type) == 1);
            }
        }
    }


    private void WithListTypeMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        var listType = NameFactory.ExtractReferenceTypeFromListType(_csharpRegistry.Tree, field.Type);

        var listTypeName = listType is null ? null : NameFactory.CreateTypeName(listType);

        var singularName = NameFactory.CreateSingularName(field);
        var methodName = $"Add{singularName}";

        if (listTypeName is not null && (listTypeName.Contains(singularName) || singularName.Contains(listTypeName)))
        {
            methodName = $"Add{listTypeName}";
        }
        else if (listTypeName is not null)
        {
            methodName = $"Add{singularName}{listTypeName}";
        }

        // Exclude builder methods for syntaz tokens.
        if (!NodeValidator.IsSyntaxToken(listType))
        {
            builder.AddMethodDeclaration(
                returnType,
                methodName,
                x =>
                {
                    var listTypeField = field;

                    var listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);

                    var addListTypeMethodName = $"Add{listTypeField.Name}";

                    if (listTypeName is null)
                    {
                        listTypeField = _csharpRegistry.Tree.GetReferenceListType(field.Type);

                        if (listTypeField is null)
                        {
                            throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                        }

                        listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                        addListTypeMethodName = $"Add{field.Name}{listTypeField.Name}";
                    }

                    string? parentListSyntaxName = null;

                    if (NodeValidator.IsAnyList(listTypeName))
                    {
                        listTypeField = _csharpRegistry.Tree.GetReferenceListType(listTypeName);

                        if (listTypeField is not null)
                        {
                            parentListSyntaxName = listTypeName;
                            listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                        }
                    }

                    if (listTypeField is null || listTypeName is null)
                    {
                        throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                    }

                    var listType = _csharpRegistry.Tree.Types.FirstOrDefault(x => x.Name == listTypeName);

                    var singularName = NameFactory.CreateSingularName(listTypeField).Camelize();

                    var builderName = NameFactory.CreateBuilderName(listTypeName);

                    if (listType is Node listTypeNode)
                    {
                        _parametersBuilder.WithParameters(x, listTypeNode);
                    }
                    else
                    {
                        x.AddParameter($"{singularName}Callback", x => x.WithType($"Action<I{builderName}>"));
                    }

                    if (isImplementation)
                    {
                        x.AddModifierToken(SyntaxKind.PublicKeyword);

                        x.WithBody(x =>
                        {
                            var syntaxVariableName = $"{NameFactory.CreateSafeIdentifier(singularName)}";

                            if (listType is Node listTypeNode)
                            {
                                var arguments = _argumentsBuilder.WithArguments(x, listTypeNode, false);

                                x.AddStatement($"var {syntaxVariableName} = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                            }
                            else
                            {
                                x.AddStatement($"var {syntaxVariableName} = {builderName}.CreateSyntax({singularName}Callback);");
                            }

                            if (parentListSyntaxName is not null)
                            {
                                var parentListTypeName = NameFactory.CreateTypeName(parentListSyntaxName);
                                syntaxVariableName = parentListSyntaxName.Camelize();

                                var grandParentListType = NameFactory.ExtractParentTypeFromListType(listTypeField.Type)
                                    ?? throw new InvalidOperationException($"Unable to find parent type for list type '{listTypeField.Type}'");

                                var grandParentListTypeName = NameFactory.CreateTypeName(grandParentListType);

                                x.AddStatement($"var {grandParentListType.Camelize()} = SyntaxFactory.{grandParentListTypeName}(new [] {{{NameFactory.CreateSafeIdentifier(singularName.Camelize())}}});");
                                x.AddStatement($"var {syntaxVariableName} = SyntaxFactory.{parentListTypeName}({grandParentListType.Camelize()});");
                            }

                            x.AddStatement($"Syntax = Syntax.{addListTypeMethodName}({syntaxVariableName});");

                            x.AddStatement("return this;");
                        });
                    }
                    else
                    {
                        x.WithSemicolon();
                    }
                });
        }

        builder.AddMethodDeclaration(
            returnType,
            methodName,
            x =>
            {
                var listTypeField = field;

                var listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);

                var addListTypeMethodName = $"Add{listTypeField.Name}";

                if (listTypeName is null)
                {
                    listTypeField = _csharpRegistry.Tree.GetReferenceListType(field.Type);

                    if (listTypeField is null)
                    {
                        throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                    }

                    listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                    addListTypeMethodName = $"Add{field.Name}{listTypeField.Name}";
                }

                string? parentListSyntaxName = null;

                if (NodeValidator.IsAnyList(listTypeName))
                {
                    listTypeField = _csharpRegistry.Tree.GetReferenceListType(listTypeName);

                    if (listTypeField is not null)
                    {
                        parentListSyntaxName = listTypeName;
                        listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                    }
                }
                else if (NodeValidator.IsSyntaxToken(listTypeName))
                {
                    listTypeName = "SyntaxKind";
                }

                if (listTypeField is null || listTypeName is null)
                {
                    throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                }

                var singularName = NameFactory.CreateSingularName(listTypeField);

                x.AddParameter(NameFactory.CreateSafeIdentifier(singularName.Camelize()), x => x.WithType(listTypeName));

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        var syntaxVariableName = singularName.Camelize();

                        if (parentListSyntaxName is not null)
                        {
                            var parentListTypeName = NameFactory.CreateTypeName(parentListSyntaxName);
                            syntaxVariableName = parentListSyntaxName.Camelize();

                            var grandParentListType = NameFactory.ExtractParentTypeFromListType(listTypeField.Type)
                                ?? throw new InvalidOperationException($"Unable to find parent type for list type '{listTypeField.Type}'");

                            var grandParentListTypeName = NameFactory.CreateTypeName(grandParentListType);

                            x.AddStatement($"var {grandParentListType.Camelize()} = SyntaxFactory.{grandParentListTypeName}(new [] {{{NameFactory.CreateSafeIdentifier(singularName.Camelize())}}});");
                            x.AddStatement($"var {syntaxVariableName} = SyntaxFactory.{parentListTypeName}({grandParentListType.Camelize()});");
                        }

                        if (NodeValidator.IsSyntaxKind(listTypeName))
                        {
                            x.AddStatement($"Syntax = Syntax.{addListTypeMethodName}(SyntaxFactory.Token({NameFactory.CreateSafeIdentifier(syntaxVariableName)}));");
                        }
                        else
                        {
                            x.AddStatement($"Syntax = Syntax.{addListTypeMethodName}({NameFactory.CreateSafeIdentifier(syntaxVariableName)});");
                        }

                        x.AddStatement("return this;");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            });
    }



    private void WithFieldMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field, bool isUnique)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        if (field.Kinds.Count > 1)
        {
            foreach (var kind in field.Kinds)
            {
                builder.AddMethodDeclaration(
                    returnType,
                    $"With{kind.Name}",
                    x =>
                    {
                        x.AddParameter(NameFactory.CreateSafeIdentifier(kind.Name.Camelize()), x => x.WithType($"{kind.Name}Syntax"));

                        if (isImplementation)
                        {
                            x.AddModifierToken(SyntaxKind.PublicKeyword);

                            x.WithBody(x =>
                            {
                                x.AddStatement($"Syntax = Syntax.With{field.Name}({NameFactory.CreateSafeIdentifier(kind.Name.Camelize())});");
                                x.AddStatement("return this;");
                            });
                        }
                        else
                        {
                            x.WithSemicolon();
                        }
                    }
                );
            }
        }

        builder.AddMethodDeclaration(
            returnType,
            $"With{field.Name}",
            x =>
            {
                var fieldName = field.Name.Camelize();
                var builderName = NameFactory.CreateBuilderName(field.Type);

                var referenceType = _csharpRegistry.Tree.Types.FirstOrDefault(x => x.Name == field.Type);

                // If we are a regular node and do not have an interface, use parameters from the node's CreateSyntax method.
                if (referenceType is Node referenceTypeNode)
                {
                    _parametersBuilder.WithParameters(x, referenceTypeNode);
                }
                else
                {
                    var callbackType = $"Action<I{builderName}>";

                    x.AddParameter($"{fieldName}Callback", x => x.WithType(callbackType));
                }

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        if (referenceType is Node referenceTypeNode)
                        {
                            var arguments = _argumentsBuilder.WithArguments(x, referenceTypeNode, false);
                            x.AddStatement($"var {fieldName}Syntax = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                        }
                        else
                        {
                            var builderName = NameFactory.CreateBuilderName(field.Type);

                            x.AddStatement($"var {fieldName}Syntax = {builderName}.CreateSyntax({fieldName}Callback);");
                        }

                        x = x.AddStatement($"Syntax = Syntax.With{field.Name}({fieldName}Syntax);");
                        x = x.AddStatement("return this;");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );

        builder.AddMethodDeclaration(
            returnType,
            $"With{field.Name}",
            x =>
            {
                x.AddParameter(NameFactory.CreateSafeIdentifier(field.Name.Camelize()), x => x.WithType(field.Type));

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        x = x.AddStatement($"Syntax = Syntax.With{field.Name}({NameFactory.CreateSafeIdentifier(field.Name.Camelize())});");
                        x = x.AddStatement("return this;");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );
    }

    private void WithTokenMethod<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.AddMethodDeclaration(
            returnType,
            $"With{field.Name}",
            x =>
            {
                if (field.Kinds.Count == 1)
                {
                    var kind = field.Kinds[0];

                    if (kind.Name == "IdentifierToken")
                    {
                        x.AddParameter("identifier", x => x.WithType("string"));
                    }
                }
                else
                {
                    // TODO: muliple methods (when identifier)

                    _enumRegistry.TryAddEnum(field.Name, field);

                    x.AddParameter(field.Name.Camelize(), x => x.WithType(field.Name));
                }

                if (isImplementation)
                {
                    x.AddModifierToken(SyntaxKind.PublicKeyword);

                    x.WithBody(x =>
                    {
                        if (field.Kinds.Count == 1)
                        {
                            var kind = field.Kinds[0];

                            if (kind.Name == "IdentifierToken")
                            {
                                x.AddStatement("Syntax = Syntax.WithIdentifier(SyntaxFactory.Identifier(identifier));");
                            }
                            else
                            {
                                x.AddStatement($"Syntax = Syntax.With{field.Name}(SyntaxFactory.Token(SyntaxKind.{kind.Name}));");
                            }
                        }
                        else
                        {
                            // TODO: Build with builder
                            // TODO: Not supported message??
                            x.AddStatement($$"""
                                Syntax = Syntax.With{{field.Name}}(SyntaxFactory.Token(
                                    {{field.Name.Camelize()}} switch
                                    {
                                        {{string.Join("\r\n", field.Kinds.Select(x =>
                                                $"{field.Name}.{x.Name} => SyntaxKind.{x.Name},"))}}
                                        _ => throw new NotSupportedException()
                                    }
                                ));
                                """
                            );
                        }

                        x.AddStatement("return this;");
                    });
                }
                else
                {
                    x.WithSemicolon();
                }
            }
        );
    }
}
