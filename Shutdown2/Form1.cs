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
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Win32;

namespace Shutdown
{

    public partial class MainForm : Form
    {
        Thread serverThread;
        Server server = new Server();
        ShutdownAction shutdown = new ShutdownAction();
        delegate void formCallback(string text);

        int milliseconds = -1;

        Minimize Mini;
        System.Windows.Forms.Timer visualTimer = new System.Windows.Forms.Timer();
        int tick = 1;

        //http://csharp.net-informations.com/communications/csharp-chat-server-programming.htm
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

             Mini = new Minimize(notifyIcon1,this,shutdown.ShutdownTimer);

            visualTimer.Tick += visual_Tick;
            notifyIcon1.Click += notifyIcon1_Click;

            serverThread = new Thread(server.ServerLoop);
            serverThread.Start();

            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            consumer.RunWorkerAsync();

            mainInterface1.Execute.Click += Execute_Click;
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
                CancelShutdown();
                }



        private void Execute_Click(object sender, EventArgs e)
        {
            milliseconds = mainInterface1.CalculateTime();

            if (mainInterface1.Execute.Text == MainInterface.cancellabel)
                CancelShutdown();
            else
                ExecuteShutdown(milliseconds, mainInterface1.ShutdownType);
        }

        public void ExecuteShutdown(ShutdownMessage s)
        {
            ExecuteShutdown(s.Interval, s.Type);
        }

        private void ExecuteShutdown(int milliseconds, ShutdownType st)
        {

            if (st != ShutdownType.Cancel)
            {
                if (milliseconds >= 0)
                {
                    shutdown.ShutdownActionExe(milliseconds, st);
                    Mini.ToTray( st);
                    tick = 1;

                    visualTimer.Interval = 1000;

                    visualTimer.Enabled = true;
                }
                else MessageBox.Show("Time must be positive");
                server.ReportClients(ServerStatus.ShutdownInitiated);
                mainInterface1.toggleActive(ServerStatus.ShutdownInitiated);
            }
            else
            {
                CancelShutdown();
            }

        }

        private void CancelShutdown()
        {
            CancelTimers();
            Mini.DeactivateTray();
            server.ReportClients(ServerStatus.ShutdownCancelled);
            mainInterface1.toggleActive(ServerStatus.ShutdownCancelled);
        }

        /// <summary>
        /// Cancels a ongoing shutdown
        /// </summary>
        private void CancelTimers()
        {
            shutdown.ShutdownCancel();
            visualTimer.Enabled = false;
        }

        

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }
        private void visual_Tick(object sender, EventArgs e)
        {

            mainInterface1.statusLabel.Text = TimeInterpreter.TimeRemaining(shutdown.ShutdownTimer.Interval, tick);
            tick++;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            ShutdownType s = mainInterface1.ShutdownType;
            notifyIcon1.BalloonTipText = "Time remaining to " + s.ToString() + ": " + TimeInterpreter.TimeRemaining(shutdown.ShutdownTimer.Interval, tick);
            notifyIcon1.ShowBalloonTip(500);
        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (shutdown.ShutdownTimer.Enabled == true)
            {
                DialogResult dialog = MessageBox.Show(this, "This will cancel the shutdown. Do you want to continue?", "Cancel shutdown?", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
                base.OnFormClosing(e);
            }

            server.RequestStop();

        }

        private void consumer_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                ShutdownMessage s;
                if (server.shutdownCollection.TryTake(out s))
                    consumer.ReportProgress(0, s);
                Thread.Sleep(1000);
            }
        }

        private void consumer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ExecuteShutdown(e.UserState as ShutdownMessage);
        }
    }
}
