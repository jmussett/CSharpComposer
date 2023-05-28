using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IBaseTypeDeclarationBuilder
{
    void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback);
    void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback);
    void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback);
    void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback);
    void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback);
}

public partial interface IBaseTypeDeclarationBuilder<TBuilder> : IMemberDeclarationBuilder<TBuilder>
{
    TBuilder AddBase(Action<IBaseTypeBuilder> typeCallback);
    TBuilder AddBase(BaseTypeSyntax type);
    TBuilder WithSemicolonToken();
}

public interface IWithBaseTypeDeclarationBuilder<TBuilder>
{
    TBuilder WithBaseTypeDeclaration(Action<IBaseTypeDeclarationBuilder> baseTypeDeclarationCallback);
    TBuilder WithBaseTypeDeclaration(BaseTypeDeclarationSyntax baseTypeDeclarationSyntax);
}

public partial class BaseTypeDeclarationBuilder : IBaseTypeDeclarationBuilder
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

    public void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback)
    {
        Syntax = EnumDeclarationBuilder.CreateSyntax(identifier, enumDeclarationCallback);
    }
}