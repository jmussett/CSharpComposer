using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class InterpolationAlignmentClauseBuilder
{
    public static InterpolationAlignmentClauseSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var commaTokenToken = SyntaxFactory.Token(SyntaxKind.CommaToken);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        return SyntaxFactory.InterpolationAlignmentClause(commaTokenToken, valueSyntax);
    }
}