using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IAttributeBuilder
{
    IAttributeBuilder WithArgument(Action<IExpressionBuilder> expressionCallback);
    IAttributeBuilder WithArgument(string name, Action<IExpressionBuilder> expressionCallback);
}

public sealed class AttributeBuilder : IAttributeBuilder
{
    private AttributeSyntax _syntax;

    private AttributeBuilder(AttributeSyntax syntax)
    {
        _syntax = syntax;
    }

    // TODO: have NameBuilder alternative?

    //TODO: Create extension for names
    public static AttributeSyntax CreateSyntax(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);

        var attributeSyntax = SyntaxFactory.Attribute(nameSyntax);

        if (attributeCallback is not null)
        {
            var builder = new AttributeBuilder(attributeSyntax).AssertInvoke(attributeCallback);

            attributeSyntax = builder._syntax;
        }

        return attributeSyntax;
    }

    public IAttributeBuilder WithArgument(string name, Action<IExpressionBuilder> expressionCallback)
    {
        // TODO: validate name

        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        _syntax = _syntax.AddArgumentListArguments(
            SyntaxFactory.AttributeArgument(
                SyntaxFactory.NameEquals(
                    SyntaxFactory.IdentifierName(name)
                ),
                null,
                expressionSyntax
            )
        );

        return this;
    }

    public IAttributeBuilder WithArgument(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        _syntax = _syntax.AddArgumentListArguments(
            SyntaxFactory.AttributeArgument(expressionSyntax)
        );

        return this;
    }
}

public static class AttributeBuilderExtensions
{
    public static IAttributeBuilder WithArgument<T>(this IAttributeBuilder builder, T value)
    {
        return builder.WithArgument(x => x.AsLiteral(value));
    }
}