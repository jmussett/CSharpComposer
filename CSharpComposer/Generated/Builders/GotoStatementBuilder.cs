using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGotoStatementBuilder : IWithExpressionBuilder<IGotoStatementBuilder>, IStatementBuilder<IGotoStatementBuilder>
{
    IGotoStatementBuilder WithCaseOrDefaultKeyword(CaseOrDefaultKeyword caseOrDefaultKeyword);
    IGotoStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback);
    IGotoStatementBuilder WithExpression(ExpressionSyntax expression);
}

public interface IWithGotoStatementBuilder<TBuilder>
{
    TBuilder WithGotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder> gotoStatementCallback);
    TBuilder WithGotoStatement(GotoStatementSyntax gotoStatementSyntax);
}

public partial class GotoStatementBuilder : IGotoStatementBuilder
{
    public GotoStatementSyntax Syntax { get; set; }

    public GotoStatementBuilder(GotoStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GotoStatementSyntax CreateSyntax(GotoStatementKind kind, Action<IGotoStatementBuilder> gotoStatementCallback)
    {
        var syntaxKind = kind switch
        {
            GotoStatementKind.GotoStatement => SyntaxKind.GotoStatement,
            GotoStatementKind.GotoCaseStatement => SyntaxKind.GotoCaseStatement,
            GotoStatementKind.GotoDefaultStatement => SyntaxKind.GotoDefaultStatement,
            _ => throw new NotSupportedException()
        };
        var gotoKeywordToken = SyntaxFactory.Token(SyntaxKind.GotoKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.GotoStatement(syntaxKind, default(SyntaxList<AttributeListSyntax>), gotoKeywordToken, default(SyntaxToken), default(ExpressionSyntax), semicolonTokenToken);
        var builder = new GotoStatementBuilder(syntax);
        gotoStatementCallback(builder);
        return builder.Syntax;
    }

    public IGotoStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IGotoStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IGotoStatementBuilder WithCaseOrDefaultKeyword(CaseOrDefaultKeyword caseOrDefaultKeyword)
    {
        Syntax = Syntax.WithCaseOrDefaultKeyword(SyntaxFactory.Token(caseOrDefaultKeyword switch
        {
            CaseOrDefaultKeyword.CaseKeyword => SyntaxKind.CaseKeyword,
            CaseOrDefaultKeyword.DefaultKeyword => SyntaxKind.DefaultKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }

    public IGotoStatementBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpression(expressionSyntax);
        return this;
    }

    public IGotoStatementBuilder WithExpression(ExpressionSyntax expression)
    {
        Syntax = Syntax.WithExpression(expression);
        return this;
    }
}