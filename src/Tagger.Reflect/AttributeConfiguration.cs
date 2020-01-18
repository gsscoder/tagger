using System.Collections.Generic;
using System.Linq;

namespace Tagger.Reflect
{
    public class AttributeConfiguration
    {
        private readonly IEnumerable<object> _ctorParameterValues;
        private readonly IDictionary<string, object> _propertyValues;

        public AttributeConfiguration()
            : this(Enumerable.Empty<object>(), new Dictionary<string, object>())
        {
        }

        internal IEnumerable<object> CtorParameterValues { get { return _ctorParameterValues; } }

        internal IDictionary<string, object> PropertyValues { get { return _propertyValues; } }

        private AttributeConfiguration(IEnumerable<object> ctorParameterValues, IDictionary<string, object> propertyValues)
        {
            _ctorParameterValues = ctorParameterValues;
            _propertyValues = propertyValues;
        }

        public AttributeConfiguration CtorValues(params object[] values)
        {
            return new AttributeConfiguration(
                _ctorParameterValues.Concat(values),
                _propertyValues);
        }

        public AttributeConfiguration CtorValue(object value)
        {
            return new AttributeConfiguration(
                _ctorParameterValues.Concat(new[] { value }),
                _propertyValues);
        }

        public AttributeConfiguration Property(string name, object value)
        {
            _propertyValues.Add(name, value);

            return new AttributeConfiguration(
                _ctorParameterValues,
                _propertyValues);
        }
    }
}