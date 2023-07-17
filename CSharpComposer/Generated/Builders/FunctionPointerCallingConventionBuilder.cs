using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerCallingConventionBuilder
{
    IFunctionPointerCallingConventionBuilder AddFunctionPointerUnmanagedCallingConvention(string name);
    IFunctionPointerCallingConventionBuilder AddFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax callingConvention);
}

internal partial class FunctionPointerCallingConventionBuilder : IFunctionPointerCallingConventionBuilder
{
    public FunctionPointerCallingConventionSyntax Syntax { get; set; }

    public FunctionPointerCallingConventionBuilder(FunctionPointerCallingConventionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerCallingConventionSyntax CreateSyntax(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder>? functionPointerCallingConventionCallback = null)
    {
        var managedOrUnmanagedKeywordToken = functionPointerCallingConventionManagedOrUnmanagedKeyword switch
        {
            FunctionPointerCallingConventionManagedOrUnmanagedKeyword.ManagedKeyword => SyntaxFactory.Token(SyntaxKind.ManagedKeyword),
            FunctionPointerCallingConventionManagedOrUnmanagedKeyword.UnmanagedKeyword => SyntaxFactory.Token(SyntaxKind.UnmanagedKeyword),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.FunctionPointerCallingConvention(managedOrUnmanagedKeywordToken, default(FunctionPointerUnmanagedCallingConventionListSyntax));
        var builder = new FunctionPointerCallingConventionBuilder(syntax);
        functionPointerCallingConventionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IFunctionPointerCallingConventionBuilder AddFunctionPointerUnmanagedCallingConvention(string name)
    {
        var callingConvention = FunctionPointerUnmanagedCallingConventionBuilder.CreateSyntax(name);
        Syntax = Syntax.AddUnmanagedCallingConventionListCallingConventions(callingConvention);
        return this;
    }

    public IFunctionPointerCallingConventionBuilder AddFunctionPointerUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax callingConvention)
    {
        Syntax = Syntax.AddUnmanagedCallingConventionListCallingConventions(callingConvention);
        return this;
    }
}