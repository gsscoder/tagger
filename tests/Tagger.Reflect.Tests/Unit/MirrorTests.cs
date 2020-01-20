using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Tagger;

public class MirrorTests
{
    [Theory]
    [InlineData("foo bar baz")]
    [InlineData("bar foo baz")]
    [InlineData("baz baz foo")]
    public void Ctor_value_in_attribute_returns_copy_with_attribute(string value)
    {
        var expected = new FooAttribute(value);

        var sut = new Mirror(
            new Foo()).Add(x => x.ForProperty("Baz")
                                    .Define<FooAttribute>()
                                    .AttributeCtor(new { name = value }));

        sut.Object.GetType().SingleAttribute<FooAttribute>("Baz")
            .Should().Be(expected);
    }

    [Theory]
    [InlineData("foo bar baz", 99321)]
    [InlineData("bar foo baz", 66123)]
    [InlineData("baz baz foo", 66123)]
    public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute(
        string ctorValue,
        int propValue)
    {
        var expected = new FooAttribute(ctorValue) { Value = propValue };

        var sut = new Mirror(
            new Foo())
                .Add(x => x.ForProperty("Baz")
                           .Define<FooAttribute>()
                           .AttributeCtor(new { name = ctorValue })
                           .AttributeProperty(new { Value = propValue }));
                                    
        sut.Object.GetType().SingleAttribute<FooAttribute>("Baz")
            .Should().Be(expected);
    }

    [Theory]
    [InlineData("DynamicString", typeof(string))]
    [InlineData("DynamicBoolean", typeof(bool))]
    [InlineData("DynamicLong", typeof(long))]
    public void Add_property_to_new_object_returns_object_with_new_property(string propertyName, Type propertyType)
    {
        var sut = new Mirror().Add(x => x.Property(propertyName)
                                         .OfType(propertyType));

        sut.Object.GetType().GetProperties().Should().Contain(
            p => p.Name.Equals(propertyName) && p.PropertyType == propertyType);
    }

    [Fact]
    public void Should_not_implement_a_non_interface_type()
    {
        var sut = new Mirror();

        Action action = () => sut.Implement<Baz>();

        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("T must be an interface type");
    }

    [Theory]
    [InlineData(0.99321)]
    [InlineData(9.66123)]
    [InlineData(1.23456)]
    public void Should_add_attribute_using_only_properties(double value)
    {
        var expected = new BarAttribute() { Value = value };

        var sut = new Mirror(
                    new { Baz = default(string) })
                    .Implement<IBar>()
                    .Add(x => x.ForProperty("Baz")
                               .Define<BarAttribute>()
                               .AttributeProperty(new { Value = value }));
        
        sut.Object.GetType().SingleAttribute<BarAttribute>("Baz")
            .Should().Be(expected);
    }

    [Theory]
    [InlineData("foo bar baz")]
    [InlineData("bar foo baz")]
    [InlineData("baz baz foo")]
    public void Should_add_attribute_using_only_constructor(string value)
    {
        var expected = new FooAttribute(value);

        var sut = new Mirror(new Foo())
                        .Implement<IFoo>()
                        .Add(x => x.ForProperty("Baz")
                                   .Define<FooAttribute>()
                                   .AttributeCtor(new { name = value }));
        
        sut.Object.GetType().SingleAttribute<FooAttribute>("Baz")
            .Should().Be(expected);
    }

    [Fact]
    public void Should_automatically_implement_all_interface_props_with_partial_anon_type()
    {
        var sut = new Mirror(
                        new { Foo = default(string), Bar = default(int) })
                        .Implement<IFoo>();

        sut.Object.Should().NotBeNull()
            .And.BeAssignableTo<IFoo>();
    }

    [Fact]
    public void Should_automatically_implement_all_interface_props_with_matching_type()
    {
        var sut = new Mirror(
                        new Foo())
                            .Implement<IFoo>();

        sut.Object.Should().NotBeNull()
            .And.BeAssignableTo<IFoo>();
    }

    [Theory]
    [InlineData(new [] { 0.99321, 9.66123, 1.23456 })]
    [InlineData(new [] { 9.66123, 0.99321, 1.23456 })]
    [InlineData(new [] { 1.23456, 0.99321, 9.66123 })]
    public void Should_handle_attribute_with_array_in_property(double[] value)
    {
        var expected = new BazAttribute() { Value = value };

        var sut = new Mirror(
            new { Foo = default(object) })
                .Add(x => x.ForProperty("Foo")
                           .Define<BazAttribute>()
                           .AttributeProperty(new { Value = value }));

        sut.Object.GetType().SingleAttribute<BazAttribute>("Foo")
            .Should().Be(expected);
    }

    [Theory]
    [InlineData(new [] { 0.99321, 9.66123, 1.23456 })]
    [InlineData(new [] { 9.66123, 0.99321, 1.23456 })]
    [InlineData(new [] { 1.23456, 0.99321, 9.66123 })]
    public void Should_handle_attribute_with_array_in_ctor(double[] value)
    {
        var expected = new BazAttribute(value);

        var sut = new Mirror(
            new { Bar = default(object) })
                .Add(x => x.ForProperty("Bar")
                           .Define<BazAttribute>()
                           .AttributeCtor(new { value = value }));

        sut.Object.GetType().SingleAttribute<BazAttribute>("Bar")
            .Should().Be(expected);
    }
}