using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class ErrorDirectiveTriviaBuilder
{
    public static ErrorDirectiveTriviaSyntax CreateSyntax(bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var errorKeywordToken = SyntaxFactory.Token(SyntaxKind.ErrorKeyword);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.ErrorDirectiveTrivia(hashTokenToken, errorKeywordToken, endOfDirectiveTokenToken, isActive);
    }
}