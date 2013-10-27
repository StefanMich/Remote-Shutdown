using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace Shutdown
{
    public static class TimeInterpreter
    {
        public static int InterpretTime(string time)
        {
            DateTime now = DateTime.Now;
            DateTime then = DateTime.Parse(time);

            TimeSpan difference = then - now;
            if (difference.Seconds < 0)
                difference = difference + new TimeSpan(24, 0, 0);

            int total = (int)difference.TotalMilliseconds;

            return total;
        }


        public static int InterpretCountdown(string time)
        {
            int total = -1;


            total = int.Parse(time) * 60 * 1000;



            return total;
        }

        public static void addTime(int add, TimeType TimeType, TextBox TimeFormat)
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

        public static string TimeRemaining(int interval, int tick)
        {
            if (interval / 1000 > tick)
                return ((interval / 60000) - (tick / 60) - 1).ToString() + ":" + (59 - (tick % 60)).ToString();
            else return "00:00";
        }
    }
}