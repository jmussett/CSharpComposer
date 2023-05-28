using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IConstructorDeclarationBuilder : IMemberDeclarationBuilder<IConstructorDeclarationBuilder>
{
    IConstructorDeclarationBuilder WithParameter(
        string identifier,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    );

    IConstructorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IConstructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
}

public sealed class ConstructorDeclarationBuilder : IConstructorDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public ConstructorDeclarationSyntax Syntax
    {
        get => (ConstructorDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public ConstructorDeclarationBuilder(ConstructorDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Constructor);
    }

    public static ConstructorDeclarationSyntax CreateSyntax(string identifier, Func<IConstructorDeclarationBuilder, IConstructorDeclarationBuilder>? callback = null)
    {
        var syntax = SyntaxFactory.ConstructorDeclaration(identifier)
            .WithBody(SyntaxFactory.Block());

        var builder = new ConstructorDeclarationBuilder(syntax);

        builder = callback is not null
            ? builder.AssertInvoke(callback)
            : builder.AssertInvoke(x => x.WithAccessModifier(MemberAccessModifier.Public));

        return builder.Syntax;
    }

    public IConstructorDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null)
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public IConstructorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithBody(blockSyntax);

        return this;
    }

    public IConstructorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(syntax)
        );

        return this;
    }

    public IConstructorDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IConstructorDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IConstructorDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IConstructorDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}

public static class ConstructorDeclarationBuilderExtensions
{
    public static IConstructorDeclarationBuilder WithParameter<T>(
        this IConstructorDeclarationBuilder builder,
        string identifier,
        Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null
    )
    {
        return builder.WithParameter(identifier, x => x.AsType<T>(), parameterCallback);
    }
}
