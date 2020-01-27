using System;

public class FooAttribute : Attribute
{
    private readonly string name;

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