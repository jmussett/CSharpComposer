using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEventDeclarationBuilder : IBasePropertyDeclarationBuilder<IEventDeclarationBuilder>
{
    IEventDeclarationBuilder WithSemicolonToken();
}

internal partial class EventDeclarationBuilder : IEventDeclarationBuilder
{
    public EventDeclarationSyntax Syntax { get; set; }

    public EventDeclarationBuilder(EventDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EventDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null)
    {
        var eventKeywordToken = SyntaxFactory.Token(SyntaxKind.EventKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.EventDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), eventKeywordToken, typeSyntax, default(ExplicitInterfaceSpecifierSyntax), identifierToken, default(AccessorListSyntax), default(SyntaxToken));
        var builder = new EventDeclarationBuilder(syntax);
        eventDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IEventDeclarationBuilder AddAccessorDeclaration(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder>? accessorDeclarationCallback = null)
    {
        var accessor = AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IEventDeclarationBuilder AddAccessorDeclaration(AccessorDeclarationSyntax accessor)
    {
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IEventDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }

    public IEventDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEventDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEventDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IEventDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IEventDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }
}