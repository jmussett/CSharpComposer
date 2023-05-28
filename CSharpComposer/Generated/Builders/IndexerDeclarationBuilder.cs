using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIndexerDeclarationBuilder : IBasePropertyDeclarationBuilder<IIndexerDeclarationBuilder>
{
    IIndexerDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IIndexerDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    IIndexerDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback);
    IIndexerDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor);
    IIndexerDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback);
    IIndexerDeclarationBuilder AddParameter(ParameterSyntax parameter);
}

public interface IWithIndexerDeclarationBuilder<TBuilder>
{
    TBuilder WithIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback);
    TBuilder WithIndexerDeclaration(IndexerDeclarationSyntax indexerDeclarationSyntax);
}

public partial class IndexerDeclarationBuilder : IIndexerDeclarationBuilder
{
    public IndexerDeclarationSyntax Syntax { get; set; }

    public IndexerDeclarationBuilder(IndexerDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IndexerDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var thisKeywordToken = SyntaxFactory.Token(SyntaxKind.ThisKeyword);
        var parameterListSyntax = SyntaxFactory.BracketedParameterList();
        var syntax = SyntaxFactory.IndexerDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), typeSyntax, default(ExplicitInterfaceSpecifierSyntax), thisKeywordToken, parameterListSyntax, default(AccessorListSyntax), null, default(SyntaxToken));
        var builder = new IndexerDeclarationBuilder(syntax);
        indexerDeclarationCallback(builder);
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

    public IIndexerDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback)
    {
        var accessor = AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IIndexerDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor)
    {
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IIndexerDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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

    public IIndexerDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
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