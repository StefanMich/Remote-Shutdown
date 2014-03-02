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
        }

        void serverStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Connection to server was lost. Closing the application");
            this.Close();
        }

        void serverStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            ServerStatus s;
            while (client.Connected)
            {
                
                    if ((s = client.ReceiveStatus()) != ServerStatus.ConnectionClosed)
                    {
                        setStatusLabel(s);

                        mainInterface1.toggleActive(s);
                    }
            }
        }
        delegate void setStatusText(ServerStatus s);

        private void setStatusLabel(ServerStatus s)
        {
            if (mainInterface1.statusLabel.InvokeRequired)
            {
                setStatusText setStatus = (status) => mainInterface1.statusLabel.Text = ServerStatusResponseLabel(status);
                mainInterface1.statusLabel.Invoke(setStatus, s);
            }
            else mainInterface1.statusLabel.Text = ServerStatusResponseLabel(s);
        }

        void Execute_Click(object sender, EventArgs e)
        {
            milliseconds = mainInterface1.CalculateTime();

            if (client.Connected)
            {
                if (mainInterface1.Execute.Text == MainInterface.cancellabel)
                    client.Transmit(new ShutdownMessage(0, ShutdownType.Cancel));
                else
                    client.Transmit(new ShutdownMessage(milliseconds, mainInterface1.ShutdownType));
            }
            else MessageBox.Show("Not connected to a server. Please input a valid IP in the settings dialog");
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
