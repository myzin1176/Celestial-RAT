using CelestialDES.Helper;
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

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для HostsAdd.xaml
    /// </summary>
    public partial class HostsAdd : Window
    {
        HostsEditor hostobj;
        public HostsAdd(HostsEditor host)
        {
            hostobj = host;
            InitializeComponent();
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Hostslist.Host host = new Hostslist.Host();
            host.IP = TextBox_IP.Text.Replace(';', ' ').Replace('|', ' ');
            host.Domain = TextBox_DOM.Text.Replace(';', ' ').Replace('|', ' ');
            Hostslist.hostslist.Add(host);
            hostobj.ListView.ItemsSource = "";
            hostobj.ListView.ItemsSource = Hostslist.hostslist;

            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }
    }
}
