# <img src="/assets/icon.png" height="30px" alt="CSharpx Logo"> Tagger.Reflect

.NET library to mock object properties with attributes.

## What is for?

I initially designed this library for writing tests for [Command Line Parser Library](https://github.com/commandlineparser/commandline). Although it's main purpose is testing, it can be helpful in the scope of dynamic programming.

## Targets

- .NET Standard 2.0
- .NET Framework 4.5, 4.6.1

## Install via NuGet

```sh
$ dotnet add package Tagger.Reflect --version 1.6.9-beta
```
The latest stable version is [1.0.3](https://github.com/gsscoder/tagger/tree/v1.0.3).

## At a glance

```csharp
interface IMyInterface
{
    string Foo { get; set; }
    int Bar { get; set; }
    bool Baz { get; set; }
}

// define an anonymous template limited to target properties
var mirror = new Mirror(new { Foo = default(string) })
    .Implement<IMyInterface>()
    .Add(x => x.ForProperty("Foo")
               .Define<MyAttribute>()
               .AttributeCtor(new { index = 0 })
               // define an anonymous instance for each attribute
               // property to set
               .AttributeProperty(new { Dir = "/etc/app" })
               .AttributeProperty(new { Config = "file.dat" });

var instance = mirror.Unwrap<IMyInterface>();

// instance layout:
// class anonymous : IMyInterface {
//     ...
//     [MyAttribute(0, Dir = "/etc/app", Config = "file.dat")]
//     public string Foo { get; set; } 
//     ...
// }
```

See this [unit test](https://github.com/gsscoder/tagger/blob/master/tests/Tagger.Reflect.Tests/Unit/MirrorTests.cs) for more examples.

## Latest Changes

- New fluent interface.
- Tests refactoring.
- New tests.

## Icon

[Tag](https://thenounproject.com/search/?q=tagger&i=3051269) icon designed by Eucalyp from [The Noun Project](https://thenounproject.com/).