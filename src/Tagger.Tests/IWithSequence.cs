using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Tests
{
    public interface IWithSequence
    {
        IEnumerable<int> IntSeqProperty { get; set; }
    }
}
