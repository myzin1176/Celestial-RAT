using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CelestialDES.Helper
{

    public static class NotepadHelper
    {
        public static void ShowMessage(string message = null, string title = null)
        {
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }

            // Create a new file     
            using (StreamWriter sw = File.CreateText("log.txt"))
            {
                sw.WriteLine(message);
            }

            Process.Start("log.txt");
            /*  Process notepad = Process.Start(new ProcessStartInfo("notepad.exe"));
              if (notepad != null)
              {
                  notepad.WaitForInputIdle();

                  if (!string.IsNullOrEmpty(title))
                      NativeMethods.SetWindowText(notepad.MainWindowHandle, title);

                  if (!string.IsNullOrEmpty(message))
                  {
                      var child = NativeMethods.FindWindowEx(notepad.MainWindowHandle, new IntPtr(0), "Edit", null);
                      NativeMethods.SendMessageW(child, 0x000C, IntPtr.Zero, message);
                  }
              }*/
        }
    }
}
