using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithOmittedArraySizeExpression<TBuilder>
{
    TBuilder WithOmittedArraySizeExpression();
    TBuilder WithOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax);
}

public interface IAddOmittedArraySizeExpression<TBuilder>
{
    TBuilder AddOmittedArraySizeExpression();
    TBuilder AddOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax);
}

public partial class OmittedArraySizeExpressionBuilder
{
    public static OmittedArraySizeExpressionSyntax CreateSyntax()
    {
        var omittedArraySizeExpressionTokenToken = SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken);
        return SyntaxFactory.OmittedArraySizeExpression(omittedArraySizeExpressionTokenToken);
    }
}