using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class UndefDirectiveTriviaBuilder
{
    public static UndefDirectiveTriviaSyntax CreateSyntax(string name, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var undefKeywordToken = SyntaxFactory.Token(SyntaxKind.UndefKeyword);
        var nameToken = SyntaxFactory.Identifier(name);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.UndefDirectiveTrivia(hashTokenToken, undefKeywordToken, nameToken, endOfDirectiveTokenToken, isActive);
    }
}