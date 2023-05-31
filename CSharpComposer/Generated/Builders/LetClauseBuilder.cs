﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithLetClause<TBuilder>
{
    TBuilder WithLetClause(string identifier, Action<IExpressionBuilder> expressionCallback);
    TBuilder WithLetClause(LetClauseSyntax letClauseSyntax);
}

public interface IAddLetClause<TBuilder>
{
    TBuilder AddLetClause(string identifier, Action<IExpressionBuilder> expressionCallback);
    TBuilder AddLetClause(LetClauseSyntax letClauseSyntax);
}

public partial class LetClauseBuilder
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