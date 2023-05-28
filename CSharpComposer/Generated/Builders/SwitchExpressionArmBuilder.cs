using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchExpressionArmBuilder : IWithWhenClauseBuilder<ISwitchExpressionArmBuilder>
{
    ISwitchExpressionArmBuilder WithWhenClause(Action<IExpressionBuilder> conditionCallback);
    ISwitchExpressionArmBuilder WithWhenClause(WhenClauseSyntax whenClause);
}

public interface IWithSwitchExpressionArmBuilder<TBuilder>
{
    TBuilder WithSwitchExpressionArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder> switchExpressionArmCallback);
    TBuilder WithSwitchExpressionArm(SwitchExpressionArmSyntax switchExpressionArmSyntax);
}

public partial class SwitchExpressionArmBuilder : ISwitchExpressionArmBuilder
{
    public SwitchExpressionArmSyntax Syntax { get; set; }

    public SwitchExpressionArmBuilder(SwitchExpressionArmSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SwitchExpressionArmSyntax CreateSyntax(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder> switchExpressionArmCallback)
    {
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        var equalsGreaterThanTokenToken = SyntaxFactory.Token(SyntaxKind.EqualsGreaterThanToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.SwitchExpressionArm(patternSyntax, default(WhenClauseSyntax), equalsGreaterThanTokenToken, expressionSyntax);
        var builder = new SwitchExpressionArmBuilder(syntax);
        switchExpressionArmCallback(builder);
        return builder.Syntax;
    }

    public ISwitchExpressionArmBuilder WithWhenClause(Action<IExpressionBuilder> conditionCallback)
    {
        var whenClauseSyntax = WhenClauseBuilder.CreateSyntax(conditionCallback);
        Syntax = Syntax.WithWhenClause(whenClauseSyntax);
        return this;
    }

    public ISwitchExpressionArmBuilder WithWhenClause(WhenClauseSyntax whenClause)
    {
        Syntax = Syntax.WithWhenClause(whenClause);
        return this;
    }
}