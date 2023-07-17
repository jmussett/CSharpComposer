using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class LetClauseBuilder
{
    public static LetClauseSyntax CreateSyntax(string identifier, Action<IExpressionBuilder> expressionCallback)
    {
        var letKeywordToken = SyntaxFactory.Token(SyntaxKind.LetKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.LetClause(letKeywordToken, identifierToken, equalsTokenToken, expressionSyntax);
    }
}