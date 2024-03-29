﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInterpolatedStringContentBuilder
{
    void FromSyntax(InterpolatedStringContentSyntax syntax);
    void AsInterpolatedStringText();
    void AsInterpolation(Action<IExpressionBuilder> expressionCallback, Action<IInterpolationBuilder>? interpolationCallback = null);
}

internal partial class InterpolatedStringContentBuilder : IInterpolatedStringContentBuilder
{
    public InterpolatedStringContentSyntax? Syntax { get; set; }

    public static InterpolatedStringContentSyntax CreateSyntax(Action<IInterpolatedStringContentBuilder> callback)
    {
        var builder = new InterpolatedStringContentBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("InterpolatedStringContentSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(InterpolatedStringContentSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsInterpolatedStringText()
    {
        Syntax = InterpolatedStringTextBuilder.CreateSyntax();
    }

    public void AsInterpolation(Action<IExpressionBuilder> expressionCallback, Action<IInterpolationBuilder>? interpolationCallback = null)
    {
        Syntax = InterpolationBuilder.CreateSyntax(expressionCallback, interpolationCallback);
    }
}