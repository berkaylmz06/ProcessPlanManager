namespace CEKA_APP
{
    partial class frmKullaniciGirisi
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
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGiris = new System.Windows.Forms.Button();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBeniHatirla = new System.Windows.Forms.CheckBox();
            this.panelBosluk = new System.Windows.Forms.Panel();
            this.panelCizgi = new System.Windows.Forms.Panel();
            this.panelPictureBox = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPictureBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSifre
            // 
            this.txtSifre.BackColor = System.Drawing.Color.White;
            this.txtSifre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSifre.Location = new System.Drawing.Point(179, 320);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(216, 30);
            this.txtSifre.TabIndex = 8;
            this.txtSifre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSifre_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(44, 322);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Şifre:";
            // 
            // btnGiris
            // 
            this.btnGiris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGiris.ForeColor = System.Drawing.Color.White;
            this.btnGiris.Location = new System.Drawing.Point(102, 416);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(250, 40);
            this.btnGiris.TabIndex = 10;
            this.btnGiris.Text = "Giriş";
            this.btnGiris.UseVisualStyleBackColor = true;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.BackColor = System.Drawing.Color.White;
            this.txtKullaniciAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKullaniciAdi.Location = new System.Drawing.Point(179, 263);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(216, 30);
            this.txtKullaniciAdi.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(44, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Kullanıcı  Adı:";
            // 
            // chkBeniHatirla
            // 
            this.chkBeniHatirla.AutoSize = true;
            this.chkBeniHatirla.ForeColor = System.Drawing.Color.White;
            this.chkBeniHatirla.Location = new System.Drawing.Point(48, 368);
            this.chkBeniHatirla.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBeniHatirla.Name = "chkBeniHatirla";
            this.chkBeniHatirla.Size = new System.Drawing.Size(120, 27);
            this.chkBeniHatirla.TabIndex = 12;
            this.chkBeniHatirla.Text = "Beni Hatırla";
            this.chkBeniHatirla.UseVisualStyleBackColor = true;
            // 
            // panelBosluk
            // 
            this.panelBosluk.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBosluk.Location = new System.Drawing.Point(0, 0);
            this.panelBosluk.Name = "panelBosluk";
            this.panelBosluk.Size = new System.Drawing.Size(445, 46);
            this.panelBosluk.TabIndex = 14;
            // 
            // panelCizgi
            // 
            this.panelCizgi.BackColor = System.Drawing.Color.White;
            this.panelCizgi.Location = new System.Drawing.Point(0, 224);
            this.panelCizgi.Name = "panelCizgi";
            this.panelCizgi.Size = new System.Drawing.Size(515, 10);
            this.panelCizgi.TabIndex = 15;
            // 
            // panelPictureBox
            // 
            this.panelPictureBox.Controls.Add(this.pictureBox1);
            this.panelPictureBox.Controls.Add(this.panel2);
            this.panelPictureBox.Controls.Add(this.panel1);
            this.panelPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPictureBox.Location = new System.Drawing.Point(0, 46);
            this.panelPictureBox.Name = "panelPictureBox";
            this.panelPictureBox.Size = new System.Drawing.Size(445, 172);
            this.panelPictureBox.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(50, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(345, 172);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(395, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 172);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(50, 172);
            this.panel1.TabIndex = 0;
            // 
            // frmKullaniciGirisi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(445, 493);
            this.Controls.Add(this.panelCizgi);
            this.Controls.Add(this.panelPictureBox);
            this.Controls.Add(this.panelBosluk);
            this.Controls.Add(this.chkBeniHatirla);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmKullaniciGirisi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CEKA";
            this.Load += new System.EventHandler(this.frmKullaniciGirisi_Load);
            this.Resize += new System.EventHandler(this.frmKullaniciGirisi_Resize);
            this.panelPictureBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.TextBox txtKullaniciAdi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkBeniHatirla;
        private System.Windows.Forms.Panel panelBosluk;
        private System.Windows.Forms.Panel panelCizgi;
        private System.Windows.Forms.Panel panelPictureBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}