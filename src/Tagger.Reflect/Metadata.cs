using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;

sealed class Metadata
{
    public Metadata()
    {
        Template = Maybe.Nothing<object>();
        Attributes = new Dictionary<string, IEnumerable<AttributeMeta>>();
        Properties = Enumerable.Empty<PropertyMeta>();
        Interfaces = Enumerable.Empty<Type>();
    }

    public Metadata(
        Maybe<object> template,
        IDictionary<string, IEnumerable<AttributeMeta>> attributes,
        IEnumerable<PropertyMeta> properties,
        IEnumerable<Type> interfaces)
    {
        Template = template;
        Attributes = attributes;
        Properties = properties;
        Interfaces = interfaces;
    }

    public Maybe<object> Template { get; private set; }

    public IDictionary<string, IEnumerable<AttributeMeta>> Attributes { get; private set; }

    public IEnumerable<PropertyMeta> Properties { get; private set; }

    public IEnumerable<Type> Interfaces { get; private set; }
}