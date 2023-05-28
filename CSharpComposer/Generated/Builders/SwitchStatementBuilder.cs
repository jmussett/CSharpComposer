using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ISwitchStatementBuilder : IStatementBuilder<ISwitchStatementBuilder>
{
    ISwitchStatementBuilder WithOpenParenToken();
    ISwitchStatementBuilder WithCloseParenToken();
    ISwitchStatementBuilder AddSection(Action<ISwitchSectionBuilder> switchSectionCallback);
    ISwitchStatementBuilder AddSection(SwitchSectionSyntax section);
}

public interface IWithSwitchStatementBuilder<TBuilder>
{
    TBuilder WithSwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder> switchStatementCallback);
    TBuilder WithSwitchStatement(SwitchStatementSyntax switchStatementSyntax);
}

public partial class SwitchStatementBuilder : ISwitchStatementBuilder
{
    public SwitchStatementSyntax Syntax { get; set; }

    public SwitchStatementBuilder(SwitchStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static SwitchStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder> switchStatementCallback)
    {
        var switchKeywordToken = SyntaxFactory.Token(SyntaxKind.SwitchKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var openBraceTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBraceToken);
        var closeBraceTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBraceToken);
        var syntax = SyntaxFactory.SwitchStatement(default(SyntaxList<AttributeListSyntax>), switchKeywordToken, default(SyntaxToken), expressionSyntax, default(SyntaxToken), openBraceTokenToken, default(SyntaxList<SwitchSectionSyntax>), closeBraceTokenToken);
        var builder = new SwitchStatementBuilder(syntax);
        switchStatementCallback(builder);
        return builder.Syntax;
    }

    public ISwitchStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ISwitchStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ISwitchStatementBuilder WithOpenParenToken()
    {
        Syntax = Syntax.WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken));
        return this;
    }

    public ISwitchStatementBuilder WithCloseParenToken()
    {
        Syntax = Syntax.WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken));
        return this;
    }

    public ISwitchStatementBuilder AddSection(Action<ISwitchSectionBuilder> switchSectionCallback)
    {
        var section = SwitchSectionBuilder.CreateSyntax(switchSectionCallback);
        Syntax = Syntax.AddSections(section);
        return this;
    }

    public ISwitchStatementBuilder AddSection(SwitchSectionSyntax section)
    {
        Syntax = Syntax.AddSections(section);
        return this;
    }
}