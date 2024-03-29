﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INameMemberCrefBuilder
{
    INameMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null);
    INameMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter);
}

internal partial class NameMemberCrefBuilder : INameMemberCrefBuilder
{
    public NameMemberCrefSyntax Syntax { get; set; }

    public NameMemberCrefBuilder(NameMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static NameMemberCrefSyntax CreateSyntax(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder>? nameMemberCrefCallback = null)
    {
        var nameSyntax = TypeBuilder.CreateSyntax(nameCallback);
        var syntax = SyntaxFactory.NameMemberCref(nameSyntax, default(CrefParameterListSyntax));
        var builder = new NameMemberCrefBuilder(syntax);
        nameMemberCrefCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public INameMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public INameMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}