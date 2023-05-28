using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IOperatorDeclarationBuilder : IWithExplicitInterfaceSpecifierBuilder<IOperatorDeclarationBuilder>, IBaseMethodDeclarationBuilder<IOperatorDeclarationBuilder>
{
    IOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback);
    IOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
    IOperatorDeclarationBuilder WithCheckedKeyword();
}

public interface IWithOperatorDeclarationBuilder<TBuilder>
{
    TBuilder WithOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback);
    TBuilder WithOperatorDeclaration(OperatorDeclarationSyntax operatorDeclarationSyntax);
}

public partial class OperatorDeclarationBuilder : IOperatorDeclarationBuilder
{
    public OperatorDeclarationSyntax Syntax { get; set; }

    public OperatorDeclarationBuilder(OperatorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static OperatorDeclarationSyntax CreateSyntax(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        var operatorKeywordToken = SyntaxFactory.Token(SyntaxKind.OperatorKeyword);
        var operatorTokenToken = operatorDeclarationOperatorToken switch
        {
            OperatorDeclarationOperatorToken.PlusToken => SyntaxFactory.Token(SyntaxKind.PlusToken),
            OperatorDeclarationOperatorToken.MinusToken => SyntaxFactory.Token(SyntaxKind.MinusToken),
            OperatorDeclarationOperatorToken.ExclamationToken => SyntaxFactory.Token(SyntaxKind.ExclamationToken),
            OperatorDeclarationOperatorToken.TildeToken => SyntaxFactory.Token(SyntaxKind.TildeToken),
            OperatorDeclarationOperatorToken.PlusPlusToken => SyntaxFactory.Token(SyntaxKind.PlusPlusToken),
            OperatorDeclarationOperatorToken.MinusMinusToken => SyntaxFactory.Token(SyntaxKind.MinusMinusToken),
            OperatorDeclarationOperatorToken.AsteriskToken => SyntaxFactory.Token(SyntaxKind.AsteriskToken),
            OperatorDeclarationOperatorToken.SlashToken => SyntaxFactory.Token(SyntaxKind.SlashToken),
            OperatorDeclarationOperatorToken.PercentToken => SyntaxFactory.Token(SyntaxKind.PercentToken),
            OperatorDeclarationOperatorToken.LessThanLessThanToken => SyntaxFactory.Token(SyntaxKind.LessThanLessThanToken),
            OperatorDeclarationOperatorToken.GreaterThanGreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanToken),
            OperatorDeclarationOperatorToken.GreaterThanGreaterThanGreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanGreaterThanGreaterThanToken),
            OperatorDeclarationOperatorToken.BarToken => SyntaxFactory.Token(SyntaxKind.BarToken),
            OperatorDeclarationOperatorToken.AmpersandToken => SyntaxFactory.Token(SyntaxKind.AmpersandToken),
            OperatorDeclarationOperatorToken.CaretToken => SyntaxFactory.Token(SyntaxKind.CaretToken),
            OperatorDeclarationOperatorToken.EqualsEqualsToken => SyntaxFactory.Token(SyntaxKind.EqualsEqualsToken),
            OperatorDeclarationOperatorToken.ExclamationEqualsToken => SyntaxFactory.Token(SyntaxKind.ExclamationEqualsToken),
            OperatorDeclarationOperatorToken.LessThanToken => SyntaxFactory.Token(SyntaxKind.LessThanToken),
            OperatorDeclarationOperatorToken.LessThanEqualsToken => SyntaxFactory.Token(SyntaxKind.LessThanEqualsToken),
            OperatorDeclarationOperatorToken.GreaterThanToken => SyntaxFactory.Token(SyntaxKind.GreaterThanToken),
            OperatorDeclarationOperatorToken.GreaterThanEqualsToken => SyntaxFactory.Token(SyntaxKind.GreaterThanEqualsToken),
            OperatorDeclarationOperatorToken.FalseKeyword => SyntaxFactory.Token(SyntaxKind.FalseKeyword),
            OperatorDeclarationOperatorToken.TrueKeyword => SyntaxFactory.Token(SyntaxKind.TrueKeyword),
            OperatorDeclarationOperatorToken.IsKeyword => SyntaxFactory.Token(SyntaxKind.IsKeyword),
            _ => throw new NotSupportedException()
        };
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.OperatorDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), returnTypeSyntax, default(ExplicitInterfaceSpecifierSyntax), operatorKeywordToken, default(SyntaxToken), operatorTokenToken, parameterListSyntax, null, null, default(SyntaxToken));
        var builder = new OperatorDeclarationBuilder(syntax);
        operatorDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IOperatorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IOperatorDeclarationBuilder WithBody(Action<IBlockBuilder> blockCallback)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IOperatorDeclarationBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IOperatorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IOperatorDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }

    public IOperatorDeclarationBuilder WithCheckedKeyword()
    {
        Syntax = Syntax.WithCheckedKeyword(SyntaxFactory.Token(SyntaxKind.CheckedKeyword));
        return this;
    }

    public IOperatorDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IOperatorDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}