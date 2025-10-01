namespace CEKA_APP
{
    partial class frmKullaniciAyarlari
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
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGüncelle = new System.Windows.Forms.Button();
            this.txtAdSoyad = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSifreTekrar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabKullaniciBilgileri = new System.Windows.Forms.TabPage();
            this.tabXMLDosyaYolu = new System.Windows.Forms.TabPage();
            this.btnDosyaSec = new System.Windows.Forms.Button();
            this.txtKlasorYolu = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabKullaniciBilgileri.SuspendLayout();
            this.tabXMLDosyaYolu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(177, 201);
            this.txtMail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(238, 22);
            this.txtMail.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "E-Mail:";
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Enabled = false;
            this.txtKullaniciAdi.Location = new System.Drawing.Point(177, 81);
            this.txtKullaniciAdi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(238, 22);
            this.txtKullaniciAdi.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Kullanıcı Adı:";
            // 
            // btnGüncelle
            // 
            this.btnGüncelle.Location = new System.Drawing.Point(151, 258);
            this.btnGüncelle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGüncelle.Name = "btnGüncelle";
            this.btnGüncelle.Size = new System.Drawing.Size(156, 30);
            this.btnGüncelle.TabIndex = 5;
            this.btnGüncelle.Text = "Güncelle";
            this.btnGüncelle.UseVisualStyleBackColor = true;
            this.btnGüncelle.Click += new System.EventHandler(this.btnGüncelle_Click);
            // 
            // txtAdSoyad
            // 
            this.txtAdSoyad.Location = new System.Drawing.Point(177, 43);
            this.txtAdSoyad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdSoyad.Name = "txtAdSoyad";
            this.txtAdSoyad.Size = new System.Drawing.Size(238, 22);
            this.txtAdSoyad.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ad Soyad:";
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(177, 122);
            this.txtSifre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.Size = new System.Drawing.Size(238, 22);
            this.txtSifre.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Şifre:";
            // 
            // txtSifreTekrar
            // 
            this.txtSifreTekrar.Location = new System.Drawing.Point(177, 161);
            this.txtSifreTekrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSifreTekrar.Name = "txtSifreTekrar";
            this.txtSifreTekrar.Size = new System.Drawing.Size(238, 22);
            this.txtSifreTekrar.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Şifre Tekrarı:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabKullaniciBilgileri);
            this.tabControl1.Controls.Add(this.tabXMLDosyaYolu);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(481, 360);
            this.tabControl1.TabIndex = 24;
            // 
            // tabKullaniciBilgileri
            // 
            this.tabKullaniciBilgileri.BackColor = System.Drawing.SystemColors.Window;
            this.tabKullaniciBilgileri.Controls.Add(this.txtAdSoyad);
            this.tabKullaniciBilgileri.Controls.Add(this.txtSifreTekrar);
            this.tabKullaniciBilgileri.Controls.Add(this.label4);
            this.tabKullaniciBilgileri.Controls.Add(this.label1);
            this.tabKullaniciBilgileri.Controls.Add(this.btnGüncelle);
            this.tabKullaniciBilgileri.Controls.Add(this.txtSifre);
            this.tabKullaniciBilgileri.Controls.Add(this.label2);
            this.tabKullaniciBilgileri.Controls.Add(this.label3);
            this.tabKullaniciBilgileri.Controls.Add(this.txtKullaniciAdi);
            this.tabKullaniciBilgileri.Controls.Add(this.txtMail);
            this.tabKullaniciBilgileri.Controls.Add(this.label6);
            this.tabKullaniciBilgileri.Location = new System.Drawing.Point(4, 25);
            this.tabKullaniciBilgileri.Name = "tabKullaniciBilgileri";
            this.tabKullaniciBilgileri.Padding = new System.Windows.Forms.Padding(3);
            this.tabKullaniciBilgileri.Size = new System.Drawing.Size(473, 331);
            this.tabKullaniciBilgileri.TabIndex = 0;
            this.tabKullaniciBilgileri.Text = "Kullanıcı Bilgileri";
            // 
            // tabXMLDosyaYolu
            // 
            this.tabXMLDosyaYolu.BackColor = System.Drawing.SystemColors.Window;
            this.tabXMLDosyaYolu.Controls.Add(this.btnDosyaSec);
            this.tabXMLDosyaYolu.Controls.Add(this.txtKlasorYolu);
            this.tabXMLDosyaYolu.Controls.Add(this.label5);
            this.tabXMLDosyaYolu.Location = new System.Drawing.Point(4, 25);
            this.tabXMLDosyaYolu.Name = "tabXMLDosyaYolu";
            this.tabXMLDosyaYolu.Padding = new System.Windows.Forms.Padding(3);
            this.tabXMLDosyaYolu.Size = new System.Drawing.Size(473, 331);
            this.tabXMLDosyaYolu.TabIndex = 1;
            this.tabXMLDosyaYolu.Text = "XML Dosya Yolu";
            // 
            // btnDosyaSec
            // 
            this.btnDosyaSec.Location = new System.Drawing.Point(58, 84);
            this.btnDosyaSec.Name = "btnDosyaSec";
            this.btnDosyaSec.Size = new System.Drawing.Size(342, 37);
            this.btnDosyaSec.TabIndex = 2;
            this.btnDosyaSec.Text = "Dosya Seç";
            this.btnDosyaSec.UseVisualStyleBackColor = true;
            this.btnDosyaSec.Click += new System.EventHandler(this.btnDosyaSec_Click);
            // 
            // txtKlasorYolu
            // 
            this.txtKlasorYolu.Location = new System.Drawing.Point(58, 144);
            this.txtKlasorYolu.Name = "txtKlasorYolu";
            this.txtKlasorYolu.Size = new System.Drawing.Size(342, 22);
            this.txtKlasorYolu.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(345, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Lütfen XML dosyasının yükleneceği dosya yolunu seçiniz.";
            // 
            // frmKullaniciAyarlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(481, 360);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "frmKullaniciAyarlari";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kullanıcı Ayarları";
            this.Load += new System.EventHandler(this.frmKullaniciBilgileri_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabKullaniciBilgileri.ResumeLayout(false);
            this.tabKullaniciBilgileri.PerformLayout();
            this.tabXMLDosyaYolu.ResumeLayout(false);
            this.tabXMLDosyaYolu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtKullaniciAdi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGüncelle;
        private System.Windows.Forms.TextBox txtAdSoyad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSifreTekrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabKullaniciBilgileri;
        private System.Windows.Forms.TabPage tabXMLDosyaYolu;
        private System.Windows.Forms.Button btnDosyaSec;
        private System.Windows.Forms.TextBox txtKlasorYolu;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
    }
}