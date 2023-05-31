﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IParenthesizedVariableDesignationBuilder
{
    IParenthesizedVariableDesignationBuilder AddVariableDesignation(Action<IVariableDesignationBuilder> variableCallback);
    IParenthesizedVariableDesignationBuilder AddVariableDesignation(VariableDesignationSyntax variable);
}

public interface IWithParenthesizedVariableDesignation<TBuilder>
{
    TBuilder WithParenthesizedVariableDesignation(Action<IParenthesizedVariableDesignationBuilder> parenthesizedVariableDesignationCallback);
    TBuilder WithParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax);
}

public interface IAddParenthesizedVariableDesignation<TBuilder>
{
    TBuilder AddParenthesizedVariableDesignation(Action<IParenthesizedVariableDesignationBuilder> parenthesizedVariableDesignationCallback);
    TBuilder AddParenthesizedVariableDesignation(ParenthesizedVariableDesignationSyntax parenthesizedVariableDesignationSyntax);
}

public partial class ParenthesizedVariableDesignationBuilder : IParenthesizedVariableDesignationBuilder
{
    public ParenthesizedVariableDesignationSyntax Syntax { get; set; }

    public ParenthesizedVariableDesignationBuilder(ParenthesizedVariableDesignationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ParenthesizedVariableDesignationSyntax CreateSyntax(Action<IParenthesizedVariableDesignationBuilder> parenthesizedVariableDesignationCallback)
    {
        var openParenTokenToken = SyntaxFactory.Token(SyntaxKind.OpenParenToken);
        var closeParenTokenToken = SyntaxFactory.Token(SyntaxKind.CloseParenToken);
        var syntax = SyntaxFactory.ParenthesizedVariableDesignation(openParenTokenToken, default(SeparatedSyntaxList<VariableDesignationSyntax>), closeParenTokenToken);
        var builder = new ParenthesizedVariableDesignationBuilder(syntax);
        parenthesizedVariableDesignationCallback(builder);
        return builder.Syntax;
    }

    public IParenthesizedVariableDesignationBuilder AddVariableDesignation(Action<IVariableDesignationBuilder> variableCallback)
    {
        var variable = VariableDesignationBuilder.CreateSyntax(variableCallback);
        Syntax = Syntax.AddVariables(variable);
        return this;
    }

    public IParenthesizedVariableDesignationBuilder AddVariableDesignation(VariableDesignationSyntax variable)
    {
        Syntax = Syntax.AddVariables(variable);
        return this;
    }
}