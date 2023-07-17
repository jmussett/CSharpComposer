using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class EndRegionDirectiveTriviaBuilder
{
    public static EndRegionDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var endRegionKeywordToken = SyntaxFactory.Token(SyntaxKind.EndRegionKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.EndRegionDirectiveTrivia(hashTokenToken, endRegionKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}