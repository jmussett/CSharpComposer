using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface ITypeParameterBuilder
{
    ITypeParameterBuilder WithClassConstraint(bool nullable = false);
    ITypeParameterBuilder WithNewConstraint();
    ITypeParameterBuilder WithNotNullConstraint();
    ITypeParameterBuilder WithStructConstraint();
    ITypeParameterBuilder WithTypeConstraint(Func<ITypeBuilder, ITypeBuilder> typeCallback);
    ITypeParameterBuilder WithUnmanagedConstraint();
}

public sealed class TypeParameterBuilder : ITypeParameterBuilder
{
    private readonly string _name;

    public TypeParameterSyntax TypeParameterSyntax { get; set; }
    public TypeParameterConstraintClauseSyntax? ConstraintClauseSyntax { get; set; }

    public TypeParameterBuilder(TypeParameterSyntax typeParameterSyntax)
    {
        TypeParameterSyntax = typeParameterSyntax;
        _name = TypeParameterSyntax.Identifier.Text;
    }

    public static (TypeParameterSyntax TypeParameter, TypeParameterConstraintClauseSyntax? ConstraintClause) CreateSyntax(
        string identifier,
        Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeParamSyntax = SyntaxFactory.TypeParameter(identifier);

        TypeParameterConstraintClauseSyntax? clauseSyntax = null;

        if (typeParameterCallback is not null)
        {
            var builder = new TypeParameterBuilder(typeParamSyntax).AssertInvoke(typeParameterCallback);

            typeParamSyntax = builder.TypeParameterSyntax;
            clauseSyntax = builder.ConstraintClauseSyntax;
        }

        return (typeParamSyntax, clauseSyntax);
    }

    public static (TypeParameterSyntax TypeParameter, TypeParameterConstraintClauseSyntax? ConstraintClause) CreateSyntax(
        string identifier,
        VarianceModifier varianceModifier,
        Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeParamSyntax = SyntaxFactory.TypeParameter(
            default,
            SyntaxFactory.Token(
                varianceModifier switch
                {
                    VarianceModifier.In => SyntaxKind.InKeyword,
                    VarianceModifier.Out => SyntaxKind.OutKeyword,
                    _ => throw new NotSupportedException($"VariantModifier '{varianceModifier}' not supported")
                }
            ),
            SyntaxFactory.Identifier(identifier)
        );

        TypeParameterConstraintClauseSyntax? clauseSyntax = null;

        if (typeParameterCallback is not null)
        {
            var builder = new TypeParameterBuilder(typeParamSyntax).AssertInvoke(typeParameterCallback);

            typeParamSyntax = builder.TypeParameterSyntax;
            clauseSyntax = builder.ConstraintClauseSyntax;
        }

        return (typeParamSyntax, clauseSyntax);
    }

    // TODO: TypeParameter attributes


    public ITypeParameterBuilder WithTypeConstraint(Func<ITypeBuilder, ITypeBuilder> typeCallback)
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.TypeConstraint(typeSyntax)
        );

        return this;
    }

    public ITypeParameterBuilder WithClassConstraint(bool nullable = false)
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.ClassOrStructConstraint(
                SyntaxKind.ClassConstraint,
                SyntaxFactory.Token(SyntaxKind.ClassKeyword),
                nullable ? SyntaxFactory.Token(SyntaxKind.QuestionToken) : default
            )
        );

        return this;
    }

    public ITypeParameterBuilder WithStructConstraint()
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.ClassOrStructConstraint(SyntaxKind.StructConstraint)
        );

        return this;
    }


    public ITypeParameterBuilder WithNewConstraint()
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.ConstructorConstraint()
        );

        return this;
    }

    public ITypeParameterBuilder WithUnmanagedConstraint()
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.TypeConstraint(SyntaxFactory.IdentifierName("unmanaged"))
        );

        return this;
    }

    public ITypeParameterBuilder WithNotNullConstraint()
    {
        ConstraintClauseSyntax ??= SyntaxFactory.TypeParameterConstraintClause(_name);

        ConstraintClauseSyntax = ConstraintClauseSyntax!.AddConstraints(
            SyntaxFactory.TypeConstraint(SyntaxFactory.IdentifierName("notnull"))
        );

        return this;
    }
}
