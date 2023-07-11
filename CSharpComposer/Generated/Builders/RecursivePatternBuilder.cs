using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IRecursivePatternBuilder : IWithType<IRecursivePatternBuilder>, IWithPositionalPatternClause<IRecursivePatternBuilder>, IWithPropertyPatternClause<IRecursivePatternBuilder>
{
    IRecursivePatternBuilder WithSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignation);
    IRecursivePatternBuilder WithDiscardDesignation(DiscardDesignationSyntax discardDesignation);
    IRecursivePatternBuilder WithDesignation(Action<IVariableDesignationBuilder> designationCallback);
    IRecursivePatternBuilder WithDesignation(VariableDesignationSyntax designation);
}

public partial class RecursivePatternBuilder : IRecursivePatternBuilder
{
    public RecursivePatternSyntax Syntax { get; set; }

    public RecursivePatternBuilder(RecursivePatternSyntax syntax)
    {
        Syntax = syntax;
    }

    public static RecursivePatternSyntax CreateSyntax(Action<IRecursivePatternBuilder>? recursivePatternCallback = null)
    {
        var syntax = SyntaxFactory.RecursivePattern(default(TypeSyntax), default(PositionalPatternClauseSyntax), default(PropertyPatternClauseSyntax), default(VariableDesignationSyntax));
        var builder = new RecursivePatternBuilder(syntax);
        recursivePatternCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IRecursivePatternBuilder WithType(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.WithType(typeSyntax);
        return this;
    }

    public IRecursivePatternBuilder WithType(TypeSyntax type)
    {
        Syntax = Syntax.WithType(type);
        return this;
    }

    public IRecursivePatternBuilder WithPositionalPatternClause(Action<IPositionalPatternClauseBuilder>? positionalPatternClauseCallback = null)
    {
        var positionalPatternClauseSyntax = PositionalPatternClauseBuilder.CreateSyntax(positionalPatternClauseCallback);
        Syntax = Syntax.WithPositionalPatternClause(positionalPatternClauseSyntax);
        return this;
    }

    public IRecursivePatternBuilder WithPositionalPatternClause(PositionalPatternClauseSyntax positionalPatternClause)
    {
        Syntax = Syntax.WithPositionalPatternClause(positionalPatternClause);
        return this;
    }

    public IRecursivePatternBuilder WithPropertyPatternClause(Action<IPropertyPatternClauseBuilder>? propertyPatternClauseCallback = null)
    {
        var propertyPatternClauseSyntax = PropertyPatternClauseBuilder.CreateSyntax(propertyPatternClauseCallback);
        Syntax = Syntax.WithPropertyPatternClause(propertyPatternClauseSyntax);
        return this;
    }

    public IRecursivePatternBuilder WithPropertyPatternClause(PropertyPatternClauseSyntax propertyPatternClause)
    {
        Syntax = Syntax.WithPropertyPatternClause(propertyPatternClause);
        return this;
    }

    public IRecursivePatternBuilder WithSingleVariableDesignation(SingleVariableDesignationSyntax singleVariableDesignation)
    {
        Syntax = Syntax.WithDesignation(singleVariableDesignation);
        return this;
    }

    public IRecursivePatternBuilder WithDiscardDesignation(DiscardDesignationSyntax discardDesignation)
    {
        Syntax = Syntax.WithDesignation(discardDesignation);
        return this;
    }

    public IRecursivePatternBuilder WithDesignation(Action<IVariableDesignationBuilder> designationCallback)
    {
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        Syntax = Syntax.WithDesignation(designationSyntax);
        return this;
    }

    public IRecursivePatternBuilder WithDesignation(VariableDesignationSyntax designation)
    {
        Syntax = Syntax.WithDesignation(designation);
        return this;
    }
}