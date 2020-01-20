using System;
using System.Linq;

static class ObjectExtensions
{
    public static T InspectAttribute<T>(this object obj, string propertyName)
        where T : Attribute
    {
        return (T)obj.GetType().GetProperties().Single(p => p.Name.Equals(propertyName))
                     .GetCustomAttributes(typeof(T), true).Single();
    }
}