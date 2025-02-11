using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialDES.Helper.compression
{
    [Flags]
    public enum ImageMetadata
    {
        Frames = 1 << 0,
        FullImage = 1 << 1,
        IncludeCursorImage = 1 << 2,
        CursorPosition = 1 << 3
    }
}
