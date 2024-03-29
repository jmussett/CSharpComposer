﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFromClauseBuilder : IWithType<IFromClauseBuilder>
{
}

internal partial class FromClauseBuilder : IFromClauseBuilder
{
    public FromClauseSyntax Syntax { get; set; }

    public FromClauseBuilder(FromClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FromClauseSyntax CreateSyntax(string identifier, Action<IExpressionBuilder> expressionCallback, Action<IFromClauseBuilder>? fromClauseCallback = null)
    {
        var fromKeywordToken = SyntaxFactory.Token(SyntaxKind.FromKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var inKeywordToken = SyntaxFactory.Token(SyntaxKind.InKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.FromClause(fromKeywordToken, default(TypeSyntax), identifierToken, inKeywordToken, expressionSyntax);
        var builder = new FromClauseBuilder(syntax);
        fromClauseCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IFromClauseBuilder WithType(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.WithType(typeSyntax);
        return this;
    }

    public IFromClauseBuilder WithType(TypeSyntax type)
    {
        Syntax = Syntax.WithType(type);
        return this;
    }
}