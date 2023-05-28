using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithElseDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithElseDirectiveTrivia(bool isActive, bool branchTaken);
    TBuilder WithElseDirectiveTrivia(ElseDirectiveTriviaSyntax elseDirectiveTriviaSyntax);
}

public partial class ElseDirectiveTriviaBuilder
{
    public static ElseDirectiveTriviaSyntax CreateSyntax(bool isActive, bool branchTaken)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var elseKeywordToken = SyntaxFactory.Token(SyntaxKind.ElseKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.ElseDirectiveTrivia(hashTokenToken, elseKeywordToken, endOfDirectiveTokenToken, isActive, branchTaken);
    }
}