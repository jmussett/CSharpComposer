using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IConversionOperatorDeclarationBuilder : IMemberDeclarationBuilder<IConversionOperatorDeclarationBuilder>
{
    IConversionOperatorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IConversionOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IConversionOperatorDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null);
}

public class ConversionOperatorDeclarationBuilder : IConversionOperatorDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public ConversionOperatorDeclarationSyntax Syntax
    {
        get => (ConversionOperatorDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public ConversionOperatorDeclarationBuilder(ConversionOperatorDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Method);
    }

    public static ConversionOperatorDeclarationSyntax CreateSyntax(
        ConversionOperatorType operatorType,
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IConversionOperatorDeclarationBuilder, IConversionOperatorDeclarationBuilder> operatorCallback
    )
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var syntax = SyntaxFactory.ConversionOperatorDeclaration(SyntaxFactory.Token(
            operatorType switch
            {
                ConversionOperatorType.Implicit => SyntaxKind.ImplicitKeyword,
                ConversionOperatorType.Explicit => SyntaxKind.ExplicitKeyword,
                _ => throw new NotSupportedException($"Conversion operator Type '{operatorType}' is not supported"),
            }
        ), typeSyntax);

        var builder = new ConversionOperatorDeclarationBuilder(syntax).AssertInvoke(operatorCallback);

        builder
            .WithAccessModifier(MemberAccessModifier.Public)
            .WithStaticModifier();

        return builder.Syntax;
    }

    // TODO: move methods to BaseMethodDeclarationBuilder
    public IConversionOperatorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithBody(blockSyntax);

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(syntax)
        );

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null)
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IConversionOperatorDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}

public static class ConversionOperatorDeclarationBuilderExtensions
{
    public static IConversionOperatorDeclarationBuilder WithParameter<T>(this IConversionOperatorDeclarationBuilder builder, string identifier, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null)
    {
        return builder.WithParameter(identifier, x => x.AsType<T>(), parameterCallback);
    }
}
