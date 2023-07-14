using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public static partial class CSharpFactory
{
    public static NameSyntax Name(Action<INameBuilder> callback)
    {
        return NameBuilder.CreateSyntax(callback);
    }

    public static SimpleNameSyntax SimpleName(Action<ISimpleNameBuilder> callback)
    {
        return SimpleNameBuilder.CreateSyntax(callback);
    }

    public static IdentifierNameSyntax IdentifierName(string identifier)
    {
        return IdentifierNameBuilder.CreateSyntax(identifier);
    }

    public static QualifiedNameSyntax QualifiedName(Action<INameBuilder> leftCallback, Action<ISimpleNameBuilder> rightCallback)
    {
        return QualifiedNameBuilder.CreateSyntax(leftCallback, rightCallback);
    }

    public static GenericNameSyntax GenericName(string identifier, Action<IGenericNameBuilder>? genericNameCallback = null)
    {
        return GenericNameBuilder.CreateSyntax(identifier, genericNameCallback);
    }

    public static AliasQualifiedNameSyntax AliasQualifiedName(string aliasIdentifier, Action<ISimpleNameBuilder> nameCallback)
    {
        return AliasQualifiedNameBuilder.CreateSyntax(aliasIdentifier, nameCallback);
    }

    public static TypeSyntax Type(Action<ITypeBuilder> callback)
    {
        return TypeBuilder.CreateSyntax(callback);
    }

    public static PredefinedTypeSyntax PredefinedType(PredefinedTypeKeyword predefinedTypeKeyword)
    {
        return PredefinedTypeBuilder.CreateSyntax(predefinedTypeKeyword);
    }

    public static ArrayTypeSyntax ArrayType(Action<ITypeBuilder> elementTypeCallback, Action<IArrayTypeBuilder>? arrayTypeCallback = null)
    {
        return ArrayTypeBuilder.CreateSyntax(elementTypeCallback, arrayTypeCallback);
    }

    public static ArrayRankSpecifierSyntax ArrayRankSpecifier(Action<IArrayRankSpecifierBuilder>? arrayRankSpecifierCallback = null)
    {
        return ArrayRankSpecifierBuilder.CreateSyntax(arrayRankSpecifierCallback);
    }

    public static PointerTypeSyntax PointerType(Action<ITypeBuilder> elementTypeCallback)
    {
        return PointerTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public static FunctionPointerTypeSyntax FunctionPointerType(Action<IFunctionPointerTypeBuilder>? functionPointerTypeCallback = null)
    {
        return FunctionPointerTypeBuilder.CreateSyntax(functionPointerTypeCallback);
    }

    public static FunctionPointerCallingConventionSyntax FunctionPointerCallingConvention(FunctionPointerCallingConventionManagedOrUnmanagedKeyword functionPointerCallingConventionManagedOrUnmanagedKeyword, Action<IFunctionPointerCallingConventionBuilder>? functionPointerCallingConventionCallback = null)
    {
        return FunctionPointerCallingConventionBuilder.CreateSyntax(functionPointerCallingConventionManagedOrUnmanagedKeyword, functionPointerCallingConventionCallback);
    }

    public static FunctionPointerUnmanagedCallingConventionSyntax FunctionPointerUnmanagedCallingConvention(string name)
    {
        return FunctionPointerUnmanagedCallingConventionBuilder.CreateSyntax(name);
    }

    public static NullableTypeSyntax NullableType(Action<ITypeBuilder> elementTypeCallback)
    {
        return NullableTypeBuilder.CreateSyntax(elementTypeCallback);
    }

    public static TupleTypeSyntax TupleType(Action<ITupleTypeBuilder>? tupleTypeCallback = null)
    {
        return TupleTypeBuilder.CreateSyntax(tupleTypeCallback);
    }

    public static TupleElementSyntax TupleElement(Action<ITypeBuilder> typeCallback, Action<ITupleElementBuilder>? tupleElementCallback = null)
    {
        return TupleElementBuilder.CreateSyntax(typeCallback, tupleElementCallback);
    }

    public static OmittedTypeArgumentSyntax OmittedTypeArgument()
    {
        return OmittedTypeArgumentBuilder.CreateSyntax();
    }

    public static RefTypeSyntax RefType(Action<ITypeBuilder> typeCallback, Action<IRefTypeBuilder>? refTypeCallback = null)
    {
        return RefTypeBuilder.CreateSyntax(typeCallback, refTypeCallback);
    }

    public static ScopedTypeSyntax ScopedType(Action<ITypeBuilder> typeCallback)
    {
        return ScopedTypeBuilder.CreateSyntax(typeCallback);
    }

    public static ExpressionOrPatternSyntax ExpressionOrPattern(Action<IExpressionOrPatternBuilder> callback)
    {
        return ExpressionOrPatternBuilder.CreateSyntax(callback);
    }

    public static ExpressionSyntax Expression(Action<IExpressionBuilder> callback)
    {
        return ExpressionBuilder.CreateSyntax(callback);
    }

    public static ParenthesizedExpressionSyntax ParenthesizedExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return ParenthesizedExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static TupleExpressionSyntax TupleExpression(Action<ITupleExpressionBuilder>? tupleExpressionCallback = null)
    {
        return TupleExpressionBuilder.CreateSyntax(tupleExpressionCallback);
    }

    public static PrefixUnaryExpressionSyntax PrefixUnaryExpression(PrefixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        return PrefixUnaryExpressionBuilder.CreateSyntax(kind, operandCallback);
    }

    public static AwaitExpressionSyntax AwaitExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return AwaitExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static PostfixUnaryExpressionSyntax PostfixUnaryExpression(PostfixUnaryExpressionKind kind, Action<IExpressionBuilder> operandCallback)
    {
        return PostfixUnaryExpressionBuilder.CreateSyntax(kind, operandCallback);
    }

    public static MemberAccessExpressionSyntax MemberAccessExpression(MemberAccessExpressionKind kind, Action<IExpressionBuilder> expressionCallback, Action<ISimpleNameBuilder> nameCallback)
    {
        return MemberAccessExpressionBuilder.CreateSyntax(kind, expressionCallback, nameCallback);
    }

    public static ConditionalAccessExpressionSyntax ConditionalAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IExpressionBuilder> whenNotNullCallback)
    {
        return ConditionalAccessExpressionBuilder.CreateSyntax(expressionCallback, whenNotNullCallback);
    }

    public static MemberBindingExpressionSyntax MemberBindingExpression(Action<ISimpleNameBuilder> nameCallback)
    {
        return MemberBindingExpressionBuilder.CreateSyntax(nameCallback);
    }

    public static ElementBindingExpressionSyntax ElementBindingExpression(Action<IElementBindingExpressionBuilder>? elementBindingExpressionCallback = null)
    {
        return ElementBindingExpressionBuilder.CreateSyntax(elementBindingExpressionCallback);
    }

    public static RangeExpressionSyntax RangeExpression(Action<IRangeExpressionBuilder>? rangeExpressionCallback = null)
    {
        return RangeExpressionBuilder.CreateSyntax(rangeExpressionCallback);
    }

    public static ImplicitElementAccessSyntax ImplicitElementAccess(Action<IImplicitElementAccessBuilder>? implicitElementAccessCallback = null)
    {
        return ImplicitElementAccessBuilder.CreateSyntax(implicitElementAccessCallback);
    }

    public static BinaryExpressionSyntax BinaryExpression(BinaryExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        return BinaryExpressionBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public static AssignmentExpressionSyntax AssignmentExpression(AssignmentExpressionKind kind, Action<IExpressionBuilder> leftCallback, Action<IExpressionBuilder> rightCallback)
    {
        return AssignmentExpressionBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public static ConditionalExpressionSyntax ConditionalExpression(Action<IExpressionBuilder> conditionCallback, Action<IExpressionBuilder> whenTrueCallback, Action<IExpressionBuilder> whenFalseCallback)
    {
        return ConditionalExpressionBuilder.CreateSyntax(conditionCallback, whenTrueCallback, whenFalseCallback);
    }

    public static InstanceExpressionSyntax InstanceExpression(Action<IInstanceExpressionBuilder> callback)
    {
        return InstanceExpressionBuilder.CreateSyntax(callback);
    }

    public static ThisExpressionSyntax ThisExpression()
    {
        return ThisExpressionBuilder.CreateSyntax();
    }

    public static BaseExpressionSyntax BaseExpression()
    {
        return BaseExpressionBuilder.CreateSyntax();
    }

    public static LiteralExpressionSyntax LiteralExpression(Action<ILiteralExpressionBuilder> callback)
    {
        return LiteralExpressionBuilder.CreateSyntax(callback);
    }

    public static MakeRefExpressionSyntax MakeRefExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return MakeRefExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static RefTypeExpressionSyntax RefTypeExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return RefTypeExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static RefValueExpressionSyntax RefValueExpression(Action<IExpressionBuilder> expressionCallback, Action<ITypeBuilder> typeCallback)
    {
        return RefValueExpressionBuilder.CreateSyntax(expressionCallback, typeCallback);
    }

    public static CheckedExpressionSyntax CheckedExpression(CheckedExpressionKind kind, Action<IExpressionBuilder> expressionCallback)
    {
        return CheckedExpressionBuilder.CreateSyntax(kind, expressionCallback);
    }

    public static DefaultExpressionSyntax DefaultExpression(Action<ITypeBuilder> typeCallback)
    {
        return DefaultExpressionBuilder.CreateSyntax(typeCallback);
    }

    public static TypeOfExpressionSyntax TypeOfExpression(Action<ITypeBuilder> typeCallback)
    {
        return TypeOfExpressionBuilder.CreateSyntax(typeCallback);
    }

    public static SizeOfExpressionSyntax SizeOfExpression(Action<ITypeBuilder> typeCallback)
    {
        return SizeOfExpressionBuilder.CreateSyntax(typeCallback);
    }

    public static InvocationExpressionSyntax InvocationExpression(Action<IExpressionBuilder> expressionCallback, Action<IInvocationExpressionBuilder>? invocationExpressionCallback = null)
    {
        return InvocationExpressionBuilder.CreateSyntax(expressionCallback, invocationExpressionCallback);
    }

    public static ElementAccessExpressionSyntax ElementAccessExpression(Action<IExpressionBuilder> expressionCallback, Action<IElementAccessExpressionBuilder>? elementAccessExpressionCallback = null)
    {
        return ElementAccessExpressionBuilder.CreateSyntax(expressionCallback, elementAccessExpressionCallback);
    }

    public static ArgumentSyntax Argument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        return ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
    }

    public static BaseExpressionColonSyntax BaseExpressionColon(Action<IBaseExpressionColonBuilder> callback)
    {
        return BaseExpressionColonBuilder.CreateSyntax(callback);
    }

    public static ExpressionColonSyntax ExpressionColon(Action<IExpressionBuilder> expressionCallback)
    {
        return ExpressionColonBuilder.CreateSyntax(expressionCallback);
    }

    public static NameColonSyntax NameColon(string nameIdentifier)
    {
        return NameColonBuilder.CreateSyntax(nameIdentifier);
    }

    public static DeclarationExpressionSyntax DeclarationExpression(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        return DeclarationExpressionBuilder.CreateSyntax(typeCallback, designationCallback);
    }

    public static CastExpressionSyntax CastExpression(Action<ITypeBuilder> typeCallback, Action<IExpressionBuilder> expressionCallback)
    {
        return CastExpressionBuilder.CreateSyntax(typeCallback, expressionCallback);
    }

    public static AnonymousFunctionExpressionSyntax AnonymousFunctionExpression(Action<IAnonymousFunctionExpressionBuilder> callback)
    {
        return AnonymousFunctionExpressionBuilder.CreateSyntax(callback);
    }

    public static AnonymousMethodExpressionSyntax AnonymousMethodExpression(Action<IBlockBuilder> blockBlockCallback, Action<IAnonymousMethodExpressionBuilder> anonymousMethodExpressionCallback)
    {
        return AnonymousMethodExpressionBuilder.CreateSyntax(blockBlockCallback, anonymousMethodExpressionCallback);
    }

    public static LambdaExpressionSyntax LambdaExpression(Action<ILambdaExpressionBuilder> callback)
    {
        return LambdaExpressionBuilder.CreateSyntax(callback);
    }

    public static SimpleLambdaExpressionSyntax SimpleLambdaExpression(string parameterIdentifier, Action<IParameterBuilder> parameterParameterCallback, Action<ISimpleLambdaExpressionBuilder> simpleLambdaExpressionCallback)
    {
        return SimpleLambdaExpressionBuilder.CreateSyntax(parameterIdentifier, parameterParameterCallback, simpleLambdaExpressionCallback);
    }

    public static RefExpressionSyntax RefExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return RefExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static ParenthesizedLambdaExpressionSyntax ParenthesizedLambdaExpression(Action<IParenthesizedLambdaExpressionBuilder>? parenthesizedLambdaExpressionCallback = null)
    {
        return ParenthesizedLambdaExpressionBuilder.CreateSyntax(parenthesizedLambdaExpressionCallback);
    }

    public static InitializerExpressionSyntax InitializerExpression(InitializerExpressionKind kind, Action<IInitializerExpressionBuilder>? initializerExpressionCallback = null)
    {
        return InitializerExpressionBuilder.CreateSyntax(kind, initializerExpressionCallback);
    }

    public static BaseObjectCreationExpressionSyntax BaseObjectCreationExpression(Action<IBaseObjectCreationExpressionBuilder> callback)
    {
        return BaseObjectCreationExpressionBuilder.CreateSyntax(callback);
    }

    public static ImplicitObjectCreationExpressionSyntax ImplicitObjectCreationExpression(Action<IImplicitObjectCreationExpressionBuilder>? implicitObjectCreationExpressionCallback = null)
    {
        return ImplicitObjectCreationExpressionBuilder.CreateSyntax(implicitObjectCreationExpressionCallback);
    }

    public static ObjectCreationExpressionSyntax ObjectCreationExpression(Action<ITypeBuilder> typeCallback, Action<IObjectCreationExpressionBuilder>? objectCreationExpressionCallback = null)
    {
        return ObjectCreationExpressionBuilder.CreateSyntax(typeCallback, objectCreationExpressionCallback);
    }

    public static WithExpressionSyntax WithExpression(Action<IExpressionBuilder> expressionCallback, InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        return WithExpressionBuilder.CreateSyntax(expressionCallback, initializerKind, initializerInitializerExpressionCallback);
    }

    public static AnonymousObjectMemberDeclaratorSyntax AnonymousObjectMemberDeclarator(Action<IExpressionBuilder> expressionCallback, Action<IAnonymousObjectMemberDeclaratorBuilder>? anonymousObjectMemberDeclaratorCallback = null)
    {
        return AnonymousObjectMemberDeclaratorBuilder.CreateSyntax(expressionCallback, anonymousObjectMemberDeclaratorCallback);
    }

    public static AnonymousObjectCreationExpressionSyntax AnonymousObjectCreationExpression(Action<IAnonymousObjectCreationExpressionBuilder>? anonymousObjectCreationExpressionCallback = null)
    {
        return AnonymousObjectCreationExpressionBuilder.CreateSyntax(anonymousObjectCreationExpressionCallback);
    }

    public static ArrayCreationExpressionSyntax ArrayCreationExpression(Action<ITypeBuilder> typeElementTypeCallback, Action<IArrayTypeBuilder> typeArrayTypeCallback, Action<IArrayCreationExpressionBuilder> arrayCreationExpressionCallback)
    {
        return ArrayCreationExpressionBuilder.CreateSyntax(typeElementTypeCallback, typeArrayTypeCallback, arrayCreationExpressionCallback);
    }

    public static ImplicitArrayCreationExpressionSyntax ImplicitArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback, Action<IImplicitArrayCreationExpressionBuilder> implicitArrayCreationExpressionCallback)
    {
        return ImplicitArrayCreationExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback, implicitArrayCreationExpressionCallback);
    }

    public static StackAllocArrayCreationExpressionSyntax StackAllocArrayCreationExpression(Action<ITypeBuilder> typeCallback, Action<IStackAllocArrayCreationExpressionBuilder>? stackAllocArrayCreationExpressionCallback = null)
    {
        return StackAllocArrayCreationExpressionBuilder.CreateSyntax(typeCallback, stackAllocArrayCreationExpressionCallback);
    }

    public static ImplicitStackAllocArrayCreationExpressionSyntax ImplicitStackAllocArrayCreationExpression(InitializerExpressionKind initializerKind, Action<IInitializerExpressionBuilder> initializerInitializerExpressionCallback)
    {
        return ImplicitStackAllocArrayCreationExpressionBuilder.CreateSyntax(initializerKind, initializerInitializerExpressionCallback);
    }

    public static QueryClauseSyntax QueryClause(Action<IQueryClauseBuilder> callback)
    {
        return QueryClauseBuilder.CreateSyntax(callback);
    }

    public static SelectOrGroupClauseSyntax SelectOrGroupClause(Action<ISelectOrGroupClauseBuilder> callback)
    {
        return SelectOrGroupClauseBuilder.CreateSyntax(callback);
    }

    public static QueryExpressionSyntax QueryExpression(string fromClauseIdentifier, Action<IExpressionBuilder> fromClauseExpressionCallback, Action<IFromClauseBuilder> fromClauseFromClauseCallback, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        return QueryExpressionBuilder.CreateSyntax(fromClauseIdentifier, fromClauseExpressionCallback, fromClauseFromClauseCallback, bodySelectOrGroupCallback, bodyQueryBodyCallback);
    }

    public static QueryBodySyntax QueryBody(Action<ISelectOrGroupClauseBuilder> selectOrGroupCallback, Action<IQueryBodyBuilder>? queryBodyCallback = null)
    {
        return QueryBodyBuilder.CreateSyntax(selectOrGroupCallback, queryBodyCallback);
    }

    public static FromClauseSyntax FromClause(string identifier, Action<IExpressionBuilder> expressionCallback, Action<IFromClauseBuilder>? fromClauseCallback = null)
    {
        return FromClauseBuilder.CreateSyntax(identifier, expressionCallback, fromClauseCallback);
    }

    public static LetClauseSyntax LetClause(string identifier, Action<IExpressionBuilder> expressionCallback)
    {
        return LetClauseBuilder.CreateSyntax(identifier, expressionCallback);
    }

    public static JoinClauseSyntax JoinClause(string identifier, Action<IExpressionBuilder> inExpressionCallback, Action<IExpressionBuilder> leftExpressionCallback, Action<IExpressionBuilder> rightExpressionCallback, Action<IJoinClauseBuilder>? joinClauseCallback = null)
    {
        return JoinClauseBuilder.CreateSyntax(identifier, inExpressionCallback, leftExpressionCallback, rightExpressionCallback, joinClauseCallback);
    }

    public static JoinIntoClauseSyntax JoinIntoClause(string identifier)
    {
        return JoinIntoClauseBuilder.CreateSyntax(identifier);
    }

    public static WhereClauseSyntax WhereClause(Action<IExpressionBuilder> conditionCallback)
    {
        return WhereClauseBuilder.CreateSyntax(conditionCallback);
    }

    public static OrderByClauseSyntax OrderByClause(Action<IOrderByClauseBuilder>? orderByClauseCallback = null)
    {
        return OrderByClauseBuilder.CreateSyntax(orderByClauseCallback);
    }

    public static OrderingSyntax Ordering(OrderingKind kind, Action<IExpressionBuilder> expressionCallback, Action<IOrderingBuilder>? orderingCallback = null)
    {
        return OrderingBuilder.CreateSyntax(kind, expressionCallback, orderingCallback);
    }

    public static SelectClauseSyntax SelectClause(Action<IExpressionBuilder> expressionCallback)
    {
        return SelectClauseBuilder.CreateSyntax(expressionCallback);
    }

    public static GroupClauseSyntax GroupClause(Action<IExpressionBuilder> groupExpressionCallback, Action<IExpressionBuilder> byExpressionCallback)
    {
        return GroupClauseBuilder.CreateSyntax(groupExpressionCallback, byExpressionCallback);
    }

    public static QueryContinuationSyntax QueryContinuation(string identifier, Action<ISelectOrGroupClauseBuilder> bodySelectOrGroupCallback, Action<IQueryBodyBuilder> bodyQueryBodyCallback)
    {
        return QueryContinuationBuilder.CreateSyntax(identifier, bodySelectOrGroupCallback, bodyQueryBodyCallback);
    }

    public static OmittedArraySizeExpressionSyntax OmittedArraySizeExpression()
    {
        return OmittedArraySizeExpressionBuilder.CreateSyntax();
    }

    public static InterpolatedStringExpressionSyntax InterpolatedStringExpression(InterpolatedStringExpressionStringStartToken interpolatedStringExpressionStringStartToken, InterpolatedStringExpressionStringEndToken interpolatedStringExpressionStringEndToken, Action<IInterpolatedStringExpressionBuilder>? interpolatedStringExpressionCallback = null)
    {
        return InterpolatedStringExpressionBuilder.CreateSyntax(interpolatedStringExpressionStringStartToken, interpolatedStringExpressionStringEndToken, interpolatedStringExpressionCallback);
    }

    public static IsPatternExpressionSyntax IsPatternExpression(Action<IExpressionBuilder> expressionCallback, Action<IPatternBuilder> patternCallback)
    {
        return IsPatternExpressionBuilder.CreateSyntax(expressionCallback, patternCallback);
    }

    public static ThrowExpressionSyntax ThrowExpression(Action<IExpressionBuilder> expressionCallback)
    {
        return ThrowExpressionBuilder.CreateSyntax(expressionCallback);
    }

    public static WhenClauseSyntax WhenClause(Action<IExpressionBuilder> conditionCallback)
    {
        return WhenClauseBuilder.CreateSyntax(conditionCallback);
    }

    public static PatternSyntax Pattern(Action<IPatternBuilder> callback)
    {
        return PatternBuilder.CreateSyntax(callback);
    }

    public static DiscardPatternSyntax DiscardPattern()
    {
        return DiscardPatternBuilder.CreateSyntax();
    }

    public static DeclarationPatternSyntax DeclarationPattern(Action<ITypeBuilder> typeCallback, Action<IVariableDesignationBuilder> designationCallback)
    {
        return DeclarationPatternBuilder.CreateSyntax(typeCallback, designationCallback);
    }

    public static VarPatternSyntax VarPattern(Action<IVariableDesignationBuilder> designationCallback)
    {
        return VarPatternBuilder.CreateSyntax(designationCallback);
    }

    public static RecursivePatternSyntax RecursivePattern(Action<IRecursivePatternBuilder>? recursivePatternCallback = null)
    {
        return RecursivePatternBuilder.CreateSyntax(recursivePatternCallback);
    }

    public static PositionalPatternClauseSyntax PositionalPatternClause(Action<IPositionalPatternClauseBuilder>? positionalPatternClauseCallback = null)
    {
        return PositionalPatternClauseBuilder.CreateSyntax(positionalPatternClauseCallback);
    }

    public static PropertyPatternClauseSyntax PropertyPatternClause(Action<IPropertyPatternClauseBuilder>? propertyPatternClauseCallback = null)
    {
        return PropertyPatternClauseBuilder.CreateSyntax(propertyPatternClauseCallback);
    }

    public static SubpatternSyntax Subpattern(Action<IPatternBuilder> patternCallback, Action<ISubpatternBuilder>? subpatternCallback = null)
    {
        return SubpatternBuilder.CreateSyntax(patternCallback, subpatternCallback);
    }

    public static ConstantPatternSyntax ConstantPattern(Action<IExpressionBuilder> expressionCallback)
    {
        return ConstantPatternBuilder.CreateSyntax(expressionCallback);
    }

    public static ParenthesizedPatternSyntax ParenthesizedPattern(Action<IPatternBuilder> patternCallback)
    {
        return ParenthesizedPatternBuilder.CreateSyntax(patternCallback);
    }

    public static RelationalPatternSyntax RelationalPattern(RelationalPatternOperatorToken relationalPatternOperatorToken, Action<IExpressionBuilder> expressionCallback)
    {
        return RelationalPatternBuilder.CreateSyntax(relationalPatternOperatorToken, expressionCallback);
    }

    public static TypePatternSyntax TypePattern(Action<ITypeBuilder> typeCallback)
    {
        return TypePatternBuilder.CreateSyntax(typeCallback);
    }

    public static BinaryPatternSyntax BinaryPattern(BinaryPatternKind kind, Action<IPatternBuilder> leftCallback, Action<IPatternBuilder> rightCallback)
    {
        return BinaryPatternBuilder.CreateSyntax(kind, leftCallback, rightCallback);
    }

    public static UnaryPatternSyntax UnaryPattern(Action<IPatternBuilder> patternCallback)
    {
        return UnaryPatternBuilder.CreateSyntax(patternCallback);
    }

    public static ListPatternSyntax ListPattern(Action<IListPatternBuilder>? listPatternCallback = null)
    {
        return ListPatternBuilder.CreateSyntax(listPatternCallback);
    }

    public static SlicePatternSyntax SlicePattern(Action<ISlicePatternBuilder>? slicePatternCallback = null)
    {
        return SlicePatternBuilder.CreateSyntax(slicePatternCallback);
    }

    public static InterpolatedStringContentSyntax InterpolatedStringContent(Action<IInterpolatedStringContentBuilder> callback)
    {
        return InterpolatedStringContentBuilder.CreateSyntax(callback);
    }

    public static InterpolatedStringTextSyntax InterpolatedStringText()
    {
        return InterpolatedStringTextBuilder.CreateSyntax();
    }

    public static InterpolationSyntax Interpolation(Action<IExpressionBuilder> expressionCallback, Action<IInterpolationBuilder>? interpolationCallback = null)
    {
        return InterpolationBuilder.CreateSyntax(expressionCallback, interpolationCallback);
    }

    public static InterpolationAlignmentClauseSyntax InterpolationAlignmentClause(Action<IExpressionBuilder> valueCallback)
    {
        return InterpolationAlignmentClauseBuilder.CreateSyntax(valueCallback);
    }

    public static InterpolationFormatClauseSyntax InterpolationFormatClause()
    {
        return InterpolationFormatClauseBuilder.CreateSyntax();
    }

    public static GlobalStatementSyntax GlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder>? globalStatementCallback = null)
    {
        return GlobalStatementBuilder.CreateSyntax(statementCallback, globalStatementCallback);
    }

    public static StatementSyntax Statement(Action<IStatementBuilder> callback)
    {
        return StatementBuilder.CreateSyntax(callback);
    }

    public static BlockSyntax Block(Action<IBlockBuilder>? blockCallback = null)
    {
        return BlockBuilder.CreateSyntax(blockCallback);
    }

    public static LocalFunctionStatementSyntax LocalFunctionStatement(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<ILocalFunctionStatementBuilder>? localFunctionStatementCallback = null)
    {
        return LocalFunctionStatementBuilder.CreateSyntax(returnTypeCallback, identifier, localFunctionStatementCallback);
    }

    public static LocalDeclarationStatementSyntax LocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback)
    {
        return LocalDeclarationStatementBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, localDeclarationStatementCallback);
    }

    public static VariableDeclarationSyntax VariableDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder>? variableDeclarationCallback = null)
    {
        return VariableDeclarationBuilder.CreateSyntax(typeCallback, variableDeclarationCallback);
    }

    public static VariableDeclaratorSyntax VariableDeclarator(string identifier, Action<IVariableDeclaratorBuilder>? variableDeclaratorCallback = null)
    {
        return VariableDeclaratorBuilder.CreateSyntax(identifier, variableDeclaratorCallback);
    }

    public static EqualsValueClauseSyntax EqualsValueClause(Action<IExpressionBuilder> valueCallback)
    {
        return EqualsValueClauseBuilder.CreateSyntax(valueCallback);
    }

    public static VariableDesignationSyntax VariableDesignation(Action<IVariableDesignationBuilder> callback)
    {
        return VariableDesignationBuilder.CreateSyntax(callback);
    }

    public static SingleVariableDesignationSyntax SingleVariableDesignation(string identifier)
    {
        return SingleVariableDesignationBuilder.CreateSyntax(identifier);
    }

    public static DiscardDesignationSyntax DiscardDesignation()
    {
        return DiscardDesignationBuilder.CreateSyntax();
    }

    public static ParenthesizedVariableDesignationSyntax ParenthesizedVariableDesignation(Action<IParenthesizedVariableDesignationBuilder>? parenthesizedVariableDesignationCallback = null)
    {
        return ParenthesizedVariableDesignationBuilder.CreateSyntax(parenthesizedVariableDesignationCallback);
    }

    public static ExpressionStatementSyntax ExpressionStatement(Action<IExpressionBuilder> expressionCallback, Action<IExpressionStatementBuilder>? expressionStatementCallback = null)
    {
        return ExpressionStatementBuilder.CreateSyntax(expressionCallback, expressionStatementCallback);
    }

    public static EmptyStatementSyntax EmptyStatement(Action<IEmptyStatementBuilder>? emptyStatementCallback = null)
    {
        return EmptyStatementBuilder.CreateSyntax(emptyStatementCallback);
    }

    public static LabeledStatementSyntax LabeledStatement(string identifier, Action<IStatementBuilder> statementCallback, Action<ILabeledStatementBuilder>? labeledStatementCallback = null)
    {
        return LabeledStatementBuilder.CreateSyntax(identifier, statementCallback, labeledStatementCallback);
    }

    public static GotoStatementSyntax GotoStatement(GotoStatementKind kind, Action<IGotoStatementBuilder>? gotoStatementCallback = null)
    {
        return GotoStatementBuilder.CreateSyntax(kind, gotoStatementCallback);
    }

    public static BreakStatementSyntax BreakStatement(Action<IBreakStatementBuilder>? breakStatementCallback = null)
    {
        return BreakStatementBuilder.CreateSyntax(breakStatementCallback);
    }

    public static ContinueStatementSyntax ContinueStatement(Action<IContinueStatementBuilder>? continueStatementCallback = null)
    {
        return ContinueStatementBuilder.CreateSyntax(continueStatementCallback);
    }

    public static ReturnStatementSyntax ReturnStatement(Action<IReturnStatementBuilder>? returnStatementCallback = null)
    {
        return ReturnStatementBuilder.CreateSyntax(returnStatementCallback);
    }

    public static ThrowStatementSyntax ThrowStatement(Action<IThrowStatementBuilder>? throwStatementCallback = null)
    {
        return ThrowStatementBuilder.CreateSyntax(throwStatementCallback);
    }

    public static YieldStatementSyntax YieldStatement(YieldStatementKind kind, Action<IYieldStatementBuilder>? yieldStatementCallback = null)
    {
        return YieldStatementBuilder.CreateSyntax(kind, yieldStatementCallback);
    }

    public static WhileStatementSyntax WhileStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IWhileStatementBuilder>? whileStatementCallback = null)
    {
        return WhileStatementBuilder.CreateSyntax(conditionCallback, statementCallback, whileStatementCallback);
    }

    public static DoStatementSyntax DoStatement(Action<IStatementBuilder> statementCallback, Action<IExpressionBuilder> conditionCallback, Action<IDoStatementBuilder>? doStatementCallback = null)
    {
        return DoStatementBuilder.CreateSyntax(statementCallback, conditionCallback, doStatementCallback);
    }

    public static ForStatementSyntax ForStatement(Action<IStatementBuilder> statementCallback, Action<IForStatementBuilder>? forStatementCallback = null)
    {
        return ForStatementBuilder.CreateSyntax(statementCallback, forStatementCallback);
    }

    public static CommonForEachStatementSyntax CommonForEachStatement(Action<ICommonForEachStatementBuilder> callback)
    {
        return CommonForEachStatementBuilder.CreateSyntax(callback);
    }

    public static ForEachStatementSyntax ForEachStatement(Action<ITypeBuilder> typeCallback, string identifier, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachStatementBuilder>? forEachStatementCallback = null)
    {
        return ForEachStatementBuilder.CreateSyntax(typeCallback, identifier, expressionCallback, statementCallback, forEachStatementCallback);
    }

    public static ForEachVariableStatementSyntax ForEachVariableStatement(Action<IExpressionBuilder> variableCallback, Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<IForEachVariableStatementBuilder>? forEachVariableStatementCallback = null)
    {
        return ForEachVariableStatementBuilder.CreateSyntax(variableCallback, expressionCallback, statementCallback, forEachVariableStatementCallback);
    }

    public static UsingStatementSyntax UsingStatement(Action<IStatementBuilder> statementCallback, Action<IUsingStatementBuilder>? usingStatementCallback = null)
    {
        return UsingStatementBuilder.CreateSyntax(statementCallback, usingStatementCallback);
    }

    public static FixedStatementSyntax FixedStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IStatementBuilder> statementCallback, Action<IFixedStatementBuilder> fixedStatementCallback)
    {
        return FixedStatementBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, statementCallback, fixedStatementCallback);
    }

    public static CheckedStatementSyntax CheckedStatement(CheckedStatementKind kind, Action<IBlockBuilder> blockBlockCallback, Action<ICheckedStatementBuilder> checkedStatementCallback)
    {
        return CheckedStatementBuilder.CreateSyntax(kind, blockBlockCallback, checkedStatementCallback);
    }

    public static UnsafeStatementSyntax UnsafeStatement(Action<IBlockBuilder> blockBlockCallback, Action<IUnsafeStatementBuilder> unsafeStatementCallback)
    {
        return UnsafeStatementBuilder.CreateSyntax(blockBlockCallback, unsafeStatementCallback);
    }

    public static LockStatementSyntax LockStatement(Action<IExpressionBuilder> expressionCallback, Action<IStatementBuilder> statementCallback, Action<ILockStatementBuilder>? lockStatementCallback = null)
    {
        return LockStatementBuilder.CreateSyntax(expressionCallback, statementCallback, lockStatementCallback);
    }

    public static IfStatementSyntax IfStatement(Action<IExpressionBuilder> conditionCallback, Action<IStatementBuilder> statementCallback, Action<IIfStatementBuilder>? ifStatementCallback = null)
    {
        return IfStatementBuilder.CreateSyntax(conditionCallback, statementCallback, ifStatementCallback);
    }

    public static ElseClauseSyntax ElseClause(Action<IStatementBuilder> statementCallback)
    {
        return ElseClauseBuilder.CreateSyntax(statementCallback);
    }

    public static SwitchStatementSyntax SwitchStatement(Action<IExpressionBuilder> expressionCallback, Action<ISwitchStatementBuilder>? switchStatementCallback = null)
    {
        return SwitchStatementBuilder.CreateSyntax(expressionCallback, switchStatementCallback);
    }

    public static SwitchSectionSyntax SwitchSection(Action<ISwitchSectionBuilder>? switchSectionCallback = null)
    {
        return SwitchSectionBuilder.CreateSyntax(switchSectionCallback);
    }

    public static SwitchLabelSyntax SwitchLabel(Action<ISwitchLabelBuilder> callback)
    {
        return SwitchLabelBuilder.CreateSyntax(callback);
    }

    public static CasePatternSwitchLabelSyntax CasePatternSwitchLabel(Action<IPatternBuilder> patternCallback, Action<ICasePatternSwitchLabelBuilder>? casePatternSwitchLabelCallback = null)
    {
        return CasePatternSwitchLabelBuilder.CreateSyntax(patternCallback, casePatternSwitchLabelCallback);
    }

    public static CaseSwitchLabelSyntax CaseSwitchLabel(Action<IExpressionBuilder> valueCallback)
    {
        return CaseSwitchLabelBuilder.CreateSyntax(valueCallback);
    }

    public static DefaultSwitchLabelSyntax DefaultSwitchLabel()
    {
        return DefaultSwitchLabelBuilder.CreateSyntax();
    }

    public static SwitchExpressionSyntax SwitchExpression(Action<IExpressionBuilder> governingExpressionCallback, Action<ISwitchExpressionBuilder>? switchExpressionCallback = null)
    {
        return SwitchExpressionBuilder.CreateSyntax(governingExpressionCallback, switchExpressionCallback);
    }

    public static SwitchExpressionArmSyntax SwitchExpressionArm(Action<IPatternBuilder> patternCallback, Action<IExpressionBuilder> expressionCallback, Action<ISwitchExpressionArmBuilder>? switchExpressionArmCallback = null)
    {
        return SwitchExpressionArmBuilder.CreateSyntax(patternCallback, expressionCallback, switchExpressionArmCallback);
    }

    public static TryStatementSyntax TryStatement(Action<IBlockBuilder> blockBlockCallback, Action<ITryStatementBuilder> tryStatementCallback)
    {
        return TryStatementBuilder.CreateSyntax(blockBlockCallback, tryStatementCallback);
    }

    public static CatchClauseSyntax CatchClause(Action<IBlockBuilder> blockBlockCallback, Action<ICatchClauseBuilder> catchClauseCallback)
    {
        return CatchClauseBuilder.CreateSyntax(blockBlockCallback, catchClauseCallback);
    }

    public static CatchDeclarationSyntax CatchDeclaration(Action<ITypeBuilder> typeCallback, Action<ICatchDeclarationBuilder>? catchDeclarationCallback = null)
    {
        return CatchDeclarationBuilder.CreateSyntax(typeCallback, catchDeclarationCallback);
    }

    public static CatchFilterClauseSyntax CatchFilterClause(Action<IExpressionBuilder> filterExpressionCallback)
    {
        return CatchFilterClauseBuilder.CreateSyntax(filterExpressionCallback);
    }

    public static FinallyClauseSyntax FinallyClause(Action<IBlockBuilder> blockBlockCallback)
    {
        return FinallyClauseBuilder.CreateSyntax(blockBlockCallback);
    }

    public static CompilationUnitSyntax CompilationUnit(Action<ICompilationUnitBuilder>? compilationUnitCallback = null)
    {
        return CompilationUnitBuilder.CreateSyntax(compilationUnitCallback);
    }

    public static ExternAliasDirectiveSyntax ExternAliasDirective(string identifier)
    {
        return ExternAliasDirectiveBuilder.CreateSyntax(identifier);
    }

    public static UsingDirectiveSyntax UsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder>? usingDirectiveCallback = null)
    {
        return UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
    }

    public static MemberDeclarationSyntax MemberDeclaration(Action<IMemberDeclarationBuilder> callback)
    {
        return MemberDeclarationBuilder.CreateSyntax(callback);
    }

    public static BaseNamespaceDeclarationSyntax BaseNamespaceDeclaration(Action<IBaseNamespaceDeclarationBuilder> callback)
    {
        return BaseNamespaceDeclarationBuilder.CreateSyntax(callback);
    }

    public static NamespaceDeclarationSyntax NamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null)
    {
        return NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceDeclarationCallback);
    }

    public static FileScopedNamespaceDeclarationSyntax FileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder>? fileScopedNamespaceDeclarationCallback = null)
    {
        return FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, fileScopedNamespaceDeclarationCallback);
    }

    public static AttributeTargetSpecifierSyntax AttributeTargetSpecifier(string identifier)
    {
        return AttributeTargetSpecifierBuilder.CreateSyntax(identifier);
    }

    public static AttributeSyntax Attribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        return AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
    }

    public static AttributeArgumentSyntax AttributeArgument(Action<IExpressionBuilder> expressionCallback, Action<IAttributeArgumentBuilder>? attributeArgumentCallback = null)
    {
        return AttributeArgumentBuilder.CreateSyntax(expressionCallback, attributeArgumentCallback);
    }

    public static NameEqualsSyntax NameEquals(string nameIdentifier)
    {
        return NameEqualsBuilder.CreateSyntax(nameIdentifier);
    }

    public static TypeParameterSyntax TypeParameter(string identifier, Action<ITypeParameterBuilder>? typeParameterCallback = null)
    {
        return TypeParameterBuilder.CreateSyntax(identifier, typeParameterCallback);
    }

    public static BaseTypeDeclarationSyntax BaseTypeDeclaration(Action<IBaseTypeDeclarationBuilder> callback)
    {
        return BaseTypeDeclarationBuilder.CreateSyntax(callback);
    }

    public static TypeDeclarationSyntax TypeDeclaration(Action<ITypeDeclarationBuilder> callback)
    {
        return TypeDeclarationBuilder.CreateSyntax(callback);
    }

    public static ClassDeclarationSyntax ClassDeclaration(string identifier, Action<IClassDeclarationBuilder>? classDeclarationCallback = null)
    {
        return ClassDeclarationBuilder.CreateSyntax(identifier, classDeclarationCallback);
    }

    public static StructDeclarationSyntax StructDeclaration(string identifier, Action<IStructDeclarationBuilder>? structDeclarationCallback = null)
    {
        return StructDeclarationBuilder.CreateSyntax(identifier, structDeclarationCallback);
    }

    public static InterfaceDeclarationSyntax InterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder>? interfaceDeclarationCallback = null)
    {
        return InterfaceDeclarationBuilder.CreateSyntax(identifier, interfaceDeclarationCallback);
    }

    public static RecordDeclarationSyntax RecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder>? recordDeclarationCallback = null)
    {
        return RecordDeclarationBuilder.CreateSyntax(kind, identifier, recordDeclarationCallback);
    }

    public static EnumDeclarationSyntax EnumDeclaration(string identifier, Action<IEnumDeclarationBuilder>? enumDeclarationCallback = null)
    {
        return EnumDeclarationBuilder.CreateSyntax(identifier, enumDeclarationCallback);
    }

    public static DelegateDeclarationSyntax DelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder>? delegateDeclarationCallback = null)
    {
        return DelegateDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, delegateDeclarationCallback);
    }

    public static EnumMemberDeclarationSyntax EnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null)
    {
        return EnumMemberDeclarationBuilder.CreateSyntax(identifier, enumMemberDeclarationCallback);
    }

    public static BaseTypeSyntax BaseType(Action<IBaseTypeBuilder> callback)
    {
        return BaseTypeBuilder.CreateSyntax(callback);
    }

    public static SimpleBaseTypeSyntax SimpleBaseType(Action<ITypeBuilder> typeCallback)
    {
        return SimpleBaseTypeBuilder.CreateSyntax(typeCallback);
    }

    public static PrimaryConstructorBaseTypeSyntax PrimaryConstructorBaseType(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder>? primaryConstructorBaseTypeCallback = null)
    {
        return PrimaryConstructorBaseTypeBuilder.CreateSyntax(typeCallback, primaryConstructorBaseTypeCallback);
    }

    public static TypeParameterConstraintClauseSyntax TypeParameterConstraintClause(string nameIdentifier, Action<ITypeParameterConstraintClauseBuilder> typeParameterConstraintClauseCallback)
    {
        return TypeParameterConstraintClauseBuilder.CreateSyntax(nameIdentifier, typeParameterConstraintClauseCallback);
    }

    public static TypeParameterConstraintSyntax TypeParameterConstraint(Action<ITypeParameterConstraintBuilder> callback)
    {
        return TypeParameterConstraintBuilder.CreateSyntax(callback);
    }

    public static ConstructorConstraintSyntax ConstructorConstraint()
    {
        return ConstructorConstraintBuilder.CreateSyntax();
    }

    public static ClassOrStructConstraintSyntax ClassOrStructConstraint(ClassOrStructConstraintKind kind, Action<IClassOrStructConstraintBuilder>? classOrStructConstraintCallback = null)
    {
        return ClassOrStructConstraintBuilder.CreateSyntax(kind, classOrStructConstraintCallback);
    }

    public static TypeConstraintSyntax TypeConstraint(Action<ITypeBuilder> typeCallback)
    {
        return TypeConstraintBuilder.CreateSyntax(typeCallback);
    }

    public static DefaultConstraintSyntax DefaultConstraint()
    {
        return DefaultConstraintBuilder.CreateSyntax();
    }

    public static BaseFieldDeclarationSyntax BaseFieldDeclaration(Action<IBaseFieldDeclarationBuilder> callback)
    {
        return BaseFieldDeclarationBuilder.CreateSyntax(callback);
    }

    public static FieldDeclarationSyntax FieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return FieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback);
    }

    public static EventFieldDeclarationSyntax EventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return EventFieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback);
    }

    public static ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier(Action<INameBuilder> nameCallback)
    {
        return ExplicitInterfaceSpecifierBuilder.CreateSyntax(nameCallback);
    }

    public static BaseMethodDeclarationSyntax BaseMethodDeclaration(Action<IBaseMethodDeclarationBuilder> callback)
    {
        return BaseMethodDeclarationBuilder.CreateSyntax(callback);
    }

    public static MethodDeclarationSyntax MethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder>? methodDeclarationCallback = null)
    {
        return MethodDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, methodDeclarationCallback);
    }

    public static OperatorDeclarationSyntax OperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder>? operatorDeclarationCallback = null)
    {
        return OperatorDeclarationBuilder.CreateSyntax(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback);
    }

    public static ConversionOperatorDeclarationSyntax ConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder>? conversionOperatorDeclarationCallback = null)
    {
        return ConversionOperatorDeclarationBuilder.CreateSyntax(conversionOperatorDeclarationImplicitOrExplicitKeyword, typeCallback, conversionOperatorDeclarationCallback);
    }

    public static ConstructorDeclarationSyntax ConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder>? constructorDeclarationCallback = null)
    {
        return ConstructorDeclarationBuilder.CreateSyntax(identifier, constructorDeclarationCallback);
    }

    public static ConstructorInitializerSyntax ConstructorInitializer(ConstructorInitializerKind kind, Action<IConstructorInitializerBuilder>? constructorInitializerCallback = null)
    {
        return ConstructorInitializerBuilder.CreateSyntax(kind, constructorInitializerCallback);
    }

    public static DestructorDeclarationSyntax DestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder>? destructorDeclarationCallback = null)
    {
        return DestructorDeclarationBuilder.CreateSyntax(identifier, destructorDeclarationCallback);
    }

    public static BasePropertyDeclarationSyntax BasePropertyDeclaration(Action<IBasePropertyDeclarationBuilder> callback)
    {
        return BasePropertyDeclarationBuilder.CreateSyntax(callback);
    }

    public static PropertyDeclarationSyntax PropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder>? propertyDeclarationCallback = null)
    {
        return PropertyDeclarationBuilder.CreateSyntax(typeCallback, identifier, propertyDeclarationCallback);
    }

    public static ArrowExpressionClauseSyntax ArrowExpressionClause(Action<IExpressionBuilder> expressionCallback)
    {
        return ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
    }

    public static EventDeclarationSyntax EventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null)
    {
        return EventDeclarationBuilder.CreateSyntax(typeCallback, identifier, eventDeclarationCallback);
    }

    public static IndexerDeclarationSyntax IndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null)
    {
        return IndexerDeclarationBuilder.CreateSyntax(typeCallback, indexerDeclarationCallback);
    }

    public static AccessorDeclarationSyntax AccessorDeclaration(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder>? accessorDeclarationCallback = null)
    {
        return AccessorDeclarationBuilder.CreateSyntax(kind, accessorDeclarationCallback);
    }

    public static BaseParameterSyntax BaseParameter(Action<IBaseParameterBuilder> callback)
    {
        return BaseParameterBuilder.CreateSyntax(callback);
    }

    public static ParameterSyntax Parameter(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        return ParameterBuilder.CreateSyntax(identifier, parameterCallback);
    }

    public static FunctionPointerParameterSyntax FunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null)
    {
        return FunctionPointerParameterBuilder.CreateSyntax(typeCallback, functionPointerParameterCallback);
    }

    public static IncompleteMemberSyntax IncompleteMember(Action<IIncompleteMemberBuilder>? incompleteMemberCallback = null)
    {
        return IncompleteMemberBuilder.CreateSyntax(incompleteMemberCallback);
    }

    public static SkippedTokensTriviaSyntax SkippedTokensTrivia(Action<ISkippedTokensTriviaBuilder>? skippedTokensTriviaCallback = null)
    {
        return SkippedTokensTriviaBuilder.CreateSyntax(skippedTokensTriviaCallback);
    }

    public static DocumentationCommentTriviaSyntax DocumentationCommentTrivia(DocumentationCommentTriviaKind kind, Action<IDocumentationCommentTriviaBuilder>? documentationCommentTriviaCallback = null)
    {
        return DocumentationCommentTriviaBuilder.CreateSyntax(kind, documentationCommentTriviaCallback);
    }

    public static CrefSyntax Cref(Action<ICrefBuilder> callback)
    {
        return CrefBuilder.CreateSyntax(callback);
    }

    public static TypeCrefSyntax TypeCref(Action<ITypeBuilder> typeCallback)
    {
        return TypeCrefBuilder.CreateSyntax(typeCallback);
    }

    public static QualifiedCrefSyntax QualifiedCref(Action<ITypeBuilder> containerCallback, Action<IMemberCrefBuilder> memberCallback)
    {
        return QualifiedCrefBuilder.CreateSyntax(containerCallback, memberCallback);
    }

    public static MemberCrefSyntax MemberCref(Action<IMemberCrefBuilder> callback)
    {
        return MemberCrefBuilder.CreateSyntax(callback);
    }

    public static NameMemberCrefSyntax NameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder>? nameMemberCrefCallback = null)
    {
        return NameMemberCrefBuilder.CreateSyntax(nameCallback, nameMemberCrefCallback);
    }

    public static IndexerMemberCrefSyntax IndexerMemberCref(Action<IIndexerMemberCrefBuilder>? indexerMemberCrefCallback = null)
    {
        return IndexerMemberCrefBuilder.CreateSyntax(indexerMemberCrefCallback);
    }

    public static OperatorMemberCrefSyntax OperatorMemberCref(OperatorMemberCrefOperatorToken operatorMemberCrefOperatorToken, Action<IOperatorMemberCrefBuilder>? operatorMemberCrefCallback = null)
    {
        return OperatorMemberCrefBuilder.CreateSyntax(operatorMemberCrefOperatorToken, operatorMemberCrefCallback);
    }

    public static ConversionOperatorMemberCrefSyntax ConversionOperatorMemberCref(ConversionOperatorMemberCrefImplicitOrExplicitKeyword conversionOperatorMemberCrefImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorMemberCrefBuilder>? conversionOperatorMemberCrefCallback = null)
    {
        return ConversionOperatorMemberCrefBuilder.CreateSyntax(conversionOperatorMemberCrefImplicitOrExplicitKeyword, typeCallback, conversionOperatorMemberCrefCallback);
    }

    public static CrefParameterSyntax CrefParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder>? crefParameterCallback = null)
    {
        return CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
    }

    public static XmlNodeSyntax XmlNode(Action<IXmlNodeBuilder> callback)
    {
        return XmlNodeBuilder.CreateSyntax(callback);
    }

    public static XmlElementSyntax XmlElement(string nameStartTagLocalName, Action<IXmlNameBuilder> nameStartTagXmlNameCallback, Action<IXmlElementStartTagBuilder> startTagXmlElementStartTagCallback, string nameEndTagLocalName, Action<IXmlNameBuilder> nameEndTagXmlNameCallback, Action<IXmlElementBuilder> xmlElementCallback)
    {
        return XmlElementBuilder.CreateSyntax(nameStartTagLocalName, nameStartTagXmlNameCallback, startTagXmlElementStartTagCallback, nameEndTagLocalName, nameEndTagXmlNameCallback, xmlElementCallback);
    }

    public static XmlElementStartTagSyntax XmlElementStartTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlElementStartTagBuilder> xmlElementStartTagCallback)
    {
        return XmlElementStartTagBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlElementStartTagCallback);
    }

    public static XmlElementEndTagSyntax XmlElementEndTag(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback)
    {
        return XmlElementEndTagBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
    }

    public static XmlEmptyElementSyntax XmlEmptyElement(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlEmptyElementBuilder> xmlEmptyElementCallback)
    {
        return XmlEmptyElementBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlEmptyElementCallback);
    }

    public static XmlNameSyntax XmlName(string localName, Action<IXmlNameBuilder>? xmlNameCallback = null)
    {
        return XmlNameBuilder.CreateSyntax(localName, xmlNameCallback);
    }

    public static XmlPrefixSyntax XmlPrefix(string prefix)
    {
        return XmlPrefixBuilder.CreateSyntax(prefix);
    }

    public static XmlAttributeSyntax XmlAttribute(Action<IXmlAttributeBuilder> callback)
    {
        return XmlAttributeBuilder.CreateSyntax(callback);
    }

    public static XmlTextAttributeSyntax XmlTextAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlTextAttributeStartQuoteToken xmlTextAttributeStartQuoteToken, XmlTextAttributeEndQuoteToken xmlTextAttributeEndQuoteToken, Action<IXmlTextAttributeBuilder> xmlTextAttributeCallback)
    {
        return XmlTextAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlTextAttributeStartQuoteToken, xmlTextAttributeEndQuoteToken, xmlTextAttributeCallback);
    }

    public static XmlCrefAttributeSyntax XmlCrefAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlCrefAttributeStartQuoteToken xmlCrefAttributeStartQuoteToken, Action<ICrefBuilder> crefCallback, XmlCrefAttributeEndQuoteToken xmlCrefAttributeEndQuoteToken)
    {
        return XmlCrefAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlCrefAttributeStartQuoteToken, crefCallback, xmlCrefAttributeEndQuoteToken);
    }

    public static XmlNameAttributeSyntax XmlNameAttribute(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, XmlNameAttributeStartQuoteToken xmlNameAttributeStartQuoteToken, string identifierIdentifier, XmlNameAttributeEndQuoteToken xmlNameAttributeEndQuoteToken)
    {
        return XmlNameAttributeBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlNameAttributeStartQuoteToken, identifierIdentifier, xmlNameAttributeEndQuoteToken);
    }

    public static XmlTextSyntax XmlText(Action<IXmlTextBuilder>? xmlTextCallback = null)
    {
        return XmlTextBuilder.CreateSyntax(xmlTextCallback);
    }

    public static XmlCDataSectionSyntax XmlCDataSection(Action<IXmlCDataSectionBuilder>? xmlCDataSectionCallback = null)
    {
        return XmlCDataSectionBuilder.CreateSyntax(xmlCDataSectionCallback);
    }

    public static XmlProcessingInstructionSyntax XmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback)
    {
        return XmlProcessingInstructionBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback, xmlProcessingInstructionCallback);
    }

    public static XmlCommentSyntax XmlComment(Action<IXmlCommentBuilder>? xmlCommentCallback = null)
    {
        return XmlCommentBuilder.CreateSyntax(xmlCommentCallback);
    }

    public static DirectiveTriviaSyntax DirectiveTrivia(Action<IDirectiveTriviaBuilder> callback)
    {
        return DirectiveTriviaBuilder.CreateSyntax(callback);
    }

    public static BranchingDirectiveTriviaSyntax BranchingDirectiveTrivia(Action<IBranchingDirectiveTriviaBuilder> callback)
    {
        return BranchingDirectiveTriviaBuilder.CreateSyntax(callback);
    }

    public static ConditionalDirectiveTriviaSyntax ConditionalDirectiveTrivia(Action<IConditionalDirectiveTriviaBuilder> callback)
    {
        return ConditionalDirectiveTriviaBuilder.CreateSyntax(callback);
    }

    public static IfDirectiveTriviaSyntax IfDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        return IfDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }

    public static ElifDirectiveTriviaSyntax ElifDirectiveTrivia(Action<IExpressionBuilder> conditionCallback, bool isActive, bool branchTaken, bool conditionValue)
    {
        return ElifDirectiveTriviaBuilder.CreateSyntax(conditionCallback, isActive, branchTaken, conditionValue);
    }

    public static ElseDirectiveTriviaSyntax ElseDirectiveTrivia(bool isActive, bool branchTaken)
    {
        return ElseDirectiveTriviaBuilder.CreateSyntax(isActive, branchTaken);
    }

    public static EndIfDirectiveTriviaSyntax EndIfDirectiveTrivia(bool isActive)
    {
        return EndIfDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static RegionDirectiveTriviaSyntax RegionDirectiveTrivia(bool isActive)
    {
        return RegionDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static EndRegionDirectiveTriviaSyntax EndRegionDirectiveTrivia(bool isActive)
    {
        return EndRegionDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static ErrorDirectiveTriviaSyntax ErrorDirectiveTrivia(bool isActive)
    {
        return ErrorDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static WarningDirectiveTriviaSyntax WarningDirectiveTrivia(bool isActive)
    {
        return WarningDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static BadDirectiveTriviaSyntax BadDirectiveTrivia(string identifier, bool isActive)
    {
        return BadDirectiveTriviaBuilder.CreateSyntax(identifier, isActive);
    }

    public static DefineDirectiveTriviaSyntax DefineDirectiveTrivia(string name, bool isActive)
    {
        return DefineDirectiveTriviaBuilder.CreateSyntax(name, isActive);
    }

    public static UndefDirectiveTriviaSyntax UndefDirectiveTrivia(string name, bool isActive)
    {
        return UndefDirectiveTriviaBuilder.CreateSyntax(name, isActive);
    }

    public static LineOrSpanDirectiveTriviaSyntax LineOrSpanDirectiveTrivia(Action<ILineOrSpanDirectiveTriviaBuilder> callback)
    {
        return LineOrSpanDirectiveTriviaBuilder.CreateSyntax(callback);
    }

    public static LineDirectiveTriviaSyntax LineDirectiveTrivia(LineDirectiveTriviaLine lineDirectiveTriviaLine, bool isActive, Action<ILineDirectiveTriviaBuilder>? lineDirectiveTriviaCallback = null)
    {
        return LineDirectiveTriviaBuilder.CreateSyntax(lineDirectiveTriviaLine, isActive, lineDirectiveTriviaCallback);
    }

    public static LineDirectivePositionSyntax LineDirectivePosition(int line, int character)
    {
        return LineDirectivePositionBuilder.CreateSyntax(line, character);
    }

    public static LineSpanDirectiveTriviaSyntax LineSpanDirectiveTrivia(int startLine, int startCharacter, int endLine, int endCharacter, string file, bool isActive, Action<ILineSpanDirectiveTriviaBuilder> lineSpanDirectiveTriviaCallback)
    {
        return LineSpanDirectiveTriviaBuilder.CreateSyntax(startLine, startCharacter, endLine, endCharacter, file, isActive, lineSpanDirectiveTriviaCallback);
    }

    public static PragmaWarningDirectiveTriviaSyntax PragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaDisableOrRestoreKeyword pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, bool isActive, Action<IPragmaWarningDirectiveTriviaBuilder>? pragmaWarningDirectiveTriviaCallback = null)
    {
        return PragmaWarningDirectiveTriviaBuilder.CreateSyntax(pragmaWarningDirectiveTriviaDisableOrRestoreKeyword, isActive, pragmaWarningDirectiveTriviaCallback);
    }

    public static PragmaChecksumDirectiveTriviaSyntax PragmaChecksumDirectiveTrivia(string file, string guid, string bytes, bool isActive)
    {
        return PragmaChecksumDirectiveTriviaBuilder.CreateSyntax(file, guid, bytes, isActive);
    }

    public static ReferenceDirectiveTriviaSyntax ReferenceDirectiveTrivia(string file, bool isActive)
    {
        return ReferenceDirectiveTriviaBuilder.CreateSyntax(file, isActive);
    }

    public static LoadDirectiveTriviaSyntax LoadDirectiveTrivia(string file, bool isActive)
    {
        return LoadDirectiveTriviaBuilder.CreateSyntax(file, isActive);
    }

    public static ShebangDirectiveTriviaSyntax ShebangDirectiveTrivia(bool isActive)
    {
        return ShebangDirectiveTriviaBuilder.CreateSyntax(isActive);
    }

    public static NullableDirectiveTriviaSyntax NullableDirectiveTrivia(NullableDirectiveTriviaSettingToken nullableDirectiveTriviaSettingToken, bool isActive, Action<INullableDirectiveTriviaBuilder>? nullableDirectiveTriviaCallback = null)
    {
        return NullableDirectiveTriviaBuilder.CreateSyntax(nullableDirectiveTriviaSettingToken, isActive, nullableDirectiveTriviaCallback);
    }
}