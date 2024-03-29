﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEmptyStatementBuilder : IStatementBuilder<IEmptyStatementBuilder>
{
}

internal partial class EmptyStatementBuilder : IEmptyStatementBuilder
{
    public EmptyStatementSyntax Syntax { get; set; }

    public EmptyStatementBuilder(EmptyStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EmptyStatementSyntax CreateSyntax(Action<IEmptyStatementBuilder>? emptyStatementCallback = null)
    {
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.EmptyStatement(default(SyntaxList<AttributeListSyntax>), semicolonTokenToken);
        var builder = new EmptyStatementBuilder(syntax);
        emptyStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IEmptyStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEmptyStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}