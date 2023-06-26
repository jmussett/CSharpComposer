using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial class NullableTypeBuilder
{
    public static NullableTypeSyntax CreateSyntax(Action<ITypeBuilder> elementTypeCallback)
    {
        var elementTypeSyntax = TypeBuilder.CreateSyntax(elementTypeCallback);
        var questionTokenToken = SyntaxFactory.Token(SyntaxKind.QuestionToken);
        return SyntaxFactory.NullableType(elementTypeSyntax, questionTokenToken);
    }
}