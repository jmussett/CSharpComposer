using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
internal partial class PointerTypeBuilder
{
    public static PointerTypeSyntax CreateSyntax(Action<ITypeBuilder> elementTypeCallback)
    {
        var elementTypeSyntax = TypeBuilder.CreateSyntax(elementTypeCallback);
        var asteriskTokenToken = SyntaxFactory.Token(SyntaxKind.AsteriskToken);
        return SyntaxFactory.PointerType(elementTypeSyntax, asteriskTokenToken);
    }
}