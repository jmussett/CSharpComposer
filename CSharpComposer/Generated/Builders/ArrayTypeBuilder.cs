using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayTypeBuilder
{
    IArrayTypeBuilder AddArrayRankSpecifier(Action<IArrayRankSpecifierBuilder>? arrayRankSpecifierCallback = null);
    IArrayTypeBuilder AddArrayRankSpecifier(ArrayRankSpecifierSyntax rankSpecifier);
}

public partial class ArrayTypeBuilder : IArrayTypeBuilder
{
    public ArrayTypeSyntax Syntax { get; set; }

    public ArrayTypeBuilder(ArrayTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArrayTypeSyntax CreateSyntax(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder>? arrayTypeCallback = null)
    {
        var elementTypeSyntax = TypeBuilder.CreateSyntax(elementTypeCallback);
        var syntax = SyntaxFactory.ArrayType(elementTypeSyntax, default(SyntaxList<ArrayRankSpecifierSyntax>));
        var builder = new ArrayTypeBuilder(syntax);
        arrayTypeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IArrayTypeBuilder AddArrayRankSpecifier(Action<IArrayRankSpecifierBuilder>? arrayRankSpecifierCallback = null)
    {
        var rankSpecifier = ArrayRankSpecifierBuilder.CreateSyntax(arrayRankSpecifierCallback);
        Syntax = Syntax.AddRankSpecifiers(rankSpecifier);
        return this;
    }

    public IArrayTypeBuilder AddArrayRankSpecifier(ArrayRankSpecifierSyntax rankSpecifier)
    {
        Syntax = Syntax.AddRankSpecifiers(rankSpecifier);
        return this;
    }
}