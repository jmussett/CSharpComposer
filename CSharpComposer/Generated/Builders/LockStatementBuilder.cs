using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILockStatementBuilder : IStatementBuilder<ILockStatementBuilder>
{
}

public interface IWithLockStatementBuilder<TBuilder>
{
    TBuilder WithLockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder> lockStatementCallback);
    TBuilder WithLockStatement(LockStatementSyntax lockStatementSyntax);
}

public partial class LockStatementBuilder : ILockStatementBuilder
{
    public LockStatementSyntax Syntax { get; set; }

    public LockStatementBuilder(LockStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LockStatementSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder> lockStatementCallback)
    {
        var lockKeywordToken = SyntaxFactory.Token(SyntaxKind.LockKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.LockStatement(default(SyntaxList<AttributeListSyntax>), lockKeywordToken, openParenTokenToken, expressionSyntax, closeParenTokenToken, statementSyntax);
        var builder = new LockStatementBuilder(syntax);
        lockStatementCallback(builder);
        return builder.Syntax;
    }

    public ILockStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILockStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}