﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IOrderingBuilder
{
    IOrderingBuilder WithAscendingOrDescendingKeyword(AscendingOrDescendingKeyword ascendingOrDescendingKeyword);
}

public interface IAddOrdering<TBuilder>
{
    TBuilder AddOrdering(OrderingSyntax orderingSyntax);
    TBuilder AddOrdering(OrderingKind kind, Action<IExpressionBuilder> expressionCallback, Action<IOrderingBuilder>? orderingCallback = null);
}

internal partial class OrderingBuilder : IOrderingBuilder
{
    public OrderingSyntax Syntax { get; set; }

    public OrderingBuilder(OrderingSyntax syntax)
    {
        Syntax = syntax;
    }

    public static OrderingSyntax CreateSyntax(OrderingKind kind, Action<IExpressionBuilder> expressionCallback, Action<IOrderingBuilder>? orderingCallback = null)
    {
        var syntaxKind = kind switch
        {
            OrderingKind.AscendingOrdering => SyntaxKind.AscendingOrdering,
            OrderingKind.DescendingOrdering => SyntaxKind.DescendingOrdering,
            _ => throw new NotSupportedException()
        };
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.Ordering(syntaxKind, expressionSyntax, default(SyntaxToken));
        var builder = new OrderingBuilder(syntax);
        orderingCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IOrderingBuilder WithAscendingOrDescendingKeyword(AscendingOrDescendingKeyword ascendingOrDescendingKeyword)
    {
        Syntax = Syntax.WithAscendingOrDescendingKeyword(SyntaxFactory.Token(ascendingOrDescendingKeyword switch
        {
            AscendingOrDescendingKeyword.AscendingKeyword => SyntaxKind.AscendingKeyword,
            AscendingOrDescendingKeyword.DescendingKeyword => SyntaxKind.DescendingKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }
}