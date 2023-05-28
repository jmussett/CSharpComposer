using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IMemberDeclarationBuilder : IMemberDeclarationBuilder<IMemberDeclarationBuilder>
{

}

public interface IMemberDeclarationBuilder<TBuilder> where TBuilder : IMemberDeclarationBuilder<TBuilder>
{
    TBuilder WithAccessModifier(MemberAccessModifier accessModifier);
    TBuilder WithStaticModifier();
    TBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
    TBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
}

public sealed class MemberDeclarationBuilder : IMemberDeclarationBuilder
{
    private readonly AttributeTargets _attributeTarget;

    public MemberDeclarationSyntax Syntax { get; set; }

    public MemberDeclarationBuilder(MemberDeclarationSyntax syntax, AttributeTargets attributeTarget)
    {
        Syntax = syntax;
        _attributeTarget = attributeTarget;
    }

    public IMemberDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(accessModifier switch
        {
            MemberAccessModifier.Public => SyntaxKind.PublicKeyword,
            MemberAccessModifier.Internal => SyntaxKind.InternalKeyword,
            MemberAccessModifier.Private => SyntaxKind.PrivateKeyword,
            MemberAccessModifier.Protected => SyntaxKind.ProtectedKeyword,
            _ => throw new NotImplementedException($"MemberAccessModifier '{accessModifier}' is not supported"),
        }));

        return this;
    }

    public IMemberDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        TypeValidator.ValidateAttributeForTarget(type, _attributeTarget);

        // TODO: generic arguments?

        return WithAttribute(x => x.ParseName(type.FullName!), attributeCallback);
    }

    public IMemberDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        var attributeSyntax = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);

        Syntax = Syntax.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(attributeSyntax)
        )
        );

        return this;
    }

    public IMemberDeclarationBuilder WithStaticModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword));

        return this;
    }
}

public static class MemberBuilderExtensions
{
    public static IMemberDeclarationBuilder WithAttribute<T>(
        this IMemberDeclarationBuilder builder,
        Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null
    ) where T : Attribute
    {
        return builder.WithAttribute(typeof(T), attributeCallback);
    }

    public static IMemberDeclarationBuilder WithAttribute(
        this IMemberDeclarationBuilder builder,
        string typeName,
        Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null
    )
    {
        return builder.WithAttribute(x => x.ParseName(typeName), attributeCallback);
    }
}