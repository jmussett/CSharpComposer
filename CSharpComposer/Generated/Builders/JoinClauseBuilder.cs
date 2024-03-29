﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IJoinClauseBuilder : IWithType<IJoinClauseBuilder>
{
    IJoinClauseBuilder WithInto(string identifier);
    IJoinClauseBuilder WithInto(JoinIntoClauseSyntax into);
}

internal partial class JoinClauseBuilder : IJoinClauseBuilder
{
    public JoinClauseSyntax Syntax { get; set; }

    public JoinClauseBuilder(JoinClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static JoinClauseSyntax CreateSyntax(string identifier, Action<IExpressionBuilder> inExpressionCallback, Action<IExpressionBuilder> leftExpressionCallback, Action<IExpressionBuilder> rightExpressionCallback, Action<IJoinClauseBuilder>? joinClauseCallback = null)
    {
        var joinKeywordToken = SyntaxFactory.Token(SyntaxKind.JoinKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var inKeywordToken = SyntaxFactory.Token(SyntaxKind.InKeyword);
        var inExpressionSyntax = ExpressionBuilder.CreateSyntax(inExpressionCallback);
        var onKeywordToken = SyntaxFactory.Token(SyntaxKind.OnKeyword);
        var leftExpressionSyntax = ExpressionBuilder.CreateSyntax(leftExpressionCallback);
        var equalsKeywordToken = SyntaxFactory.Token(SyntaxKind.EqualsKeyword);
        var rightExpressionSyntax = ExpressionBuilder.CreateSyntax(rightExpressionCallback);
        var syntax = SyntaxFactory.JoinClause(joinKeywordToken, default(TypeSyntax), identifierToken, inKeywordToken, inExpressionSyntax, onKeywordToken, leftExpressionSyntax, equalsKeywordToken, rightExpressionSyntax, default(JoinIntoClauseSyntax));
        var builder = new JoinClauseBuilder(syntax);
        joinClauseCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IJoinClauseBuilder WithType(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.WithType(typeSyntax);
        return this;
    }

    public IJoinClauseBuilder WithType(TypeSyntax type)
    {
        Syntax = Syntax.WithType(type);
        return this;
    }

    public IJoinClauseBuilder WithInto(string identifier)
    {
        var intoSyntax = JoinIntoClauseBuilder.CreateSyntax(identifier);
        Syntax = Syntax.WithInto(intoSyntax);
        return this;
    }

    public IJoinClauseBuilder WithInto(JoinIntoClauseSyntax into)
    {
        Syntax = Syntax.WithInto(into);
        return this;
    }
}