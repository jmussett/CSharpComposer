﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeParameterBuilder : IAddAttribute<ITypeParameterBuilder>
{
    ITypeParameterBuilder WithVarianceKeyword(VarianceKeyword varianceKeyword);
}

public interface IAddTypeParameter<TBuilder>
{
    TBuilder AddTypeParameter(TypeParameterSyntax typeParameterSyntax);
    TBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder>? typeParameterCallback = null);
}

internal partial class TypeParameterBuilder : ITypeParameterBuilder
{
    public TypeParameterSyntax Syntax { get; set; }

    public TypeParameterBuilder(TypeParameterSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TypeParameterSyntax CreateSyntax(string identifier, Action<ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.TypeParameter(default(SyntaxList<AttributeListSyntax>), default(SyntaxToken), identifierToken);
        var builder = new TypeParameterBuilder(syntax);
        typeParameterCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ITypeParameterBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ITypeParameterBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ITypeParameterBuilder WithVarianceKeyword(VarianceKeyword varianceKeyword)
    {
        Syntax = Syntax.WithVarianceKeyword(SyntaxFactory.Token(varianceKeyword switch
        {
            VarianceKeyword.InKeyword => SyntaxKind.InKeyword,
            VarianceKeyword.OutKeyword => SyntaxKind.OutKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }
}