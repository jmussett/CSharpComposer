using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithPragmaChecksumDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithPragmaChecksumDirectiveTrivia(string file, string guid, string bytes, bool isActive);
    TBuilder WithPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax pragmaChecksumDirectiveTriviaSyntax);
}

public partial class PragmaChecksumDirectiveTriviaBuilder
{
    public static PragmaChecksumDirectiveTriviaSyntax CreateSyntax(string file, string guid, string bytes, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var pragmaKeywordToken = SyntaxFactory.Token(SyntaxKind.PragmaKeyword);
        var checksumKeywordToken = SyntaxFactory.Token(SyntaxKind.ChecksumKeyword);
        var fileToken = SyntaxFactory.Literal(file);
        var guidToken = SyntaxFactory.Literal(guid);
        var bytesToken = SyntaxFactory.Literal(bytes);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.PragmaChecksumDirectiveTrivia(hashTokenToken, pragmaKeywordToken, checksumKeywordToken, fileToken, guidToken, bytesToken, endOfDirectiveTokenToken, isActive);
    }
}