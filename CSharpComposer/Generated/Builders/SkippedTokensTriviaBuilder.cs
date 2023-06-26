using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISkippedTokensTriviaBuilder
{
    ISkippedTokensTriviaBuilder AddToken(SyntaxToken token);
}

public partial class SkippedTokensTriviaBuilder : ISkippedTokensTriviaBuilder
{
    public SkippedTokensTriviaSyntax Syntax { get; set; }

    public SkippedTokensTriviaBuilder(SkippedTokensTriviaSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SkippedTokensTriviaSyntax CreateSyntax(Action<ISkippedTokensTriviaBuilder> skippedTokensTriviaCallback)
    {
        var syntax = SyntaxFactory.SkippedTokensTrivia(default(SyntaxTokenList));
        var builder = new SkippedTokensTriviaBuilder(syntax);
        skippedTokensTriviaCallback(builder);
        return builder.Syntax;
    }

    public ISkippedTokensTriviaBuilder AddToken(SyntaxToken token)
    {
        Syntax = Syntax.AddTokens(token);
        return this;
    }
}