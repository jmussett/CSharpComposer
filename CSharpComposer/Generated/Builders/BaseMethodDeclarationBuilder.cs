using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseMethodDeclarationBuilder
{
    void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder>? methodDeclarationCallback = null);
    void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder>? operatorDeclarationCallback = null);
    void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder>? conversionOperatorDeclarationCallback = null);
    void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder>? constructorDeclarationCallback = null);
    void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder>? destructorDeclarationCallback = null);
}

public partial interface IBaseMethodDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>, IAddParameter<TBuilder>
{
}

internal partial class BaseMethodDeclarationBuilder : IBaseMethodDeclarationBuilder
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

    public void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder>? methodDeclarationCallback = null)
    {
        Syntax = MethodDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, methodDeclarationCallback);
    }

    public void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder>? operatorDeclarationCallback = null)
    {
        Syntax = OperatorDeclarationBuilder.CreateSyntax(returnTypeCallback, operatorDeclarationOperatorToken, operatorDeclarationCallback);
    }

    public void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder>? conversionOperatorDeclarationCallback = null)
    {
        Syntax = ConversionOperatorDeclarationBuilder.CreateSyntax(conversionOperatorDeclarationImplicitOrExplicitKeyword, typeCallback, conversionOperatorDeclarationCallback);
    }

    public void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder>? constructorDeclarationCallback = null)
    {
        Syntax = ConstructorDeclarationBuilder.CreateSyntax(identifier, constructorDeclarationCallback);
    }

    public void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder>? destructorDeclarationCallback = null)
    {
        Syntax = DestructorDeclarationBuilder.CreateSyntax(identifier, destructorDeclarationCallback);
    }
}