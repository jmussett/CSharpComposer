﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEventFieldDeclarationBuilder : IBaseFieldDeclarationBuilder<IEventFieldDeclarationBuilder>
{
}

internal partial class EventFieldDeclarationBuilder : IEventFieldDeclarationBuilder
{
    public EventFieldDeclarationSyntax Syntax { get; set; }

    public EventFieldDeclarationBuilder(EventFieldDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EventFieldDeclarationSyntax CreateSyntax(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        var eventKeywordToken = SyntaxFactory.Token(SyntaxKind.EventKeyword);
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.EventFieldDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), eventKeywordToken, declarationSyntax, semicolonTokenToken);
        var builder = new EventFieldDeclarationBuilder(syntax);
        eventFieldDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IEventFieldDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEventFieldDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEventFieldDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }
}