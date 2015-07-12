using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Tests
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
