﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArrayRankSpecifierBuilder
{
    IArrayRankSpecifierBuilder AddSizeExpression(Action<IExpressionBuilder> sizeCallback);
    IArrayRankSpecifierBuilder AddSizeExpression(ExpressionSyntax size);
}

internal partial class ArrayRankSpecifierBuilder : IArrayRankSpecifierBuilder
{
    public ArrayRankSpecifierSyntax Syntax { get; set; }

    public ArrayRankSpecifierBuilder(ArrayRankSpecifierSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArrayRankSpecifierSyntax CreateSyntax(Action<IArrayRankSpecifierBuilder>? arrayRankSpecifierCallback = null)
    {
        var openBracketTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBracketToken);
        var closeBracketTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBracketToken);
        var syntax = SyntaxFactory.ArrayRankSpecifier(openBracketTokenToken, default(SeparatedSyntaxList<ExpressionSyntax>), closeBracketTokenToken);
        var builder = new ArrayRankSpecifierBuilder(syntax);
        arrayRankSpecifierCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IArrayRankSpecifierBuilder AddSizeExpression(Action<IExpressionBuilder> sizeCallback)
    {
        var size = ExpressionBuilder.CreateSyntax(sizeCallback);
        Syntax = Syntax.AddSizes(size);
        return this;
    }

    public IArrayRankSpecifierBuilder AddSizeExpression(ExpressionSyntax size)
    {
        Syntax = Syntax.AddSizes(size);
        return this;
    }
}