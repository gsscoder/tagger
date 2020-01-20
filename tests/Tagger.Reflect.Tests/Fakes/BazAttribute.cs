using System;

public class BazAttribute : Attribute
{
    public BazAttribute()
    {
    }

    public BazAttribute(double[] value)
    {
        Value = value;
    }

    public double[] Value { get; set; }
}