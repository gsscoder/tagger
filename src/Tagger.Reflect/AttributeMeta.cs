using System;
using System.Collections.Generic;

sealed class AttributeMeta
{
    public AttributeMeta(Type attributeType, AttributeConfiguration configuration)
    {
        AttributeType = attributeType;
        CtorParameterValues = configuration.CtorParameterValues;
        PropertyValues = configuration.PropertyValues;
    }

    public Type AttributeType { get; private set; }

    public IEnumerable<object> CtorParameterValues { get; private set; }

    public IDictionary<string, object> PropertyValues { get; private set; }
}