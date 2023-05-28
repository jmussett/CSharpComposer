using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Builders;

namespace SyntaxBuilder.Builders;

// TODO: Make type builder non-chainable to prevent errors.

public interface ITypeBuilder : ITypeBuilder<ITypeBuilder>
{

}

public interface ITypeBuilder<TTypeBuilder> : IBaseTypeBuilder<TTypeBuilder>
    where TTypeBuilder : ITypeBuilder<TTypeBuilder>
{
    TTypeBuilder AsArray(byte arrayRank, Func<ITypeBuilder, ITypeBuilder> typeCallback);
    TTypeBuilder AsArray(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    TTypeBuilder AsFunctionPointer(Func<IFunctionPointerTypeBuilder, IFunctionPointerTypeBuilder> functionPointerCallback);
    TTypeBuilder AsNullable(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    TTypeBuilder AsPointer(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    TTypeBuilder AsRef(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    TTypeBuilder AsTuple(Func<ITupleTypeBuilder, ITupleTypeBuilder> tupleCallback);
    TTypeBuilder AsVoid();
}

public sealed class TypeBuilder : ITypeBuilder
{
    private BaseTypeBuilder _baseTypeBuilder = new();

    public TypeSyntax? Syntax
    {
        get => _baseTypeBuilder.Syntax;
        set => _baseTypeBuilder.Syntax = value;
    }

    public TypeBuilder() { }

    public static TypeSyntax CreateSyntax(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var builder = new TypeBuilder().AssertInvoke(typeCallback);

        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("Type syntax was not specified.");
        }

        return builder.Syntax;
    }

    // TODO: Refactor this method
    public static TypeSyntax CreateSyntax(Type type)
    {
        SyntaxKind? syntaxKind = Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => SyntaxKind.BoolKeyword,
            TypeCode.Byte => SyntaxKind.ByteKeyword,
            TypeCode.SByte => SyntaxKind.SByteKeyword,
            TypeCode.Char => SyntaxKind.CharKeyword,
            TypeCode.Decimal => SyntaxKind.DecimalKeyword,
            TypeCode.Double => SyntaxKind.DoubleKeyword,
            TypeCode.Single => SyntaxKind.FloatKeyword,
            TypeCode.Int16 => SyntaxKind.ShortKeyword,
            TypeCode.Int32 => SyntaxKind.IntKeyword,
            TypeCode.Int64 => SyntaxKind.LongKeyword,
            TypeCode.UInt16 => SyntaxKind.UShortKeyword,
            TypeCode.UInt32 => SyntaxKind.UIntKeyword,
            TypeCode.UInt64 => SyntaxKind.ULongKeyword,
            TypeCode.String => SyntaxKind.StringKeyword,
            _ => null
        };

        if (syntaxKind is not null)
        {
            return SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(syntaxKind.Value)
            );
        }

        // Missing syntax kind for nint and nuint

        if (type == typeof(IntPtr))
        {
            return SyntaxFactory.IdentifierName("nint");
        }

        if (type == typeof(UIntPtr))
        {
            return SyntaxFactory.IdentifierName("nuint");
        }

        var nullableType = Nullable.GetUnderlyingType(type);

        if (nullableType is not null)
        {
            return SyntaxFactory.NullableType(CreateSyntax(nullableType));
        }

        if (type.IsArray)
        {
            // TODO: Move to ArrayTypeBuilder

            var elementType = CreateSyntax(type.GetElementType()!);
            var arrayRank = type.GetArrayRank();

            var arraySize = new SyntaxList<ArrayRankSpecifierSyntax>();
            for (int i = 0; i < arrayRank; i++)
            {
                arraySize = arraySize.Add(SyntaxFactory.ArrayRankSpecifier());
            }

            return SyntaxFactory.ArrayType(elementType, arraySize);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTuple<,>))
        {
            // TODO: Move to TupleTypeBuilder
            var tupleElements = type.GetGenericArguments().Select(CreateSyntax);

            return SyntaxFactory.TupleType(
                SyntaxFactory.SeparatedList(
                    tupleElements.Select(SyntaxFactory.TupleElement)
                )
            );
        }

        if (type.IsPointer)
        {
            return SyntaxFactory.PointerType(CreateSyntax(type.GetElementType()!));
        }

        if (type.IsByRef)
        {
            return SyntaxFactory.RefType(CreateSyntax(type.GetElementType()!));
        }

        var typeName = type.Name;
        var namespaceName = type.Namespace;

        if (type.IsGenericType)
        {
            typeName = type.Name[..type.Name.IndexOf('`')];

            Func<IGenericNameBuilder, IGenericNameBuilder> genericNameCallback = x =>
            {
                foreach (var genericArgumentType in type.GetGenericArguments())
                {
                    x = x.WithTypeArgument(x => x.AsType(genericArgumentType));
                }

                return x;
            };

            if (namespaceName == null)
            {
                return GenericNameBuilder.CreateSyntax(typeName, genericNameCallback);
            }

            return NameBuilder.CreateSyntax(
                x => x.AsQualified(x => x.ParseName(namespaceName),
                x => x.AsGeneric(typeName, genericNameCallback))
            );
        }

        if (namespaceName is null)
        {
            return NameBuilder.CreateSyntax(x => x.AsIdentifier(typeName));
        }

        return NameBuilder.CreateSyntax(
            x => x.AsQualified(x => x.ParseName(namespaceName),
            x => x.AsIdentifier(typeName))
        );
    }

    public static TypeSyntax CreateSyntax(string typeName)
    {
        SyntaxKind? syntaxKind = typeName switch
        {
            "bool" => SyntaxKind.BoolKeyword,
            "byte" => SyntaxKind.ByteKeyword,
            "sbyte" => SyntaxKind.SByteKeyword,
            "short" => SyntaxKind.ShortKeyword,
            "ushort" => SyntaxKind.UShortKeyword,
            "int" => SyntaxKind.IntKeyword,
            "uint" => SyntaxKind.UIntKeyword,
            "long" => SyntaxKind.LongKeyword,
            "ulong" => SyntaxKind.ULongKeyword,
            "float" => SyntaxKind.FloatKeyword,
            "double" => SyntaxKind.DoubleKeyword,
            "decimal" => SyntaxKind.DecimalKeyword,
            "char" => SyntaxKind.CharKeyword,
            "string" => SyntaxKind.StringKeyword,
            "object" => SyntaxKind.ObjectKeyword,
            "void" => SyntaxKind.VoidKeyword,
            _ => null
        };

        if (syntaxKind is not null)
        {
            return SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(syntaxKind.Value)
            );
        }

        return SyntaxFactory.ParseTypeName(typeName);
    }

    public ITypeBuilder AsFunctionPointer(Func<IFunctionPointerTypeBuilder, IFunctionPointerTypeBuilder> functionPointerCallback)
    {
        Syntax = FunctionPointerTypeBuilder.CreateSyntax(functionPointerCallback);

        return this;
    }

    public ITypeBuilder AsNullable(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var underlyingTypeSyntax = CreateSyntax(typeCallback);

        Syntax = SyntaxFactory.NullableType(underlyingTypeSyntax);

        return this;
    }

    public ITypeBuilder AsArray(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var underlyingTypeSyntax = CreateSyntax(typeCallback);

        Syntax = SyntaxFactory.ArrayType(underlyingTypeSyntax);

        return this;
    }

    public ITypeBuilder AsArray(byte arrayRank, Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var underlyingTypeSyntax = CreateSyntax(typeCallback);

        var arraySize = new SyntaxList<ArrayRankSpecifierSyntax>();
        for (int i = 0; i < arrayRank; i++)
        {
            arraySize = arraySize.Add(SyntaxFactory.ArrayRankSpecifier());
        }

        Syntax = SyntaxFactory.ArrayType(underlyingTypeSyntax, arraySize);

        return this;
    }

    public ITypeBuilder AsTuple(Func<ITupleTypeBuilder, ITupleTypeBuilder> tupleCallback)
    {
        Syntax = TupleTypeBuilder.CreateSyntax(tupleCallback);

        return this;
    }

    public ITypeBuilder AsPointer(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var underlyingTypeSyntax = CreateSyntax(typeCallback);

        Syntax = SyntaxFactory.PointerType(underlyingTypeSyntax);

        return this;
    }

    public ITypeBuilder AsRef(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        var underlyingTypeSyntax = CreateSyntax(typeCallback);

        Syntax = SyntaxFactory.RefType(underlyingTypeSyntax);

        return this;
    }

    public ITypeBuilder AsGeneric(string typeName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsGeneric(typeName, genericCallback));

        return this;
    }

    public ITypeBuilder AsGeneric(string identifier, string namespaceName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsGeneric(identifier, namespaceName, genericCallback));

        return this;
    }

    public ITypeBuilder AsGeneric(Type type, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsGeneric(type, genericCallback));

        return this;
    }

    public ITypeBuilder AsType(Func<INameBuilder, INameBuilder> nameCallback)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsType(nameCallback));

        return this;
    }

    public ITypeBuilder AsType(Type type)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsType(type));

        return this;
    }

    public ITypeBuilder AsType(string typeName)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.AsType(typeName));

        return this;
    }

    public ITypeBuilder AsVoid()
    {
        Syntax = SyntaxFactory.PredefinedType(
            SyntaxFactory.Token(SyntaxKind.VoidKeyword)
        );

        return this;
    }
}

public static class TypeBuilderExtensions
{
    public static ITypeBuilder AsType<T>(this ITypeBuilder builder)
    {
        return builder.AsType(typeof(T));
    }

    public static ITypeBuilder AsType(this ITypeBuilder builder, string typeName)
    {
        return builder.AsType(x => x.ParseName(typeName));
    }
}
