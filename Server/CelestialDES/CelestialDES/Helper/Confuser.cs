using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CelestialDES.Helper
{
    internal class Confuser
    {
        public static string Obfuscate(string file)
        {
            string configpath = Path.GetTempPath() + "configconfuser.crproj";
            string configconfuser = CelestialDES.Properties.Resources.config;
            string confuserdirectory = Path.GetTempPath() + "Confuser";
            string basedir = new FileInfo(file).Directory.ToString();

            configconfuser = configconfuser.Replace("%path%", basedir + "\\Obfuscated")
                .Replace("%basedir%", basedir)
                .Replace("%stub%", file);

            File.WriteAllText(configpath, configconfuser);
            File.WriteAllBytes(Path.GetTempPath() + "confuser.zip", CelestialDES.Properties.Resources.ConfuserEx);

            File.WriteAllBytes(Path.GetTempPath() + "AForge.Video.DirectShow.dll", File.ReadAllBytes(@"data\libs\AForge.Video.DirectShow.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "AForge.Video.dll", File.ReadAllBytes(@"data\libs\AForge.Video.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "DotNetZip.dll", File.ReadAllBytes(@"data\libs\DotNetZip.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "SharpDX.Direct3D9.dll", File.ReadAllBytes(@"data\libs\SharpDX.Direct3D9.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "SharpDX.Direct3D11.dll", File.ReadAllBytes(@"data\libs\SharpDX.Direct3D11.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "SharpDX.dll", File.ReadAllBytes(@"data\libs\SharpDX.dll"));
            File.WriteAllBytes(Path.GetTempPath() + "SharpDX.DXGI.dll", File.ReadAllBytes(@"data\libs\SharpDX.DXGI.dll"));


            if (Directory.Exists(confuserdirectory))
            {
                Directory.Delete(confuserdirectory, true);
            }

            Directory.CreateDirectory(confuserdirectory);
            ZipFile.ExtractToDirectory(Path.GetTempPath() + "confuser.zip", confuserdirectory);

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = confuserdirectory + "\\Confuser.CLI.exe";
            process.UseShellExecute = true;
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.Arguments = "-n " + configpath;

            Process p = Process.Start(process);
            p.WaitForExit();

            File.Delete(Path.GetTempPath() + "confuser.zip");
            File.Delete(Path.GetTempPath() + "configconfuser.crproj");
            Directory.Delete(confuserdirectory, true);

            File.Delete(Path.GetTempPath() + "AForge.Video.DirectShow.dll");
            File.Delete(Path.GetTempPath() + "AForge.Video.dll");
            File.Delete(Path.GetTempPath() + "DotNetZip.dll");
            File.Delete(Path.GetTempPath() + "SharpDX.Direct3D9.dll");
            File.Delete(Path.GetTempPath() + "SharpDX.Direct3D11.dll");
            File.Delete(Path.GetTempPath() + "SharpDX.dll");
            File.Delete(Path.GetTempPath() + "SharpDX.DXGI.dll");

            return basedir + "\\Obfuscated\\" + new FileInfo(file).Name;
        }
    }
}
