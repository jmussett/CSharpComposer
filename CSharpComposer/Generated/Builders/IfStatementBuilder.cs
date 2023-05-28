using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIfStatementBuilder : IStatementBuilder<IIfStatementBuilder>
{
    IIfStatementBuilder WithElse(Action<IStatementBuilder> statementCallback);
    IIfStatementBuilder WithElse(ElseClauseSyntax @else);
}

public interface IWithIfStatementBuilder<TBuilder>
{
    TBuilder WithIfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder> ifStatementCallback);
    TBuilder WithIfStatement(IfStatementSyntax ifStatementSyntax);
}

public partial class IfStatementBuilder : IIfStatementBuilder
{
    public IfStatementSyntax Syntax { get; set; }

    public IfStatementBuilder(IfStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IfStatementSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder> ifStatementCallback)
    {
        var ifKeywordToken = SyntaxFactory.Token(SyntaxKind.IfKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.IfStatement(default(SyntaxList<AttributeListSyntax>), ifKeywordToken, openParenTokenToken, conditionSyntax, closeParenTokenToken, statementSyntax, default(ElseClauseSyntax));
        var builder = new IfStatementBuilder(syntax);
        ifStatementCallback(builder);
        return builder.Syntax;
    }

    public IIfStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIfStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIfStatementBuilder WithElse(Action<IStatementBuilder> statementCallback)
    {
        var elseSyntax = ElseClauseBuilder.CreateSyntax(statementCallback);
        Syntax = Syntax.WithElse(elseSyntax);
        return this;
    }

    public IIfStatementBuilder WithElse(ElseClauseSyntax @else)
    {
        Syntax = Syntax.WithElse(@else);
        return this;
    }
}