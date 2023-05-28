using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayTypeBuilder
{
    IArrayTypeBuilder AddRankSpecifier(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback);
    IArrayTypeBuilder AddRankSpecifier(ArrayRankSpecifierSyntax rankSpecifier);
}

public interface IWithArrayTypeBuilder<TBuilder>
{
    TBuilder WithArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback);
    TBuilder WithArrayType(ArrayTypeSyntax arrayTypeSyntax);
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

    public IArrayTypeBuilder AddRankSpecifier(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback)
    {
        var rankSpecifier = ArrayRankSpecifierBuilder.CreateSyntax(arrayRankSpecifierCallback);
        Syntax = Syntax.AddRankSpecifiers(rankSpecifier);
        return this;
    }

    public IArrayTypeBuilder AddRankSpecifier(ArrayRankSpecifierSyntax rankSpecifier)
    {
        Syntax = Syntax.AddRankSpecifiers(rankSpecifier);
        return this;
    }
}