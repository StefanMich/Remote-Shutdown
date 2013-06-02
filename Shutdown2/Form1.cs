using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace Shutdown
{

    public partial class MainForm : Form
    {

        TimeInterpreter Interpreter = new TimeInterpreter();
        ShutdownAction shutdown = new ShutdownAction();
        ShutdownType ShutdownType;
        int milliseconds = -1;
        TimeType TimeType;
        Minimize Mini = new Minimize();
        System.Windows.Forms.Timer visualTimer = new System.Windows.Forms.Timer();
        int tick = 1;



        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            visualTimer.Tick += new System.EventHandler(visual_Tick);
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(AddTen, "Right click to toggle between 1 and 10 minutes. Hold ctrl to subtract");
            shutdown.Timer_Elapsed += new EventHandler(Timer_Elapsed);
            
        }

        void Timer_Elapsed(object sender, EventArgs e)
        {
            //this.Close();
        }

        private void Sleep_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Hibernate;
        }


        //timer radio buttons
        private void Countdown_Checked(object sender, EventArgs e)
        {
            TimeType = TimeType.Countdown;
        }

        private void radioButton2_Checked_1(object sender, EventArgs e)
        {
            TimeType = TimeType.Time;
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            label1.Text = (shutdown.MyTimer.Interval / 60000).ToString();
        }

        private void Execute_Click(object sender, EventArgs e)
        {

            try
            {
                if (TimeType == TimeType.Countdown)
                    milliseconds = Interpreter.InterpretCountdown(TimeFormat.Text);
                else if (TimeType == TimeType.Time)
                    milliseconds = Interpreter.InterpretTime(TimeFormat.Text);
                else
                    MessageBox.Show("Choose timer type");

                if (milliseconds >= 0)
                {
                    shutdown.ShutdownActionExe(milliseconds, ShutdownType);
                    Mini.ToTray(notifyIcon1, this, shutdown.MyTimer, ShutdownType);
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

        private void Shutdown_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Shutdown;
        }

        private void Reboot_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Reboot;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void visual_Tick(object sender, EventArgs e)
        {
            
            label1.Text = Interpreter.TimeRemaining(shutdown.MyTimer, tick);
            tick++;
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



        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "Time remaining to " + ShutdownType.ToString() + ": " + Interpreter.TimeRemaining(shutdown.MyTimer, tick);
            notifyIcon1.ShowBalloonTip(500);
        }

        private void AddTen_MouseClick(object sender, MouseEventArgs e)
        {
       
        }

        private void AddTen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (AddTen.Text == "+10") AddTen.Text = "+1";
                else AddTen.Text = "+10";
            else if (e.Button == MouseButtons.Left)
            {
                try
                {
                    if (AddTen.Text == "+10" && Control.ModifierKeys == Keys.Control) Interpreter.addTime(-10, TimeType, TimeFormat);
                    else if (AddTen.Text == "+10") Interpreter.addTime(10, TimeType, TimeFormat);
                    else if (AddTen.Text == "+1" && Control.ModifierKeys == Keys.Control) Interpreter.addTime(-1, TimeType, TimeFormat);
                    else if (AddTen.Text == "+1") Interpreter.addTime(1, TimeType, TimeFormat);
                }
                catch (FormatException exception)
                {
                    MessageBox.Show("Incorrect time format");
                    Console.WriteLine(exception);
                }
            }
        }

    }
}

