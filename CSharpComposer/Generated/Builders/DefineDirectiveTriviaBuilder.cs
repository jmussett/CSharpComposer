using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithDefineDirectiveTriviaBuilder<TBuilder>
{
    TBuilder WithDefineDirectiveTrivia(string name, bool isActive);
    TBuilder WithDefineDirectiveTrivia(DefineDirectiveTriviaSyntax defineDirectiveTriviaSyntax);
}

public partial class DefineDirectiveTriviaBuilder
{
    public static DefineDirectiveTriviaSyntax CreateSyntax(string name, bool isActive)
    {
        var hashTokenToken = SyntaxFactory.Token(SyntaxKind.HashToken);
        var defineKeywordToken = SyntaxFactory.Token(SyntaxKind.DefineKeyword);
        var nameToken = SyntaxFactory.Identifier(name);
        var endOfDirectiveTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfDirectiveToken);
        return SyntaxFactory.DefineDirectiveTrivia(hashTokenToken, defineKeywordToken, nameToken, endOfDirectiveTokenToken, isActive);
    }
}