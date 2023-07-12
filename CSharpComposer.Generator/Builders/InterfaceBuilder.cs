using Humanizer;
using CSharpComposer.Generator.Models;
using Microsoft.CodeAnalysis.CSharp;
using CSharpComposer.Generator.Utility;
using CSharpComposer.Generator.Registries;
using CSharpComposer;

namespace CSharpComposer.Generator.Builders;

internal class InterfaceBuilder
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly MethodBuilder _methodBuilder;
    private readonly ParametersBuilder _parameterBuilder;

    public InterfaceBuilder(CSharpRegistry csharpRegistry, MethodBuilder methodBuilder, ParametersBuilder parameterBuilder)
    {
        _csharpRegistry = csharpRegistry;
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

                _methodBuilder.WithCastMethods(builder, false, type);
            });
        }

        if (_csharpRegistry.Tree.HasOptionalChildren(type.Name))
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
                        var baseType = _csharpRegistry.Tree.Types.First(x => x.Name == type.Base);

                        if (_csharpRegistry.Tree.HasOptionalChildren(baseType.Name))
                        {
                            var baseBuilderName = NameFactory.CreateBuilderName(type.Base);
                            x.AddSimpleBaseType($"I{baseBuilderName}<{returnType}>");
                        }
                    }

                    _methodBuilder.WithChildMethods(x, false, returnType, type, type.Children);
                });
        }

        var typeName = NameFactory.CreateTypeName(type.Name!);

        // TODO: ITypeDeclarationInterface, remove with type? derived type children cointains choice? 
        if (_csharpRegistry.Tree.Types.Any(t => t.Children.Any(f => _csharpRegistry.Tree.AnyValidFieldMethod(t, f, false))))
        {
            // TODO: Remove when unused
            builder
                .AddInterfaceDeclaration($"IWith{typeName}", x =>
                {
                    x.AddModifierToken(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    x.AddTypeParameter("TBuilder");

                    x.AddMethodDeclaration("TBuilder", $"With{typeName}", x => x
                        .AddParameter(type.Name.Camelize(), x => x
                            .WithType(type.Name)
                        )
                        .WithSemicolon()
                    );

                    if (type is AbstractNode)
                    {
                        x.AddMethodDeclaration("TBuilder", $"With{typeName}", x => x
                            .AddParameter($"{typeName.Camelize()}Callback", x => x
                                .WithType($"Action<{interfaceName}>"))
                            .WithSemicolon()
                        );
                    }
                    else if (type is Node node)
                    {
                        x.AddMethodDeclaration("TBuilder", $"With{typeName}", x =>
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

                   x.AddMethodDeclaration("TBuilder", $"Add{typeName}", x => x
                       .AddParameter(type.Name.Camelize(), x => x
                           .WithType(type.Name)
                       )
                       .WithSemicolon()
                   );

                   if (type is AbstractNode)
                   {
                       x.AddMethodDeclaration("TBuilder", $"Add{typeName}", x => x
                           .AddParameter($"{typeName.Camelize()}Callback", x => x
                               .WithType($"Action<{interfaceName}>"))
                           .WithSemicolon()
                       );
                   }
                   else if (type is Node node)
                   {
                       x.AddMethodDeclaration("TBuilder", $"Add{typeName}", x =>
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
