namespace CEKA_APP.Forms
{
    partial class frmFaturaOlustur
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
            this.lblTutar = new System.Windows.Forms.Label();
            this.lblAcıklama = new System.Windows.Forms.Label();
            this.txtTutar = new System.Windows.Forms.TextBox();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.btnPdfOlustur = new System.Windows.Forms.Button();
            this.txtTarih = new System.Windows.Forms.TextBox();
            this.lblTarih = new System.Windows.Forms.Label();
            this.chkEnglish = new System.Windows.Forms.CheckBox();
            this.chkTurkish = new System.Windows.Forms.CheckBox();
            this.txtMusteri = new System.Windows.Forms.TextBox();
            this.btnNotEkle = new System.Windows.Forms.Button();
            this.lblMusteri = new System.Windows.Forms.Label();
            this.btnGoruntule = new System.Windows.Forms.Button();
            this.lblFaturaBilgi = new System.Windows.Forms.Label();
            this.btnSil = new System.Windows.Forms.Button();
            this.txtMusteriAdresi = new System.Windows.Forms.TextBox();
            this.lblMusteriAdresi = new System.Windows.Forms.Label();
            this.txtVergiMerkezi = new System.Windows.Forms.TextBox();
            this.lblVergiMerkezi = new System.Windows.Forms.Label();
            this.txtVergiNo = new System.Windows.Forms.TextBox();
            this.lblVergiNo = new System.Windows.Forms.Label();
            this.txtKdv = new System.Windows.Forms.TextBox();
            this.lblKdv = new System.Windows.Forms.Label();
            this.lblProje = new System.Windows.Forms.Label();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(49, 311);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 1;
            this.lblTutar.Text = "Tutar:";
            // 
            // lblAcıklama
            // 
            this.lblAcıklama.AutoSize = true;
            this.lblAcıklama.Location = new System.Drawing.Point(49, 367);
            this.lblAcıklama.Name = "lblAcıklama";
            this.lblAcıklama.Size = new System.Drawing.Size(66, 16);
            this.lblAcıklama.TabIndex = 2;
            this.lblAcıklama.Text = "Açıklama:";
            // 
            // txtTutar
            // 
            this.txtTutar.Location = new System.Drawing.Point(171, 308);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(216, 22);
            this.txtTutar.TabIndex = 4;
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(171, 364);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(216, 22);
            this.txtAciklama.TabIndex = 5;
            // 
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.Location = new System.Drawing.Point(287, 551);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(110, 46);
            this.btnPdfOlustur.TabIndex = 6;
            this.btnPdfOlustur.Text = "Pdf Oluştur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // txtTarih
            // 
            this.txtTarih.Location = new System.Drawing.Point(171, 414);
            this.txtTarih.Name = "txtTarih";
            this.txtTarih.Size = new System.Drawing.Size(216, 22);
            this.txtTarih.TabIndex = 8;
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Location = new System.Drawing.Point(49, 417);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(41, 16);
            this.lblTarih.TabIndex = 7;
            this.lblTarih.Text = "Tarih:";
            // 
            // chkEnglish
            // 
            this.chkEnglish.AutoSize = true;
            this.chkEnglish.Location = new System.Drawing.Point(102, 504);
            this.chkEnglish.Name = "chkEnglish";
            this.chkEnglish.Size = new System.Drawing.Size(77, 20);
            this.chkEnglish.TabIndex = 9;
            this.chkEnglish.Text = "İngilizce";
            this.chkEnglish.UseVisualStyleBackColor = true;
            this.chkEnglish.CheckedChanged += new System.EventHandler(this.chkEnglish_CheckedChanged);
            // 
            // chkTurkish
            // 
            this.chkTurkish.AutoSize = true;
            this.chkTurkish.Location = new System.Drawing.Point(234, 504);
            this.chkTurkish.Name = "chkTurkish";
            this.chkTurkish.Size = new System.Drawing.Size(71, 20);
            this.chkTurkish.TabIndex = 10;
            this.chkTurkish.Text = "Türkçe";
            this.chkTurkish.UseVisualStyleBackColor = true;
            this.chkTurkish.CheckedChanged += new System.EventHandler(this.chkTurkish_CheckedChanged);
            // 
            // txtMusteri
            // 
            this.txtMusteri.Location = new System.Drawing.Point(173, 88);
            this.txtMusteri.Name = "txtMusteri";
            this.txtMusteri.Size = new System.Drawing.Size(216, 22);
            this.txtMusteri.TabIndex = 11;
            // 
            // btnNotEkle
            // 
            this.btnNotEkle.Location = new System.Drawing.Point(493, 31);
            this.btnNotEkle.Name = "btnNotEkle";
            this.btnNotEkle.Size = new System.Drawing.Size(159, 46);
            this.btnNotEkle.TabIndex = 13;
            this.btnNotEkle.Text = "Not Ekle";
            this.btnNotEkle.UseVisualStyleBackColor = true;
            this.btnNotEkle.Click += new System.EventHandler(this.btnNotEkle_Click);
            // 
            // lblMusteri
            // 
            this.lblMusteri.AutoSize = true;
            this.lblMusteri.Location = new System.Drawing.Point(51, 91);
            this.lblMusteri.Name = "lblMusteri";
            this.lblMusteri.Size = new System.Drawing.Size(53, 16);
            this.lblMusteri.TabIndex = 14;
            this.lblMusteri.Text = "Müşteri:";
            // 
            // btnGoruntule
            // 
            this.btnGoruntule.Location = new System.Drawing.Point(171, 551);
            this.btnGoruntule.Name = "btnGoruntule";
            this.btnGoruntule.Size = new System.Drawing.Size(110, 46);
            this.btnGoruntule.TabIndex = 15;
            this.btnGoruntule.Text = "Görüntüle";
            this.btnGoruntule.UseVisualStyleBackColor = true;
            this.btnGoruntule.Click += new System.EventHandler(this.btnGoruntule_Click);
            // 
            // lblFaturaBilgi
            // 
            this.lblFaturaBilgi.AutoSize = true;
            this.lblFaturaBilgi.Location = new System.Drawing.Point(12, 612);
            this.lblFaturaBilgi.Name = "lblFaturaBilgi";
            this.lblFaturaBilgi.Size = new System.Drawing.Size(0, 16);
            this.lblFaturaBilgi.TabIndex = 16;
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(55, 551);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(110, 46);
            this.btnSil.TabIndex = 17;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // txtMusteriAdresi
            // 
            this.txtMusteriAdresi.Location = new System.Drawing.Point(173, 141);
            this.txtMusteriAdresi.Name = "txtMusteriAdresi";
            this.txtMusteriAdresi.Size = new System.Drawing.Size(216, 22);
            this.txtMusteriAdresi.TabIndex = 19;
            // 
            // lblMusteriAdresi
            // 
            this.lblMusteriAdresi.AutoSize = true;
            this.lblMusteriAdresi.Location = new System.Drawing.Point(51, 144);
            this.lblMusteriAdresi.Name = "lblMusteriAdresi";
            this.lblMusteriAdresi.Size = new System.Drawing.Size(95, 16);
            this.lblMusteriAdresi.TabIndex = 18;
            this.lblMusteriAdresi.Text = "Müşteri Adresi:";
            // 
            // txtVergiMerkezi
            // 
            this.txtVergiMerkezi.Location = new System.Drawing.Point(173, 198);
            this.txtVergiMerkezi.Name = "txtVergiMerkezi";
            this.txtVergiMerkezi.Size = new System.Drawing.Size(216, 22);
            this.txtVergiMerkezi.TabIndex = 21;
            // 
            // lblVergiMerkezi
            // 
            this.lblVergiMerkezi.AutoSize = true;
            this.lblVergiMerkezi.Location = new System.Drawing.Point(51, 201);
            this.lblVergiMerkezi.Name = "lblVergiMerkezi";
            this.lblVergiMerkezi.Size = new System.Drawing.Size(92, 16);
            this.lblVergiMerkezi.TabIndex = 20;
            this.lblVergiMerkezi.Text = "Vergi Merkezi:";
            // 
            // txtVergiNo
            // 
            this.txtVergiNo.Location = new System.Drawing.Point(173, 254);
            this.txtVergiNo.Name = "txtVergiNo";
            this.txtVergiNo.Size = new System.Drawing.Size(216, 22);
            this.txtVergiNo.TabIndex = 23;
            // 
            // lblVergiNo
            // 
            this.lblVergiNo.AutoSize = true;
            this.lblVergiNo.Location = new System.Drawing.Point(51, 257);
            this.lblVergiNo.Name = "lblVergiNo";
            this.lblVergiNo.Size = new System.Drawing.Size(63, 16);
            this.lblVergiNo.TabIndex = 22;
            this.lblVergiNo.Text = "Vergi No:";
            // 
            // txtKdv
            // 
            this.txtKdv.Location = new System.Drawing.Point(171, 467);
            this.txtKdv.Name = "txtKdv";
            this.txtKdv.Size = new System.Drawing.Size(216, 22);
            this.txtKdv.TabIndex = 25;
            this.txtKdv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKdv_KeyPress);
            // 
            // lblKdv
            // 
            this.lblKdv.AutoSize = true;
            this.lblKdv.Location = new System.Drawing.Point(49, 470);
            this.lblKdv.Name = "lblKdv";
            this.lblKdv.Size = new System.Drawing.Size(37, 16);
            this.lblKdv.TabIndex = 24;
            this.lblKdv.Text = "KDV:";
            // 
            // lblProje
            // 
            this.lblProje.AutoSize = true;
            this.lblProje.Location = new System.Drawing.Point(51, 42);
            this.lblProje.Name = "lblProje";
            this.lblProje.Size = new System.Drawing.Size(42, 16);
            this.lblProje.TabIndex = 27;
            this.lblProje.Text = "Proje:";
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(173, 39);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(216, 22);
            this.txtProjeNo.TabIndex = 26;
            // 
            // frmFaturaOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(979, 638);
            this.Controls.Add(this.lblProje);
            this.Controls.Add(this.txtProjeNo);
            this.Controls.Add(this.txtKdv);
            this.Controls.Add(this.lblKdv);
            this.Controls.Add(this.txtVergiNo);
            this.Controls.Add(this.lblVergiNo);
            this.Controls.Add(this.txtVergiMerkezi);
            this.Controls.Add(this.lblVergiMerkezi);
            this.Controls.Add(this.txtMusteriAdresi);
            this.Controls.Add(this.lblMusteriAdresi);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.lblFaturaBilgi);
            this.Controls.Add(this.btnGoruntule);
            this.Controls.Add(this.lblMusteri);
            this.Controls.Add(this.btnNotEkle);
            this.Controls.Add(this.txtMusteri);
            this.Controls.Add(this.chkTurkish);
            this.Controls.Add(this.chkEnglish);
            this.Controls.Add(this.txtTarih);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.btnPdfOlustur);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.lblAcıklama);
            this.Controls.Add(this.lblTutar);
            this.MaximizeBox = false;
            this.Name = "frmFaturaOlustur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fatura Oluştur";
            this.Load += new System.EventHandler(this.frmFaturaOlustur_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTutar;
        private System.Windows.Forms.Label lblAcıklama;
        private System.Windows.Forms.TextBox txtTutar;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Button btnPdfOlustur;
        private System.Windows.Forms.TextBox txtTarih;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.CheckBox chkEnglish;
        private System.Windows.Forms.CheckBox chkTurkish;
        private System.Windows.Forms.TextBox txtMusteri;
        private System.Windows.Forms.Button btnNotEkle;
        private System.Windows.Forms.Label lblMusteri;
        private System.Windows.Forms.Button btnGoruntule;
        private System.Windows.Forms.Label lblFaturaBilgi;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.TextBox txtMusteriAdresi;
        private System.Windows.Forms.Label lblMusteriAdresi;
        private System.Windows.Forms.TextBox txtVergiMerkezi;
        private System.Windows.Forms.Label lblVergiMerkezi;
        private System.Windows.Forms.TextBox txtVergiNo;
        private System.Windows.Forms.Label lblVergiNo;
        private System.Windows.Forms.TextBox txtKdv;
        private System.Windows.Forms.Label lblKdv;
        private System.Windows.Forms.Label lblProje;
        private System.Windows.Forms.TextBox txtProjeNo;
    }
}