using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAttributeBuilder
{
    IAttributeBuilder AddAttributeArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback);
    IAttributeBuilder AddAttributeArgument(AttributeArgumentSyntax argument);
}

public interface IAddAttribute<TBuilder>
{
    TBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback);
    TBuilder AddAttribute(AttributeSyntax attributeSyntax);
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

    public IAttributeBuilder AddAttributeArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback)
    {
        var argument = AttributeArgumentBuilder.CreateSyntax(expressionCallback, attributeArgumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IAttributeBuilder AddAttributeArgument(AttributeArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}