using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IVariableDesignationBuilder
{
    void AsSingleVariableDesignation(string identifier);
    void AsDiscardDesignation();
    void AsParenthesizedVariableDesignation(Action<IParenthesizedVariableDesignationBuilder>? parenthesizedVariableDesignationCallback = null);
}

internal partial class VariableDesignationBuilder : IVariableDesignationBuilder
{
    public VariableDesignationSyntax? Syntax { get; set; }

    public static VariableDesignationSyntax CreateSyntax(Action<IVariableDesignationBuilder> callback)
    {
        var builder = new VariableDesignationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("VariableDesignationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsSingleVariableDesignation(string identifier)
    {
        Syntax = SingleVariableDesignationBuilder.CreateSyntax(identifier);
    }

    public void AsDiscardDesignation()
    {
        Syntax = DiscardDesignationBuilder.CreateSyntax();
    }

    public void AsParenthesizedVariableDesignation(Action<IParenthesizedVariableDesignationBuilder>? parenthesizedVariableDesignationCallback = null)
    {
        Syntax = ParenthesizedVariableDesignationBuilder.CreateSyntax(parenthesizedVariableDesignationCallback);
    }
}