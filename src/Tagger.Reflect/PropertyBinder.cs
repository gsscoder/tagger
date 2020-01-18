using System;

public sealed class PropertyBinder
{
    public PropertyBinder Property(string name)
    {
        Guard.AgainstNull(nameof(name), name);
        Guard.AgainstEmptyWhiteSpace(nameof(name), name);

        Name = name;
        return this;
    }

#if DEBUG
    public PropertyBinder OfType(Type type)
    {
        if (Name == null) throw new InvalidOperationException();

        Type = type;
        return this;
    }
#endif

    public PropertyBinder OfType<T>()
    {
        if (Name == null) throw new InvalidOperationException();

        Type = typeof(T);
        return this;
    }

    internal string Name { get; private set; }

    internal Type Type { get; private set; }

    internal PropertyMeta ToPropertyMeta()
    {
        if (Name == null || Type == null) throw new InvalidOperationException();

        return new PropertyMeta(Name, Type);
    }
}