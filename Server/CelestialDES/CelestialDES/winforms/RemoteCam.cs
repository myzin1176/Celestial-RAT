using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelestialDES.winforms
{
    public partial class RemoteCam : Form
    {
        public int ConnectionID { get; set; }
        public bool RemoteCamActive { get; set; }
        public Image GetImage { get; set; }
        public RemoteCam()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim().Length < 2)
                textBox2.Text = "10";
            if (comboBox1.SelectedIndex == -1) return;
            if (RemoteCamActive)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                comboBox1.Enabled = true;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)53);
                ToSend.AddRange(Encoding.UTF8.GetBytes("2"));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                RemoteCamActive = false;
                button1.Text = "Start capture";
            }
            else
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                comboBox1.Enabled = false;
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)53);
                ToSend.AddRange(Encoding.UTF8.GetBytes("1;" + comboBox1.SelectedIndex + ";" + textBox1.Text + ";" + textBox2.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                RemoteCamActive = true;
                button1.Text = "Stop capture";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void RemoteCam_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)53);
            ToSend.AddRange(Encoding.UTF8.GetBytes("2"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            RemoteCamActive = false;
        }
    }
}
