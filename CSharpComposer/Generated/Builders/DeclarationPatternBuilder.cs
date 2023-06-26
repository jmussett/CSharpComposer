using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class DeclarationPatternBuilder
{
    public static DeclarationPatternSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        return SyntaxFactory.DeclarationPattern(typeSyntax, designationSyntax);
    }
}