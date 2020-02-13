using System;

public class FooAttribute : Attribute
{
    readonly string _name;

    public FooAttribute(string name)
    {
        _name = name;
    }

    public string Name => _name;

    public int Value { get; set; }
}