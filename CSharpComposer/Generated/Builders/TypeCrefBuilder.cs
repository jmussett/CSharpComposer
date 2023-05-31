using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithTypeCref<TBuilder>
{
    TBuilder WithTypeCref(Action<ITypeBuilder> typeCallback);
    TBuilder WithTypeCref(TypeCrefSyntax typeCrefSyntax);
}

public interface IAddTypeCref<TBuilder>
{
    TBuilder AddTypeCref(Action<ITypeBuilder> typeCallback);
    TBuilder AddTypeCref(TypeCrefSyntax typeCrefSyntax);
}

public partial class TypeCrefBuilder
{
    public static TypeCrefSyntax CreateSyntax(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        return SyntaxFactory.TypeCref(typeSyntax);
    }
}