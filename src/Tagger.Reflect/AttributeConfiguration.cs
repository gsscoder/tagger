using System.Collections.Generic;
using System.Linq;

public sealed class AttributeConfiguration
{
    private AttributeConfiguration(
        IEnumerable<object> ctorParameterValues,
        IDictionary<string, object> propertyValues)
    {
        CtorParameterValues = ctorParameterValues;
        PropertyValues = propertyValues;
    }

    public AttributeConfiguration()
        : this(Enumerable.Empty<object>(), new Dictionary<string, object>())
    {
    }

    internal IEnumerable<object> CtorParameterValues { get; private set;}

    internal IDictionary<string, object> PropertyValues { get; private set; }

    public AttributeConfiguration CtorValues(params object[] values)
    {
        return new AttributeConfiguration(
            CtorParameterValues.Concat(values),
            PropertyValues);
    }

    public AttributeConfiguration CtorValue(object value)
    {
        return new AttributeConfiguration(
            CtorParameterValues.Concat(new[] { value }),
            PropertyValues);
    }

    public AttributeConfiguration Property(string name, object value)
    {
        PropertyValues.Add(name, value);

        return new AttributeConfiguration(
            CtorParameterValues,
            PropertyValues);
    }
}