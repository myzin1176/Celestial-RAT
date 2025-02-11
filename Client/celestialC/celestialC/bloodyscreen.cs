using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC
{
    public partial class bloodyscreen : Form
    {
        Random rnd = new Random();
        int maxy; int maxx;
        void sagaCiz()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush b = new SolidBrush(Color.FromArgb(rnd.Next(100, 255), Color.Red));
            int mysize = rnd.Next(3, 15);
            int genpos = 0;
            int ranint = rnd.Next(0, 11);
            if (ranint < 5)
            {
                genpos = rnd.Next(-5, 65);
            }
            if (ranint > 4 && ranint < 8)
            {
                genpos = rnd.Next(65, 120);
            }
            if (ranint == 8 || ranint == 9)
            {
                genpos = rnd.Next(120, 250);
            }
            if (ranint == 10)
            {
                genpos = rnd.Next(250, 500);
            }

            if (rnd.Next(0, 2) == 1)
            {
                g.FillEllipse(b, new Rectangle(maxx - genpos, rnd.Next(0, maxy - 50), mysize, rnd.Next(3, 15)));
            }
            else
            {
                g.FillRectangle(b, new Rectangle(maxx - genpos, rnd.Next(0, maxy - 50), mysize, rnd.Next(3, 15)));
            }
            g.Dispose();
        }
        void solaCiz()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush b = new SolidBrush(Color.FromArgb(rnd.Next(100, 255), Color.Red));
            int mysize = rnd.Next(3, 15);
            int genpos = 0;
            int ranint = rnd.Next(0, 11);
            if (ranint < 5)
            {
                genpos = rnd.Next(-5, 65);
            }
            if (ranint > 4 && ranint < 8)
            {
                genpos = rnd.Next(65, 120);
            }
            if (ranint == 8 || ranint == 9)
            {
                genpos = rnd.Next(120, 250);
            }
            if (ranint == 10)
            {
                genpos = rnd.Next(250, 500);
            }

            if (rnd.Next(0, 2) == 1)
            {
                g.FillEllipse(b, new Rectangle(genpos, rnd.Next(0, maxy - 50), mysize, rnd.Next(3, 15)));
            }
            else
            {
                g.FillRectangle(b, new Rectangle(genpos, rnd.Next(0, maxy - 50), mysize, rnd.Next(3, 15)));
            }
            g.Dispose();
        }
        void usteCiz()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush b = new SolidBrush(Color.FromArgb(rnd.Next(100, 255), Color.Red));
            int mysize = rnd.Next(3, 15);
            int genpos = 0;
            int ranint = rnd.Next(0, 11);
            if (ranint < 5)
            {
                genpos = rnd.Next(-5, 20);
            }
            if (ranint > 4 && ranint < 8)
            {
                genpos = rnd.Next(20, 40);
            }
            if (ranint == 8 || ranint == 9)
            {
                genpos = rnd.Next(40, 60);
            }
            if (ranint == 10)
            {
                genpos = rnd.Next(60, 100);
            }

            if (rnd.Next(0, 2) == 1)
            {
                g.FillEllipse(b, new Rectangle(rnd.Next(20, maxx - 20), genpos, mysize, rnd.Next(3, 15)));
            }
            else
            {
                g.FillRectangle(b, new Rectangle(rnd.Next(20, maxx - 20), genpos, mysize, rnd.Next(3, 15)));
            }
            g.Dispose();
        }
        void altaCiz()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush b = new SolidBrush(Color.FromArgb(rnd.Next(100, 255), Color.Red));
            int mysize = 0;
            mysize = rnd.Next(3, 15);
            int genpos = 0;
            int ranint = rnd.Next(0, 11);
            if (ranint < 5)
            {
                genpos = rnd.Next(-5, 20);
            }
            if (ranint > 4 && ranint < 8)
            {
                genpos = rnd.Next(20, 40);
            }
            if (ranint == 8 || ranint == 9)
            {
                genpos = rnd.Next(40, 60);
            }
            if (ranint == 10)
            {
                genpos = rnd.Next(60, 100);
            }

            if (rnd.Next(0, 2) == 1)
            {
                g.FillEllipse(b, new Rectangle(rnd.Next(20, maxx - 20), maxy - genpos, mysize, rnd.Next(3, 15)));
            }
            else
            {
                g.FillRectangle(b, new Rectangle(rnd.Next(20, maxx - 20), maxy - genpos, mysize, rnd.Next(3, 15)));
            }
            g.Dispose();
        }
        int dropdownxpos = 0;
        int finishdrop = 0;
        int kalinlik = 0;

        void dropit(object state)
        {
            int startpos = dropdownxpos;
            int bitir = finishdrop;

            for (int a = 0; a < bitir; a++)
            {
                try
                {
                    Graphics g = this.CreateGraphics();
                    SolidBrush b = new SolidBrush(Color.FromArgb(255, Color.Red));
                    g.FillEllipse(b, new Rectangle(startpos, -20, kalinlik, a));
                    g.Dispose();
                    Thread.Sleep(2);
                }
                catch { }
            }
        }

        void verticalDrop()
        {
            dropdownxpos = rnd.Next(1, maxx - 5);
            finishdrop = rnd.Next(5, maxy - 20);
            kalinlik = rnd.Next(5, 25);
            ThreadPool.QueueUserWorkItem(dropit);
        }
        public bloodyscreen()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams { get { CreateParams wew = base.CreateParams; wew.ExStyle = base.CreateParams.ExStyle | 0x20; return wew; } }

        private void bloodyscreen_Load(object sender, EventArgs e)
        {
            this.Opacity = .99;
            this.BackColor = Color.Maroon;
            this.TransparencyKey = Color.Maroon;
            this.TopMost = true;
            maxx = this.Size.Width;
            maxy = this.Size.Height + 20;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int k = rnd.Next(0, 10);
            if (k < 3)
            {
                sagaCiz();
            }
            if (k > 2 && k < 5)
            {
                solaCiz();
            }
            if (k == 5 || k == 8)
            {
                usteCiz();
            }
            if (k == 6 || k == 7)
            {
                altaCiz();
            }
            if (k == 9)
            {
                verticalDrop();
            }
        }
    }
}
