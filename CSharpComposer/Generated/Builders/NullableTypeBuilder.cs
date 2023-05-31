﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithNullableType<TBuilder>
{
    TBuilder WithNullableType(Action<ITypeBuilder> elementTypeCallback);
    TBuilder WithNullableType(NullableTypeSyntax nullableTypeSyntax);
}

public interface IAddNullableType<TBuilder>
{
    TBuilder AddNullableType(Action<ITypeBuilder> elementTypeCallback);
    TBuilder AddNullableType(NullableTypeSyntax nullableTypeSyntax);
}

public partial class NullableTypeBuilder
{
    public static NullableTypeSyntax CreateSyntax(Action<ITypeBuilder> elementTypeCallback)
    {
        var elementTypeSyntax = TypeBuilder.CreateSyntax(elementTypeCallback);
        var questionTokenToken = SyntaxFactory.Token(SyntaxKind.QuestionToken);
        return SyntaxFactory.NullableType(elementTypeSyntax, questionTokenToken);
    }
}