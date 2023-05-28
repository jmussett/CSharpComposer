using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IBaseTypeDeclarationBuilder : IBaseTypeDeclarationBuilder<IBaseTypeDeclarationBuilder>
{

}

public interface IBaseTypeDeclarationBuilder<TBuilder>
    where TBuilder : IBaseTypeDeclarationBuilder<TBuilder>
{
    TBuilder WithAccessModifier(TypeAccessModifier accessModifier);
    TBuilder WithStaticModifier();
    TBuilder WithPartialModifier();
    TBuilder WithUnsafeModifier();
    TBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
    TBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
}

public sealed class BaseTypeDeclarationBuilder : IBaseTypeDeclarationBuilder
{
    private readonly AttributeTargets _attributeTarget;

    public BaseTypeDeclarationSyntax Syntax { get; set; }

    public BaseTypeDeclarationBuilder(BaseTypeDeclarationSyntax syntax, AttributeTargets attributeTarget)
    {
        Syntax = syntax;
        _attributeTarget = attributeTarget;
    }

    // TODO: Create ???

    public IBaseTypeDeclarationBuilder WithAccessModifier(TypeAccessModifier accessModifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(accessModifier switch
        {
            TypeAccessModifier.Public => SyntaxKind.PublicKeyword,
            TypeAccessModifier.Internal => SyntaxKind.InternalKeyword,
            _ => throw new NotImplementedException($"TypeAccessModifier '{accessModifier}' is not supported"),
        }));

        return this;
    }

    public IBaseTypeDeclarationBuilder WithStaticModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

        return this;
    }

    public IBaseTypeDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        // TODO: Move to AttributeBuilder ??

        TypeValidator.ValidateAttributeForTarget(type, _attributeTarget);

        // TODO: generic arguments?

        var typeName = type.Name!;

        if (typeName.EndsWith("Attribute"))
        {
            typeName = typeName.Substring(0, typeName.Length - "Attribute".Length);
        }


        return WithAttribute(x => x.ParseName(typeName), attributeCallback);
    }

    //TODO: create name extension
    public IBaseTypeDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        var attributeSyntax = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);

        Syntax = Syntax.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(attributeSyntax)
            )
        );

        return this;
    }

    public IBaseTypeDeclarationBuilder WithUnsafeModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.UnsafeKeyword));

        return this;
    }

    public IBaseTypeDeclarationBuilder WithPartialModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        return this;
    }
}
