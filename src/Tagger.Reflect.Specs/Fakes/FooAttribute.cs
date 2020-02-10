using System;

public class FooAttribute : Attribute
{
    readonly string name;

    public FooAttribute(string name)
    {
        this.name = name;
    }

    public string Name
    {
        get { return name; }
    }

    public int Value { get; set; }
}