using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISkippedTokensTriviaBuilder
{
}

public interface IWithSkippedTokensTriviaBuilder<TBuilder>
{
    TBuilder WithSkippedTokensTrivia(Action<ISkippedTokensTriviaBuilder> skippedTokensTriviaCallback);
    TBuilder WithSkippedTokensTrivia(SkippedTokensTriviaSyntax skippedTokensTriviaSyntax);
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
}