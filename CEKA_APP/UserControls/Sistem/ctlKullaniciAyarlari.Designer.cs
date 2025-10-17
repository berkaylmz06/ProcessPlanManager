namespace CEKA_APP.UsrControl
{
    partial class ctlKullaniciAyarlari
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
            this.btnYeniKullanici = new System.Windows.Forms.Button();
            this.btnRolAta = new System.Windows.Forms.Button();
            this.panelDisContainer = new System.Windows.Forms.Panel();
            this.panelList = new System.Windows.Forms.Panel();
            this.lstKullanicilar = new System.Windows.Forms.ListBox();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelDetay = new System.Windows.Forms.Panel();
            this.ctlBaslik2 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKullaniciSil = new System.Windows.Forms.Button();
            this.panelRolAtaCizgi = new System.Windows.Forms.Panel();
            this.cbKullaniciRol = new System.Windows.Forms.ComboBox();
            this.lblKullaniciRolu = new System.Windows.Forms.Label();
            this.btnGüncelle = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.lblMail = new System.Windows.Forms.Label();
            this.lblSifre = new System.Windows.Forms.Label();
            this.lblKullaniciAdi = new System.Windows.Forms.Label();
            this.lblAdSoyad = new System.Windows.Forms.Label();
            this.txtAdSoyad = new System.Windows.Forms.TextBox();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelDisContainer.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelDetay.SuspendLayout();
            this.panelRolAtaCizgi.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnYeniKullanici
            // 
            this.btnYeniKullanici.Location = new System.Drawing.Point(8, 10);
            this.btnYeniKullanici.Name = "btnYeniKullanici";
            this.btnYeniKullanici.Size = new System.Drawing.Size(207, 47);
            this.btnYeniKullanici.TabIndex = 0;
            this.btnYeniKullanici.Text = "+ Yeni Kullanıcı";
            this.btnYeniKullanici.UseVisualStyleBackColor = true;
            this.btnYeniKullanici.Click += new System.EventHandler(this.btnYeniKullanici_Click);
            // 
            // btnRolAta
            // 
            this.btnRolAta.Location = new System.Drawing.Point(224, 10);
            this.btnRolAta.Name = "btnRolAta";
            this.btnRolAta.Size = new System.Drawing.Size(207, 47);
            this.btnRolAta.TabIndex = 1;
            this.btnRolAta.Text = "Rol Atama\r\n";
            this.btnRolAta.UseVisualStyleBackColor = true;
            this.btnRolAta.Click += new System.EventHandler(this.btnRolAta_Click);
            // 
            // panelDisContainer
            // 
            this.panelDisContainer.BackColor = System.Drawing.SystemColors.Control;
            this.panelDisContainer.Controls.Add(this.panelList);
            this.panelDisContainer.Controls.Add(this.panelSpacer1);
            this.panelDisContainer.Controls.Add(this.panelHeader);
            this.panelDisContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDisContainer.Location = new System.Drawing.Point(0, 50);
            this.panelDisContainer.Name = "panelDisContainer";
            this.panelDisContainer.Padding = new System.Windows.Forms.Padding(10);
            this.panelDisContainer.Size = new System.Drawing.Size(460, 969);
            this.panelDisContainer.TabIndex = 139;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.lstKullanicilar);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(10, 87);
            this.panelList.Margin = new System.Windows.Forms.Padding(10);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(440, 872);
            this.panelList.TabIndex = 139;
            // 
            // lstKullanicilar
            // 
            this.lstKullanicilar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstKullanicilar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstKullanicilar.FormattingEnabled = true;
            this.lstKullanicilar.ItemHeight = 16;
            this.lstKullanicilar.Location = new System.Drawing.Point(0, 0);
            this.lstKullanicilar.Name = "lstKullanicilar";
            this.lstKullanicilar.Size = new System.Drawing.Size(440, 872);
            this.lstKullanicilar.TabIndex = 128;
            this.lstKullanicilar.SelectedIndexChanged += new System.EventHandler(this.lstKullanicilar_SelectedIndexChanged);
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer1.Location = new System.Drawing.Point(10, 77);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(440, 10);
            this.panelSpacer1.TabIndex = 140;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelHeader.Controls.Add(this.btnYeniKullanici);
            this.panelHeader.Controls.Add(this.btnRolAta);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(10, 10);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(5);
            this.panelHeader.Size = new System.Drawing.Size(440, 67);
            this.panelHeader.TabIndex = 138;
            // 
            // panelDetay
            // 
            this.panelDetay.Controls.Add(this.ctlBaslik2);
            this.panelDetay.Controls.Add(this.panel1);
            this.panelDetay.Controls.Add(this.btnKullaniciSil);
            this.panelDetay.Controls.Add(this.panelRolAtaCizgi);
            this.panelDetay.Controls.Add(this.btnGüncelle);
            this.panelDetay.Controls.Add(this.txtEmail);
            this.panelDetay.Controls.Add(this.txtSifre);
            this.panelDetay.Controls.Add(this.txtKullaniciAdi);
            this.panelDetay.Controls.Add(this.lblMail);
            this.panelDetay.Controls.Add(this.lblSifre);
            this.panelDetay.Controls.Add(this.lblKullaniciAdi);
            this.panelDetay.Controls.Add(this.lblAdSoyad);
            this.panelDetay.Controls.Add(this.txtAdSoyad);
            this.panelDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetay.Location = new System.Drawing.Point(460, 50);
            this.panelDetay.Name = "panelDetay";
            this.panelDetay.Size = new System.Drawing.Size(1247, 969);
            this.panelDetay.TabIndex = 140;
            // 
            // ctlBaslik2
            // 
            this.ctlBaslik2.Baslik = "Başlık";
            this.ctlBaslik2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik2.Location = new System.Drawing.Point(0, 10);
            this.ctlBaslik2.Name = "ctlBaslik2";
            this.ctlBaslik2.Size = new System.Drawing.Size(1247, 50);
            this.ctlBaslik2.TabIndex = 142;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1247, 10);
            this.panel1.TabIndex = 141;
            // 
            // btnKullaniciSil
            // 
            this.btnKullaniciSil.Location = new System.Drawing.Point(33, 380);
            this.btnKullaniciSil.Name = "btnKullaniciSil";
            this.btnKullaniciSil.Size = new System.Drawing.Size(207, 47);
            this.btnKullaniciSil.TabIndex = 5;
            this.btnKullaniciSil.Text = "Kullanıcıyı Sil";
            this.btnKullaniciSil.UseVisualStyleBackColor = true;
            this.btnKullaniciSil.Click += new System.EventHandler(this.btnKullaniciSil_Click);
            // 
            // panelRolAtaCizgi
            // 
            this.panelRolAtaCizgi.Controls.Add(this.cbKullaniciRol);
            this.panelRolAtaCizgi.Controls.Add(this.lblKullaniciRolu);
            this.panelRolAtaCizgi.Location = new System.Drawing.Point(57, 261);
            this.panelRolAtaCizgi.Name = "panelRolAtaCizgi";
            this.panelRolAtaCizgi.Size = new System.Drawing.Size(410, 38);
            this.panelRolAtaCizgi.TabIndex = 12;
            // 
            // cbKullaniciRol
            // 
            this.cbKullaniciRol.FormattingEnabled = true;
            this.cbKullaniciRol.Items.AddRange(new object[] {
            "Yönetici",
            "Destek",
            "İş Hazırlama",
            "Muhasebe",
            "Operatör",
            "Kullanıcı",
            "Ressam",
            "Erp",
            "Proje Mühendisi 1",
            "Proje Mühendisi 2"});
            this.cbKullaniciRol.Location = new System.Drawing.Point(121, 7);
            this.cbKullaniciRol.Name = "cbKullaniciRol";
            this.cbKullaniciRol.Size = new System.Drawing.Size(274, 24);
            this.cbKullaniciRol.TabIndex = 2;
            // 
            // lblKullaniciRolu
            // 
            this.lblKullaniciRolu.AutoSize = true;
            this.lblKullaniciRolu.Location = new System.Drawing.Point(6, 11);
            this.lblKullaniciRolu.Name = "lblKullaniciRolu";
            this.lblKullaniciRolu.Size = new System.Drawing.Size(87, 16);
            this.lblKullaniciRolu.TabIndex = 4;
            this.lblKullaniciRolu.Text = "Kullanıcı Rolü";
            // 
            // btnGüncelle
            // 
            this.btnGüncelle.Location = new System.Drawing.Point(270, 380);
            this.btnGüncelle.Name = "btnGüncelle";
            this.btnGüncelle.Size = new System.Drawing.Size(207, 47);
            this.btnGüncelle.TabIndex = 4;
            this.btnGüncelle.Text = "Güncelle";
            this.btnGüncelle.UseVisualStyleBackColor = true;
            this.btnGüncelle.Click += new System.EventHandler(this.btnGüncelle_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(179, 321);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(274, 22);
            this.txtEmail.TabIndex = 3;
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(179, 214);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.Size = new System.Drawing.Size(274, 22);
            this.txtSifre.TabIndex = 1;
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Enabled = false;
            this.txtKullaniciAdi.Location = new System.Drawing.Point(179, 155);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(274, 22);
            this.txtKullaniciAdi.TabIndex = 100;
            this.txtKullaniciAdi.TabStop = false;
            // 
            // lblMail
            // 
            this.lblMail.AutoSize = true;
            this.lblMail.Location = new System.Drawing.Point(63, 324);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(45, 16);
            this.lblMail.TabIndex = 5;
            this.lblMail.Text = "E-Mail";
            // 
            // lblSifre
            // 
            this.lblSifre.AutoSize = true;
            this.lblSifre.Location = new System.Drawing.Point(63, 220);
            this.lblSifre.Name = "lblSifre";
            this.lblSifre.Size = new System.Drawing.Size(34, 16);
            this.lblSifre.TabIndex = 3;
            this.lblSifre.Text = "Şifre";
            // 
            // lblKullaniciAdi
            // 
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Location = new System.Drawing.Point(63, 161);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Size = new System.Drawing.Size(82, 16);
            this.lblKullaniciAdi.TabIndex = 2;
            this.lblKullaniciAdi.Text = "Kullanıcı Adı:";
            // 
            // lblAdSoyad
            // 
            this.lblAdSoyad.AutoSize = true;
            this.lblAdSoyad.Location = new System.Drawing.Point(63, 103);
            this.lblAdSoyad.Name = "lblAdSoyad";
            this.lblAdSoyad.Size = new System.Drawing.Size(70, 16);
            this.lblAdSoyad.TabIndex = 1;
            this.lblAdSoyad.Text = "Ad Soyad:";
            // 
            // txtAdSoyad
            // 
            this.txtAdSoyad.Location = new System.Drawing.Point(179, 97);
            this.txtAdSoyad.Name = "txtAdSoyad";
            this.txtAdSoyad.Size = new System.Drawing.Size(274, 22);
            this.txtAdSoyad.TabIndex = 0;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1707, 50);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // ctlKullaniciAyarlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDetay);
            this.Controls.Add(this.panelDisContainer);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlKullaniciAyarlari";
            this.Size = new System.Drawing.Size(1707, 1019);
            this.Load += new System.EventHandler(this.ctlKullaniciAyarlari_Load);
            this.panelDisContainer.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelDetay.ResumeLayout(false);
            this.panelDetay.PerformLayout();
            this.panelRolAtaCizgi.ResumeLayout(false);
            this.panelRolAtaCizgi.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnYeniKullanici;
        private System.Windows.Forms.Button btnRolAta;
        private System.Windows.Forms.Panel panelDisContainer;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.ListBox lstKullanicilar;
        private System.Windows.Forms.Panel panelSpacer1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelDetay;
        private System.Windows.Forms.ComboBox cbKullaniciRol;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.TextBox txtKullaniciAdi;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.Label lblKullaniciRolu;
        private System.Windows.Forms.Label lblSifre;
        private System.Windows.Forms.Label lblKullaniciAdi;
        private System.Windows.Forms.Label lblAdSoyad;
        private System.Windows.Forms.TextBox txtAdSoyad;
        private System.Windows.Forms.Button btnGüncelle;
        private System.Windows.Forms.Panel panelRolAtaCizgi;
        private System.Windows.Forms.Button btnKullaniciSil;
        private ctlBaslik ctlBaslik1;
        private ctlBaslik ctlBaslik2;
        private System.Windows.Forms.Panel panel1;
    }
}
