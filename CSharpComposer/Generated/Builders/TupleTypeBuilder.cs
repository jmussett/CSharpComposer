using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITupleTypeBuilder
{
    ITupleTypeBuilder AddElement(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder> tupleElementCallback);
    ITupleTypeBuilder AddElement(TupleElementSyntax element);
}

public interface IWithTupleTypeBuilder<TBuilder>
{
    TBuilder WithTupleType(Action<ITupleTypeBuilder> tupleTypeCallback);
    TBuilder WithTupleType(TupleTypeSyntax tupleTypeSyntax);
}

public partial class TupleTypeBuilder : ITupleTypeBuilder
{
    public TupleTypeSyntax Syntax { get; set; }

    public TupleTypeBuilder(TupleTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TupleTypeSyntax CreateSyntax(Action<ITupleTypeBuilder> tupleTypeCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.TupleType(openParenTokenToken, default(SeparatedSyntaxList<TupleElementSyntax>), closeParenTokenToken);
        var builder = new TupleTypeBuilder(syntax);
        tupleTypeCallback(builder);
        return builder.Syntax;
    }

    public ITupleTypeBuilder AddElement(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder> tupleElementCallback)
    {
        var element = TupleElementBuilder.CreateSyntax(typeCallback, tupleElementCallback);
        Syntax = Syntax.AddElements(element);
        return this;
    }

    public ITupleTypeBuilder AddElement(TupleElementSyntax element)
    {
        Syntax = Syntax.AddElements(element);
        return this;
    }
}