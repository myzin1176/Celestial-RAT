using CelestialDES.Helper;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static CelestialDES.MainWindow;

namespace CelestialDES.Pages.BuilderPages
{
    /// <summary>
    /// Логика взаимодействия для ScriptManager.xaml
    /// </summary>
    public partial class ScriptManager : Window
    {
        public List<Script> scripts = new List<Script>();
        public List<string> scriptswaiting = new List<string>();
        int CurrentSelected = 0;
        Final FinalObj;
        public ScriptManager(Final fn)
        {
            InitializeComponent();
            FinalObj = fn;
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\scripts\\", "*.dll", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                string author = "N/A";
                string name = "N/A";
                string description = "N/A";
                using (ModuleDefMD asmDef = ModuleDefMD.Load(file))
                {
                    try
                    {
                        foreach (TypeDef type in asmDef.Types)
                        {
                            if (type.Name == "mainClass")
                                foreach (FieldDef field in type.Fields)
                                {
                                    if(field.ToString().Contains("mainClass::name"))
                                        name = field.Constant.Value.ToString();
                                    if (field.ToString().Contains("mainClass::author"))
                                        author = field.Constant.Value.ToString();
                                    if (field.ToString().Contains("mainClass::description"))
                                        description = field.Constant.Value.ToString();
                                }
                        }
                    }
                    catch (Exception ex){ MessageBox.Show(ex.Message); }
                    asmDef.Dispose();
                }
                Script script = new Script
                {
                    Path = file,
                    NameDescription = name + Environment.NewLine + description,
                    Author = author,
                    IsLoaded = "Unloaded"
                };
                scripts.Add(script);
            }
            scriptlist.ItemsSource = "";
            scriptlist.ItemsSource = scripts;
        }

        public struct Script
        {
            public string Path { get; set; }
            public string NameDescription { get; set; }
            public string Author { get; set; }
            public string IsLoaded { get; set; }
        }

        private void Load_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (scriptlist.SelectedItems.Count != 1) return;

            foreach (Script scr in scripts)
            {
                if(scr.Path.ToLower() == scripts[scriptlist.SelectedIndex].Path.ToLower())
                {
                    if (scr.IsLoaded == "Loaded") break;
                    ListViewItem item = scriptlist.ItemContainerGenerator.ContainerFromIndex(CurrentSelected) as ListViewItem;
                    if (item != null)
                    {
                        item.Foreground = Brushes.Green;
                    }
                    Script sc = scripts[scriptlist.SelectedIndex];
                    sc.IsLoaded = "Loaded";
                    scripts[scriptlist.SelectedIndex] = sc;
                    scriptswaiting.Add(System.IO.Path.GetFileNameWithoutExtension(scr.Path));
                    break;
                }
            }
        }

        private void Unload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (scriptlist.SelectedItems.Count != 1) return;

            foreach (Script scr in scripts)
            {
                if (scr.Path.ToLower() == scripts[scriptlist.SelectedIndex].Path.ToLower())
                {
                    if (scr.IsLoaded == "Unloaded") break;
                    var item = scriptlist.ItemContainerGenerator.ContainerFromIndex(CurrentSelected) as ListViewItem;
                    if (item != null)
                    {
                        item.Foreground = Brushes.Red;
                    }
                    Script sc = scripts[scriptlist.SelectedIndex];
                    sc.IsLoaded = "Unloaded";
                    scripts[scriptlist.SelectedIndex] = sc;
                    scriptswaiting.Remove(System.IO.Path.GetFileNameWithoutExtension(scr.Path));
                    break;
                }
            }
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (scriptlist.SelectedItems.Count != 1) return;

            foreach (Script scr in scripts)
            {
                if (scr.Path.ToLower() == scripts[scriptlist.SelectedIndex].Path.ToLower())
                {
                    var item = scriptlist.ItemContainerGenerator.ContainerFromIndex(CurrentSelected) as ListViewItem;
                    if (item != null)
                    {
                        item.Foreground = Brushes.Red;
                    }
                    scripts.Remove(scr);
                    System.IO.File.Delete(scr.Path);
                    scriptlist.ItemsSource = "";
                    scriptlist.ItemsSource = scripts;
                    scriptswaiting.Clear();
                    for (int i = 0; i < scripts.Count; i++)
                    {
                        ListViewItem items = scriptlist.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                        if (items != null)
                        {
                            items.Foreground = Brushes.Red;
                        }
                    }
                    break;
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

        bool needtobrush = true;
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
            if(needtobrush)
            {
                for (int i = 0; i < scripts.Count; i++)
                {
                    ListViewItem item = scriptlist.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                    if (item != null)
                    {
                        item.Foreground = Brushes.Red;
                    }
                }
                needtobrush = false;
            }
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

        private void scriptlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CurrentSelected = scriptlist.SelectedIndex;
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // FinalObj.loadedscripts = scriptswaiting;
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
