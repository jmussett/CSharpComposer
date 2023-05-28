using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithConditionalExpressionBuilder<TBuilder>
{
    TBuilder WithConditionalExpression(Action<IExpressionBuilder> conditionCallback, Action<IExpressionBuilder> whenTrueCallback, Action<IExpressionBuilder> whenFalseCallback);
    TBuilder WithConditionalExpression(ConditionalExpressionSyntax conditionalExpressionSyntax);
}

public partial class ConditionalExpressionBuilder
{
    public static ConditionalExpressionSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback, Action<IExpressionBuilder> whenTrueCallback, Action<IExpressionBuilder> whenFalseCallback)
    {
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var questionTokenToken = SyntaxFactory.Token(SyntaxKind.QuestionToken);
        var whenTrueSyntax = ExpressionBuilder.CreateSyntax(whenTrueCallback);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var whenFalseSyntax = ExpressionBuilder.CreateSyntax(whenFalseCallback);
        return SyntaxFactory.ConditionalExpression(conditionSyntax, questionTokenToken, whenTrueSyntax, colonTokenToken, whenFalseSyntax);
    }
}