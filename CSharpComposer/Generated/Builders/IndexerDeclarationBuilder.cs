using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIndexerDeclarationBuilder : IBasePropertyDeclarationBuilder<IIndexerDeclarationBuilder>, IAddParameter<IIndexerDeclarationBuilder>
{
    IIndexerDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IIndexerDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
}

internal partial class IndexerDeclarationBuilder : IIndexerDeclarationBuilder
{
    public IndexerDeclarationSyntax Syntax { get; set; }

    public IndexerDeclarationBuilder(IndexerDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IndexerDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var thisKeywordToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        var parameterListSyntax = SyntaxFactory.BracketedParameterList();
        var syntax = SyntaxFactory.IndexerDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), typeSyntax, default(ExplicitInterfaceSpecifierSyntax), thisKeywordToken, parameterListSyntax, default(AccessorListSyntax), null, default(SyntaxToken));
        var builder = new IndexerDeclarationBuilder(syntax);
        indexerDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IIndexerDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IIndexerDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IIndexerDeclarationBuilder AddAccessorDeclaration(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder>? accessorDeclarationCallback = null)
    {
        var accessor = AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IIndexerDeclarationBuilder AddAccessorDeclaration(AccessorDeclarationSyntax accessor)
    {
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IIndexerDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIndexerDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIndexerDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IIndexerDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IIndexerDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }

    public IIndexerDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IIndexerDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}