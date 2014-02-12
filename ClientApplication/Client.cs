using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Shutdown;
using System.Windows.Forms;
using System.Collections.Concurrent;


namespace ClientApplication
{
    class Client
    {
        TcpClient tcp;
        public BlockingCollection<string> status;
        public Client()
        {
            //ip som parameter
            tcp = new TcpClient();
            status = new BlockingCollection<string>();
        }

        public bool Connect()
        {
            try
            {
                Console.WriteLine("Connecting");
                tcp.Connect("127.0.0.1", 8001);
                Console.WriteLine("Connected");
            }
            catch (SocketException e)
            {
                MessageBox.Show( "Could not connect to specified host (" + e.SocketErrorCode + ")\nErrormessage: " + e.Message);

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.StackTrace);
                return false;
            }
            return true;
        }

        public void Transmit(ShutdownMessage msg)
        {
            Stream stream = tcp.GetStream();

            Console.WriteLine("Transmitting");

            byte[] b = msg.GetMessage();
            stream.Write(b, 0, b.Length);

            b = new byte[100];
            stream.Read(b, 0, 100);
            Console.WriteLine(b.GetString());
            status.Add(b.GetString());
        }
    }
}
