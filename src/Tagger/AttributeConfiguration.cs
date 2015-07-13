// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Tagger
{
    public class AttributeConfiguration
    {
        private readonly IEnumerable<object> ctorParameterValues;
        private readonly IDictionary<string, object> propertyValues;

        public AttributeConfiguration()
            : this(Enumerable.Empty<object>(), new Dictionary<string, object>())
        {
        }

        public IEnumerable<object> CtorParameterValues { get { return ctorParameterValues; } }

        public IDictionary<string, object> PropertyValues { get { return propertyValues; } }

        private AttributeConfiguration(IEnumerable<object> ctorParameterValues, IDictionary<string, object> propertyValues)
        {
            this.ctorParameterValues = ctorParameterValues;
            this.propertyValues = propertyValues;
        }

        public AttributeConfiguration CtorValues(params object[] values)
        {
            return new AttributeConfiguration(
                this.ctorParameterValues.Concat(values),
                this.propertyValues);
        }

        public AttributeConfiguration CtorValue(object value)
        {
            return new AttributeConfiguration(
                this.ctorParameterValues.Concat(new[] { value }),
                this.propertyValues);
        }

        public AttributeConfiguration Property(string name, object value)
        {
            this.propertyValues.Add(name, value);

            return new AttributeConfiguration(
                this.ctorParameterValues,
                this.propertyValues);
        }
    }
}
