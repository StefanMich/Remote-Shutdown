using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Shutdown
{
    class TimeInterpreter
    {
        public int InterpretTime(string time)
        {
            DateTime now = DateTime.Now;
            DateTime then = DateTime.Parse(time);

            TimeSpan difference = then - now;
            if (difference.Seconds < 0)
                difference = difference + new TimeSpan(24, 0, 0);

            int total = (int)difference.TotalMilliseconds;

            return total;
        }


        public int InterpretCountdown(string time)
        {
            int total = -1;


            total = int.Parse(time) * 60 * 1000;



            return total;
        }

        public void addTime(int add, TimeType TimeType, TextBox TimeFormat)
        {
            if (TimeType == TimeType.Countdown)
                TimeFormat.Text = (int.Parse(TimeFormat.Text) + add).ToString();
            else
            {
                double NewTime = ((InterpretTime(TimeFormat.Text) / 60000) + add);
                TimeSpan timeTo = new TimeSpan((int)(NewTime / 60), (int)(NewTime % 60) + 1, 0); // This +1 is a confusing. When removed, the time seems to sometimes add 8 or 9 instead of 10. This might not be a fix!
                DateTime NewDateTime = DateTime.Now + timeTo;
                TimeFormat.Text = (NewDateTime).ToShortTimeString();
            }
        }

        public string TimeRemaining(System.Windows.Forms.Timer MyTimer, int tick)
        {
            return ((MyTimer.Interval / 60000) - (tick / 60) - 1).ToString() + ":" + (59 - (tick % 60)).ToString();
        }
    }
}