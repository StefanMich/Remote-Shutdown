using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Shutdown;


namespace ClientApplication
{
    public partial class Form1 : Form
    {
        Client c;
        bool ready;
        int milliseconds;

        public Form1()
        {
            InitializeComponent();
            milliseconds = 0;
            c = new Client();
            ready = c.Connect();

            mainInterface1.Execute.Click += Execute_Click;
        }

        void Execute_Click(object sender, EventArgs e)
        {
            milliseconds = mainInterface1.CalculateTime();

            c.Transmit(new ShutdownMessage(milliseconds, mainInterface1.ShutdownType));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ready)
                c.Transmit(new ShutdownMessage(1, Shutdown.ShutdownType.Shutdown));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ready)
                c.Transmit(new ShutdownMessage(900, Shutdown.ShutdownType.Reboot));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ready)
                c.Transmit(new ShutdownMessage(45006, Shutdown.ShutdownType.Hibernate));
        }


    }
}
