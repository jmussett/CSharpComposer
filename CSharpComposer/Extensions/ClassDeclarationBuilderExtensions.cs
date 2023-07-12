namespace CSharpComposer;

public static class ClassDeclarationBuilderExtensions
{
    public static IClassDeclarationBuilder AddFieldDeclaration(this IClassDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddFieldDeclaration(this IClassDeclarationBuilder builder, string declarationTypeName, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFieldDeclaration(x => x.ParseTypeName(declarationTypeName), declarationVariableDeclarationCallback, fieldDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddConstructorDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsConstructorDeclaration(identifier, constructorDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddDestructorDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IDestructorDeclarationBuilder> destructorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDestructorDeclaration(identifier, destructorDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddEventFieldDeclaration(this IClassDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEventFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddEventFieldDeclaration(this IClassDeclarationBuilder builder, string declarationType, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEventFieldDeclaration(x => x.ParseTypeName(declarationType), declarationVariableDeclarationCallback, eventFieldDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddOperatorDeclaration(this IClassDeclarationBuilder builder, Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsOperatorDeclaration(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddOperatorDeclaration(this IClassDeclarationBuilder builder, string returnTypeName, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsOperatorDeclaration(x => x.ParseTypeName(returnTypeName), operatorDeclarationOperatorToken, operatorDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddClassDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsClassDeclaration(identifier, classDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddStructDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsStructDeclaration(identifier, structDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddInterfaceDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsInterfaceDeclaration(identifier, interfaceDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddEnumDeclaration(this IClassDeclarationBuilder builder, string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEnumDeclaration(identifier, enumDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddDelegateDeclaration(this IClassDeclarationBuilder builder, Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(returnTypeCallback, identifier, delegateDeclarationCallback));
    }

    public static IClassDeclarationBuilder AddDelegateDeclaration(this IClassDeclarationBuilder builder, string returnTypeName, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsDelegateDeclaration(x => x.ParseTypeName(returnTypeName), identifier, delegateDeclarationCallback));
    }
}
