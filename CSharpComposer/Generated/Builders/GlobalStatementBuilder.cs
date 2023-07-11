using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGlobalStatementBuilder : IMemberDeclarationBuilder<IGlobalStatementBuilder>
{
}

public partial class GlobalStatementBuilder : IGlobalStatementBuilder
{
    public GlobalStatementSyntax Syntax { get; set; }

    public GlobalStatementBuilder(GlobalStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GlobalStatementSyntax CreateSyntax(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder>? globalStatementCallback = null)
    {
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.GlobalStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), statementSyntax);
        var builder = new GlobalStatementBuilder(syntax);
        globalStatementCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IGlobalStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IGlobalStatementBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IGlobalStatementBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }
}