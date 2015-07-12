// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tagger.Tests
{
    static class ReflectionExtensions
    {
        public static IEnumerable<object> AllAttributes(this Type type, string propertyName)
        {
            var prop = type.GetProperties().Single(p => p.Name.Equals(propertyName));
            return prop.GetCustomAttributes(typeof(TestAttribute), true);
        }

        public static TAttr SingleAttribute<TAttr>(this Type type, string propertyName)
            where TAttr : Attribute
        {
            return (TAttr)type.AllAttributes(propertyName).Single();
        }
    }
}
