using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpComposer;
public partial interface INameMemberCrefBuilder
{
    INameMemberCrefBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder> crefParameterCallback);
    INameMemberCrefBuilder AddParameter(CrefParameterSyntax parameter);
}

public interface IWithNameMemberCrefBuilder<TBuilder>
{
    TBuilder WithNameMemberCref(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder> nameMemberCrefCallback);
    TBuilder WithNameMemberCref(NameMemberCrefSyntax nameMemberCrefSyntax);
}

public partial class NameMemberCrefBuilder : INameMemberCrefBuilder
{
    public NameMemberCrefSyntax Syntax { get; set; }

    public NameMemberCrefBuilder(NameMemberCrefSyntax syntax)
    {
        Syntax = syntax;
    }

    public static NameMemberCrefSyntax CreateSyntax(Action<ITypeBuilder> nameCallback, Action<INameMemberCrefBuilder> nameMemberCrefCallback)
    {
        var nameSyntax = TypeBuilder.CreateSyntax(nameCallback);
        var syntax = SyntaxFactory.NameMemberCref(nameSyntax, default(CrefParameterListSyntax));
        var builder = new NameMemberCrefBuilder(syntax);
        nameMemberCrefCallback(builder);
        return builder.Syntax;
    }

    public INameMemberCrefBuilder AddParameter(Action<ITypeBuilder> typeCallback, Action<ICrefParameterBuilder> crefParameterCallback)
    {
        var parameter = CrefParameterBuilder.CreateSyntax(typeCallback, crefParameterCallback);
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }

    public INameMemberCrefBuilder AddParameter(CrefParameterSyntax parameter)
    {
        Syntax = Syntax.AddParametersParameters(parameter);
        return this;
    }
}