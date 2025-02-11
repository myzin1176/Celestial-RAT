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

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Socket.xaml
    /// </summary>
    public partial class Socket : Page
    {
        public MainWindow MainWindowform;
        PortAdd portAdd;
        ConnectServer connectServer;
        public Socket(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindowform = mainWindow;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (InformationA.ServersCountInt >= 1)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Socket listen",
                    Message = "Server is already active!",
                    Type = NotificationType.Error
                });
                return;
            }
            portAdd?.Close();
            portAdd = new PortAdd(this);
            portAdd.Show();
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            if (InformationA.ServersCountInt == 0)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Socket listen",
                    Message = "No active servers found",
                    Type = NotificationType.Error
                });
                return;
            }
            InformationA.ServersCountInt--;
            Networking.Server.MainServer.Stop();
            Networking.NClient.MainClient.Disconnect();
            ListViewP.Items.Clear();
            MainWindowform.clientsobj.ClearUsersList();
            ProgramSettings.isServer = true;
            MainWindowform.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Stopped port listening/server connection");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (InformationA.ServersCountInt >= 1)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Socket listen",
                    Message = "Server is already active!",
                    Type = NotificationType.Error
                });
                return;
            }

            connectServer?.Close();
            connectServer = new ConnectServer(this);
            connectServer.Show();
        }
    }
}
