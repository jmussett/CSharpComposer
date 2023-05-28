using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IGlobalStatementBuilder : IMemberDeclarationBuilder<IGlobalStatementBuilder>
{
}

public interface IWithGlobalStatementBuilder<TBuilder>
{
    TBuilder WithGlobalStatement(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder> globalStatementCallback);
    TBuilder WithGlobalStatement(GlobalStatementSyntax globalStatementSyntax);
}

public partial class GlobalStatementBuilder : IGlobalStatementBuilder
{
    public GlobalStatementSyntax Syntax { get; set; }

    public GlobalStatementBuilder(GlobalStatementSyntax syntax)
    {
        Syntax = syntax;
    }

    public static GlobalStatementSyntax CreateSyntax(Action<IStatementBuilder> statementCallback, Action<IGlobalStatementBuilder> globalStatementCallback)
    {
        var statementSyntax = StatementBuilder.CreateSyntax(statementCallback);
        var syntax = SyntaxFactory.GlobalStatement(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), statementSyntax);
        var builder = new GlobalStatementBuilder(syntax);
        globalStatementCallback(builder);
        return builder.Syntax;
    }

    public IGlobalStatementBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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
}