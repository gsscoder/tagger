// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Tagger.dll")]
[assembly: AssemblyDescription("C# library to mock object properties with attributes.")]
[assembly: AssemblyCulture("")]
[assembly: InternalsVisibleTo("Tagger.Tests")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]