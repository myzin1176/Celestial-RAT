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

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public class ServersForw_ClientsCon
        {
            public string ServersForwarding { get; set; }
            public string ClientsConnected { get; set; }
            public string LogsCount { get; set; }
        }
        public Home()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServersForw_ClientsCon datacon = new ServersForw_ClientsCon() { ServersForwarding = InformationA.ServersCountInt.ToString(), ClientsConnected = InformationA.ClientsConnectedInt.ToString(), LogsCount = InformationA.LogsCountInt.ToString() };
            DataContext = datacon;
        }
    }
}
