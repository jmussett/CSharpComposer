﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISlicePatternBuilder : IWithPattern<ISlicePatternBuilder>
{
}

internal partial class SlicePatternBuilder : ISlicePatternBuilder
{
    public SlicePatternSyntax Syntax { get; set; }

    public SlicePatternBuilder(SlicePatternSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SlicePatternSyntax CreateSyntax(Action<ISlicePatternBuilder>? slicePatternCallback = null)
    {
        var dotDotTokenToken = SyntaxFactory.Token(SyntaxKind.DotDotToken);
        var syntax = SyntaxFactory.SlicePattern(dotDotTokenToken, default(PatternSyntax));
        var builder = new SlicePatternBuilder(syntax);
        slicePatternCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ISlicePatternBuilder WithPattern(Action<IPatternBuilder> patternCallback)
    {
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        Syntax = Syntax.WithPattern(patternSyntax);
        return this;
    }

    public ISlicePatternBuilder WithPattern(PatternSyntax pattern)
    {
        Syntax = Syntax.WithPattern(pattern);
        return this;
    }
}