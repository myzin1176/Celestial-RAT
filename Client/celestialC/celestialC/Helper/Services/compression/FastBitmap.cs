using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    internal class FastBitmap
    {
        public static int CalcImageOffset(int x, int y, PixelFormat format, int width)
        {
            switch (format)
            {
                case PixelFormat.Format32bppArgb:
                    return y * width * 4 + x * 4;
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                    return y * width * 3 + x * 3;
                case PixelFormat.Format8bppIndexed:
                    return y * width + x;
                case PixelFormat.Format4bppIndexed:
                    return y * (width / 2) + x / 2;
                case PixelFormat.Format1bppIndexed:
                    return y * width * 8 + x * 8;
                default:
                    throw new NotSupportedException(format + " is not supported.");
            }
        }
    }
}
