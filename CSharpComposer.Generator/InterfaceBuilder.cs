using Humanizer;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator.Models;
using SyntaxBuilder.Types;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Net.Http.Headers;

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
            builder.WithInterface(interfaceName, builder =>
            {
                builder.WithAccessModifier(TypeAccessModifier.Public);
                builder.WithPartialModifier();

                // I don't think we need this as we don't want to polute base type casting methods with builder methods

                //x = x.WithBaseType(x =>
                //{
                //    x.AsGeneric($"I{builderName}", x => x.WithTypeArgument(x => x.AsType($"I{builderName}")));

                //    return x;
                //});

                _methodBuilder.WithCastMethods(builder, false, type);

                return builder;
            });
        }

        if (NodeValidator.HasOptionalChildren(type))
        {
            builder
                .WithInterface(interfaceName, x =>
                {
                    x.WithAccessModifier(TypeAccessModifier.Public);
                    x.WithPartialModifier();

                    var returnType = interfaceName;
                    
                    if (type is AbstractNode)
                    {
                        returnType = "TBuilder";

                        x.WithTypeParameter(returnType);
                    }

                    foreach (var optionalField in type.Children.OfType<Field>()
                        .Where(x => x.Name == NameFactory.CreateTypeName(x.Type) && x.IsOptional && !x.IsOverride && !NodeValidator.IsAnyList(x.Type)))
                    {
                        x.WithBaseType(x => x.AsGeneric($"IWith{optionalField.Name}Builder", x => x.WithTypeArgument(x => x.AsType(returnType))));
                    }


                    if (NodeValidator.IsValidNode(type.Base))
                    {
                        var baseType = _tree.Types.First(x => x.Name == type.Base);

                        if (NodeValidator.HasOptionalChildren(baseType))
                        {
                            var baseBuilderName = NameFactory.CreateBuilderName(type.Base);
                            x.WithBaseType(x => x.AsType($"I{baseBuilderName}<{returnType}>"));
                        }
                    }

                    _methodBuilder.WithChildMethods(x, false, _tree, returnType, type, type.Children);

                    return x;
                });
        }

        var typeName = NameFactory.CreateTypeName(type.Name!);

        // TODO: ITypeDEclarationInterface, remove with type? derived type children cointains choice? 
        if (_tree.Types.Any(t => t.Children.Any(f => _tree.AnyValidFieldMethod(t, f, false))))
        {
            builder
                .WithInterface($"IWith{typeName}Builder", x =>
                {
                    x.WithAccessModifier(TypeAccessModifier.Public);
                    x.WithTypeParameter("TBuilder");

                    if (type is AbstractNode)
                    {
                        x.WithMethod($"With{typeName}", x => x.AsType("TBuilder"), x => x.WithParameter($"{typeName.Camelize()}Callback", $"Action<{interfaceName}>"));
                    }
                    else if (type is Node node)
                    {
                        x.WithMethod($"With{typeName}", x => x.AsType("TBuilder"), x =>
                        {
                            _parameterBuilder.WithParameters(x, node);

                            return x;
                        });
                    }

                    x.WithMethod($"With{typeName}", x => x.AsType("TBuilder"), x => x.WithParameter(type.Name.Camelize(), type.Name));

                    return x;
                });
        }

        return builder;
    }

    
}
