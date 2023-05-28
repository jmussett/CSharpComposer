using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public static class VariableDeclaratorBuilder
{

    public static VariableDeclaratorSyntax CreateSyntax(string identifier)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        return SyntaxFactory.VariableDeclarator(
            SyntaxFactory.Identifier(identifier)
        );
    }
}