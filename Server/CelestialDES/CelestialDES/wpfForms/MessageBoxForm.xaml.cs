﻿using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для MessageBoxForm.xaml
    /// </summary>
    public partial class MessageBoxForm : Window
    {
        public int ConnectionID { get; set; }
        public MessageBoxForm()
        {
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            InitializeComponent();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboIcon.SelectedIndex != -1 && ComboButton.SelectedIndex != -2)
            {
                /*        Networking.Server.MainServer.Send(Connectionid,
                        Encoding.UTF8.GetBytes("MsgBox<{<" + metroTextBox2.Text + ">[" + metroTextBox1.Text + "]{" +
                                    metroComboBox2.SelectedIndex + "}" + "/" + metroComboBox1.SelectedIndex + @"\}>"));*/
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)0);
                ToSend.AddRange(Encoding.UTF8.GetBytes(TextBox_C.Text.Replace(";", " ")));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(TextBox_T.Text.Replace(";", " ")));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(ComboButton.SelectedIndex.ToString()));
                ToSend.Add((byte)';');
                ToSend.AddRange(Encoding.UTF8.GetBytes(ComboIcon.SelectedIndex.ToString()));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                DoubleAnimation tabanim = new DoubleAnimation();
                tabanim.EasingFunction = new PowerEase();
                tabanim.From = 1;
                tabanim.To = 0;
                tabanim.Duration = TimeSpan.FromMilliseconds(125);
                tabanim.Completed += anim_Completed;
                this.BeginAnimation(Window.OpacityProperty, tabanim);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Some fields are empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
