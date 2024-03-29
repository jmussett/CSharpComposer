﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISelectOrGroupClauseBuilder
{
    void FromSyntax(SelectOrGroupClauseSyntax syntax);
    void AsSelectClause(Action<IExpressionBuilder> expressionCallback);
    void AsGroupClause(Action<IExpressionBuilder> groupExpressionCallback, Action<IExpressionBuilder> byExpressionCallback);
}

internal partial class SelectOrGroupClauseBuilder : ISelectOrGroupClauseBuilder
{
    public SelectOrGroupClauseSyntax? Syntax { get; set; }

    public static SelectOrGroupClauseSyntax CreateSyntax(Action<ISelectOrGroupClauseBuilder> callback)
    {
        var builder = new SelectOrGroupClauseBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("SelectOrGroupClauseSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(SelectOrGroupClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsSelectClause(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = SelectClauseBuilder.CreateSyntax(expressionCallback);
    }

    public void AsGroupClause(Action<IExpressionBuilder> groupExpressionCallback, Action<IExpressionBuilder> byExpressionCallback)
    {
        Syntax = GroupClauseBuilder.CreateSyntax(groupExpressionCallback, byExpressionCallback);
    }
}