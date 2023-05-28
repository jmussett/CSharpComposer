using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IDelegateDeclarationBuilder : IMemberDeclarationBuilder<IDelegateDeclarationBuilder>
{
    IDelegateDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null);
    IDelegateDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null);
    IDelegateDeclarationBuilder WithTypeParameter(string identifier, VarianceModifier varianceModifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null);
}

public sealed class DelegateDeclarationBuilder : IDelegateDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public DelegateDeclarationSyntax Syntax
    {
        get => (DelegateDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public DelegateDeclarationBuilder(DelegateDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Delegate);
    }

    public static DelegateDeclarationSyntax CreateSyntax(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IDelegateDeclarationBuilder, IDelegateDeclarationBuilder> delegateCallback
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var delegateSyntax = SyntaxFactory.DelegateDeclaration(typeSyntax, identifier);

        var builder = new DelegateDeclarationBuilder(delegateSyntax).AssertInvoke(delegateCallback);

        return builder.Syntax;
    }

    public IDelegateDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var syntax = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);

        Syntax = Syntax.AddTypeParameterListParameters(syntax.TypeParameter);

        if (syntax.ConstraintClause is not null)
        {
            Syntax = Syntax.AddConstraintClauses(syntax.ConstraintClause);
        }

        return this;
    }

    public IDelegateDeclarationBuilder WithTypeParameter(string identifier, VarianceModifier varianceModifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var syntax = TypeParameterBuilder.CreateSyntax(identifier, varianceModifier, typeParameterCallback);

        Syntax = Syntax.AddTypeParameterListParameters(syntax.TypeParameter);

        if (syntax.ConstraintClause is not null)
        {
            Syntax = Syntax.AddConstraintClauses(syntax.ConstraintClause);
        }

        return this;
    }

    public IDelegateDeclarationBuilder WithParameter(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public IDelegateDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IDelegateDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IDelegateDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IDelegateDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}
