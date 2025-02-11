using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WinMM
{
    // Token: 0x0200000E RID: 14
    public sealed class PlaySound
    {
        // Token: 0x060000C5 RID: 197 RVA: 0x00004D48 File Offset: 0x00002F48
        private PlaySound()
        {
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x00004D50 File Offset: 0x00002F50
        public static void PlaySystemSound(string systemSoundName)
        {
            if (NativeMethods.PlaySound(systemSoundName, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_ASYNC | NativeMethods.PLAYSOUNDFLAGS.SND_NODEFAULT | NativeMethods.PLAYSOUNDFLAGS.SND_PURGE | NativeMethods.PLAYSOUNDFLAGS.SND_ALIAS) == 0U)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x00004D84 File Offset: 0x00002F84
        public static void StopAllSounds()
        {
            if (NativeMethods.PlaySound(null, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_PURGE) == 0U)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x00004DB8 File Offset: 0x00002FB8
        public static void PlaySoundFile(string soundFileName)
        {
            if (NativeMethods.PlaySound(soundFileName, (IntPtr)0, NativeMethods.PLAYSOUNDFLAGS.SND_ASYNC | NativeMethods.PLAYSOUNDFLAGS.SND_NODEFAULT | NativeMethods.PLAYSOUNDFLAGS.SND_PURGE | NativeMethods.PLAYSOUNDFLAGS.SND_FILENAME) == 0U)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
