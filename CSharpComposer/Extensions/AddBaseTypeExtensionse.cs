﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpComposer;

public static class AddBaseTypeExtensionse
{
    public static TBuilder AddSimpleBaseType<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback)
        where TBuilder : IAddBaseType<TBuilder>
    {
        return builder.AddBaseType(x => x.AsSimpleBaseType(typeCallback));
    }

    public static TBuilder AddSimpleBaseType<TBuilder>(this TBuilder builder, string type)
        where TBuilder : IAddBaseType<TBuilder>
    {
        return builder.AddBaseType(x => x.AsSimpleBaseType(x => x.ParseTypeName(type)));
    }

    public static TBuilder AddPrimaryConstructorBaseType<TBuilder>(this TBuilder builder, Action<ITypeBuilder> typeCallback, Action<IPrimaryConstructorBaseTypeBuilder>? primaryConstructorBaseTypeCallback = null)
        where TBuilder : IAddBaseType<TBuilder>
    {
        return builder.AddBaseType(x => x.AsPrimaryConstructorBaseType(typeCallback, primaryConstructorBaseTypeCallback));
    }

    public static TBuilder AddPrimaryConstructorBaseType<TBuilder>(this TBuilder builder, string type, Action<IPrimaryConstructorBaseTypeBuilder>? primaryConstructorBaseTypeCallback = null)
        where TBuilder : IAddBaseType<TBuilder>
    {
        return builder.AddBaseType(x => x.AsPrimaryConstructorBaseType(x => x.ParseTypeName(type), primaryConstructorBaseTypeCallback));
    }
}
