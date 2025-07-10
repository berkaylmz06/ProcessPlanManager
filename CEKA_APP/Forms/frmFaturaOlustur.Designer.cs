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
            this.lblKilometreTasi = new System.Windows.Forms.Label();
            this.lblTutar = new System.Windows.Forms.Label();
            this.lblAcıklama = new System.Windows.Forms.Label();
            this.txtKilometreTasiAdi = new System.Windows.Forms.TextBox();
            this.txtTutar = new System.Windows.Forms.TextBox();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.btnPdfOlustur = new System.Windows.Forms.Button();
            this.txtTarih = new System.Windows.Forms.TextBox();
            this.lblTarih = new System.Windows.Forms.Label();
            this.btnUstResim = new System.Windows.Forms.Button();
            this.bnAltResim = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblKilometreTasi
            // 
            this.lblKilometreTasi.AutoSize = true;
            this.lblKilometreTasi.Location = new System.Drawing.Point(50, 71);
            this.lblKilometreTasi.Name = "lblKilometreTasi";
            this.lblKilometreTasi.Size = new System.Drawing.Size(96, 16);
            this.lblKilometreTasi.TabIndex = 0;
            this.lblKilometreTasi.Text = "Kilometre Taşı:";
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(50, 131);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 1;
            this.lblTutar.Text = "Tutar:";
            // 
            // lblAcıklama
            // 
            this.lblAcıklama.AutoSize = true;
            this.lblAcıklama.Location = new System.Drawing.Point(50, 202);
            this.lblAcıklama.Name = "lblAcıklama";
            this.lblAcıklama.Size = new System.Drawing.Size(66, 16);
            this.lblAcıklama.TabIndex = 2;
            this.lblAcıklama.Text = "Açıklama:";
            // 
            // txtKilometreTasiAdi
            // 
            this.txtKilometreTasiAdi.Location = new System.Drawing.Point(172, 68);
            this.txtKilometreTasiAdi.Name = "txtKilometreTasiAdi";
            this.txtKilometreTasiAdi.Size = new System.Drawing.Size(216, 22);
            this.txtKilometreTasiAdi.TabIndex = 3;
            // 
            // txtTutar
            // 
            this.txtTutar.Location = new System.Drawing.Point(172, 128);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(216, 22);
            this.txtTutar.TabIndex = 4;
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(172, 199);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(216, 22);
            this.txtAciklama.TabIndex = 5;
            // 
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.Location = new System.Drawing.Point(629, 392);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(159, 46);
            this.btnPdfOlustur.TabIndex = 6;
            this.btnPdfOlustur.Text = "Pdf Oluştur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // txtTarih
            // 
            this.txtTarih.Location = new System.Drawing.Point(172, 249);
            this.txtTarih.Name = "txtTarih";
            this.txtTarih.Size = new System.Drawing.Size(216, 22);
            this.txtTarih.TabIndex = 8;
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Location = new System.Drawing.Point(50, 252);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(41, 16);
            this.lblTarih.TabIndex = 7;
            this.lblTarih.Text = "Tarih:";
            // 
            // btnUstResim
            // 
            this.btnUstResim.Location = new System.Drawing.Point(12, 392);
            this.btnUstResim.Name = "btnUstResim";
            this.btnUstResim.Size = new System.Drawing.Size(159, 46);
            this.btnUstResim.TabIndex = 9;
            this.btnUstResim.Text = "Üst Resmi Değiştir";
            this.btnUstResim.UseVisualStyleBackColor = true;
            this.btnUstResim.Click += new System.EventHandler(this.btnUstResim_Click);
            // 
            // bnAltResim
            // 
            this.bnAltResim.Location = new System.Drawing.Point(177, 392);
            this.bnAltResim.Name = "bnAltResim";
            this.bnAltResim.Size = new System.Drawing.Size(159, 46);
            this.bnAltResim.TabIndex = 10;
            this.bnAltResim.Text = "Alt Resmi Değiştir";
            this.bnAltResim.UseVisualStyleBackColor = true;
            this.bnAltResim.Click += new System.EventHandler(this.bnAltResim_Click);
            // 
            // frmFaturaOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bnAltResim);
            this.Controls.Add(this.btnUstResim);
            this.Controls.Add(this.txtTarih);
            this.Controls.Add(this.lblTarih);
            this.Controls.Add(this.btnPdfOlustur);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.txtKilometreTasiAdi);
            this.Controls.Add(this.lblAcıklama);
            this.Controls.Add(this.lblTutar);
            this.Controls.Add(this.lblKilometreTasi);
            this.Name = "frmFaturaOlustur";
            this.Text = "Fatura Oluştur";
            this.Load += new System.EventHandler(this.frmFaturaOlustur_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKilometreTasi;
        private System.Windows.Forms.Label lblTutar;
        private System.Windows.Forms.Label lblAcıklama;
        private System.Windows.Forms.TextBox txtKilometreTasiAdi;
        private System.Windows.Forms.TextBox txtTutar;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Button btnPdfOlustur;
        private System.Windows.Forms.TextBox txtTarih;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.Button btnUstResim;
        private System.Windows.Forms.Button bnAltResim;
    }
}