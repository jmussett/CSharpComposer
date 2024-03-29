﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConversionOperatorMemberCrefBuilder
{
    IConversionOperatorMemberCrefBuilder WithCheckedKeyword();
    IConversionOperatorMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null);
    IConversionOperatorMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter);
}

internal partial class ConversionOperatorMemberCrefBuilder : IConversionOperatorMemberCrefBuilder
{
    public ConversionOperatorMemberCrefSyntax Syntax { get; set; }

    public ConversionOperatorMemberCrefBuilder(ConversionOperatorMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ConversionOperatorMemberCrefSyntax CreateSyntax(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder>? conversionOperatorMemberCrefCallback = null)
    {
        var implicitOrExplicitKeywordToken = conversionOperatorMemberCrefImplicitOrExplicitKeyword switch
        {
            ConversionOperatorMemberCrefImplicitOrExplicitKeyword.ImplicitKeyword => SyntaxFactory.Token(SyntaxKind.ImplicitKeyword),
            ConversionOperatorMemberCrefImplicitOrExplicitKeyword.ExplicitKeyword => SyntaxFactory.Token(SyntaxKind.ExplicitKeyword),
            _ => throw new NotSupportedException()
        };
        var operatorKeywordToken = SyntaxFactory.Token(SyntaxKind.OperatorKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.ConversionOperatorMemberCref(implicitOrExplicitKeywordToken, operatorKeywordToken, default(SyntaxToken), typeSyntax, default(CrefParameterListSyntax));
        var builder = new ConversionOperatorMemberCrefBuilder(syntax);
        conversionOperatorMemberCrefCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IConversionOperatorMemberCrefBuilder WithCheckedKeyword()
    {
        Syntax = Syntax.WithCheckedKeyword(SyntaxFactory.Token(SyntaxKind.CheckedKeyword));
        return this;
    }

    public IConversionOperatorMemberCrefBuilder AddCrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public IConversionOperatorMemberCrefBuilder AddCrefParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}