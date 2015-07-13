# Tagger
C# library to mock object properties with attributes.

# Work in progress
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
See this [unit test](https://github.com/gsscoder/tagger/blob/master/src/Tagger.Tests/MirrorTests.cs) for usage info.
