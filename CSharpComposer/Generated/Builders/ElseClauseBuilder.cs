﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithElseClause<TBuilder>
{
    TBuilder WithElseClause(Action<IStatementBuilder> statementCallback);
    TBuilder WithElseClause(ElseClauseSyntax elseClauseSyntax);
}

public interface IAddElseClause<TBuilder>
{
    TBuilder AddElseClause(Action<IStatementBuilder> statementCallback);
    TBuilder AddElseClause(ElseClauseSyntax elseClauseSyntax);
}

public partial class ElseClauseBuilder
{
    public static ElseClauseSyntax CreateSyntax(Action<IStatementBuilder> statementCallback)
    {
        var elseKeywordToken = SyntaxFactory.Token(SyntaxKind.ElseKeyword);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        return SyntaxFactory.ElseClause(elseKeywordToken, statementSyntax);
    }
}