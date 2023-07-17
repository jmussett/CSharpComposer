using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITupleElementBuilder
{
    ITupleElementBuilder WithIdentifier(string identifier);
}

internal partial class TupleElementBuilder : ITupleElementBuilder
{
    public TupleElementSyntax Syntax { get; set; }

    public TupleElementBuilder(TupleElementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static TupleElementSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder>? tupleElementCallback = null)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.TupleElement(typeSyntax, default(SyntaxToken));
        var builder = new TupleElementBuilder(syntax);
        tupleElementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public ITupleElementBuilder WithIdentifier(string identifier)
    {
        Syntax = Syntax.WithIdentifier(SyntaxFactory.Identifier(identifier));
        return this;
    }
}