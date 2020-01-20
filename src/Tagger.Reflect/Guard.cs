using System;
using System.Linq;

static class Guard
{
    public static void AgainstNull(string argumentName, object value)
    {
        if (value == null) throw new ArgumentNullException(argumentName,
            $"{argumentName} cannot be null");
    }

    public static void AgainstEmptyWhiteSpace(string argumentName, string value)
    {
        if (value.Trim() == string.Empty) throw new ArgumentException(
            $"{argumentName} cannot be empty or contains only white spaces", argumentName);
    }

    public static void AgainstExceptInterface<T>()
    {
        if (!typeof(T).IsInterface) throw new ArgumentException(
            "T must be an interface type");
    }

    public static void AgainstExceptAnonymous(string argumentName, object value)
    {
        if (!value.GetType().IsAnonymous()) throw new ArgumentException(
            $"{nameof(argumentName)} must be an anonymous type instance");
    }

    public static void AgainstExceptSingleProperty(string argumentName, object value)
    {
        if (value.GetType().GetProperties().Count() != 1) throw new ArgumentException(
            $"{nameof(argumentName)} must have exactly a single property");
    }
}