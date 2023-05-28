using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IStructDeclarationBuilder : IConcreteTypeDeclarationBuilder<IStructDeclarationBuilder>
{
    IStructDeclarationBuilder WithReadOnlyModifier();
}

public sealed class StructDeclarationBuilder : IStructDeclarationBuilder
{
    private ConcreteTypeDeclarationBuilder _concreteTypeBuilder;

    public StructDeclarationSyntax Syntax
    {
        get => (StructDeclarationSyntax)_concreteTypeBuilder.Syntax;
        set => _concreteTypeBuilder.Syntax = value;
    }

    public StructDeclarationBuilder(StructDeclarationSyntax syntax)
    {
        _concreteTypeBuilder = new(syntax, AttributeTargets.Struct);
    }

    public static StructDeclarationSyntax CreateSyntax(string identifier, Func<IStructDeclarationBuilder, IStructDeclarationBuilder> interfaceCallback)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var structSyntax = SyntaxFactory.StructDeclaration(identifier);

        var builder = new StructDeclarationBuilder(structSyntax).AssertInvoke(interfaceCallback);

        return builder.Syntax;
    }

    public IStructDeclarationBuilder WithReadOnlyModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));

        return this;
    }

    public IStructDeclarationBuilder WithConstructor(Func<IConstructorDeclarationBuilder, IConstructorDeclarationBuilder>? callback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithConstructor(callback));

        return this;
    }

    public IStructDeclarationBuilder WithField(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithField(identifier, typeCallback, fieldCallback));

        return this;
    }

    public IStructDeclarationBuilder WithBaseType(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithBaseType(typeCallback));

        return this;
    }

    public IStructDeclarationBuilder WithMethod(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithMethod(identifier, typeCallback, methodCallback));

        return this;
    }

    public IStructDeclarationBuilder WithProperty(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithProperty(identifier, typeCallback, propertyCallback));
        return this;
    }

    public IStructDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithTypeParameter(identifier, typeParameterCallback));

        return this;
    }

    public IStructDeclarationBuilder WithAccessModifier(TypeAccessModifier accessModifier)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IStructDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IStructDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IStructDeclarationBuilder WithUnsafeModifier()
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithUnsafeModifier());

        return this;
    }

    public IStructDeclarationBuilder WithOperator(Func<ITypeBuilder, ITypeBuilder> typeCallback, OperatorType operatorType, Func<IOperatorDeclarationBuilder, IOperatorDeclarationBuilder> operatorCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithOperator(typeCallback, operatorType, operatorCallback));

        return this;
    }

    public IStructDeclarationBuilder WithConversionOperator(ConversionOperatorType operatorType, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IConversionOperatorDeclarationBuilder, IConversionOperatorDeclarationBuilder> operatorCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithConversionOperator(operatorType, typeCallback, operatorCallback));

        return this;
    }

    public IStructDeclarationBuilder WithStaticModifier()
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }

    public IStructDeclarationBuilder WithPartialModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        return this;
    }
}

public static class StructBuilderExtensions
{

    public static IStructDeclarationBuilder WithField<T>(
        this IStructDeclarationBuilder builder,
        string identifier,
        Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null
    )
    {
        return builder.WithField(identifier, x => x.AsType<T>(), fieldCallback);
    }

    public static IStructDeclarationBuilder WithProperty<T>(
        this IStructDeclarationBuilder builder,
        string identifier,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? IPropertyBuilder = null
    )
    {
        return builder.WithProperty(identifier, x => x.AsType<T>(), IPropertyBuilder);
    }

    public static IStructDeclarationBuilder WithMethod(
        this IStructDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsVoid(), methodCallback);
    }

    public static IStructDeclarationBuilder WithAttribute<T>(
        this IStructDeclarationBuilder builder,
        Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null
    )
    {
        return builder.WithAttribute(typeof(T), attributeCallback);
    }
}