using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public class MemberAccessExpressionBuilder
{
    public static MemberAccessExpressionSyntax CreateSyntax(
        Action<IExpressionBuilder> expressionCallback,
        Func<ISimpleNameBuilder, ISimpleNameBuilder> simpleNameCallback
    )
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);
        var simpleNameSyntax = SimpleNameBuilder.CreateSyntax(simpleNameCallback);

        // TODO: Pointer Access Expression
        return SyntaxFactory.MemberAccessExpression(
           SyntaxKind.SimpleMemberAccessExpression,
           expressionSyntax,
           simpleNameSyntax
        );
    }
}
