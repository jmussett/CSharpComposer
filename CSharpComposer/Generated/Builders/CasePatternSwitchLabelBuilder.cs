using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICasePatternSwitchLabelBuilder : IWithWhenClause<ICasePatternSwitchLabelBuilder>
{
}

public partial class CasePatternSwitchLabelBuilder : ICasePatternSwitchLabelBuilder
{
    public CasePatternSwitchLabelSyntax Syntax { get; set; }

    public CasePatternSwitchLabelBuilder(CasePatternSwitchLabelSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CasePatternSwitchLabelSyntax CreateSyntax(Action<IPatternBuilder> patternCallback, Action<ICasePatternSwitchLabelBuilder>? casePatternSwitchLabelCallback = null)
    {
        var keywordToken = SyntaxFactory.Token(SyntaxKind.CaseKeyword);
        var patternSyntax = PatternBuilder.CreateSyntax(patternCallback);
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var syntax = SyntaxFactory.CasePatternSwitchLabel(keywordToken, patternSyntax, default(WhenClauseSyntax), colonTokenToken);
        var builder = new CasePatternSwitchLabelBuilder(syntax);
        casePatternSwitchLabelCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ICasePatternSwitchLabelBuilder WithWhenClause(Action<IExpressionBuilder> conditionCallback)
    {
        var whenClauseSyntax = WhenClauseBuilder.CreateSyntax(conditionCallback);
        Syntax = Syntax.WithWhenClause(whenClauseSyntax);
        return this;
    }

    public ICasePatternSwitchLabelBuilder WithWhenClause(WhenClauseSyntax whenClause)
    {
        Syntax = Syntax.WithWhenClause(whenClause);
        return this;
    }
}