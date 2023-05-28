using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeParameterConstraintBuilder
{
    void AsConstructorConstraint();
    void AsClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder> classOrStructConstraintCallback);
    void AsTypeConstraint(Action<ITypeBuilder> typeCallback);
    void AsDefaultConstraint();
}

public interface IWithTypeParameterConstraintBuilder<TBuilder>
{
    TBuilder WithTypeParameterConstraint(Action<ITypeParameterConstraintBuilder> typeParameterConstraintCallback);
    TBuilder WithTypeParameterConstraint(TypeParameterConstraintSyntax typeParameterConstraintSyntax);
}

public partial class TypeParameterConstraintBuilder : ITypeParameterConstraintBuilder
{
    public TypeParameterConstraintSyntax? Syntax { get; set; }

    public static TypeParameterConstraintSyntax CreateSyntax(Action<ITypeParameterConstraintBuilder> callback)
    {
        var builder = new TypeParameterConstraintBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("TypeParameterConstraintSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsConstructorConstraint()
    {
        Syntax = ConstructorConstraintBuilder.CreateSyntax();
    }

    public void AsClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder> classOrStructConstraintCallback)
    {
        Syntax = ClassOrStructConstraintBuilder.CreateSyntax(kind, classOrStructConstraintCallback);
    }

    public void AsTypeConstraint(Action<ITypeBuilder> typeCallback)
    {
        Syntax = TypeConstraintBuilder.CreateSyntax(typeCallback);
    }

    public void AsDefaultConstraint()
    {
        Syntax = DefaultConstraintBuilder.CreateSyntax();
    }
}