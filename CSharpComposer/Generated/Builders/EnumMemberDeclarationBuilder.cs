using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IEnumMemberDeclarationBuilder : IMemberDeclarationBuilder<IEnumMemberDeclarationBuilder>, IAddAttribute<IEnumMemberDeclarationBuilder>
{
    IEnumMemberDeclarationBuilder AddModifierToken(SyntaxToken modifier);
    IEnumMemberDeclarationBuilder WithEqualsValue(Action<IExpressionBuilder> valueCallback);
    IEnumMemberDeclarationBuilder WithEqualsValue(EqualsValueClauseSyntax equalsValue);
}

public interface IWithEnumMemberDeclaration<TBuilder>
{
    TBuilder WithEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback);
    TBuilder WithEnumMemberDeclaration(EnumMemberDeclarationSyntax enumMemberDeclarationSyntax);
}

public interface IAddEnumMemberDeclaration<TBuilder>
{
    TBuilder AddEnumMemberDeclaration(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback);
    TBuilder AddEnumMemberDeclaration(EnumMemberDeclarationSyntax enumMemberDeclarationSyntax);
}

public partial class EnumMemberDeclarationBuilder : IEnumMemberDeclarationBuilder
{
    public EnumMemberDeclarationSyntax Syntax { get; set; }

    public EnumMemberDeclarationBuilder(EnumMemberDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static EnumMemberDeclarationSyntax CreateSyntax(string identifier, Action<IEnumMemberDeclarationBuilder> enumMemberDeclarationCallback)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.EnumMemberDeclaration(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), identifierToken, default(EqualsValueClauseSyntax));
        var builder = new EnumMemberDeclarationBuilder(syntax);
        enumMemberDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IEnumMemberDeclarationBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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

    public IEnumMemberDeclarationBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
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