using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Shutdown;
using System.Windows.Forms;

namespace ClientApplication
{
    class Client
    {
        TcpClient tcp;
        public Client()
        {
            //ip som parameter
            tcp = new TcpClient();
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
            string str = Console.ReadLine();
            Stream stream = tcp.GetStream();

            Console.WriteLine("Transmitting");

            byte[] b = msg.GetMessage();
            stream.Write(b, 0, b.Length);
        }
    }
}
