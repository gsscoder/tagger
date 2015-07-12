# Tagger
C# library to mock object properties with attributes.

# Work in progress
```csharp
var sut = new Mirror(new { StringProperty = "", IntProperty = 0, LongProperty = 0L })
  .AddAttribute<TestAttribute>(
    "StringProperty",
    info => {
      info.CtorParameterValues = new object[] { ctorValue };
      info.PropertyValues.Add("IntValue", 999);
    });

var instance = sut.Object;
```
See this [unit test](https://github.com/gsscoder/tagger/blob/master/src/Tagger.Tests/MirrorTests.cs) for usage info.
