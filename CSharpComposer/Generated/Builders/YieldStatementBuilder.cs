using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IYieldStatementBuilder : IStatementBuilder<IYieldStatementBuilder>, IWithExpression<IYieldStatementBuilder>
{
}

public partial class YieldStatementBuilder : IYieldStatementBuilder
{
    public YieldStatementSyntax Syntax { get; set; }

    public YieldStatementBuilder(YieldStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static YieldStatementSyntax CreateSyntax(YieldStatementKind kind, Action<IYieldStatementBuilder>? yieldStatementCallback = null)
    {
        var syntaxKind = kind switch
        {
            YieldStatementKind.YieldReturnStatement => SyntaxKind.YieldReturnStatement,
            YieldStatementKind.YieldBreakStatement => SyntaxKind.YieldBreakStatement,
            _ => throw new NotSupportedException()
        };
        var yieldKeywordToken = SyntaxFactory.Token(SyntaxKind.YieldKeyword);
        var returnOrBreakKeywordToken = kind switch
        {
            YieldStatementKind.YieldReturnStatement => SyntaxFactory.Token(SyntaxKind.ReturnKeyword),
            YieldStatementKind.YieldBreakStatement => SyntaxFactory.Token(SyntaxKind.BreakKeyword),
            _ => throw new NotSupportedException()
        };
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.YieldStatement(syntaxKind, default(SyntaxList<AttributeListSyntax>), yieldKeywordToken, returnOrBreakKeywordToken, default(ExpressionSyntax), semicolonTokenToken);
        var builder = new YieldStatementBuilder(syntax);
        yieldStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IYieldStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IYieldStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IYieldStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpression(expressionSyntax);
        return this;
    }

    public IYieldStatementBuilder WithExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.WithExpression(expression);
        return this;
    }
}