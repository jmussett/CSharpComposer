using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithIdentifierNameBuilder<TBuilder>
{
    TBuilder WithIdentifierName(string identifier);
    TBuilder WithIdentifierName(IdentifierNameSyntax identifierNameSyntax);
}

public partial class IdentifierNameBuilder
{
    public static IdentifierNameSyntax CreateSyntax(string identifier)
    {
        return SyntaxFactory.IdentifierName(identifier);
    }
}