using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEventDeclarationBuilder : IBasePropertyDeclarationBuilder<IEventDeclarationBuilder>
{
    IEventDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback);
    IEventDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor);
    IEventDeclarationBuilder WithSemicolonToken();
}

public interface IWithEventDeclarationBuilder<TBuilder>
{
    TBuilder WithEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback);
    TBuilder WithEventDeclaration(EventDeclarationSyntax eventDeclarationSyntax);
}

public partial class EventDeclarationBuilder : IEventDeclarationBuilder
{
    public EventDeclarationSyntax Syntax { get; set; }

    public EventDeclarationBuilder(EventDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EventDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback)
    {
        var eventKeywordToken = SyntaxFactory.Token(SyntaxKind.EventKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.EventDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), eventKeywordToken, typeSyntax, default(ExplicitInterfaceSpecifierSyntax), identifierToken, default(AccessorListSyntax), default(SyntaxToken));
        var builder = new EventDeclarationBuilder(syntax);
        eventDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IEventDeclarationBuilder AddAccessor(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback)
    {
        var accessor = AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IEventDeclarationBuilder AddAccessor(AccessorDeclarationSyntax accessor)
    {
        Syntax = Syntax.AddAccessorListAccessors(accessor);
        return this;
    }

    public IEventDeclarationBuilder WithSemicolonToken()
    {
        Syntax = Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        return this;
    }

    public IEventDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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