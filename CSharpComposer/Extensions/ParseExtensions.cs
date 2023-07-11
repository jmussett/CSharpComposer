using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;

public static class ParseExtensions
{
    public static TBuilder AddAttribute<TBuilder>(this TBuilder builder, string name, Action<IAttributeBuilder>? attributeCallback = null)
        where TBuilder : IAddAttribute<TBuilder>
    {
        return builder.AddAttribute(x => x.ParseName(name), attributeCallback);
    }

    public static TBuilder AddUsingDirective<TBuilder>(this TBuilder builder, string name, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
        where TBuilder : IBaseNamespaceDeclarationBuilder<TBuilder>
    {
        return builder.AddUsingDirective(x => x.ParseName(name), usingDirectiveCallback);
    }

    public static ICompilationUnitBuilder AddUsingDirective(this ICompilationUnitBuilder builder, string name, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
    {
        return builder.AddUsingDirective(x => x.ParseName(name), usingDirectiveCallback);
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
}