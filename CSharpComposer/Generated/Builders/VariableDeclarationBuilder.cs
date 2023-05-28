using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IVariableDeclarationBuilder
{
    IVariableDeclarationBuilder AddVariable(string identifier, Action<IVariableDeclaratorBuilder> variableDeclaratorCallback);
    IVariableDeclarationBuilder AddVariable(VariableDeclaratorSyntax variable);
}

public interface IWithVariableDeclarationBuilder<TBuilder>
{
    TBuilder WithVariableDeclaration(Action<ITypeBuilder> typeCallback, Action<IVariableDeclarationBuilder> variableDeclarationCallback);
    TBuilder WithVariableDeclaration(VariableDeclarationSyntax variableDeclarationSyntax);
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

    public IVariableDeclarationBuilder AddVariable(string identifier, Action<IVariableDeclaratorBuilder> variableDeclaratorCallback)
    {
        var variable = VariableDeclaratorBuilder.CreateSyntax(identifier, variableDeclaratorCallback);
        Syntax = Syntax.AddVariables(variable);
        return this;
    }

    public IVariableDeclarationBuilder AddVariable(VariableDeclaratorSyntax variable)
    {
        Syntax = Syntax.AddVariables(variable);
        return this;
    }
}