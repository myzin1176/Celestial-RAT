using Microsoft.Win32.SafeHandles;
using System;

namespace WinMM
{
    // Token: 0x0200000C RID: 12
    internal sealed class WaveOutSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        // Token: 0x060000BF RID: 191 RVA: 0x00004CB8 File Offset: 0x00002EB8
        public WaveOutSafeHandle() : base(true)
        {
        }

        // Token: 0x060000C0 RID: 192 RVA: 0x00004CC4 File Offset: 0x00002EC4
        public WaveOutSafeHandle(IntPtr tempHandle) : base(true)
        {
            this.handle = tempHandle;
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x00004CD4 File Offset: 0x00002ED4
        protected override bool ReleaseHandle()
        {
            if (!base.IsClosed)
            {
                NativeMethods.MMSYSERROR mmsyserror = NativeMethods.waveOutClose(this);
                return mmsyserror == NativeMethods.MMSYSERROR.MMSYSERR_NOERROR;
            }
            return true;
        }
    }
}
