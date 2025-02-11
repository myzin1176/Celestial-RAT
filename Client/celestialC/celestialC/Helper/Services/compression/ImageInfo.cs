using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    public struct ImageInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public PixelFormat PixelFormat { get; set; }
    }
}
