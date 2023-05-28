using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

internal static class LocalDeclarationStatementBuilder
{

    public static LocalDeclarationStatementSyntax CreateSyntax(string identifier, Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback, Action<IExpressionBuilder>? expressionCallback)
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var variableSyntax = SyntaxFactory.VariableDeclarator(
            SyntaxFactory.Identifier(identifier)
        );

        if (expressionCallback is not null)
        {
            var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

            variableSyntax = variableSyntax.WithInitializer(
                SyntaxFactory.EqualsValueClause(expressionSyntax)
            );
        }

        var typeSyntax = LiteralTypeBuilder.CreateSyntax(typeCallback);

        return SyntaxFactory.LocalDeclarationStatement(
            SyntaxFactory
                .VariableDeclaration(
                    typeSyntax
                )
                .AddVariables(variableSyntax)
        );
    }
}