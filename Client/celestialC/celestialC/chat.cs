using celestialC.Helper.Networking;
using celestialC.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC
{
    public partial class chat : Form
    {
        public chat()
        {
            InitializeComponent();
        }

        private void chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            List<byte> ToSend = new List<byte>();
            ToSend.Add(17);
            ToSend.AddRange(Encoding.UTF8.GetBytes(textBox1.Text));
            PacketSender.Send(ToSend.ToArray());
            textBox2.AppendText("You: " + textBox1.Text + Environment.NewLine);
            textBox1.Clear();
        }
    }
}
