using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IClassDeclarationBuilder : IConcreteTypeDeclarationBuilder<IClassDeclarationBuilder>
{
}

public sealed class ClassDeclarationBuilder : IClassDeclarationBuilder
{
    private ConcreteTypeDeclarationBuilder _concreteTypeBuilder;

    public ClassDeclarationSyntax Syntax
    {
        get => (ClassDeclarationSyntax)_concreteTypeBuilder.Syntax;
        set => _concreteTypeBuilder.Syntax = value;
    }

    public ClassDeclarationBuilder(ClassDeclarationSyntax syntax)
    {
        _concreteTypeBuilder = new(syntax, AttributeTargets.Class);
    }

    public static ClassDeclarationSyntax CreateSyntax(string identifier, Func<IClassDeclarationBuilder, IClassDeclarationBuilder> classCallback)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var syntax = SyntaxFactory.ClassDeclaration(identifier);

        var builder = new ClassDeclarationBuilder(syntax).AssertInvoke(classCallback);

        return builder.Syntax;
    }

    public IClassDeclarationBuilder WithAccessModifier(TypeAccessModifier accessModifier)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));
        return this;
    }

    public IClassDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));
        return this;
    }

    public IClassDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));
        return this;
    }

    public IClassDeclarationBuilder WithBaseType(Func<IBaseTypeBuilder, IBaseTypeBuilder> typeCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithBaseType(typeCallback));
        return this;
    }

    public IClassDeclarationBuilder WithConstructor(Func<IConstructorDeclarationBuilder, IConstructorDeclarationBuilder>? callback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithConstructor(callback));
        return this;
    }

    public IClassDeclarationBuilder WithField(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithField(identifier, typeCallback, fieldCallback));
        return this;
    }

    public IClassDeclarationBuilder WithMethod(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithMethod(identifier, typeCallback, methodCallback));
        return this;
    }

    public IClassDeclarationBuilder WithProperty(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithProperty(identifier, typeCallback, propertyCallback));
        return this;
    }

    public IClassDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithTypeParameter(identifier, typeParameterCallback));
        return this;
    }

    public IClassDeclarationBuilder WithUnsafeModifier()
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithUnsafeModifier());
        return this;
    }

    public IClassDeclarationBuilder WithOperator(Func<ITypeBuilder, ITypeBuilder> typeCallback, OperatorType operatorType, Func<IOperatorDeclarationBuilder, IOperatorDeclarationBuilder> operatorCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithOperator(typeCallback, operatorType, operatorCallback));

        return this;
    }

    public IClassDeclarationBuilder WithConversionOperator(ConversionOperatorType operatorType, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IConversionOperatorDeclarationBuilder, IConversionOperatorDeclarationBuilder> operatorCallback)
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithConversionOperator(operatorType, typeCallback, operatorCallback));

        return this;
    }

    public IClassDeclarationBuilder WithStaticModifier()
    {
        _concreteTypeBuilder = _concreteTypeBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }

    public IClassDeclarationBuilder WithPartialModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        return this;
    }

    // Destructors
}

public static class ClassDeclarationBuilderExtensions
{
    public static IClassDeclarationBuilder WithMethod(
        this IClassDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsVoid(), methodCallback);
    }

    public static IClassDeclarationBuilder WithMethod<T>(
        this IClassDeclarationBuilder builder,
        string identifier,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        return builder.WithMethod(identifier, x => x.AsType<T>(), methodCallback);
    }

    public static IClassDeclarationBuilder WithProperty<T>(
        this IClassDeclarationBuilder builder,
        string identifier,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? IPropertyBuilder = null
    )
    {
        return builder.WithProperty(identifier, x => x.AsType<T>(), IPropertyBuilder);
    }

    public static IClassDeclarationBuilder WithField<T>(
        this IClassDeclarationBuilder builder,
        string identifier,
        Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null
    )
    {
        return builder.WithField(identifier, x => x.AsType<T>(), fieldCallback);
    }

    public static IClassDeclarationBuilder WithAttribute(
       this IClassDeclarationBuilder builder,
       string name,
       Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null
   )
    {
        return builder.WithAttribute(x => x.ParseName(name), attributeCallback);
    }


}
