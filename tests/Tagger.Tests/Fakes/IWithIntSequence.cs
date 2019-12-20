using System.Collections.Generic;

namespace Tagger.Tests.Fakes
{
    public interface IWithIntSequence
    {
        IEnumerable<int> IntSeqProperty { get; set; }
    }
}