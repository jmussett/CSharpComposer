using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public static class VariableDeclarationBuilder
{

    public static VariableDeclarationSyntax CreateSyntax(Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback)
    {
        var typeSyntax = LiteralTypeBuilder.CreateSyntax(typeCallback);

        return SyntaxFactory.VariableDeclaration(typeSyntax);
    }
}