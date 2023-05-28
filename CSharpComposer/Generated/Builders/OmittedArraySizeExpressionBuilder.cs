using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public interface IWithOmittedArraySizeExpressionBuilder<TBuilder>
{
    TBuilder WithOmittedArraySizeExpression();
    TBuilder WithOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax omittedArraySizeExpressionSyntax);
}

public partial class OmittedArraySizeExpressionBuilder
{
    public static OmittedArraySizeExpressionSyntax CreateSyntax()
    {
        var omittedArraySizeExpressionTokenToken = SyntaxFactory.Token(SyntaxKind.OmittedArraySizeExpressionToken);
        return SyntaxFactory.OmittedArraySizeExpression(omittedArraySizeExpressionTokenToken);
    }
}