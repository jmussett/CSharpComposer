using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayTypeBuilder
{
    IArrayTypeBuilder AddArrayRankSpecifier(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback);
    IArrayTypeBuilder AddArrayRankSpecifier(ArrayRankSpecifierSyntax rankSpecifier);
}

public interface IWithArrayType<TBuilder>
{
    TBuilder WithArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback);
    TBuilder WithArrayType(ArrayTypeSyntax arrayTypeSyntax);
}

public interface IAddArrayType<TBuilder>
{
    TBuilder AddArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback);
    TBuilder AddArrayType(ArrayTypeSyntax arrayTypeSyntax);
}

public partial class ArrayTypeBuilder : IArrayTypeBuilder
{
    public ArrayTypeSyntax Syntax { get; set; }

    public ArrayTypeBuilder(ArrayTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArrayTypeSyntax CreateSyntax(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback)
    {
        var elementTypeSyntax = TypeBuilder.CreateSyntax(elementTypeCallback);
        var syntax = SyntaxFactory.ArrayType(elementTypeSyntax, default(SyntaxList<ArrayRankSpecifierSyntax>));
        var builder = new ArrayTypeBuilder(syntax);
        arrayTypeCallback(builder);
        return builder.Syntax;
    }

    public IArrayTypeBuilder AddArrayRankSpecifier(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback)
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