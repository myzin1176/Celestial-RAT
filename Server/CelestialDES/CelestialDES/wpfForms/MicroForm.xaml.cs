using CelestialDES.Helper;
using CelestialDES.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для MicroForm.xaml
    /// </summary>
    /// 
    public partial class MicroForm : Window
    {
        public int ConnectionID { get; set; }
        public int devicescount { get; set; }
        Clients clientobj;
        public MicroForm(Clients clients)
        {
            InitializeComponent();
            clientobj = clients;
            for (int i = 0; i < devicescount+1; i++)
            {
                DevicesBox.Items.Add(i.ToString());
            }
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
        }
        private bool animation_is_running = false;
        private void Grid_MouseMove_1(object sender, RoutedEventArgs e)
        {
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (!animation_is_running)
                    {
                        var decreaseOpacityAnim = new DoubleAnimation(0.3, (Duration)TimeSpan.FromSeconds(0.15));
                        this.BeginAnimation(UIElement.OpacityProperty, decreaseOpacityAnim);
                        animation_is_running = true;
                    }

                    this.DragMove();
                }
                else
                {
                    if (animation_is_running)
                    {
                        var increaseOpacityAnim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(0.15));
                        this.BeginAnimation(UIElement.OpacityProperty, increaseOpacityAnim);
                        animation_is_running = false;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
            Clients.needplay = false;
        }

        void anim_Completed(object sender, EventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)19);
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PowerEase easeing = new PowerEase();
            easeing.EasingMode = EasingMode.EaseOut;
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = easeing;
            tabanim.From = 0;
            tabanim.To = 1;
            tabanim.Duration = TimeSpan.FromMilliseconds(70);
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)19);
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            Clients.needplay = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BufferBox.Text == string.Empty || BufferBox2.Text == string.Empty || DevicesBox.Text == string.Empty) return;

            List<byte> ToSend = new List<byte>();
            if (Clients.needplay)
            {
                ButtonS.Content = "Start";
                ToSend.Add((int)19);
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                Clients.needplay = false;
            }else
            {
                ButtonS.Content = "Stop";
                ToSend.Add((int)18);
                string HQ;
                if (BufferBox2.Text.Contains("8"))
                {
                    HQ = "0";
                    Clients.UsingHQPlayer = false;
                }
                else
                {
                    HQ = "1";
                    Clients.UsingHQPlayer = true;
                }
                ToSend.AddRange(Encoding.UTF8.GetBytes(DevicesBox.Text + ";" + Convert.ToInt32(BufferBox.Text.Split(' ')[0]) * 1024 + ";" + HQ));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                Clients.needplay = true;
            }
        }
    }
}
