﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class GroupClauseBuilder
{
    public static GroupClauseSyntax CreateSyntax(Action<IExpressionBuilder> groupExpressionCallback, Action<IExpressionBuilder> byExpressionCallback)
    {
        var groupKeywordToken = SyntaxFactory.Token(SyntaxKind.GroupKeyword);
        var groupExpressionSyntax = ExpressionBuilder.CreateSyntax(groupExpressionCallback);
        var byKeywordToken = SyntaxFactory.Token(SyntaxKind.ByKeyword);
        var byExpressionSyntax = ExpressionBuilder.CreateSyntax(byExpressionCallback);
        return SyntaxFactory.GroupClause(groupKeywordToken, groupExpressionSyntax, byKeywordToken, byExpressionSyntax);
    }
}