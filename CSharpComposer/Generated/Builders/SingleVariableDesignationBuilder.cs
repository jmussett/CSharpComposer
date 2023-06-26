using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class SingleVariableDesignationBuilder
{
    public static SingleVariableDesignationSyntax CreateSyntax(string identifier)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        return SyntaxFactory.SingleVariableDesignation(identifierToken);
    }
}