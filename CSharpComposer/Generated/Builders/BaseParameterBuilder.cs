using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseParameterBuilder
{
    void AsParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null);
    void AsFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null);
}

public partial interface IBaseParameterBuilder<TBuilder> : IAddAttribute<TBuilder>
{
    TBuilder AddModifierToken(SyntaxKind modifier);
}

internal partial class BaseParameterBuilder : IBaseParameterBuilder
{
    public BaseParameterSyntax? Syntax { get; set; }

    public static BaseParameterSyntax CreateSyntax(Action<IBaseParameterBuilder> callback)
    {
        var builder = new BaseParameterBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseParameterSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        Syntax = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
    }

    public void AsFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null)
    {
        Syntax = FunctionPointerParameterBuilder.CreateSyntax(typeCallback, functionPointerParameterCallback);
    }
}