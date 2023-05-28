using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IOrderByClauseBuilder
{
    IOrderByClauseBuilder AddOrdering(OrderingKind kind, Action<IExpressionBuilder> expressionCallback, Action<IOrderingBuilder> orderingCallback);
    IOrderByClauseBuilder AddOrdering(OrderingSyntax ordering);
}

public interface IWithOrderByClauseBuilder<TBuilder>
{
    TBuilder WithOrderByClause(Action<IOrderByClauseBuilder> orderByClauseCallback);
    TBuilder WithOrderByClause(OrderByClauseSyntax orderByClauseSyntax);
}

public partial class OrderByClauseBuilder : IOrderByClauseBuilder
{
    public OrderByClauseSyntax Syntax { get; set; }

    public OrderByClauseBuilder(OrderByClauseSyntax syntax)
    {
        Syntax = syntax;
    }

    public static OrderByClauseSyntax CreateSyntax(Action<IOrderByClauseBuilder> orderByClauseCallback)
    {
        var orderByKeywordToken = SyntaxFactory.Token(SyntaxKind.OrderByKeyword);
        var syntax = SyntaxFactory.OrderByClause(orderByKeywordToken, default(SeparatedSyntaxList<OrderingSyntax>));
        var builder = new OrderByClauseBuilder(syntax);
        orderByClauseCallback(builder);
        return builder.Syntax;
    }

    public IOrderByClauseBuilder AddOrdering(OrderingKind kind, Action<IExpressionBuilder> expressionCallback, Action<IOrderingBuilder> orderingCallback)
    {
        var ordering = OrderingBuilder.CreateSyntax(kind, expressionCallback, orderingCallback);
        Syntax = Syntax.AddOrderings(ordering);
        return this;
    }

    public IOrderByClauseBuilder AddOrdering(OrderingSyntax ordering)
    {
        Syntax = Syntax.AddOrderings(ordering);
        return this;
    }
}