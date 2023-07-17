using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class InterpolatedStringTextBuilder
{
    public static InterpolatedStringTextSyntax CreateSyntax()
    {
        var textTokenToken = SyntaxFactory.Token(SyntaxKind.InterpolatedStringTextToken);
        return SyntaxFactory.InterpolatedStringText(textTokenToken);
    }
}