using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IForEachVariableStatementBuilder : ICommonForEachStatementBuilder<IForEachVariableStatementBuilder>
{
}

public interface IWithForEachVariableStatementBuilder<TBuilder>
{
    TBuilder WithForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback);
    TBuilder WithForEachVariableStatement(ForEachVariableStatementSyntax forEachVariableStatementSyntax);
}

public partial class ForEachVariableStatementBuilder : IForEachVariableStatementBuilder
{
    public ForEachVariableStatementSyntax Syntax { get; set; }

    public ForEachVariableStatementBuilder(ForEachVariableStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ForEachVariableStatementSyntax CreateSyntax(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder> forEachVariableStatementCallback)
    {
        var forEachKeywordToken = SyntaxFactory.Token(SyntaxKind.ForEachKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var variableSyntax = ExpressionBuilder.CreateSyntax(variableCallback);
        var inKeywordToken = SyntaxFactory.Token(SyntaxKind.InKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.ForEachVariableStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxToken), forEachKeywordToken, openParenTokenToken, variableSyntax, inKeywordToken, expressionSyntax, closeParenTokenToken, statementSyntax);
        var builder = new ForEachVariableStatementBuilder(syntax);
        forEachVariableStatementCallback(builder);
        return builder.Syntax;
    }

    public IForEachVariableStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForEachVariableStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForEachVariableStatementBuilder WithAwaitKeyword()
    {
        Syntax = Syntax.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword));
        return this;
    }
}