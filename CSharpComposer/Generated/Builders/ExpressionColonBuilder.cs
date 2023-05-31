﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithExpressionColon<TBuilder>
{
    TBuilder WithExpressionColon(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithExpressionColon(ExpressionColonSyntax expressionColonSyntax);
}

public interface IAddExpressionColon<TBuilder>
{
    TBuilder AddExpressionColon(Action<IExpressionBuilder> expressionCallback);
    TBuilder AddExpressionColon(ExpressionColonSyntax expressionColonSyntax);
}

public partial class ExpressionColonBuilder
{
    public static ExpressionColonSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        return SyntaxFactory.ExpressionColon(expressionSyntax, colonTokenToken);
    }
}