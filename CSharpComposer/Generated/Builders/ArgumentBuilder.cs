using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IArgumentBuilder : IWithNameColonBuilder<IArgumentBuilder>
{
    IArgumentBuilder WithNameColon(string nameIdentifier);
    IArgumentBuilder WithNameColon(NameColonSyntax nameColon);
    IArgumentBuilder WithRefKindKeyword(RefKindKeyword refKindKeyword);
}

public interface IWithArgumentBuilder<TBuilder>
{
    TBuilder WithArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    TBuilder WithArgument(ArgumentSyntax argumentSyntax);
}

public partial class ArgumentBuilder : IArgumentBuilder
{
    public ArgumentSyntax Syntax { get; set; }

    public ArgumentBuilder(ArgumentSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ArgumentSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.Argument(default(NameColonSyntax), default(SyntaxToken), expressionSyntax);
        var builder = new ArgumentBuilder(syntax);
        argumentCallback(builder);
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