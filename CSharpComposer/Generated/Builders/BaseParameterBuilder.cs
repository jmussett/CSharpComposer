using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseParameterBuilder
{
    void AsParameter(string identifier, Action<IParameterBuilder> parameterCallback);
    void AsFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback);
}

public partial interface IBaseParameterBuilder<TBuilder> : IWithTypeBuilder<TBuilder>
{
}

public interface IWithBaseParameterBuilder<TBuilder>
{
    TBuilder WithBaseParameter(Action<IBaseParameterBuilder> baseParameterCallback);
    TBuilder WithBaseParameter(BaseParameterSyntax baseParameterSyntax);
}

public partial class BaseParameterBuilder : IBaseParameterBuilder
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

    public void AsParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        Syntax = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
    }

    public void AsFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback)
    {
        Syntax = FunctionPointerParameterBuilder.CreateSyntax(typeCallback, functionPointerParameterCallback);
    }
}