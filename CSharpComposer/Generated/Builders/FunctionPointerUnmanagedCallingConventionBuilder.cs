using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithFunctionPointerUnmanagedCallingConventionBuilder<TBuilder>
{
    TBuilder WithFunctionPointerUnmanagedCallingConvention(string name);
    TBuilder WithFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax functionPointerUnmanagedCallingConventionSyntax);
}

public partial class FunctionPointerUnmanagedCallingConventionBuilder
{
    public static FunctionPointerUnmanagedCallingConventionSyntax CreateSyntax(string name)
    {
        var nameToken = SyntaxFactory.Identifier(name);
        return SyntaxFactory.FunctionPointerUnmanagedCallingConvention(nameToken);
    }
}