using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IFieldDeclarationBuilder : IMemberDeclarationBuilder<IFieldDeclarationBuilder>
{
    IFieldDeclarationBuilder WithReadOnlyModifier();
}

public sealed class FieldDeclarationBuilder : IFieldDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public FieldDeclarationSyntax Syntax
    {
        get => (FieldDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public FieldDeclarationBuilder(FieldDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Field);
    }

    public static FieldDeclarationSyntax CreateSyntax(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IFieldDeclarationBuilder, IFieldDeclarationBuilder>? fieldCallback = null)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var fieldSyntax = SyntaxFactory.FieldDeclaration(
            SyntaxFactory
                .VariableDeclaration(typeSyntax)
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(identifier)
                        )
                    )
                )
        );

        var builder = new FieldDeclarationBuilder(fieldSyntax);

        builder = fieldCallback is not null
            ? builder.AssertInvoke(fieldCallback)
            : builder.AssertInvoke(x => x.WithAccessModifier(MemberAccessModifier.Private));

        return builder.Syntax;
    }

    public IFieldDeclarationBuilder WithReadOnlyModifier()
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));

        return this;
    }

    public IFieldDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IFieldDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IFieldDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IFieldDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}
