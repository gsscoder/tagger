using System;
using System.Collections.Generic;

sealed class AttributeMeta
{
    public AttributeMeta(
        string propertyName, Type attributeType, AttributeConfiguration configuration)
    {
        PropertyName = propertyName;
        AttributeType = attributeType;
        CtorParameterValues = configuration.CtorParameterValues;
        PropertyValues = configuration.PropertyValues;
    }

    public string PropertyName { get; private set; }

    public Type AttributeType { get; private set; }

    public IEnumerable<object> CtorParameterValues { get; private set; }

    public IDictionary<string, object> PropertyValues { get; private set; }
}