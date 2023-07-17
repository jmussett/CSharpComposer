using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class WithExpressionBuilder
{
    public static WithExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var withKeywordToken = SyntaxFactory.Token(SyntaxKind.WithKeyword);
        var initializerSyntax = InitializerExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback);
        return SyntaxFactory.WithExpression(expressionSyntax, withKeywordToken, initializerSyntax);
    }
}