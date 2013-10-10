using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace Shutdown
{

    public partial class MainForm : Form
    {
        Thread server;
        Server worker = new Server();
        Queue<ShutdownMessage> shutdownQueue;
        ShutdownAction shutdown = new ShutdownAction();
        
        int milliseconds = -1;
        
        Minimize Mini = new Minimize();
        System.Windows.Forms.Timer visualTimer = new System.Windows.Forms.Timer();
        int tick = 1;
        static string text = "";



        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            visualTimer.Tick += new System.EventHandler(visual_Tick);
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(mainInterface1.AddTen, "Right click to toggle between 1 and 10 minutes. Hold ctrl to subtract");
            shutdown.Timer_Elapsed += new EventHandler(Timer_Elapsed);
            server = new Thread(worker.ServerLoop);
            server.Start();

            mainInterface1.Execute.Click +=Execute_Click;


        }

        void Timer_Elapsed(object sender, EventArgs e)
        {
            //this.Close();
        }

        public static void SetText(string s)
        {
            text = s;
        }

        

        private void timer_Tick(object sender, EventArgs e)
        {
            //label1.Text = (shutdown.MyTimer.Interval / 60000).ToString();
        }


        private void Execute_Click(object sender, EventArgs e)
        {

            try
            {
                if (mainInterface1.TimeType == TimeType.Countdown)
                    milliseconds = TimeInterpreter.InterpretCountdown(mainInterface1.TimeFormat.Text);
                else if (mainInterface1.TimeType == TimeType.Time)
                    milliseconds = TimeInterpreter.InterpretTime(mainInterface1.TimeFormat.Text);
                else
                    MessageBox.Show("Choose timer type");

                if (milliseconds >= 0)
                {
                    shutdown.ShutdownActionExe(milliseconds, mainInterface1.ShutdownType);
                    Mini.ToTray(notifyIcon1, this, shutdown.MyTimer, mainInterface1.ShutdownType);
                    tick = 1;
                    visualTimer.Interval = 1000;
                    visualTimer.Enabled = true;
                }
                else MessageBox.Show("Time must be positive");

            }
            catch (FormatException exception)
            {
                MessageBox.Show("Incorrect time format");
                Console.WriteLine(exception);
            }

        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }
        private void visual_Tick(object sender, EventArgs e)
        {

            mainInterface1.label1.Text = TimeInterpreter.TimeRemaining(shutdown.MyTimer, tick);
            tick++;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "Time remaining to " + mainInterface1.ShutdownType.ToString() + ": " +TimeInterpreter .TimeRemaining(shutdown.MyTimer, tick);
            notifyIcon1.ShowBalloonTip(500);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (shutdown.MyTimer.Enabled == true)
            {
                DialogResult dialog = MessageBox.Show(this, "This will cancel the shutdown. Do you want to continue?", "Cancel shutdown?", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
                base.OnFormClosing(e);
            }

            worker.RequestStop();

        }

        

    }
}

