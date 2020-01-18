using System;
using System.Collections.Generic;
using System.Linq;

static class ReflectionExtensions
{
    public static IEnumerable<object> AllAttributes<TAttr>(this Type type, string propertyName)
        where TAttr : Attribute
    {
        var prop = type.GetProperties().Single(p => p.Name.Equals(propertyName));
        return prop.GetCustomAttributes(typeof(TAttr), true);
    }

    public static TAttr SingleAttribute<TAttr>(this Type type, string propertyName)
        where TAttr : Attribute
    {
        return (TAttr)type.AllAttributes<TAttr>(propertyName).Single();
    }
}