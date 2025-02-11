using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.FTP
{
    internal struct FTPAccountFormat
    {
        internal readonly string Url;
        internal readonly string Username;
        internal readonly string Password;

        internal FTPAccountFormat(string url, string username, string password)
        {
            Url = url;
            Username = username;
            Password = password;
        }
    }
}
