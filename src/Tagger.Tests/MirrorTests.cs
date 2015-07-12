using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tagger.Tests
{
    public class MirrorTests
    {
        public class AddAttributeMethod
        {
            [Theory]
            [InlineData("StringProperty", "value")]
            [InlineData("IntProperty", "another value")]
            [InlineData("LongProperty", "another value, more")]
            public void Ctor_value_in_attribute_returns_copy_with_attribute(string propertyName, string ctorValue)
            {
                var expected = new TestAttribute(ctorValue);

                var sut = new Mirror(new TestType()).AddAttribute<TestAttribute>(
                    propertyName,
                    info => info.CtorParameterValues = new object[] { ctorValue });

                sut.Object.GetType().SingleAttribute<TestAttribute>(propertyName).ShouldBeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("StringProperty", "value", 999)]
            [InlineData("IntProperty", "another value", 9999)]
            [InlineData("LongProperty", "another value, more", 99999)]
            public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute(
                string propertyName,
                string ctorValue,
                int memberData)
            {
                var expected = new TestAttribute(ctorValue) { IntValue = memberData };

                var sut = new Mirror(new TestType()).AddAttribute<TestAttribute>(
                    propertyName,
                    info =>
                    {
                        info.CtorParameterValues = new object[] { ctorValue };
                        info.PropertyValues.Add("IntValue", memberData);
                    });

                sut.Object.GetType().SingleAttribute<TestAttribute>(propertyName).ShouldBeEquivalentTo(expected);
            }

            [Theory]
            [InlineData("StringProperty", "value", 999)]
            [InlineData("IntProperty", "another value", 9999)]
            [InlineData("LongProperty", "another value, more", 99999)]
            public void Ctor_and_property_set_value_in_attribute_returns_copy_with_attribute_with_Anonymous_type(
                string propertyName,
                string ctorValue,
                int memberData)
            {
                var expected = new TestAttribute(ctorValue) { IntValue = memberData };
                var anonymous = new { StringProperty = "", IntProperty = 0, LongProperty = 0L };

                var sut = new Mirror(anonymous).AddAttribute<TestAttribute>(
                    propertyName,
                    info =>
                    {
                        info.CtorParameterValues = new object[] { ctorValue };
                        info.PropertyValues.Add("IntValue", memberData);
                    });

                sut.Object.GetType().SingleAttribute<TestAttribute>(propertyName).ShouldBeEquivalentTo(expected);
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
    }
}
