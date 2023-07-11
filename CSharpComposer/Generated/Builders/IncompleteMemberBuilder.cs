using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IIncompleteMemberBuilder : IMemberDeclarationBuilder<IIncompleteMemberBuilder>, IWithType<IIncompleteMemberBuilder>
{
}

public partial class IncompleteMemberBuilder : IIncompleteMemberBuilder
{
    public IncompleteMemberSyntax Syntax { get; set; }

    public IncompleteMemberBuilder(IncompleteMemberSyntax syntax)
    {
        Syntax = syntax;
    }

    public static IncompleteMemberSyntax CreateSyntax(Action<IIncompleteMemberBuilder>? incompleteMemberCallback = null)
    {
        var syntax = SyntaxFactory.IncompleteMember(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), default(TypeSyntax));
        var builder = new IncompleteMemberBuilder(syntax);
        incompleteMemberCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IIncompleteMemberBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIncompleteMemberBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IIncompleteMemberBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IIncompleteMemberBuilder WithType(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.WithType(typeSyntax);
        return this;
    }

    public IIncompleteMemberBuilder WithType(TypeSyntax type)
    {
        Syntax = Syntax.WithType(type);
        return this;
    }
}