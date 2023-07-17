using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithWhenClause<TBuilder>
{
    TBuilder WithWhenClause(WhenClauseSyntax whenClauseSyntax);
    TBuilder WithWhenClause(Action<IExpressionBuilder> conditionCallback);
}

internal partial class WhenClauseBuilder
{
    public static WhenClauseSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback)
    {
        var whenKeywordToken = SyntaxFactory.Token(SyntaxKind.WhenKeyword);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        return SyntaxFactory.WhenClause(whenKeywordToken, conditionSyntax);
    }
}