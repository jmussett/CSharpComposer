using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerParameterBuilder : IBaseParameterBuilder<IFunctionPointerParameterBuilder>
{
}

public partial class FunctionPointerParameterBuilder : IFunctionPointerParameterBuilder
{
    public FunctionPointerParameterSyntax Syntax { get; set; }

    public FunctionPointerParameterBuilder(FunctionPointerParameterSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerParameterSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder>? functionPointerParameterCallback = null)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.FunctionPointerParameter(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), typeSyntax);
        var builder = new FunctionPointerParameterBuilder(syntax);
        functionPointerParameterCallback?.Invoke(builder);
        return builder.Syntax;
    }

    public IFunctionPointerParameterBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder>? attributeCallback = null)
    {
        var attribute = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFunctionPointerParameterBuilder AddAttribute(AttributeSyntax attribute)
    {
        var separatedSyntaxList = SyntaxFactory.SeparatedList(new[] { attribute });
        var attributeListSyntax = SyntaxFactory.AttributeList(separatedSyntaxList);
        Syntax = Syntax.AddAttributeLists(attributeListSyntax);
        return this;
    }

    public IFunctionPointerParameterBuilder AddModifierToken(SyntaxKind modifier)
    {
        Syntax = Syntax.AddModifiers(SyntaxFactory.Token(modifier));
        return this;
    }
}