using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shutdown
{
    public partial class MainInterface : UserControl
    {

        public TimeType TimeType;
        public ShutdownType ShutdownType;
        private bool shutdownActive;

        public bool ShutdownActive
        {
            get { return shutdownActive; }
        }


        public MainInterface()
        {
            InitializeComponent();
            AddTenTooltip.ShowAlways = true;
            AddTenTooltip.SetToolTip(AddTen, "Right click to toggle between 1 and 10 minutes. Hold ctrl to subtract");

        }

        public int CalculateTime()
        {
            int milliseconds = 0;
            try
            {
                if (TimeType == TimeType.Countdown)
                    milliseconds = TimeInterpreter.InterpretCountdown(TimeFormat.Text);
                else if (TimeType == TimeType.Time)
                    milliseconds = TimeInterpreter.InterpretTime(TimeFormat.Text);
                else
                    MessageBox.Show("Choose timer type");

            }
            catch (FormatException exception)
            {
                MessageBox.Show("Incorrect time format");
                Console.WriteLine(exception);
            }

            return milliseconds;
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
                    if (AddTen.Text == "+10" && Control.ModifierKeys == Keys.Control) TimeInterpreter.addTime(-10, TimeType, TimeFormat);
                    else if (AddTen.Text == "+10") TimeInterpreter.addTime(10, TimeType, TimeFormat);
                    else if (AddTen.Text == "+1" && Control.ModifierKeys == Keys.Control) TimeInterpreter.addTime(-1, TimeType, TimeFormat);
                    else if (AddTen.Text == "+1") TimeInterpreter.addTime(1, TimeType, TimeFormat);
                }
                catch (FormatException exception)
                {
                    MessageBox.Show("Incorrect time format");
                    Console.WriteLine(exception);
                }
            }
        }

        private void Sleep_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Hibernate;
        }

        private void Shutdown_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Shutdown;
        }

        private void Reboot_CheckedChanged(object sender, EventArgs e)
        {
            ShutdownType = ShutdownType.Reboot;
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

        public void toggleActive()
        {
            timetypePanel.Enabled = !timetypePanel.Enabled;
            shutdowntypePanel.Enabled = !shutdowntypePanel.Enabled;

            if (Execute.Text == "Execute")
            {
                shutdownActive = true;
                Execute.Text = "Cancel";
            }
            else
            {
                Execute.Text = "Execute";
                shutdownActive = false;
            }
        }
    }

}
