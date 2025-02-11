using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace celestialC.Helper
{
    public static class PluginManager
    {
            static byte[] GetEmbeddedResource(string resourceName)
            {
                var self = Assembly.GetExecutingAssembly();

                using (var rs = self.GetManifestResourceStream(resourceName))
                {
                    if (rs != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            rs.CopyTo(ms);
                            return ms.ToArray();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public static byte[] Decompress(byte[] data)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(memoryStream);
                    }

                    return memoryStream.ToArray();
                }
            }

            public static void ExecuteFromMemory(byte[] bytecode)
            {
                try
                {
                    var assembly1 = Assembly.Load(bytecode);
                    var type = assembly1.GetType("CelestialLib.mainClass");
                    var obj1 = Activator.CreateInstance(type);
                    var method = type.GetMethod("entrypoint");
                    Task.Factory.StartNew(() => { method.Invoke(obj1, null); });
                }
                catch { }
            }

            public static void InitAll()
            {
                int c = 0;
                foreach (var resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
                {
                    if (resourceName.ToLower().Contains("properties.resources.resources") || resourceName.ToLower().Contains("celestialc")) continue;
                    if(resourceName.ToLower().Contains("lib")) c++;
                }

            if (c == 0) return;
                for (int i = 0; i < c; i++)
                {
                        var fileBytes = Decompress(GetEmbeddedResource("lib" + i));
                        var assembly1 = Assembly.Load(fileBytes);
                        var type = assembly1.GetType("CelestialLib.mainClass");
                        var obj1 = Activator.CreateInstance(type);
                        var method = type.GetMethod("entrypoint");
                        Task.Factory.StartNew(() => { method.Invoke(obj1, new object[] { }); });
                }
            }
    }
}
