using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchLabelBuilder
{
    void FromSyntax(SwitchLabelSyntax syntax);
    void AsCasePatternSwitchLabel(Action<IPatternBuilder> patternCallback, Action<ICasePatternSwitchLabelBuilder>? casePatternSwitchLabelCallback = null);
    void AsCaseSwitchLabel(Action<IExpressionBuilder> valueCallback);
    void AsDefaultSwitchLabel();
}

internal partial class SwitchLabelBuilder : ISwitchLabelBuilder
{
    public SwitchLabelSyntax? Syntax { get; set; }

    public static SwitchLabelSyntax CreateSyntax(Action<ISwitchLabelBuilder> callback)
    {
        var builder = new SwitchLabelBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("SwitchLabelSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(SwitchLabelSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsCasePatternSwitchLabel(Action<IPatternBuilder> patternCallback, Action<ICasePatternSwitchLabelBuilder>? casePatternSwitchLabelCallback = null)
    {
        Syntax = CasePatternSwitchLabelBuilder.CreateSyntax(patternCallback, casePatternSwitchLabelCallback);
    }

    public void AsCaseSwitchLabel(Action<IExpressionBuilder> valueCallback)
    {
        Syntax = CaseSwitchLabelBuilder.CreateSyntax(valueCallback);
    }

    public void AsDefaultSwitchLabel()
    {
        Syntax = DefaultSwitchLabelBuilder.CreateSyntax();
    }
}