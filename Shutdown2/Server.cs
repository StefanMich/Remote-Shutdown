using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Shutdown
{
    public class Server
    {
        private volatile bool shouldStop = false;
        TcpListener listen;

        public void RequestStop()
        {
            listen.Stop();
            shouldStop = true;
        }

        public void ServerLoop()
        {
            try
            {
                IPAddress idAP = IPAddress.Parse("192.168.10.118");
                idAP = IPAddress.Parse("127.0.0.1");
                listen = new TcpListener(IPAddress.Any, 8001);
                listen.Start();
                
                Console.WriteLine("Server running at port 8001");
                Console.WriteLine("The local endpoint is " + listen.LocalEndpoint);
                Console.WriteLine("Waiting for connection");

                Socket s = listen.AcceptSocket();

                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
                while (!shouldStop)
                {
                    byte[] b = new byte[100];
                    int k = s.Receive(b);

                    Console.Write("Received.." + ShutdownMessage.ReadMessage(b, k));
                    MainForm.SetText(ShutdownMessage.ReadMessage(b,k).ToString());
                }

                Console.ReadKey();
                s.Close();
                listen.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.StackTrace);
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
