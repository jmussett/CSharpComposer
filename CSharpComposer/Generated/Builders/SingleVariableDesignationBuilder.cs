﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithSingleVariableDesignation<TBuilder>
{
    TBuilder WithSingleVariableDesignation(string identifier);
    TBuilder WithSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignationSyntax);
}

public interface IAddSingleVariableDesignation<TBuilder>
{
    TBuilder AddSingleVariableDesignation(string identifier);
    TBuilder AddSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignationSyntax);
}

public partial class SingleVariableDesignationBuilder
{
    public static SingleVariableDesignationSyntax CreateSyntax(string identifier)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        return SyntaxFactory.SingleVariableDesignation(identifierToken);
    }
}