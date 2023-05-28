using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseTypeBuilder
{
    void AsSimpleBaseType(Action<ITypeBuilder> typeCallback);
    void AsPrimaryConstructorBaseType(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder> primaryConstructorBaseTypeCallback);
}

public interface IWithBaseTypeBuilder<TBuilder>
{
    TBuilder WithBaseType(Action<IBaseTypeBuilder> baseTypeCallback);
    TBuilder WithBaseType(BaseTypeSyntax baseTypeSyntax);
}

public partial class BaseTypeBuilder : IBaseTypeBuilder
{
    public BaseTypeSyntax? Syntax { get; set; }

    public static BaseTypeSyntax CreateSyntax(Action<IBaseTypeBuilder> callback)
    {
        var builder = new BaseTypeBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseTypeSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsSimpleBaseType(Action<ITypeBuilder> typeCallback)
    {
        Syntax = SimpleBaseTypeBuilder.CreateSyntax(typeCallback);
    }

    public void AsPrimaryConstructorBaseType(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder> primaryConstructorBaseTypeCallback)
    {
        Syntax = PrimaryConstructorBaseTypeBuilder.CreateSyntax(typeCallback, primaryConstructorBaseTypeCallback);
    }
}