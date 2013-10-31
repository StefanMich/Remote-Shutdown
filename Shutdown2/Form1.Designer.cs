namespace Shutdown
{
    /// <summary>
    /// The main form
    /// </summary>
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                shutdown.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ttTime = new System.Windows.Forms.ToolTip(this.components);
            this.consumer = new System.ComponentModel.BackgroundWorker();
            this.mainInterface1 = new Shutdown.MainInterface();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // consumer
            // 
            this.consumer.WorkerReportsProgress = true;
            this.consumer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.consumer_DoWork);
            this.consumer.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.consumer_ProgressChanged);
            // 
            // mainInterface1
            // 
            this.mainInterface1.Location = new System.Drawing.Point(2, 3);
            this.mainInterface1.Name = "mainInterface1";
            this.mainInterface1.Size = new System.Drawing.Size(226, 137);
            this.mainInterface1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 143);
            this.Controls.Add(this.mainInterface1);
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip ttTime;
        private MainInterface mainInterface1;
        private System.ComponentModel.BackgroundWorker consumer;
    }
}

