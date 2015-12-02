using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis
{
    public interface INonNullableReferenceTypeSymbol: ITypeSymbol
    {
        ITypeSymbol UnderlyingType { get; }
    }
}
