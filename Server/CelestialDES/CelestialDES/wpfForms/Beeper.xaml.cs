using CelestialDES.Helper;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для Beeper.xaml
    /// </summary>
    public partial class Beeper : Window
    {
        public int ConnectionID { get; set; }
        public Beeper()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<byte> ToSend = new List<byte>();
            ToSend.Add((int)56);
            ToSend.AddRange(Encoding.UTF8.GetBytes(TextBox_F.Text + ";" + TextBox_T.Text));
           // Networking.Server.MainServer.Send(ConnectionID, ToSend.ToArray());
           PacketSender.Send(ConnectionID, ToSend.ToArray());
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
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

        private void TextBox_F_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(TextBox_F.Text, "[^0-9]"))
            {
                TextBox_F.Text = TextBox_F.Text.Remove(TextBox_F.Text.Length - 1);
            }
        }

        private void TextBox_T_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(TextBox_T.Text, "[^0-9]"))
            {
                TextBox_T.Text = TextBox_T.Text.Remove(TextBox_T.Text.Length - 1);
            }
        }
    }
}
