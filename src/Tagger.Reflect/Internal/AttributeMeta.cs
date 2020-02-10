using System;
using System.Collections.Generic;

struct AttributeMeta
{
    public AttributeMeta(
        string propertyName,
        Type attributeType,
        object ctorLayout,
        IDictionary<string, object> propertyValues)
    {
        PropertyName = propertyName;
        AttributeType = attributeType;
        CtorLayout = ctorLayout;
        PropertyValues = propertyValues;
    }

    public string PropertyName { get; private set; }

    public Type AttributeType { get; private set; }

    public object CtorLayout { get; private set; }

    public IDictionary<string, object> PropertyValues { get; private set; }
}