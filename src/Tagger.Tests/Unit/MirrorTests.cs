// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;
using Tagger.Tests.Fakes;
using Xunit;
using FluentAssertions;

namespace Tagger.Tests.Unit
{
    public class MirrorTests
    {
        public class AddAttributeMethod
        {
            [Theory]
            [InlineData("StringProperty", "value")]
            [InlineData("IntProperty", "another value")]
            [InlineData("BooleanProperty", "another value, more")]
            public void Ctor_value_in_attribute_returns_copy_with_attribute(string propertyName, string ctorValue)
            {
                var expected = new SimpleAttribute(ctorValue);

                var sut = new Mirror(new SimpleType()).AddAttribute<SimpleAttribute>(
                    propertyName,
                    new AttributeConfiguration().CtorValue(ctorValue));

                sut.Object.GetType().SingleAttribute<SimpleAttribute>(propertyName).ShouldBeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("StringProperty", "value", 999)]
            [InlineData("IntProperty", "another value", 9999)]
            [InlineData("BooleanProperty", "another value, more", 99999)]
            public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute(
                string propertyName,
                string ctorValue,
                int memberData)
            {
                var expected = new SimpleAttribute(ctorValue) { IntValue = memberData };

                var sut = new Mirror(new SimpleType()).AddAttribute<SimpleAttribute>(
                    propertyName,
                    new AttributeConfiguration().CtorValue(ctorValue).Property("IntValue", memberData));

                sut.Object.GetType().SingleAttribute<SimpleAttribute>(propertyName).ShouldBeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("StringProperty", "value", 999)]
            [InlineData("IntProperty", "another value", 9999)]
            [InlineData("BooleanProperty", "another value, more", 99999)]
            public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute_with_Anonymous_type(
                string propertyName,
                string ctorValue,
                int memberData)
            {
                var expected = new SimpleAttribute(ctorValue) { IntValue = memberData };
                var anonymous = new { StringProperty = default(string), IntProperty = default(int), BooleanProperty = default(bool) };

                var sut = new Mirror(anonymous).AddAttribute<SimpleAttribute>(
                    propertyName,
                    new AttributeConfiguration().CtorValue(ctorValue).Property("IntValue", memberData));

                sut.Object.GetType().SingleAttribute<SimpleAttribute>(propertyName).ShouldBeEquivalentTo(expected);
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
                var sut = new Mirror(new SimpleType()).Implement<ISimpleInterface>();

                var result = sut.Unwrap<ISimpleInterface>();

                result.StringProperty = stringValue;
                result.IntProperty = intValue;
                result.BooleanProperty = boolValue;

                result.StringProperty.ShouldBeEquivalentTo(stringValue);
                result.IntProperty.ShouldBeEquivalentTo(intValue);
                result.BooleanProperty.ShouldBeEquivalentTo(boolValue);
            }

            [Theory]
            [InlineData("this value", 123456, true)]
            [InlineData("this is another value", -123456, false)]
            public void Can_set_and_get_directly_when_implementing_proper_interface_with_Anonymous_type(string stringValue, int intValue, bool boolValue)
            {
                var anonymous = new { StringProperty = default(string), IntProperty = default(int), BooleanProperty = default(bool) };
                var sut = new Mirror(anonymous).Implement<ISimpleInterface>();

                var result = sut.Unwrap<ISimpleInterface>();

                result.StringProperty = stringValue;
                result.IntProperty = intValue;
                result.BooleanProperty = boolValue;

                result.StringProperty.ShouldBeEquivalentTo(stringValue);
                result.IntProperty.ShouldBeEquivalentTo(intValue);
                result.BooleanProperty.ShouldBeEquivalentTo(boolValue);
            }

            [Theory]
            [InlineData(new[] {1, 2, 3, 4, 5, 6 ,7 ,8, 9})]
            [InlineData(new int[] { })]
            public void Can_set_and_get_sequence_directly_when_implementing_proper_interface(int[] values)
            {
                var sut = new Mirror(new IntSequenceType()).Implement<IWithIntSequence>();

                var result = sut.Unwrap<IWithIntSequence>();

                result.IntSeqProperty = values;

                result.IntSeqProperty.ShouldBeEquivalentTo(values);
            }
        }
    }
}
