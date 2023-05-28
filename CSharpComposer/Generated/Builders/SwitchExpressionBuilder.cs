using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchExpressionBuilder
{
    ISwitchExpressionBuilder AddArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder> switchExpressionArmCallback);
    ISwitchExpressionBuilder AddArm(SwitchExpressionArmSyntax arm);
}

public interface IWithSwitchExpressionBuilder<TBuilder>
{
    TBuilder WithSwitchExpression(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder> switchExpressionCallback);
    TBuilder WithSwitchExpression(SwitchExpressionSyntax switchExpressionSyntax);
}

public partial class SwitchExpressionBuilder : ISwitchExpressionBuilder
{
    public SwitchExpressionSyntax Syntax { get; set; }

    public SwitchExpressionBuilder(SwitchExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SwitchExpressionSyntax CreateSyntax(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder> switchExpressionCallback)
    {
        var governingExpressionSyntax = ExpressionBuilder.CreateSyntax(governingExpressionCallback);
        var switchKeywordToken = SyntaxFactory.Token(SyntaxKind.SwitchKeyword);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.SwitchExpression(governingExpressionSyntax, switchKeywordToken, openBraceTokenToken, default(SeparatedSyntaxList<SwitchExpressionArmSyntax>), closeBraceTokenToken);
        var builder = new SwitchExpressionBuilder(syntax);
        switchExpressionCallback(builder);
        return builder.Syntax;
    }

    public ISwitchExpressionBuilder AddArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder> switchExpressionArmCallback)
    {
        var arm = SwitchExpressionArmBuilder.CreateSyntax(patternCallback, expressionCallback, switchExpressionArmCallback);
        Syntax = Syntax.AddArms(arm);
        return this;
    }

    public ISwitchExpressionBuilder AddArm(SwitchExpressionArmSyntax arm)
    {
        Syntax = Syntax.AddArms(arm);
        return this;
    }
}