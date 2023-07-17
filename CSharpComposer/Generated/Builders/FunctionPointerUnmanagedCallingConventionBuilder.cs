using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class FunctionPointerUnmanagedCallingConventionBuilder
{
    public static FunctionPointerUnmanagedCallingConventionSyntax CreateSyntax(string name)
    {
        var nameToken = SyntaxFactory.Identifier(name);
        return SyntaxFactory.FunctionPointerUnmanagedCallingConvention(nameToken);
    }
}