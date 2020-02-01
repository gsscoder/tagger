using System;
using System.Collections.Generic;

static class MetadataExtensions
{
    public static Metadata WithInterfaces(this Metadata metadata, IEnumerable<Type> interfaces) =>
        new Metadata(
            metadata.Template,
            metadata.Attributes,
            metadata.Properties,
            interfaces);

    public static Metadata WithProperties(this Metadata metadata, IEnumerable<PropertyMeta> properties) =>
         new Metadata(
            metadata.Template,
            metadata.Attributes,
            properties,
            metadata.Interfaces);
}