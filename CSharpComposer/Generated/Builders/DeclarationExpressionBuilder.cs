using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithDeclarationExpression<TBuilder>
{
    TBuilder WithDeclarationExpression(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback);
    TBuilder WithDeclarationExpression(DeclarationExpressionSyntax declarationExpressionSyntax);
}

public interface IAddDeclarationExpression<TBuilder>
{
    TBuilder AddDeclarationExpression(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback);
    TBuilder AddDeclarationExpression(DeclarationExpressionSyntax declarationExpressionSyntax);
}

public partial class DeclarationExpressionBuilder
{
    public static DeclarationExpressionSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var designationSyntax = VariableDesignationBuilder.CreateSyntax(designationCallback);
        return SyntaxFactory.DeclarationExpression(typeSyntax, designationSyntax);
    }
}