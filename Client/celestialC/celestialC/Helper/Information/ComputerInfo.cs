using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC.Helper.Information
{
    internal class ComputerInfo
    {

        public static string username = Environment.UserName;
        public static string culture = System.Globalization.CultureInfo.CurrentCulture.ToString();
        public static string datenow = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");

        public static string ScreenMetrics()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            int width = bounds.Width;
            int height = bounds.Height;
            return width + "x" + height;
        }
        public static string GetLocalIP()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        return ip.ToString();
            }
            catch { }

            return "No network adapters with an IPv4 address in the system!";
        }
        public static string GetPublicIP()
        {
#if DEBUG
            return "DEBUG";
#endif
            try
            {
                string externalip = new WebClient()
                .DownloadString(
                    "http://icanhazip.com")
                .Replace("\n", "");
                return externalip;
            }
            catch { }
            return "Request failed";
        }
        public static string GetRamAmount()
        {
            try
            {
                int RamAmount = 0;
                using (ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_ComputerSystem"))
                {
                    foreach (ManagementObject MO in MOS.Get())
                    {
                        double Bytes = Convert.ToDouble(MO["TotalPhysicalMemory"]);
                        RamAmount = (int)(Bytes / 1048576);
                        break;
                    }
                }
                return RamAmount.ToString() + "MB";
            }
            catch { }
            return "-1";
        }
        public static string RemoveLastChars(string input, int amount = 2)
        {
            if (input.Length > amount)
                input = input.Remove(input.Length - amount);
            return input;
        }

        public static string GetAntivirus()
        {
            try
            {
                string Name = string.Empty;
                bool WinDefend = false;
                string Path = @"\\" + Environment.MachineName + @"\root\SecurityCenter2";
                using (ManagementObjectSearcher MOS =
                    new ManagementObjectSearcher(Path, "SELECT * FROM AntivirusProduct"))
                {
                    foreach (var Instance in MOS.Get())
                    {
                        if (Instance.GetPropertyValue("displayName").ToString() == "Windows Defender")
                            WinDefend = true;
                        if (Instance.GetPropertyValue("displayName").ToString() != "Windows Defender")
                            Name = Instance.GetPropertyValue("displayName").ToString();
                    }

                    if (Name == string.Empty && WinDefend)
                        Name = "Windows Defender";
                    if (Name == "")
                        Name = "N/A";
                    return Name;
                }
            }
            catch
            {
                return "N/A";
            }
        }

        public static string GetName()
        {
            return Environment.MachineName;
        }
        public static string GetPriv()
        {
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            if (isAdmin)
                return "Administrator";
            else
                return "User";
        }

        public static string GetGPU()
        {
            try
            {
                string Name = string.Empty;
                using (ManagementObjectSearcher MOS =
                    new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration"))
                {
                    foreach (ManagementObject MO in MOS.Get()) Name += MO["Description"] + " ;";
                }

                Name = RemoveLastChars(Name);
                return !string.IsNullOrEmpty(Name) ? Name : "N/A";
            }
            catch
            {
                return "N/A";
            }
        }

        public static string GetCountry()
        {
            System.Globalization.CultureInfo c = new System.Globalization.CultureInfo(CultureInfo.CurrentCulture.Name);
            var ri = new RegionInfo(c.Name);
            return ri.TwoLetterISORegionName;
        }

        public static string GetCPU()
        {
            try
            {
                string Name = string.Empty;
                using (ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                {
                    foreach (ManagementObject MO in MOS.Get()) Name += MO["Name"] + "; ";
                }

                Name = RemoveLastChars(Name);
                return !string.IsNullOrEmpty(Name) ? Name : "N/A";
            }
            catch { }

            return "N/A";
        }


        [DllImport("kernel32.dll")]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);


        public static bool Is64BitOperatingSystem()
        {
            // Check if this process is natively an x64 process. If it is, it will only run on x64 environments, thus, the environment must be x64.
            if (IntPtr.Size == 8)
                return true;
            // Check if this process is an x86 process running on an x64 environment.
            IntPtr moduleHandle = GetModuleHandle("kernel32");
            if (moduleHandle != IntPtr.Zero)
            {
                IntPtr processAddress = GetProcAddress(moduleHandle, "IsWow64Process");
                if (processAddress != IntPtr.Zero)
                {
                    bool result;
                    if (IsWow64Process(GetCurrentProcess(), out result) && result)
                        return true;
                }
            }

            // The environment must be an x86 environment.
            return false;
        }

        private static string HKLM_GetString(string key, string value)
        {
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(key);
                return registryKey?.GetValue(value).ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetWindowsVersion()
        {
            string osArchitecture;
            try
            {
                osArchitecture = Is64BitOperatingSystem() ? "64-bit" : "32-bit";
            }
            catch (Exception)
            {
                osArchitecture = "32/64-bit (Undetermined)";
            }

            string productName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string csdVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            string currentBuild = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild");
            if (!string.IsNullOrEmpty(productName))
                return
                    $"{productName}{(!string.IsNullOrEmpty(csdVersion) ? " " + csdVersion : string.Empty)} {osArchitecture} (OS Build {currentBuild})";
            return string.Empty;
        }
    }
}
