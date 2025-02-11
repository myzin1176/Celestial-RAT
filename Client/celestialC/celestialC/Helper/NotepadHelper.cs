using celestialC.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class NotepadHelper
    {
        public static void ShowMessage(string message = null, string title = null)
        {
            if (File.Exists(System.IO.Path.GetTempPath() + "log.txt"))
            {
                File.Delete(System.IO.Path.GetTempPath() + "log.txt");
            }
  
                using (StreamWriter sw = File.CreateText(System.IO.Path.GetTempPath() + "log.txt"))
                {
                    sw.WriteLine(message);
                }

                Process.Start(System.IO.Path.GetTempPath() + "log.txt");

        }
    }
}
