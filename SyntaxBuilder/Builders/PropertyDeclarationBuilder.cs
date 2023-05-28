using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IPropertyDeclarationBuilder : IMemberDeclarationBuilder<IPropertyDeclarationBuilder>
{
    IPropertyDeclarationBuilder WithGetter(Func<IAccessorDeclarationBuilder, IAccessorDeclarationBuilder>? accessorCallback = null);
    IPropertyDeclarationBuilder WithSetter(Func<IAccessorDeclarationBuilder, IAccessorDeclarationBuilder>? accessorCallback = null);
}

public sealed class PropertyDeclarationBuilder : IPropertyDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public PropertyDeclarationSyntax Syntax
    {
        get => (PropertyDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public PropertyDeclarationBuilder(PropertyDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Property);
    }

    public static PropertyDeclarationSyntax CreateSyntax(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IPropertyDeclarationBuilder, IPropertyDeclarationBuilder>? propertyCallback = null
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var syntax = SyntaxFactory.PropertyDeclaration(typeSyntax, identifier);

        var builder = new PropertyDeclarationBuilder(syntax);

        builder = propertyCallback is not null
            ? builder.AssertInvoke(propertyCallback)
            : builder.AssertInvoke(x => x
                .WithAccessModifier(MemberAccessModifier.Public)
                .WithGetter()
                .WithSetter()
            );

        return builder.Syntax;
    }

    public IPropertyDeclarationBuilder WithGetter(Func<IAccessorDeclarationBuilder, IAccessorDeclarationBuilder>? accessorCallback = null)
    {
        var accessorSyntax = AccessorDeclarationBuilder.CreateSyntax(AccessorType.Get, accessorCallback);

        Syntax = Syntax.AddAccessorListAccessors(accessorSyntax);

        return this;
    }

    public IPropertyDeclarationBuilder WithSetter(Func<IAccessorDeclarationBuilder, IAccessorDeclarationBuilder>? accessorCallback = null)
    {
        var accessorSyntax = AccessorDeclarationBuilder.CreateSyntax(AccessorType.Set, accessorCallback);

        Syntax = Syntax.AddAccessorListAccessors(accessorSyntax);

        return this;
    }

    public IPropertyDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IPropertyDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IPropertyDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IPropertyDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}
