using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IForEachStatementBuilder : ICommonForEachStatementBuilder<IForEachStatementBuilder>
{
}

public interface IWithForEachStatementBuilder<TBuilder>
{
    TBuilder WithForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback);
    TBuilder WithForEachStatement(ForEachStatementSyntax forEachStatementSyntax);
}

public partial class ForEachStatementBuilder : IForEachStatementBuilder
{
    public ForEachStatementSyntax Syntax { get; set; }

    public ForEachStatementBuilder(ForEachStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ForEachStatementSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder> forEachStatementCallback)
    {
        var forEachKeywordToken = SyntaxFactory.Token(SyntaxKind.ForEachKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var inKeywordToken = SyntaxFactory.Token(SyntaxKind.InKeyword);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.ForEachStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxToken), forEachKeywordToken, openParenTokenToken, typeSyntax, identifierToken, inKeywordToken, expressionSyntax, closeParenTokenToken, statementSyntax);
        var builder = new ForEachStatementBuilder(syntax);
        forEachStatementCallback(builder);
        return builder.Syntax;
    }

    public IForEachStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForEachStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IForEachStatementBuilder WithAwaitKeyword()
    {
        Syntax = Syntax.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword));
        return this;
    }
}