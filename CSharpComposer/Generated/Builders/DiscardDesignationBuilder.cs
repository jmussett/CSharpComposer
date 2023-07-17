using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class DiscardDesignationBuilder
{
    public static DiscardDesignationSyntax CreateSyntax()
    {
        var underscoreTokenToken = SyntaxFactory.Token(SyntaxKind.UnderscoreToken);
        return SyntaxFactory.DiscardDesignation(underscoreTokenToken);
    }
}