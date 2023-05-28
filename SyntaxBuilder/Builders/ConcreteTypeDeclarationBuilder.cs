using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IConcreteTypeDeclarationBuilder : IConcreteTypeDeclarationBuilder<IConcreteTypeDeclarationBuilder>
{

}

public interface IConcreteTypeDeclarationBuilder<TTypeBuilder> : ITypeDeclarationBuilder<TTypeBuilder>
    where TTypeBuilder : IConcreteTypeDeclarationBuilder<TTypeBuilder>
{
    TTypeBuilder WithConstructor(Func<IConstructorDeclarationBuilder, IConstructorDeclarationBuilder>? callback = null);
    TTypeBuilder WithField(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null
    );
    TTypeBuilder WithOperator(
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        OperatorType operatorType,
        Func<IOperatorDeclarationBuilder, IOperatorDeclarationBuilder> operatorCallback
    );
    TTypeBuilder WithConversionOperator(
        ConversionOperatorType operatorType,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IConversionOperatorDeclarationBuilder, IConversionOperatorDeclarationBuilder> operatorCallback
    );
}

public sealed class ConcreteTypeDeclarationBuilder : IConcreteTypeDeclarationBuilder
{
    private TypeDeclarationBuilder _typeBuilder;

    public TypeDeclarationSyntax Syntax
    {
        get => _typeBuilder.Syntax;
        set => _typeBuilder.Syntax = value;
    }

    public ConcreteTypeDeclarationBuilder(TypeDeclarationSyntax syntax, AttributeTargets targets)
    {
        _typeBuilder = new(syntax, targets);
    }

    public IConcreteTypeDeclarationBuilder WithConstructor(Func<IConstructorDeclarationBuilder, IConstructorDeclarationBuilder>? callback = null)
    {
        var syntax = ConstructorDeclarationBuilder.CreateSyntax(Syntax.Identifier.Text, callback);

        Syntax = Syntax.AddMembers(syntax);

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithField(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null
    )
    {
        var fieldSyntax = FieldDeclarationBuilder.CreateSyntax(identifier, typeCallback, fieldCallback);

        Syntax = Syntax.AddMembers(fieldSyntax);

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithOperator(Func<ITypeBuilder, ITypeBuilder> typeCallback, OperatorType operatorType, Func<IOperatorDeclarationBuilder, IOperatorDeclarationBuilder> operatorCallback)
    {
        var operatorSyntax = OperatorDeclarationBuilder.CreateSyntax(typeCallback, operatorType, operatorCallback);

        Syntax = Syntax.AddMembers(operatorSyntax);

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithConversionOperator(ConversionOperatorType operatorType, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IConversionOperatorDeclarationBuilder, IConversionOperatorDeclarationBuilder> operatorCallback)
    {
        var operatorSyntax = ConversionOperatorDeclarationBuilder.CreateSyntax(operatorType, typeCallback, operatorCallback);

        Syntax = Syntax.AddMembers(operatorSyntax);

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithBaseType(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithBaseType(typeCallback));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithMethod(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithMethod(identifier, typeCallback, methodCallback));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithProperty(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null
    )
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithProperty(identifier, typeCallback, propertyCallback));
        return this;
    }

    public IConcreteTypeDeclarationBuilder WithTypeParameter(
        string identifier,
        Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null
    )
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithTypeParameter(identifier, typeParameterCallback));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithAccessModifier(TypeAccessModifier accessModifier)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithUnsafeModifier()
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithUnsafeModifier());

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithStaticModifier()
    {
        _typeBuilder = _typeBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }

    public IConcreteTypeDeclarationBuilder WithPartialModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        return this;
    }
}


public static class ConcreteTypeDeclarationBuilderExtensions
{
    public static IConcreteTypeDeclarationBuilder WithMethod<T>(
        this IConcreteTypeDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsType<T>(), methodCallback);
    }

    public static IConcreteTypeDeclarationBuilder WithMethod(
        this IConcreteTypeDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsVoid(), methodCallback);
    }
}
