using CelestialDES.Helper;
using CelestialDES.Pages;
using CelestialDES.Pages.BuilderPages;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для StartupManager.xaml
    /// </summary>
    public partial class StartupManager : Window
    {
        public int ConnectionID { get; set; }
        Clients ClientsObj;
        public StartupManager(Clients clients)
        {
            ClientsObj = clients;
            InitializeComponent();
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
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
        }

        void anim_Completed(object sender, EventArgs e)
        {
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
        private bool animation_is_running = false;
        private void StackPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void ST_remove(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)51);
            if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKCU/Run"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("1;"));
            else if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKLM/Run"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("2;"));
            else if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKCU/RunOnce"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("3;"));
            else if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKLM/RunOnce"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("4;"));
            else if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKLM/WOW6432NODE"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("5;"));
            else if (ClientsObj.startuplist[ListViewF.SelectedIndex].type.Contains("HKLM/WOW6432NODEOnce"))
                ToSend.AddRange(Encoding.UTF8.GetBytes("6;"));
            ToSend.AddRange(Encoding.UTF8.GetBytes(ClientsObj.startuplist[ListViewF.SelectedIndex].name));
            PacketSender.Send(ConnectionID, ToSend.ToArray());

            List<byte> ToSend1 = new List<byte>();
            ToSend1.Add((int)51);
            ToSend1.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(ConnectionID, ToSend1.ToArray());
        }

        private void ST_add(object sender, RoutedEventArgs e)
        {
            StartupManagerAdd startupManagerAdd = new StartupManagerAdd();
            startupManagerAdd.ConnectionID = ConnectionID;
            startupManagerAdd.Show();
        }

        private void ST_Refresh(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)51);
            ToSend.AddRange(Encoding.UTF8.GetBytes("0"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }
    }
}
