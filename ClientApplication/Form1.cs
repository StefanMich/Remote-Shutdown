using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Shutdown;
using System.Threading.Tasks;


namespace ClientApplication
{
    public partial class Form1 : Form
    {
        Client client;
        bool ready;
        int milliseconds;
        BackgroundWorker serverStatus;

        public Form1()
        {
            InitializeComponent();
            milliseconds = 0;
            client = new Client();


            mainInterface1.Execute.Click += Execute_Click;

            serverStatus = new BackgroundWorker();
            serverStatus.DoWork += serverStatus_DoWork;
            serverStatus.RunWorkerCompleted += serverStatus_RunWorkerCompleted;
            serverStatus.RunWorkerAsync();
            Task.Factory.StartNew(ServerStatusConsumer);
        }

        void serverStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        void serverStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            ServerStatus s;
            while ((s = client.ReceiveStatus()) != ServerStatus.ConnectionClosed)
            {
                {
                    if (mainInterface1.statusLabel.InvokeRequired)
                    {
                        setStatusText setStatus = () => mainInterface1.statusLabel.Text = ServerStatusResponseLabel(s);
                        mainInterface1.statusLabel.Invoke(setStatus);
                    }
                    else mainInterface1.statusLabel.Text = ServerStatusResponseLabel(s);

                    mainInterface1.toggleActive();

                }
            }
            MessageBox.Show("Connection to server was closed.");

        }

        void Execute_Click(object sender, EventArgs e)
        {
            milliseconds = mainInterface1.CalculateTime();

            if (mainInterface1.ShutdownActive == true)
                client.Transmit(new ShutdownMessage(0, ShutdownType.Cancel));
            else
                client.Transmit(new ShutdownMessage(milliseconds, mainInterface1.ShutdownType));
        }


        delegate void setStatusText();

        private void ServerStatusConsumer()
        {
            // serverstatus dowork
        }

        private string ServerStatusResponseLabel(ServerStatus s)
        {
            string response;
            switch (s)
            {
                case ServerStatus.ShutdownInitiated:
                    response = "Shutdown in progress";
                    break;
                case ServerStatus.ShutdownCancelled:
                    response = "Ready";
                    break;
                default:
                    response = null;
                    break;
            }
            return response;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPform ipf = new IPform(Properties.Settings.Default.IP);
            if (ipf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                client.Connect();
        }
    }
}
