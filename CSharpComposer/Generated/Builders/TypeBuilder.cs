using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeBuilder
{
    void AsIdentifierName(string identifier);
    void AsGenericName(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null);
    void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback);
    void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback);
    void AsPredefinedType(PredefinedTypeKeyword predefinedTypeKeyword);
    void AsArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder>? arrayTypeCallback = null);
    void AsPointerType(Action<ITypeBuilder> elementTypeCallback);
    void AsFunctionPointerType(Action<IFunctionPointerTypeBuilder>? functionPointerTypeCallback = null);
    void AsNullableType(Action<ITypeBuilder> elementTypeCallback);
    void AsTupleType(Action<ITupleTypeBuilder>? tupleTypeCallback = null);
    void AsOmittedTypeArgument();
    void AsRefType(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder>? refTypeCallback = null);
    void AsScopedType(Action<ITypeBuilder> typeCallback);
}

public interface IWithType<TBuilder>
{
    TBuilder WithType(TypeSyntax typeSyntax);
    TBuilder WithType(Action<ITypeBuilder> typeCallback);
}

public interface IAddType<TBuilder>
{
    TBuilder AddType(TypeSyntax typeSyntax);
    TBuilder AddType(Action<ITypeBuilder> typeCallback);
}

internal partial class TypeBuilder : ITypeBuilder
{
    public TypeSyntax? Syntax { get; set; }

    public static TypeSyntax CreateSyntax(Action<ITypeBuilder> callback)
    {
        var builder = new TypeBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("TypeSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsIdentifierName(string identifier)
    {
        Syntax = IdentifierNameBuilder.CreateSyntax(identifier);
    }

    public void AsGenericName(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null)
    {
        Syntax = GenericNameBuilder.CreateSyntax(identifier, genericNameCallback);
    }

    public void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback)
    {
        Syntax = QualifiedNameBuilder.CreateSyntax(leftCallback, rightCallback);
    }

    public void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback)
    {
        Syntax = AliasQualifiedNameBuilder.CreateSyntax(aliasIdentifier, nameCallback);
    }

    public void AsPredefinedType(PredefinedTypeKeyword predefinedTypeKeyword)
    {
        Syntax = PredefinedTypeBuilder.CreateSyntax(predefinedTypeKeyword);
    }

    public void AsArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder>? arrayTypeCallback = null)
    {
        Syntax = ArrayTypeBuilder.CreateSyntax(elementTypeCallback, arrayTypeCallback);
    }

    public void AsPointerType(Action<ITypeBuilder> elementTypeCallback)
    {
        Syntax = PointerTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public void AsFunctionPointerType(Action<IFunctionPointerTypeBuilder>? functionPointerTypeCallback = null)
    {
        Syntax = FunctionPointerTypeBuilder.CreateSyntax(functionPointerTypeCallback);
    }

    public void AsNullableType(Action<ITypeBuilder> elementTypeCallback)
    {
        Syntax = NullableTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public void AsTupleType(Action<ITupleTypeBuilder>? tupleTypeCallback = null)
    {
        Syntax = TupleTypeBuilder.CreateSyntax(tupleTypeCallback);
    }

    public void AsOmittedTypeArgument()
    {
        Syntax = OmittedTypeArgumentBuilder.CreateSyntax();
    }

    public void AsRefType(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder>? refTypeCallback = null)
    {
        Syntax = RefTypeBuilder.CreateSyntax(typeCallback, refTypeCallback);
    }

    public void AsScopedType(Action<ITypeBuilder> typeCallback)
    {
        Syntax = ScopedTypeBuilder.CreateSyntax(typeCallback);
    }
}