using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    public class AttributeInfo
    {
        private readonly Type attributeType;
        private readonly object[] ctorParameterValues;
        private readonly IDictionary<string, object> propertyValues;

        public AttributeInfo(Type attributeType, AttributeConfiguration configuration)
        {
            this.attributeType = attributeType;
            this.ctorParameterValues = configuration.CtorParameterValues;
            this.propertyValues = configuration.PropertyValues;
        }

        public Type AttributeType { get { return attributeType; } }

        public object[] CtorParameterValues { get { return ctorParameterValues; } }

        public IDictionary<string, object> PropertyValues { get { return propertyValues; } }
    }
}
