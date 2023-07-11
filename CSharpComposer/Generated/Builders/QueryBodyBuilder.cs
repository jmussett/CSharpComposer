using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IQueryBodyBuilder
{
    IQueryBodyBuilder AddQueryClause(Action<IQueryClauseBuilder> clauseCallback);
    IQueryBodyBuilder AddQueryClause(QueryClauseSyntax clause);
    IQueryBodyBuilder WithContinuation(string identifier, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback);
    IQueryBodyBuilder WithContinuation(QueryContinuationSyntax continuation);
}

public partial class QueryBodyBuilder : IQueryBodyBuilder
{
    public QueryBodySyntax Syntax { get; set; }

    public QueryBodyBuilder(QueryBodySyntax syntax)
    {
        Syntax = syntax;
    }

    public static QueryBodySyntax CreateSyntax(Action<ISelectOrGroupClauseBuilder> selectOrGroupCallback, Action<IQueryBodyBuilder>? queryBodyCallback = null)
    {
        var selectOrGroupSyntax = SelectOrGroupClauseBuilder.CreateSyntax(selectOrGroupCallback);
        var syntax = SyntaxFactory.QueryBody(default(SyntaxList<QueryClauseSyntax>), selectOrGroupSyntax, default(QueryContinuationSyntax));
        var builder = new QueryBodyBuilder(syntax);
        queryBodyCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IQueryBodyBuilder AddQueryClause(Action<IQueryClauseBuilder> clauseCallback)
    {
        var clause = QueryClauseBuilder.CreateSyntax(clauseCallback);
        Syntax = Syntax.AddClauses(clause);
        return this;
    }

    public IQueryBodyBuilder AddQueryClause(QueryClauseSyntax clause)
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