using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IVariableDeclarationBuilder
{
    IVariableDeclarationBuilder AddVariableDeclarator(string identifier, Action<IVariableDeclaratorBuilder> variableDeclaratorCallback);
    IVariableDeclarationBuilder AddVariableDeclarator(VariableDeclaratorSyntax variable);
}

public partial class VariableDeclarationBuilder : IVariableDeclarationBuilder
{
    public VariableDeclarationSyntax Syntax { get; set; }

    public VariableDeclarationBuilder(VariableDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    public static VariableDeclarationSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder> variableDeclarationCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.VariableDeclaration(typeSyntax, default(SeparatedSyntaxList<VariableDeclaratorSyntax>));
        var builder = new VariableDeclarationBuilder(syntax);
        variableDeclarationCallback(builder);
        return builder.Syntax;
    }

    public IVariableDeclarationBuilder AddVariableDeclarator(string identifier, Action<IVariableDeclaratorBuilder> variableDeclaratorCallback)
    {
        var variable = VariableDeclaratorBuilder.CreateSyntax(identifier, variableDeclaratorCallback);
        Syntax = Syntax.AddVariables(variable);
        return this;
    }

    public IVariableDeclarationBuilder AddVariableDeclarator(VariableDeclaratorSyntax variable)
    {
        Syntax = Syntax.AddVariables(variable);
        return this;
    }
}