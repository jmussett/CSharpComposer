using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFixedStatementBuilder : IStatementBuilder<IFixedStatementBuilder>
{
}

public interface IWithFixedStatementBuilder<TBuilder>
{
    TBuilder WithFixedStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback);
    TBuilder WithFixedStatement(FixedStatementSyntax fixedStatementSyntax);
}

public partial class FixedStatementBuilder : IFixedStatementBuilder
{
    public FixedStatementSyntax Syntax { get; set; }

    public FixedStatementBuilder(FixedStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FixedStatementSyntax CreateSyntax(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback)
    {
        var fixedKeywordToken = SyntaxFactory.Token(SyntaxKind.FixedKeyword);
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.FixedStatement(default(SyntaxList<AttributeListSyntax>), fixedKeywordToken, openParenTokenToken, declarationSyntax, closeParenTokenToken, statementSyntax);
        var builder = new FixedStatementBuilder(syntax);
        fixedStatementCallback(builder);
        return builder.Syntax;
    }

    public IFixedStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFixedStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}