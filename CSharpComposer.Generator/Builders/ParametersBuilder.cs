using Humanizer;
using CSharpComposer.Generator.Models;
using CSharpComposer.Generator.Utility;
using CSharpComposer.Generator.Registries;

namespace CSharpComposer.Generator.Builders;

internal class ParametersBuilder
{
    private readonly CSharpRegistry _csharpRegistry;
    private readonly EnumRegistry _enumRegistry;

    public ParametersBuilder(CSharpRegistry csharpRegistry, EnumRegistry enumRegistry)
    {
        _csharpRegistry = csharpRegistry;
        _enumRegistry = enumRegistry;
    }

    public void WithParameters(IMethodDeclarationBuilder builder, Node node, bool optionalTypeBuilder = true, string? prefix = null)
    {
        var typeName = NameFactory.CreateTypeName(node.Name);

        if (node.Kinds.Count > 1 && !NodeValidator.IsTokenized(node))
        {
            var enumName = $"{typeName}Kind";
            _enumRegistry.TryAddEnum(enumName, node);

            var kindParameterName = prefix is null ? "kind" : $"{prefix}Kind";
            builder.AddParameter(kindParameterName, x => x.WithType(enumName));
        }

        foreach (var field in node.Children.OfType<Field>())
        {
            if (field.IsOptional)
            {
                continue;
            }

            if (field.Name == "Identifier" && field.IsToken ||
               field.Kinds.Count == 1 && field.Kinds.FirstOrDefault()?.Name == "IdentifierToken")
            {
                var identifierParameterName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                builder.AddParameter(NameFactory.CreateSafeIdentifier(identifierParameterName), x => x.WithType("string"));
            }
            else if (field.Kinds.Count > 1 && field.IsToken &&
                // Exclude keywords if node has multiple kinds
                (node.Kinds.Count <= 1 || !(field.Name?.EndsWith("Keyword") ?? false) && field.Name != "OperatorToken" && field.Name != "Token")
            )
            {
                var enumName = $"{typeName}{field.Name}";
                _enumRegistry.TryAddEnum(enumName, field);

                var enumParameterName = prefix is null
                    ? enumName.Camelize()
                    : $"{prefix}{enumName}";

                builder.AddParameter(NameFactory.CreateSafeIdentifier(enumParameterName), x => x.WithType(enumName));
            }
            else if (field.Kinds.Count == 1)
            {
                var kind = field.Kinds.First();

                var literalParameterName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                WithLiteralParameter(builder, literalParameterName, kind);
            }

            if (NodeValidator.IsSyntaxNode(field.Type) &&
                !NodeValidator.IsAnyList(field.Type))
            {
                var referenceType = _csharpRegistry.Tree.Types.FirstOrDefault(x => x.Name == field.Type);

                // If we are a regular node and do not have an interface, use parameters from the node's CreateSyntax method.
                if (referenceType is Node referenceTypeNode && NodeValidator.HasMandatoryChildren(referenceTypeNode))
                {
                    var newPrefix = prefix is null 
                        ? field.Name.Camelize() 
                        : $"{field.Name.Camelize()}{prefix.Pascalize()}";

                    optionalTypeBuilder = false;

                    WithParameters(builder, referenceTypeNode, optionalTypeBuilder, newPrefix);
                }
                else
                {
                    var builderName = NameFactory.CreateBuilderName(field.Type);
                    var callbackType = $"Action<I{builderName}>";

                    var callbackName = prefix is null
                        ? $"{field.Name.Camelize()}Callback"
                        : $"{prefix}{field.Name}Callback";

                    builder.AddParameter(callbackName, x => x.WithType(callbackType));
                }
            }

            if (field.Type == "bool")
            {
                var parameterName = prefix is null
                     ? $"{field.Name.Camelize()}"
                     : $"{prefix}{field.Name}";

                builder.AddParameter(parameterName, x => x.WithType("bool"));
            }
        }

        if (_csharpRegistry.Tree.HasOptionalChildren(node.Name) || NodeValidator.IsTokenized(node))
        {
            var builderName = NameFactory.CreateBuilderName(node.Name);
            var callbackType = $"Action<I{builderName}>" + (optionalTypeBuilder ? "?" : "");

            var callbackName = prefix is null 
                ? $"{typeName.Camelize()}Callback"
                : $"{prefix}{typeName}Callback";

            builder.AddParameter(callbackName, x =>
            {
                x.WithType(callbackType);

                if (optionalTypeBuilder)
                {
                    x.WithDefault("null");
                }
            });
        }
    }

    public void WithLiteralParameter(IMethodDeclarationBuilder builder, string name, KindBase kind)
    {
        var parameterName = NameFactory.CreateSafeIdentifier(name);

        if (kind?.Name == "StringLiteralToken")
        {
            builder.AddParameter(parameterName, x => x.WithType("string"));
        }
        else if (kind?.Name == "NumericLiteralToken")
        {
            builder.AddParameter(parameterName, x => x.WithType("int"));
        }
        else if (kind?.Name == "CharacterLiteralToken")
        {
            builder.AddParameter(parameterName, x => x.WithType("char"));
        }
    }
}
