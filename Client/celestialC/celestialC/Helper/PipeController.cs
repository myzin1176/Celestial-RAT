using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Helper
{
    public static class PipeController
    {
        public static bool sendcommand(int code, byte[] command = null, string pipename = "$CELcnotroler")
        {
            NamedPipeClientStream pipe = new NamedPipeClientStream(".", pipename, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);
            try
            {
                pipe.Connect(2500);
                using (BinaryWriter writer = new BinaryWriter(pipe))
                {
                    writer.Write(code);
                    if (command?.Length > 0) writer.Write(command);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
