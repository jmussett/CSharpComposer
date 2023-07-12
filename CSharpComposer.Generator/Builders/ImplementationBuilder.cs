using Humanizer;
using CSharpComposer.Generator.Models;
using Microsoft.CodeAnalysis.CSharp;
using CSharpComposer.Generator.Utility;
using CSharpComposer.Generator.Registries;

namespace CSharpComposer.Generator.Builders;

internal class ImplementationBuilder
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly ParametersBuilder _parametersBuilder;
    private readonly ArgumentsBuilder _argumentsBuilder;
    private readonly MethodBuilder _methodBuilder;

    public ImplementationBuilder(CSharpRegistry csharpRegistry, ParametersBuilder parametersBuilder, ArgumentsBuilder argumentsBuilder, MethodBuilder methodBuilder)
    {
        _csharpRegistry = csharpRegistry;
        _parametersBuilder = parametersBuilder;
        _argumentsBuilder = argumentsBuilder;
        _methodBuilder = methodBuilder;
    }

    public IFileScopedNamespaceDeclarationBuilder WithImplementation(IFileScopedNamespaceDeclarationBuilder builder, TreeType type)
    {
        if (!NodeValidator.IsValidNode(type.Name))
        {
            return builder;
        }

        var builderName = NameFactory.CreateBuilderName(type.Name);

        builder.AddClassDeclaration(builderName, builder =>
        {
            builder.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            builder.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
            if (type is AbstractNode || NodeValidator.IsTokenized(type))
            {
                // TODO: Do we exclude base types when abstract nodes have no derived types?
                // Abstract nodes with no derived types? unlikely
                builder.AddSimpleBaseType($"I{builderName}");

                builder.AddPropertyDeclaration($"{type.Name}?", "Syntax", x => x
                    .AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorDeclaration(AccessorDeclarationKind.GetAccessorDeclaration, x => x.WithSemicolon())
                    .AddAccessorDeclaration(AccessorDeclarationKind.SetAccessorDeclaration, x => x.WithSemicolon())
                );
            }

            if (type is Node)
            {
                // If we don't have optional children; interface, syntax and constructor are excluded.
                if (_csharpRegistry.Tree.HasOptionalChildren(type.Name))
                {
                    builder.AddSimpleBaseType($"I{builderName}");

                    builder.AddPropertyDeclaration(type.Name, "Syntax",
                        x => x
                        .AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddAccessorDeclaration(AccessorDeclarationKind.GetAccessorDeclaration, x => x.WithSemicolon())
                        .AddAccessorDeclaration(AccessorDeclarationKind.SetAccessorDeclaration, x => x.WithSemicolon())
                    );

                    builder.AddConstructorDeclaration(builderName, x => x
                        .AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddParameter("syntax", x => x.WithType(type.Name))
                        .WithBody(x => x
                            .AddExpressionStatement(x =>
                                x.AsAssignmentExpression(
                                    AssignmentExpressionKind.SimpleAssignmentExpression,
                                    x => x.ParseExpression("Syntax"),
                                    x => x.ParseExpression("syntax")
                                )
                            )
                        )
                    );
                }
                else
                {
                    // TODO: Make static if no optional children?
                }
            }

            WithCreateSyntaxMethod(builder, type);

            _methodBuilder.WithMethods(builder, true, type);
        });

        return builder;
    }

    private void WithCreateSyntaxMethod(IClassDeclarationBuilder builder, TreeType type)
    {
        var builderName = NameFactory.CreateBuilderName(type.Name);

        if (type is AbstractNode || NodeValidator.IsTokenized(type))
        {
            builder.AddMethodDeclaration(
                type.Name,
                "CreateSyntax",
                x => x
                    .AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .AddParameter("callback", x => x.WithType($"Action<I{builderName}>"))
                    .WithBody(x =>
                    {
                        x.AddStatement($"var builder = new {builderName}();")
                         .AddStatement($"callback(builder);")
                         .AddIfStatement(
                            x => x.ParseExpression("builder.Syntax is null"),
                            x => x.AsBlock(x =>
                                x.AddStatement($"throw new InvalidOperationException(\"{type.Name} has not been specified\");")
                            )
                         )
                         .AddStatement("return builder.Syntax;");
                    })
            ); ;
        }

        if (type is Node node && !NodeValidator.IsTokenized(type))
        {
            builder.AddMethodDeclaration(
                type.Name,
                "CreateSyntax",
                methodBuilder =>
                {
                    methodBuilder.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddModifierToken(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

                    _parametersBuilder.WithParameters(methodBuilder, node);

                    var typeName = NameFactory.CreateTypeName(node.Name);

                    methodBuilder = methodBuilder.WithBody(blockBuilder =>
                    {
                        var arguments = _argumentsBuilder.WithArguments(blockBuilder, node, true);

                        if (_csharpRegistry.Tree.HasOptionalChildren(type.Name))
                        {
                            blockBuilder.AddStatement($"var syntax = SyntaxFactory.{typeName}({string.Join(", ", arguments)});");

                            var builderName = NameFactory.CreateBuilderName(type.Name);
                            blockBuilder.AddStatement($"var builder = new {builderName}(syntax);");
                            blockBuilder.AddStatement($"{typeName.Camelize()}Callback?.Invoke(builder);");
                            blockBuilder.AddStatement("return builder.Syntax;");
                        }
                        else
                        {
                            blockBuilder.AddStatement($"return SyntaxFactory.{typeName}({string.Join(", ", arguments)});");
                        }
                    });
                }
            );
        }
    }


}
