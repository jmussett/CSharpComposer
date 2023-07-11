using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;

public partial interface INameBuilder
{
    void ParseName(string text);
}

public partial class NameBuilder
{
    public void ParseName(string text)
    {
        Syntax = SyntaxFactory.ParseName(text);
    }
}

public partial interface ITypeBuilder
{
    void ParseTypeName(string text);
}

public partial class TypeBuilder
{
    public void ParseTypeName(string text)
    {
        Syntax = SyntaxFactory.ParseTypeName(text);
    }
}

public partial interface IExpressionBuilder
{
    void ParseExpression(string text);
}

public partial class ExpressionBuilder
{
    public void ParseExpression(string text)
    {
        Syntax = SyntaxFactory.ParseExpression(text);
    }
}

public partial interface IStatementBuilder
{
    void ParseStatement(string text);
}

public static class AddStatementExtensions
{
    public static TBuilder AddStatement<TBuilder>(this IAddStatement<TBuilder> addStatement, string text)
    {
        var syntax = SyntaxFactory.ParseStatement(text);

        return addStatement.AddStatement(syntax);
    }
}

public partial class StatementBuilder
{
    public void ParseStatement(string text)
    {
        Syntax = SyntaxFactory.ParseStatement(text);
    }
}

public partial interface IMemberDeclarationBuilder
{
    void ParseMemberDeclaration(string text);
}

public partial class MemberDeclarationBuilder
{
    public void ParseMemberDeclaration(string text)
    {
        Syntax = SyntaxFactory.ParseMemberDeclaration(text);
    }
}

public partial interface ICompilationUnitBuilder
{
    void ParseCompilationUnit(string text);
}

public partial class CompilationUnitBuilder
{
    public void ParseCompilationUnit(string text)
    {
        Syntax = SyntaxFactory.ParseCompilationUnit(text);
    }
}

public static partial class CSharpFactory
{
    public static NameSyntax Name(string name)
    {
        return NameBuilder.CreateSyntax(x => x.ParseName(name));
    }

    public static TypeSyntax Type(string typeName)
    {
        return TypeBuilder.CreateSyntax(x => x.ParseTypeName(typeName));
    }

    public static ExpressionSyntax Expression(string expression)
    {
        return ExpressionBuilder.CreateSyntax(x => x.ParseExpression(expression));
    }

    public static StatementSyntax Statement(string statement)
    {
        return StatementBuilder.CreateSyntax(x => x.ParseStatement(statement));
    }

    public static MemberDeclarationSyntax MemberDeclaration(string memberDeclaration)
    {
        return MemberDeclarationBuilder.CreateSyntax(x => x.ParseMemberDeclaration(memberDeclaration));
    }

    public static CompilationUnitSyntax CompilationUnit(string compilationUnit)
    {
        return CompilationUnitBuilder.CreateSyntax(x => x.ParseCompilationUnit(compilationUnit));
    }
}