using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class JoinIntoClauseBuilder
{
    public static JoinIntoClauseSyntax CreateSyntax(string identifier)
    {
        var intoKeywordToken = SyntaxFactory.Token(SyntaxKind.IntoKeyword);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        return SyntaxFactory.JoinIntoClause(intoKeywordToken, identifierToken);
    }
}