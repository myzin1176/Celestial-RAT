using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialDES.Helper
{
    public static class SettingsC
    {
        public static void SaveSetting(string name, string value)
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Celestial");
                key.SetValue(Encryption.Base64Encode(name), Encryption.Base64Encode(value));
                key.Close();
            }
            catch { }
        }

        public static string readSetting(string settingname)
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Celestial", false);
                return Encryption.Base64Decode(key.GetValue(Encryption.Base64Encode(settingname)).ToString());
            }
            catch { return "null"; }
        }
    }
}