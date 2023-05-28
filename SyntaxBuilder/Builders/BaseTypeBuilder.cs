using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IBaseTypeBuilder : IBaseTypeBuilder<IBaseTypeBuilder>
{

}

public interface IBaseTypeBuilder<TTypeBuilder> where TTypeBuilder : IBaseTypeBuilder<TTypeBuilder>
{
    TTypeBuilder AsGeneric(string typeName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback);
    TTypeBuilder AsGeneric(string identifier, string namespaceName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback);
    TTypeBuilder AsGeneric(Type type, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback);
    TTypeBuilder AsType(Func<INameBuilder, INameBuilder> nameCallback);
    TTypeBuilder AsType(Type type);
    TTypeBuilder AsType(string typeName);
}

public sealed class BaseTypeBuilder : IBaseTypeBuilder
{
    public TypeSyntax? Syntax { get; set; }

    public BaseTypeBuilder() { }

    public static TypeSyntax CreateSyntax(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback)
    {
        var builder = new BaseTypeBuilder().AssertInvoke(typeCallback);

        if (builder.Syntax == null)
        {
            throw new InvalidOperationException("Type syntax was not specified.");
        }

        return builder.Syntax;
    }

    public IBaseTypeBuilder AsType(Type type)
    {
        Syntax = TypeBuilder.CreateSyntax(type);

        return this;
    }

    public IBaseTypeBuilder AsType(string typeName)
    {
        Syntax = TypeBuilder.CreateSyntax(typeName);

        return this;
    }

    public IBaseTypeBuilder AsType(Func<INameBuilder, INameBuilder> nameCallback)
    {
        Syntax = NameBuilder.CreateSyntax(nameCallback);

        return this;
    }

    public IBaseTypeBuilder AsGeneric(Type type, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        // TODO: Move to GenericameBuilder ???

        if (!type.IsGenericTypeDefinition)
        {
            throw new InvalidOperationException($"Type {type.Name} is not a generic type definition");
        }

        var typeName = type.Name[..type.Name.IndexOf('`')];

        var syntax = GenericNameBuilder.CreateSyntax(typeName, genericCallback);

        if (syntax.TypeArgumentList.Arguments.Count != type.GetGenericArguments().Length)
        {
            throw new InvalidOperationException($"The number of specified type arguments do not match the type arguments for type '{type.Name}'");
        }

        Syntax = syntax;

        return this;
    }

    public IBaseTypeBuilder AsGeneric(string typeName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        Syntax = GenericNameBuilder.CreateSyntax(typeName, genericCallback);

        return this;
    }

    public IBaseTypeBuilder AsGeneric(string identifier, string namespaceName, Func<IGenericNameBuilder, IGenericNameBuilder> genericCallback)
    {
        // TODO: validate namespace

        var syntax = GenericNameBuilder.CreateSyntax(identifier, genericCallback);

        // TODO: QualifiedNameBuilder?
        Syntax = SyntaxFactory.QualifiedName(
            SyntaxFactory.IdentifierName(namespaceName),
            syntax
        );

        return this;
    }
}

public static class BaseTypeBuilderExtensions
{
    public static IBaseTypeBuilder AsType<T>(this IBaseTypeBuilder builder)
    {
        return builder.AsType(typeof(T));
    }

    public static TTypeBuilder AsType<TTypeBuilder>(this TTypeBuilder builder, string typeName)
        where TTypeBuilder : IBaseTypeBuilder<TTypeBuilder>
    {
        return builder.AsType(x => x.ParseName(typeName));
    }
}