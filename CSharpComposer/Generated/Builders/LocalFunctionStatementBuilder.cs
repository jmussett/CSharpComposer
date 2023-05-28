using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILocalFunctionStatementBuilder : IStatementBuilder<ILocalFunctionStatementBuilder>
{
    ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    ILocalFunctionStatementBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody);
    ILocalFunctionStatementBuilder WithBody(Action<IBlockBuilder> blockCallback);
    ILocalFunctionStatementBuilder WithBody(BlockSyntax body);
    ILocalFunctionStatementBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback);
    ILocalFunctionStatementBuilder AddTypeParameter(TypeParameterSyntax parameter);
    ILocalFunctionStatementBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback);
    ILocalFunctionStatementBuilder AddParameter(ParameterSyntax parameter);
    ILocalFunctionStatementBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback);
    ILocalFunctionStatementBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause);
}

public interface IWithLocalFunctionStatementBuilder<TBuilder>
{
    TBuilder WithLocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder> localFunctionStatementCallback);
    TBuilder WithLocalFunctionStatement(LocalFunctionStatementSyntax localFunctionStatementSyntax);
}

public partial class LocalFunctionStatementBuilder : ILocalFunctionStatementBuilder
{
    public LocalFunctionStatementSyntax Syntax { get; set; }

    public LocalFunctionStatementBuilder(LocalFunctionStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LocalFunctionStatementSyntax CreateSyntax(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder> localFunctionStatementCallback)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.LocalFunctionStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), returnTypeSyntax, identifierToken, default(TypeParameterListSyntax), parameterListSyntax, default(SyntaxList<TypeParameterConstraintClauseSyntax>), null, null, default(SyntaxToken));
        var builder = new LocalFunctionStatementBuilder(syntax);
        localFunctionStatementCallback(builder);
        return builder.Syntax;
    }

    public ILocalFunctionStatementBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public ILocalFunctionStatementBuilder WithBody(Action<IBlockBuilder> blockCallback)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public ILocalFunctionStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public ILocalFunctionStatementBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public ILocalFunctionStatementBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }
}