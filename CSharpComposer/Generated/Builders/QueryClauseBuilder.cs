﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IQueryClauseBuilder
{
    void FromSyntax(QueryClauseSyntax syntax);
    void AsFromClause(string identifier, Action<IExpressionBuilder> expressionCallback, Action<IFromClauseBuilder>? fromClauseCallback = null);
    void AsLetClause(string identifier, Action<IExpressionBuilder> expressionCallback);
    void AsJoinClause(string identifier, Action<IExpressionBuilder> inExpressionCallback, Action<IExpressionBuilder> leftExpressionCallback, Action<IExpressionBuilder> rightExpressionCallback, Action<IJoinClauseBuilder>? joinClauseCallback = null);
    void AsWhereClause(Action<IExpressionBuilder> conditionCallback);
    void AsOrderByClause(Action<IOrderByClauseBuilder>? orderByClauseCallback = null);
}

internal partial class QueryClauseBuilder : IQueryClauseBuilder
{
    public QueryClauseSyntax? Syntax { get; set; }

    public static QueryClauseSyntax CreateSyntax(Action<IQueryClauseBuilder> callback)
    {
        var builder = new QueryClauseBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("QueryClauseSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(QueryClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsFromClause(string identifier, Action<IExpressionBuilder> expressionCallback, Action<IFromClauseBuilder>? fromClauseCallback = null)
    {
        Syntax = FromClauseBuilder.CreateSyntax(identifier, expressionCallback, fromClauseCallback);
    }

    public void AsLetClause(string identifier, Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = LetClauseBuilder.CreateSyntax(identifier, expressionCallback);
    }

    public void AsJoinClause(string identifier, Action<IExpressionBuilder> inExpressionCallback, Action<IExpressionBuilder> leftExpressionCallback, Action<IExpressionBuilder> rightExpressionCallback, Action<IJoinClauseBuilder>? joinClauseCallback = null)
    {
        Syntax = JoinClauseBuilder.CreateSyntax(identifier, inExpressionCallback, leftExpressionCallback, rightExpressionCallback, joinClauseCallback);
    }

    public void AsWhereClause(Action<IExpressionBuilder> conditionCallback)
    {
        Syntax = WhereClauseBuilder.CreateSyntax(conditionCallback);
    }

    public void AsOrderByClause(Action<IOrderByClauseBuilder>? orderByClauseCallback = null)
    {
        Syntax = OrderByClauseBuilder.CreateSyntax(orderByClauseCallback);
    }
}