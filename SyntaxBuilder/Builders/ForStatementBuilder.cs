using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IForStatementBuilder
{
    IForStatementBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IForStatementBuilder WithDeclaration(string identifier, Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback, Action<IExpressionBuilder>? expressionCallback = null);
}

public sealed class ForStatementBuilder : IForStatementBuilder
{
    public ForStatementSyntax Syntax { get; set; }

    public ForStatementBuilder(ForStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ForStatementSyntax CreateSyntax(Func<IForStatementBuilder, IForStatementBuilder> forStatementCallback)
    {
        var forStatementSyntax = SyntaxFactory.ForStatement(SyntaxFactory.Block());

        var builder = new ForStatementBuilder(forStatementSyntax).AssertInvoke(forStatementCallback);

        return builder.Syntax;
    }

    public IForStatementBuilder WithDeclaration(
        string identifier,
        Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback,
        Action<IExpressionBuilder>? expressionCallback = null
    )
    {
        // TODO: Move to VariableDeclarationBuilder ???

        VariableDeclaratorSyntax variableSyntax = VariableDeclaratorBuilder.CreateSyntax(identifier);

        if (expressionCallback is not null)
        {
            var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

            variableSyntax = variableSyntax.WithInitializer(
                SyntaxFactory.EqualsValueClause(expressionSyntax)
            );
        }

        if (Syntax.Declaration is null)
        {
            VariableDeclarationSyntax variableDeclaration = VariableDeclarationBuilder.CreateSyntax(typeCallback);

            variableDeclaration = variableDeclaration.AddVariables(variableSyntax);

            Syntax = Syntax.WithDeclaration(variableDeclaration);
        }
        else
        {
            var variableDeclaration = Syntax.Declaration.AddVariables(variableSyntax);

            Syntax = Syntax.WithDeclaration(variableDeclaration);
        }

        return this;
    }

    public IForStatementBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithStatement(blockSyntax);

        return this;
    }

    // TODO: Create BooleanExpressionBuilder ??
    // TODO: WithCondition

    // TODO: Create IncrementorExpressionBuilder ??
    // TODO: WithIncrementor

}