using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchSectionBuilder
{
    ISwitchSectionBuilder AddLabel(Action<ISwitchLabelBuilder> labelCallback);
    ISwitchSectionBuilder AddLabel(SwitchLabelSyntax label);
    ISwitchSectionBuilder AddStatement(Action<IStatementBuilder> statementCallback);
    ISwitchSectionBuilder AddStatement(StatementSyntax statement);
}

public interface IWithSwitchSectionBuilder<TBuilder>
{
    TBuilder WithSwitchSection(Action<ISwitchSectionBuilder> switchSectionCallback);
    TBuilder WithSwitchSection(SwitchSectionSyntax switchSectionSyntax);
}

public partial class SwitchSectionBuilder : ISwitchSectionBuilder
{
    public SwitchSectionSyntax Syntax { get; set; }

    public SwitchSectionBuilder(SwitchSectionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SwitchSectionSyntax CreateSyntax(Action<ISwitchSectionBuilder> switchSectionCallback)
    {
        var syntax = SyntaxFactory.SwitchSection(default(SyntaxList<SwitchLabelSyntax>), default(SyntaxList<StatementSyntax>));
        var builder = new SwitchSectionBuilder(syntax);
        switchSectionCallback(builder);
        return builder.Syntax;
    }

    public ISwitchSectionBuilder AddLabel(Action<ISwitchLabelBuilder> labelCallback)
    {
        var label = SwitchLabelBuilder.CreateSyntax(labelCallback);
        Syntax = Syntax.AddLabels(label);
        return this;
    }

    public ISwitchSectionBuilder AddLabel(SwitchLabelSyntax label)
    {
        Syntax = Syntax.AddLabels(label);
        return this;
    }

    public ISwitchSectionBuilder AddStatement(Action<IStatementBuilder> statementCallback)
    {
        var statement = StatementBuilder.CreateSyntax(statementCallback);
        Syntax = Syntax.AddStatements(statement);
        return this;
    }

    public ISwitchSectionBuilder AddStatement(StatementSyntax statement)
    {
        Syntax = Syntax.AddStatements(statement);
        return this;
    }
}