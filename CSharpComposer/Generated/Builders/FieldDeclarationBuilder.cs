using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFieldDeclarationBuilder
{
}

public interface IWithFieldDeclarationBuilder<TBuilder>
{
    TBuilder WithFieldDeclaration(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback);
    TBuilder WithFieldDeclaration(FieldDeclarationSyntax fieldDeclarationSyntax);
}

public partial class FieldDeclarationBuilder : IFieldDeclarationBuilder
{
    public FieldDeclarationSyntax Syntax { get; set; }

    public FieldDeclarationBuilder(FieldDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FieldDeclarationSyntax CreateSyntax(Action<ITypeBuilder> declarationTypeCallback, Action<IVariableDeclarationBuilder> declarationVariableDeclarationCallback, Action<IFieldDeclarationBuilder> fieldDeclarationCallback)
    {
        var declarationSyntax = VariableDeclarationBuilder.CreateSyntax(declarationTypeCallback, declarationVariableDeclarationCallback);
        var semicolonTokenToken = SyntaxFactory.Token(SyntaxKind.SemicolonToken);
        var syntax = SyntaxFactory.FieldDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), declarationSyntax, semicolonTokenToken);
        var builder = new FieldDeclarationBuilder(syntax);
        fieldDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IFieldDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFieldDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }
}