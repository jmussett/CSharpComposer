using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IFunctionPointerParameterBuilder : IBaseParameterBuilder<IFunctionPointerParameterBuilder>
{
}

public interface IWithFunctionPointerParameterBuilder<TBuilder>
{
    TBuilder WithFunctionPointerParameter(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback);
    TBuilder WithFunctionPointerParameter(FunctionPointerParameterSyntax functionPointerParameterSyntax);
}

public partial class FunctionPointerParameterBuilder : IFunctionPointerParameterBuilder
{
    public FunctionPointerParameterSyntax Syntax { get; set; }

    public FunctionPointerParameterBuilder(FunctionPointerParameterSyntax syntax)
    {
        Syntax = syntax;
    }

    public static FunctionPointerParameterSyntax CreateSyntax(Action<ITypeBuilder> typeCallback, Action<IFunctionPointerParameterBuilder> functionPointerParameterCallback)
    {
        var typeSyntax = TypeBuilder.CreateSyntax(typeCallback);
        var syntax = SyntaxFactory.FunctionPointerParameter(default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), typeSyntax);
        var builder = new FunctionPointerParameterBuilder(syntax);
        functionPointerParameterCallback(builder);
        return builder.Syntax;
    }

    public IFunctionPointerParameterBuilder AddAttribute(Action<INameBuilder> nameCallback, Action<IAttributeBuilder> attributeCallback)
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
}