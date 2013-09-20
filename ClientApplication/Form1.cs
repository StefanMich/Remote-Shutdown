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
        public Form1()
        {
            InitializeComponent();
            c = new Client();
            ready = c.Connect();
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
