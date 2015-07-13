using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tagger.Tests.Fakes
{
    public interface ISimpleInterface
    {
        string StringProperty { get; set; }

        int IntProperty { get; set; }

        bool BooleanProperty { get; set; }
    }
}
