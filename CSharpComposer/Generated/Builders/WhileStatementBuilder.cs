using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IWhileStatementBuilder : IStatementBuilder<IWhileStatementBuilder>
{
}

public partial class WhileStatementBuilder : IWhileStatementBuilder
{
    public WhileStatementSyntax Syntax { get; set; }

    public WhileStatementBuilder(WhileStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static WhileStatementSyntax CreateSyntax(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder>? whileStatementCallback = null)
    {
        var whileKeywordToken = SyntaxFactory.Token(SyntaxKind.WhileKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.WhileStatement(default(SyntaxList<AttributeListSyntax>), whileKeywordToken, openParenTokenToken, conditionSyntax, closeParenTokenToken, statementSyntax);
        var builder = new WhileStatementBuilder(syntax);
        whileStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IWhileStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IWhileStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}