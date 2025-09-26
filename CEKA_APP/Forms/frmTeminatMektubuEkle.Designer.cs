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
            this.lblMektupTur = new System.Windows.Forms.Label();
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
            this.btnKaydet = new System.Windows.Forms.Button();
            this.llbMusteriNo = new System.Windows.Forms.Label();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.cmbMektupTuru = new System.Windows.Forms.ComboBox();
            this.cmbKomisyonVadesi = new System.Windows.Forms.ComboBox();
            this.cmbBankalar = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtMektupNo
            // 
            this.txtMektupNo.Location = new System.Drawing.Point(191, 45);
            this.txtMektupNo.Name = "txtMektupNo";
            this.txtMektupNo.Size = new System.Drawing.Size(279, 22);
            this.txtMektupNo.TabIndex = 1;
            // 
            // lblMektupNo
            // 
            this.lblMektupNo.AutoSize = true;
            this.lblMektupNo.Location = new System.Drawing.Point(67, 48);
            this.lblMektupNo.Name = "lblMektupNo";
            this.lblMektupNo.Size = new System.Drawing.Size(75, 16);
            this.lblMektupNo.TabIndex = 1;
            this.lblMektupNo.Text = "Mektup No:";
            // 
            // lblMusteriAdi
            // 
            this.lblMusteriAdi.AutoSize = true;
            this.lblMusteriAdi.Location = new System.Drawing.Point(67, 151);
            this.lblMusteriAdi.Name = "lblMusteriAdi";
            this.lblMusteriAdi.Size = new System.Drawing.Size(76, 16);
            this.lblMusteriAdi.TabIndex = 3;
            this.lblMusteriAdi.Text = "Müşteri Adi:";
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.Location = new System.Drawing.Point(191, 94);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(279, 22);
            this.txtMusteriNo.TabIndex = 2;
            this.txtMusteriNo.Leave += new System.EventHandler(this.txtMusteriNo_Leave);
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(67, 241);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 5;
            this.lblTutar.Text = "Tutar:";
            // 
            // txtTutar
            // 
            this.txtTutar.Location = new System.Drawing.Point(191, 238);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(279, 22);
            this.txtTutar.TabIndex = 7;
            // 
            // lblBanka
            // 
            this.lblBanka.AutoSize = true;
            this.lblBanka.Location = new System.Drawing.Point(67, 298);
            this.lblBanka.Name = "lblBanka";
            this.lblBanka.Size = new System.Drawing.Size(49, 16);
            this.lblBanka.TabIndex = 7;
            this.lblBanka.Text = "Banka:";
            // 
            // lblMektupTur
            // 
            this.lblMektupTur.AutoSize = true;
            this.lblMektupTur.Location = new System.Drawing.Point(67, 357);
            this.lblMektupTur.Name = "lblMektupTur";
            this.lblMektupTur.Size = new System.Drawing.Size(84, 16);
            this.lblMektupTur.TabIndex = 9;
            this.lblMektupTur.Text = "Mektup Türü:";
            // 
            // chkTL
            // 
            this.chkTL.AutoSize = true;
            this.chkTL.Location = new System.Drawing.Point(358, 197);
            this.chkTL.Name = "chkTL";
            this.chkTL.Size = new System.Drawing.Size(109, 20);
            this.chkTL.TabIndex = 6;
            this.chkTL.Text = "Türk Lirası (₺)";
            this.chkTL.UseVisualStyleBackColor = true;
            this.chkTL.CheckedChanged += new System.EventHandler(this.chkTL_CheckedChanged);
            // 
            // chkDolar
            // 
            this.chkDolar.AutoSize = true;
            this.chkDolar.Location = new System.Drawing.Point(210, 197);
            this.chkDolar.Name = "chkDolar";
            this.chkDolar.Size = new System.Drawing.Size(80, 20);
            this.chkDolar.TabIndex = 5;
            this.chkDolar.Text = "Dolar ($)";
            this.chkDolar.UseVisualStyleBackColor = true;
            this.chkDolar.CheckedChanged += new System.EventHandler(this.chkDolar_CheckedChanged);
            // 
            // chkEuro
            // 
            this.chkEuro.AutoSize = true;
            this.chkEuro.Location = new System.Drawing.Point(67, 197);
            this.chkEuro.Name = "chkEuro";
            this.chkEuro.Size = new System.Drawing.Size(75, 20);
            this.chkEuro.TabIndex = 4;
            this.chkEuro.Text = "Euro (€)";
            this.chkEuro.UseVisualStyleBackColor = true;
            this.chkEuro.CheckedChanged += new System.EventHandler(this.chkEuro_CheckedChanged);
            // 
            // lblVadeTarihi
            // 
            this.lblVadeTarihi.AutoSize = true;
            this.lblVadeTarihi.Location = new System.Drawing.Point(67, 418);
            this.lblVadeTarihi.Name = "lblVadeTarihi";
            this.lblVadeTarihi.Size = new System.Drawing.Size(43, 16);
            this.lblVadeTarihi.TabIndex = 14;
            this.lblVadeTarihi.Text = "Vade:";
            // 
            // dtVadeTarihi
            // 
            this.dtVadeTarihi.Location = new System.Drawing.Point(191, 412);
            this.dtVadeTarihi.Name = "dtVadeTarihi";
            this.dtVadeTarihi.Size = new System.Drawing.Size(279, 22);
            this.dtVadeTarihi.TabIndex = 10;
            this.dtVadeTarihi.ValueChanged += new System.EventHandler(this.dtVadeTarihi_ValueChanged);
            // 
            // dtIadeTarihi
            // 
            this.dtIadeTarihi.Location = new System.Drawing.Point(191, 652);
            this.dtIadeTarihi.Name = "dtIadeTarihi";
            this.dtIadeTarihi.Size = new System.Drawing.Size(279, 22);
            this.dtIadeTarihi.TabIndex = 14;
            // 
            // lblIadeTarihi
            // 
            this.lblIadeTarihi.AutoSize = true;
            this.lblIadeTarihi.Location = new System.Drawing.Point(67, 657);
            this.lblIadeTarihi.Name = "lblIadeTarihi";
            this.lblIadeTarihi.Size = new System.Drawing.Size(74, 16);
            this.lblIadeTarihi.TabIndex = 16;
            this.lblIadeTarihi.Text = "İade Tarihi:";
            // 
            // lblKomisyonTutari
            // 
            this.lblKomisyonTutari.AutoSize = true;
            this.lblKomisyonTutari.Location = new System.Drawing.Point(67, 602);
            this.lblKomisyonTutari.Name = "lblKomisyonTutari";
            this.lblKomisyonTutari.Size = new System.Drawing.Size(106, 16);
            this.lblKomisyonTutari.TabIndex = 19;
            this.lblKomisyonTutari.Text = "Komisyon Tutarı:";
            // 
            // txtKomisyonTutari
            // 
            this.txtKomisyonTutari.Enabled = false;
            this.txtKomisyonTutari.Location = new System.Drawing.Point(191, 599);
            this.txtKomisyonTutari.Name = "txtKomisyonTutari";
            this.txtKomisyonTutari.Size = new System.Drawing.Size(279, 22);
            this.txtKomisyonTutari.TabIndex = 13;
            // 
            // lblKomisyonOrani
            // 
            this.lblKomisyonOrani.AutoSize = true;
            this.lblKomisyonOrani.Location = new System.Drawing.Point(67, 541);
            this.lblKomisyonOrani.Name = "lblKomisyonOrani";
            this.lblKomisyonOrani.Size = new System.Drawing.Size(104, 16);
            this.lblKomisyonOrani.TabIndex = 21;
            this.lblKomisyonOrani.Text = "Komisyon Oranı:";
            // 
            // txtKomisyonOrani
            // 
            this.txtKomisyonOrani.Location = new System.Drawing.Point(191, 538);
            this.txtKomisyonOrani.Name = "txtKomisyonOrani";
            this.txtKomisyonOrani.Size = new System.Drawing.Size(279, 22);
            this.txtKomisyonOrani.TabIndex = 12;
            this.txtKomisyonOrani.TextChanged += new System.EventHandler(this.txtKomisyonOrani_TextChanged);
            // 
            // lblKomisyonVadesi
            // 
            this.lblKomisyonVadesi.AutoSize = true;
            this.lblKomisyonVadesi.Location = new System.Drawing.Point(67, 480);
            this.lblKomisyonVadesi.Name = "lblKomisyonVadesi";
            this.lblKomisyonVadesi.Size = new System.Drawing.Size(115, 16);
            this.lblKomisyonVadesi.TabIndex = 23;
            this.lblKomisyonVadesi.Text = "Komisyon Vadesi:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(149, 718);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(202, 51);
            this.btnKaydet.TabIndex = 15;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // llbMusteriNo
            // 
            this.llbMusteriNo.AutoSize = true;
            this.llbMusteriNo.Location = new System.Drawing.Point(67, 97);
            this.llbMusteriNo.Name = "llbMusteriNo";
            this.llbMusteriNo.Size = new System.Drawing.Size(74, 16);
            this.llbMusteriNo.TabIndex = 25;
            this.llbMusteriNo.Text = "Müşteri No:";
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.Enabled = false;
            this.txtMusteriAdi.Location = new System.Drawing.Point(191, 148);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(279, 22);
            this.txtMusteriAdi.TabIndex = 3;
            // 
            // cmbMektupTuru
            // 
            this.cmbMektupTuru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMektupTuru.FormattingEnabled = true;
            this.cmbMektupTuru.Items.AddRange(new object[] {
            "Kesin",
            "Avans",
            "Kati",
            "Geçici"});
            this.cmbMektupTuru.Location = new System.Drawing.Point(191, 354);
            this.cmbMektupTuru.Name = "cmbMektupTuru";
            this.cmbMektupTuru.Size = new System.Drawing.Size(279, 24);
            this.cmbMektupTuru.TabIndex = 9;
            // 
            // cmbKomisyonVadesi
            // 
            this.cmbKomisyonVadesi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKomisyonVadesi.FormattingEnabled = true;
            this.cmbKomisyonVadesi.Items.AddRange(new object[] {
            "1 Aylık",
            "2 Aylık",
            "3 Aylık",
            "4 Aylık",
            "5 Aylık",
            "6 Aylık",
            "7 Aylık",
            "8 Aylık",
            "9 Aylık",
            "10 Aylık",
            "11 Aylık",
            "12 Aylık"});
            this.cmbKomisyonVadesi.Location = new System.Drawing.Point(188, 477);
            this.cmbKomisyonVadesi.Name = "cmbKomisyonVadesi";
            this.cmbKomisyonVadesi.Size = new System.Drawing.Size(279, 24);
            this.cmbKomisyonVadesi.TabIndex = 11;
            // 
            // cmbBankalar
            // 
            this.cmbBankalar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankalar.FormattingEnabled = true;
            this.cmbBankalar.Items.AddRange(new object[] {
            "Kesin",
            "Avans",
            "Kati",
            "Geçici"});
            this.cmbBankalar.Location = new System.Drawing.Point(191, 298);
            this.cmbBankalar.Name = "cmbBankalar";
            this.cmbBankalar.Size = new System.Drawing.Size(279, 24);
            this.cmbBankalar.TabIndex = 8;
            // 
            // frmTeminatMektubuEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(542, 793);
            this.Controls.Add(this.cmbBankalar);
            this.Controls.Add(this.cmbKomisyonVadesi);
            this.Controls.Add(this.cmbMektupTuru);
            this.Controls.Add(this.txtMusteriAdi);
            this.Controls.Add(this.llbMusteriNo);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.lblKomisyonVadesi);
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
            this.Controls.Add(this.lblBanka);
            this.Controls.Add(this.lblTutar);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.lblMusteriAdi);
            this.Controls.Add(this.txtMusteriNo);
            this.Controls.Add(this.lblMektupNo);
            this.Controls.Add(this.txtMektupNo);
            this.MaximizeBox = false;
            this.Name = "frmTeminatMektubuEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teminat Mektubu Ekle";
            this.Load += new System.EventHandler(this.frmTeminatMektubuEkle_Load);
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
        private System.Windows.Forms.Label lblMektupTur;
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
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Label llbMusteriNo;
        private System.Windows.Forms.TextBox txtMusteriAdi;
        private System.Windows.Forms.ComboBox cmbMektupTuru;
        private System.Windows.Forms.ComboBox cmbKomisyonVadesi;
        private System.Windows.Forms.ComboBox cmbBankalar;
    }
}