using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithWhereClauseBuilder<TBuilder>
{
    TBuilder WithWhereClause(Action<IExpressionBuilder> conditionCallback);
    TBuilder WithWhereClause(WhereClauseSyntax whereClauseSyntax);
}

public partial class WhereClauseBuilder
{
    public static WhereClauseSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback)
    {
        var whereKeywordToken = SyntaxFactory.Token(SyntaxKind.WhereKeyword);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        return SyntaxFactory.WhereClause(whereKeywordToken, conditionSyntax);
    }
}