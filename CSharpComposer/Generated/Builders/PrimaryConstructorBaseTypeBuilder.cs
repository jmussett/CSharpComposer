using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPrimaryConstructorBaseTypeBuilder : IAddArgument<IPrimaryConstructorBaseTypeBuilder>
{
}

public partial class PrimaryConstructorBaseTypeBuilder : IPrimaryConstructorBaseTypeBuilder
{
    public PrimaryConstructorBaseTypeSyntax Syntax { get; set; }

    public PrimaryConstructorBaseTypeBuilder(PrimaryConstructorBaseTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PrimaryConstructorBaseTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder>? primaryConstructorBaseTypeCallback = null)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.PrimaryConstructorBaseType(typeSyntax, argumentListSyntax);
        var builder = new PrimaryConstructorBaseTypeBuilder(syntax);
        primaryConstructorBaseTypeCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IPrimaryConstructorBaseTypeBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder>? argumentCallback = null)
    {
        var argument = ArgumentBuilder.CreateSyntax(expressionCallback, argumentCallback);
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }

    public IPrimaryConstructorBaseTypeBuilder AddArgument(ArgumentSyntax argument)
    {
        Syntax = Syntax.AddArgumentListArguments(argument);
        return this;
    }
}