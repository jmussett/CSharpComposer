using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithDiscardDesignationBuilder<TBuilder>
{
    TBuilder WithDiscardDesignation();
    TBuilder WithDiscardDesignation(DiscardDesignationSyntax discardDesignationSyntax);
}

public partial class DiscardDesignationBuilder
{
    public static DiscardDesignationSyntax CreateSyntax()
    {
        var underscoreTokenToken = SyntaxFactory.Token(SyntaxKind.UnderscoreToken);
        return SyntaxFactory.DiscardDesignation(underscoreTokenToken);
    }
}