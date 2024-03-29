﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInterpolatedStringExpressionBuilder
{
    IInterpolatedStringExpressionBuilder AddInterpolatedStringContent(Action<IInterpolatedStringContentBuilder> contentCallback);
    IInterpolatedStringExpressionBuilder AddInterpolatedStringContent(InterpolatedStringContentSyntax content);
}

internal partial class InterpolatedStringExpressionBuilder : IInterpolatedStringExpressionBuilder
{
    public InterpolatedStringExpressionSyntax Syntax { get; set; }

    public InterpolatedStringExpressionBuilder(InterpolatedStringExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static InterpolatedStringExpressionSyntax CreateSyntax(InterpolatedStringExpressionStringStartToken interpolatedStringExpressionStringStartToken, InterpolatedStringExpressionStringEndToken interpolatedStringExpressionStringEndToken, Action<IInterpolatedStringExpressionBuilder>? interpolatedStringExpressionCallback = null)
    {
        var stringStartTokenToken = interpolatedStringExpressionStringStartToken switch
        {
            InterpolatedStringExpressionStringStartToken.InterpolatedStringStartToken => SyntaxFactory.Token(SyntaxKind.InterpolatedStringStartToken),
            InterpolatedStringExpressionStringStartToken.InterpolatedVerbatimStringStartToken => SyntaxFactory.Token(SyntaxKind.InterpolatedVerbatimStringStartToken),
            InterpolatedStringExpressionStringStartToken.InterpolatedSingleLineRawStringStartToken => SyntaxFactory.Token(SyntaxKind.InterpolatedSingleLineRawStringStartToken),
            InterpolatedStringExpressionStringStartToken.InterpolatedMultiLineRawStringStartToken => SyntaxFactory.Token(SyntaxKind.InterpolatedMultiLineRawStringStartToken),
            _ => throw new NotSupportedException()
        };
        var stringEndTokenToken = interpolatedStringExpressionStringEndToken switch
        {
            InterpolatedStringExpressionStringEndToken.InterpolatedStringEndToken => SyntaxFactory.Token(SyntaxKind.InterpolatedStringEndToken),
            InterpolatedStringExpressionStringEndToken.InterpolatedRawStringEndToken => SyntaxFactory.Token(SyntaxKind.InterpolatedRawStringEndToken),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.InterpolatedStringExpression(stringStartTokenToken, default(SyntaxList<InterpolatedStringContentSyntax>), stringEndTokenToken);
        var builder = new InterpolatedStringExpressionBuilder(syntax);
        interpolatedStringExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IInterpolatedStringExpressionBuilder AddInterpolatedStringContent(Action<IInterpolatedStringContentBuilder> contentCallback)
    {
        var content = InterpolatedStringContentBuilder.CreateSyntax(contentCallback);
        Syntax = Syntax.AddContents(content);
        return this;
    }

    public IInterpolatedStringExpressionBuilder AddInterpolatedStringContent(InterpolatedStringContentSyntax content)
    {
        Syntax = Syntax.AddContents(content);
        return this;
    }
}