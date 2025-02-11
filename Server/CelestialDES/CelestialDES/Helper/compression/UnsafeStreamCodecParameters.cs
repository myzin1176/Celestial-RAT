using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialDES.Helper.compression
{
    [Flags]
    public enum UnsafeStreamCodecParameters
    {
        None = 0,
        UpdateImageEveryTwoSeconds = 1 << 1,
        DontDisposeImageCompressor
    }
}
