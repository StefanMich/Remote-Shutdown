using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace Shutdown
{
    public class Server
    {
        private volatile bool shouldStop = false;
        TcpListener listen;
        public BlockingCollection<ShutdownMessage> shutdownCollection;
        private List<Socket> ConnectedClients;

        public Server()
        {
            shutdownCollection = new BlockingCollection<ShutdownMessage>();
            ConnectedClients = new List<Socket>();
        }

        /// <summary>
        /// Stops the serverloop 
        /// </summary>
        public void RequestStop()
        {
            listen.Stop();
            shouldStop = true;
        }

        /// <summary>
        /// The main loop waiting for incoming connections
        /// </summary>
        public void ServerLoop()
        {
            try
            {
                IPAddress idAP = IPAddress.Parse("127.0.0.1");
                idAP = IPAddress.Parse("127.0.0.1");
                listen = new TcpListener(IPAddress.Any, 8001);
                listen.Start();

                Console.WriteLine("Server running at port 8001");
                Console.WriteLine("The local endpoint is " + listen.LocalEndpoint);
                Console.WriteLine("Waiting for connection");

                Socket s;

                while (true)
                {
                    s = listen.AcceptSocket();
                    Console.WriteLine("Succesfully connected to " + s.RemoteEndPoint);
                    ConnectedClients.Add(s);
                    Task.Factory.StartNew(() => ClientConnectedLoop(s));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
            }
        }

        private void ClientConnectedLoop(Socket s)
        {
            try
            {
                while (!shouldStop)
                {
                    byte[] b = new byte[100];
                    if (s.Connected)
                    {
                        int k = s.Receive(b);
                        ShutdownMessage message = ShutdownMessage.ReadMessage(b, k);
                        

                        shutdownCollection.Add(message);
                    }
                }
            }
            catch (SocketException)
            {
                ConnectedClients.Remove(s);
            }
        }

        private void MessageClient(Socket s, ServerStatus status)
        {
            if (s.Connected)
            {
                using (Stream stream = new NetworkStream(s))
                {
                    stream.WriteByte((byte)status);
                } 
            }
        }

        /// <summary>
        /// Reports the specified status to all connected clients
        /// </summary>
        /// <param name="status"></param>
        public void ReportClients(ServerStatus status)
        {
            foreach (var s in ConnectedClients)
            {
                MessageClient(s, status);
            }
        }

        private static string byteToString(byte[] b, int size)
        {
            string s = "";
            for (int i = 0; i < size; i++)
            {
                s = s + b[i].ToString();
            }

            return s;
        }



    }
}
