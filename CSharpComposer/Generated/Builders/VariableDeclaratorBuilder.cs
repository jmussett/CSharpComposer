﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IVariableDeclaratorBuilder : IAddArgument<IVariableDeclaratorBuilder>
{
    IVariableDeclaratorBuilder WithInitializer(Action<IExpressionBuilder> valueCallback);
    IVariableDeclaratorBuilder WithInitializer(EqualsValueClauseSyntax initializer);
}

internal partial class VariableDeclaratorBuilder : IVariableDeclaratorBuilder
{
    public VariableDeclaratorSyntax Syntax { get; set; }

    public VariableDeclaratorBuilder(VariableDeclaratorSyntax syntax)
    {
        Syntax = syntax;
    }

    public static VariableDeclaratorSyntax CreateSyntax(string identifier, Action<IVariableDeclaratorBuilder>? variableDeclaratorCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.VariableDeclarator(identifierToken, default(BracketedArgumentListSyntax), default(EqualsValueClauseSyntax));
        var builder = new VariableDeclaratorBuilder(syntax);
        variableDeclaratorCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IVariableDeclaratorBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IVariableDeclaratorBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IVariableDeclaratorBuilder WithInitializer(Action<IExpressionBuilder> valueCallback)
    {
        var initializerSyntax = EqualsValueClauseBuilder.CreateSyntax(valueCallback);
        Syntax = Syntax.WithInitializer(initializerSyntax);
        return this;
    }

    public IVariableDeclaratorBuilder WithInitializer(EqualsValueClauseSyntax initializer)
    {
        Syntax = Syntax.WithInitializer(initializer);
        return this;
    }
}