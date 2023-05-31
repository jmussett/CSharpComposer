using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithVarPattern<TBuilder>
{
    TBuilder WithVarPattern(Action<IVariableDesignationBuilder> designationCallback);
    TBuilder WithVarPattern(VarPatternSyntax varPatternSyntax);
}

public interface IAddVarPattern<TBuilder>
{
    TBuilder AddVarPattern(Action<IVariableDesignationBuilder> designationCallback);
    TBuilder AddVarPattern(VarPatternSyntax varPatternSyntax);
}

public partial class VarPatternBuilder
{
    public static VarPatternSyntax CreateSyntax(Action<IVariableDesignationBuilder> designationCallback)
    {
        var varKeywordToken = SyntaxFactory.Token(SyntaxKind.VarKeyword);
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        return SyntaxFactory.VarPattern(varKeywordToken, designationSyntax);
    }
}