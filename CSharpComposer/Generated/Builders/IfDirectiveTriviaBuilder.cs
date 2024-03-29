﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class IfDirectiveTriviaBuilder
{
    public static IfDirectiveTriviaSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var ifKeywordToken = SyntaxFactory.Token(SyntaxKind.IfKeyword);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.IfDirectiveTrivia(hashTokenToken, ifKeywordToken, conditionSyntax, endOfDirectiveTokenToken, isActive, branchTaken, conditionValue);
    }
}