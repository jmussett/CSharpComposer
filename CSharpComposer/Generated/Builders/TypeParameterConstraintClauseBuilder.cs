using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeParameterConstraintClauseBuilder
{
    ITypeParameterConstraintClauseBuilder AddConstraint(Action<ITypeParameterConstraintBuilder> constraintCallback);
    ITypeParameterConstraintClauseBuilder AddConstraint(TypeParameterConstraintSyntax constraint);
}

public interface IWithTypeParameterConstraintClauseBuilder<TBuilder>
{
    TBuilder WithTypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback);
    TBuilder WithTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax typeParameterConstraintClauseSyntax);
}

public partial class TypeParameterConstraintClauseBuilder : ITypeParameterConstraintClauseBuilder
{
    public TypeParameterConstraintClauseSyntax Syntax { get; set; }

    public TypeParameterConstraintClauseBuilder(TypeParameterConstraintClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TypeParameterConstraintClauseSyntax CreateSyntax(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var whereKeywordToken = SyntaxFactory.Token(SyntaxKind.WhereKeyword);
        var nameSyntax = IdentifierNameBuilder.CreateSyntax(nameIdentifier);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var syntax = SyntaxFactory.TypeParameterConstraintClause(whereKeywordToken, nameSyntax, colonTokenToken, default(SeparatedSyntaxList<TypeParameterConstraintSyntax>));
        var builder = new TypeParameterConstraintClauseBuilder(syntax);
        typeParameterConstraintClauseCallback(builder);
        return builder.Syntax;
    }

    public ITypeParameterConstraintClauseBuilder AddConstraint(Action<ITypeParameterConstraintBuilder> constraintCallback)
    {
        var constraint = TypeParameterConstraintBuilder.CreateSyntax(constraintCallback);
        Syntax = Syntax.AddConstraints(constraint);
        return this;
    }

    public ITypeParameterConstraintClauseBuilder AddConstraint(TypeParameterConstraintSyntax constraint)
    {
        Syntax = Syntax.AddConstraints(constraint);
        return this;
    }
}