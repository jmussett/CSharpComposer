using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IMemberDeclarationBuilder
{
    void AsGlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder> globalStatementCallback);
    void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback);
    void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback);
    void AsClassDeclaration(string identifier, Action<IClassDeclarationBuilder> classDeclarationCallback);
    void AsStructDeclaration(string identifier, Action<IStructDeclarationBuilder> structDeclarationCallback);
    void AsInterfaceDeclaration(string identifier, Action<IInterfaceDeclarationBuilder> interfaceDeclarationCallback);
    void AsRecordDeclaration(RecordDeclarationKind kind, string identifier, Action<IRecordDeclarationBuilder> recordDeclarationCallback);
    void AsEnumDeclaration(string identifier, Action<IEnumDeclarationBuilder> enumDeclarationCallback);
    void AsDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback);
    void AsEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback);
    void AsFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback);
    void AsEventFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IEventFieldDeclarationBuilder> eventFieldDeclarationCallback);
    void AsMethodDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IMethodDeclarationBuilder> methodDeclarationCallback);
    void AsOperatorDeclaration(Action<ITypeBuilder> returnTypeCallback, OperatorDeclarationOperatorToken operatorDeclarationOperatorToken, Action<IOperatorDeclarationBuilder> operatorDeclarationCallback);
    void AsConversionOperatorDeclaration(ConversionOperatorDeclarationImplicitOrExplicitKeyword conversionOperatorDeclarationImplicitOrExplicitKeyword, Action<ITypeBuilder> typeCallback, Action<IConversionOperatorDeclarationBuilder> conversionOperatorDeclarationCallback);
    void AsConstructorDeclaration(string identifier, Action<IConstructorDeclarationBuilder> constructorDeclarationCallback);
    void AsDestructorDeclaration(string identifier, Action<IDestructorDeclarationBuilder> destructorDeclarationCallback);
    void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback);
    void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback);
    void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback);
    void AsIncompleteMember(Action<IIncompleteMemberBuilder> incompleteMemberCallback);
}

public partial interface IMemberDeclarationBuilder<TBuilder>
{
}

public interface IWithMemberDeclarationBuilder<TBuilder>
{
    TBuilder WithMemberDeclaration(Action<IMemberDeclarationBuilder> memberDeclarationCallback);
    TBuilder WithMemberDeclaration(MemberDeclarationSyntax memberDeclarationSyntax);
}

public partial class MemberDeclarationBuilder : IMemberDeclarationBuilder
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

    public void AsGlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder> globalStatementCallback)
    {
        Syntax = GlobalStatementBuilder.CreateSyntax(statementCallback, globalStatementCallback);
    }

    public void AsNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<INamespaceDeclarationBuilder> namespaceDeclarationCallback)
    {
        Syntax = NamespaceDeclarationBuilder.CreateSyntax(nameCallback, namespaceDeclarationCallback);
    }

    public void AsFileScopedNamespaceDeclaration(Action<INameBuilder> nameCallback, Action<IFileScopedNamespaceDeclarationBuilder> fileScopedNamespaceDeclarationCallback)
    {
        Syntax = FileScopedNamespaceDeclarationBuilder.CreateSyntax(nameCallback, fileScopedNamespaceDeclarationCallback);
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

    public void AsDelegateDeclaration(Action<ITypeBuilder> returnTypeCallback, string identifier, Action<IDelegateDeclarationBuilder> delegateDeclarationCallback)
    {
        Syntax = DelegateDeclarationBuilder.CreateSyntax(returnTypeCallback, identifier, delegateDeclarationCallback);
    }

    public void AsEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback)
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

    public void AsPropertyDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IPropertyDeclarationBuilder> propertyDeclarationCallback)
    {
        Syntax = PropertyDeclarationBuilder.CreateSyntax(typeCallback, identifier, propertyDeclarationCallback);
    }

    public void AsEventDeclaration(Action<ITypeBuilder> typeCallback, string identifier, Action<IEventDeclarationBuilder> eventDeclarationCallback)
    {
        Syntax = EventDeclarationBuilder.CreateSyntax(typeCallback, identifier, eventDeclarationCallback);
    }

    public void AsIndexerDeclaration(Action<ITypeBuilder> typeCallback, Action<IIndexerDeclarationBuilder> indexerDeclarationCallback)
    {
        Syntax = IndexerDeclarationBuilder.CreateSyntax(typeCallback, indexerDeclarationCallback);
    }

    public void AsIncompleteMember(Action<IIncompleteMemberBuilder> incompleteMemberCallback)
    {
        Syntax = IncompleteMemberBuilder.CreateSyntax(incompleteMemberCallback);
    }
}