using CelestialDES.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CelestialDES.Networking.Telepathy
{
    public class Client : Common
    {
        private readonly ManualResetEvent sendPending = new ManualResetEvent(false);

        private readonly SafeQueue<byte[]> sendQueue = new SafeQueue<byte[]>();
        private volatile bool _Connecting;
        public TcpClient client;
        private Thread receiveThread;
        private Thread sendThread;

        public bool Connected => client != null &&
                                 client.Client != null &&
                                 client.Client.Connected;

        public bool Connecting => _Connecting;

        private void ReceiveThreadFunction(string ip, int port)
        {
            try
            {
                client.Connect(ip, port);
                _Connecting = false;

                sendThread = new Thread(() => { SendLoop(0, client, sendQueue, sendPending); });
                sendThread.IsBackground = true;
                sendThread.Start();

                ReceiveLoop(0, client, receiveQueue, MaxMessageSize);
            }
            catch
            {
                receiveQueue.Enqueue(new Message(0, EventType.Disconnected, null));
            }

            sendThread?.Interrupt();

            _Connecting = false;

            client.Close();
        }

        public void Connect(string ip, int port)
        {
            if (Connecting || Connected) return;

            _Connecting = true;

            client = new TcpClient();
            client.NoDelay = NoDelay;
            client.SendTimeout = SendTimeout;

            receiveQueue = new ConcurrentQueue<Message>();
            sendQueue.Clear();

            receiveThread = new Thread(() => { ReceiveThreadFunction(ip, port); });
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        public void Disconnect()
        {
            if (Connecting || Connected)
            {
                client.Close();

                receiveThread?.Join();

                sendQueue.Clear();

                client = null;
            }
        }

        public bool Send(byte[] dataz, string pass)
        {
            byte[] data = Encryption.Encrypt(dataz, pass);
           // byte[] data = dataz;
            if (Connected)
            {
                if (data.Length <= MaxMessageSize)
                {
                    sendQueue.Enqueue(data);
                    sendPending.Set();
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
