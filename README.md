# Tagger.Reflect

.NET library to mock object properties with attributes.

## What is for?

I initially designed this library for writing tests for [Command Line Parser Library](https://github.com/commandlineparser/commandline). Although it's main purpose is testing, it can be helpful in the scope of dynamic programming.

## Targets

- .NET Standard 2.0
- .NET Framework 4.5, 4.6.1

## Install via NuGet

```sh
$ dotnet add package Tagger.Reflect --version 1.5.0-alpha
```

## At a glance

```csharp
var mirror = new Mirror(new {
	Foo = default(string), Bar = default(int), Baz = default(bool) })
  .Implement<IMyInterface>()
  .Add(x => x.InProperty(propertyName)
                .DefineType<MyAttribute>()
                .WithCtorParameters("ctor")
                .WithPropertyValue("value", memberData));

var instance = mirror.Unwrap<IMyInterface>();

// instance layout:
// anonymous : IMyInterface {
// ...
//     [MyAttribute("ctor", AttrProp="value")]
//     public string Foo { get; set; } 
// ...
// }
```

See this [unit test](https://github.com/gsscoder/tagger/blob/master/tests/Tagger.Reflect.Tests/Unit/MirrorTests.cs) for more examples.

## Latest Changes

- New fluent interface.