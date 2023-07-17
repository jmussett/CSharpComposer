using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ParenthesizedExpressionBuilder
{
    public static ParenthesizedExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        return SyntaxFactory.ParenthesizedExpression(openParenTokenToken, expressionSyntax, closeParenTokenToken);
    }
}