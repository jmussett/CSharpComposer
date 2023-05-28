using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface IXmlProcessingInstructionBuilder
{
}

public interface IWithXmlProcessingInstructionBuilder<TBuilder>
{
    TBuilder WithXmlProcessingInstruction(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback);
    TBuilder WithXmlProcessingInstruction(XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax);
}

public partial class XmlProcessingInstructionBuilder : IXmlProcessingInstructionBuilder
{
    public XmlProcessingInstructionSyntax Syntax { get; set; }

    public XmlProcessingInstructionBuilder(XmlProcessingInstructionSyntax syntax)
    {
        Syntax = syntax;
    }

    public static XmlProcessingInstructionSyntax CreateSyntax(string nameLocalName, Action<IXmlNameBuilder> nameXmlNameCallback, Action<IXmlProcessingInstructionBuilder> xmlProcessingInstructionCallback)
    {
        var startProcessingInstructionTokenToken = SyntaxFactory.Token(SyntaxKind.XmlProcessingInstructionStartToken);
        var nameSyntax = XmlNameBuilder.CreateSyntax(nameLocalName, nameXmlNameCallback);
        var endProcessingInstructionTokenToken = SyntaxFactory.Token(SyntaxKind.XmlProcessingInstructionEndToken);
        var syntax = SyntaxFactory.XmlProcessingInstruction(startProcessingInstructionTokenToken, nameSyntax, default(SyntaxTokenList), endProcessingInstructionTokenToken);
        var builder = new XmlProcessingInstructionBuilder(syntax);
        xmlProcessingInstructionCallback(builder);
        return builder.Syntax;
    }
}