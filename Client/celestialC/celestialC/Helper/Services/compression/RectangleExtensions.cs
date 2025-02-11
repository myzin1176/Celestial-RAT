using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace celestialC.Helper.Services.compression
{
    internal static class RectangleExtensions
    {
        public static Int32Rect ToInt32Rect(this Rectangle rectangle)
        {
            return new Int32Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }
}
