using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CelestialDES.winforms
{
    public partial class RemoteDesk : Form
    {
        public int ConnectionID { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }

        public bool RemoteDesktopActive { get; set; }
        public RemoteDesk()
        {
            DoubleBuffered = true;
            KeyPreview = true;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text.Trim().Length < 2)
                textBox2.Text = "10";
            if (comboBox1.SelectedIndex == -1) return;
            if (RemoteDesktopActive)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                comboBox1.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                comboBox2.Enabled = true;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)17);
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                RemoteDesktopActive = false;
                button1.Text = "Start capture";
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                comboBox1.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                comboBox2.Enabled = false;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)16);
                ToSend.AddRange(Encoding.UTF8.GetBytes(textBox1.Text));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(textBox2.Text));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(comboBox1.Text.Replace("\\\\.\\DISPLAY", "")));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(comboBox2.SelectedIndex.ToString()));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                RemoteDesktopActive = true;
                button1.Text = "Stop capture";
            }
        }

        private void RemoteDesk_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)17);
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            RemoteDesktopActive = false;
        }

        private void pbDesktop_MouseClick(object sender, MouseEventArgs e)
        {
            if (!checkBox1.Checked) return;
            if (!RemoteDesktopActive) return;
            pbDesktop.Focus();
            // Point PP = new Point(e.X,e.Y);
            // Point PP = new Point(e.X * (1920 / pbDesktop.Width), e.Y * (1080 / pbDesktop.Height));
            // int PPX = e.X * (1920 / pbDesktop.Width);
            // Point point = new Point((int)(Math.Round((double)(e.X * ((double)ResX / (double)pbDesktop.Width))))),(int)(Math.Round((double)(e.Y * ((double)ResY / (double)pbDesktop.Height)))));
            // int PPX = (e.X / pbDesktop.Width) * ResX;
            int PPX = (int)(((double)e.X / pbDesktop.Width) * ResX);
            int PPY = (int)(((double)e.Y / pbDesktop.Height) * ResY);
            MessageBox.Show(e.X.ToString());
            switch (e.Button)
            {
                case MouseButtons.Left:
                    List<byte> ToSend = new List<byte>();
                    ToSend.Add((int)31);
                    ToSend.AddRange(Encoding.UTF8.GetBytes("" + PPX));
                    ToSend.Add((byte)';');
                    ToSend.AddRange(Encoding.UTF8.GetBytes("" + PPY));
                    ToSend.Add((byte)';');
                    ToSend.AddRange(Encoding.UTF8.GetBytes("L"));
                    PacketSender.Send(ConnectionID, ToSend.ToArray());
                    break;
                case MouseButtons.Right:
                    List<byte> ToSends = new List<byte>();
                    ToSends.Add((int)31);
                    ToSends.AddRange(Encoding.UTF8.GetBytes("" + PPX));
                    ToSends.Add((byte)';');
                    ToSends.AddRange(Encoding.UTF8.GetBytes("" + PPY));
                    ToSends.Add((byte)';');
                    ToSends.AddRange(Encoding.UTF8.GetBytes("R"));
                    PacketSender.Send(ConnectionID, ToSends.ToArray());
                    break;
            }
        }

        private void pbDesktop_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (!RemoteDesktopActive) return;
                int PPX = (int)(((double)e.X / pbDesktop.Width) * ResX);
                int PPY = (int)(((double)e.Y / pbDesktop.Height) * ResY);
                pbDesktop.Focus();
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)31);
                ToSend.AddRange(Encoding.UTF8.GetBytes("" + PPX));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes("" + PPY));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes("D"));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void RemoteDesk_KeyDown(object sender, KeyEventArgs e)
        {
            if (checkBox2.Checked && RemoteDesktopActive)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                List<byte> ToSends = new List<byte>();
                ToSends.Add((int)32);
                ToSends.AddRange(Encoding.UTF8.GetBytes("true;" + Convert.ToInt32(e.KeyCode)));
                PacketSender.Send(ConnectionID, ToSends.ToArray());
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void RemoteDesk_KeyUp(object sender, KeyEventArgs e)
        {
            if (checkBox2.Checked && RemoteDesktopActive)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

                List<byte> ToSends = new List<byte>();
                ToSends.Add((int)32);
                ToSends.AddRange(Encoding.UTF8.GetBytes("false;" + Convert.ToInt32(e.KeyCode)));
                PacketSender.Send(ConnectionID, ToSends.ToArray());
            }
        }

        private bool IsLockKey(Keys key)
        {
            return ((key & Keys.CapsLock) == Keys.CapsLock)
                   || ((key & Keys.NumLock) == Keys.NumLock)
                   || ((key & Keys.Scroll) == Keys.Scroll);
        }
    }
}
