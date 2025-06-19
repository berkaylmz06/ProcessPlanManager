namespace CEKA_APP.UsrControl
{
    partial class ctlNavigasyon
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureNavigationForward = new System.Windows.Forms.PictureBox();
            this.pictureNavigationBack = new System.Windows.Forms.PictureBox();
            this.panelNavigation = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNavigationForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNavigationBack)).BeginInit();
            this.panelNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureNavigationForward
            // 
            this.pictureNavigationForward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureNavigationForward.Location = new System.Drawing.Point(0, 0);
            this.pictureNavigationForward.Name = "pictureNavigationForward";
            this.pictureNavigationForward.Size = new System.Drawing.Size(100, 50);
            this.pictureNavigationForward.TabIndex = 0;
            this.pictureNavigationForward.TabStop = false;
            // 
            // pictureNavigationBack
            // 
            this.pictureNavigationBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureNavigationBack.Location = new System.Drawing.Point(0, 0);
            this.pictureNavigationBack.Name = "pictureNavigationBack";
            this.pictureNavigationBack.Size = new System.Drawing.Size(50, 50);
            this.pictureNavigationBack.TabIndex = 1;
            this.pictureNavigationBack.TabStop = false;
            // 
            // panelNavigation
            // 
            this.panelNavigation.Controls.Add(this.pictureNavigationBack);
            this.panelNavigation.Controls.Add(this.pictureNavigationForward);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavigation.Location = new System.Drawing.Point(0, 0);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(100, 50);
            this.panelNavigation.TabIndex = 2;
            // 
            // ctlNavigasyon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelNavigation);
            this.Name = "ctlNavigasyon";
            this.Size = new System.Drawing.Size(1683, 50);
            ((System.ComponentModel.ISupportInitialize)(this.pictureNavigationForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureNavigationBack)).EndInit();
            this.panelNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureNavigationForward;
        private System.Windows.Forms.PictureBox pictureNavigationBack;
        private System.Windows.Forms.Panel panelNavigation;
    }
}
