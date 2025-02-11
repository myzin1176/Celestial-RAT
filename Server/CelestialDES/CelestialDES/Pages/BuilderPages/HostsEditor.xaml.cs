using CelestialDES.wpfForms;
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
using static CelestialDES.Pages.BuilderPages.HostsEditor;
using static CelestialDES.Pages.BuilderPages.Hostslist;

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для HostsEditor.xaml
    /// </summary>
    public partial class HostsEditor : Page
    {
        string awsites = "www.adaware.com;www.arcabit.pl;www.avira.com;www.clamav.net;www.drweb.ru;www.f-prot-antivirus-for-windows.en.softonic.com;www.ikarussecurity.com;www.maxpcsecure.com;www.nanoav.ru;www.quickheal.com;www.sophos.com;www.vba32-antivirus.en.softonic.com;www.xvirus.net;www.zonerantivirus.com;www.avast.ru;www.avast.com;www.bitdefender.com;www.comodo.com;www.emsisoft.com;www.f-secure.com;www.immunet.com;www.mcafee.com;www.eset.com;www.tgsoft.it;www.zillya.ua;www.avg.com;www.bullguard.com;www.escanav.com;www.gdatasoftware.com;www.kaspersky.ru;www.norman.com;www.virustotal.com;www.seqrite.com;www.trendmicro.com;www.spamfighter.com;www.zonealarm.com;www.esetnod32.ru";
        public HostsEditor()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ListView.ItemsSource = "";
            ListView.ItemsSource = Hostslist.hostslist;
            ReqCheck.IsChecked = Convert.ToBoolean(Setting.HostsEdit);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            HostsAdd hostsAdd = new HostsAdd(this);
            hostsAdd.Show();
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            if(ListView.SelectedIndex != -1)
            {
                Hostslist.hostslist.Remove(Hostslist.hostslist.Find(delegate (Hostslist.Host h) { return h.Domain == Hostslist.hostslist[ListView.SelectedIndex].Domain; }));
                ListView.ItemsSource = "";
                ListView.ItemsSource = Hostslist.hostslist;
            }
        }

        private void FetchAV_Click(object sender, RoutedEventArgs e)
        {
            foreach (string aw in awsites.Split(';'))
            {
                Hostslist.Host host = new Hostslist.Host();
                host.IP = "127.0.0.1";
                host.Domain = aw;
                Hostslist.hostslist.Add(host);
            }
            ListView.ItemsSource = "";
            ListView.ItemsSource = Hostslist.hostslist;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Setting.HostsEdit = "true";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Setting.HostsEdit = "false";
        }
    }

    public static class Hostslist
    {
        public static List<Host> hostslist = new List<Host>();
        public struct Host
        {
            public string IP { get; set; }
            public string Domain { get; set; }
        }
    }
}
