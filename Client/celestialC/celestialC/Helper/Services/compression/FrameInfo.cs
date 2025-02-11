using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    public struct FrameInfo
    {
        public FrameInfo(Rectangle updatedArea, FrameFlags frameFlags)
        {
            UpdatedArea = updatedArea;
            FrameFlags = frameFlags;
        }

        public Rectangle UpdatedArea { get; set; }
        public FrameFlags FrameFlags { get; set; }
    }
}
