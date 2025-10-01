namespace CEKA_APP.Forms
{
    partial class frmStatuBilgi
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
            this.listStatuBilgi = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listStatuBilgi
            // 
            this.listStatuBilgi.BackColor = System.Drawing.SystemColors.Window;
            this.listStatuBilgi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listStatuBilgi.FormattingEnabled = true;
            this.listStatuBilgi.ItemHeight = 16;
            this.listStatuBilgi.Location = new System.Drawing.Point(0, 0);
            this.listStatuBilgi.Name = "listStatuBilgi";
            this.listStatuBilgi.Size = new System.Drawing.Size(439, 461);
            this.listStatuBilgi.TabIndex = 0;
            // 
            // frmStatuBilgi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(439, 461);
            this.Controls.Add(this.listStatuBilgi);
            this.Name = "frmStatuBilgi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statü Bilgi";
            this.Load += new System.EventHandler(this.frmStatuBilgi_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listStatuBilgi;
    }
}