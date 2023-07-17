using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IMemberDeclarationBuilder
{
    void FromSyntax(MemberDeclarationSyntax syntax);
    void AsGlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder>? globalStatementCallback = null);
    void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null);
    void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder>? fileScopedNamespaceDeclarationCallback = null);
    void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder>? classDeclarationCallback = null);
    void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder>? structDeclarationCallback = null);
    void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder>? interfaceDeclarationCallback = null);
    void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder>? recordDeclarationCallback = null);
    void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder>? enumDeclarationCallback = null);
    void AsDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder>? delegateDeclarationCallback = null);
    void AsEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null);
    void AsFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback);
    void AsEventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback);
    void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder>? methodDeclarationCallback = null);
    void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder>? operatorDeclarationCallback = null);
    void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder>? conversionOperatorDeclarationCallback = null);
    void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder>? constructorDeclarationCallback = null);
    void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder>? destructorDeclarationCallback = null);
    void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder>? propertyDeclarationCallback = null);
    void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null);
    void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null);
    void AsIncompleteMember(Action<IIncompleteMemberBuilder>? incompleteMemberCallback = null);
}

public partial interface IMemberDeclarationBuilder<TBuilder> : IAddAttribute<TBuilder>
{
    TBuilder AddModifierToken(SyntaxKind modifier);
}

internal partial class MemberDeclarationBuilder : IMemberDeclarationBuilder
{
    public MemberDeclarationSyntax? Syntax { get; set; }

    public static MemberDeclarationSyntax CreateSyntax(Action<IMemberDeclarationBuilder> callback)
    {
        var builder = new MemberDeclarationBuilder();
        callback(builder);
        if (builder.Syntax is null)
        {
            throw new InvalidOperationException("MemberDeclarationSyntax has not been specified");
        }

        return builder.Syntax;
    }

    public void FromSyntax(MemberDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public void AsGlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder>? globalStatementCallback = null)
    {
        Syntax = GlobalStatementBuilder.CreateSyntax(statementCallback, globalStatementCallback);
    }

    public void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder>? namespaceDeclarationCallback = null)
    {
        Syntax = NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceDeclarationCallback);
    }

    public void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder>? fileScopedNamespaceDeclarationCallback = null)
    {
        Syntax = FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, fileScopedNamespaceDeclarationCallback);
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

    public void AsDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder>? delegateDeclarationCallback = null)
    {
        Syntax = DelegateDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, delegateDeclarationCallback);
    }

    public void AsEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null)
    {
        Syntax = EnumMemberDeclarationBuilder.CreateSyntax(identifier, enumMemberDeclarationCallback);
    }

    public void AsFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        Syntax = FieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, fieldDeclarationCallback);
    }

    public void AsEventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback)
    {
        Syntax = EventFieldDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback, eventFieldDeclarationCallback);
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

    public void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder>? propertyDeclarationCallback = null)
    {
        Syntax = PropertyDeclarationBuilder.CreateSyntax(typeCallback, identifier, propertyDeclarationCallback);
    }

    public void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder>? eventDeclarationCallback = null)
    {
        Syntax = EventDeclarationBuilder.CreateSyntax(typeCallback, identifier, eventDeclarationCallback);
    }

    public void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder>? indexerDeclarationCallback = null)
    {
        Syntax = IndexerDeclarationBuilder.CreateSyntax(typeCallback, indexerDeclarationCallback);
    }

    public void AsIncompleteMember(Action<IIncompleteMemberBuilder>? incompleteMemberCallback = null)
    {
        Syntax = IncompleteMemberBuilder.CreateSyntax(incompleteMemberCallback);
    }
}