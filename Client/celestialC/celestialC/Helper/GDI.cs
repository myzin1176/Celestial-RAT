using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Security.AccessControl;

namespace celestialC.Helper
{
    public static class GDI
    {
        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll")]
        static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, TernaryRasterOperations dwRop);
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(uint crColor);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }
        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
        int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        BLENDFUNCTION blendFunction);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }

            public override string ToString()
            {
                return $"X: {X}, Y: {Y}";
            }
        }
        [DllImport("gdi32.dll")]
        static extern bool PlgBlt(IntPtr hdcDest, POINT[] lpPoint, IntPtr hdcSrc,
   int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask,
   int yMask);

        private static bool NegativeEnabled = false;
        private static bool BlurEnabled = false;
        private static bool RadialEnabled = false;
        private static bool RoundedTunnelEnabled = false;
        private static bool MelterEnabled = false;
        private static bool bloodyEnabled = false;
        public static void InvertedColor()
        {
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            NegativeEnabled = !NegativeEnabled;
            if (NegativeEnabled)
                Task.Factory.StartNew(() =>
                {
                    while (NegativeEnabled)
                    {
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        IntPtr brush = CreateSolidBrush(0xF0FFFF);
                        SelectObject(hdc, brush);
                        PatBlt(hdc, 0, 0, x, y, TernaryRasterOperations.PATINVERT);
                        DeleteObject(brush);
                        DeleteDC(hdc);
                        Thread.Sleep(1000);
                    }
                });
        }

        public static void BlurGDI()
        {
            Random r;
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            BlurEnabled = !BlurEnabled;
            if (BlurEnabled)
                Task.Factory.StartNew(() =>
                {
                    while (BlurEnabled)
                    {
                        r = new Random();
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        IntPtr mhdc = CreateCompatibleDC(hdc);
                        IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                        IntPtr holdbit = SelectObject(mhdc, hbit);
                        BitBlt(mhdc, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                        AlphaBlend(hdc, r.Next(-4, 4), r.Next(-4, 4), x, y, mhdc, 0, 0, x, y, new BLENDFUNCTION(0, 0, 70, 0));
                        SelectObject(mhdc, holdbit);
                        DeleteObject(holdbit);
                        DeleteObject(hbit);
                        DeleteDC(mhdc);
                        DeleteDC(hdc);
                        Thread.Sleep(100);
                    }
                });
        }

        public static void RoundedTunnel()
        {
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            int left = Screen.PrimaryScreen.Bounds.Left, right = Screen.PrimaryScreen.Bounds.Right, top = Screen.PrimaryScreen.Bounds.Top, bottom = Screen.PrimaryScreen.Bounds.Bottom;
            RoundedTunnelEnabled = !RoundedTunnelEnabled;
            POINT[] lppoint = new POINT[3];
            if (RoundedTunnelEnabled)
                Task.Factory.StartNew(() =>
                {
                    while (RoundedTunnelEnabled)
                    {
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        IntPtr mhdc = CreateCompatibleDC(hdc);
                        IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                        IntPtr holdbit = SelectObject(mhdc, hbit);
                        lppoint[0].X = (left + 50);
                        lppoint[0].Y = (top - 50);
                        lppoint[1].X = (right + 50);
                        lppoint[1].Y = (top + 50);
                        lppoint[2].X = (left - 50);
                        lppoint[2].Y = (bottom - 50);
                        PlgBlt(hdc, lppoint, hdc, left - 20, top - 20, (right - left) + 40, (bottom - top) + 40, IntPtr.Zero, 0, 0);
                        DeleteDC(hdc);
                        Thread.Sleep(250);
                    }
                });
        }

        public static void RadialBlur()
        {
            Random r;
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            int left = Screen.PrimaryScreen.Bounds.Left, right = Screen.PrimaryScreen.Bounds.Right, top = Screen.PrimaryScreen.Bounds.Top, bottom = Screen.PrimaryScreen.Bounds.Bottom;
            RadialEnabled = !RadialEnabled;
            POINT[] lppoint = new POINT[3];
            if (RadialEnabled)
                Task.Factory.StartNew(() =>
                {
                    while (RadialEnabled)
                    {
                        r = new Random();
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        IntPtr mhdc = CreateCompatibleDC(hdc);
                        IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);
                        IntPtr holdbit = SelectObject(mhdc, hbit);
                        if(r.Next(2) == 1)
                        {
                            lppoint[0].X = (left + 40);
                            lppoint[0].Y = (top - 40);
                            lppoint[1].X = (right + 40);
                            lppoint[1].Y = (top + 40);
                            lppoint[2].X = (left - 40);
                            lppoint[2].Y = (bottom - 40);
                        }
                        else
                        {
                            lppoint[0].X = (left - 40);
                            lppoint[0].Y = (top + 40);
                            lppoint[1].X = (right - 40);
                            lppoint[1].Y = (top - 40);
                            lppoint[2].X = (left + 40);
                            lppoint[2].Y = (bottom + 40);
                        }
                        PlgBlt(mhdc, lppoint, hdc, left, top, right - left, bottom - top, IntPtr.Zero, 0, 0);
                        AlphaBlend(hdc, 0, 0, x, y, mhdc, 0, 0, x, y, new BLENDFUNCTION(0,0,100,0));
                        SelectObject(mhdc, holdbit);
                        DeleteObject(holdbit);
                        DeleteObject(hbit);
                        DeleteDC(mhdc);
                        DeleteDC(hdc);
                        Thread.Sleep(200);
                    }
                });
        }

        public static void Melter()
        {
            Random r;
            int x = Screen.PrimaryScreen.Bounds.Width, y = Screen.PrimaryScreen.Bounds.Height;
            MelterEnabled = !MelterEnabled;
            if (MelterEnabled)
                Task.Factory.StartNew(() =>
                {
                    while (MelterEnabled)
                    {
                        r = new Random();
                        IntPtr hdc = GetDC(IntPtr.Zero);
                        IntPtr mhdc = CreateCompatibleDC(hdc);
                        IntPtr hbit = CreateCompatibleBitmap(hdc, x, y);

                        int xm = (r.Next() % x) - (150 / 2);
                        int ym = (r.Next() % 15);
                        int width = (r.Next() % 150);

                        BitBlt(hdc, xm, ym, width, y, hdc, xm, 0, TernaryRasterOperations.SRCCOPY);
                        DeleteDC(hdc);
                        Thread.Sleep(25);
                    }
                });
        }
        static bloodyscreen bs1;
        static bloodyscreen bs2;
        static bloodyscreen bs3;
        static bloodyscreen bs4;
        static pinksavage ps;
        public static void BloodyScreen()
        {
            bloodyEnabled = !bloodyEnabled;
            if (bloodyEnabled)
            {
                bs1 = new bloodyscreen();
                bs2 = new bloodyscreen();
                bs3 = new bloodyscreen();
                bs4 = new bloodyscreen();
                ps = new pinksavage();
                bs1.Opacity = .99;
                bs2.Opacity = .80;
                bs3.Opacity = .75;
                bs4.Opacity = .60;
                bs1.Show();
                bs2.Show();
                bs3.Show();
                bs4.Show();
                ps.Show();
            }
            else
            {
                bs1?.Dispose();
                bs2?.Dispose();
                bs3?.Dispose();
                bs4?.Dispose();
                ps?.Dispose();
                GC.Collect();
            }
        }

    }
}
