using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class IdentifierNameBuilder
{
    public static IdentifierNameSyntax CreateSyntax(string identifier)
    {
        return SyntaxFactory.IdentifierName(identifier);
    }
}