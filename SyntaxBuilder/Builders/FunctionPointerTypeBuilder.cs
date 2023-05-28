using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IFunctionPointerTypeBuilder
{
    IFunctionPointerTypeBuilder AsManaged();
    IFunctionPointerTypeBuilder AsUnmanaged(params CallingConvention[] callingConventions);
    IFunctionPointerTypeBuilder WithParameter(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    // TODO Prevent return from being chained
    IFunctionPointerTypeBuilder WithReturn(Func<ITypeBuilder, ITypeBuilder> typeCallback);
}

public sealed class FunctionPointerTypeBuilder : IFunctionPointerTypeBuilder
{
    public FunctionPointerTypeSyntax Syntax { get; set; }

    public FunctionPointerTypeBuilder(FunctionPointerTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerTypeSyntax CreateSyntax(Func<IFunctionPointerTypeBuilder, IFunctionPointerTypeBuilder> functionPointerCallback)
    {
        var functionPointerTypeSyntax = SyntaxFactory.FunctionPointerType();

        var builder = new FunctionPointerTypeBuilder(functionPointerTypeSyntax).AssertInvoke(functionPointerCallback);

        return builder.Syntax;
    }



    public IFunctionPointerTypeBuilder AsManaged()
    {
        Syntax = Syntax.WithCallingConvention(
            SyntaxFactory.FunctionPointerCallingConvention(
                SyntaxFactory.Token(
                    SyntaxKind.ManagedKeyword
                )
            )
        );

        return this;
    }

    public IFunctionPointerTypeBuilder AsUnmanaged(params CallingConvention[] callingConventions)
    {
        Syntax = Syntax.WithCallingConvention(
            SyntaxFactory.FunctionPointerCallingConvention(
                SyntaxFactory.Token(
                    SyntaxKind.UnmanagedKeyword
                ),
                callingConventions.Any()
                    ? SyntaxFactory.FunctionPointerUnmanagedCallingConventionList(
                        SyntaxFactory.SeparatedList(
                            callingConventions.Select(x =>
                                SyntaxFactory.FunctionPointerUnmanagedCallingConvention(
                                    SyntaxFactory.Identifier(x.ToString())
                                )
                            )
                        )
                    )
                    : null
            )
        );

        return this;
    }

    public IFunctionPointerTypeBuilder WithParameter(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var parameterTypeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        Syntax = Syntax.AddParameterListParameters(
            SyntaxFactory.FunctionPointerParameter(parameterTypeSyntax)
        );

        return this;
    }

    public IFunctionPointerTypeBuilder WithReturn(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        Syntax = Syntax.AddParameterListParameters(
            SyntaxFactory.FunctionPointerParameter(returnTypeSyntax)
        );

        return this;
    }
}
