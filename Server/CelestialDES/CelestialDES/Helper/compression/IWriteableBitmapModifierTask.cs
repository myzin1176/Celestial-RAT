using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CelestialDES.Helper.compression
{
    public interface IWriteableBitmapModifierTask
    {
        void PreProcessing(IntPtr backBuffer, int stride, int pixelSize, System.Drawing.Size size, List<Int32Rect> updatedAreas);
        void PostProcessing(IntPtr backBuffer, int stride, int pixelSize, System.Drawing.Size size, List<Int32Rect> updatedAreas);
    }
}
