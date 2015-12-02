using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Symbols
{
    public enum NullabilityPreservationKind
    {
        None = 0,

        ImplicitlyPreserved = 1,

        ExplicitlyPreserved = 2
    }
}
