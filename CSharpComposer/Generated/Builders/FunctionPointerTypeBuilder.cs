using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerTypeBuilder
{
    IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder>? functionPointerCallingConventionCallback = null);
    IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionSyntax callingConvention);
    IFunctionPointerTypeBuilder AddFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null);
    IFunctionPointerTypeBuilder AddFunctionPointerParameter(FunctionPointerParameterSyntax parameter);
}

internal partial class FunctionPointerTypeBuilder : IFunctionPointerTypeBuilder
{
    public FunctionPointerTypeSyntax Syntax { get; set; }

    public FunctionPointerTypeBuilder(FunctionPointerTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerTypeSyntax CreateSyntax(Action<IFunctionPointerTypeBuilder>? functionPointerTypeCallback = null)
    {
        var delegateKeywordToken = SyntaxFactory.Token(SyntaxKind.DelegateKeyword);
        var asteriskTokenToken = SyntaxFactory.Token(SyntaxKind.AsteriskToken);
        var parameterListSyntax = SyntaxFactory.FunctionPointerParameterList();
        var syntax = SyntaxFactory.FunctionPointerType(delegateKeywordToken, asteriskTokenToken, default(FunctionPointerCallingConventionSyntax), parameterListSyntax);
        var builder = new FunctionPointerTypeBuilder(syntax);
        functionPointerTypeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder>? functionPointerCallingConventionCallback = null)
    {
        var callingConventionSyntax = FunctionPointerCallingConventionBuilder.CreateSyntax(functionPointerCallingConventionManagedOrUnmanagedKeyword, functionPointerCallingConventionCallback);
        Syntax = Syntax.WithCallingConvention(callingConventionSyntax);
        return this;
    }

    public IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionSyntax callingConvention)
    {
        Syntax = Syntax.WithCallingConvention(callingConvention);
        return this;
    }

    public IFunctionPointerTypeBuilder AddFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null)
    {
        var parameter = FunctionPointerParameterBuilder.CreateSyntax(typeCallback, functionPointerParameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IFunctionPointerTypeBuilder AddFunctionPointerParameter(FunctionPointerParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}