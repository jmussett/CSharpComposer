﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithInterpolatedStringText<TBuilder>
{
    TBuilder WithInterpolatedStringText();
    TBuilder WithInterpolatedStringText(InterpolatedStringTextSyntax interpolatedStringTextSyntax);
}

public interface IAddInterpolatedStringText<TBuilder>
{
    TBuilder AddInterpolatedStringText();
    TBuilder AddInterpolatedStringText(InterpolatedStringTextSyntax interpolatedStringTextSyntax);
}

public partial class InterpolatedStringTextBuilder
{
    public static InterpolatedStringTextSyntax CreateSyntax()
    {
        var textTokenToken = SyntaxFactory.Token(SyntaxKind.InterpolatedStringTextToken);
        return SyntaxFactory.InterpolatedStringText(textTokenToken);
    }
}