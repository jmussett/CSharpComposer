using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IPrimaryConstructorBaseTypeBuilder
{
    IPrimaryConstructorBaseTypeBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback);
    IPrimaryConstructorBaseTypeBuilder AddArgument(ArgumentSyntax argument);
}

public interface IWithPrimaryConstructorBaseTypeBuilder<TBuilder>
{
    TBuilder WithPrimaryConstructorBaseType(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder> primaryConstructorBaseTypeCallback);
    TBuilder WithPrimaryConstructorBaseType(PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax);
}

public partial class PrimaryConstructorBaseTypeBuilder : IPrimaryConstructorBaseTypeBuilder
{
    public PrimaryConstructorBaseTypeSyntax Syntax { get; set; }

    public PrimaryConstructorBaseTypeBuilder(PrimaryConstructorBaseTypeSyntax syntax)
    {
        Syntax = syntax;
    }

    public static PrimaryConstructorBaseTypeSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder> primaryConstructorBaseTypeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var argumentListSyntax = SyntaxFactory.ArgumentList();
        var syntax = SyntaxFactory.PrimaryConstructorBaseType(typeSyntax, argumentListSyntax);
        var builder = new PrimaryConstructorBaseTypeBuilder(syntax);
        primaryConstructorBaseTypeCallback(builder);
        return builder.Syntax;
    }

    public IPrimaryConstructorBaseTypeBuilder AddArgument(Action<IExpressionBuilder> expressionCallback, Action<IArgumentBuilder> argumentCallback)
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