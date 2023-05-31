using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithEqualsValueClause<TBuilder>
{
    TBuilder WithEqualsValueClause(Action<IExpressionBuilder> valueCallback);
    TBuilder WithEqualsValueClause(EqualsValueClauseSyntax equalsValueClauseSyntax);
}

public interface IAddEqualsValueClause<TBuilder>
{
    TBuilder AddEqualsValueClause(Action<IExpressionBuilder> valueCallback);
    TBuilder AddEqualsValueClause(EqualsValueClauseSyntax equalsValueClauseSyntax);
}

public partial class EqualsValueClauseBuilder
{
    public static EqualsValueClauseSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        return SyntaxFactory.EqualsValueClause(equalsTokenToken, valueSyntax);
    }
}