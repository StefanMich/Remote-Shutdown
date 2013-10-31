namespace ClientApplication
{
    partial class Form1
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
            this.mainInterface1 = new Shutdown.MainInterface();
            this.SuspendLayout();
            // 
            // mainInterface1
            // 
            this.mainInterface1.Location = new System.Drawing.Point(12, 12);
            this.mainInterface1.Name = "mainInterface1";
            this.mainInterface1.Size = new System.Drawing.Size(226, 137);
            this.mainInterface1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 150);
            this.Controls.Add(this.mainInterface1);
            this.Name = "Form1";
            this.Text = "Remote Shutdown";
            this.ResumeLayout(false);

        }

        #endregion

        private Shutdown.MainInterface mainInterface1;
    }
}

