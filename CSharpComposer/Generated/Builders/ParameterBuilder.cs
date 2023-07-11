using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IParameterBuilder : IBaseParameterBuilder<IParameterBuilder>, IWithType<IParameterBuilder>
{
    IParameterBuilder WithDefault(Action<IExpressionBuilder> valueCallback);
    IParameterBuilder WithDefault(EqualsValueClauseSyntax @default);
}

public interface IAddParameter<TBuilder>
{
    TBuilder AddParameter(ParameterSyntax parameterSyntax);
    TBuilder AddParameter(string identifier, Action<IParameterBuilder>? parameterCallback = null);
}

public partial class ParameterBuilder : IParameterBuilder
{
    public ParameterSyntax Syntax { get; set; }

    public ParameterBuilder(ParameterSyntax syntax)
    {
        Syntax = syntax;
    }

    public static ParameterSyntax CreateSyntax(string identifier, Action<IParameterBuilder>? parameterCallback = null)
    {
        var identifierToken = SyntaxFactory.Identifier(identifier);
        var syntax = SyntaxFactory.Parameter(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), default(TypeSyntax), identifierToken, default(EqualsValueClauseSyntax));
        var builder = new ParameterBuilder(syntax);
        parameterCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IParameterBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IParameterBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IParameterBuilder AddModifierToken(SyntaxToken modifier)
    {
        Syntax = Syntax.AddModifiers(modifier);
        return this;
    }

    public IParameterBuilder WithType(Action<ITypeBuilder> typeCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        Syntax = Syntax.WithType(typeSyntax);
        return this;
    }

    public IParameterBuilder WithType(TypeSyntax type)
    {
        Syntax = Syntax.WithType(type);
        return this;
    }

    public IParameterBuilder WithDefault(Action<IExpressionBuilder> valueCallback)
    {
        var defaultSyntax = EqualsValueClauseBuilder.CreateSyntax(valueCallback);
        Syntax = Syntax.WithDefault(defaultSyntax);
        return this;
    }

    public IParameterBuilder WithDefault(EqualsValueClauseSyntax @default)
    {
        Syntax = Syntax.WithDefault(@default);
        return this;
    }
}