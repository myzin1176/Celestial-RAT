using CelestialDES.Helper;
using CelestialDES.Pages;
using CelestialDES.wpfForms;
using Notifications.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CelestialDES
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static class InformationA
        {
            public static int ServersCountInt { get; set; } = 0;
            public static int ClientsConnectedInt { get; set; } = 0;
            public static int LogsCountInt { get; set; } = 0;

#if DEBUG
            public static string buildtype = "debug";
#else
            public static string buildtype = "release";
#endif
            public static string username;

            public static string version = "1.13.1";
        }
        public static class ProgramSettings
        {
            public static bool EnableNotifications = true;
            public static bool NotificationsCC = true;
            public static bool NewLogA = true;

            public static bool PlayClientCon = true;
            public static bool WindowBlur = true;

            public static bool LogNewClient = true;
            public static bool LogClientTrying = true;
            public static bool LogNewLogA = true;

            public static int crashcode = 0;
            public static string drivername = "";
            public static string insname = "";
            public static bool isServer = true;
            public static string password = "";
        }

        public static string getRAW(string url)
        {
            WebRequest request = WebRequest.CreateHttp(url);
            WebResponse response = request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = null;


            readStream = new StreamReader(receiveStream);

            string data = readStream.ReadToEnd();

            response.Dispose();
            readStream.Dispose();

            return data;

        }

        private static string getHWIDelement(string command, string returnvalue)
        {
            var mbs = new ManagementObjectSearcher(command);
            ManagementObjectCollection mbsList = mbs.Get();
            foreach (ManagementObject mo in mbsList)
            {
                return mo[returnvalue].ToString();
            }
            return null;
        }

        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public Home homeobj = new Home();
        public Builder builderobj = new Builder();
        Settings settingsobj;
        Changelog changelog;
        Downloader downloader;
        Socket socket;
        public Clients clientsobj;
        ThicknessAnimation tabMarginAnim;

        private void InitSettings()
        {
            try
            {
                if (SettingsC.readSetting("EnableNotifications") != null) ProgramSettings.EnableNotifications = Convert.ToBoolean(SettingsC.readSetting("EnableNotifications"));
                if (SettingsC.readSetting("NotificationsCC") != null) ProgramSettings.NotificationsCC = Convert.ToBoolean(SettingsC.readSetting("NotificationsCC"));
                if (SettingsC.readSetting("NewLogA") != null) ProgramSettings.NewLogA = Convert.ToBoolean(SettingsC.readSetting("NewLogA"));
                if (SettingsC.readSetting("WindowBlur") != null) ProgramSettings.WindowBlur = Convert.ToBoolean(SettingsC.readSetting("WindowBlur"));

                if (SettingsC.readSetting("PlayClientCon") != null) ProgramSettings.PlayClientCon = Convert.ToBoolean(SettingsC.readSetting("PlayClientCon"));

                if (SettingsC.readSetting("LogNewClient") != null) ProgramSettings.LogNewClient = Convert.ToBoolean(SettingsC.readSetting("LogNewClient"));
                if (SettingsC.readSetting("LogClientTrying") != null) ProgramSettings.LogClientTrying = Convert.ToBoolean(SettingsC.readSetting("LogClientTrying"));
                if (SettingsC.readSetting("LogNewLogA") != null) ProgramSettings.LogNewLogA = Convert.ToBoolean(SettingsC.readSetting("LogNewLogA"));
            }
            catch (Exception ex) {

                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Settings Manager",
                    Message = "Error while loading settings.\n" + ex.Message,
                    Type = NotificationType.Error
                });
            }
        }
        private void InitSSL()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.DefaultConnectionLimit = 9999;
            }
            catch { }
            try
            {
                WebRequest.DefaultWebProxy = new WebProxy();
            }
            catch { }
        }

        public MainWindow()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandling);
            InitSSL();

            InformationA.username = "Owner";

            clientsobj = new Clients(this);
            socket = new Socket(this);
            settingsobj = new Settings(this);
            changelog = new Changelog();
            downloader = new Downloader();

            InitializeComponent();

            /*string needeula = "true";
            try
            {
                Microsoft.Win32.RegistryKey keyy;
                keyy = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Celestial", false);
                needeula = Encryption.Base64Decode(keyy.GetValue(Encryption.Base64Encode("eula")).ToString());
            }
            catch { }
            if(needeula == "true") 
            {
                this.Hide();
                eula eula = new eula();
                eula.Show();
                return;
            }*/
            InitSettings();
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            tabMarginAnim = new ThicknessAnimation();
            tabMarginAnim.EasingFunction = new PowerEase();
            tabMarginAnim.From = new Thickness(66);
            tabMarginAnim.To = new Thickness(16);
            tabMarginAnim.Duration = TimeSpan.FromMilliseconds(150);

            homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Celestial " + InformationA.version);
            homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Welcome back, " + InformationA.username);

            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
#if !DEBUG
                Title = "Celestial",
#endif
#if DEBUG
                Title = "Celestial blyat edition",
#endif
                Message = "Welcome back, " + InformationA.username + "\nYou currently running the " + InformationA.buildtype + " build",
                Type = NotificationType.Success
            });
        }

        static void UnhandledExceptionHandling(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            System.Threading.Thread workerThread1 = new System.Threading.Thread(() =>
            {
                Thread.Sleep(10000);
                Environment.Exit(0);
            });
            workerThread1.IsBackground = true;
            workerThread1.Start();
            MessageBox.Show("Celestial Crashed! \nException handler caught: " + e.Message + "\n Stack:" + e.StackTrace, "", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(0);
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tg_Btn.IsChecked = false;
        }

        private bool animation_is_running = false;
        private void Grid_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (!animation_is_running)
                {
                    var decreaseOpacityAnim = new DoubleAnimation(0.3, TimeSpan.FromSeconds(0.15));
                    this.BeginAnimation(UIElement.OpacityProperty, decreaseOpacityAnim);
                    animation_is_running = true;
                }

                this.DragMove();
            }
            else
            {
                if (animation_is_running)
                {
                    var increaseOpacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.15));
                    this.BeginAnimation(UIElement.OpacityProperty, increaseOpacityAnim);
                    animation_is_running = false;
                }
            }
        }
        private void DoubleAnimation_Completed(object sender, RoutedEventArgs e)
        {
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(200);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }
        void anim_Completed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnHome_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnHome;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Home";
            }
        }

        private void btnHome_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnDashboard_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnDashboard;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Clients";
            }
        }

        private void btnDashboard_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnSocket_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnSocket;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Socket / Net";
            }
        }

        private void btnSocket_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InformationA.LogsCountInt = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\logs\\", "*", SearchOption.TopDirectoryOnly).Length;
            }
            catch { }
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = homeobj;
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = clientsobj;
        }
        private void btnSocket_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = socket;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                About Aboutwin = new About();
                Aboutwin.Show();
            }
        }

        private void home_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnBuilder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnBuilder;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Builder";
            }
        }

        private void btnBuilder_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnBuilder_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = builderobj;
        }

        private void btnSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnSettings;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Settings";
            }
        }

        private void btnSettings_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = settingsobj;
        }

        private void btnChange_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnChange;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Changelog";
            }
        }

        private void btnChange_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = changelog;
        }

        private void btnDownloader_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Tg_Btn.IsChecked == false)
            {
                Popup.PlacementTarget = btnDownloader;
                Popup.Placement = PlacementMode.Right;
                Popup.IsOpen = true;
                Header.PopupText.Text = "Downloader";
            }
        }

        private void btnDownloader_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnDownloader_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = downloader;
        }

        private void home_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
