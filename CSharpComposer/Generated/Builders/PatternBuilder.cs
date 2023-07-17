using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPatternBuilder
{
    void FromSyntax(PatternSyntax syntax);
    void AsDiscardPattern();
    void AsDeclarationPattern(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback);
    void AsVarPattern(Action<IVariableDesignationBuilder> designationCallback);
    void AsRecursivePattern(Action<IRecursivePatternBuilder>? recursivePatternCallback = null);
    void AsConstantPattern(Action<IExpressionBuilder> expressionCallback);
    void AsParenthesizedPattern(Action<IPatternBuilder> patternCallback);
    void AsRelationalPattern(RelationalPatternOperatorToken relationalPatternOperatorToken, Action<IExpressionBuilder> expressionCallback);
    void AsTypePattern(Action<ITypeBuilder> typeCallback);
    void AsBinaryPattern(BinaryPatternKind kind, Action<IPatternBuilder> leftCallback, Action<IPatternBuilder> rightCallback);
    void AsUnaryPattern(Action<IPatternBuilder> patternCallback);
    void AsListPattern(Action<IListPatternBuilder>? listPatternCallback = null);
    void AsSlicePattern(Action<ISlicePatternBuilder>? slicePatternCallback = null);
}

public interface IWithPattern<TBuilder>
{
    TBuilder WithPattern(PatternSyntax patternSyntax);
    TBuilder WithPattern(Action<IPatternBuilder> patternCallback);
}

public interface IAddPattern<TBuilder>
{
    TBuilder AddPattern(PatternSyntax patternSyntax);
    TBuilder AddPattern(Action<IPatternBuilder> patternCallback);
}

internal partial class PatternBuilder : IPatternBuilder
{
    public PatternSyntax? Syntax { get; set; }

    public static PatternSyntax CreateSyntax(Action<IPatternBuilder> callback)
    {
        var builder = new PatternBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("PatternSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(PatternSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsDiscardPattern()
    {
        Syntax = DiscardPatternBuilder.CreateSyntax();
    }

    public void AsDeclarationPattern(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        Syntax = DeclarationPatternBuilder.CreateSyntax(typeCallback, designationCallback);
    }

    public void AsVarPattern(Action<IVariableDesignationBuilder> designationCallback)
    {
        Syntax = VarPatternBuilder.CreateSyntax(designationCallback);
    }

    public void AsRecursivePattern(Action<IRecursivePatternBuilder>? recursivePatternCallback = null)
    {
        Syntax = RecursivePatternBuilder.CreateSyntax(recursivePatternCallback);
    }

    public void AsConstantPattern(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = ConstantPatternBuilder.CreateSyntax(expressionCallback);
    }

    public void AsParenthesizedPattern(Action<IPatternBuilder> patternCallback)
    {
        Syntax = ParenthesizedPatternBuilder.CreateSyntax(patternCallback);
    }

    public void AsRelationalPattern(RelationalPatternOperatorToken relationalPatternOperatorToken, Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = RelationalPatternBuilder.CreateSyntax(relationalPatternOperatorToken, expressionCallback);
    }

    public void AsTypePattern(Action<ITypeBuilder> typeCallback)
    {
        Syntax = TypePatternBuilder.CreateSyntax(typeCallback);
    }

    public void AsBinaryPattern(BinaryPatternKind kind, Action<IPatternBuilder> leftCallback, Action<IPatternBuilder> rightCallback)
    {
        Syntax = BinaryPatternBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public void AsUnaryPattern(Action<IPatternBuilder> patternCallback)
    {
        Syntax = UnaryPatternBuilder.CreateSyntax(patternCallback);
    }

    public void AsListPattern(Action<IListPatternBuilder>? listPatternCallback = null)
    {
        Syntax = ListPatternBuilder.CreateSyntax(listPatternCallback);
    }

    public void AsSlicePattern(Action<ISlicePatternBuilder>? slicePatternCallback = null)
    {
        Syntax = SlicePatternBuilder.CreateSyntax(slicePatternCallback);
    }
}