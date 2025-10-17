namespace CEKA_APP.UserControls.KesimTakip
{
    partial class ctlKesimYonetimi
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnUygulamayiKapat; // Designer'a eklendi
        private System.Windows.Forms.Button btnOturumuKapat;      // Designer'a eklendi

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
            this.btnUygulamayiKapat = new System.Windows.Forms.Button();
            this.btnOturumuKapat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddPage
            // 
            this.buttonAddPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddPage.BackColor = System.Drawing.Color.White;
            this.buttonAddPage.FlatAppearance.BorderSize = 0;
            this.buttonAddPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.buttonAddPage.Location = new System.Drawing.Point(1450, 5);
            this.buttonAddPage.Name = "buttonAddPage";
            this.buttonAddPage.Size = new System.Drawing.Size(130, 30);
            this.buttonAddPage.TabIndex = 2;
            this.buttonAddPage.Text = "Yeni Sayfa (+)";
            this.buttonAddPage.UseVisualStyleBackColor = false;
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
            // btnUygulamayiKapat
            // 
            this.btnUygulamayiKapat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUygulamayiKapat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnUygulamayiKapat.FlatAppearance.BorderSize = 0;
            this.btnUygulamayiKapat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUygulamayiKapat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnUygulamayiKapat.ForeColor = System.Drawing.Color.White;
            this.btnUygulamayiKapat.Location = new System.Drawing.Point(1801, 0);
            this.btnUygulamayiKapat.Name = "btnUygulamayiKapat";
            this.btnUygulamayiKapat.Size = new System.Drawing.Size(35, 40);
            this.btnUygulamayiKapat.TabIndex = 4;
            this.btnUygulamayiKapat.Text = "X";
            this.btnUygulamayiKapat.UseVisualStyleBackColor = false;
            this.btnUygulamayiKapat.Click += new System.EventHandler(this.btnUygulamayiKapat_Click);
            // 
            // btnOturumuKapat
            // 
            this.btnOturumuKapat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOturumuKapat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnOturumuKapat.FlatAppearance.BorderSize = 0;
            this.btnOturumuKapat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOturumuKapat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOturumuKapat.ForeColor = System.Drawing.Color.White;
            this.btnOturumuKapat.Location = new System.Drawing.Point(1585, 5);
            this.btnOturumuKapat.Name = "btnOturumuKapat";
            this.btnOturumuKapat.Size = new System.Drawing.Size(210, 30);
            this.btnOturumuKapat.TabIndex = 3;
            this.btnOturumuKapat.Text = "OTURUMU KAPAT";
            this.btnOturumuKapat.UseVisualStyleBackColor = false;
            this.btnOturumuKapat.Click += new System.EventHandler(this.btnOturumuKapat_Click);
            // 
            // ctlKesimYonetimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUygulamayiKapat);
            this.Controls.Add(this.btnOturumuKapat);
            this.Controls.Add(this.buttonAddPage);
            this.Controls.Add(this.tabControlMain);
            this.Name = "ctlKesimYonetimi";
            this.Size = new System.Drawing.Size(1836, 1031);
            this.Load += new System.EventHandler(this.ctlKesimYonetimi_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAddPage;
        private System.Windows.Forms.TabControl tabControlMain;
    }
}