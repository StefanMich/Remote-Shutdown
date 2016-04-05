using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;

namespace Shutdown
{
    /// <summary>
    /// Enum with the shutdown types, shutdown, reboot and hibernate
    /// </summary>
    public enum ShutdownType : byte
    {
        /// <summary>
        /// Shutdown mode
        /// </summary>
        Shutdown = 0,
        /// <summary>
        /// Reboot mde
        /// </summary>
        Reboot = 1,
        /// <summary>
        /// Hibernate mode
        /// </summary>
        Hibernate = 2,
        /// <summary>
        /// Cancel a shutdown
        /// </summary>
        Cancel = 3

    }

    public enum ServerStatus : byte
    {
        /// <summary>
        /// Shutdown is initiated on the server
        /// </summary>
        ShutdownInitiated,
        /// <summary>
        /// Shutdown was cancelled on the server
        /// </summary>
        ShutdownCancelled,
        /// <summary>
        /// Connection to server was closed
        /// </summary>
        ConnectionClosed
    }

    /// <summary>
    /// Time timer types, countdown and time (ex. 12:00)
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// Countdown
        /// </summary>
        Countdown,
        /// <summary>
        /// Time (ex 12:00)
        /// </summary>
        Time
    }


    class ShutdownAction : IDisposable
    {

        /// <summary>
        /// The shutdown timer
        /// </summary>
        public System.Windows.Forms.Timer ShutdownTimer = new System.Windows.Forms.Timer();

        ShutdownType MyShutdownType;
        string modifier;

        /// <summary>
        /// Executes the requested shutdown
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="ShutdownType">Type of the shutdown.</param>
        public void ShutdownActionExe(int interval, ShutdownType ShutdownType)
        {
            MyShutdownType = ShutdownType;

            if (interval > 0)
            {
                ShutdownTimer.Interval = interval;
                ShutdownTimer.Tick += new EventHandler(MyTimer_Elapsed);

            }
            else if (interval == 0)
            {
                ShutdownTimer.Interval = 1;
                ShutdownTimer.Tick += new EventHandler(MyTimer_Elapsed);
            }
            else ShutdownTimer.Interval = 1; // interval cannot be 0, so it is set to 1 millisecond instead

            ShutdownTimer.Start();

            if (ShutdownType == ShutdownType.Shutdown) modifier = "s -f -t 0";
            else if (ShutdownType == ShutdownType.Reboot) modifier = "r -f -t 0";
            else if (ShutdownType == ShutdownType.Hibernate) modifier = "h";


        }

        public void ShutdownCancel()
        {
            ShutdownTimer.Enabled = false;
        }

        /// <summary>
        /// Occurs when the timer is elapsed
        /// </summary>
        public event EventHandler Timer_Elapsed;
        protected virtual void OnTimer_Elapsed(EventArgs e)
        {
            if (Timer_Elapsed != null)
                Timer_Elapsed(this, e);
        }


        void MyTimer_Elapsed(object sender, EventArgs e)
        {
            ShutdownTimer.Enabled = false;

            string arguments = "-" + modifier;
#if !DEBUG
            Process.Start("shutdown", arguments);
#endif
            OnTimer_Elapsed(EventArgs.Empty);

        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ShutdownTimer.Dispose();
        }
    }
}
