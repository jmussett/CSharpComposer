using Microsoft.CodeAnalysis.CSharp;

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