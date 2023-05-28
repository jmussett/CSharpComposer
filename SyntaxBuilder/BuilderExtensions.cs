using SyntaxBuilder.Builders;

namespace SyntaxBuilder;

public static class BuilderExtensions
{
    public static TBuilderImpl AssertInvoke<TBuilder, TBuilderImpl>(this TBuilderImpl builder, Func<TBuilder, TBuilder> callback)
        where TBuilderImpl : class, TBuilder
    {
        TBuilder newBuilder = callback(builder);

        if (!builder.Equals(newBuilder))
        {
            throw new InvalidOperationException($"Builder instance '{typeof(TBuilder).Name}' does not match parameter instance");
        }

        return (TBuilderImpl)newBuilder!;
    }

    public static TBuilderImpl AssertInvoke<TBuilder, TBuilderImpl>(this TBuilderImpl builder, Func<TBuilderImpl, TBuilder> callback)
       where TBuilderImpl : class, TBuilder
    {
        TBuilder newBuilder = callback(builder);

        if (!builder.Equals(newBuilder))
        {
            throw new InvalidOperationException($"Builder instance '{typeof(TBuilder).Name}' does not match parameter instance");
        }

        return (TBuilderImpl)newBuilder!;
    }


    // TODO: Move to dedicated Builder Extension classes

    public static IBaseTypeDeclarationBuilder WithAttribute<T>(this IBaseTypeDeclarationBuilder builder, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
        where T : Attribute
    {
        return builder.WithAttribute(typeof(T), attributeCallback);
    }

    public static IForStatementBuilder WithDeclaration<T>(
        this IForStatementBuilder builder,
        string identifier,
        T literalValue
    )
    {
        return builder.WithDeclaration(identifier, x => x.AsType<T>(), x => x.AsLiteral(literalValue));
    }

    public static IConcreteTypeDeclarationBuilder WithField<T>(
        this IConcreteTypeDeclarationBuilder builder,
        string identifier,
        Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null
    )
    {
        return builder.WithField(identifier, x => x.AsType<T>(), fieldCallback);
    }

    public static IAccessorDeclarationBuilder WithAttribute<T>(
        this IAccessorDeclarationBuilder builder, 
        Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
       where T : Attribute
    {
        return builder.WithAttribute(typeof(T), attributeCallback);
    }

    public static IDelegateDeclarationBuilder WithParameter<T>(
        this IDelegateDeclarationBuilder builder,
        string identifier,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        return builder.WithParameter(identifier, x => x.AsType<T>(), parameterCallback);
    }

    public static IFunctionPointerTypeBuilder WithParameter<T>(
        this IFunctionPointerTypeBuilder builder
    )
    {
        return builder.WithParameter(x => x.AsType<T>());
    }

    public static ILocalFunctionStatementBuilder WithParameter<T>(
        this ILocalFunctionStatementBuilder builder,
        string identifier,
        Func<IParameterBuilder, IParameterBuilder>? paramCallback = null
    )
    {
        return builder.WithParameter(identifier, x => x.AsType<T>(), paramCallback);
    }

    public static IMethodDeclarationBuilder WithParameter<T>(
        this IMethodDeclarationBuilder builder,
        string identifier,
        Func<IParameterBuilder, IParameterBuilder>? paramCallback = null
    )
    {
        return builder.WithParameter(identifier, x => x.AsType<T>(), paramCallback);
    }

    public static ITypeParameterBuilder WithTypeConstraint<T>(this ITypeParameterBuilder builder)
    {
        return builder.WithTypeConstraint(x => x.AsType<T>());
    }
}
