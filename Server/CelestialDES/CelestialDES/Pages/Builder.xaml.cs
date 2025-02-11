using CelestialDES.Pages.BuilderPages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Builder.xaml
    /// </summary>
    public partial class Builder : Page
    {
        ThicknessAnimation tabMarginAnim;
        General generalobj = new General();
        Connection connectionobj = new Connection();
        Install installobj = new Install();
        Misc miscobj = new Misc();
        Assembly assemblyobj = new Assembly();
        Final finalobj = new Final();
        HostsEditor hostsobj = new HostsEditor();

        public Builder()
        {
            InitializeComponent();

            tabMarginAnim = new ThicknessAnimation();
            tabMarginAnim.EasingFunction = new PowerEase();
            tabMarginAnim.From = new Thickness(66);
            tabMarginAnim.To = new Thickness(16);
            tabMarginAnim.Duration = TimeSpan.FromMilliseconds(150);
        }

        private void btnGeneral_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnGeneral;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "General";
        }

        private void btnGeneral_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = generalobj;
        }

        private void btnConnection_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnConnection;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Connection";
        }

        private void btnConnection_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = connectionobj;
        }

        private void btnInstall_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnInstall;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Install";
        }

        private void btnInstall_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = installobj;
        }

        private void btnHosts_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnHosts;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Hosts";
        }

        private void btnHosts_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnHosts_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = hostsobj;
        }

        private void btnMisc_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnMisc;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Miscellaneous";
        }

        private void btnMisc_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnMisc_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = miscobj;
        }

        private void btnAssembly_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnAssembly;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Assembly";
        }

        private void btnAssembly_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnAssembly_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = assemblyobj;
        }

        private void btnFinal_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.PlacementTarget = btnFinal;
            Popup.Placement = PlacementMode.Right;
            Popup.IsOpen = true;
            Header.PopupText.Text = "Final";
        }

        private void btnFinal_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.Visibility = Visibility.Collapsed;
            Popup.IsOpen = false;
        }

        private void btnFinal_Click(object sender, RoutedEventArgs e)
        {
            fContainer.BeginAnimation(Frame.MarginProperty, tabMarginAnim);
            fContainer.Content = finalobj;
        }
    }

    public class Setting
    {
        //General
        public static string MutexName = "Celestial_9112213264593432444112";
        public static string adminRight = "false";
        public static string ClientTag = "Guest";
        public static bool needadmin = false;
        public static string LibFolder = "Program folder";
        public static string LibSubFolder = "CelestialLibs";

        public static string isLotl = "false";
        public static string Lotlfilename = "Random";
        public static string LotlCMD = "/\\ visit celestialsoft.su \\/";
        public static string LotlHollowedProcess = "RuntimeBroker";

        //Connection
        public static string DNS = "127.0.0.1";
        public static string Port = "3333";
        public static string UsePastebin = "false";
        public static string PastebinLink = "https://pastebin.com/raw/XXXXX";
        public static string needserver = "false";
        public static string DecryptionPassword = "P@$$w0rd";

        //Install
        public static string needInstall = "true";
        public static string InstallFolder = "%appdata%";
        public static string InstallSubFolder = "Celestial";
        public static string InstallName = "Celestial.exe";
        public static string InstallMelt = "false";
        public static string AutoRunMethod = "none";
        public static string TryAnotherIfFails = "false";
        public static string AutoRunhidden = "false";
        public static string AutoRunHighest = "false";
        public static string RegistryKeyName = "Celestial";
        public static string NameInTasks = "Celestial";
        public static string HideEverything = "false";
        public static string HideEverythingSys = "false";
        public static string USBSpread = "false";
        public static string USBSpreadn = "Celestial.exe";
        
        // Hosts
        public static string HostsEdit = "false";

        //Misc
        public static string DetectDebugger = "true";
        public static string DetectSandboxie = "true";
        public static string DetectSmallDisk = "true";
        public static string DetectXP = "true";
        public static string DetectVirtualEnviroment = "true";
        public static string CritProcess = "false";
        public static string needwatchdog = "false";
        public static string watchdogfolder = "%temp%";
        public static string watchdogname = "watchdog.vbs";

        public static string DisableTaskMGR = "false";
        public static string DisableRegistry = "false";
        public static string DisableUAC = "false";
        public static string DisableFirewall = "false";
        public static string DisableWinDef = "false";
        public static string DisableCMD = "false";
        public static string DisableRun = "false";
        public static string DisableWinKeys = "false";

        public static string CBlack = "false";
        public static string CBlacklist = "RU;US;BY";

        public static string needweb = "false";
        public static string weburl = "";
        public static string chromekill = "false";

        //Assembly
        public static string product = "";
        public static string description = "";
        public static string company = "";
        public static string copyright = "";
        public static string trademarks = "";
        public static string originalfilename = "";
        public static string productver = "0.0.0.0";
        public static string filever = "0.0.0.0";
        public static bool needicon = false;
        public static string icon = "";
        public static bool needsignature = false;
        public static string signature = "";

        //Final
        public static string askInstall = "true";

    }
}
