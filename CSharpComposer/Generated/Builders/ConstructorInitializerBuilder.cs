﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConstructorInitializerBuilder : IAddArgument<IConstructorInitializerBuilder>
{
}

internal partial class ConstructorInitializerBuilder : IConstructorInitializerBuilder
{
    public ConstructorInitializerSyntax Syntax { get; set; }

    public ConstructorInitializerBuilder(ConstructorInitializerSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ConstructorInitializerSyntax CreateSyntax(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder>? constructorInitializerCallback = null)
    {
        var syntaxKind = kind switch
        {
            ConstructorInitializerKind.BaseConstructorInitializer => SyntaxKind.BaseConstructorInitializer,
            ConstructorInitializerKind.ThisConstructorInitializer => SyntaxKind.ThisConstructorInitializer,
            _ => throw new NotSupportedException()
        };
        var colonTokenToken = SyntaxFactory.Token(SyntaxKind.ColonToken);
        var thisOrBaseKeywordToken = kind switch
        {
            ConstructorInitializerKind.BaseConstructorInitializer => SyntaxFactory.Token(SyntaxKind.BaseKeyword),
            ConstructorInitializerKind.ThisConstructorInitializer => SyntaxFactory.Token(SyntaxKind.ThisKeyword),
            _ => throw new NotSupportedException()
        };
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.ConstructorInitializer(syntaxKind, colonTokenToken, thisOrBaseKeywordToken, argumentListSyntax);
        var builder = new ConstructorInitializerBuilder(syntax);
        constructorInitializerCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IConstructorInitializerBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IConstructorInitializerBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}