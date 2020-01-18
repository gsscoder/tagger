using System;
using System.Collections.Generic;
using System.Linq;

static class TypeExtensions
{
    public static IEnumerable<object> AllAttributes<T>(this Type type, string propertyName)
        where T : Attribute
    {
        var prop = type.GetProperties().Single(p => p.Name.Equals(propertyName));
        return prop.GetCustomAttributes(typeof(T), true);
    }

    public static T SingleAttribute<T>(this Type type, string propertyName)
        where T : Attribute
    {
        return (T)type.AllAttributes<T>(propertyName).Single();
    }
}