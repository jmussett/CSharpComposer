﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeParameterConstraintBuilder
{
    void FromSyntax(TypeParameterConstraintSyntax syntax);
    void AsConstructorConstraint();
    void AsClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder>? classOrStructConstraintCallback = null);
    void AsTypeConstraint(Action<ITypeBuilder> typeCallback);
    void AsDefaultConstraint();
}

internal partial class TypeParameterConstraintBuilder : ITypeParameterConstraintBuilder
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

    public void FromSyntax(TypeParameterConstraintSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsConstructorConstraint()
    {
        Syntax = ConstructorConstraintBuilder.CreateSyntax();
    }

    public void AsClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder>? classOrStructConstraintCallback = null)
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