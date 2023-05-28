using Humanizer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator.Models;
using SyntaxBuilder.Types;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;
using System.Xml.Linq;
using SyntaxBuilder;

namespace CSharpComposer.Generator;

internal class MethodBuilder
{
    private readonly Tree _tree;
    private readonly EnumStore _enumStore;
    private readonly ParametersBuilder _parametersBuilder;
    private readonly ArgumentsBuilder _argumentsBuilder;

    public MethodBuilder(Tree tree, EnumStore enumStore, ParametersBuilder parametersBuilder, ArgumentsBuilder argumentsBuilder)
    {
        _tree = tree;
        _enumStore = enumStore;
        _parametersBuilder = parametersBuilder;
        _argumentsBuilder = argumentsBuilder;
    }

    public void WithMethods(IClassDeclarationBuilder builder, bool isImplementation, TreeType type)
    {
        var builderName = $"I{NameFactory.CreateBuilderName(type.Name)}";

        if (type is AbstractNode || NodeValidator.IsTokenized(type))
        {
            WithCastMethods(builder, isImplementation, type);
        }
        else
        {
            WithChildMethods(builder, isImplementation, _tree, builderName, type, type.Children);
        }
    }

    public void WithChildMethods<TBuilder>(TBuilder builder, bool isImplementation, Tree tree, string returnType, TreeType type, IEnumerable<TreeTypeChild> children, bool isChoice = false, bool isSequence = false)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        foreach (var choice in children.OfType<Choice>())
        {
            WithChildMethods(builder, isImplementation, tree, returnType, type, choice.Children, true);
        }

        foreach (var sequence in children.OfType<Sequence>())
        {
            WithChildMethods(builder, isImplementation, tree, returnType, type, sequence.Children, isChoice, true);
        }

        WithFieldMethods(builder, isImplementation, returnType, type, children.OfType<Field>(), isChoice, isSequence);
    }

    public void WithCastMethods<TBuilder>(TBuilder builder, bool isImplementation, TreeType type)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        if (type is Node node && NodeValidator.IsTokenized(type))
        {
            foreach(var kind in node.Kinds)
            {
                WithTokenizedCastMethod(builder, isImplementation, node, kind);
            }

            return;
        }

        var derivedTypes = _tree.Types.Where(x => x.Base == type.Name);

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

        builder.WithMethod(
            $"As{shortenedName}",
            x => x.AsVoid(),
            x => {
                _parametersBuilder.WithLiteralParameter(x, shortenedName.Camelize(), tokenKind);

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        if (tokenKind.Name.EndsWith("Token"))
                        {
                            x.WithStatement($"var {tokenKind.Name.Camelize()} = SyntaxFactory.Literal({shortenedName.Camelize()});");
                            x.WithStatement($"Syntax = SyntaxFactory.{typeName}(SyntaxKind.{kind.Name}, {tokenKind.Name.Camelize()});");
                        }
                        else
                        {
                            x.WithStatement($"Syntax = SyntaxFactory.{typeName}(SyntaxKind.{kind.Name});");
                        }

                        return x;
                    });
                }

                return x;
            }
        );
    }

    private void WithCastMethod<TBuilder>(TBuilder builder, bool isImplementation, Node derivedNode)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.WithMethod(
            $"As{derivedNode.Name[..^"Syntax".Length]}",
            x => x.AsVoid(),
            x => {
                _parametersBuilder.WithParameters(x, derivedNode);

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        var builderName = NameFactory.CreateBuilderName(derivedNode.Name);
                        var arguments = _argumentsBuilder.WithArguments(x, derivedNode, false);

                        x.WithStatement($"Syntax = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                        return x;
                    });
                }
                
                return x;
            }
        );
    }

    private void WithFieldMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, TreeType type, IEnumerable<Field> fields, bool isChoice = false, bool isSequence = false)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        foreach (var field in fields)
        {
            if (!_tree.IsValidFieldMethod(type, field, isImplementation, isChoice))
            {
                continue;
            }

            // Remove overriden field methods from interfaces
            // TODO: don't remove if overriding field is optional?
            if (!isImplementation && field.IsOverride)
            {
                _tree.TryGetBaseField(type, field, out var baseType, out var baseField);

                if (baseField is not null && !baseField.IsOptional)
                {
                    continue;
                }
                else if (baseField is not null && baseField.IsOptional)
                {
                    // Are any derived fields mandatory? If so, do not skip.

                    var derivedFields = _tree.GetDerivedFields(baseType, field);

                    if (!derivedFields.Any() || !derivedFields.Any(x => !x.IsOptional))
                    {
                        continue;
                    }
                }
            }

            if (NodeValidator.IsAnyList(field.Type))
            {
                var listType = NameFactory.ExtractSyntaxTypeFromListType(field.Type);

                // Exlude syntax token list methods.
                // Fields like modifiers will require pre-defined extension methods.

                if (!NodeValidator.IsSyntaxToken(listType))
                {
                    WithListTypeMethods(builder, isImplementation, returnType, field);
                }
            }
            else if (NodeValidator.IsSyntaxToken(field.Type))
            {
                // Ignore syntax tokens, like semicolons, if we are in a sequence

                if (isSequence) return;

                WithTokenMethod(builder, isImplementation, returnType, field);
            }
            else
            {
                WithFieldMethods(builder, isImplementation, returnType, field);

                //var referenceTypeNode = tree.Types.FirstOrDefault(x => x.Name == field.Type);

                //if (referenceTypeNode is not null)
                //{
                //    foreach (var referencedNodeField in referenceTypeNode.Children.OfType<Field>())
                //    {
                //        if (NodeValidator.IsAnyNodeList(referencedNodeField.Type))
                //        {
                //            builder = AddReferencedListType(builder, builderName, field, referencedNodeField);
                //        }
                //    }
                //}
            }
        }
    }


    private void WithListTypeMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.WithMethod(
            $"Add{NameFactory.CreateSingularName(field)}",
            x => x.AsType(returnType),
            x => {
                var listTypeField = field;

                var listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);

                var addListTypeMethodName = $"Add{listTypeField.Name}";


                if (listTypeName is null)
                {
                    listTypeField = _tree.GetReferenceListType(field.Type);

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
                    listTypeField = _tree.GetReferenceListType(listTypeName);

                    if (listTypeField is null)
                    {
                        throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                    }

                    parentListSyntaxName = listTypeName;
                    listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                }

                var listType = _tree.Types.FirstOrDefault(x => x.Name == listTypeName);

                var singularName = NameFactory.CreateSingularName(listTypeField).Camelize();

                var builderName = NameFactory.CreateBuilderName(listTypeName);

                if (listType is Node listTypeNode)
                {
                    _parametersBuilder.WithParameters(x, listTypeNode);
                }
                else
                {
                    x.WithParameter($"{singularName}Callback", $"Action<I{builderName}>");
                }

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        var syntaxVariableName = $"{NameFactory.CreateSafeIdentifier(singularName)}";

                        if (listType is Node listTypeNode)
                        {
                            var arguments = _argumentsBuilder.WithArguments(x, listTypeNode, false);

                            x.WithStatement($"var {syntaxVariableName} = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                        }
                        else
                        {
                            x.WithStatement($"var {syntaxVariableName} = {builderName}.CreateSyntax({singularName}Callback);");
                        }

                        if (parentListSyntaxName is not null)
                        {
                            var parentListTypeName = NameFactory.CreateTypeName(parentListSyntaxName);
                            syntaxVariableName = parentListSyntaxName.Camelize();

                            var grandParentListType = NameFactory.ExtractParentTypeFromListType(listTypeField.Type);
                            var grandParentListTypeName = NameFactory.CreateTypeName(grandParentListType);

                            x.WithStatement($"var {grandParentListType.Camelize()} = SyntaxFactory.{grandParentListTypeName}(new [] {{{NameFactory.CreateSafeIdentifier(singularName.Camelize())}}});");
                            x.WithStatement($"var {syntaxVariableName} = SyntaxFactory.{parentListTypeName}({grandParentListType.Camelize()});");
                        }

                        x.WithStatement($"Syntax = Syntax.{addListTypeMethodName}({syntaxVariableName});");

                        x.WithReturnStatement(x => x.ParseExpression("this"));

                        return x;
                    });
                }

                return x;
            });

        builder.WithMethod(
            $"Add{NameFactory.CreateSingularName(field)}",
            x => x.AsType(returnType),
            x => {
                var listTypeField = field;

                var listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);

                var addListTypeMethodName = $"Add{listTypeField.Name}";

                if (listTypeName is null)
                {
                    listTypeField = _tree.GetReferenceListType(field.Type);

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
                    listTypeField = _tree.GetReferenceListType(listTypeName);

                    if (listTypeField is null)
                    {
                        throw new InvalidOperationException($"Unable to find list type for field '{field.Name}'");
                    }
                    parentListSyntaxName = listTypeName;
                    listTypeName = NameFactory.ExtractSyntaxTypeFromListType(listTypeField.Type);
                }

                var singularName = NameFactory.CreateSingularName(listTypeField);

                x.WithParameter(NameFactory.CreateSafeIdentifier(singularName.Camelize()), listTypeName);

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        var syntaxVariableName = singularName.Camelize();

                        if (parentListSyntaxName is not null)
                        {
                            var parentListTypeName = NameFactory.CreateTypeName(parentListSyntaxName);
                            syntaxVariableName = parentListSyntaxName.Camelize();

                            var grandParentListType = NameFactory.ExtractParentTypeFromListType(listTypeField.Type);
                            var grandParentListTypeName = NameFactory.CreateTypeName(grandParentListType);

                            x.WithStatement($"var {grandParentListType.Camelize()} = SyntaxFactory.{grandParentListTypeName}(new [] {{{NameFactory.CreateSafeIdentifier(singularName.Camelize())}}});");
                            x.WithStatement($"var {syntaxVariableName} = SyntaxFactory.{parentListTypeName}({grandParentListType.Camelize()});");
                        }

                        x.WithStatement($"Syntax = Syntax.{addListTypeMethodName}({NameFactory.CreateSafeIdentifier(syntaxVariableName)});");

                        x.WithReturnStatement(x => x.ParseExpression("this"));

                        return x;
                    });
                }

                return x;
            });

        
    }

    

    private void WithFieldMethods<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        if (field.Kinds.Count > 1)
        {
            foreach (var kind in field.Kinds)
            {
                builder.WithMethod(
                    $"With{kind.Name}",
                    x => x.AsType(returnType),
                    x =>
                    {
                        x.WithParameter(NameFactory.CreateSafeIdentifier(kind.Name.Camelize()), x => x.AsType($"{kind.Name}Syntax"));

                        if (isImplementation)
                        {
                            x.WithAccessModifier(MemberAccessModifier.Public);

                            x.WithBody(x =>
                            {
                                x.WithStatement($"Syntax = Syntax.With{field.Name}({NameFactory.CreateSafeIdentifier(kind.Name.Camelize())});");
                                x.WithReturnStatement(x => x.ParseExpression("this"));
                                return x;
                            });
                        }
                        

                        return x;
                    }
                );

                //builder.WithMethod(
                //    $"With{kind.Name}",
                //    x => x.AsType($"I{builderName}"),
                //    x =>
                //    {
                //        var kindArgument = kind.Name.Camelize();
                //        x.WithParameter($"{kindArgument}Callback", x => x.AsType($"Action<I{kind.Name}Builder>"));

                //        if (isImplementation)
                //        {
                //            x.WithAccessModifier(MemberAccessModifier.Public);

                //            x.WithBody(x =>
                //            {
                //                x.WithStatement($"var {kindArgument} = {kind.Name}Builder.CreateSyntax({kindArgument}Callback);");
                //                x.WithStatement($"Syntax = Syntax.With{field.Name}({kindArgument});");
                //                x.WithReturnStatement(x => x.ParseExpression("this"));
                //                return x;
                //            });
                //        }


                //        return x;
                //    }
                //);
            }
        }

        builder.WithMethod(
            $"With{field.Name}",
            x => x.AsType(returnType),
            x =>
            {
                var fieldName = field.Name.Camelize();
                var builderName = NameFactory.CreateBuilderName(field.Type);

                var referenceType = _tree.Types.FirstOrDefault(x => x.Name == field.Type);

                // If we are a regular node and do not have an interface, use parameters from the node's CreateSyntax method.
                if (referenceType is Node referenceTypeNode)
                {
                    _parametersBuilder.WithParameters(x, referenceTypeNode);
                }
                else
                {
                    
                    var callbackType = $"Action<I{builderName}>";

                    x.WithParameter($"{fieldName}Callback", callbackType);
                }

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        if (referenceType is Node referenceTypeNode)
                        {
                            var arguments = _argumentsBuilder.WithArguments(x, referenceTypeNode, false);
                            x.WithStatement($"var {fieldName}Syntax = {builderName}.CreateSyntax({string.Join(", ", arguments)});");
                        }
                        else
                        {
                            var builderName = NameFactory.CreateBuilderName(field.Type);
                            var callbackType = $"Action<I{builderName}>";

                            x.WithStatement($"var {fieldName}Syntax = {builderName}.CreateSyntax({fieldName}Callback);");
                        }

                        x = x.WithStatement($"Syntax = Syntax.With{field.Name}({fieldName}Syntax);");
                        x = x.WithReturnStatement(x => x.ParseExpression("this"));

                        return x;
                    });
                }

                return x;
            }
        );

        builder.WithMethod(
            $"With{field.Name}",
            x => x.AsType(returnType),
            x =>
            {
                x.WithParameter(NameFactory.CreateSafeIdentifier(field.Name.Camelize()), x => x.AsType(field.Type));

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        x = x.WithStatement($"Syntax = Syntax.With{field.Name}({NameFactory.CreateSafeIdentifier(field.Name.Camelize())});");
                        x = x.WithReturnStatement(x => x.ParseExpression("this"));
                        return x;
                    });
                }

                return x;
            }
        );
    }

    private void WithTokenMethod<TBuilder>(TBuilder builder, bool isImplementation, string returnType, Field field)
        where TBuilder : ITypeDeclarationBuilder<TBuilder>
    {
        builder.WithMethod(
            $"With{field.Name}",
            x => x.AsType(returnType),
            x =>
            {
                if (field.Kinds.Count == 1)
                {
                    var kind = field.Kinds[0];

                    if (kind.Name == "IdentifierToken")
                    {
                        x.WithParameter<string>("identifier");
                    }
                }
                else
                {
                    // TODO: muliple methods (when identifier)

                    _enumStore.TryAddEnum(field.Name, field);

                    x.WithParameter(field.Name.Camelize(), field.Name);
                }

                if (isImplementation)
                {
                    x.WithAccessModifier(MemberAccessModifier.Public);

                    x.WithBody(x =>
                    {
                        if (field.Kinds.Count == 1)
                        {
                            var kind = field.Kinds[0];

                            if (kind.Name == "IdentifierToken")
                            {
                                x.WithStatement("Syntax = Syntax.WithIdentifier(SyntaxFactory.Identifier(identifier));");
                            }
                            else
                            {
                                x.WithStatement($"Syntax = Syntax.With{field.Name}(SyntaxFactory.Token(SyntaxKind.{kind.Name}));");
                            }
                        }
                        else
                        {
                            // TODO: Build with builder
                            // TODO: Not supported message??
                            x.WithStatement($$"""
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

                        x.WithReturnStatement(x => x.ParseExpression("this"));
                        return x;
                    });
                }

                return x;
            }
        );
    }


}
