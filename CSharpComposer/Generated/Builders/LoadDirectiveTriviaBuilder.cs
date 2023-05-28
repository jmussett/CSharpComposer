using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithLoadDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithLoadDirectiveTrivia(string file, bool isActive);
    TBuilder WithLoadDirectiveTrivia(LoadDirectiveTriviaSyntax loadDirectiveTriviaSyntax);
}

public partial class LoadDirectiveTriviaBuilder
{
    public static LoadDirectiveTriviaSyntax CreateSyntax(string file, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var loadKeywordToken = SyntaxFactory.Token(SyntaxKind.LoadKeyword);
        var fileToken = SyntaxFactory.Literal(file);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.LoadDirectiveTrivia(hashTokenToken, loadKeywordToken, fileToken, endOfDirectiveTokenToken, isActive);
    }
}