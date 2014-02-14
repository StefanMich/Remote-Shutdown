using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClientApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Properties.Settings.Default.IP == "")
            { IPform ipf = new IPform("");
            if (ipf.ShowDialog() == DialogResult.Cancel)
                Application.Exit();
            }
            Application.Run(new Form1());
        }
    }
}
