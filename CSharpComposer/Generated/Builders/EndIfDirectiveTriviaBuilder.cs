using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithEndIfDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithEndIfDirectiveTrivia(bool isActive);
    TBuilder WithEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax endIfDirectiveTriviaSyntax);
}

public partial class EndIfDirectiveTriviaBuilder
{
    public static EndIfDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var endIfKeywordToken = SyntaxFactory.Token(SyntaxKind.EndIfKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.EndIfDirectiveTrivia(hashTokenToken, endIfKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}