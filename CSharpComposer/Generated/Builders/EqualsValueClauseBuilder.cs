using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class EqualsValueClauseBuilder
{
    public static EqualsValueClauseSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var equalsTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsToken);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        return SyntaxFactory.EqualsValueClause(equalsTokenToken, valueSyntax);
    }
}