using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CelestialDES.MainWindow;

namespace CelestialDES.Helper
{
    public static class PacketSender
    {
        public static bool Send(int ConnectionID, byte[] data)
        {
            if(ProgramSettings.isServer)
                if (Networking.Server.MainServer.Send(ConnectionID, data)) return true;

            if (!ProgramSettings.isServer)
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((byte)1);
                ToSend.AddRange(Encoding.UTF8.GetBytes(ConnectionID.ToString().Length.ToString()));
                ToSend.AddRange(Encoding.UTF8.GetBytes(ConnectionID.ToString()));
                ToSend.AddRange(data);
                if (Networking.NClient.MainClient.Send(ToSend.ToArray(), ProgramSettings.password)) return true;
            }
            return false;
        }
    }
}
