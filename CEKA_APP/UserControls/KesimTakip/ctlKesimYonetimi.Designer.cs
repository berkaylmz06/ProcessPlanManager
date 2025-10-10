namespace CEKA_APP.UserControls.KesimTakip
{
    partial class ctlKesimYonetimi
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
            this.buttonAddPage = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // buttonAddPage
            // 
            this.buttonAddPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddPage.FlatAppearance.BorderSize = 0;
            this.buttonAddPage.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonAddPage.Location = new System.Drawing.Point(1706, 3);
            this.buttonAddPage.Name = "buttonAddPage";
            this.buttonAddPage.Size = new System.Drawing.Size(130, 28);
            this.buttonAddPage.TabIndex = 1;
            this.buttonAddPage.Text = "Yeni Sayfa (+)";
            this.buttonAddPage.BackColor = System.Drawing.Color.White;
            this.buttonAddPage.UseVisualStyleBackColor = false;
            this.buttonAddPage.BringToFront();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1836, 1031);
            this.tabControlMain.TabIndex = 1;
            // 
            // ctlKesimYonetimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonAddPage);
            this.Controls.Add(this.tabControlMain);
            this.Name = "ctlKesimYonetimi";
            this.Size = new System.Drawing.Size(1836, 1031);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.Button buttonAddPage;
    }
}