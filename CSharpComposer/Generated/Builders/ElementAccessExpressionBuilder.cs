using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IElementAccessExpressionBuilder
{
    IElementAccessExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    IElementAccessExpressionBuilder AddArgument(ArgumentSyntax argument);
}

public interface IWithElementAccessExpressionBuilder<TBuilder>
{
    TBuilder WithElementAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IElementAccessExpressionBuilder> elementAccessExpressionCallback);
    TBuilder WithElementAccessExpression(ElementAccessExpressionSyntax elementAccessExpressionSyntax);
}

public partial class ElementAccessExpressionBuilder : IElementAccessExpressionBuilder
{
    public ElementAccessExpressionSyntax Syntax { get; set; }

    public ElementAccessExpressionBuilder(ElementAccessExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ElementAccessExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IElementAccessExpressionBuilder> elementAccessExpressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var argumentListSyntax = SyntaxFactory.BracketedArgumentList();
        var syntax = SyntaxFactory.ElementAccessExpression(expressionSyntax, argumentListSyntax);
        var builder = new ElementAccessExpressionBuilder(syntax);
        elementAccessExpressionCallback(builder);
        return builder.Syntax;
    }

    public IElementAccessExpressionBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IElementAccessExpressionBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}