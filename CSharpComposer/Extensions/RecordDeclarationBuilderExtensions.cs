namespace CSharpComposer;

public static class RecordDeclarationBuilderExtensions
{
    public static IRecordDeclarationBuilder AddFieldDeclaration(this IRecordDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddFieldDeclaration(this IRecordDeclarationBuilder builder, string declarationTypeName, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFieldDeclaration(x => x.ParseTypeName(declarationTypeName), declarationVariableDeclarationCallback, fieldDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddConstructorDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsConstructorDeclaration(identifier, constructorDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddDestructorDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IDestructorDeclarationBuilder> destructorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDestructorDeclaration(identifier, destructorDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddEventFieldDeclaration(this IRecordDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEventFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddEventFieldDeclaration(this IRecordDeclarationBuilder builder, string declarationTypeName, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEventFieldDeclaration(x => x.ParseTypeName(declarationTypeName), declarationVariableDeclarationCallback, eventFieldDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddOperatorDeclaration(this IRecordDeclarationBuilder builder, Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsOperatorDeclaration(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddOperatorDeclaration(this IRecordDeclarationBuilder builder, string returnTypeName, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsOperatorDeclaration(x => x.ParseTypeName(returnTypeName), operatorDeclarationOperatorToken, operatorDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddClassDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsClassDeclaration(identifier, classDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddStructDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsStructDeclaration(identifier, structDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddInterfaceDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsInterfaceDeclaration(identifier, interfaceDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddEnumDeclaration(this IRecordDeclarationBuilder builder, string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEnumDeclaration(identifier, enumDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddDelegateDeclaration(this IRecordDeclarationBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(returnTypeCallback, identifier, delegateDeclarationCallback));
    }

    public static IRecordDeclarationBuilder AddDelegateDeclaration(this IRecordDeclarationBuilder builder, string returnTypeName, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(x => x.ParseTypeName(returnTypeName), identifier, delegateDeclarationCallback));
    }
}