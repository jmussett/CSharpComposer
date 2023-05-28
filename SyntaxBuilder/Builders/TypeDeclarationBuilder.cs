using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface ITypeDeclarationBuilder : ITypeDeclarationBuilder<ITypeDeclarationBuilder>
{

}

public interface ITypeDeclarationBuilder<TBuilder> : IBaseTypeDeclarationBuilder<TBuilder>
    where TBuilder : ITypeDeclarationBuilder<TBuilder>
{
    TBuilder WithBaseType(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback);
    TBuilder WithMethod(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback);
    TBuilder WithProperty(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null);
    TBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null);
}

public sealed class TypeDeclarationBuilder : ITypeDeclarationBuilder
{
    private BaseTypeDeclarationBuilder _baseTypeBuilder;

    public TypeDeclarationSyntax Syntax
    {
        get => (TypeDeclarationSyntax)_baseTypeBuilder.Syntax;
        set => _baseTypeBuilder.Syntax = value;
    }

    public TypeDeclarationBuilder(TypeDeclarationSyntax syntax, AttributeTargets attributeTarget)
    {
        _baseTypeBuilder = new(syntax, attributeTarget);
    }

    public ITypeDeclarationBuilder WithBaseType(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback)
    {
        var typeSyntax = BaseTypeBuilder.CreateSyntax(typeCallback);

        Syntax = (TypeDeclarationSyntax)Syntax.AddBaseListTypes(
            SyntaxFactory.SimpleBaseType(typeSyntax)
        );

        return this;
    }

    public ITypeDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var (typeParameter, constraintClause) = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);

        Syntax = Syntax.AddTypeParameterListParameters(typeParameter);

        if (constraintClause is not null)
        {
            Syntax = Syntax.AddConstraintClauses(constraintClause);
        }

        return this;
    }


    public ITypeDeclarationBuilder WithMethod(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        var methodSyntax = MethodDeclarationBuilder.CreateSyntax(identifier, typeCallback, methodCallback);

        Syntax = Syntax.AddMembers(methodSyntax);

        return this;
    }



    public ITypeDeclarationBuilder WithProperty(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null)
    {
        var propertySyntax = PropertyDeclarationBuilder.CreateSyntax(identifier, typeCallback, propertyCallback);

        Syntax = Syntax.AddMembers(propertySyntax);

        return this;
    }

    public ITypeDeclarationBuilder WithAccessModifier(TypeAccessModifier accessModifier)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public ITypeDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public ITypeDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public ITypeDeclarationBuilder WithUnsafeModifier()
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.WithUnsafeModifier());

        return this;
    }

    public ITypeDeclarationBuilder WithStaticModifier()
    {
        _baseTypeBuilder = _baseTypeBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }

    public ITypeDeclarationBuilder WithPartialModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        return this;
    }

    // TODO: WithDocumentation
    // TODO: Indexer
    // TODO: Events
    // TODO: Nested types
}

public static class TypeDeclarationBuilderExtensions
{

    public static ITypeDeclarationBuilder WithBaseType<T>(
        this ITypeDeclarationBuilder builder
    )
    {
        return builder.WithBaseType(x => x.AsType<T>());
    }

    public static ITypeDeclarationBuilder WithBaseType(
        this ITypeDeclarationBuilder builder,
        Type type
    )
    {
        return builder.WithBaseType(x => x.AsType(type));
    }

    public static ITypeDeclarationBuilder WithBaseType(
        this ITypeDeclarationBuilder builder,
        string typeName
    )
    {
        return builder.WithBaseType(x => x.AsType(typeName));
    }

    public static ITypeDeclarationBuilder WithMethod<T>(
        this ITypeDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsType<T>(), methodCallback);
    }

    public static ITypeDeclarationBuilder WithMethod(
       this ITypeDeclarationBuilder builder,
       string identifier,
       Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
   )
    {
        return builder.WithMethod(identifier, x => x.AsVoid(), methodCallback);
    }

    public static ITypeDeclarationBuilder WithProperty<T>(
        this ITypeDeclarationBuilder builder,
        string identifier,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? IPropertyBuilder = null
    )
    {
        return builder.WithProperty(identifier, x => x.AsType<T>(), IPropertyBuilder);
    }
}