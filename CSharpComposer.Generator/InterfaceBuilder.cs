using Humanizer;
using CSharpComposer.Generator.Models;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Net.Http.Headers;
using CSharpComposer.Extensions;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpComposer.Generator;

internal class InterfaceBuilder
{
    private readonly Tree _tree;
    private readonly MethodBuilder _methodBuilder;
    private readonly ParametersBuilder _parameterBuilder;

    public InterfaceBuilder(Tree tree, MethodBuilder methodBuilder, ParametersBuilder parameterBuilder)
    {
        _tree = tree;
        _methodBuilder = methodBuilder;
        _parameterBuilder = parameterBuilder;
    }

    public IFileScopedNamespaceDeclarationBuilder WithInterfaces(IFileScopedNamespaceDeclarationBuilder builder, TreeType type)
    {
        if (!NodeValidator.IsValidNode(type.Name))
        {
            return builder;
        }

        var builderName = NameFactory.CreateBuilderName(type.Name!);

        var interfaceName = $"I{builderName}";

        if (type is AbstractNode || NodeValidator.IsTokenized(type))
        {
            builder.AddInterfaceDeclaration(interfaceName, builder =>
            {
                builder.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                builder.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

                // I don't think we need this as we don't want to polute base type casting methods with builder methods

                //x = x.WithBaseType(x =>
                //{
                //    x.AsGeneric($"I{builderName}", x => x.WithTypeArgument(x => x.AsType($"I{builderName}")));

                //    return x;
                //});

                _methodBuilder.WithCastMethods(builder, false, type);
            });
        }

        if (_tree.HasOptionalChildren(type.Name))
        {
            builder
                .AddInterfaceDeclaration(interfaceName, x =>
                {
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

                    var returnType = interfaceName;

                    if (type is AbstractNode)
                    {
                        returnType = "TBuilder";

                        x.AddTypeParameter(returnType);
                    }

                    if (NodeValidator.IsValidNode(type.Base))
                    {
                        var baseType = _tree.Types.First(x => x.Name == type.Base);

                        if (_tree.HasOptionalChildren(baseType.Name))
                        {
                            var baseBuilderName = NameFactory.CreateBuilderName(type.Base);
                            x.AddBaseType(x => x.AsSimpleBaseType(x => x.ParseTypeName($"I{baseBuilderName}<{returnType}>")));
                        }
                    }

                    _methodBuilder.WithChildMethods(x, false, _tree, returnType, type, type.Children);
                });
        }

        var typeName = NameFactory.CreateTypeName(type.Name!);

        // TODO: ITypeDeclarationInterface, remove with type? derived type children cointains choice? 
        if (_tree.Types.Any(t => t.Children.Any(f => _tree.AnyValidFieldMethod(t, f, false))))
        {
            // TODO: Remove when unused
            builder
                .AddInterfaceDeclaration($"IWith{typeName}", x =>
                {
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    x.AddTypeParameter("TBuilder");

                    x.AddMethodDeclaration(x => x.ParseTypeName("TBuilder"), $"With{typeName}", x => x
                        .AddParameter(type.Name.Camelize(), x => x
                            .WithType(x => x.ParseTypeName(type.Name))
                        )
                        .WithSemicolon()
                    );

                    if (type is AbstractNode)
                    {
                        x.AddMethodDeclaration(x => x.ParseTypeName("TBuilder"), $"With{typeName}", x => x
                            .AddParameter($"{typeName.Camelize()}Callback", x => x
                                .WithType( x=> x.ParseTypeName($"Action<{interfaceName}>")))
                            .WithSemicolon()
                        );
                    }
                    else if (type is Node node)
                    {
                        x.AddMethodDeclaration( x => x.ParseTypeName("TBuilder"), $"With{typeName}", x =>
                        {
                            _parameterBuilder.WithParameters(x, node);

                            x.WithSemicolon();
                        });
                    }
                });

            // TODO: Remove when unused
            builder
               .AddInterfaceDeclaration($"IAdd{typeName}", x =>
               {
                   x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                   x.AddTypeParameter("TBuilder");

                   x.AddMethodDeclaration(x => x.ParseTypeName("TBuilder"), $"Add{typeName}", x => x
                       .AddParameter(type.Name.Camelize(), x => x
                           .WithType(x => x.ParseTypeName(type.Name))
                       )
                       .WithSemicolon()
                   );

                   if (type is AbstractNode)
                   {
                       x.AddMethodDeclaration(x => x.ParseTypeName("TBuilder"), $"Add{typeName}", x => x
                           .AddParameter($"{typeName.Camelize()}Callback", x => x
                               .WithType(x => x.ParseTypeName($"Action<{interfaceName}>")))
                           .WithSemicolon()
                       );
                   }
                   else if (type is Node node)
                   {
                       x.AddMethodDeclaration(x => x.ParseTypeName("TBuilder"), $"Add{typeName}", x =>
                       {
                           _parameterBuilder.WithParameters(x, node);

                           x.WithSemicolon();
                       });
                   }
               });
        }

        return builder;
    }
}
