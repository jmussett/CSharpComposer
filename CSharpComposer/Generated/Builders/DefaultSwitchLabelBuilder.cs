using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class DefaultSwitchLabelBuilder
{
    public static DefaultSwitchLabelSyntax CreateSyntax()
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.DefaultKeyword);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.DefaultSwitchLabel(keywordToken, colonTokenToken);
    }
}