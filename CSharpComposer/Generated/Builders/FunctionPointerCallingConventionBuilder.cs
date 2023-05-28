using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerCallingConventionBuilder
{
    IFunctionPointerCallingConventionBuilder AddUnmanagedCallingConvention(string name);
    IFunctionPointerCallingConventionBuilder AddUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax callingConvention);
}

public interface IWithFunctionPointerCallingConventionBuilder<TBuilder>
{
    TBuilder WithFunctionPointerCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder> functionPointerCallingConventionCallback);
    TBuilder WithFunctionPointerCallingConvention(FunctionPointerCallingConventionSyntax functionPointerCallingConventionSyntax);
}

public partial class FunctionPointerCallingConventionBuilder : IFunctionPointerCallingConventionBuilder
{
    public FunctionPointerCallingConventionSyntax Syntax { get; set; }

    public FunctionPointerCallingConventionBuilder(FunctionPointerCallingConventionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerCallingConventionSyntax CreateSyntax(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder> functionPointerCallingConventionCallback)
    {
        var managedOrUnmanagedKeywordToken = functionPointerCallingConventionManagedOrUnmanagedKeyword switch
        {
            FunctionPointerCallingConventionManagedOrUnmanagedKeyword.ManagedKeyword => SyntaxFactory.Token(SyntaxKind.ManagedKeyword),
            FunctionPointerCallingConventionManagedOrUnmanagedKeyword.UnmanagedKeyword => SyntaxFactory.Token(SyntaxKind.UnmanagedKeyword),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.FunctionPointerCallingConvention(managedOrUnmanagedKeywordToken, default(FunctionPointerUnmanagedCallingConventionListSyntax));
        var builder = new FunctionPointerCallingConventionBuilder(syntax);
        functionPointerCallingConventionCallback(builder);
        return builder.Syntax;
    }

    public IFunctionPointerCallingConventionBuilder AddUnmanagedCallingConvention(string name)
    {
        var callingConvention = FunctionPointerUnmanagedCallingConventionBuilder.CreateSyntax(name);
        Syntax = Syntax.AddUnmanagedCallingConventionListCallingConventions(callingConvention);
        return this;
    }

    public IFunctionPointerCallingConventionBuilder AddUnmanagedCallingConvention(FunctionPointerUnmanagedCallingConventionSyntax callingConvention)
    {
        Syntax = Syntax.AddUnmanagedCallingConventionListCallingConventions(callingConvention);
        return this;
    }
}