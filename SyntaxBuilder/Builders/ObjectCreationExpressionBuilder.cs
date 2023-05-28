using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SyntaxBuilder.Builders;

public interface IObjectCreationExpressionBuilder
{
    IObjectCreationExpressionBuilder WithArgument(Action<IExpressionBuilder> expressionCallback);
}

public class ObjectCreationExpressionBuilder : IObjectCreationExpressionBuilder
{
    public ObjectCreationExpressionSyntax Syntax { get; set; }

    public ObjectCreationExpressionBuilder(ObjectCreationExpressionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ObjectCreationExpressionSyntax CreateSyntax(
        Func<ITypeBuilder, ITypeBuilder> typeCallback,
        Func<IObjectCreationExpressionBuilder, IObjectCreationExpressionBuilder> objectCreationCallback
    )
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);

        var syntax = SyntaxFactory.ObjectCreationExpression(typeSyntax);

        var builder = new ObjectCreationExpressionBuilder(syntax).AssertInvoke(objectCreationCallback);

        return builder.Syntax;
    }

    public IObjectCreationExpressionBuilder WithArgument(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = Syntax.AddArgumentListArguments(
            ArgumentBuilder.CreateSyntax(expressionCallback)
        );

        return this;
    }

    // TODO: Initializer
}