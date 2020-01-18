using System;
using System.Collections.Generic;

sealed class AttributeMeta
{
    private readonly Type _attributeType;
    private readonly IEnumerable<object> _ctorParameterValues;
    private readonly IDictionary<string, object> _propertyValues;

    public AttributeMeta(Type attributeType, AttributeConfiguration configuration)
    {
        _attributeType = attributeType;
        _ctorParameterValues = configuration.CtorParameterValues;
        _propertyValues = configuration.PropertyValues;
    }

    public Type AttributeType { get { return _attributeType; } }

    public IEnumerable<object> CtorParameterValues { get { return _ctorParameterValues; } }

    public IDictionary<string, object> PropertyValues { get { return _propertyValues; } }
}