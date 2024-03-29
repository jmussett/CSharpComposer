﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAnonymousObjectMemberDeclaratorBuilder : IWithNameEquals<IAnonymousObjectMemberDeclaratorBuilder>
{
}

internal partial class AnonymousObjectMemberDeclaratorBuilder : IAnonymousObjectMemberDeclaratorBuilder
{
    public AnonymousObjectMemberDeclaratorSyntax Syntax { get; set; }

    public AnonymousObjectMemberDeclaratorBuilder(AnonymousObjectMemberDeclaratorSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AnonymousObjectMemberDeclaratorSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder>? anonymousObjectMemberDeclaratorCallback = null)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var syntax = SyntaxFactory.AnonymousObjectMemberDeclarator(default(NameEqualsSyntax), expressionSyntax);
        var builder = new AnonymousObjectMemberDeclaratorBuilder(syntax);
        anonymousObjectMemberDeclaratorCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IAnonymousObjectMemberDeclaratorBuilder WithNameEquals(string nameIdentifier)
    {
        var nameEqualsSyntax = NameEqualsBuilder.CreateSyntax(nameIdentifier);
        Syntax = Syntax.WithNameEquals(nameEqualsSyntax);
        return this;
    }

    public IAnonymousObjectMemberDeclaratorBuilder WithNameEquals(NameEqualsSyntax nameEquals)
    {
        Syntax = Syntax.WithNameEquals(nameEquals);
        return this;
    }
}