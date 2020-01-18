using System;
using Xunit;
using FluentAssertions;

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

            var sut = new Mirror(new Foo()).AddAttribute<FooAttribute>(
                propertyName,
                new AttributeConfiguration().CtorValue(ctorValue));

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

            var sut = new Mirror(new Foo()).AddAttribute<FooAttribute>(
                propertyName,
                new AttributeConfiguration().CtorValue(ctorValue).Property("Value", memberData));

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

            var sut = new Mirror(anonymous).AddAttribute<FooAttribute>(
                propertyName,
                new AttributeConfiguration().CtorValue(ctorValue).Property("Value", memberData));

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
            var sut = new Mirror().AddProperty(propertyName, propertyType);

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
    }
}