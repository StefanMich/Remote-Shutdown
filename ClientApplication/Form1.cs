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

        public Form1()
        {
            InitializeComponent();
            milliseconds = 0;
            client = new Client();
            ready = client.Connect();

            mainInterface1.Execute.Click += Execute_Click;
            Task.Factory.StartNew(Consumer);
        }

        void Execute_Click(object sender, EventArgs e)
        {
            milliseconds = mainInterface1.CalculateTime();

            if (mainInterface1.ShutdownActive == true)
                client.Transmit(new ShutdownMessage(0, ShutdownType.Cancel));
            else client.Transmit(new ShutdownMessage(milliseconds, mainInterface1.ShutdownType));
            mainInterface1.toggleActive();
        }


        delegate void setStatusText();
        private void Consumer()
        {
            while (true)
            {
                string s;
                if (client.status.TryTake(out s))
                {
                    if (mainInterface1.statusLabel.InvokeRequired)
                    {
                        setStatusText setStatus = () => mainInterface1.statusLabel.Text = s;
                        mainInterface1.statusLabel.Invoke(setStatus);
                    }
                    else mainInterface1.statusLabel.Text = s;
                }
            }
        }
    }
}
