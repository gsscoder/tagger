using System.Collections.Generic;

namespace Tagger.Reflect.Tests.Fakes
{
    public interface IWithIntSequence
    {
        IEnumerable<int> IntSeqProperty { get; set; }
    }
}