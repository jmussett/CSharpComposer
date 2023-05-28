using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithInterpolatedStringTextBuilder<TBuilder>
{
    TBuilder WithInterpolatedStringText();
    TBuilder WithInterpolatedStringText(InterpolatedStringTextSyntax interpolatedStringTextSyntax);
}

public partial class InterpolatedStringTextBuilder
{
    public static InterpolatedStringTextSyntax CreateSyntax()
    {
        var textTokenToken = SyntaxFactory.Token(SyntaxKind.InterpolatedStringTextToken);
        return SyntaxFactory.InterpolatedStringText(textTokenToken);
    }
}