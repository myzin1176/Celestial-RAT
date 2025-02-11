using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CelestialDES.Helper.compression
{
    internal struct HeaderInfo
    {
        public HeaderInfo(ImageMetadata imageMetadata, System.Drawing.Imaging.PixelFormat format, int width, int height)
        {
            ImageMetadata = imageMetadata;
            Format = format;
            Width = width;
            Height = height;
        }

        public ImageMetadata ImageMetadata { get; }
        public System.Drawing.Imaging.PixelFormat Format { get; }
        public int Width { get; }
        public int Height { get; }
    }
}
