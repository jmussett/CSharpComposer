﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ElseClauseBuilder
{
    public static ElseClauseSyntax CreateSyntax(Action<IStatementBuilder> statementCallback)
    {
        var elseKeywordToken = SyntaxFactory.Token(SyntaxKind.ElseKeyword);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        return SyntaxFactory.ElseClause(elseKeywordToken, statementSyntax);
    }
}