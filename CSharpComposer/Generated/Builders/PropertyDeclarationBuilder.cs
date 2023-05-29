using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPropertyDeclarationBuilder : IWithExplicitInterfaceSpecifierBuilder<IPropertyDeclarationBuilder>, IBasePropertyDeclarationBuilder<IPropertyDeclarationBuilder>
{
    IPropertyDeclarationBuilder WithArrowExpressionClause(Action<IExpressionBuilder> expressionCallback);
    IPropertyDeclarationBuilder WithArrowExpressionClause(ArrowExpressionClauseSyntax expressionBody);
    IPropertyDeclarationBuilder WithEqualsValueClause(Action<IExpressionBuilder> valueCallback);
    IPropertyDeclarationBuilder WithEqualsValueClause(EqualsValueClauseSyntax initializer);
    IPropertyDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback);
    IPropertyDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor);
}

public interface IWithPropertyDeclarationBuilder<TBuilder>
{
    TBuilder WithPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback);
    TBuilder WithPropertyDeclaration(PropertyDeclarationSyntax propertyDeclarationSyntax);
}

public partial class PropertyDeclarationBuilder : IPropertyDeclarationBuilder
{
    public PropertyDeclarationSyntax Syntax { get; set; }

    public PropertyDeclarationBuilder(PropertyDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PropertyDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.PropertyDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), typeSyntax, default(ExplicitInterfaceSpecifierSyntax), identifierToken, default(AccessorListSyntax), null, null, default(SyntaxToken));
        var builder = new PropertyDeclarationBuilder(syntax);
        propertyDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IPropertyDeclarationBuilder WithArrowExpressionClause(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IPropertyDeclarationBuilder WithArrowExpressionClause(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IPropertyDeclarationBuilder WithEqualsValueClause(Action<IExpressionBuilder> valueCallback)
    {
        var initializerSyntax = EqualsValueClauseBuilder.CreateSyntax(valueCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IPropertyDeclarationBuilder WithEqualsValueClause(EqualsValueClauseSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }

    public IPropertyDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback)
    {
        var accessor = AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IPropertyDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor)
    {
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IPropertyDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IPropertyDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IPropertyDeclarationBuilder AddModifier(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IPropertyDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IPropertyDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }
}