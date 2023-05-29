using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IAccessorDeclarationBuilder : IWithBlock<IAccessorDeclarationBuilder>, IWithArrowExpressionClause<IAccessorDeclarationBuilder>
{
    IAccessorDeclarationBuilder WithArrowExpressionClause(Action<IExpressionBuilder> expressionCallback);
    IAccessorDeclarationBuilder WithArrowExpressionClause(ArrowExpressionClauseSyntax expressionBody);
    IAccessorDeclarationBuilder WithBlock(Action<IBlockBuilder> blockCallback);
    IAccessorDeclarationBuilder WithBlock(BlockSyntax body);
    IAccessorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback);
    IAccessorDeclarationBuilder AddAttribute(AttributeSyntax attribute);
    IAccessorDeclarationBuilder AddModifier(SyntaxToken modifier);
}

public interface IWithAccessorDeclaration<TBuilder>
{
    TBuilder WithAccessorDeclaration(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback);
    TBuilder WithAccessorDeclaration(AccessorDeclarationSyntax accessorDeclarationSyntax);
}

public partial class AccessorDeclarationBuilder : IAccessorDeclarationBuilder
{
    public AccessorDeclarationSyntax Syntax { get; set; }

    public AccessorDeclarationBuilder(AccessorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static AccessorDeclarationSyntax CreateSyntax(AccessorDeclarationKind kind, Action<IAccessorDeclarationBuilder> accessorDeclarationCallback)
    {
        var syntaxKind = kind switch
        {
            AccessorDeclarationKind.GetAccessorDeclaration => SyntaxKind.GetAccessorDeclaration,
            AccessorDeclarationKind.SetAccessorDeclaration => SyntaxKind.SetAccessorDeclaration,
            AccessorDeclarationKind.InitAccessorDeclaration => SyntaxKind.InitAccessorDeclaration,
            AccessorDeclarationKind.AddAccessorDeclaration => SyntaxKind.AddAccessorDeclaration,
            AccessorDeclarationKind.RemoveAccessorDeclaration => SyntaxKind.RemoveAccessorDeclaration,
            AccessorDeclarationKind.UnknownAccessorDeclaration => SyntaxKind.UnknownAccessorDeclaration,
            _ => throw new NotSupportedException()
        };
        var keywordToken = kind switch
        {
            AccessorDeclarationKind.GetAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.GetKeyword),
            AccessorDeclarationKind.SetAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.SetKeyword),
            AccessorDeclarationKind.InitAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.InitKeyword),
            AccessorDeclarationKind.AddAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.AddKeyword),
            AccessorDeclarationKind.RemoveAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.RemoveKeyword),
            AccessorDeclarationKind.UnknownAccessorDeclaration => SyntaxFactory.Token(SyntaxKind.IdentifierToken),
            _ => throw new NotSupportedException()
        };
        var syntax = SyntaxFactory.AccessorDeclaration(syntaxKind, default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keywordToken, null, null, default(SyntaxToken));
        var builder = new AccessorDeclarationBuilder(syntax);
        accessorDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IAccessorDeclarationBuilder WithArrowExpressionClause(Action<IExpressionBuilder> expressionCallback)
    {
        var expressionBodySyntax = ArrowExpressionClauseBuilder.CreateSyntax(expressionCallback);
        Syntax = Syntax.WithExpressionBody(expressionBodySyntax);
        return this;
    }

    public IAccessorDeclarationBuilder WithArrowExpressionClause(ArrowExpressionClauseSyntax expressionBody)
    {
        Syntax = Syntax.WithExpressionBody(expressionBody);
        return this;
    }

    public IAccessorDeclarationBuilder WithBlock(Action<IBlockBuilder> blockCallback)
    {
        var bodySyntax = BlockBuilder.CreateSyntax(blockCallback);
        Syntax = Syntax.WithBody(bodySyntax);
        return this;
    }

    public IAccessorDeclarationBuilder WithBlock(BlockSyntax body)
    {
        Syntax = Syntax.WithBody(body);
        return this;
    }

    public IAccessorDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IAccessorDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IAccessorDeclarationBuilder AddModifier(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }
}