using CelestialDES.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Toolbelt.Drawing;

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для Assembly.xaml
    /// </summary>
    public partial class Assembly : Page
    {
        public Assembly()
        {
            InitializeComponent();
        }

        private string GetIcon(string path)
        {
            try
            {
                string tempFile = Path.GetTempFileName() + ".ico";
                using (FileStream fs = new FileStream(tempFile, FileMode.Create))
                {
                    IconExtractor.Extract1stIconTo(path, fs);
                }
                return tempFile;
            }
            catch { }
            return "";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Choose Icon";
                ofd.Filter = "Icons Files(*.exe;*.ico;)|*.exe;*.ico";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.ToLower().EndsWith(".exe"))
                    {
                        string ico = GetIcon(ofd.FileName);
                        BitmapImage image = new BitmapImage(
                             new Uri(ico));
                        imageIcon.Source = image;
                        Setting.needicon = true;
                        Setting.icon = ico;
                    }
                    else
                    {
                        BitmapImage image = new BitmapImage(
                             new Uri(ofd.FileName));
                        imageIcon.Source = image;
                        Setting.needicon = true;
                        Setting.icon = ofd.FileName;
                    }
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            imageIcon.Source = null;
            Setting.needicon = false;
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Choose Icon";
                ofd.Filter = "Exe Files(*.exe)|*.exe";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Setting.needsignature = true;
                    Setting.signature = ofd.FileName;
                }
            }
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.needsignature = false;
        }

        private void Clone_Click(object sender, RoutedEventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Executable (*.exe)|*.exe";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(openFileDialog.FileName);

                    OriginalFNText.Text = fileVersionInfo.InternalName ?? string.Empty;
                    DescriptionText.Text = fileVersionInfo.FileDescription ?? string.Empty;
                    CompanyText.Text = fileVersionInfo.CompanyName ?? string.Empty;
                    ProductText.Text = fileVersionInfo.ProductName ?? string.Empty;
                    CopyrightText.Text = fileVersionInfo.LegalCopyright ?? string.Empty;
                    TradeMarksText.Text = fileVersionInfo.LegalTrademarks ?? string.Empty;

                    FVerText.Text = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";
                    PVerText.Text = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";

                    Setting.originalfilename = fileVersionInfo.InternalName ?? string.Empty;
                    Setting.description = fileVersionInfo.FileDescription ?? string.Empty;
                    Setting.company = fileVersionInfo.CompanyName ?? string.Empty;
                    Setting.product = fileVersionInfo.ProductName ?? string.Empty;
                    Setting.copyright = fileVersionInfo.LegalCopyright ?? string.Empty;
                    Setting.trademarks = fileVersionInfo.LegalTrademarks ?? string.Empty;
                    Setting.productver = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";
                    Setting.filever = $"{fileVersionInfo.FileMajorPart.ToString()}.{fileVersionInfo.FileMinorPart.ToString()}.{fileVersionInfo.FileBuildPart.ToString()}.{fileVersionInfo.FilePrivatePart.ToString()}";
                }
            }
        }
    }
}
