using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class RegionDirectiveTriviaBuilder
{
    public static RegionDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var regionKeywordToken = SyntaxFactory.Token(SyntaxKind.RegionKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.RegionDirectiveTrivia(hashTokenToken, regionKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}