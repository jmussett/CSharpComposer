using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IExpressionBuilder
{
    void AsIdentifierName(string identifier);
    void AsGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback);
    void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback);
    void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback);
    void AsPredefinedType(PredefinedTypeKeyword predefinedTypeKeyword);
    void AsArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback);
    void AsPointerType(Action<ITypeBuilder> elementTypeCallback);
    void AsFunctionPointerType(Action<IFunctionPointerTypeBuilder> functionPointerTypeCallback);
    void AsNullableType(Action<ITypeBuilder> elementTypeCallback);
    void AsTupleType(Action<ITupleTypeBuilder> tupleTypeCallback);
    void AsOmittedTypeArgument();
    void AsRefType(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder> refTypeCallback);
    void AsScopedType(Action<ITypeBuilder> typeCallback);
    void AsParenthesizedExpression(Action<IExpressionBuilder> expressionCallback);
    void AsTupleExpression(Action<ITupleExpressionBuilder> tupleExpressionCallback);
    void AsPrefixUnaryExpression(PrefixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback);
    void AsAwaitExpression(Action<IExpressionBuilder> expressionCallback);
    void AsPostfixUnaryExpression(PostfixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback);
    void AsMemberAccessExpression(MemberAccessExpressionKind kind, Action<IExpressionBuilder> expressionCallback, Action<ISimpleNameBuilder> nameCallback);
    void AsConditionalAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback);
    void AsMemberBindingExpression(Action<ISimpleNameBuilder> nameCallback);
    void AsElementBindingExpression(Action<IElementBindingExpressionBuilder> elementBindingExpressionCallback);
    void AsRangeExpression(Action<IRangeExpressionBuilder> rangeExpressionCallback);
    void AsImplicitElementAccess(Action<IImplicitElementAccessBuilder> implicitElementAccessCallback);
    void AsBinaryExpression(BinaryExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback);
    void AsAssignmentExpression(AssignmentExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback);
    void AsConditionalExpression(Action<IExpressionBuilder> conditionCallback, Action<IExpressionBuilder> whenTrueCallback, Action<IExpressionBuilder> whenFalseCallback);
    void AsThisExpression();
    void AsBaseExpression();
    void AsLiteralExpression(Action<ILiteralExpressionBuilder> literalExpressionCallback);
    void AsMakeRefExpression(Action<IExpressionBuilder> expressionCallback);
    void AsRefTypeExpression(Action<IExpressionBuilder> expressionCallback);
    void AsRefValueExpression(Action<IExpressionBuilder> expressionCallback, Action<ITypeBuilder> typeCallback);
    void AsCheckedExpression(CheckedExpressionKind kind, Action<IExpressionBuilder> expressionCallback);
    void AsDefaultExpression(Action<ITypeBuilder> typeCallback);
    void AsTypeOfExpression(Action<ITypeBuilder> typeCallback);
    void AsSizeOfExpression(Action<ITypeBuilder> typeCallback);
    void AsInvocationExpression(Action<IExpressionBuilder> expressionCallback, Action<IInvocationExpressionBuilder> invocationExpressionCallback);
    void AsElementAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IElementAccessExpressionBuilder> elementAccessExpressionCallback);
    void AsDeclarationExpression(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback);
    void AsCastExpression(Action<ITypeBuilder> typeCallback, Action<IExpressionBuilder> expressionCallback);
    void AsAnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback);
    void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback);
    void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder> parenthesizedLambdaExpressionCallback);
    void AsRefExpression(Action<IExpressionBuilder> expressionCallback);
    void AsInitializerExpression(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback);
    void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder> implicitObjectCreationExpressionCallback);
    void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder> objectCreationExpressionCallback);
    void AsWithExpression(Action<IExpressionBuilder> expressionCallback, InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback);
    void AsAnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback);
    void AsArrayCreationExpression(Action<ITypeBuilder> typeElementTypeCallback, Action<IArrayTypeBuilder> typeArrayTypeCallback, Action<IArrayCreationExpressionBuilder> arrayCreationExpressionCallback);
    void AsImplicitArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback, Action<IImplicitArrayCreationExpressionBuilder> implicitArrayCreationExpressionCallback);
    void AsStackAllocArrayCreationExpression(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder> stackAllocArrayCreationExpressionCallback);
    void AsImplicitStackAllocArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback);
    void AsQueryExpression(string fromClauseIdentifier, Action<IExpressionBuilder> fromClauseExpressionCallback, Action<IFromClauseBuilder> fromClauseFromClauseCallback, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback);
    void AsOmittedArraySizeExpression();
    void AsInterpolatedStringExpression(InterpolatedStringExpressionStringStartToken interpolatedStringExpressionStringStartToken, InterpolatedStringExpressionStringEndToken interpolatedStringExpressionStringEndToken, Action<IInterpolatedStringExpressionBuilder> interpolatedStringExpressionCallback);
    void AsIsPatternExpression(Action<IExpressionBuilder> expressionCallback, Action<IPatternBuilder> patternCallback);
    void AsThrowExpression(Action<IExpressionBuilder> expressionCallback);
    void AsSwitchExpression(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder> switchExpressionCallback);
}

public interface IWithExpressionBuilder<TBuilder>
{
    TBuilder WithExpression(Action<IExpressionBuilder> expressionCallback);
    TBuilder WithExpression(ExpressionSyntax expressionSyntax);
}

public partial class ExpressionBuilder : IExpressionBuilder
{
    public ExpressionSyntax? Syntax { get; set; }

    public static ExpressionSyntax CreateSyntax(Action<IExpressionBuilder> callback)
    {
        var builder = new ExpressionBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("ExpressionSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsIdentifierName(string identifier)
    {
        Syntax = IdentifierNameBuilder.CreateSyntax(identifier);
    }

    public void AsGenericName(string identifier, Action<IGenericNameBuilder> genericNameCallback)
    {
        Syntax = GenericNameBuilder.CreateSyntax(identifier, genericNameCallback);
    }

    public void AsQualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback)
    {
        Syntax = QualifiedNameBuilder.CreateSyntax(leftCallback, rightCallback);
    }

    public void AsAliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback)
    {
        Syntax = AliasQualifiedNameBuilder.CreateSyntax(aliasIdentifier, nameCallback);
    }

    public void AsPredefinedType(PredefinedTypeKeyword predefinedTypeKeyword)
    {
        Syntax = PredefinedTypeBuilder.CreateSyntax(predefinedTypeKeyword);
    }

    public void AsArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder> arrayTypeCallback)
    {
        Syntax = ArrayTypeBuilder.CreateSyntax(elementTypeCallback, arrayTypeCallback);
    }

    public void AsPointerType(Action<ITypeBuilder> elementTypeCallback)
    {
        Syntax = PointerTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public void AsFunctionPointerType(Action<IFunctionPointerTypeBuilder> functionPointerTypeCallback)
    {
        Syntax = FunctionPointerTypeBuilder.CreateSyntax(functionPointerTypeCallback);
    }

    public void AsNullableType(Action<ITypeBuilder> elementTypeCallback)
    {
        Syntax = NullableTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public void AsTupleType(Action<ITupleTypeBuilder> tupleTypeCallback)
    {
        Syntax = TupleTypeBuilder.CreateSyntax(tupleTypeCallback);
    }

    public void AsOmittedTypeArgument()
    {
        Syntax = OmittedTypeArgumentBuilder.CreateSyntax();
    }

    public void AsRefType(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder> refTypeCallback)
    {
        Syntax = RefTypeBuilder.CreateSyntax(typeCallback, refTypeCallback);
    }

    public void AsScopedType(Action<ITypeBuilder> typeCallback)
    {
        Syntax = ScopedTypeBuilder.CreateSyntax(typeCallback);
    }

    public void AsParenthesizedExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = ParenthesizedExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsTupleExpression(Action<ITupleExpressionBuilder> tupleExpressionCallback)
    {
        Syntax = TupleExpressionBuilder.CreateSyntax(tupleExpressionCallback);
    }

    public void AsPrefixUnaryExpression(PrefixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        Syntax = PrefixUnaryExpressionBuilder.CreateSyntax(kind, operandCallback);
    }

    public void AsAwaitExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = AwaitExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsPostfixUnaryExpression(PostfixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        Syntax = PostfixUnaryExpressionBuilder.CreateSyntax(kind, operandCallback);
    }

    public void AsMemberAccessExpression(MemberAccessExpressionKind kind, Action<IExpressionBuilder> expressionCallback, Action<ISimpleNameBuilder> nameCallback)
    {
        Syntax = MemberAccessExpressionBuilder.CreateSyntax(kind, expressionCallback, nameCallback);
    }

    public void AsConditionalAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback)
    {
        Syntax = ConditionalAccessExpressionBuilder.CreateSyntax(expressionCallback, whenNotNullCallback);
    }

    public void AsMemberBindingExpression(Action<ISimpleNameBuilder> nameCallback)
    {
        Syntax = MemberBindingExpressionBuilder.CreateSyntax(nameCallback);
    }

    public void AsElementBindingExpression(Action<IElementBindingExpressionBuilder> elementBindingExpressionCallback)
    {
        Syntax = ElementBindingExpressionBuilder.CreateSyntax(elementBindingExpressionCallback);
    }

    public void AsRangeExpression(Action<IRangeExpressionBuilder> rangeExpressionCallback)
    {
        Syntax = RangeExpressionBuilder.CreateSyntax(rangeExpressionCallback);
    }

    public void AsImplicitElementAccess(Action<IImplicitElementAccessBuilder> implicitElementAccessCallback)
    {
        Syntax = ImplicitElementAccessBuilder.CreateSyntax(implicitElementAccessCallback);
    }

    public void AsBinaryExpression(BinaryExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        Syntax = BinaryExpressionBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public void AsAssignmentExpression(AssignmentExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        Syntax = AssignmentExpressionBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public void AsConditionalExpression(Action<IExpressionBuilder> conditionCallback, Action<IExpressionBuilder> whenTrueCallback, Action<IExpressionBuilder> whenFalseCallback)
    {
        Syntax = ConditionalExpressionBuilder.CreateSyntax(conditionCallback, whenTrueCallback, whenFalseCallback);
    }

    public void AsThisExpression()
    {
        Syntax = ThisExpressionBuilder.CreateSyntax();
    }

    public void AsBaseExpression()
    {
        Syntax = BaseExpressionBuilder.CreateSyntax();
    }

    public void AsLiteralExpression(Action<ILiteralExpressionBuilder> literalExpressionCallback)
    {
        Syntax = LiteralExpressionBuilder.CreateSyntax(literalExpressionCallback);
    }

    public void AsMakeRefExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = MakeRefExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsRefTypeExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = RefTypeExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsRefValueExpression(Action<IExpressionBuilder> expressionCallback, Action<ITypeBuilder> typeCallback)
    {
        Syntax = RefValueExpressionBuilder.CreateSyntax(expressionCallback, typeCallback);
    }

    public void AsCheckedExpression(CheckedExpressionKind kind, Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = CheckedExpressionBuilder.CreateSyntax(kind, expressionCallback);
    }

    public void AsDefaultExpression(Action<ITypeBuilder> typeCallback)
    {
        Syntax = DefaultExpressionBuilder.CreateSyntax(typeCallback);
    }

    public void AsTypeOfExpression(Action<ITypeBuilder> typeCallback)
    {
        Syntax = TypeOfExpressionBuilder.CreateSyntax(typeCallback);
    }

    public void AsSizeOfExpression(Action<ITypeBuilder> typeCallback)
    {
        Syntax = SizeOfExpressionBuilder.CreateSyntax(typeCallback);
    }

    public void AsInvocationExpression(Action<IExpressionBuilder> expressionCallback, Action<IInvocationExpressionBuilder> invocationExpressionCallback)
    {
        Syntax = InvocationExpressionBuilder.CreateSyntax(expressionCallback, invocationExpressionCallback);
    }

    public void AsElementAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IElementAccessExpressionBuilder> elementAccessExpressionCallback)
    {
        Syntax = ElementAccessExpressionBuilder.CreateSyntax(expressionCallback, elementAccessExpressionCallback);
    }

    public void AsDeclarationExpression(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        Syntax = DeclarationExpressionBuilder.CreateSyntax(typeCallback, designationCallback);
    }

    public void AsCastExpression(Action<ITypeBuilder> typeCallback, Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = CastExpressionBuilder.CreateSyntax(typeCallback, expressionCallback);
    }

    public void AsAnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback)
    {
        Syntax = AnonymousMethodExpressionBuilder.CreateSyntax(blockBlockCallback, anonymousMethodExpressionCallback);
    }

    public void AsSimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback)
    {
        Syntax = SimpleLambdaExpressionBuilder.CreateSyntax(parameterIdentifier, parameterParameterCallback, simpleLambdaExpressionCallback);
    }

    public void AsParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder> parenthesizedLambdaExpressionCallback)
    {
        Syntax = ParenthesizedLambdaExpressionBuilder.CreateSyntax(parenthesizedLambdaExpressionCallback);
    }

    public void AsRefExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = RefExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsInitializerExpression(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder> initializerExpressionCallback)
    {
        Syntax = InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
    }

    public void AsImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder> implicitObjectCreationExpressionCallback)
    {
        Syntax = ImplicitObjectCreationExpressionBuilder.CreateSyntax(implicitObjectCreationExpressionCallback);
    }

    public void AsObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder> objectCreationExpressionCallback)
    {
        Syntax = ObjectCreationExpressionBuilder.CreateSyntax(typeCallback, objectCreationExpressionCallback);
    }

    public void AsWithExpression(Action<IExpressionBuilder> expressionCallback, InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        Syntax = WithExpressionBuilder.CreateSyntax(expressionCallback, initializerKind, initializerInitializerExpressionCallback);
    }

    public void AsAnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder> anonymousObjectCreationExpressionCallback)
    {
        Syntax = AnonymousObjectCreationExpressionBuilder.CreateSyntax(anonymousObjectCreationExpressionCallback);
    }

    public void AsArrayCreationExpression(Action<ITypeBuilder> typeElementTypeCallback, Action<IArrayTypeBuilder> typeArrayTypeCallback, Action<IArrayCreationExpressionBuilder> arrayCreationExpressionCallback)
    {
        Syntax = ArrayCreationExpressionBuilder.CreateSyntax(typeElementTypeCallback, typeArrayTypeCallback, arrayCreationExpressionCallback);
    }

    public void AsImplicitArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback, Action<IImplicitArrayCreationExpressionBuilder> implicitArrayCreationExpressionCallback)
    {
        Syntax = ImplicitArrayCreationExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback, implicitArrayCreationExpressionCallback);
    }

    public void AsStackAllocArrayCreationExpression(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder> stackAllocArrayCreationExpressionCallback)
    {
        Syntax = StackAllocArrayCreationExpressionBuilder.CreateSyntax(typeCallback, stackAllocArrayCreationExpressionCallback);
    }

    public void AsImplicitStackAllocArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        Syntax = ImplicitStackAllocArrayCreationExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback);
    }

    public void AsQueryExpression(string fromClauseIdentifier, Action<IExpressionBuilder> fromClauseExpressionCallback, Action<IFromClauseBuilder> fromClauseFromClauseCallback, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        Syntax = QueryExpressionBuilder.CreateSyntax(fromClauseIdentifier, fromClauseExpressionCallback, fromClauseFromClauseCallback, bodySelectOrGroupCallback, bodyQueryBodyCallback);
    }

    public void AsOmittedArraySizeExpression()
    {
        Syntax = OmittedArraySizeExpressionBuilder.CreateSyntax();
    }

    public void AsInterpolatedStringExpression(InterpolatedStringExpressionStringStartToken interpolatedStringExpressionStringStartToken, InterpolatedStringExpressionStringEndToken interpolatedStringExpressionStringEndToken, Action<IInterpolatedStringExpressionBuilder> interpolatedStringExpressionCallback)
    {
        Syntax = InterpolatedStringExpressionBuilder.CreateSyntax(interpolatedStringExpressionStringStartToken, interpolatedStringExpressionStringEndToken, interpolatedStringExpressionCallback);
    }

    public void AsIsPatternExpression(Action<IExpressionBuilder> expressionCallback, Action<IPatternBuilder> patternCallback)
    {
        Syntax = IsPatternExpressionBuilder.CreateSyntax(expressionCallback, patternCallback);
    }

    public void AsThrowExpression(Action<IExpressionBuilder> expressionCallback)
    {
        Syntax = ThrowExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public void AsSwitchExpression(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder> switchExpressionCallback)
    {
        Syntax = SwitchExpressionBuilder.CreateSyntax(governingExpressionCallback, switchExpressionCallback);
    }
}