using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IQueryBodyBuilder
{
    IQueryBodyBuilder AddClause(Action<IQueryClauseBuilder> clauseCallback);
    IQueryBodyBuilder AddClause(QueryClauseSyntax clause);
    IQueryBodyBuilder WithContinuation(string identifier, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback);
    IQueryBodyBuilder WithContinuation(QueryContinuationSyntax continuation);
}

public interface IWithQueryBodyBuilder<TBuilder>
{
    TBuilder WithQueryBody(Action<ISelectOrGroupClauseBuilder> selectOrGroupCallback, Action<IQueryBodyBuilder> queryBodyCallback);
    TBuilder WithQueryBody(QueryBodySyntax queryBodySyntax);
}

public partial class QueryBodyBuilder : IQueryBodyBuilder
{
    public QueryBodySyntax Syntax { get; set; }

    public QueryBodyBuilder(QueryBodySyntax syntax)
    {
        Syntax = syntax;
    }

    public static QueryBodySyntax CreateSyntax(Action<ISelectOrGroupClauseBuilder> selectOrGroupCallback, Action<IQueryBodyBuilder> queryBodyCallback)
    {
        var selectOrGroupSyntax = SelectOrGroupClauseBuilder.CreateSyntax(selectOrGroupCallback);
        var syntax = SyntaxFactory.QueryBody(default(SyntaxList<QueryClauseSyntax>), selectOrGroupSyntax, default(QueryContinuationSyntax));
        var builder = new QueryBodyBuilder(syntax);
        queryBodyCallback(builder);
        return builder.Syntax;
    }

    public IQueryBodyBuilder AddClause(Action<IQueryClauseBuilder> clauseCallback)
    {
        var clause = QueryClauseBuilder.CreateSyntax(clauseCallback);
        Syntax = Syntax.AddClauses(clause);
        return this;
    }

    public IQueryBodyBuilder AddClause(QueryClauseSyntax clause)
    {
        Syntax = Syntax.AddClauses(clause);
        return this;
    }

    public IQueryBodyBuilder WithContinuation(string identifier, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        var continuationSyntax = QueryContinuationBuilder.CreateSyntax(identifier, bodySelectOrGroupCallback, bodyQueryBodyCallback);
        Syntax = Syntax.WithContinuation(continuationSyntax);
        return this;
    }

    public IQueryBodyBuilder WithContinuation(QueryContinuationSyntax continuation)
    {
        Syntax = Syntax.WithContinuation(continuation);
        return this;
    }
}