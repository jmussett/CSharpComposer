using Humanizer;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator.Models;
using SyntaxBuilder.Types;

namespace CSharpComposer.Generator;

internal class ImplementationBuilder
{
    private readonly ParametersBuilder _parametersBuilder;
    private readonly ArgumentsBuilder _argumentsBuilder;
    private readonly MethodBuilder _methodBuilder;

    public ImplementationBuilder(ParametersBuilder parametersBuilder, ArgumentsBuilder argumentsBuilder, MethodBuilder methodBuilder)
    {
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

        builder.WithClass(builderName, builder =>
        {
            builder.WithAccessModifier(TypeAccessModifier.Public);
            builder.WithPartialModifier();
            if (type is AbstractNode || NodeValidator.IsTokenized(type))
            {
                // TODO: Do we exclude base types when abstract nodes have no derived types?
                // Abstract nodes with no derived types? unlikely
                builder.WithBaseType(x => x.AsType($"I{builderName}"));

                builder.WithProperty("Syntax", x => x.AsType($"{type.Name}?"), x => x
                    .WithAccessModifier(MemberAccessModifier.Public)
                    .WithGetter()
                    .WithSetter()
                );
            }

            if (type is Node)
            {
                // If we don't have optional children; interface, syntax and constructor are excluded.
                if (NodeValidator.HasOptionalChildren(type))
                {
                    builder.WithBaseType(x => x.AsType($"I{builderName}"));

                    builder.WithProperty("Syntax", x => x.AsType(type.Name), x => x
                        .WithAccessModifier(MemberAccessModifier.Public)
                        .WithGetter()
                        .WithSetter()
                    );

                    builder.WithConstructor(x => x
                        .WithAccessModifier(MemberAccessModifier.Public)
                        .WithParameter("syntax", x => x.AsType(type.Name))
                        .WithBody(x => x
                            .WithExpression(x => x
                                .AsAssignment(x => x.AsName("Syntax"), AssignmentType.Equal, x => x.AsName("syntax"))
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

            return builder;
        });

        return builder;
    }

    private void WithCreateSyntaxMethod(IClassDeclarationBuilder builder, TreeType type)
    {
        var builderName = NameFactory.CreateBuilderName(type.Name);

        if (type is AbstractNode || NodeValidator.IsTokenized(type))
        {
            builder.WithMethod(
                "CreateSyntax",
                x => x.AsType(type.Name),
                x => x
                    .WithAccessModifier(MemberAccessModifier.Public)
                    .WithStaticModifier()
                    .WithParameter("callback", $"Action<I{builderName}>")
                    .WithBody(x =>
                    {
                        x.WithStatement($"var builder = new {builderName}();")
                         .WithStatement($"callback(builder);")
                         .WithIfStatement(
                            x => x.ParseExpression("builder.Syntax is null"),
                            x => x.WithStatement($"throw new InvalidOperationException(\"{type.Name} has not been specified\");")
                         )
                         .WithStatement("return builder.Syntax;");
                        return x;
                    })
            );
        }

        if (type is Node node && !NodeValidator.IsTokenized(type))
        {
            builder.WithMethod(
                "CreateSyntax",
                x => x.AsType(type.Name),
                methodBuilder =>
                {
                    methodBuilder.WithAccessModifier(MemberAccessModifier.Public)
                        .WithStaticModifier();

                    _parametersBuilder.WithParameters(methodBuilder, node);

                    var typeName = NameFactory.CreateTypeName(node.Name);

                    methodBuilder = methodBuilder.WithBody(blockBuilder =>
                    {
                        var arguments = _argumentsBuilder.WithArguments(blockBuilder, node, true);

                        if (NodeValidator.HasOptionalChildren(type))
                        {
                            blockBuilder.WithStatement($"var syntax = SyntaxFactory.{typeName}({string.Join(", ", arguments)});");

                            var builderName = NameFactory.CreateBuilderName(type.Name);
                            blockBuilder.WithStatement($"var builder = new {builderName}(syntax);");
                            blockBuilder.WithStatement($"{typeName.Camelize()}Callback(builder);");
                            blockBuilder.WithStatement("return builder.Syntax;");
                        }
                        else
                        {
                            blockBuilder.WithStatement($"return SyntaxFactory.{typeName}({string.Join(", ", arguments)});");
                        }

                        return blockBuilder;
                    });

                    return methodBuilder;
                }
            );
        }
    }

    
    //private void AddReferencedListType(IClassDeclarationBuilder builder, string builderName, Field field, Field referencedNodeField)
    //{
    //    builder.WithMethod(
    //        NameFactory.CreateReferenceAddMethodName(field, referencedNodeField),
    //        x => x.AsType($"I{builderName}"),
    //        x => x.WithAccessModifier(MemberAccessModifier.Public)
    //            .WithBody(x =>
    //            {
    //                x = x.WithReturnStatement(x => x.ParseExpression("this"));
    //                return x;
    //            })
    //        );
    //}

    
}
