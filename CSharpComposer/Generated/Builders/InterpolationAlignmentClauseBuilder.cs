using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithInterpolationAlignmentClauseBuilder<TBuilder>
{
    TBuilder WithInterpolationAlignmentClause(Action<IExpressionBuilder> valueCallback);
    TBuilder WithInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax interpolationAlignmentClauseSyntax);
}

public partial class InterpolationAlignmentClauseBuilder
{
    public static InterpolationAlignmentClauseSyntax CreateSyntax(Action<IExpressionBuilder> valueCallback)
    {
        var commaTokenToken = SyntaxFactory.Token(SyntaxKind.CommaToken);
        var valueSyntax = ExpressionBuilder.CreateSyntax(valueCallback);
        return SyntaxFactory.InterpolationAlignmentClause(commaTokenToken, valueSyntax);
    }
}