using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseMethodDeclarationBuilder
{
    void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback);
    void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback);
    void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder> conversionOperatorDeclarationCallback);
    void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback);
    void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder> destructorDeclarationCallback);
}

public partial interface IBaseMethodDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>
{
}

public interface IWithBaseMethodDeclarationBuilder<TBuilder>
{
    TBuilder WithBaseMethodDeclaration(Action<IBaseMethodDeclarationBuilder> baseMethodDeclarationCallback);
    TBuilder WithBaseMethodDeclaration(BaseMethodDeclarationSyntax baseMethodDeclarationSyntax);
}

public partial class BaseMethodDeclarationBuilder : IBaseMethodDeclarationBuilder
{
    public BaseMethodDeclarationSyntax? Syntax { get; set; }

    public static BaseMethodDeclarationSyntax CreateSyntax(Action<IBaseMethodDeclarationBuilder> callback)
    {
        var builder = new BaseMethodDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseMethodDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback)
    {
        Syntax = MethodDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, methodDeclarationCallback);
    }

    public void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback)
    {
        Syntax = OperatorDeclarationBuilder.CreateSyntax(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback);
    }

    public void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder> conversionOperatorDeclarationCallback)
    {
        Syntax = ConversionOperatorDeclarationBuilder.CreateSyntax(conversionOperatorDeclarationImplicitOrExplicitKeyword, typeCallback, conversionOperatorDeclarationCallback);
    }

    public void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback)
    {
        Syntax = ConstructorDeclarationBuilder.CreateSyntax(identifier, constructorDeclarationCallback);
    }

    public void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder> destructorDeclarationCallback)
    {
        Syntax = DestructorDeclarationBuilder.CreateSyntax(identifier, destructorDeclarationCallback);
    }
}