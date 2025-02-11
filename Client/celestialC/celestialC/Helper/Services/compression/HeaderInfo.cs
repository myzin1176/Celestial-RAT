using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    internal struct HeaderInfo
    {
        public HeaderInfo(ImageMetadata imageMetadata, PixelFormat format, int width, int height)
        {
            ImageMetadata = imageMetadata;
            Format = format;
            Width = width;
            Height = height;
        }

        public ImageMetadata ImageMetadata { get; }
        public PixelFormat Format { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
