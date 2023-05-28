using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayRankSpecifierBuilder
{
    IArrayRankSpecifierBuilder AddSize(Action<IExpressionBuilder> sizeCallback);
    IArrayRankSpecifierBuilder AddSize(ExpressionSyntax size);
}

public interface IWithArrayRankSpecifierBuilder<TBuilder>
{
    TBuilder WithArrayRankSpecifier(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback);
    TBuilder WithArrayRankSpecifier(ArrayRankSpecifierSyntax arrayRankSpecifierSyntax);
}

public partial class ArrayRankSpecifierBuilder : IArrayRankSpecifierBuilder
{
    public ArrayRankSpecifierSyntax Syntax { get; set; }

    public ArrayRankSpecifierBuilder(ArrayRankSpecifierSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArrayRankSpecifierSyntax CreateSyntax(Action<IArrayRankSpecifierBuilder> arrayRankSpecifierCallback)
    {
        var openBracketTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBracketToken);
        var closeBracketTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBracketToken);
        var syntax = SyntaxFactory.ArrayRankSpecifier(openBracketTokenToken, default(SeparatedSyntaxList<ExpressionSyntax>), closeBracketTokenToken);
        var builder = new ArrayRankSpecifierBuilder(syntax);
        arrayRankSpecifierCallback(builder);
        return builder.Syntax;
    }

    public IArrayRankSpecifierBuilder AddSize(Action<IExpressionBuilder> sizeCallback)
    {
        var size = ExpressionBuilder.CreateSyntax(sizeCallback);
        Syntax = Syntax.AddSizes(size);
        return this;
    }

    public IArrayRankSpecifierBuilder AddSize(ExpressionSyntax size)
    {
        Syntax = Syntax.AddSizes(size);
        return this;
    }
}