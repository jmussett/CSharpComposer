using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class MakeRefExpressionBuilder
{
    public static MakeRefExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.MakeRefKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.MakeRefExpression(keywordToken, openParenTokenToken, expressionSyntax, closeParenTokenToken);
    }
}