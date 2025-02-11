using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC
{
    public partial class pinksavage : Form
    {
        public pinksavage()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams { get { CreateParams wew = base.CreateParams; wew.ExStyle = base.CreateParams.ExStyle | 0x20; return wew; } }
        double cur = .05;
        int akis = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = true;

            if (cur > 0.6)
            {
                akis = 1;
                cur = 0.59;
            }
            if (cur < 0.2)
            {
                akis = 0;

            }

            if (akis == 1)
            {
                cur = cur - 0.01;
            }
            else
            {
                cur = cur + 0.01;
            }
            this.Opacity = cur;
        }

        private void pinksavage_Load(object sender, EventArgs e)
        {

        }
    }
}
