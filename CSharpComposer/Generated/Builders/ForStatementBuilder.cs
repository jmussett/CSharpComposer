using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IForStatementBuilder : IStatementBuilder<IForStatementBuilder>
{
    IForStatementBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null);
    IForStatementBuilder WithDeclaration(VariableDeclarationSyntax declaration);
    IForStatementBuilder AddInitializerExpression(Action<IExpressionBuilder> initializerCallback);
    IForStatementBuilder AddInitializerExpression(ExpressionSyntax initializer);
    IForStatementBuilder WithCondition(Action<IExpressionBuilder> conditionCallback);
    IForStatementBuilder WithCondition(ExpressionSyntax condition);
    IForStatementBuilder AddIncrementorExpression(Action<IExpressionBuilder> incrementorCallback);
    IForStatementBuilder AddIncrementorExpression(ExpressionSyntax incrementor);
}

public partial class ForStatementBuilder : IForStatementBuilder
{
    public ForStatementSyntax Syntax { get; set; }

    public ForStatementBuilder(ForStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ForStatementSyntax CreateSyntax(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder>? forStatementCallback = null)
    {
        var forKeywordToken = SyntaxFactory.Token(SyntaxKind.ForKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var firstSemicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var secondSemicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.ForStatement(default(SyntaxList<AttributeListSyntax>), forKeywordToken, openParenTokenToken, null, default(SeparatedSyntaxList<ExpressionSyntax>), firstSemicolonTokenToken, default(ExpressionSyntax), secondSemicolonTokenToken, default(SeparatedSyntaxList<ExpressionSyntax>), closeParenTokenToken, statementSyntax);
        var builder = new ForStatementBuilder(syntax);
        forStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IForStatementBuilder WithDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null)
    {
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(typeCallback, variableDeclarationCallback);
        Syntax = Syntax.WithDeclaration(declarationSyntax);
        return this;
    }

    public IForStatementBuilder WithDeclaration(VariableDeclarationSyntax declaration)
    {
        Syntax = Syntax.WithDeclaration(declaration);
        return this;
    }

    public IForStatementBuilder AddInitializerExpression(Action<IExpressionBuilder> initializerCallback)
    {
        var initializer = ExpressionBuilder.CreateSyntax(initializerCallback);
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }

    public IForStatementBuilder AddInitializerExpression(ExpressionSyntax initializer)
    {
        Syntax = Syntax.AddInitializers(initializer);
        return this;
    }

    public IForStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForStatementBuilder WithCondition(Action<IExpressionBuilder> conditionCallback)
    {
        var conditionSyntax = ExpressionBuilder.CreateSyntax(conditionCallback);
        Syntax = Syntax.WithCondition(conditionSyntax);
        return this;
    }

    public IForStatementBuilder WithCondition(ExpressionSyntax condition)
    {
        Syntax = Syntax.WithCondition(condition);
        return this;
    }

    public IForStatementBuilder AddIncrementorExpression(Action<IExpressionBuilder> incrementorCallback)
    {
        var incrementor = ExpressionBuilder.CreateSyntax(incrementorCallback);
        Syntax = Syntax.AddIncrementors(incrementor);
        return this;
    }

    public IForStatementBuilder AddIncrementorExpression(ExpressionSyntax incrementor)
    {
        Syntax = Syntax.AddIncrementors(incrementor);
        return this;
    }
}