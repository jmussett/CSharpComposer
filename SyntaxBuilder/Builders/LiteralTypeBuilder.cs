using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface ILiteralTypeBuilder : ITypeBuilder<ILiteralTypeBuilder>
{
    ILiteralTypeBuilder AsVar();
}

public sealed class LiteralTypeBuilder : ILiteralTypeBuilder
{
    private TypeBuilder _typeBuilder = new();

    public TypeSyntax? Syntax
    {
        get => _typeBuilder.Syntax;
        set => _typeBuilder.Syntax = value;
    }

    public LiteralTypeBuilder() { }

    public static TypeSyntax CreateSyntax(Func<ILiteralTypeBuilder, ILiteralTypeBuilder> returnTypeCallback)
    {
        var builder = new LiteralTypeBuilder().AssertInvoke(returnTypeCallback);

        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("Type syntax was not specified.");
        }

        return builder.Syntax;
    }

    public ILiteralTypeBuilder AsVar()
    {
        // We use a hardcoded identifier name instead of the VarKeyword token
        // as we require a TypeSyntax, not a SyntaxToken
        Syntax = SyntaxFactory.IdentifierName("var");

        return this;
    }

    public ILiteralTypeBuilder AsArray(byte arrayRank, Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsArray(arrayRank, typeCallback));

        return this;
    }

    public ILiteralTypeBuilder AsArray(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsArray(typeCallback));

        return this;
    }

    public ILiteralTypeBuilder AsFunctionPointer(Func<IFunctionPointerTypeBuilder, IFunctionPointerTypeBuilder> functionPointerCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsFunctionPointer(functionPointerCallback));

        return this;
    }

    public ILiteralTypeBuilder AsNullable(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsNullable(typeCallback));

        return this;
    }

    public ILiteralTypeBuilder AsPointer(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsPointer(typeCallback));

        return this;
    }

    public ILiteralTypeBuilder AsRef(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsRef(typeCallback));

        return this;
    }

    public ILiteralTypeBuilder AsTuple(Func<ITupleTypeBuilder, ITupleTypeBuilder> tupleCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsTuple(tupleCallback));

        return this;
    }

    public ILiteralTypeBuilder AsGeneric(string typeName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsGeneric(typeName, genericCallback));

        return this;
    }

    public ILiteralTypeBuilder AsGeneric(string identifier, string namespaceName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsGeneric(identifier, namespaceName, genericCallback));

        return this;
    }

    public ILiteralTypeBuilder AsGeneric(Type type, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsGeneric(type, genericCallback));

        return this;
    }

    public ILiteralTypeBuilder AsType(Func<INameBuilder, INameBuilder> nameCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsType(nameCallback));

        return this;
    }

    public ILiteralTypeBuilder AsType(Type type)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsType(type));

        return this;
    }

    public ILiteralTypeBuilder AsType(string typeName)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsType(typeName));

        return this;
    }

    public ILiteralTypeBuilder AsVoid()
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.AsVoid());

        return this;
    }
}

public static class LiteralTypeBuilderExtensions
{

    public static ILiteralTypeBuilder AsType<T>(this ILiteralTypeBuilder builder)
    {
        return builder.AsType(typeof(T));
    }

    public static ILiteralTypeBuilder AsType(this ILiteralTypeBuilder builder, string typeName)
    {
        return builder.AsType(x => x.ParseName(typeName));
    }
}