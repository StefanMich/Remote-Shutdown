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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainInterface1
            // 
            this.mainInterface1.Location = new System.Drawing.Point(-2, 1);
            this.mainInterface1.Name = "mainInterface1";
            this.mainInterface1.Size = new System.Drawing.Size(226, 137);
            this.mainInterface1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::ClientApplication.Properties.Resources.application_x_desktop;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(204, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 21);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 135);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainInterface1);
            this.Name = "Form1";
            this.Text = "Remote Shutdown";
            this.ResumeLayout(false);

        }

        #endregion

        private Shutdown.MainInterface mainInterface1;
        private System.Windows.Forms.Button button1;
    }
}

