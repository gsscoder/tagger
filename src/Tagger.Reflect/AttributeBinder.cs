using System;
using System.Collections.Generic;

public sealed class AttributeBinder
{
    public AttributeBinder()
    {
        PropertyValues = new Dictionary<string, object>();
    }

    public AttributeBinder InProperty(string name)
    {
        PropertyName = name;
        return this;
    }

    public AttributeBinder DefineType<T>()
    {
        if (PropertyName == null) throw new InvalidOperationException();

        Type = typeof(T);
        return this;
    }

    public AttributeBinder WithCtorParameters(params object[] values)
    {
        if (PropertyName == null) throw new InvalidOperationException();

        CtorParameters = values;
        return this;
    }

    public AttributeBinder WithPropertyValue(string name, object value)
    {
         if (PropertyName == null) throw new InvalidOperationException();

         PropertyValues.Add(name, value);
         return this;
    }

    internal string PropertyName { get; private set; }

    internal Type Type { get; private set; }

    internal object[] CtorParameters { get; private set; }

    internal IDictionary<string, object> PropertyValues { get; private set; }

    internal AttributeMeta ToAttributeMeta()
    {
        if (PropertyName == null || Type == null) throw new InvalidOperationException();

        return new AttributeMeta(
            PropertyName,
            Type,
            CtorParameters,
            PropertyValues);
    }
}