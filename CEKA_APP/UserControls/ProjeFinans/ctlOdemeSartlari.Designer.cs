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
            this.lblToplamBedel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProjeAra = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.lblTeminatBilgi = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.btnTopluFaturaOlustur = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxOdemeTutari = new System.Windows.Forms.GroupBox();
            this.dtOdemeTarihi = new System.Windows.Forms.DateTimePicker();
            this.lblOdemeTarihi = new System.Windows.Forms.Label();
            this.lblOdemeTutari = new System.Windows.Forms.Label();
            this.lblÖdemeAciklama = new System.Windows.Forms.Label();
            this.rtxtAciklama = new System.Windows.Forms.RichTextBox();
            this.chkTutarTamaminiKullan = new System.Windows.Forms.CheckBox();
            this.txtEksilenTutar = new System.Windows.Forms.TextBox();
            this.btnHesapla = new System.Windows.Forms.Button();
            this.btnKilometreTasiEkle = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.txtStatu = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnKopyala = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.lblToplamBedelBilgi = new System.Windows.Forms.Label();
            this.panelFill = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelAlt.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxOdemeTutari.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.panelFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtToplamBedel
            // 
            this.txtToplamBedel.BackColor = System.Drawing.Color.White;
            this.txtToplamBedel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToplamBedel.Enabled = false;
            this.txtToplamBedel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtToplamBedel.Location = new System.Drawing.Point(372, 40);
            this.txtToplamBedel.Name = "txtToplamBedel";
            this.txtToplamBedel.Size = new System.Drawing.Size(163, 30);
            this.txtToplamBedel.TabIndex = 0;
            // 
            // lblToplamBedel
            // 
            this.lblToplamBedel.AutoSize = true;
            this.lblToplamBedel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblToplamBedel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblToplamBedel.Location = new System.Drawing.Point(368, 14);
            this.lblToplamBedel.Name = "lblToplamBedel";
            this.lblToplamBedel.Size = new System.Drawing.Size(124, 23);
            this.lblToplamBedel.TabIndex = 1;
            this.lblToplamBedel.Text = "Toplam Bedel:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label2.Location = new System.Drawing.Point(30, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Proje:";
            // 
            // txtProjeAra
            // 
            this.txtProjeAra.BackColor = System.Drawing.Color.White;
            this.txtProjeAra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeAra.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProjeAra.Location = new System.Drawing.Point(34, 40);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(163, 30);
            this.txtProjeAra.TabIndex = 2;
            this.txtProjeAra.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeAra_KeyDown);
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.Location = new System.Drawing.Point(1201, 40);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(149, 31);
            this.btnAra.TabIndex = 4;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = false;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // panelAlt
            // 
            this.panelAlt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelAlt.Controls.Add(this.lblTeminatBilgi);
            this.panelAlt.Controls.Add(this.panelRight);
            this.panelAlt.Controls.Add(this.panelLeft);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 691);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1674, 308);
            this.panelAlt.TabIndex = 6;
            // 
            // lblTeminatBilgi
            // 
            this.lblTeminatBilgi.AutoSize = true;
            this.lblTeminatBilgi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTeminatBilgi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblTeminatBilgi.Location = new System.Drawing.Point(443, 22);
            this.lblTeminatBilgi.Name = "lblTeminatBilgi";
            this.lblTeminatBilgi.Size = new System.Drawing.Size(36, 20);
            this.lblTeminatBilgi.TabIndex = 10;
            this.lblTeminatBilgi.Text = ".........";
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelRight.Controls.Add(this.btnTopluFaturaOlustur);
            this.panelRight.Controls.Add(this.btnKaydet);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1297, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(377, 308);
            this.panelRight.TabIndex = 9;
            // 
            // btnTopluFaturaOlustur
            // 
            this.btnTopluFaturaOlustur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnTopluFaturaOlustur.FlatAppearance.BorderSize = 0;
            this.btnTopluFaturaOlustur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTopluFaturaOlustur.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTopluFaturaOlustur.ForeColor = System.Drawing.Color.White;
            this.btnTopluFaturaOlustur.Location = new System.Drawing.Point(38, 242);
            this.btnTopluFaturaOlustur.Name = "btnTopluFaturaOlustur";
            this.btnTopluFaturaOlustur.Size = new System.Drawing.Size(146, 48);
            this.btnTopluFaturaOlustur.TabIndex = 5;
            this.btnTopluFaturaOlustur.Text = "Toplu Fatura Oluştur";
            this.btnTopluFaturaOlustur.UseVisualStyleBackColor = false;
            this.btnTopluFaturaOlustur.Click += new System.EventHandler(this.btnTopluFaturaOlustur_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(202, 242);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelLeft.Controls.Add(this.groupBoxOdemeTutari);
            this.panelLeft.Controls.Add(this.btnKilometreTasiEkle);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(416, 308);
            this.panelLeft.TabIndex = 8;
            // 
            // groupBoxOdemeTutari
            // 
            this.groupBoxOdemeTutari.Controls.Add(this.dtOdemeTarihi);
            this.groupBoxOdemeTutari.Controls.Add(this.lblOdemeTarihi);
            this.groupBoxOdemeTutari.Controls.Add(this.lblOdemeTutari);
            this.groupBoxOdemeTutari.Controls.Add(this.lblÖdemeAciklama);
            this.groupBoxOdemeTutari.Controls.Add(this.rtxtAciklama);
            this.groupBoxOdemeTutari.Controls.Add(this.chkTutarTamaminiKullan);
            this.groupBoxOdemeTutari.Controls.Add(this.txtEksilenTutar);
            this.groupBoxOdemeTutari.Controls.Add(this.btnHesapla);
            this.groupBoxOdemeTutari.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxOdemeTutari.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxOdemeTutari.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBoxOdemeTutari.Location = new System.Drawing.Point(0, 60);
            this.groupBoxOdemeTutari.Name = "groupBoxOdemeTutari";
            this.groupBoxOdemeTutari.Size = new System.Drawing.Size(416, 248);
            this.groupBoxOdemeTutari.TabIndex = 10;
            this.groupBoxOdemeTutari.TabStop = false;
            this.groupBoxOdemeTutari.Text = "Ödeme Tutarı";
            // 
            // dtOdemeTarihi
            // 
            this.dtOdemeTarihi.Checked = false;
            this.dtOdemeTarihi.Location = new System.Drawing.Point(123, 94);
            this.dtOdemeTarihi.Name = "dtOdemeTarihi";
            this.dtOdemeTarihi.ShowCheckBox = true;
            this.dtOdemeTarihi.Size = new System.Drawing.Size(258, 27);
            this.dtOdemeTarihi.TabIndex = 11;
            // 
            // lblOdemeTarihi
            // 
            this.lblOdemeTarihi.AutoSize = true;
            this.lblOdemeTarihi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOdemeTarihi.Location = new System.Drawing.Point(12, 96);
            this.lblOdemeTarihi.Name = "lblOdemeTarihi";
            this.lblOdemeTarihi.Size = new System.Drawing.Size(100, 20);
            this.lblOdemeTarihi.TabIndex = 13;
            this.lblOdemeTarihi.Text = "Ödeme Tarihi:";
            // 
            // lblOdemeTutari
            // 
            this.lblOdemeTutari.AutoSize = true;
            this.lblOdemeTutari.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOdemeTutari.Location = new System.Drawing.Point(13, 33);
            this.lblOdemeTutari.Name = "lblOdemeTutari";
            this.lblOdemeTutari.Size = new System.Drawing.Size(46, 20);
            this.lblOdemeTutari.TabIndex = 12;
            this.lblOdemeTutari.Text = "Tutar:";
            // 
            // lblÖdemeAciklama
            // 
            this.lblÖdemeAciklama.AutoSize = true;
            this.lblÖdemeAciklama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblÖdemeAciklama.Location = new System.Drawing.Point(13, 133);
            this.lblÖdemeAciklama.Name = "lblÖdemeAciklama";
            this.lblÖdemeAciklama.Size = new System.Drawing.Size(73, 20);
            this.lblÖdemeAciklama.TabIndex = 11;
            this.lblÖdemeAciklama.Text = "Açıklama:";
            // 
            // rtxtAciklama
            // 
            this.rtxtAciklama.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtAciklama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtxtAciklama.Location = new System.Drawing.Point(123, 133);
            this.rtxtAciklama.Name = "rtxtAciklama";
            this.rtxtAciklama.Size = new System.Drawing.Size(258, 77);
            this.rtxtAciklama.TabIndex = 6;
            this.rtxtAciklama.Text = "";
            // 
            // chkTutarTamaminiKullan
            // 
            this.chkTutarTamaminiKullan.AutoSize = true;
            this.chkTutarTamaminiKullan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTutarTamaminiKullan.Location = new System.Drawing.Point(123, 61);
            this.chkTutarTamaminiKullan.Name = "chkTutarTamaminiKullan";
            this.chkTutarTamaminiKullan.Size = new System.Drawing.Size(228, 24);
            this.chkTutarTamaminiKullan.TabIndex = 10;
            this.chkTutarTamaminiKullan.Text = "Kalan tutarın tamamını kullan.";
            this.chkTutarTamaminiKullan.UseVisualStyleBackColor = true;
            this.chkTutarTamaminiKullan.CheckedChanged += new System.EventHandler(this.ChkTutarTamaminiKullan_CheckedChanged);
            // 
            // txtEksilenTutar
            // 
            this.txtEksilenTutar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEksilenTutar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEksilenTutar.Location = new System.Drawing.Point(123, 33);
            this.txtEksilenTutar.Name = "txtEksilenTutar";
            this.txtEksilenTutar.Size = new System.Drawing.Size(258, 27);
            this.txtEksilenTutar.TabIndex = 6;
            this.txtEksilenTutar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCikarilacakTutar_KeyPress);
            // 
            // btnHesapla
            // 
            this.btnHesapla.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnHesapla.FlatAppearance.BorderSize = 0;
            this.btnHesapla.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHesapla.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHesapla.ForeColor = System.Drawing.Color.White;
            this.btnHesapla.Location = new System.Drawing.Point(194, 216);
            this.btnHesapla.Name = "btnHesapla";
            this.btnHesapla.Size = new System.Drawing.Size(100, 29);
            this.btnHesapla.TabIndex = 7;
            this.btnHesapla.Text = "Hesapla";
            this.btnHesapla.UseVisualStyleBackColor = false;
            this.btnHesapla.Click += new System.EventHandler(this.btnHesapla_Click);
            // 
            // btnKilometreTasiEkle
            // 
            this.btnKilometreTasiEkle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKilometreTasiEkle.FlatAppearance.BorderSize = 0;
            this.btnKilometreTasiEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKilometreTasiEkle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKilometreTasiEkle.ForeColor = System.Drawing.Color.White;
            this.btnKilometreTasiEkle.Location = new System.Drawing.Point(16, 6);
            this.btnKilometreTasiEkle.Name = "btnKilometreTasiEkle";
            this.btnKilometreTasiEkle.Size = new System.Drawing.Size(146, 48);
            this.btnKilometreTasiEkle.TabIndex = 4;
            this.btnKilometreTasiEkle.Text = "+ Kilometre Taşı Ekle";
            this.btnKilometreTasiEkle.UseVisualStyleBackColor = false;
            this.btnKilometreTasiEkle.Click += new System.EventHandler(this.btnKilometreTasiEkle_Click);
            // 
            // panelUst
            // 
            this.panelUst.BackColor = System.Drawing.Color.White;
            this.panelUst.Controls.Add(this.txtStatu);
            this.panelUst.Controls.Add(this.label1);
            this.panelUst.Controls.Add(this.btnKopyala);
            this.panelUst.Controls.Add(this.btnSil);
            this.panelUst.Controls.Add(this.lblToplamBedelBilgi);
            this.panelUst.Controls.Add(this.txtToplamBedel);
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.btnAra);
            this.panelUst.Controls.Add(this.lblToplamBedel);
            this.panelUst.Controls.Add(this.label2);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 50);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1674, 90);
            this.panelUst.TabIndex = 7;
            // 
            // txtStatu
            // 
            this.txtStatu.BackColor = System.Drawing.Color.White;
            this.txtStatu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatu.Enabled = false;
            this.txtStatu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtStatu.Location = new System.Drawing.Point(203, 40);
            this.txtStatu.Name = "txtStatu";
            this.txtStatu.Size = new System.Drawing.Size(163, 30);
            this.txtStatu.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label1.Location = new System.Drawing.Point(199, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Statü:";
            // 
            // btnKopyala
            // 
            this.btnKopyala.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnKopyala.FlatAppearance.BorderSize = 0;
            this.btnKopyala.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKopyala.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKopyala.ForeColor = System.Drawing.Color.White;
            this.btnKopyala.Location = new System.Drawing.Point(1511, 40);
            this.btnKopyala.Name = "btnKopyala";
            this.btnKopyala.Size = new System.Drawing.Size(149, 31);
            this.btnKopyala.TabIndex = 7;
            this.btnKopyala.Text = "Kopyala";
            this.btnKopyala.UseVisualStyleBackColor = false;
            this.btnKopyala.Click += new System.EventHandler(this.btnKopyala_Click);
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(1356, 40);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(149, 31);
            this.btnSil.TabIndex = 6;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // lblToplamBedelBilgi
            // 
            this.lblToplamBedelBilgi.AutoSize = true;
            this.lblToplamBedelBilgi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblToplamBedelBilgi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblToplamBedelBilgi.Location = new System.Drawing.Point(541, 46);
            this.lblToplamBedelBilgi.Name = "lblToplamBedelBilgi";
            this.lblToplamBedelBilgi.Size = new System.Drawing.Size(0, 20);
            this.lblToplamBedelBilgi.TabIndex = 5;
            // 
            // panelFill
            // 
            this.panelFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelFill.Controls.Add(this.tableLayoutPanel1);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 140);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(1674, 551);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1674, 551);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1674, 50);
            this.ctlBaslik1.TabIndex = 5;
            // 
            // ctlOdemeSartlari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.Controls.Add(this.panelFill);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelAlt);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlOdemeSartlari";
            this.Size = new System.Drawing.Size(1674, 999);
            this.Load += new System.EventHandler(this.ctlOdemeSartlari_Load);
            this.panelAlt.ResumeLayout(false);
            this.panelAlt.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.groupBoxOdemeTutari.ResumeLayout(false);
            this.groupBoxOdemeTutari.PerformLayout();
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelFill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtToplamBedel;
        private System.Windows.Forms.Label lblToplamBedel;
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
        private System.Windows.Forms.Label lblToplamBedelBilgi;
        private System.Windows.Forms.Label lblTeminatBilgi;
        private System.Windows.Forms.RichTextBox rtxtAciklama;
        private System.Windows.Forms.Label lblÖdemeAciklama;
        private System.Windows.Forms.Label lblOdemeTutari;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Label lblOdemeTarihi;
        private System.Windows.Forms.DateTimePicker dtOdemeTarihi;
        private System.Windows.Forms.Button btnKopyala;
        private System.Windows.Forms.TextBox txtStatu;
        private System.Windows.Forms.Label label1;
    }
}