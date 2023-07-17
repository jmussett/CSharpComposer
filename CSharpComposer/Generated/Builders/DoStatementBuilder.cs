using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IDoStatementBuilder : IStatementBuilder<IDoStatementBuilder>
{
}

internal partial class DoStatementBuilder : IDoStatementBuilder
{
    public DoStatementSyntax Syntax { get; set; }

    public DoStatementBuilder(DoStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static DoStatementSyntax CreateSyntax(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder>? doStatementCallback = null)
    {
        var doKeywordToken = SyntaxFactory.Token(SyntaxKind.DoKeyword);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var whileKeywordToken = SyntaxFactory.Token(SyntaxKind.WhileKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.DoStatement(default(SyntaxList<AttributeListSyntax>), doKeywordToken, statementSyntax, whileKeywordToken, openParenTokenToken, conditionSyntax, closeParenTokenToken, semicolonTokenToken);
        var builder = new DoStatementBuilder(syntax);
        doStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IDoStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IDoStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}