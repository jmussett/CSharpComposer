using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IMethodDeclarationBuilder : IWithExplicitInterfaceSpecifierBuilder<IMethodDeclarationBuilder>, IBaseMethodDeclarationBuilder<IMethodDeclarationBuilder>
{
    IMethodDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback);
    IMethodDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
    IMethodDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback);
    IMethodDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter);
    IMethodDeclarationBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback);
    IMethodDeclarationBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause);
}

public interface IWithMethodDeclarationBuilder<TBuilder>
{
    TBuilder WithMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback);
    TBuilder WithMethodDeclaration(MethodDeclarationSyntax methodDeclarationSyntax);
}

public partial class MethodDeclarationBuilder : IMethodDeclarationBuilder
{
    public MethodDeclarationSyntax Syntax { get; set; }

    public MethodDeclarationBuilder(MethodDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static MethodDeclarationSyntax CreateSyntax(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback)
    {
        var returnTypeSyntax = TypeBuilder.CreateSyntax(returnTypeCallback);
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var parameterListSyntax = SyntaxFactory.ParameterList();
        var syntax = SyntaxFactory.MethodDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), returnTypeSyntax, default(ExplicitInterfaceSpecifierSyntax), identifierToken, default(TypeParameterListSyntax), parameterListSyntax, default(SyntaxList<TypeParameterConstraintClauseSyntax>), null, null, default(SyntaxToken));
        var builder = new MethodDeclarationBuilder(syntax);
        methodDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IMethodDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IMethodDeclarationBuilder WithExpressionBody(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IMethodDeclarationBuilder WithBody(Action<IBlockBuilder> blockCallback)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IMethodDeclarationBuilder WithBody(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IMethodDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IMethodDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IMethodDeclarationBuilder WithExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        var explicitInterfaceSpecifierSyntax = ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifierSyntax);
        return this;
    }

    public IMethodDeclarationBuilder WithExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier)
    {
        Syntax = Syntax.WithExplicitInterfaceSpecifier(explicitInterfaceSpecifier);
        return this;
    }

    public IMethodDeclarationBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback)
    {
        var parameter = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IMethodDeclarationBuilder AddTypeParameter(TypeParameterSyntax parameter)
    {
        Syntax = Syntax.AddTypeParameterListParameters(parameter);
        return this;
    }

    public IMethodDeclarationBuilder AddParameter(string identifier, Action<IParameterBuilder> parameterCallback)
    {
        var parameter = ParameterBuilder.CreateSyntax(identifier, parameterCallback);
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IMethodDeclarationBuilder AddParameter(ParameterSyntax parameter)
    {
        Syntax = Syntax.AddParameterListParameters(parameter);
        return this;
    }

    public IMethodDeclarationBuilder AddConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        var constraintClause = TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }

    public IMethodDeclarationBuilder AddConstraintClause(TypeParameterConstraintClauseSyntax constraintClause)
    {
        Syntax = Syntax.AddConstraintClauses(constraintClause);
        return this;
    }
}