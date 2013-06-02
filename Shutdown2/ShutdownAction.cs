﻿using System;
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
    public enum ShutdownType {
        /// <summary>
        /// Shutdown mode
        /// </summary>
        Shutdown,
        /// <summary>
        /// Reboot mde
        /// </summary>
        Reboot,
        /// <summary>
        /// Hibernate mode
        /// </summary>
        Hibernate 
    }

    /// <summary>
    /// Time timer types, countdown and time (ex. 12:00)
    /// </summary>
    public enum TimeType {
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
        public System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
        ShutdownType MyShutdownType;
        public Stopwatch Stopwatch = new Stopwatch();
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
                MyTimer.Interval = interval;
                MyTimer.Tick += new EventHandler(MyTimer_Elapsed);

            }
            else if (interval == 0)
            {
                MyTimer.Interval = 1;
                MyTimer.Tick += new EventHandler(MyTimer_Elapsed);
            }
            else MyTimer.Interval = 1; // interval cannot be 0, so it is set to 1 millisecond instead

            MyTimer.Start();
            Stopwatch.Start();


            if (ShutdownType == ShutdownType.Shutdown) modifier = "s -f -t 0";
            else if (ShutdownType == ShutdownType.Reboot) modifier = "r -f -t 0";
            else if (ShutdownType == ShutdownType.Hibernate) modifier = "h";


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
            MyTimer.Enabled = false;

            string arguments = "-" + modifier;
            Process.Start("shutdown", arguments);
            OnTimer_Elapsed(EventArgs.Empty);

        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            MyTimer.Dispose();
        }
    }
}