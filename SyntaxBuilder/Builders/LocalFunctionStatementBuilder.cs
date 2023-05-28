using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface ILocalFunctionStatementBuilder
{
    ILocalFunctionStatementBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    ILocalFunctionStatementBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null);
    ILocalFunctionStatementBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null);
}

public class LocalFunctionStatementBuilder : ILocalFunctionStatementBuilder
{
    public StatementBuilder _statementBuilder;

    public LocalFunctionStatementSyntax Syntax
    {
        get => (LocalFunctionStatementSyntax)_statementBuilder.Syntax;
        set => _statementBuilder.Syntax = value;
    }

    public LocalFunctionStatementBuilder(LocalFunctionStatementSyntax syntax)
    {
        _statementBuilder = new StatementBuilder(syntax);
    }

    public static LocalFunctionStatementSyntax CreateSyntax(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<ILocalFunctionStatementBuilder, ILocalFunctionStatementBuilder> localFunctionCallback
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var localFunctionSyntax = SyntaxFactory.LocalFunctionStatement(typeSyntax, identifier)
            .WithBody(SyntaxFactory.Block());

        var builder = new LocalFunctionStatementBuilder(localFunctionSyntax).AssertInvoke(localFunctionCallback);

        return builder.Syntax;

    }

    public ILocalFunctionStatementBuilder WithTypeParameter(
        string identifier,
        Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null
    )
    {
        var syntax = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);

        Syntax = Syntax.AddTypeParameterListParameters(syntax.TypeParameter);

        if (syntax.ConstraintClause is not null)
        {
            Syntax = Syntax.AddConstraintClauses(syntax.ConstraintClause);
        }

        return this;
    }



    public ILocalFunctionStatementBuilder WithParameter(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public ILocalFunctionStatementBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var syntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithBody(syntax);

        return this;
    }

    public ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(expressionSyntax)
        );

        return this;
    }
}
