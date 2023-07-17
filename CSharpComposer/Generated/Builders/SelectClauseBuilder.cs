using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class SelectClauseBuilder
{
    public static SelectClauseSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var selectKeywordToken = SyntaxFactory.Token(SyntaxKind.SelectKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        return SyntaxFactory.SelectClause(selectKeywordToken, expressionSyntax);
    }
}