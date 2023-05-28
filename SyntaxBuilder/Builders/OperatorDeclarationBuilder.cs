using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IOperatorDeclarationBuilder : IMemberDeclarationBuilder<IOperatorDeclarationBuilder>
{
    IOperatorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback);
    IOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
    IOperatorDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null);
}

public class OperatorDeclarationBuilder : IOperatorDeclarationBuilder
{
    private MemberDeclarationBuilder _memberBuilder;

    public OperatorDeclarationSyntax Syntax
    {
        get => (OperatorDeclarationSyntax)_memberBuilder.Syntax;
        set => _memberBuilder.Syntax = value;
    }

    public OperatorDeclarationBuilder(OperatorDeclarationSyntax syntax)
    {
        _memberBuilder = new(syntax, AttributeTargets.Method);
    }

    public static OperatorDeclarationSyntax CreateSyntax(Func<ITypeBuilder, ITypeBuilder> typeCallback, OperatorType operatorType, Func<IOperatorDeclarationBuilder, IOperatorDeclarationBuilder> operatorCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var syntax = SyntaxFactory.OperatorDeclaration(typeSyntax, SyntaxFactory.Token(
            operatorType switch
            {
                OperatorType.Plus => SyntaxKind.PlusToken,
                OperatorType.Minus => SyntaxKind.MinusToken,
                OperatorType.Exclamation => SyntaxKind.ExclamationToken,
                OperatorType.Tilde => SyntaxKind.TildeToken,
                OperatorType.PlusPlus => SyntaxKind.PlusPlusToken,
                OperatorType.MinusMinus => SyntaxKind.MinusMinusToken,
                OperatorType.Asterisk => SyntaxKind.AsteriskToken,
                OperatorType.Slash => SyntaxKind.SlashToken,
                OperatorType.Percent => SyntaxKind.PercentToken,
                OperatorType.LessThanLessThan => SyntaxKind.LessThanLessThanToken,
                OperatorType.GreaterThanGreaterThan => SyntaxKind.GreaterThanGreaterThanToken,
                OperatorType.GreaterThanGreaterThanGreaterThan => SyntaxKind.GreaterThanGreaterThanGreaterThanToken,
                OperatorType.Bar => SyntaxKind.BarToken,
                OperatorType.Ampersand => SyntaxKind.AmpersandToken,
                OperatorType.Caret => SyntaxKind.CaretToken,
                OperatorType.EqualsEquals => SyntaxKind.EqualsEqualsToken,
                OperatorType.ExclamationEquals => SyntaxKind.ExclamationEqualsToken,
                OperatorType.LessThan => SyntaxKind.LessThanToken,
                OperatorType.LessThanEquals => SyntaxKind.LessThanEqualsToken,
                OperatorType.GreaterThan => SyntaxKind.GreaterThanToken,
                OperatorType.GreaterThanEquals => SyntaxKind.GreaterThanEqualsToken,
                OperatorType.False => SyntaxKind.FalseKeyword,
                OperatorType.True => SyntaxKind.TrueKeyword,
                OperatorType.Is => SyntaxKind.IsKeyword,
                _ => throw new NotSupportedException($"Operator Type '{operatorType}' is not supported"),
            }
        ));

        var builder = new OperatorDeclarationBuilder(syntax).AssertInvoke(operatorCallback);

        builder
            .WithAccessModifier(MemberAccessModifier.Public)
            .WithStaticModifier();

        return builder.Syntax;
    }

    // TODO: move methods to BaseMethodDeclarationBuilder
    public IOperatorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> blockCallback)
    {
        var blockSyntax = BlockBuilder.CreateSyntax(blockCallback);

        Syntax = Syntax.WithBody(blockSyntax);

        return this;
    }

    public IOperatorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(syntax)
        );

        return this;
    }

    public IOperatorDeclarationBuilder WithParameter(string identifier, Func<ITypeBuilder, ITypeBuilder> typeCallback, Func<IParameterBuilder, IParameterBuilder>? parameterCallback = null)
    {
        var parameterSyntax = ParameterBuilder.CreateSyntax(identifier, typeCallback, parameterCallback);

        Syntax = Syntax.AddParameterListParameters(parameterSyntax);

        return this;
    }

    public IOperatorDeclarationBuilder WithAccessModifier(MemberAccessModifier accessModifier)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAccessModifier(accessModifier));

        return this;
    }

    public IOperatorDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(nameCallback, attributeCallback));

        return this;
    }

    public IOperatorDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithAttribute(type, attributeCallback));

        return this;
    }

    public IOperatorDeclarationBuilder WithStaticModifier()
    {
        _memberBuilder = _memberBuilder.AssertInvoke(x => x.WithStaticModifier());

        return this;
    }
}
