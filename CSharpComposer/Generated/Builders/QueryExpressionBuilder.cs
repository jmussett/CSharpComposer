using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithQueryExpressionBuilder<TBuilder>
{
    TBuilder WithQueryExpression(string fromClauseIdentifier, Action<IExpressionBuilder> fromClauseExpressionCallback, Action<IFromClauseBuilder> fromClauseFromClauseCallback, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback);
    TBuilder WithQueryExpression(QueryExpressionSyntax queryExpressionSyntax);
}

public partial class QueryExpressionBuilder
{
    public static QueryExpressionSyntax CreateSyntax(string fromClauseIdentifier, Action<IExpressionBuilder> fromClauseExpressionCallback, Action<IFromClauseBuilder> fromClauseFromClauseCallback, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        var fromClauseSyntax = FromClauseBuilder.CreateSyntax(fromClauseIdentifier, fromClauseExpressionCallback, fromClauseFromClauseCallback);
        var bodySyntax = QueryBodyBuilder.CreateSyntax(bodySelectOrGroupCallback, bodyQueryBodyCallback);
        return SyntaxFactory.QueryExpression(fromClauseSyntax, bodySyntax);
    }
}