// Copyright 2015 Giacomo Stelluti Scala. All rights reserved. See doc/License.md in the project root for license information.

using System;

namespace Tagger.Tests.Fakes
{
    public class TestAttribute : Attribute
    {
        private readonly string name;

        public TestAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public int IntValue { get; set; }
    }
}
