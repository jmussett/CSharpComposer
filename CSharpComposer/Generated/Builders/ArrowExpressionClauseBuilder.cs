using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithArrowExpressionClauseBuilder<TBuilder>
{
    TBuilder WithArrowExpressionClause(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithArrowExpressionClause(ArrowExpressionClauseSyntax arrowExpressionClauseSyntax);
}

public partial class ArrowExpressionClauseBuilder
{
    public static ArrowExpressionClauseSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var arrowTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.ArrowExpressionClause(arrowTokenToken, expressionSyntax);
    }
}