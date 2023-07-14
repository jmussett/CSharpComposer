using System;

namespace CSharpComposer;

public static class ParseExtensions
{
    public static TBuilder AddAttribute<TBuilder>(this TBuilder builder, string name, Action<IAttributeBuilder>? attributeCallback = null)
        where TBuilder : IAddAttribute<TBuilder>
    {
        return builder.AddAttribute(x => x.ParseName(name), attributeCallback);
    }

    public static TBuilder WithExplicitInterfaceSpecifier<TBuilder>(this TBuilder builder, string name)
        where TBuilder : IWithExplicitInterfaceSpecifier<TBuilder>
    {
        return builder.WithExplicitInterfaceSpecifier(x => x.ParseName(name));
    }

    public static ICatchClauseBuilder WithDeclaration(this ICatchClauseBuilder builder, string typeName, Action<ICatchDeclarationBuilder>? catchDeclarationCallback = null)
    {
        return builder.WithDeclaration(x => x.ParseTypeName(typeName), catchDeclarationCallback);
    }

    public static IConversionOperatorMemberCrefBuilder AddCrefParameter(this IConversionOperatorMemberCrefBuilder builder, string typeName, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        return builder.AddCrefParameter(x => x.ParseTypeName(typeName), crefParameterCallback);
    }

    public static IForStatementBuilder WithDeclaration(this IForStatementBuilder builder, string typeName, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null)
    {
        return builder.WithDeclaration(x => x.ParseTypeName(typeName), variableDeclarationCallback);
    }

    public static IFunctionPointerTypeBuilder AddFunctionPointerParameter(this IFunctionPointerTypeBuilder builder, string typeName, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null)
    {
        return builder.AddFunctionPointerParameter(x => x.ParseTypeName(typeName), functionPointerParameterCallback);
    }

    public static IIndexerMemberCrefBuilder AddCrefParameter(this IIndexerMemberCrefBuilder builder, string typeName, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        return builder.AddCrefParameter(x => x.ParseTypeName(typeName), crefParameterCallback);
    }

    public static INameMemberCrefBuilder AddCrefParameter(this INameMemberCrefBuilder builder, string typeName, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        return builder.AddCrefParameter(x => x.ParseTypeName(typeName), crefParameterCallback);
    }

    public static IOperatorMemberCrefBuilder AddCrefParameter(this IOperatorMemberCrefBuilder builder, string typeName, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        return builder.AddCrefParameter(x => x.ParseTypeName(typeName), crefParameterCallback);
    }

    public static IParenthesizedLambdaExpressionBuilder WithReturnType(this IParenthesizedLambdaExpressionBuilder builder, string returnTypeName)
    {
        return builder.WithReturnType(x => x.ParseTypeName(returnTypeName));
    }

    public static ITupleTypeBuilder AddTupleElement(this ITupleTypeBuilder builder, string typeName, Action<ITupleElementBuilder>? tupleElementCallback = null)
    {
        return builder.AddTupleElement(x => x.ParseTypeName(typeName), tupleElementCallback);
    }

    public static TBuilder WithType<TBuilder>(this TBuilder builder, string typeName)
        where TBuilder : IWithType<TBuilder>
    {
        return builder.WithType(x => x.ParseTypeName(typeName));
    }

    public static TBuilder AddType<TBuilder>(this TBuilder builder, string typeName)
        where TBuilder : IAddType<TBuilder>
    {
        return builder.AddType(x => x.ParseTypeName(typeName));
    }

    public static IUsingStatementBuilder WithDeclaration(this IUsingStatementBuilder builder, string typeName, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null)
    {
        return builder.WithDeclaration(x => x.ParseTypeName(typeName), variableDeclarationCallback);
    }

    public static IAccessorDeclarationBuilder WithExpressionBody(this IAccessorDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IAnonymousObjectCreationExpressionBuilder AddInitializerAnonymousObjectMemberDeclarator(this IAnonymousObjectCreationExpressionBuilder builder, string expression, Action<IAnonymousObjectMemberDeclaratorBuilder>? anonymousObjectMemberDeclaratorCallback = null)
    {
        return builder.AddInitializerAnonymousObjectMemberDeclarator(x => x.ParseExpression(expression), anonymousObjectMemberDeclaratorCallback);
    }

    public static TBuilder AddArgument<TBuilder>(this TBuilder builder, string expression, Action<IArgumentBuilder>? argumentCallback = null)
        where TBuilder : IAddArgument<TBuilder>
    {
        return builder.AddArgument(x => x.ParseExpression(expression), argumentCallback);
    }

    public static IArrayRankSpecifierBuilder AddSizeExpression(this IArrayRankSpecifierBuilder builder, string sizeCallback)
    {
        return builder.AddSizeExpression(x => x.ParseExpression(sizeCallback));
    }

    public static IAttributeBuilder AddAttributeArgument(this IAttributeBuilder builder, string expression, Action<IAttributeArgumentBuilder>? attributeArgumentCallback = null)
    {
        return builder.AddAttributeArgument(x => x.ParseExpression(expression), attributeArgumentCallback);
    }

    public static ICatchClauseBuilder WithFilter(this ICatchClauseBuilder builder, string filterExpression)
    {
        return builder.WithFilter(x => x.ParseExpression(filterExpression));
    }

    public static IConstructorDeclarationBuilder WithExpressionBody(this IConstructorDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IConversionOperatorDeclarationBuilder WithExpressionBody(this IConversionOperatorDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IDestructorDeclarationBuilder WithExpressionBody(this IDestructorDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IForStatementBuilder AddInitializerExpression(this IForStatementBuilder builder, string initializer)
    {
        return builder.AddInitializerExpression(x => x.ParseExpression(initializer));
    }

    public static IForStatementBuilder WithCondition(this IForStatementBuilder builder, string condition)
    {
        return builder.WithCondition(x => x.ParseExpression(condition));
    }

    public static IIndexerDeclarationBuilder WithExpressionBody(this IIndexerDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IInterpolationBuilder WithAlignmentClause(this IInterpolationBuilder builder, string value)
    {
        return builder.WithAlignmentClause(x => x.ParseExpression(value));
    }

    public static ILocalFunctionStatementBuilder WithExpressionBody(this ILocalFunctionStatementBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IMethodDeclarationBuilder WithExpressionBody(this IMethodDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IOperatorDeclarationBuilder WithExpressionBody(this IOperatorDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IParenthesizedLambdaExpressionBuilder WithExpressionBody(this IParenthesizedLambdaExpressionBuilder builder, string expressionBody)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expressionBody));
    }

    public static IPragmaWarningDirectiveTriviaBuilder AddErrorCodeExpression(this IPragmaWarningDirectiveTriviaBuilder builder, string errorCode)
    {
        return builder.AddErrorCodeExpression(x => x.ParseExpression(errorCode));
    }

    public static IPropertyDeclarationBuilder WithExpressionBody(this IPropertyDeclarationBuilder builder, string expression)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expression));
    }

    public static IPropertyDeclarationBuilder WithInitializer(this IPropertyDeclarationBuilder builder, string value)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(value));
    }

    public static IRangeExpressionBuilder WithLeftOperand(this IRangeExpressionBuilder builder, string leftOperand)
    {
        return builder.WithLeftOperand(x => x.ParseExpression(leftOperand));
    }

    public static IRangeExpressionBuilder WithRightOperand(this IRangeExpressionBuilder builder, string rightOperand)
    {
        return builder.WithRightOperand(x => x.ParseExpression(rightOperand));
    }

    public static ISimpleLambdaExpressionBuilder WithExpressionBody(this ISimpleLambdaExpressionBuilder builder, string expressionBody)
    {
        return builder.WithExpressionBody(x => x.ParseExpression(expressionBody));
    }

    public static IVariableDeclaratorBuilder WithInitializer(this IVariableDeclaratorBuilder builder, string value)
    {
        return builder.WithInitializer(x => x.ParseExpression(value));
    }

    public static TBuilder WithWhenClause<TBuilder>(this TBuilder builder, string condition)
        where TBuilder : IWithWhenClause<TBuilder>
    {
        return builder.WithWhenClause(x => x.ParseExpression(condition));
    }

    public static IParameterBuilder WithDefault(this IParameterBuilder builder, string value)
    {
        return builder.WithDefault(x => x.ParseExpression(value));
    }

    public static TBuilder AddStatement<TBuilder>(this TBuilder builder, string statement)
        where TBuilder : IAddStatement<TBuilder>
    {
        return builder.AddStatement(x => x.ParseStatement(statement));
    }

    public static IIfStatementBuilder WithElse(this IIfStatementBuilder builder, string statement)
    {
        return builder.WithElse(x => x.ParseStatement(statement));
    }
}
