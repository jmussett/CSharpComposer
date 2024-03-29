﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class AwaitExpressionBuilder
{
    public static AwaitExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var awaitKeywordToken = SyntaxFactory.Token(SyntaxKind.AwaitKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.AwaitExpression(awaitKeywordToken, expressionSyntax);
    }
}