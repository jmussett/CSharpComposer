using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithTypeConstraintBuilder<TBuilder>
{
    TBuilder WithTypeConstraint(Action<ITypeBuilder> typeCallback);
    TBuilder WithTypeConstraint(TypeConstraintSyntax typeConstraintSyntax);
}

public partial class TypeConstraintBuilder
{
    public static TypeConstraintSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.TypeConstraint(typeSyntax);
    }
}