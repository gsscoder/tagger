using System;
using Xunit;
using FluentAssertions;
using Tagger;

public class MirrorTests
{
    public class AddAttributeMethod
    {
        [Theory]
        [InlineData("FooString", "value")]
        [InlineData("BarInt32", "another value")]
        [InlineData("BazBoolean", "another value, more")]
        public void Ctor_value_in_attribute_returns_copy_with_attribute(string propertyName, string ctorValue)
        {
            var expected = new FooAttribute(ctorValue);

            var sut = new Mirror(
                new Foo()).Add(x => x.ForProperty(propertyName)
                                     .Define<FooAttribute>()
                                     .CtorValues(ctorValue));

            sut.Object.GetType().SingleAttribute<FooAttribute>(propertyName).Should().Be(expected);
        }

        [Theory]
        [InlineData("FooString", "value", 999)]
        [InlineData("BarInt32", "another value", 9999)]
        [InlineData("BazBoolean", "another value, more", 99999)]
        public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute(
            string propertyName,
            string ctorValue,
            int memberData)
        {
            var expected = new FooAttribute(ctorValue) { Value = memberData };

            var sut = new Mirror(
                new Foo()).Add(x => x.ForProperty(propertyName)
                                     .Define<FooAttribute>()
                                     .CtorValues(ctorValue)
                                     .PropertyValue("Value", memberData));

            sut.Object.GetType().SingleAttribute<FooAttribute>(propertyName).Should().Be(expected);
        }

        [Theory]
        [InlineData("FooString", "value", 999)]
        [InlineData("BarInt32", "another value", 9999)]
        [InlineData("BazBoolean", "another value, more", 99999)]
        public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute_with_Anonymous_type(
            string propertyName,
            string ctorValue,
            int memberData)
        {
            var expected = new FooAttribute(ctorValue) { Value = memberData };
            var anonymous = new { FooString = default(string), BarInt32 = default(int), BazBoolean = default(bool) };

            var sut = new Mirror(anonymous).Add(x => x.ForProperty(propertyName)
                                                      .Define<FooAttribute>()
                                                      .CtorValues(ctorValue)
                                                      .PropertyValue("Value", memberData));

            sut.Object.GetType().SingleAttribute<FooAttribute>(propertyName).Should().Be(expected);
        }
    }

    public class AddPropertyMethod
    {
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
    }

    public class ImplementInterfaceMethod
    {
        [Theory]
        [InlineData("this value", 123456, true)]
        [InlineData("this is another value", -123456, false)]
        public void Can_set_and_get_directly_when_implementing_proper_interface(string stringValue, int intValue, bool boolValue)
        {
            var sut = new Mirror(new Foo()).Implement<IFoo>();

            var result = sut.Unwrap<IFoo>();

            result.FooString = stringValue;
            result.BarInt32 = intValue;
            result.BazBoolean = boolValue;

            result.FooString.Should().Be(stringValue);
            result.BarInt32.Should().Be(intValue);
            result.BazBoolean.Should().Be(boolValue);
        }

        [Theory]
        [InlineData("this value", 123456, true)]
        [InlineData("this is another value", -123456, false)]
        public void Can_set_and_get_directly_when_implementing_proper_interface_with_Anonymous_type(string stringValue, int intValue, bool boolValue)
        {
            var anonymous = new { FooString = default(string), BarInt32 = default(int), BazBoolean = default(bool) };
            var sut = new Mirror(anonymous).Implement<IFoo>();

            var result = sut.Unwrap<IFoo>();

            result.FooString = stringValue;
            result.BarInt32 = intValue;
            result.BazBoolean = boolValue;

            result.FooString.Should().Be(stringValue);
            result.BarInt32.Should().Be(intValue);
            result.BazBoolean.Should().Be(boolValue);
        }

        [Theory]
        [InlineData(new[] {1, 2, 3, 4, 5, 6 ,7 ,8, 9})]
        [InlineData(new int[] { })]
        public void Can_set_and_get_sequence_directly_when_implementing_proper_interface(int[] values)
        {
            var sut = new Mirror(new FooSequence()).Implement<IFooSequence>();

            var result = sut.Unwrap<IFooSequence>();

            result.BarInt32Sequence = values;

            result.BarInt32Sequence.Should().BeEquivalentTo(values);
        }

        [Fact]
        public void Should_not_implement_a_non_interface_type()
        {
            var sut = new Mirror();

            Action action = () => sut.Implement<FooAbstract>();

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
                        new { Foo = default(string) })
                        .Implement<IBar>()
                        .Add(x => x.ForProperty("Foo")
                                   .Define<BarAttribute>()
                                   .PropertyValue("Value", value));
            
            sut.Object.GetType().SingleAttribute<BarAttribute>("Foo").Should().Be(expected);
        }

        [Theory]
        [InlineData("foo bar baz")]
        [InlineData("bar foo baz")]
        [InlineData("baz baz foo")]
        public void Should_add_attribute_using_only_constructor(string value)
        {
            var expected = new FooAttribute(value);
            var anonymous = new { FooString = default(string), BarInt32 = default(int), BazBoolean = default(bool) };

            var sut = new Mirror(anonymous)
                            .Implement<IFoo>()
                            .Add(x => x.ForProperty("FooString")
                                       .Define<FooAttribute>()
                                       .CtorValues(value));
            
            sut.Object.GetType().SingleAttribute<FooAttribute>("FooString").Should().Be(expected);
        }

        [Fact]
        public void Should_automatically_implement_all_interface_props_with_partial_anon_type()
        {
            var sut = new Mirror(
                            new { FooString = default(string) })
                            .Implement<IFoo>();

            sut.Object.Should().NotBeNull()
                .And.BeAssignableTo<IFoo>();
        }
    }
}