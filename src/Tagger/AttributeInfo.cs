using System;
using System.Collections.Generic;

namespace Tagger
{
    sealed class AttributeInfo
    {
        private readonly Type attributeType;
        private readonly IEnumerable<object> ctorParameterValues;
        private readonly IDictionary<string, object> propertyValues;

        public AttributeInfo(Type attributeType, AttributeConfiguration configuration)
        {
            this.attributeType = attributeType;
            this.ctorParameterValues = configuration.CtorParameterValues;
            this.propertyValues = configuration.PropertyValues;
        }

        public Type AttributeType { get { return attributeType; } }

        public IEnumerable<object> CtorParameterValues { get { return ctorParameterValues; } }

        public IDictionary<string, object> PropertyValues { get { return propertyValues; } }
    }
}