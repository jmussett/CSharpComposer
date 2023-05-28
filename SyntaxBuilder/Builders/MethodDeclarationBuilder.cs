using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

public interface IMethodDeclarationBuilder : IMemberDeclarationBuilder<IMethodDeclarationBuilder>
{
    IMethodDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IMethodDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IMethodDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null);
    IMethodDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null);
}


public sealed class MethodDeclarationBuilder : IMethodDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;
    private bool _includesBody = false;

    public MethodDeclarationSyntax Syntax
    {
        get => (MethodDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public MethodDeclarationBuilder(MethodDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Method);
    }

    public static MethodDeclarationSyntax CreateSyntax(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IMethodDeclarationBuilder, IMethodDeclarationBuilder> methodCallback
    )
    {
        SyntaxValidator.ValidateIdentifier(identifier);

        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var methodSyntax = SyntaxFactory.MethodDeclaration(typeSyntax, identifier);

        var builder = new MethodDeclarationBuilder(methodSyntax).AssertInvoke(methodCallback);

        if (!builder._includesBody)
        {
            builder.Syntax = builder.Syntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        return builder.Syntax;
    }

    public IMethodDeclarationBuilder WithTypeParameter(string identifier, Func<ITypeParameterBuilder, ITypeParameterBuilder>? typeParameterCallback = null)
    {
        var syntax = TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);

        Syntax = Syntax.AddTypeParameterListParameters(syntax.TypeParameter);

        if (syntax.ConstraintClause is not null)
        {
            Syntax = Syntax.AddConstraintClauses(syntax.ConstraintClause);
        }

        return this;
    }

    public IMethodDeclarationBuilder WithParameter(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public IMethodDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithBody(blockSyntax);

        _includesBody = true;

        return this;
    }

    public IMethodDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(syntax)
        );

        return this;
    }

    public IMethodDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IMethodDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IMethodDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IMethodDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}

public static class MethodBuilderExtensions
{
    public static IMethodDeclarationBuilder WithParameter(
        this IMethodDeclarationBuilder builder,
        string identifier,
        string typeName,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        return builder.WithParameter(identifier, x => x.AsType(typeName), parameterCallback);
    }
}
