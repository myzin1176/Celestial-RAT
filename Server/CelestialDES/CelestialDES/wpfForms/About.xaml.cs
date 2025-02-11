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
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
        }
        private bool animation_is_running = false;

        private void Grid_MouseMove_1(object sender, RoutedEventArgs e)
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
        private void btnDark_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void btnBlue_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Themes/BlueTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void BtnWhite_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Themes/WhiteTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Label_Reg.Content = InformationA.username;
            Label_Ver.Content = InformationA.version;
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
