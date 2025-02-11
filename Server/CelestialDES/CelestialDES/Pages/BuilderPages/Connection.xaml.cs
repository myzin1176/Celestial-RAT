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
    /// Логика взаимодействия для Connection.xaml
    /// </summary>
    public partial class Connection : Page
    {
        private string getRAW(string url)
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
        public Connection()
        {
            InitializeComponent();
        }

        public void Check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pastedata = getRAW(DynamicText.Text);
                string[] words = pastedata.Split(':');
                TextCheckDNS.Text = "IP/DNS - " + words[0];
                TextCheckPort.Text = "Port - " + words[1];
                TextCheckDNS.Visibility = Visibility.Visible;
                TextCheckPort.Visibility = Visibility.Visible;
                TextCheckError.Visibility = Visibility.Hidden;
            }
            catch { TextCheckError.Visibility = Visibility.Visible; }
        }

        private void StaticRadio_Checked(object sender, RoutedEventArgs e)
        {
            Setting.UsePastebin = "false";
        }

        private void DynamicRadio_Checked(object sender, RoutedEventArgs e)
        {
            Setting.UsePastebin = "true";
        }

        private void DNS_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.DNS = DNS.Text;
        }

        private void passwordtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.DecryptionPassword = passwordtext.Text;
        }

        private void Port_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.Port = Port.Text;
        }

        private void DynamicText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting.PastebinLink = DynamicText.Text;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0)
            {
                Setting.needserver = "false";
                passwordtext.Visibility = Visibility.Hidden;
                passwordtblock.Visibility = Visibility.Hidden;
            }
            else
            {
                Setting.needserver = "true";
                passwordtext.Visibility = Visibility.Visible;
                passwordtblock.Visibility = Visibility.Visible;
            }
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0)
            {
                Setting.needserver = "false";
                passwordtext.Visibility = Visibility.Hidden;
                passwordtblock.Visibility = Visibility.Hidden;
            }
            else
            {
                Setting.needserver = "true";
                passwordtext.Visibility = Visibility.Visible;
                passwordtblock.Visibility = Visibility.Visible;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DNS.Text = Setting.DNS;
            Port.Text = Setting.Port;
            DynamicText.Text = Setting.PastebinLink;
            passwordtext.Text = Setting.DecryptionPassword;
            if (Convert.ToBoolean(Setting.UsePastebin))
            {
                StaticRadio.IsChecked = false;
                DynamicRadio.IsChecked = true;
            }
            else
            {
                StaticRadio.IsChecked = true;
                DynamicRadio.IsChecked = false;
            }

            if (Convert.ToBoolean(Setting.needserver)) { cmbSelect.SelectedIndex = 1; passwordtext.Visibility = Visibility.Visible; passwordtblock.Visibility = Visibility.Visible; }
            else { cmbSelect.SelectedIndex = 0; passwordtext.Visibility = Visibility.Hidden; passwordtblock.Visibility = Visibility.Hidden; }
        }
    }
}
