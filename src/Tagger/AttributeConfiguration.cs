// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System.Collections.Generic;

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
