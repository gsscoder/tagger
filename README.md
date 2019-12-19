# Tagger

.NET library to mock object properties with attributes.

## At a glance

```csharp
var sut = new Mirror(new {
	StringProperty = default(string), IntProperty = default(int), BooleanProperty = default(bool) })
  .Implement<IMyInterface>()
  .AddAttribute<MyAttribute>(
    "StringProperty",
    new AttributeConfiguration()
      .CtorValue("ctor value")
      .Property("AttributeProperty", "property value"));

var instance = sut.Unwrap<IMyInterface>();
```

## Usage

```csharp
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
```
See this [unit test](https://github.com/gsscoder/tagger/blob/master/tests/Tagger.Tests/Unit/MirrorTests.cs) for more examples.