# <img src="/assets/icon.png" height="30px" alt="CSharpx Logo"> Tagger.Reflect

.NET library to mock object properties with attributes.

## What is for?

I initially designed this library for writing tests for [Command Line Parser Library](https://github.com/commandlineparser/commandline). Although it's main purpose is testing, it can be helpful in the scope of dynamic programming.

## Targets

- .NET Standard 2.0
- .NET Framework 4.5, 4.6.1

## Install via NuGet

```sh
$ dotnet add package Tagger.Reflect --version 1.5.5-alpha
```
The latest stable version is [1.0.3](https://github.com/gsscoder/tagger/tree/v1.0.3).

## At a glance

```csharp
var mirror = new Mirror(new {
	Foo = default(string), Bar = default(int), Baz = default(bool) })
  .Implement<IMyInterface>()
  .Add(x => x.ForProperty(propertyName)
                .Define<MyAttribute>()
                .WithCtorParameters("ctor")
                .WithPropertyValue("AttrProp", "value"));

var instance = mirror.Unwrap<IMyInterface>();

// instance layout:
// class anonymous : IMyInterface {
// ...
//     [MyAttribute("ctor", AttrProp = "value")]
//     public string Foo { get; set; } 
// ...
// }
```

See this [unit test](https://github.com/gsscoder/tagger/blob/master/tests/Tagger.Reflect.Tests/Unit/MirrorTests.cs) for more examples.

## Latest Changes

- New fluent interface.

## Icon

[Tag](https://thenounproject.com/search/?q=tagger&i=3051269) icon designed by Eucalyp from [The Noun Project](https://thenounproject.com/).