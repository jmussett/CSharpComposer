﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITupleTypeBuilder
{
    ITupleTypeBuilder AddTupleElement(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder>? tupleElementCallback = null);
    ITupleTypeBuilder AddTupleElement(TupleElementSyntax element);
}

internal partial class TupleTypeBuilder : ITupleTypeBuilder
{
    public TupleTypeSyntax Syntax { get; set; }

    public TupleTypeBuilder(TupleTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TupleTypeSyntax CreateSyntax(Action<ITupleTypeBuilder>? tupleTypeCallback = null)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.TupleType(openParenTokenToken, default(SeparatedSyntaxList<TupleElementSyntax>), closeParenTokenToken);
        var builder = new TupleTypeBuilder(syntax);
        tupleTypeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ITupleTypeBuilder AddTupleElement(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder>? tupleElementCallback = null)
    {
        var element = TupleElementBuilder.CreateSyntax(typeCallback, tupleElementCallback);
        Syntax = Syntax.AddElements(element);
        return this;
    }

    public ITupleTypeBuilder AddTupleElement(TupleElementSyntax element)
    {
        Syntax = Syntax.AddElements(element);
        return this;
    }
}