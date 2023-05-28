using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;

namespace SyntaxBuilder.Builders;

public interface IExpressionBuilder
{
    void ParseExpression(string expression);
    void AsDefaultExpression();
    void AsDefaultExpression(string typeName);
    void AsLiteral<T>(T value);
    void AsNullLiteral();
    void AsName(Func<INameBuilder, INameBuilder> nameBuilder);
    void AsAssignment(
        Action<IExpressionBuilder> leftBuilder,
        AssignmentType assignmentType,
        Action<IExpressionBuilder> rightBuilder
    );
    void AsNew(
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IObjectCreationExpressionBuilder, IObjectCreationExpressionBuilder> objectCreationCallback
    );
    void AsMemberAccess(
        Action<IExpressionBuilder> expressionCallback,
        Func<ISimpleNameBuilder, ISimpleNameBuilder> simpleNameCallback
    );
    void AsInvocation(Action<IExpressionBuilder> expressionCallback);
}

public sealed class ExpressionBuilder : IExpressionBuilder
{
    public ExpressionSyntax? Syntax { get; set; }

    public static ExpressionSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var builder = new ExpressionBuilder();

        expressionCallback.Invoke(builder);

        if (builder.Syntax == null)
        {
            throw new InvalidOperationException("No expression was set.");
        }

        return builder.Syntax;
    }

    public void ParseExpression(string expression)
    {
        Syntax = SyntaxFactory.ParseExpression(expression);
    }

    public void AsLiteral<T>(T value)
    {
        Syntax = value switch
        {
            byte byteValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(byteValue)
            ),
            sbyte sbyteValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(sbyteValue)
            ),
            short shortValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(shortValue)
            ),
            ushort ushortValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(ushortValue)
            ),
            int intValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(intValue)
            ),
            uint uintValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(uintValue)
            ),
            long longValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(longValue)
            ),
            ulong ulongValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(ulongValue)
            ),
            double doubleValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(doubleValue)
            ),
            decimal decimalValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(decimalValue)
            ),
            float floatValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(floatValue)
            ),
            string stringValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxFactory.Literal(stringValue)
            ),
            char charValue => SyntaxFactory.LiteralExpression(
                SyntaxKind.CharacterLiteralExpression,
                SyntaxFactory.Literal(charValue)
            ),
            bool boolValue => SyntaxFactory.LiteralExpression(
                boolValue ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression
            ),
            Enum enumValue => SyntaxFactory.ParseExpression(
                $"{enumValue.GetType().Name}.{enumValue}"
            ),
            _ => throw new NotSupportedException($"Type '{typeof(T).Name}' is not a valid literal type")
        };
    }

    public void AsNullLiteral()
    {
        Syntax = SyntaxFactory.LiteralExpression(
            SyntaxKind.NullLiteralExpression
        );
    }

    public void AsDefaultExpression()
    {
        Syntax = SyntaxFactory.DefaultExpression(null!);
    }

    public void AsDefaultExpression(string typeName)
    {
        // TODO: Validate names.

        Syntax = SyntaxFactory.DefaultExpression(
            SyntaxFactory.ParseTypeName(typeName)
        );
    }

    public void AsAssignment(
        Action<IExpressionBuilder> leftCallback,
        AssignmentType assignmentType,
        Action<IExpressionBuilder> rightCallback
    )
    {
        var leftExpression = CreateSyntax(leftCallback);
        var rightExpression = CreateSyntax(rightCallback);

        Syntax = SyntaxFactory.AssignmentExpression(
            assignmentType switch
            {
                AssignmentType.Equal => SyntaxKind.SimpleAssignmentExpression,
                AssignmentType.Add => SyntaxKind.AddAssignmentExpression,
                AssignmentType.Subtract => SyntaxKind.SubtractAssignmentExpression,
                AssignmentType.Multiply => SyntaxKind.MultiplyAssignmentExpression,
                AssignmentType.Divide => SyntaxKind.DivideAssignmentExpression,
                AssignmentType.Modulo => SyntaxKind.ModuloAssignmentExpression,
                AssignmentType.And => SyntaxKind.AndAssignmentExpression,
                AssignmentType.ExclusiveOr => SyntaxKind.ExclusiveOrAssignmentExpression,
                AssignmentType.Or => SyntaxKind.OrAssignmentExpression,
                AssignmentType.LeftShift => SyntaxKind.LeftShiftAssignmentExpression,
                AssignmentType.RightShift => SyntaxKind.RightShiftAssignmentExpression,
                AssignmentType.UnsignedRightShift => SyntaxKind.UnsignedRightShiftAssignmentExpression,
                AssignmentType.Coalesce => SyntaxKind.CoalesceAssignmentExpression,
                _ => throw new NotSupportedException($"{assignmentType} is not supported."),
            },
            leftExpression,
            rightExpression
        );
    }

    public void AsName(Func<INameBuilder, INameBuilder> nameBuilder)
    {
        Syntax = NameBuilder.CreateSyntax(nameBuilder);
    }

    // TODO: ImplicitObjectCreationExpression (implicit type)
    public void AsNew(
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IObjectCreationExpressionBuilder, IObjectCreationExpressionBuilder> objectCreationCallback
    )
    {
        Syntax = ObjectCreationExpressionBuilder.CreateSyntax(typeCallback, objectCreationCallback);
    }

    public void AsMemberAccess(
        Action<IExpressionBuilder> expressionCallback,
        Func<ISimpleNameBuilder, ISimpleNameBuilder> simpleNameCallback
    )
    {
        Syntax = MemberAccessExpressionBuilder.CreateSyntax(expressionCallback, simpleNameCallback);
    }

    public void AsInvocation(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = InvocationExpressionBuilder.CreateSyntax(expressionCallback);
    }

    // TODO: Assignment Expression
    // TODO: Increment / Decremement Expression
    // TODO: Method Call Expression
    // TODO: Object Creation Expression
    // TODO: Anynymous Object Creation
    // TODO: Array Creation Expression
    // TODO: Element Access Expression
    // TODO: Member Access Expression
    // TODO: Compound Assignment Expression
    // TODO: Lambda Expression
    // TODO: Await Expression
}

public static class ExpressionBuilderExtensions
{
    public static void AsName(this IExpressionBuilder builder, string name)
    {
        builder.AsName(x => x.ParseName(name));
    }
}