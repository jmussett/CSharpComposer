using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IBlockBuilder
{
    IBlockBuilder WithStatement(string statement);
    IBlockBuilder WithDoStatement(Func<IBlockBuilder, IBlockBuilder> blockCallback, Action<IExpressionBuilder> expressionCallback);
    IBlockBuilder WithExpression(Action<IExpressionBuilder> expressionCallback);
    IBlockBuilder WithForStatement(Func<IForStatementBuilder, IForStatementBuilder> forCallback);
    IBlockBuilder WithIfStatement(Action<IExpressionBuilder> expressionCallback, Func<IBlockBuilder, IBlockBuilder> blockCallback, Func<IElseClauseBuilder, IElseClauseBuilder>? elseCallback = null);
    IBlockBuilder WithLocalDeclaration(string identifier, Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback, Action<IExpressionBuilder>? expressionCallback = null);
    IBlockBuilder WithLocalFunction(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<ILocalFunctionStatementBuilder, ILocalFunctionStatementBuilder> localFunctionCallback);
    IBlockBuilder WithReturnStatement(Action<IExpressionBuilder> expressionCallback);
    IBlockBuilder WithWhileStatement(Action<IExpressionBuilder> expressionCallback, Func<IBlockBuilder, IBlockBuilder> blockCallback);
}

public sealed class BlockBuilder : IBlockBuilder
{
    public BlockSyntax Syntax { get; set; }

    public BlockBuilder(BlockSyntax syntax)
    {
        Syntax = syntax;
    }

    public static BlockSyntax CreateSyntax(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = SyntaxFactory.Block();

        var builder = new BlockBuilder(blockSyntax).AssertInvoke(blockCallback);

        return builder.Syntax;
    }

    public IBlockBuilder WithStatement(string statement)
    {
        var syntax = StatementBuilder.ParseStatement(statement);

        Syntax = Syntax.AddStatements(syntax);

        return this;
    }

    public IBlockBuilder WithLocalDeclaration(
        string identifier,
        Func<ILiteralTypeBuilder, ILiteralTypeBuilder> typeCallback,
        Action<IExpressionBuilder>? expressionCallback = null
    )
    {
        var syntax = LocalDeclarationStatementBuilder.CreateSyntax(identifier, typeCallback, expressionCallback);

        Syntax = Syntax.AddStatements(syntax);

        return this;
    }

    public IBlockBuilder WithLocalFunction(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<ILocalFunctionStatementBuilder, ILocalFunctionStatementBuilder> localFunctionCallback
    )
    {
        var localFunctionSyntax = LocalFunctionStatementBuilder.CreateSyntax(identifier, typeCallback, localFunctionCallback);

        Syntax = Syntax.AddStatements(localFunctionSyntax);

        return this;
    }

    public IBlockBuilder WithExpression(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionStatementSyntax = ExpressionStatementBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.AddStatements(expressionStatementSyntax);

        return this;
    }

    public IBlockBuilder WithReturnStatement(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ReturnStatementBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.AddStatements(syntax);

        return this;
    }

    // TODO: Create BooleanExpressionBuilder
    public IBlockBuilder WithIfStatement(
        Action<IExpressionBuilder> expressionCallback,
        Func<IBlockBuilder, IBlockBuilder> blockCallback,
        Func<IElseClauseBuilder, IElseClauseBuilder>? elseCallback = null
    )
    {
        var ifStatementSyntax = IfStatementBuilder.CreateSyntax(expressionCallback, blockCallback, elseCallback);

        Syntax = Syntax.AddStatements(ifStatementSyntax);

        return this;
    }

    // TODO: Create BooleanExpressionBuilder
    public IBlockBuilder WithWhileStatement(
        Action<IExpressionBuilder> expressionCallback,
        Func<IBlockBuilder, IBlockBuilder> blockCallback
    )
    {
        var whileStatementSyntax = WhileStatementBuilder.CreateWhileLoopSyntax(expressionCallback, blockCallback);

        Syntax = Syntax.AddStatements(whileStatementSyntax);

        return this;
    }

    // TODO: Create BooleanExpressionBuilder
    public IBlockBuilder WithDoStatement(
        Func<IBlockBuilder, IBlockBuilder> blockCallback,
        Action<IExpressionBuilder> expressionCallback
    )
    {
        var syntax = DoStatementBuilder.CreateSyntax(blockCallback, expressionCallback);

        Syntax = Syntax.AddStatements(syntax);

        return this;
    }

    public IBlockBuilder WithForStatement(Func<IForStatementBuilder, IForStatementBuilder> forCallback)
    {
        var forStatementSyntax = ForStatementBuilder.CreateSyntax(forCallback);

        Syntax = Syntax.AddStatements(forStatementSyntax);

        return this;
    }


    // TODO: Constant Declaration
    // TODO: Using Declaration
    // TODO: Pattern variable declaration
    // TODO: Deconstruction Declaration

    // Selection Statement
}

public static class BlockBuilderExtensions
{
    public static IBlockBuilder WithLocalDeclaration<T>(this IBlockBuilder builder, string identifier, T literalValue)
    {
        return builder.WithLocalDeclaration(identifier, x => x.AsVar(), x => x.AsLiteral(literalValue));
    }

    public static IBlockBuilder WithLocalDeclaration
    (
        this IBlockBuilder builder,
        string identifier,
        Action<IExpressionBuilder>? expressionCallback = null
    )
    {
        return builder.WithLocalDeclaration(identifier, x => x.AsVar(), expressionCallback);
    }

    public static IBlockBuilder WithLocalFunction<T>(
         this IBlockBuilder builder,
         string identifier,
         Func<ILocalFunctionStatementBuilder, ILocalFunctionStatementBuilder> methodCallback
     )
    {
        return builder.WithLocalFunction(identifier, x => x.AsType<T>(), methodCallback);
    }

    public static IBlockBuilder WithLocalFunction(
         this IBlockBuilder builder,
         string identifier,
         Func<ILocalFunctionStatementBuilder, ILocalFunctionStatementBuilder> methodCallback
     )
    {
        return builder.WithLocalFunction(identifier, x => x.AsVoid(), methodCallback);
    }
}