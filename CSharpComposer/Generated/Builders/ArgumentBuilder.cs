using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArgumentBuilder : IWithNameColon<IArgumentBuilder>
{
    IArgumentBuilder WithRefKindKeyword(RefKindKeyword refKindKeyword);
}

public interface IAddArgument<TBuilder>
{
    TBuilder AddArgument(ArgumentSyntax argumentSyntax);
    TBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null);
}

internal partial class ArgumentBuilder : IArgumentBuilder
{
    public ArgumentSyntax Syntax { get; set; }

    public ArgumentBuilder(ArgumentSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArgumentSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.Argument(default(NameColonSyntax), default(SyntaxToken), expressionSyntax);
        var builder = new ArgumentBuilder(syntax);
        argumentCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IArgumentBuilder WithNameColon(string nameIdentifier)
    {
        var nameColonSyntax = NameColonBuilder.CreateSyntax(nameIdentifier);
        Syntax = Syntax.WithNameColon(nameColonSyntax);
        return this;
    }

    public IArgumentBuilder WithNameColon(NameColonSyntax nameColon)
    {
        Syntax = Syntax.WithNameColon(nameColon);
        return this;
    }

    public IArgumentBuilder WithRefKindKeyword(RefKindKeyword refKindKeyword)
    {
        Syntax = Syntax.WithRefKindKeyword(SyntaxFactory.Token(refKindKeyword switch
        {
            RefKindKeyword.RefKeyword => SyntaxKind.RefKeyword,
            RefKindKeyword.OutKeyword => SyntaxKind.OutKeyword,
            RefKindKeyword.InKeyword => SyntaxKind.InKeyword,
            _ => throw new NotSupportedException()
        }));
        return this;
    }
}