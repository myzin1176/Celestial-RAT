using Microsoft.Win32.SafeHandles;
using System;

namespace WinMM
{
    // Token: 0x0200000D RID: 13
    internal sealed class WaveInSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        // Token: 0x060000C2 RID: 194 RVA: 0x00004D00 File Offset: 0x00002F00
        public WaveInSafeHandle() : base(true)
        {
        }

        // Token: 0x060000C3 RID: 195 RVA: 0x00004D0C File Offset: 0x00002F0C
        public WaveInSafeHandle(IntPtr tempHandle) : base(true)
        {
            this.handle = tempHandle;
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x00004D1C File Offset: 0x00002F1C
        protected override bool ReleaseHandle()
        {
            if (!base.IsClosed)
            {
                NativeMethods.MMSYSERROR mmsyserror = NativeMethods.waveInClose(this);
                return mmsyserror == NativeMethods.MMSYSERROR.MMSYSERR_NOERROR;
            }
            return true;
        }
    }
}
