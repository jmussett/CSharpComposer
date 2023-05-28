using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerTypeBuilder
{
    IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder> functionPointerCallingConventionCallback);
    IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionSyntax callingConvention);
    IFunctionPointerTypeBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback);
    IFunctionPointerTypeBuilder AddParameter(FunctionPointerParameterSyntax parameter);
}

public interface IWithFunctionPointerTypeBuilder<TBuilder>
{
    TBuilder WithFunctionPointerType(Action<IFunctionPointerTypeBuilder> functionPointerTypeCallback);
    TBuilder WithFunctionPointerType(FunctionPointerTypeSyntax functionPointerTypeSyntax);
}

public partial class FunctionPointerTypeBuilder : IFunctionPointerTypeBuilder
{
    public FunctionPointerTypeSyntax Syntax { get; set; }

    public FunctionPointerTypeBuilder(FunctionPointerTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerTypeSyntax CreateSyntax(Action<IFunctionPointerTypeBuilder> functionPointerTypeCallback)
    {
        var delegateKeywordToken = SyntaxFactory.Token(SyntaxKind.DelegateKeyword);
        var asteriskTokenToken = SyntaxFactory.Token(SyntaxKind.AsteriskToken);
        var parameterListSyntax = SyntaxFactory.FunctionPointerParameterList();
        var syntax = SyntaxFactory.FunctionPointerType(delegateKeywordToken, asteriskTokenToken, default(FunctionPointerCallingConventionSyntax), parameterListSyntax);
        var builder = new FunctionPointerTypeBuilder(syntax);
        functionPointerTypeCallback(builder);
        return builder.Syntax;
    }

    public IFunctionPointerTypeBuilder WithCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder> functionPointerCallingConventionCallback)
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

    public IFunctionPointerTypeBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback)
    {
        var parameter = FunctionPointerParameterBuilder.CreateSyntax(typeCallback, functionPointerParameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IFunctionPointerTypeBuilder AddParameter(FunctionPointerParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}