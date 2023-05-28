using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithCatchFilterClauseBuilder<TBuilder>
{
    TBuilder WithCatchFilterClause(Action<IExpressionBuilder> filterExpressionCallback);
    TBuilder WithCatchFilterClause(CatchFilterClauseSyntax catchFilterClauseSyntax);
}

public partial class CatchFilterClauseBuilder
{
    public static CatchFilterClauseSyntax CreateSyntax(Action<IExpressionBuilder> filterExpressionCallback)
    {
        var whenKeywordToken = SyntaxFactory.Token(SyntaxKind.WhenKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var filterExpressionSyntax = ExpressionBuilder.CreateSyntax(filterExpressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.CatchFilterClause(whenKeywordToken, openParenTokenToken, filterExpressionSyntax, closeParenTokenToken);
    }
}