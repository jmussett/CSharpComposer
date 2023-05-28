using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IElementBindingExpressionBuilder
{
    IElementBindingExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    IElementBindingExpressionBuilder AddArgument(ArgumentSyntax argument);
}

public interface IWithElementBindingExpressionBuilder<TBuilder>
{
    TBuilder WithElementBindingExpression(Action<IElementBindingExpressionBuilder> elementBindingExpressionCallback);
    TBuilder WithElementBindingExpression(ElementBindingExpressionSyntax elementBindingExpressionSyntax);
}

public partial class ElementBindingExpressionBuilder : IElementBindingExpressionBuilder
{
    public ElementBindingExpressionSyntax Syntax { get; set; }

    public ElementBindingExpressionBuilder(ElementBindingExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ElementBindingExpressionSyntax CreateSyntax(Action<IElementBindingExpressionBuilder> elementBindingExpressionCallback)
    {
        var argumentListSyntax = SyntaxFactory.BracketedArgumentList();
        var syntax = SyntaxFactory.ElementBindingExpression(argumentListSyntax);
        var builder = new ElementBindingExpressionBuilder(syntax);
        elementBindingExpressionCallback(builder);
        return builder.Syntax;
    }

    public IElementBindingExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IElementBindingExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}