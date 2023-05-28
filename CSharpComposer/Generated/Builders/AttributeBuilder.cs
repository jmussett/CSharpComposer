using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAttributeBuilder
{
    IAttributeBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback);
    IAttributeBuilder AddArgument(AttributeArgumentSyntax argument);
}

public interface IWithAttributeBuilder<TBuilder>
{
    TBuilder WithAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback);
    TBuilder WithAttribute(AttributeSyntax attributeSyntax);
}

public partial class AttributeBuilder : IAttributeBuilder
{
    public AttributeSyntax Syntax { get; set; }

    public AttributeBuilder(AttributeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AttributeSyntax CreateSyntax(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var nameSyntax = NameBuilder.CreateSyntax(nameCallback);
        var syntax = SyntaxFactory.Attribute(nameSyntax, default(AttributeArgumentListSyntax));
        var builder = new AttributeBuilder(syntax);
        attributeCallback(builder);
        return builder.Syntax;
    }

    public IAttributeBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback)
    {
        var argument = AttributeArgumentBuilder.CreateSyntax(expressionCallback, attributeArgumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IAttributeBuilder AddArgument(AttributeArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}