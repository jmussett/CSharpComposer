﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IConversionOperatorDeclarationBuilder : IBaseMethodDeclarationBuilder<IConversionOperatorDeclarationBuilder>, IWithExplicitInterfaceSpecifier<IConversionOperatorDeclarationBuilder>
{
    IConversionOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IConversionOperatorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    IConversionOperatorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null);
    IConversionOperatorDeclarationBuilder WithBody(BlockSyntax body);
    IConversionOperatorDeclarationBuilder WithCheckedKeyword();
}

internal partial class ConversionOperatorDeclarationBuilder : IConversionOperatorDeclarationBuilder
{
    public ConversionOperatorDeclarationSyntax Syntax { get; set; }

    public ConversionOperatorDeclarationBuilder(ConversionOperatorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ConversionOperatorDeclarationSyntax CreateSyntax(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder>? conversionOperatorDeclarationCallback = null)
    {
        var implicitOrExplicitKeywordToken = conversionOperatorDeclarationImplicitOrExplicitKeyword switch
        {
            ConversionOperatorDeclarationImplicitOrExplicitKeyword.ImplicitKeyword => SyntaxFactory.Token(SyntaxKind.ImplicitKeyword),
            ConversionOperatorDeclarationImplicitOrExplicitKeyword.ExplicitKeyword => SyntaxFactory.Token(SyntaxKind.ExplicitKeyword),
            _ => throw new NotSupportedException()
        };
        var operatorKeywordToken = SyntaxFactory.Token(SyntaxKind.OperatorKeyword);
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.ConversionOperatorDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), implicitOrExplicitKeywordToken, default(ExplicitInterfaceSpecifierSyntax), operatorKeywordToken, default(SyntaxToken), typeSyntax, parameterListSyntax, null, null, default(SyntaxToken));
        var builder = new ConversionOperatorDeclarationBuilder(syntax);
        conversionOperatorDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IConversionOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithBody(Action<IBlockBuilder>? blockCallback = null)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IConversionOperatorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IConversionOperatorDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IConversionOperatorDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }

    public IConversionOperatorDeclarationBuilder WithCheckedKeyword()
    {
        Syntax = Syntax.WithCheckedKeyword(SyntaxFactory.Token(SyntaxKind.CheckedKeyword));
        return this;
    }

    public IConversionOperatorDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IConversionOperatorDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }
}