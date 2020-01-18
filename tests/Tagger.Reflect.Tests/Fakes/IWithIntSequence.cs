using System.Collections.Generic;

public interface IWithIntSequence
{
    IEnumerable<int> IntSeqProperty { get; set; }
}