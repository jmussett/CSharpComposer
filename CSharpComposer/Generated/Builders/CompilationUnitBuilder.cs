using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICompilationUnitBuilder
{
    ICompilationUnitBuilder AddExtern(string identifier);
    ICompilationUnitBuilder AddExtern(ExternAliasDirectiveSyntax @extern);
    ICompilationUnitBuilder AddUsing(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback);
    ICompilationUnitBuilder AddUsing(UsingDirectiveSyntax @using);
    ICompilationUnitBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback);
    ICompilationUnitBuilder AddAttribute(AttributeSyntax attribute);
    ICompilationUnitBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback);
    ICompilationUnitBuilder AddMember(MemberDeclarationSyntax member);
}

public interface IWithCompilationUnitBuilder<TBuilder>
{
    TBuilder WithCompilationUnit(Action<ICompilationUnitBuilder> compilationUnitCallback);
    TBuilder WithCompilationUnit(CompilationUnitSyntax compilationUnitSyntax);
}

public partial class CompilationUnitBuilder : ICompilationUnitBuilder
{
    public CompilationUnitSyntax Syntax { get; set; }

    public CompilationUnitBuilder(CompilationUnitSyntax syntax)
    {
        Syntax = syntax;
    }

    public static CompilationUnitSyntax CreateSyntax(Action<ICompilationUnitBuilder> compilationUnitCallback)
    {
        var endOfFileTokenToken = SyntaxFactory.Token(SyntaxKind.EndOfFileToken);
        var syntax = SyntaxFactory.CompilationUnit(default(SyntaxList<ExternAliasDirectiveSyntax>), default(SyntaxList<UsingDirectiveSyntax>), default(SyntaxList<AttributeListSyntax>), default(SyntaxList<MemberDeclarationSyntax>), endOfFileTokenToken);
        var builder = new CompilationUnitBuilder(syntax);
        compilationUnitCallback(builder);
        return builder.Syntax;
    }

    public ICompilationUnitBuilder AddExtern(string identifier)
    {
        var @extern = ExternAliasDirectiveBuilder.CreateSyntax(identifier);
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public ICompilationUnitBuilder AddExtern(ExternAliasDirectiveSyntax @extern)
    {
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public ICompilationUnitBuilder AddUsing(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback)
    {
        var @using = UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public ICompilationUnitBuilder AddUsing(UsingDirectiveSyntax @using)
    {
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public ICompilationUnitBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ICompilationUnitBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public ICompilationUnitBuilder AddMember(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public ICompilationUnitBuilder AddMember(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }
}