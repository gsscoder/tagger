using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

static class TypeExtensions
{
    public static bool ContainsProperty(this IEnumerable<Type> types, string propertyName)
    {
        var allNames = (from p in types.SelectMany(t => t.GetProperties())
                        select p.Name).Distinct();
        return allNames.Any(n => n.Equals(propertyName));
    }

    public static MethodInfo FindGetter(this IEnumerable<Type> types, string propertyName)
    {
        var allMethods =
            from m in types.SelectMany(t => t.GetMethods())
            select m;
        return allMethods.Single(m => m.Name.Equals(string.Concat("get_", propertyName)));
    }

    public static MethodInfo FindSetter(this IEnumerable<Type> types, string propertyName)
    {
        var allMethods =
            from m in types.SelectMany(t => t.GetMethods())
            select m;
        return allMethods.Single(m => m.Name.Equals(string.Concat("set_", propertyName)));
    }

    public static IEnumerable<PropertyMeta> GetProperties(this Type type, IEnumerable<string> excluded)
    {
        return (from p in type.GetProperties()
                where p.CanRead &&
                      p.CanWrite &&
                      !excluded.Contains(p.Name)
                select new PropertyMeta(p.Name, p.PropertyType));
    }
}