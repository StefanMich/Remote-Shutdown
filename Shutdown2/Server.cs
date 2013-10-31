using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Shutdown
{
    public class Server
    {
        private volatile bool shouldStop = false;
        TcpListener listen;
        public BlockingCollection<ShutdownMessage> shutdownCollection;

        public Server()
        {
            shutdownCollection = new BlockingCollection<ShutdownMessage>();

        }

        public void RequestStop()
        {
            listen.Stop();
            shouldStop = true;
        }

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

                        Console.Write("Received.." + ShutdownMessage.ReadMessage(b, k));

                        byte[] msg = "Succesfully initiated shutdown".GetBytes();
                        s.Send(msg);

                        shutdownCollection.Add(ShutdownMessage.ReadMessage(b, k));
                    }
                }
            }
            catch (SocketException)
            {
                //breaks the loop
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
