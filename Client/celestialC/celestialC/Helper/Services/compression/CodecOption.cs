using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    [Flags]
    public enum CodecOption
    {
        /// <summary>
        ///     The Previous and next image size must be equal
        /// </summary>
        RequireSameSize = 1 << 0,

        /// <summary>
        ///     If the codec is having a stream buffer
        /// </summary>
        HasBuffers = 1 << 1,

        /// <summary>
        ///     The image will be disposed by the codec and shall not be disposed by the user
        /// </summary>
        AutoDispose = 1 << 2,

        /// <summary> No codec options were used </summary>
        None = 1 << 3
    }
}
