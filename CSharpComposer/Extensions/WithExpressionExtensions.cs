namespace CSharpComposer;

public static class WithExpressionExtensions
{
    public static TBuilder WithExpression<TBuilder>(this TBuilder builder, string expression)
        where TBuilder : IWithExpression<TBuilder>
    {
        return builder.WithExpression(x => x.ParseExpression(expression));
    }
}
