using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace Shutdown
{
    class Minimize
    {
        delegate void hideform();

        public void ToTray(NotifyIcon notify, Form form, System.Windows.Forms.Timer MyTimer, ShutdownType ShutdownType)
        {
            notify.Icon =  new System.Drawing.Icon("favicon.ico");
            notify.BalloonTipText =  ShutdownType + " in " + (MyTimer.Interval / 60000).ToString() + " minutes.";
            notify.BalloonTipTitle = "Shutdown";
            notify.Text = "Shutdown";
            notify.Visible = true;

            //notify.ShowBalloonTip(500, "Shutdown", "Shutting down in x", System.Windows.Forms.ToolTipIcon.Info);
            notify.ShowBalloonTip(500);

            if (form.InvokeRequired)
            {
                hideform hidedelegate = () => form.Hide();
                form.Invoke(hidedelegate);
            }
            else form.Hide();
        }
    }
}
