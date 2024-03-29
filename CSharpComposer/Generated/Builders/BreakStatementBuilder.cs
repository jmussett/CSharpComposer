﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBreakStatementBuilder : IStatementBuilder<IBreakStatementBuilder>
{
}

internal partial class BreakStatementBuilder : IBreakStatementBuilder
{
    public BreakStatementSyntax Syntax { get; set; }

    public BreakStatementBuilder(BreakStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static BreakStatementSyntax CreateSyntax(Action<IBreakStatementBuilder>? breakStatementCallback = null)
    {
        var breakKeywordToken = SyntaxFactory.Token(SyntaxKind.BreakKeyword);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.BreakStatement(default(SyntaxList<AttributeListSyntax>), breakKeywordToken, semicolonTokenToken);
        var builder = new BreakStatementBuilder(syntax);
        breakStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IBreakStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IBreakStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}