﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILineOrSpanDirectiveTriviaBuilder
{
    void FromSyntax(LineOrSpanDirectiveTriviaSyntax syntax);
    void AsLineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder>? lineDirectiveTriviaCallback = null);
    void AsLineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback);
}

public partial interface ILineOrSpanDirectiveTriviaBuilder<TBuilder>
{
}

internal partial class LineOrSpanDirectiveTriviaBuilder : ILineOrSpanDirectiveTriviaBuilder
{
    public LineOrSpanDirectiveTriviaSyntax? Syntax { get; set; }

    public static LineOrSpanDirectiveTriviaSyntax CreateSyntax(Action<ILineOrSpanDirectiveTriviaBuilder> callback)
    {
        var builder = new LineOrSpanDirectiveTriviaBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("LineOrSpanDirectiveTriviaSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(LineOrSpanDirectiveTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsLineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder>? lineDirectiveTriviaCallback = null)
    {
        Syntax = LineDirectiveTriviaBuilder.CreateSyntax(lineDirectiveTriviaLine, isActive, lineDirectiveTriviaCallback);
    }

    public void AsLineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback)
    {
        Syntax = LineSpanDirectiveTriviaBuilder.CreateSyntax(startLine, startCharacter, endLine, endCharacter, file, isActive, lineSpanDirectiveTriviaCallback);
    }
}