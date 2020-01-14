using System;

namespace Tagger.Reflect.Tests.Fakes
{
    public class SimpleAttribute : Attribute
    {
        private readonly string name;

        public SimpleAttribute(string name)
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