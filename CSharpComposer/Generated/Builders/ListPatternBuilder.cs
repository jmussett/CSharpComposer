using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IListPatternBuilder
{
    IListPatternBuilder AddPattern(Action<IPatternBuilder> patternCallback);
    IListPatternBuilder AddPattern(PatternSyntax pattern);
    IListPatternBuilder WithSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignation);
    IListPatternBuilder WithDiscardDesignation(DiscardDesignationSyntax discardDesignation);
    IListPatternBuilder WithDesignation(Action<IVariableDesignationBuilder> designationCallback);
    IListPatternBuilder WithDesignation(VariableDesignationSyntax designation);
}

public interface IWithListPatternBuilder<TBuilder>
{
    TBuilder WithListPattern(Action<IListPatternBuilder> listPatternCallback);
    TBuilder WithListPattern(ListPatternSyntax listPatternSyntax);
}

public partial class ListPatternBuilder : IListPatternBuilder
{
    public ListPatternSyntax Syntax { get; set; }

    public ListPatternBuilder(ListPatternSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ListPatternSyntax CreateSyntax(Action<IListPatternBuilder> listPatternCallback)
    {
        var openBracketTokenToken = SyntaxFactory.Token(SyntaxKind.OpenBracketToken);
        var closeBracketTokenToken = SyntaxFactory.Token(SyntaxKind.CloseBracketToken);
        var syntax = SyntaxFactory.ListPattern(openBracketTokenToken, default(SeparatedSyntaxList<PatternSyntax>), closeBracketTokenToken, default(VariableDesignationSyntax));
        var builder = new ListPatternBuilder(syntax);
        listPatternCallback(builder);
        return builder.Syntax;
    }

    public IListPatternBuilder AddPattern(Action<IPatternBuilder> patternCallback)
    {
        var pattern = PatternBuilder.CreateSyntax(patternCallback);
        Syntax = Syntax.AddPatterns(pattern);
        return this;
    }

    public IListPatternBuilder AddPattern(PatternSyntax pattern)
    {
        Syntax = Syntax.AddPatterns(pattern);
        return this;
    }

    public IListPatternBuilder WithSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignation)
    {
        Syntax = Syntax.WithDesignation(singleVariableDesignation);
        return this;
    }

    public IListPatternBuilder WithDiscardDesignation(DiscardDesignationSyntax discardDesignation)
    {
        Syntax = Syntax.WithDesignation(discardDesignation);
        return this;
    }

    public IListPatternBuilder WithDesignation(Action<IVariableDesignationBuilder> designationCallback)
    {
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        Syntax = Syntax.WithDesignation(designationSyntax);
        return this;
    }

    public IListPatternBuilder WithDesignation(VariableDesignationSyntax designation)
    {
        Syntax = Syntax.WithDesignation(designation);
        return this;
    }
}