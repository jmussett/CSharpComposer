using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseTypeDeclarationBuilder
{
    void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder>? classDeclarationCallback = null);
    void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder>? structDeclarationCallback = null);
    void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder>? interfaceDeclarationCallback = null);
    void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder>? recordDeclarationCallback = null);
    void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder>? enumDeclarationCallback = null);
}

public partial interface IBaseTypeDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>, IAddBaseType<TBuilder>
{
    TBuilder WithSemicolonToken();
}

internal partial class BaseTypeDeclarationBuilder : IBaseTypeDeclarationBuilder
{
    public BaseTypeDeclarationSyntax? Syntax { get; set; }

    public static BaseTypeDeclarationSyntax CreateSyntax(Action<IBaseTypeDeclarationBuilder> callback)
    {
        var builder = new BaseTypeDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("BaseTypeDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder>? classDeclarationCallback = null)
    {
        Syntax = ClassDeclarationBuilder.CreateSyntax(identifier, classDeclarationCallback);
    }

    public void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder>? structDeclarationCallback = null)
    {
        Syntax = StructDeclarationBuilder.CreateSyntax(identifier, structDeclarationCallback);
    }

    public void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder>? interfaceDeclarationCallback = null)
    {
        Syntax = InterfaceDeclarationBuilder.CreateSyntax(identifier, interfaceDeclarationCallback);
    }

    public void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder>? recordDeclarationCallback = null)
    {
        Syntax = RecordDeclarationBuilder.CreateSyntax(kind, identifier, recordDeclarationCallback);
    }

    public void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder>? enumDeclarationCallback = null)
    {
        Syntax = EnumDeclarationBuilder.CreateSyntax(identifier, enumDeclarationCallback);
    }
}