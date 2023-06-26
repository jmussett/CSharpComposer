using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class VarPatternBuilder
{
    public static VarPatternSyntax CreateSyntax(Action<IVariableDesignationBuilder> designationCallback)
    {
        var varKeywordToken = SyntaxFactory.Token(SyntaxKind.VarKeyword);
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        return SyntaxFactory.VarPattern(varKeywordToken, designationSyntax);
    }
}