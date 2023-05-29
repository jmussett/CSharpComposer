using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IInterpolationBuilder : IWithInterpolationAlignmentClause<IInterpolationBuilder>, IWithInterpolationFormatClause<IInterpolationBuilder>
{
    IInterpolationBuilder WithInterpolationAlignmentClause(Action<IExpressionBuilder> valueCallback);
    IInterpolationBuilder WithInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax alignmentClause);
    IInterpolationBuilder WithInterpolationFormatClause();
    IInterpolationBuilder WithInterpolationFormatClause(InterpolationFormatClauseSyntax formatClause);
}

public interface IWithInterpolation<TBuilder>
{
    TBuilder WithInterpolation(Action<IExpressionBuilder> expressionCallback, Action<IInterpolationBuilder> interpolationCallback);
    TBuilder WithInterpolation(InterpolationSyntax interpolationSyntax);
}

public partial class InterpolationBuilder : IInterpolationBuilder
{
    public InterpolationSyntax Syntax { get; set; }

    public InterpolationBuilder(InterpolationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static InterpolationSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IInterpolationBuilder> interpolationCallback)
    {
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.Interpolation(openBraceTokenToken, expressionSyntax, default(InterpolationAlignmentClauseSyntax), default(InterpolationFormatClauseSyntax), closeBraceTokenToken);
        var builder = new InterpolationBuilder(syntax);
        interpolationCallback(builder);
        return builder.Syntax;
    }

    public IInterpolationBuilder WithInterpolationAlignmentClause(Action<IExpressionBuilder> valueCallback)
    {
        var alignmentClauseSyntax = InterpolationAlignmentClauseBuilder.CreateSyntax(valueCallback);
        Syntax = Syntax.WithAlignmentClause(alignmentClauseSyntax);
        return this;
    }

    public IInterpolationBuilder WithInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax alignmentClause)
    {
        Syntax = Syntax.WithAlignmentClause(alignmentClause);
        return this;
    }

    public IInterpolationBuilder WithInterpolationFormatClause()
    {
        var formatClauseSyntax = InterpolationFormatClauseBuilder.CreateSyntax();
        Syntax = Syntax.WithFormatClause(formatClauseSyntax);
        return this;
    }

    public IInterpolationBuilder WithInterpolationFormatClause(InterpolationFormatClauseSyntax formatClause)
    {
        Syntax = Syntax.WithFormatClause(formatClause);
        return this;
    }
}