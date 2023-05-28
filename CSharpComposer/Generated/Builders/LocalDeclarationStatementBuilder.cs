using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ILocalDeclarationStatementBuilder : IStatementBuilder<ILocalDeclarationStatementBuilder>
{
    ILocalDeclarationStatementBuilder WithAwaitKeyword();
    ILocalDeclarationStatementBuilder WithUsingKeyword();
}

public interface IWithLocalDeclarationStatementBuilder<TBuilder>
{
    TBuilder WithLocalDeclarationStatement(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback);
    TBuilder WithLocalDeclarationStatement(LocalDeclarationStatementSyntax localDeclarationStatementSyntax);
}

public partial class LocalDeclarationStatementBuilder : ILocalDeclarationStatementBuilder
{
    public LocalDeclarationStatementSyntax Syntax { get; set; }

    public LocalDeclarationStatementBuilder(LocalDeclarationStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static LocalDeclarationStatementSyntax CreateSyntax(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<ILocalDeclarationStatementBuilder> localDeclarationStatementCallback)
    {
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.LocalDeclarationStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxToken), default(SyntaxToken), default(SyntaxTokenList), declarationSyntax, semicolonTokenToken);
        var builder = new LocalDeclarationStatementBuilder(syntax);
        localDeclarationStatementCallback(builder);
        return builder.Syntax;
    }

    public ILocalDeclarationStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalDeclarationStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ILocalDeclarationStatementBuilder WithAwaitKeyword()
    {
        Syntax = Syntax.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword));
        return this;
    }

    public ILocalDeclarationStatementBuilder WithUsingKeyword()
    {
        Syntax = Syntax.WithUsingKeyword(SyntaxFactory.Token(SyntaxKind.UsingKeyword));
        return this;
    }
}