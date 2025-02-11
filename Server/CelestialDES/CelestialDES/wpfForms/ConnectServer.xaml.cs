using CelestialDES.Helper;
using CelestialDES.Networking;
using CelestialDES.Pages;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для ConnectServer.xaml
    /// </summary>
    public partial class ConnectServer : Window
    {
        Socket socketform;
        public ConnectServer(Socket socket)
        {
            InitializeComponent();
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            socketform = socket;
        }

        private bool animation_is_running = false;
        private void Grid_MouseMove_1(object sender, RoutedEventArgs e)
        {
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (!animation_is_running)
                    {
                        var decreaseOpacityAnim = new DoubleAnimation(0.3, (Duration)TimeSpan.FromSeconds(0.15));
                        this.BeginAnimation(UIElement.OpacityProperty, decreaseOpacityAnim);
                        animation_is_running = true;
                    }

                    this.DragMove();
                }
                else
                {
                    if (animation_is_running)
                    {
                        var increaseOpacityAnim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(0.15));
                        this.BeginAnimation(UIElement.OpacityProperty, increaseOpacityAnim);
                        animation_is_running = false;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }

        void anim_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PowerEase easeing = new PowerEase();
            easeing.EasingMode = EasingMode.EaseOut;
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = easeing;
            tabanim.From = 0;
            tabanim.To = 1;
            tabanim.Duration = TimeSpan.FromMilliseconds(70);
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }

        private struct Listener
        {
            public string Ip { get; set; }
            public int Port { get; set; }
            public string Status { get; set; }
        }

        private void TextBox_P_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(TextBox_P.Text, "[^0-9]"))
            {
                TextBox_P.Text = TextBox_P.Text.Remove(TextBox_P.Text.Length - 1);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pswrd = TextBox_PSWRD.Text;
            bool noconnect = false;
            Listener listener = new Listener();
            listener.Ip = TextBox_IP.Text;
            listener.Port = Int32.Parse(TextBox_P.Text);
       //     try
            {
                NClient.MainClient.Connect(TextBox_IP.Text, Int32.Parse(TextBox_P.Text));
                Thread.Sleep(100);
                List<byte> ToSendDE = new List<byte>();
                ToSendDE.Add(0);
                ToSendDE.AddRange(Encoding.UTF8.GetBytes("AdmIniStRaTorConNecTIon"));
                NClient.MainClient.Send(ToSendDE.ToArray(), pswrd);
                Thread.Sleep(1000);
                if (!NClient.MainClient.Connected)
                {
                    var notificationManager = new NotificationManager();

                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Server connection",
                        Message = "Invalid hostname/password/administrator limit reached",
                        Type = NotificationType.Error
                    });
                    noconnect = true;
                }
                listener.Status = "Connected";
            }
            /*catch
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Server connection (ex)",
                    Message = "Invalid hostname/password/administrator limit reached",
                    Type = NotificationType.Error
                });
                noconnect = true;
            }*/
            if (noconnect) return;
            socketform.ListViewP.Items.Add(listener);
            socketform.MainWindowform.homeobj.Logs_Listbox.Items.Add("[" + DateTime.Now.ToString("hh:mm:ss") + " INFO] Connected to - " + TextBox_IP.Text);
            socketform.MainWindowform.clientsobj.ClearUsersList();
            ProgramSettings.password = pswrd;
            ProgramSettings.isServer = false;
            System.Threading.Thread workerThread = new System.Threading.Thread(() =>
            {
                bool noabrt = true;
                while (noabrt)
                {
                    if (ProgramSettings.isServer) { noabrt = false; continue; }
                    if(!NClient.MainClient.Connected)
                    {
                        var notificationManager = new NotificationManager();

                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Server connection",
                            Message = "Connection to server lost!",
                            Type = NotificationType.Warning
                        });
                        noabrt = false;
                    }
                    else
                    {
                        List<byte> ToSendDE = new List<byte>();
                        ToSendDE.Add(2);
                        NClient.MainClient.Send(ToSendDE.ToArray(), pswrd);
                    }
                    System.Threading.Thread.Sleep(10000);
                }
            });
            workerThread.IsBackground = true;
            workerThread.Start();
            InformationA.ServersCountInt++;

            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }
    }
}
