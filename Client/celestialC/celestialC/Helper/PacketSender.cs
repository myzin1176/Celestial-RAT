using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class PacketSender
    {
        public static bool Send(byte[] data)
        {
            if (Settings.isServer)
            {
                try
                {
                    List<byte> ToSend = new List<byte>();
                    ToSend.Add((byte)3);
                    ToSend.AddRange(data);
                    if (Networking.Networking.MainClient.Send(ToSend.ToArray())) return true;
                }
                catch { }
            }
            else
                if (Networking.Networking.MainClient.Send(data)) return true;
            return false;
        }
    }
}
