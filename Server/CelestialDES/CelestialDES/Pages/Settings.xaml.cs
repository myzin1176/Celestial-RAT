using CelestialDES.Helper;
using Notifications.Wpf;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        MainWindow mainwindowobj;
        private bool neednotifi = true;
        public Settings(MainWindow mw)
        {
            InitializeComponent();
            mainwindowobj = mw;
        }

        private void EnabledNotifi_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.EnableNotifications = true;
            SettingsC.SaveSetting("EnableNotifications", Convert.ToString(ProgramSettings.EnableNotifications));
        }

        private void EnabledNotifi_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.EnableNotifications = false;
            SettingsC.SaveSetting("EnableNotifications", Convert.ToString(ProgramSettings.EnableNotifications));
        }

        private void EnabledCC_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.NotificationsCC = true;
            SettingsC.SaveSetting("NotificationsCC", Convert.ToString(ProgramSettings.NotificationsCC));
        }

        private void EnabledCC_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.NotificationsCC = false;
            SettingsC.SaveSetting("NotificationsCC", Convert.ToString(ProgramSettings.NotificationsCC));
        }

        private void EnabledLA_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.NewLogA = true;
            SettingsC.SaveSetting("NewLogA", Convert.ToString(ProgramSettings.NewLogA));
        }

        private void EnabledLA_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.NewLogA = false;
            SettingsC.SaveSetting("NewLogA", Convert.ToString(ProgramSettings.NewLogA));
        }

        private void Connected_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogNewClient = true;
            SettingsC.SaveSetting("LogNewClient", Convert.ToString(ProgramSettings.LogNewClient));
        }

        private void Connected_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogNewClient = false;
            SettingsC.SaveSetting("LogNewClient", Convert.ToString(ProgramSettings.LogNewClient));
        }

        private void Trying_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogClientTrying = true;
            SettingsC.SaveSetting("LogClientTrying", Convert.ToString(ProgramSettings.LogClientTrying));

        }
        private void Trying_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogClientTrying = false;
            SettingsC.SaveSetting("LogClientTrying", Convert.ToString(ProgramSettings.LogClientTrying));
        }
        private void NewLogA_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogNewLogA = false;
            SettingsC.SaveSetting("LogNewLogA", Convert.ToString(ProgramSettings.LogNewLogA));
        }
        private void NewLogA_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.LogNewLogA = false;
            SettingsC.SaveSetting("LogNewLogA", Convert.ToString(ProgramSettings.LogNewLogA));
        }

        private void EnabledClC_Checked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.PlayClientCon = true;
            SettingsC.SaveSetting("PlayClientCon", Convert.ToString(ProgramSettings.PlayClientCon));
        }

        private void EnabledClC_Unchecked(object sender, RoutedEventArgs e)
        {
            ProgramSettings.PlayClientCon = false;
            SettingsC.SaveSetting("PlayClientCon", Convert.ToString(ProgramSettings.PlayClientCon));
        }

        private void WindowBlurEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (neednotifi)
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "Settings",
                    Message = "To apply window blur restart celestial!",
                    Type = NotificationType.Information
                });
            }
            SettingsC.SaveSetting("WindowBlur", "true");
        }

        private void WindowBlurEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (neednotifi)
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "Settings",
                    Message = "To apply window blur restart celestial!",
                    Type = NotificationType.Information
                });
            }
            SettingsC.SaveSetting("WindowBlur", "false");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainwindowobj.Height = 650;
            mainwindowobj.Width = 1000;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            neednotifi = false;
            EnabledNotifi.IsChecked = ProgramSettings.EnableNotifications;
            ShowClientConnected.IsChecked = ProgramSettings.NotificationsCC;
            ShowNewLog.IsChecked = ProgramSettings.NewLogA;

            ShowArrive.IsChecked = ProgramSettings.LogNewClient;
            ShowTrying.IsChecked = ProgramSettings.LogClientTrying;
            ShowNewLogA.IsChecked = ProgramSettings.LogNewLogA;

            WindowBlurEnabled.IsChecked = ProgramSettings.WindowBlur;
            EnabledSound.IsChecked = ProgramSettings.PlayClientCon;
            neednotifi = true;
        }
    }
}
