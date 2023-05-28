using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithSelectClauseBuilder<TBuilder>
{
    TBuilder WithSelectClause(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithSelectClause(SelectClauseSyntax selectClauseSyntax);
}

public partial class SelectClauseBuilder
{
    public static SelectClauseSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var selectKeywordToken = SyntaxFactory.Token(SyntaxKind.SelectKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.SelectClause(selectKeywordToken, expressionSyntax);
    }
}