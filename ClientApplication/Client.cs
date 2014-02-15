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
        Stream stream;
        bool ready;
        public BlockingCollection<ServerStatus> status;
        public Client()
        {
            tcp = new TcpClient();
            status = new BlockingCollection<ServerStatus>();
            ready = Connect();
        }

        public bool Ready { get { return ready; } }

        public bool Connect()
        {
            if (tcp.Connected)
                tcp = new TcpClient();
            try
            {
                Console.WriteLine("Connecting");
                tcp.Connect(Properties.Settings.Default.IP, 8001);
                Console.WriteLine("Connected");
            }
            catch (SocketException e)
            {
                MessageBox.Show("Could not connect to specified host (" + e.SocketErrorCode + ")\nErrormessage: " + e.Message);

                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.StackTrace);
                return false;
            }
            stream = tcp.GetStream();
            return true;
        }

        public ServerStatus ReceiveStatus()
        {
            byte[] b = new byte[1];
            ServerStatus s;
            try
            {
                stream.Read(b, 0, 1);
                s = (ServerStatus)b[0];
            }
            catch (IOException e)
            { 
                //connection was closed
                s = ServerStatus.ConnectionClosed;
            }

            return s;
        }

        public void Transmit(ShutdownMessage msg)
        {
            Console.WriteLine("Transmitting");
            byte[] b = msg.GetMessage();
            stream.Write(b, 0, b.Length);
        }
    }
}
