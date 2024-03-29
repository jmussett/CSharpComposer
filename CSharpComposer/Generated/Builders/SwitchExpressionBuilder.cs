﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchExpressionBuilder
{
    ISwitchExpressionBuilder AddSwitchExpressionArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder>? switchExpressionArmCallback = null);
    ISwitchExpressionBuilder AddSwitchExpressionArm(SwitchExpressionArmSyntax arm);
}

internal partial class SwitchExpressionBuilder : ISwitchExpressionBuilder
{
    public SwitchExpressionSyntax Syntax { get; set; }

    public SwitchExpressionBuilder(SwitchExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SwitchExpressionSyntax CreateSyntax(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder>? switchExpressionCallback = null)
    {
        var governingExpressionSyntax = ExpressionBuilder.CreateSyntax(governingExpressionCallback);
        var switchKeywordToken = SyntaxFactory.Token(SyntaxKind.SwitchKeyword);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.SwitchExpression(governingExpressionSyntax, switchKeywordToken, openBraceTokenToken, default(SeparatedSyntaxList<SwitchExpressionArmSyntax>), closeBraceTokenToken);
        var builder = new SwitchExpressionBuilder(syntax);
        switchExpressionCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ISwitchExpressionBuilder AddSwitchExpressionArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder>? switchExpressionArmCallback = null)
    {
        var arm = SwitchExpressionArmBuilder.CreateSyntax(patternCallback, expressionCallback, switchExpressionArmCallback);
        Syntax = Syntax.AddArms(arm);
        return this;
    }

    public ISwitchExpressionBuilder AddSwitchExpressionArm(SwitchExpressionArmSyntax arm)
    {
        Syntax = Syntax.AddArms(arm);
        return this;
    }
}