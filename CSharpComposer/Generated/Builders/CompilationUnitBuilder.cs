using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface ICompilationUnitBuilder : IAddAttribute<ICompilationUnitBuilder>
{
    ICompilationUnitBuilder AddExternAliasDirective(string identifier);
    ICompilationUnitBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax @extern);
    ICompilationUnitBuilder AddUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback);
    ICompilationUnitBuilder AddUsingDirective(UsingDirectiveSyntax @using);
    ICompilationUnitBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback);
    ICompilationUnitBuilder AddMemberDeclaration(MemberDeclarationSyntax member);
}

public interface IWithCompilationUnit<TBuilder>
{
    TBuilder WithCompilationUnit(Action<ICompilationUnitBuilder> compilationUnitCallback);
    TBuilder WithCompilationUnit(CompilationUnitSyntax compilationUnitSyntax);
}

public interface IAddCompilationUnit<TBuilder>
{
    TBuilder AddCompilationUnit(Action<ICompilationUnitBuilder> compilationUnitCallback);
    TBuilder AddCompilationUnit(CompilationUnitSyntax compilationUnitSyntax);
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

    public ICompilationUnitBuilder AddExternAliasDirective(string identifier)
    {
        var @extern = ExternAliasDirectiveBuilder.CreateSyntax(identifier);
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public ICompilationUnitBuilder AddExternAliasDirective(ExternAliasDirectiveSyntax @extern)
    {
        Syntax = Syntax.AddExterns(@extern);
        return this;
    }

    public ICompilationUnitBuilder AddUsingDirective(Action<INameBuilder> nameCallback, Action<IUsingDirectiveBuilder> usingDirectiveCallback)
    {
        var @using = UsingDirectiveBuilder.CreateSyntax(nameCallback, usingDirectiveCallback);
        Syntax = Syntax.AddUsings(@using);
        return this;
    }

    public ICompilationUnitBuilder AddUsingDirective(UsingDirectiveSyntax @using)
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

    public ICompilationUnitBuilder AddMemberDeclaration(Action<IMemberDeclarationBuilder> memberCallback)
    {
        var member = MemberDeclarationBuilder.CreateSyntax(memberCallback);
        Syntax = Syntax.AddMembers(member);
        return this;
    }

    public ICompilationUnitBuilder AddMemberDeclaration(MemberDeclarationSyntax member)
    {
        Syntax = Syntax.AddMembers(member);
        return this;
    }
}