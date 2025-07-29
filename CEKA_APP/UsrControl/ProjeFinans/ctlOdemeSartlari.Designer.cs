namespace CEKA_APP.UsrControl
{
    partial class ctlOdemeSartlari
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
            this.txtToplamBedel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProjeAra = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.btnHesapla = new System.Windows.Forms.Button();
            this.txtEksilenTutar = new System.Windows.Forms.TextBox();
            this.btnTopluFaturaOlustur = new System.Windows.Forms.Button();
            this.btnKilometreTasiEkle = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.panelFill = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxOdemeTutari = new System.Windows.Forms.GroupBox();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.chkTutarTamaminiKullan = new System.Windows.Forms.CheckBox();
            this.panelAlt.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.panelFill.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxOdemeTutari.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtToplamBedel
            // 
            this.txtToplamBedel.Location = new System.Drawing.Point(1188, 31);
            this.txtToplamBedel.Name = "txtToplamBedel";
            this.txtToplamBedel.Size = new System.Drawing.Size(293, 22);
            this.txtToplamBedel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1083, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Toplam Bedel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Proje Ara";
            // 
            // txtProjeAra
            // 
            this.txtProjeAra.Location = new System.Drawing.Point(127, 26);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(163, 22);
            this.txtProjeAra.TabIndex = 2;
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(296, 22);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(149, 31);
            this.btnAra.TabIndex = 4;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // panelAlt
            // 
            this.panelAlt.Controls.Add(this.panelRight);
            this.panelAlt.Controls.Add(this.panelLeft);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 689);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1674, 181);
            this.panelAlt.TabIndex = 6;
            // 
            // btnHesapla
            // 
            this.btnHesapla.Location = new System.Drawing.Point(190, 24);
            this.btnHesapla.Name = "btnHesapla";
            this.btnHesapla.Size = new System.Drawing.Size(100, 29);
            this.btnHesapla.TabIndex = 7;
            this.btnHesapla.Text = "Hesapla";
            this.btnHesapla.UseVisualStyleBackColor = true;
            this.btnHesapla.Click += new System.EventHandler(this.btnHesapla_Click);
            // 
            // txtEksilenTutar
            // 
            this.txtEksilenTutar.Location = new System.Drawing.Point(16, 27);
            this.txtEksilenTutar.Name = "txtEksilenTutar";
            this.txtEksilenTutar.Size = new System.Drawing.Size(163, 22);
            this.txtEksilenTutar.TabIndex = 6;
            // 
            // btnTopluFaturaOlustur
            // 
            this.btnTopluFaturaOlustur.Location = new System.Drawing.Point(44, 114);
            this.btnTopluFaturaOlustur.Name = "btnTopluFaturaOlustur";
            this.btnTopluFaturaOlustur.Size = new System.Drawing.Size(146, 48);
            this.btnTopluFaturaOlustur.TabIndex = 5;
            this.btnTopluFaturaOlustur.Text = "Toplu Fatura Oluştur";
            this.btnTopluFaturaOlustur.UseVisualStyleBackColor = true;
            this.btnTopluFaturaOlustur.Click += new System.EventHandler(this.btnTopluFaturaOlustur_Click);
            // 
            // btnKilometreTasiEkle
            // 
            this.btnKilometreTasiEkle.Location = new System.Drawing.Point(16, 6);
            this.btnKilometreTasiEkle.Name = "btnKilometreTasiEkle";
            this.btnKilometreTasiEkle.Size = new System.Drawing.Size(146, 48);
            this.btnKilometreTasiEkle.TabIndex = 4;
            this.btnKilometreTasiEkle.Text = "+ Kilometre Taşı Ekle";
            this.btnKilometreTasiEkle.UseVisualStyleBackColor = true;
            this.btnKilometreTasiEkle.Click += new System.EventHandler(this.btnKilometreTasiEkle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(215, 114);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // panelUst
            // 
            this.panelUst.Controls.Add(this.txtToplamBedel);
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.btnAra);
            this.panelUst.Controls.Add(this.label1);
            this.panelUst.Controls.Add(this.label2);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 63);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1674, 95);
            this.panelUst.TabIndex = 7;
            // 
            // panelFill
            // 
            this.panelFill.Controls.Add(this.tableLayoutPanel1);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 158);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(1674, 531);
            this.panelFill.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.28358F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.71642F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 227F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1674, 531);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.btnKilometreTasiEkle);
            this.panelLeft.Controls.Add(this.groupBoxOdemeTutari);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(305, 181);
            this.panelLeft.TabIndex = 8;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.btnTopluFaturaOlustur);
            this.panelRight.Controls.Add(this.btnKaydet);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1291, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(383, 181);
            this.panelRight.TabIndex = 9;
            // 
            // groupBoxOdemeTutari
            // 
            this.groupBoxOdemeTutari.Controls.Add(this.chkTutarTamaminiKullan);
            this.groupBoxOdemeTutari.Controls.Add(this.txtEksilenTutar);
            this.groupBoxOdemeTutari.Controls.Add(this.btnHesapla);
            this.groupBoxOdemeTutari.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxOdemeTutari.Location = new System.Drawing.Point(0, 90);
            this.groupBoxOdemeTutari.Name = "groupBoxOdemeTutari";
            this.groupBoxOdemeTutari.Size = new System.Drawing.Size(305, 91);
            this.groupBoxOdemeTutari.TabIndex = 10;
            this.groupBoxOdemeTutari.TabStop = false;
            this.groupBoxOdemeTutari.Text = "Ödeme Tutarı";
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1674, 63);
            this.ctlBaslik1.TabIndex = 5;
            // 
            // chkTutarTamaminiKullan
            // 
            this.chkTutarTamaminiKullan.AutoSize = true;
            this.chkTutarTamaminiKullan.Location = new System.Drawing.Point(16, 55);
            this.chkTutarTamaminiKullan.Name = "chkTutarTamaminiKullan";
            this.chkTutarTamaminiKullan.Size = new System.Drawing.Size(199, 20);
            this.chkTutarTamaminiKullan.TabIndex = 10;
            this.chkTutarTamaminiKullan.Text = "Kalan tutarın tamamını kullan.";
            this.chkTutarTamaminiKullan.UseVisualStyleBackColor = true;
            // 
            // ctlOdemeSartlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelFill);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelAlt);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlOdemeSartlari";
            this.Size = new System.Drawing.Size(1674, 870);
            this.Load += new System.EventHandler(this.ctlOdemeSartlari_Load);
            this.panelAlt.ResumeLayout(false);
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelFill.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.groupBoxOdemeTutari.ResumeLayout(false);
            this.groupBoxOdemeTutari.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtToplamBedel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProjeAra;
        private System.Windows.Forms.Button btnAra;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.Button btnKilometreTasiEkle;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.Panel panelFill;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnTopluFaturaOlustur;
        private System.Windows.Forms.Button btnHesapla;
        private System.Windows.Forms.TextBox txtEksilenTutar;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox groupBoxOdemeTutari;
        private System.Windows.Forms.CheckBox chkTutarTamaminiKullan;
    }
}
