using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithWhenClauseBuilder<TBuilder>
{
    TBuilder WithWhenClause(Action<IExpressionBuilder> conditionCallback);
    TBuilder WithWhenClause(WhenClauseSyntax whenClauseSyntax);
}

public partial class WhenClauseBuilder
{
    public static WhenClauseSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback)
    {
        var whenKeywordToken = SyntaxFactory.Token(SyntaxKind.WhenKeyword);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        return SyntaxFactory.WhenClause(whenKeywordToken, conditionSyntax);
    }
}