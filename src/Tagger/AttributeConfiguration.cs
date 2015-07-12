using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger
{
    public class AttributeConfiguration
    {
        public AttributeConfiguration()
        {
            CtorParameterValues = new object[] {};
            PropertyValues = new Dictionary<string, object>();
        }

        public object[] CtorParameterValues { get; set; }

        public IDictionary<string, object> PropertyValues { get; private set; }
    }
}
