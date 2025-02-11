using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для SysInfo.xaml
    /// </summary>
    public partial class eula : Window
    {
        public eula()
        {
            WindowBlur.SetIsEnabled(this, true);
            InitializeComponent();
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
            System.Windows.Application.Current.Shutdown();
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

            CultureInfo ci = CultureInfo.InstalledUICulture;

            if (ci.Name == "ru-RU")
                RU.Visibility = Visibility.Visible;
            else
                EN.Visibility = Visibility.Visible;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (metroCheckBox2.IsChecked == true)
            {

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    try
                    {
                        SettingsC.SaveSetting("eula", "false");
                        SettingsC.SaveSetting("EnableNotifications", "true");
                        SettingsC.SaveSetting("NotificationsCC", "true");
                        SettingsC.SaveSetting("NewLogA", "true");
                        SettingsC.SaveSetting("WindowBlur", "true"); 
                        SettingsC.SaveSetting("PlayClientCon", "true");
                        SettingsC.SaveSetting("LogNewClient", "true");
                        SettingsC.SaveSetting("LogClientTrying", "true");
                        SettingsC.SaveSetting("LogNewLogA", "true");
                        Environment.Exit(0);
                    }
                    catch { }
                }
                else System.Windows.MessageBox.Show("Read agreement once again!");
            }
        }

        private void metroCheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void metroCheckBox2_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}