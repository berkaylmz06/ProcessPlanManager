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
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.btnNotEkle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGoruntule = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(51, 95);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 1;
            this.lblTutar.Text = "Tutar:";
            // 
            // lblAcıklama
            // 
            this.lblAcıklama.AutoSize = true;
            this.lblAcıklama.Location = new System.Drawing.Point(51, 151);
            this.lblAcıklama.Name = "lblAcıklama";
            this.lblAcıklama.Size = new System.Drawing.Size(66, 16);
            this.lblAcıklama.TabIndex = 2;
            this.lblAcıklama.Text = "Açıklama:";
            // 
            // txtTutar
            // 
            this.txtTutar.Location = new System.Drawing.Point(173, 92);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(216, 22);
            this.txtTutar.TabIndex = 4;
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(173, 148);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(216, 22);
            this.txtAciklama.TabIndex = 5;
            // 
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.Location = new System.Drawing.Point(215, 347);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(159, 46);
            this.btnPdfOlustur.TabIndex = 6;
            this.btnPdfOlustur.Text = "Pdf Oluştur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // txtTarih
            // 
            this.txtTarih.Location = new System.Drawing.Point(173, 198);
            this.txtTarih.Name = "txtTarih";
            this.txtTarih.Size = new System.Drawing.Size(216, 22);
            this.txtTarih.TabIndex = 8;
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Location = new System.Drawing.Point(51, 201);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(41, 16);
            this.lblTarih.TabIndex = 7;
            this.lblTarih.Text = "Tarih:";
            // 
            // chkEnglish
            // 
            this.chkEnglish.AutoSize = true;
            this.chkEnglish.Location = new System.Drawing.Point(99, 257);
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
            this.chkTurkish.Location = new System.Drawing.Point(231, 257);
            this.chkTurkish.Name = "chkTurkish";
            this.chkTurkish.Size = new System.Drawing.Size(71, 20);
            this.chkTurkish.TabIndex = 10;
            this.chkTurkish.Text = "Türkçe";
            this.chkTurkish.UseVisualStyleBackColor = true;
            this.chkTurkish.CheckedChanged += new System.EventHandler(this.chkTurkish_CheckedChanged);
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(173, 43);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(216, 22);
            this.txtProjeNo.TabIndex = 11;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Müşteri:";
            // 
            // btnGoruntule
            // 
            this.btnGoruntule.Location = new System.Drawing.Point(50, 347);
            this.btnGoruntule.Name = "btnGoruntule";
            this.btnGoruntule.Size = new System.Drawing.Size(159, 46);
            this.btnGoruntule.TabIndex = 15;
            this.btnGoruntule.Text = "Görüntüle";
            this.btnGoruntule.UseVisualStyleBackColor = true;
            this.btnGoruntule.Click += new System.EventHandler(this.btnGoruntule_Click);
            // 
            // frmFaturaOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(955, 449);
            this.Controls.Add(this.btnGoruntule);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNotEkle);
            this.Controls.Add(this.txtProjeNo);
            this.Controls.Add(this.chkTurkish);
            this.Controls.Add(this.chkEnglish);
            this.Controls.Add(this.txtTarih);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.btnPdfOlustur);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.lblAcıklama);
            this.Controls.Add(this.lblTutar);
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
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Button btnNotEkle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGoruntule;
    }
}