using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static CelestialDES.MainWindow;
using Microsoft.Win32;
using Notifications.Wpf;
using CelestialDES.Pages;
using System.Windows.Controls;
using dnlib.DotNet.MD;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для Scripting.xaml
    /// </summary>
    public partial class Scripting : Window
    {
        public int ConnectionID { get; set; }
        private int selectedmethod = -1;
        public Scripting()
        {
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            InitializeComponent();
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

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = false;
            OFD.Filter = "CSharp (.cs)|*.cs";
            if (OFD.ShowDialog() == true)
            {
                string FileString = OFD.FileName;
                using (StreamReader streamReader = new StreamReader(FileString, Encoding.UTF8))
                {
                    ScriptText.Text = streamReader.ReadToEnd();
                }
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (selectedmethod == -1)
            {
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Scripting",
                    Message = "Select script type.",
                    Type = NotificationType.Error
                });
                return;
            }

            if (selectedmethod == 1) //batch
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)39);
                ToSend.AddRange(Encoding.UTF8.GetBytes(ScriptText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }

            if (selectedmethod == 2) // basic
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)38);
                ToSend.AddRange(Encoding.UTF8.GetBytes(ScriptText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }

            if (selectedmethod == 3) // C#
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)49);
                ToSend.AddRange(Encoding.UTF8.GetBytes(ScriptText.Text));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0) //sel ind already updated
            {
                ScriptText.Text = "echo hello world\npause";
                selectedmethod = 1;
            }
            if (cmbSelect.SelectedIndex == 1) //sel ind already updated
            {
                ScriptText.Text = "MsgBox(\"Hello World\")";
                selectedmethod = 2;
            }
            if (cmbSelect.SelectedIndex == 2) //sel ind already updated
            {
                ScriptText.Text = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Windows.Forms;\r\n\r\nnamespace Celestial\r\n{\r\n    public class Scripting\r\n    {\r\n        public void Main()\r\n        {\r\n            MessageBox.Show(\"Hello World!\");\r\n        }\r\n    }\r\n}";
                selectedmethod = 3;
            }
        }

        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0) //sel ind already updated
            {
                selectedmethod = 1;
            }
            if (cmbSelect.SelectedIndex == 1) //sel ind already updated
            {
                selectedmethod = 2;
            }
            if (cmbSelect.SelectedIndex == 2) //sel ind already updated
            {
                selectedmethod = 3;
            }
        }


        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSharp file (.cs)|*.cs";
            sfd.FileName = "FinalDad.cs";
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;
                if(File.Exists(filename))
                {
                    File.Delete(filename);
                }

                File.WriteAllText(filename, ScriptText.Text);
            }
        }
    }
}
