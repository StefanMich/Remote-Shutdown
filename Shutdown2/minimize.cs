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
        NotifyIcon notify;
        Form form;
        System.Windows.Forms.Timer timer;

        delegate void hideform();

        public Minimize(NotifyIcon notify, Form form, System.Windows.Forms.Timer timer)
        {
            this.notify = notify;
            this.form = form;
            this.timer = timer;
        }

        public void ToTray(ShutdownType ShutdownType)
        {
            notify.Icon = Properties.Resources.favicon;
            notify.BalloonTipText =  ShutdownType + " in " + (timer.Interval / 60000).ToString() + " minutes.";
            notify.BalloonTipTitle = "Shutdown";
            notify.Text = "Shutdown";
            notify.Visible = true;

            notify.ShowBalloonTip(500);

            if (form.InvokeRequired)
            {
                hideform hidedelegate = () => form.Hide();
                form.Invoke(hidedelegate);
            }
            else form.Hide();
        }

        public void DeactivateTray()
        {
            notify.Visible = false;

            if (form.InvokeRequired)
            {
                hideform showdelegate = () => form.Show();
                form.Invoke(showdelegate);
            }
            else form.Show();
        }
    }
}
