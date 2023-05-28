using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAttributeArgumentBuilder
{
    IAttributeArgumentBuilder WithNameEquals(string nameIdentifier);
    IAttributeArgumentBuilder WithNameEquals(NameEqualsSyntax nameEquals);
    IAttributeArgumentBuilder WithNameColon(string nameIdentifier);
    IAttributeArgumentBuilder WithNameColon(NameColonSyntax nameColon);
}

public interface IWithAttributeArgumentBuilder<TBuilder>
{
    TBuilder WithAttributeArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback);
    TBuilder WithAttributeArgument(AttributeArgumentSyntax attributeArgumentSyntax);
}

public partial class AttributeArgumentBuilder : IAttributeArgumentBuilder
{
    public AttributeArgumentSyntax Syntax { get; set; }

    public AttributeArgumentBuilder(AttributeArgumentSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AttributeArgumentSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder> attributeArgumentCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.AttributeArgument(null, null, expressionSyntax);
        var builder = new AttributeArgumentBuilder(syntax);
        attributeArgumentCallback(builder);
        return builder.Syntax;
    }

    public IAttributeArgumentBuilder WithNameEquals(string nameIdentifier)
    {
        var nameEqualsSyntax = NameEqualsBuilder.CreateSyntax(nameIdentifier);
        Syntax = Syntax.WithNameEquals(nameEqualsSyntax);
        return this;
    }

    public IAttributeArgumentBuilder WithNameEquals(NameEqualsSyntax nameEquals)
    {
        Syntax = Syntax.WithNameEquals(nameEquals);
        return this;
    }

    public IAttributeArgumentBuilder WithNameColon(string nameIdentifier)
    {
        var nameColonSyntax = NameColonBuilder.CreateSyntax(nameIdentifier);
        Syntax = Syntax.WithNameColon(nameColonSyntax);
        return this;
    }

    public IAttributeArgumentBuilder WithNameColon(NameColonSyntax nameColon)
    {
        Syntax = Syntax.WithNameColon(nameColon);
        return this;
    }
}