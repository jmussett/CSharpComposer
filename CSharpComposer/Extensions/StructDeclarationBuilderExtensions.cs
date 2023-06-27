namespace CSharpComposer.Extensions;

public static class StructDeclarationBuilderExtensions
{
    public static IStructDeclarationBuilder AddFieldDeclaration(this IStructDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback));
    }

    public static IStructDeclarationBuilder AddConstructorDeclaration(this IStructDeclarationBuilder builder, string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsConstructorDeclaration(identifier, constructorDeclarationCallback));
    }

    public static IStructDeclarationBuilder AddEventFieldDeclaration(this IStructDeclarationBuilder builder, Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsEventFieldDeclaration(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback));
    }

    public static IStructDeclarationBuilder AddOperatorDeclaration(this IStructDeclarationBuilder builder, Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        return builder.AddMemberDeclaration(x => x.AsOperatorDeclaration(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback));
    }
}
