using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public class ArgumentBuilder
{
    // TODO: ref / in / out
    // TODO: name: 
     
    public static ArgumentSyntax CreateSyntax(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionSyntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        return SyntaxFactory.Argument(expressionSyntax);
    }
}