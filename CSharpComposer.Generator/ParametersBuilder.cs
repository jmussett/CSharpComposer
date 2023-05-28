using Humanizer;
using SyntaxBuilder.Builders;
using CSharpComposer.Generator.Models;
using SyntaxBuilder;

namespace CSharpComposer.Generator;

internal class ParametersBuilder
{
    private readonly Tree _tree;
    private readonly EnumStore _enumStore;

    public ParametersBuilder(Tree tree, EnumStore enumStore)
    {
        _tree = tree;
        _enumStore = enumStore;
    }

    public void WithParameters(IMethodDeclarationBuilder builder, Node node, string? prefix = null)
    {
        var typeName = NameFactory.CreateTypeName(node.Name);

        if (node.Kinds.Count > 1 && !NodeValidator.IsTokenized(node))
        {
            var enumName = $"{typeName}Kind";
            _enumStore.TryAddEnum(enumName, node);

            var kindParameterName = prefix is null ? "kind" : $"{prefix}Kind";
            builder.WithParameter(kindParameterName, enumName);
        }

        foreach (var field in node.Children.OfType<Field>())
        {
            if (field.IsOptional)
            {
                continue;
            }

            if (field.Name == "Identifier" && field.IsToken || 
               (field.Kinds.Count == 1 && field.Kinds.FirstOrDefault()?.Name == "IdentifierToken"))
            {
                var identifierParameterName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                builder.WithParameter<string>(NameFactory.CreateSafeIdentifier(identifierParameterName));
            }
            else if (field.Kinds.Count > 1 && field.IsToken &&
                // Exclude keywords if node has multiple kinds
                (node.Kinds.Count <= 1 || (!(field.Name?.EndsWith("Keyword") ?? false) && field.Name != "OperatorToken" && field.Name != "Token"))
            )
            {
                var enumName = $"{typeName}{field.Name}";
                _enumStore.TryAddEnum(enumName, field);

                var enumParameterName = prefix is null
                    ? enumName.Camelize()
                    : $"{prefix}{enumName}";

                builder.WithParameter(NameFactory.CreateSafeIdentifier(enumParameterName), enumName);
            }
            else if (field.Kinds.Count == 1)
            {
                var kind = field.Kinds.FirstOrDefault();

                var literalParameterName = prefix is null
                    ? field.Name.Camelize()
                    : $"{prefix}{field.Name}";

                WithLiteralParameter(builder, literalParameterName, kind);
            }

            if (NodeValidator.IsSyntaxNode(field.Type) &&
                !NodeValidator.IsAnyList(field.Type))
            {
                var referenceType = _tree.Types.FirstOrDefault(x => x.Name == field.Type);

                // If we are a regular node and do not have an interface, use parameters from the node's CreateSyntax method.
                if (referenceType is Node referenceTypeNode && NodeValidator.HasMandatoryChildren(referenceTypeNode))
                {
                    var newPrefix = prefix is null ? field.Name.Camelize() : $"{field.Name.Camelize()}{prefix.Pascalize()}";
                    WithParameters(builder, referenceTypeNode, newPrefix);
                }
                else
                {
                    var builderName = NameFactory.CreateBuilderName(field.Type);
                    var callbackType = $"Action<I{builderName}>";

                    var callbackName = prefix is null
                        ? $"{field.Name.Camelize()}Callback"
                        : $"{prefix}{field.Name}Callback";

                    builder.WithParameter(callbackName, callbackType);
                }
            }

            if (field.Type == "bool")
            {
               var parameterName = prefix is null
                    ? $"{field.Name.Camelize()}"
                    : $"{prefix}{field.Name}";

                builder.WithParameter(parameterName, "bool");
            }
        }

        if (NodeValidator.HasOptionalChildren(node) || NodeValidator.IsTokenized(node))
        {
            var builderName = NameFactory.CreateBuilderName(node.Name);
            var callbackType = $"Action<I{builderName}>";

            var callbackName = prefix is null ?
                $"{typeName.Camelize()}Callback" 
                : $"{prefix}{typeName}Callback";

            builder.WithParameter(callbackName, callbackType);
        }
    }

    public void WithLiteralParameter(IMethodDeclarationBuilder builder, string name, KindBase kind)
    {
        var parameterName = NameFactory.CreateSafeIdentifier(name);

        if (kind?.Name == "StringLiteralToken")
        {
            builder.WithParameter<string>(parameterName);
        }
        else if (kind?.Name == "NumericLiteralToken")
        {
            builder.WithParameter<int>(parameterName);
        }
        else if (kind?.Name == "CharacterLiteralToken")
        {
            builder.WithParameter<char>(parameterName);
        }
    }
}
