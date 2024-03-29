﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInstanceExpressionBuilder
{
    void FromSyntax(InstanceExpressionSyntax syntax);
    void AsThisExpression();
    void AsBaseExpression();
}

internal partial class InstanceExpressionBuilder : IInstanceExpressionBuilder
{
    public InstanceExpressionSyntax? Syntax { get; set; }

    public static InstanceExpressionSyntax CreateSyntax(Action<IInstanceExpressionBuilder> callback)
    {
        var builder = new InstanceExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("InstanceExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(InstanceExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsThisExpression()
    {
        Syntax = ThisExpressionBuilder.CreateSyntax();
    }

    public void AsBaseExpression()
    {
        Syntax = BaseExpressionBuilder.CreateSyntax();
    }
}