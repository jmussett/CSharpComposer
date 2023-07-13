using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEnumMemberDeclarationBuilder : IMemberDeclarationBuilder<IEnumMemberDeclarationBuilder>
{
    IEnumMemberDeclarationBuilder WithEqualsValue(Action<IExpressionBuilder> valueCallback);
    IEnumMemberDeclarationBuilder WithEqualsValue(EqualsValueClauseSyntax equalsValue);
}

public partial class EnumMemberDeclarationBuilder : IEnumMemberDeclarationBuilder
{
    public EnumMemberDeclarationSyntax Syntax { get; set; }

    public EnumMemberDeclarationBuilder(EnumMemberDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EnumMemberDeclarationSyntax CreateSyntax(string identifier, Action<IEnumMemberDeclarationBuilder>? enumMemberDeclarationCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.EnumMemberDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), identifierToken, default(EqualsValueClauseSyntax));
        var builder = new EnumMemberDeclarationBuilder(syntax);
        enumMemberDeclarationCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IEnumMemberDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEnumMemberDeclarationBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IEnumMemberDeclarationBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }

    public IEnumMemberDeclarationBuilder WithEqualsValue(Action<IExpressionBuilder> valueCallback)
    {
        var equalsValueSyntax = EqualsValueClauseBuilder.CreateSyntax(valueCallback);
        Syntax = Syntax.WithEqualsValue(equalsValueSyntax);
        return this;
    }

    public IEnumMemberDeclarationBuilder WithEqualsValue(EqualsValueClauseSyntax equalsValue)
    {
        Syntax = Syntax.WithEqualsValue(equalsValue);
        return this;
    }
}