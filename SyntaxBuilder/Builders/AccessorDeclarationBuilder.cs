using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SyntaxBuilder.Types;
using SyntaxBuilder.Validators;

namespace SyntaxBuilder.Builders;

// TODO: For all builders: Add ISyntaxBuilder<T> with Syntax property
public interface IAccessorDeclarationBuilder
{
    IAccessorDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
    IAccessorDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null);
    IAccessorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> callback);
    IAccessorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback);
}

public sealed class AccessorDeclarationBuilder : IAccessorDeclarationBuilder
{
    private bool _includesBody = false;

    public AccessorDeclarationSyntax Syntax { get; set; }

    public AccessorDeclarationBuilder(AccessorDeclarationSyntax syntax)
    {
        Syntax = syntax;
    }

    // TODO: For all builders: Add CreateSyntax with child syntax parameters
    public static AccessorDeclarationSyntax CreateSyntax(AccessorType accessorType, Func<IAccessorDeclarationBuilder, IAccessorDeclarationBuilder>? accessorCallback = null)
    {
        var accessorSyntax = SyntaxFactory.AccessorDeclaration(
            accessorType switch
            {
                AccessorType.Get => SyntaxKind.GetAccessorDeclaration,
                AccessorType.Set => SyntaxKind.SetAccessorDeclaration,
                _ => throw new NotSupportedException("Accessor Type '{accessorType} is not supported'"),
            }
        );

        if (accessorCallback is not null)
        {
            var builder = new AccessorDeclarationBuilder(accessorSyntax).AssertInvoke(accessorCallback);

            accessorSyntax = builder.Syntax;

            if (!builder._includesBody)
            {
                accessorSyntax = accessorSyntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }
        }
        else
        {
            accessorSyntax = accessorSyntax.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        return accessorSyntax;
    }

    public IAccessorDeclarationBuilder WithBody(Func<IBlockBuilder, IBlockBuilder> callback)
    {
        var syntax = BlockBuilder.CreateSyntax(callback);

        Syntax = Syntax.WithBody(syntax);

        _includesBody = true;

        return this;
    }

    public IAccessorDeclarationBuilder WithExpressionBody(Action<IExpressionBuilder> expressionCallback)
    {
        var syntax = ExpressionBuilder.CreateSyntax(expressionCallback);

        Syntax = Syntax.WithExpressionBody(
            SyntaxFactory.ArrowExpressionClause(syntax)
        );

        return this;
    }

    // with modifier

    public IAccessorDeclarationBuilder WithAttribute(Type type, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        TypeValidator.ValidateAttributeForTarget(type, AttributeTargets.Method);

        // TODO: generic arguments?

        var typeName = type.FullName!;

        return WithAttribute(x => x.AsIdentifier(typeName), attributeCallback);
    }

    // TODO: name extension
    public IAccessorDeclarationBuilder WithAttribute(Func<INameBuilder, INameBuilder> nameCallback, Func<IAttributeBuilder, IAttributeBuilder>? attributeCallback = null)
    {
        var attributeSyntax = AttributeBuilder.CreateSyntax(nameCallback, attributeCallback);

        Syntax = Syntax.AddAttributeLists(
            SyntaxFactory.AttributeList(
                SyntaxFactory.SingletonSeparatedList(attributeSyntax)
            )
        );

        return this;
    }
}