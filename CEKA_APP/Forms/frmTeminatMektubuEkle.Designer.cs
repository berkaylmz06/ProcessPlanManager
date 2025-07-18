namespace CEKA_APP.Forms
{
    partial class frmTeminatMektubuEkle
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
            this.txtMektupNo = new System.Windows.Forms.TextBox();
            this.lblMektupNo = new System.Windows.Forms.Label();
            this.lblMusteriAdi = new System.Windows.Forms.Label();
            this.txtMusteriNo = new System.Windows.Forms.TextBox();
            this.lblTutar = new System.Windows.Forms.Label();
            this.txtTutar = new System.Windows.Forms.TextBox();
            this.lblBanka = new System.Windows.Forms.Label();
            this.txtBanka = new System.Windows.Forms.TextBox();
            this.lblMektupTur = new System.Windows.Forms.Label();
            this.txtMektupTuru = new System.Windows.Forms.TextBox();
            this.chkTL = new System.Windows.Forms.CheckBox();
            this.chkDolar = new System.Windows.Forms.CheckBox();
            this.chkEuro = new System.Windows.Forms.CheckBox();
            this.lblVadeTarihi = new System.Windows.Forms.Label();
            this.dtVadeTarihi = new System.Windows.Forms.DateTimePicker();
            this.dtIadeTarihi = new System.Windows.Forms.DateTimePicker();
            this.lblIadeTarihi = new System.Windows.Forms.Label();
            this.lblKomisyonTutari = new System.Windows.Forms.Label();
            this.txtKomisyonTutari = new System.Windows.Forms.TextBox();
            this.lblKomisyonOrani = new System.Windows.Forms.Label();
            this.txtKomisyonOrani = new System.Windows.Forms.TextBox();
            this.lblKomisyonVadesi = new System.Windows.Forms.Label();
            this.txtKomisyonVadesi = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.llbMusteriNo = new System.Windows.Forms.Label();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMektupNo
            // 
            this.txtMektupNo.Location = new System.Drawing.Point(192, 56);
            this.txtMektupNo.Name = "txtMektupNo";
            this.txtMektupNo.Size = new System.Drawing.Size(279, 22);
            this.txtMektupNo.TabIndex = 0;
            // 
            // lblMektupNo
            // 
            this.lblMektupNo.AutoSize = true;
            this.lblMektupNo.Location = new System.Drawing.Point(68, 59);
            this.lblMektupNo.Name = "lblMektupNo";
            this.lblMektupNo.Size = new System.Drawing.Size(77, 16);
            this.lblMektupNo.TabIndex = 1;
            this.lblMektupNo.Text = "Mektup NO:";
            // 
            // lblMusteriAdi
            // 
            this.lblMusteriAdi.AutoSize = true;
            this.lblMusteriAdi.Location = new System.Drawing.Point(68, 160);
            this.lblMusteriAdi.Name = "lblMusteriAdi";
            this.lblMusteriAdi.Size = new System.Drawing.Size(95, 20);
            this.lblMusteriAdi.TabIndex = 3;
            this.lblMusteriAdi.Text = "Müşteri Adi:";
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.Location = new System.Drawing.Point(192, 103);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(279, 22);
            this.txtMusteriNo.TabIndex = 2;
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(68, 250);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 5;
            this.lblTutar.Text = "Tutar:";
            // 
            // txtTutar
            // 
            this.txtTutar.Location = new System.Drawing.Point(192, 247);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(279, 22);
            this.txtTutar.TabIndex = 4;
            // 
            // lblBanka
            // 
            this.lblBanka.AutoSize = true;
            this.lblBanka.Location = new System.Drawing.Point(68, 307);
            this.lblBanka.Name = "lblBanka";
            this.lblBanka.Size = new System.Drawing.Size(49, 16);
            this.lblBanka.TabIndex = 7;
            this.lblBanka.Text = "Banka:";
            // 
            // txtBanka
            // 
            this.txtBanka.Location = new System.Drawing.Point(192, 304);
            this.txtBanka.Name = "txtBanka";
            this.txtBanka.Size = new System.Drawing.Size(279, 22);
            this.txtBanka.TabIndex = 6;
            // 
            // lblMektupTur
            // 
            this.lblMektupTur.AutoSize = true;
            this.lblMektupTur.Location = new System.Drawing.Point(68, 366);
            this.lblMektupTur.Name = "lblMektupTur";
            this.lblMektupTur.Size = new System.Drawing.Size(84, 16);
            this.lblMektupTur.TabIndex = 9;
            this.lblMektupTur.Text = "Mektup Türü:";
            // 
            // txtMektupTuru
            // 
            this.txtMektupTuru.Location = new System.Drawing.Point(192, 363);
            this.txtMektupTuru.Name = "txtMektupTuru";
            this.txtMektupTuru.Size = new System.Drawing.Size(279, 22);
            this.txtMektupTuru.TabIndex = 8;
            // 
            // chkTL
            // 
            this.chkTL.AutoSize = true;
            this.chkTL.Location = new System.Drawing.Point(362, 199);
            this.chkTL.Name = "chkTL";
            this.chkTL.Size = new System.Drawing.Size(109, 20);
            this.chkTL.TabIndex = 10;
            this.chkTL.Text = "Türk Lirası (₺)";
            this.chkTL.UseVisualStyleBackColor = true;
            // 
            // chkDolar
            // 
            this.chkDolar.AutoSize = true;
            this.chkDolar.Location = new System.Drawing.Point(214, 199);
            this.chkDolar.Name = "chkDolar";
            this.chkDolar.Size = new System.Drawing.Size(80, 20);
            this.chkDolar.TabIndex = 11;
            this.chkDolar.Text = "Dolar ($)";
            this.chkDolar.UseVisualStyleBackColor = true;
            // 
            // chkEuro
            // 
            this.chkEuro.AutoSize = true;
            this.chkEuro.Location = new System.Drawing.Point(71, 199);
            this.chkEuro.Name = "chkEuro";
            this.chkEuro.Size = new System.Drawing.Size(75, 20);
            this.chkEuro.TabIndex = 12;
            this.chkEuro.Text = "Euro (€)";
            this.chkEuro.UseVisualStyleBackColor = true;
            // 
            // lblVadeTarihi
            // 
            this.lblVadeTarihi.AutoSize = true;
            this.lblVadeTarihi.Location = new System.Drawing.Point(68, 427);
            this.lblVadeTarihi.Name = "lblVadeTarihi";
            this.lblVadeTarihi.Size = new System.Drawing.Size(43, 16);
            this.lblVadeTarihi.TabIndex = 14;
            this.lblVadeTarihi.Text = "Vade:";
            // 
            // dtVadeTarihi
            // 
            this.dtVadeTarihi.Location = new System.Drawing.Point(192, 421);
            this.dtVadeTarihi.Name = "dtVadeTarihi";
            this.dtVadeTarihi.Size = new System.Drawing.Size(279, 22);
            this.dtVadeTarihi.TabIndex = 15;
            // 
            // dtIadeTarihi
            // 
            this.dtIadeTarihi.Location = new System.Drawing.Point(192, 481);
            this.dtIadeTarihi.Name = "dtIadeTarihi";
            this.dtIadeTarihi.Size = new System.Drawing.Size(279, 22);
            this.dtIadeTarihi.TabIndex = 17;
            // 
            // lblIadeTarihi
            // 
            this.lblIadeTarihi.AutoSize = true;
            this.lblIadeTarihi.Location = new System.Drawing.Point(68, 487);
            this.lblIadeTarihi.Name = "lblIadeTarihi";
            this.lblIadeTarihi.Size = new System.Drawing.Size(74, 16);
            this.lblIadeTarihi.TabIndex = 16;
            this.lblIadeTarihi.Text = "İade Tarihi:";
            // 
            // lblKomisyonTutari
            // 
            this.lblKomisyonTutari.AutoSize = true;
            this.lblKomisyonTutari.Location = new System.Drawing.Point(68, 555);
            this.lblKomisyonTutari.Name = "lblKomisyonTutari";
            this.lblKomisyonTutari.Size = new System.Drawing.Size(106, 16);
            this.lblKomisyonTutari.TabIndex = 19;
            this.lblKomisyonTutari.Text = "Komisyon Tutarı:";
            // 
            // txtKomisyonTutari
            // 
            this.txtKomisyonTutari.Location = new System.Drawing.Point(192, 552);
            this.txtKomisyonTutari.Name = "txtKomisyonTutari";
            this.txtKomisyonTutari.Size = new System.Drawing.Size(279, 22);
            this.txtKomisyonTutari.TabIndex = 18;
            // 
            // lblKomisyonOrani
            // 
            this.lblKomisyonOrani.AutoSize = true;
            this.lblKomisyonOrani.Location = new System.Drawing.Point(68, 615);
            this.lblKomisyonOrani.Name = "lblKomisyonOrani";
            this.lblKomisyonOrani.Size = new System.Drawing.Size(104, 16);
            this.lblKomisyonOrani.TabIndex = 21;
            this.lblKomisyonOrani.Text = "Komisyon Oranı:";
            // 
            // txtKomisyonOrani
            // 
            this.txtKomisyonOrani.Location = new System.Drawing.Point(192, 612);
            this.txtKomisyonOrani.Name = "txtKomisyonOrani";
            this.txtKomisyonOrani.Size = new System.Drawing.Size(279, 22);
            this.txtKomisyonOrani.TabIndex = 20;
            // 
            // lblKomisyonVadesi
            // 
            this.lblKomisyonVadesi.AutoSize = true;
            this.lblKomisyonVadesi.Location = new System.Drawing.Point(68, 677);
            this.lblKomisyonVadesi.Name = "lblKomisyonVadesi";
            this.lblKomisyonVadesi.Size = new System.Drawing.Size(115, 16);
            this.lblKomisyonVadesi.TabIndex = 23;
            this.lblKomisyonVadesi.Text = "Komisyon Vadesi:";
            // 
            // txtKomisyonVadesi
            // 
            this.txtKomisyonVadesi.Location = new System.Drawing.Point(192, 674);
            this.txtKomisyonVadesi.Name = "txtKomisyonVadesi";
            this.txtKomisyonVadesi.Size = new System.Drawing.Size(279, 22);
            this.txtKomisyonVadesi.TabIndex = 22;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(158, 718);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(202, 51);
            this.btnKaydet.TabIndex = 24;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // llbMusteriNo
            // 
            this.llbMusteriNo.AutoSize = true;
            this.llbMusteriNo.Location = new System.Drawing.Point(68, 106);
            this.llbMusteriNo.Name = "llbMusteriNo";
            this.llbMusteriNo.Size = new System.Drawing.Size(74, 16);
            this.llbMusteriNo.TabIndex = 25;
            this.llbMusteriNo.Text = "Müşteri No:";
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.Location = new System.Drawing.Point(192, 157);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(279, 22);
            this.txtMusteriAdi.TabIndex = 26;
            // 
            // frmTeminatMektubuEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 781);
            this.Controls.Add(this.txtMusteriAdi);
            this.Controls.Add(this.llbMusteriNo);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.lblKomisyonVadesi);
            this.Controls.Add(this.txtKomisyonVadesi);
            this.Controls.Add(this.lblKomisyonOrani);
            this.Controls.Add(this.txtKomisyonOrani);
            this.Controls.Add(this.lblKomisyonTutari);
            this.Controls.Add(this.txtKomisyonTutari);
            this.Controls.Add(this.dtIadeTarihi);
            this.Controls.Add(this.lblIadeTarihi);
            this.Controls.Add(this.dtVadeTarihi);
            this.Controls.Add(this.lblVadeTarihi);
            this.Controls.Add(this.chkEuro);
            this.Controls.Add(this.chkDolar);
            this.Controls.Add(this.chkTL);
            this.Controls.Add(this.lblMektupTur);
            this.Controls.Add(this.txtMektupTuru);
            this.Controls.Add(this.lblBanka);
            this.Controls.Add(this.txtBanka);
            this.Controls.Add(this.lblTutar);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.lblMusteriAdi);
            this.Controls.Add(this.txtMusteriNo);
            this.Controls.Add(this.lblMektupNo);
            this.Controls.Add(this.txtMektupNo);
            this.Name = "frmTeminatMektubuEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teminat Mektubu Ekle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMektupNo;
        private System.Windows.Forms.Label lblMektupNo;
        private System.Windows.Forms.Label lblMusteriAdi;
        private System.Windows.Forms.TextBox txtMusteriNo;
        private System.Windows.Forms.Label lblTutar;
        private System.Windows.Forms.TextBox txtTutar;
        private System.Windows.Forms.Label lblBanka;
        private System.Windows.Forms.TextBox txtBanka;
        private System.Windows.Forms.Label lblMektupTur;
        private System.Windows.Forms.TextBox txtMektupTuru;
        private System.Windows.Forms.CheckBox chkTL;
        private System.Windows.Forms.CheckBox chkDolar;
        private System.Windows.Forms.CheckBox chkEuro;
        private System.Windows.Forms.Label lblVadeTarihi;
        private System.Windows.Forms.DateTimePicker dtVadeTarihi;
        private System.Windows.Forms.DateTimePicker dtIadeTarihi;
        private System.Windows.Forms.Label lblIadeTarihi;
        private System.Windows.Forms.Label lblKomisyonTutari;
        private System.Windows.Forms.TextBox txtKomisyonTutari;
        private System.Windows.Forms.Label lblKomisyonOrani;
        private System.Windows.Forms.TextBox txtKomisyonOrani;
        private System.Windows.Forms.Label lblKomisyonVadesi;
        private System.Windows.Forms.TextBox txtKomisyonVadesi;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Label llbMusteriNo;
        private System.Windows.Forms.TextBox txtMusteriAdi;
    }
}