using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISkippedTokensTriviaBuilder
{
    ISkippedTokensTriviaBuilder AddToken(SyntaxKind token);
}

public partial class SkippedTokensTriviaBuilder : ISkippedTokensTriviaBuilder
{
    public SkippedTokensTriviaSyntax Syntax { get; set; }

    public SkippedTokensTriviaBuilder(SkippedTokensTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SkippedTokensTriviaSyntax CreateSyntax(Action<ISkippedTokensTriviaBuilder>? skippedTokensTriviaCallback = null)
    {
        var syntax = SyntaxFactory.SkippedTokensTrivia(default(SyntaxTokenList));
        var builder = new SkippedTokensTriviaBuilder(syntax);
        skippedTokensTriviaCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ISkippedTokensTriviaBuilder AddToken(SyntaxKind token)
    {
        Syntax = Syntax.AddTokens(SyntaxFactory.Token(token));
        return this;
    }
}