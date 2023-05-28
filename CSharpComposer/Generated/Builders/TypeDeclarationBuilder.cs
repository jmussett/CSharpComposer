using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ITypeDeclarationBuilder
{
    void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback);
    void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback);
    void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback);
    void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback);
}

public partial interface ITypeDeclarationBuilder<TBuilder> : IBaseTypeDeclarationBuilder<TBuilder>
{
    TBuilder AddTypeParameter(string identifier, Action<ITypeParameterBuilder> typeParameterCallback);
    TBuilder AddTypeParameter(TypeParameterSyntax parameter);
}

public interface IWithTypeDeclarationBuilder<TBuilder>
{
    TBuilder WithTypeDeclaration(Action<ITypeDeclarationBuilder> typeDeclarationCallback);
    TBuilder WithTypeDeclaration(TypeDeclarationSyntax typeDeclarationSyntax);
}

public partial class TypeDeclarationBuilder : ITypeDeclarationBuilder
{
    public TypeDeclarationSyntax? Syntax { get; set; }

    public static TypeDeclarationSyntax CreateSyntax(Action<ITypeDeclarationBuilder> callback)
    {
        var builder = new TypeDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("TypeDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback)
    {
        Syntax = ClassDeclarationBuilder.CreateSyntax(identifier, classDeclarationCallback);
    }

    public void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback)
    {
        Syntax = StructDeclarationBuilder.CreateSyntax(identifier, structDeclarationCallback);
    }

    public void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback)
    {
        Syntax = InterfaceDeclarationBuilder.CreateSyntax(identifier, interfaceDeclarationCallback);
    }

    public void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback)
    {
        Syntax = RecordDeclarationBuilder.CreateSyntax(kind, identifier, recordDeclarationCallback);
    }
}