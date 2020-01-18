using System;

sealed class PropertyMeta
{
    public PropertyMeta(string name, Type type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; private set;}

    public Type Type { get; private set; }
}