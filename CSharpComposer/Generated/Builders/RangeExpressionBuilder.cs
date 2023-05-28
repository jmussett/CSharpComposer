using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IRangeExpressionBuilder
{
    IRangeExpressionBuilder WithLeftOperand(Action<IExpressionBuilder> leftOperandCallback);
    IRangeExpressionBuilder WithLeftOperand(ExpressionSyntax leftOperand);
    IRangeExpressionBuilder WithRightOperand(Action<IExpressionBuilder> rightOperandCallback);
    IRangeExpressionBuilder WithRightOperand(ExpressionSyntax rightOperand);
}

public interface IWithRangeExpressionBuilder<TBuilder>
{
    TBuilder WithRangeExpression(Action<IRangeExpressionBuilder> rangeExpressionCallback);
    TBuilder WithRangeExpression(RangeExpressionSyntax rangeExpressionSyntax);
}

public partial class RangeExpressionBuilder : IRangeExpressionBuilder
{
    public RangeExpressionSyntax Syntax { get; set; }

    public RangeExpressionBuilder(RangeExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static RangeExpressionSyntax CreateSyntax(Action<IRangeExpressionBuilder> rangeExpressionCallback)
    {
        var operatorTokenToken = SyntaxFactory.Token(SyntaxKind.DotDotToken);
        var syntax = SyntaxFactory.RangeExpression(default(ExpressionSyntax), operatorTokenToken, default(ExpressionSyntax));
        var builder = new RangeExpressionBuilder(syntax);
        rangeExpressionCallback(builder);
        return builder.Syntax;
    }

    public IRangeExpressionBuilder WithLeftOperand(Action<IExpressionBuilder> leftOperandCallback)
    {
        var leftOperandSyntax = ExpressionBuilder.CreateSyntax(leftOperandCallback);
        Syntax = Syntax.WithLeftOperand(leftOperandSyntax);
        return this;
    }

    public IRangeExpressionBuilder WithLeftOperand(ExpressionSyntax leftOperand)
    {
        Syntax = Syntax.WithLeftOperand(leftOperand);
        return this;
    }

    public IRangeExpressionBuilder WithRightOperand(Action<IExpressionBuilder> rightOperandCallback)
    {
        var rightOperandSyntax = ExpressionBuilder.CreateSyntax(rightOperandCallback);
        Syntax = Syntax.WithRightOperand(rightOperandSyntax);
        return this;
    }

    public IRangeExpressionBuilder WithRightOperand(ExpressionSyntax rightOperand)
    {
        Syntax = Syntax.WithRightOperand(rightOperand);
        return this;
    }
}