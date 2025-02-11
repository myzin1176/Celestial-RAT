using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.Services.compression
{
    public interface IModifiedDecoder : IImageDecoder
    {
        IModifiedDecoder AppendModifier<T>(T writeableBitmapModifierTask) where T : IWriteableBitmapModifierTask;
    }
}
