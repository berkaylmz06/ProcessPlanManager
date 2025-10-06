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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tpTeminatMektubu = new System.Windows.Forms.TabControl();
            this.tpTeminatMektubuEkle = new System.Windows.Forms.TabPage();
            this.tlpMainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIadeTarihi = new System.Windows.Forms.Label();
            this.dtIadeTarihi = new System.Windows.Forms.DateTimePicker();
            this.gbKomisyonBilgileri = new System.Windows.Forms.GroupBox();
            this.tlpKomisyon = new System.Windows.Forms.TableLayoutPanel();
            this.cmbKomisyonVadesi = new System.Windows.Forms.ComboBox();
            this.txtKomisyonOrani = new System.Windows.Forms.TextBox();
            this.txtKomisyonTutari = new System.Windows.Forms.TextBox();
            this.lblKomisyonVadesi = new System.Windows.Forms.Label();
            this.lblKomisyonOrani = new System.Windows.Forms.Label();
            this.lblKomisyonTutari = new System.Windows.Forms.Label();
            this.gbMektupTemelBilgileri = new System.Windows.Forms.GroupBox();
            this.tlpTemelBilgiler = new System.Windows.Forms.TableLayoutPanel();
            this.txtMektupNo = new System.Windows.Forms.TextBox();
            this.lblMektupNo = new System.Windows.Forms.Label();
            this.txtMusteriNo = new System.Windows.Forms.TextBox();
            this.llbMusteriNo = new System.Windows.Forms.Label();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.lblMusteriAdi = new System.Windows.Forms.Label();
            this.cmbBankalar = new System.Windows.Forms.ComboBox();
            this.lblBanka = new System.Windows.Forms.Label();
            this.lblMektupTur = new System.Windows.Forms.Label();
            this.cmbMektupTuru = new System.Windows.Forms.ComboBox();
            this.gbTutarVeTarih = new System.Windows.Forms.GroupBox();
            this.tlpTutarTarih = new System.Windows.Forms.TableLayoutPanel();
            this.lblTutar = new System.Windows.Forms.Label();
            this.txtTutar = new System.Windows.Forms.TextBox();
            this.pnlParaBirimi = new System.Windows.Forms.Panel();
            this.chkEuro = new System.Windows.Forms.CheckBox();
            this.chkDolar = new System.Windows.Forms.CheckBox();
            this.chkTL = new System.Windows.Forms.CheckBox();
            this.lblVadeTarihi = new System.Windows.Forms.Label();
            this.dtVadeTarihi = new System.Windows.Forms.DateTimePicker();
            this.tpMektupDagit = new System.Windows.Forms.TabPage();
            this.tlpDagitMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvMektupDagitim = new System.Windows.Forms.DataGridView();
            this.tlpDagitSummary = new System.Windows.Forms.TableLayoutPanel();
            this.lblAnaTutar = new System.Windows.Forms.Label();
            this.txtAnaTutar = new System.Windows.Forms.TextBox();
            this.lblToplamDagitilan = new System.Windows.Forms.Label();
            this.txtToplamDagitilan = new System.Windows.Forms.TextBox();
            this.lblFark = new System.Windows.Forms.Label();
            this.txtFark = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.ProjeNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tutar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpTeminatMektubu.SuspendLayout();
            this.tpTeminatMektubuEkle.SuspendLayout();
            this.tlpMainLayout.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbKomisyonBilgileri.SuspendLayout();
            this.tlpKomisyon.SuspendLayout();
            this.gbMektupTemelBilgileri.SuspendLayout();
            this.tlpTemelBilgiler.SuspendLayout();
            this.gbTutarVeTarih.SuspendLayout();
            this.tlpTutarTarih.SuspendLayout();
            this.pnlParaBirimi.SuspendLayout();
            this.tpMektupDagit.SuspendLayout();
            this.tlpDagitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMektupDagitim)).BeginInit();
            this.tlpDagitSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpTeminatMektubu
            // 
            this.tpTeminatMektubu.Controls.Add(this.tpTeminatMektubuEkle);
            this.tpTeminatMektubu.Controls.Add(this.tpMektupDagit);
            this.tpTeminatMektubu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpTeminatMektubu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tpTeminatMektubu.Location = new System.Drawing.Point(0, 0);
            this.tpTeminatMektubu.Name = "tpTeminatMektubu";
            this.tpTeminatMektubu.SelectedIndex = 0;
            this.tpTeminatMektubu.Size = new System.Drawing.Size(832, 658);
            this.tpTeminatMektubu.TabIndex = 0;
            this.tpTeminatMektubu.SelectedIndexChanged += new System.EventHandler(this.TpTeminatMektubu_SelectedIndexChanged);
            // 
            // tpTeminatMektubuEkle
            // 
            this.tpTeminatMektubuEkle.Controls.Add(this.tlpMainLayout);
            this.tpTeminatMektubuEkle.Location = new System.Drawing.Point(4, 30);
            this.tpTeminatMektubuEkle.Name = "tpTeminatMektubuEkle";
            this.tpTeminatMektubuEkle.Padding = new System.Windows.Forms.Padding(10);
            this.tpTeminatMektubuEkle.Size = new System.Drawing.Size(824, 624);
            this.tpTeminatMektubuEkle.TabIndex = 0;
            this.tpTeminatMektubuEkle.Text = "Teminat Mektubu Bilgileri";
            this.tpTeminatMektubuEkle.UseVisualStyleBackColor = true;
            // 
            // tlpMainLayout
            // 
            this.tlpMainLayout.ColumnCount = 1;
            this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMainLayout.Controls.Add(this.groupBox1, 0, 3);
            this.tlpMainLayout.Controls.Add(this.gbKomisyonBilgileri, 0, 2);
            this.tlpMainLayout.Controls.Add(this.gbMektupTemelBilgileri, 0, 0);
            this.tlpMainLayout.Controls.Add(this.gbTutarVeTarih, 0, 1);
            this.tlpMainLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMainLayout.Location = new System.Drawing.Point(10, 10);
            this.tlpMainLayout.Name = "tlpMainLayout";
            this.tlpMainLayout.RowCount = 4;
            this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpMainLayout.Size = new System.Drawing.Size(804, 570);
            this.tlpMainLayout.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(3, 485);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(798, 82);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "İade Tarihi Bilgisi";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 609F));
            this.tableLayoutPanel1.Controls.Add(this.lblIadeTarihi, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dtIadeTarihi, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(778, 39);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblIadeTarihi
            // 
            this.lblIadeTarihi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblIadeTarihi.AutoSize = true;
            this.lblIadeTarihi.Location = new System.Drawing.Point(74, 8);
            this.lblIadeTarihi.Name = "lblIadeTarihi";
            this.lblIadeTarihi.Size = new System.Drawing.Size(92, 23);
            this.lblIadeTarihi.TabIndex = 14;
            this.lblIadeTarihi.Text = "İade Tarihi:";
            this.lblIadeTarihi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtIadeTarihi
            // 
            this.dtIadeTarihi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtIadeTarihi.Location = new System.Drawing.Point(172, 5);
            this.dtIadeTarihi.Name = "dtIadeTarihi";
            this.dtIadeTarihi.Size = new System.Drawing.Size(603, 29);
            this.dtIadeTarihi.TabIndex = 10;
            // 
            // gbKomisyonBilgileri
            // 
            this.gbKomisyonBilgileri.Controls.Add(this.tlpKomisyon);
            this.gbKomisyonBilgileri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbKomisyonBilgileri.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbKomisyonBilgileri.Location = new System.Drawing.Point(3, 333);
            this.gbKomisyonBilgileri.Name = "gbKomisyonBilgileri";
            this.gbKomisyonBilgileri.Padding = new System.Windows.Forms.Padding(10);
            this.gbKomisyonBilgileri.Size = new System.Drawing.Size(798, 146);
            this.gbKomisyonBilgileri.TabIndex = 2;
            this.gbKomisyonBilgileri.TabStop = false;
            this.gbKomisyonBilgileri.Text = "Komisyon Bilgileri";
            // 
            // tlpKomisyon
            // 
            this.tlpKomisyon.ColumnCount = 2;
            this.tlpKomisyon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tlpKomisyon.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKomisyon.Controls.Add(this.cmbKomisyonVadesi, 1, 2);
            this.tlpKomisyon.Controls.Add(this.txtKomisyonOrani, 1, 0);
            this.tlpKomisyon.Controls.Add(this.txtKomisyonTutari, 1, 1);
            this.tlpKomisyon.Controls.Add(this.lblKomisyonVadesi, 0, 2);
            this.tlpKomisyon.Controls.Add(this.lblKomisyonOrani, 0, 0);
            this.tlpKomisyon.Controls.Add(this.lblKomisyonTutari, 0, 1);
            this.tlpKomisyon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKomisyon.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tlpKomisyon.Location = new System.Drawing.Point(10, 33);
            this.tlpKomisyon.Name = "tlpKomisyon";
            this.tlpKomisyon.RowCount = 3;
            this.tlpKomisyon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpKomisyon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpKomisyon.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpKomisyon.Size = new System.Drawing.Size(778, 103);
            this.tlpKomisyon.TabIndex = 0;
            // 
            // cmbKomisyonVadesi
            // 
            this.cmbKomisyonVadesi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbKomisyonVadesi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKomisyonVadesi.FormattingEnabled = true;
            this.cmbKomisyonVadesi.Location = new System.Drawing.Point(173, 73);
            this.cmbKomisyonVadesi.Name = "cmbKomisyonVadesi";
            this.cmbKomisyonVadesi.Size = new System.Drawing.Size(602, 29);
            this.cmbKomisyonVadesi.TabIndex = 13;
            this.cmbKomisyonVadesi.SelectedIndexChanged += new System.EventHandler(this.cmbKomisyonVadesi_SelectedIndexChanged);
            // 
            // txtKomisyonOrani
            // 
            this.txtKomisyonOrani.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKomisyonOrani.Location = new System.Drawing.Point(173, 3);
            this.txtKomisyonOrani.Name = "txtKomisyonOrani";
            this.txtKomisyonOrani.Size = new System.Drawing.Size(602, 29);
            this.txtKomisyonOrani.TabIndex = 12;
            this.txtKomisyonOrani.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtKomisyonOrani.TextChanged += new System.EventHandler(this.TxtKomisyonOrani_TextChanged);
            // 
            // txtKomisyonTutari
            // 
            this.txtKomisyonTutari.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKomisyonTutari.BackColor = System.Drawing.SystemColors.Info;
            this.txtKomisyonTutari.Location = new System.Drawing.Point(173, 37);
            this.txtKomisyonTutari.Name = "txtKomisyonTutari";
            this.txtKomisyonTutari.ReadOnly = true;
            this.txtKomisyonTutari.Size = new System.Drawing.Size(602, 29);
            this.txtKomisyonTutari.TabIndex = 11;
            this.txtKomisyonTutari.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblKomisyonVadesi
            // 
            this.lblKomisyonVadesi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblKomisyonVadesi.AutoSize = true;
            this.lblKomisyonVadesi.Location = new System.Drawing.Point(25, 74);
            this.lblKomisyonVadesi.Name = "lblKomisyonVadesi";
            this.lblKomisyonVadesi.Size = new System.Drawing.Size(142, 23);
            this.lblKomisyonVadesi.TabIndex = 19;
            this.lblKomisyonVadesi.Text = "Komisyon Vadesi:";
            this.lblKomisyonVadesi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKomisyonOrani
            // 
            this.lblKomisyonOrani.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblKomisyonOrani.AutoSize = true;
            this.lblKomisyonOrani.Location = new System.Drawing.Point(32, 5);
            this.lblKomisyonOrani.Name = "lblKomisyonOrani";
            this.lblKomisyonOrani.Size = new System.Drawing.Size(135, 23);
            this.lblKomisyonOrani.TabIndex = 17;
            this.lblKomisyonOrani.Text = "Komisyon Oranı:";
            this.lblKomisyonOrani.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKomisyonTutari
            // 
            this.lblKomisyonTutari.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblKomisyonTutari.AutoSize = true;
            this.lblKomisyonTutari.Location = new System.Drawing.Point(30, 39);
            this.lblKomisyonTutari.Name = "lblKomisyonTutari";
            this.lblKomisyonTutari.Size = new System.Drawing.Size(137, 23);
            this.lblKomisyonTutari.TabIndex = 15;
            this.lblKomisyonTutari.Text = "Komisyon Tutarı:";
            this.lblKomisyonTutari.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbMektupTemelBilgileri
            // 
            this.gbMektupTemelBilgileri.Controls.Add(this.tlpTemelBilgiler);
            this.gbMektupTemelBilgileri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMektupTemelBilgileri.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbMektupTemelBilgileri.Location = new System.Drawing.Point(3, 3);
            this.gbMektupTemelBilgileri.Name = "gbMektupTemelBilgileri";
            this.gbMektupTemelBilgileri.Padding = new System.Windows.Forms.Padding(10);
            this.gbMektupTemelBilgileri.Size = new System.Drawing.Size(798, 209);
            this.gbMektupTemelBilgileri.TabIndex = 0;
            this.gbMektupTemelBilgileri.TabStop = false;
            this.gbMektupTemelBilgileri.Text = "Mektup ve Müşteri Bilgileri";
            // 
            // tlpTemelBilgiler
            // 
            this.tlpTemelBilgiler.ColumnCount = 2;
            this.tlpTemelBilgiler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpTemelBilgiler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTemelBilgiler.Controls.Add(this.txtMektupNo, 1, 0);
            this.tlpTemelBilgiler.Controls.Add(this.lblMektupNo, 0, 0);
            this.tlpTemelBilgiler.Controls.Add(this.txtMusteriNo, 1, 1);
            this.tlpTemelBilgiler.Controls.Add(this.llbMusteriNo, 0, 1);
            this.tlpTemelBilgiler.Controls.Add(this.txtMusteriAdi, 1, 2);
            this.tlpTemelBilgiler.Controls.Add(this.lblMusteriAdi, 0, 2);
            this.tlpTemelBilgiler.Controls.Add(this.cmbBankalar, 1, 3);
            this.tlpTemelBilgiler.Controls.Add(this.lblBanka, 0, 3);
            this.tlpTemelBilgiler.Controls.Add(this.lblMektupTur, 0, 4);
            this.tlpTemelBilgiler.Controls.Add(this.cmbMektupTuru, 1, 4);
            this.tlpTemelBilgiler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTemelBilgiler.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tlpTemelBilgiler.Location = new System.Drawing.Point(10, 33);
            this.tlpTemelBilgiler.Name = "tlpTemelBilgiler";
            this.tlpTemelBilgiler.RowCount = 5;
            this.tlpTemelBilgiler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTemelBilgiler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTemelBilgiler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTemelBilgiler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTemelBilgiler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpTemelBilgiler.Size = new System.Drawing.Size(778, 166);
            this.tlpTemelBilgiler.TabIndex = 0;
            // 
            // txtMektupNo
            // 
            this.txtMektupNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMektupNo.Location = new System.Drawing.Point(153, 3);
            this.txtMektupNo.Name = "txtMektupNo";
            this.txtMektupNo.Size = new System.Drawing.Size(622, 29);
            this.txtMektupNo.TabIndex = 1;
            // 
            // lblMektupNo
            // 
            this.lblMektupNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMektupNo.AutoSize = true;
            this.lblMektupNo.Location = new System.Drawing.Point(47, 5);
            this.lblMektupNo.Name = "lblMektupNo";
            this.lblMektupNo.Size = new System.Drawing.Size(100, 23);
            this.lblMektupNo.TabIndex = 1;
            this.lblMektupNo.Text = "Mektup No:";
            this.lblMektupNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMusteriNo.Location = new System.Drawing.Point(153, 36);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(622, 29);
            this.txtMusteriNo.TabIndex = 2;
            this.txtMusteriNo.Leave += new System.EventHandler(this.txtMusteriNo_Leave);
            // 
            // llbMusteriNo
            // 
            this.llbMusteriNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.llbMusteriNo.AutoSize = true;
            this.llbMusteriNo.Location = new System.Drawing.Point(48, 38);
            this.llbMusteriNo.Name = "llbMusteriNo";
            this.llbMusteriNo.Size = new System.Drawing.Size(99, 23);
            this.llbMusteriNo.TabIndex = 20;
            this.llbMusteriNo.Text = "Müşteri No:";
            this.llbMusteriNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMusteriAdi.BackColor = System.Drawing.SystemColors.Info;
            this.txtMusteriAdi.Location = new System.Drawing.Point(153, 69);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.ReadOnly = true;
            this.txtMusteriAdi.Size = new System.Drawing.Size(622, 29);
            this.txtMusteriAdi.TabIndex = 3;
            this.txtMusteriAdi.TabStop = false;
            // 
            // lblMusteriAdi
            // 
            this.lblMusteriAdi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMusteriAdi.AutoSize = true;
            this.lblMusteriAdi.Location = new System.Drawing.Point(46, 71);
            this.lblMusteriAdi.Name = "lblMusteriAdi";
            this.lblMusteriAdi.Size = new System.Drawing.Size(101, 23);
            this.lblMusteriAdi.TabIndex = 21;
            this.lblMusteriAdi.Text = "Müşteri Adı:";
            this.lblMusteriAdi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBankalar
            // 
            this.cmbBankalar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBankalar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankalar.FormattingEnabled = true;
            this.cmbBankalar.Location = new System.Drawing.Point(153, 103);
            this.cmbBankalar.Name = "cmbBankalar";
            this.cmbBankalar.Size = new System.Drawing.Size(622, 29);
            this.cmbBankalar.TabIndex = 4;
            // 
            // lblBanka
            // 
            this.lblBanka.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBanka.AutoSize = true;
            this.lblBanka.Location = new System.Drawing.Point(87, 104);
            this.lblBanka.Name = "lblBanka";
            this.lblBanka.Size = new System.Drawing.Size(60, 23);
            this.lblBanka.TabIndex = 7;
            this.lblBanka.Text = "Banka:";
            this.lblBanka.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMektupTur
            // 
            this.lblMektupTur.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMektupTur.AutoSize = true;
            this.lblMektupTur.Location = new System.Drawing.Point(35, 137);
            this.lblMektupTur.Name = "lblMektupTur";
            this.lblMektupTur.Size = new System.Drawing.Size(112, 23);
            this.lblMektupTur.TabIndex = 8;
            this.lblMektupTur.Text = "Mektup Türü:";
            this.lblMektupTur.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMektupTuru
            // 
            this.cmbMektupTuru.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMektupTuru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMektupTuru.FormattingEnabled = true;
            this.cmbMektupTuru.Location = new System.Drawing.Point(153, 137);
            this.cmbMektupTuru.Name = "cmbMektupTuru";
            this.cmbMektupTuru.Size = new System.Drawing.Size(622, 29);
            this.cmbMektupTuru.TabIndex = 5;
            this.cmbMektupTuru.SelectedIndexChanged += new System.EventHandler(this.CmbMektupTuru_SelectedIndexChanged);
            // 
            // gbTutarVeTarih
            // 
            this.gbTutarVeTarih.Controls.Add(this.tlpTutarTarih);
            this.gbTutarVeTarih.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTutarVeTarih.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.gbTutarVeTarih.Location = new System.Drawing.Point(3, 218);
            this.gbTutarVeTarih.Name = "gbTutarVeTarih";
            this.gbTutarVeTarih.Padding = new System.Windows.Forms.Padding(10);
            this.gbTutarVeTarih.Size = new System.Drawing.Size(798, 109);
            this.gbTutarVeTarih.TabIndex = 1;
            this.gbTutarVeTarih.TabStop = false;
            this.gbTutarVeTarih.Text = "Tutar ve Tarih Bilgileri";
            // 
            // tlpTutarTarih
            // 
            this.tlpTutarTarih.ColumnCount = 3;
            this.tlpTutarTarih.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.tlpTutarTarih.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTutarTarih.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTutarTarih.Controls.Add(this.lblTutar, 0, 0);
            this.tlpTutarTarih.Controls.Add(this.txtTutar, 1, 0);
            this.tlpTutarTarih.Controls.Add(this.pnlParaBirimi, 2, 0);
            this.tlpTutarTarih.Controls.Add(this.lblVadeTarihi, 0, 1);
            this.tlpTutarTarih.Controls.Add(this.dtVadeTarihi, 1, 1);
            this.tlpTutarTarih.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTutarTarih.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tlpTutarTarih.Location = new System.Drawing.Point(10, 33);
            this.tlpTutarTarih.Name = "tlpTutarTarih";
            this.tlpTutarTarih.RowCount = 2;
            this.tlpTutarTarih.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpTutarTarih.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpTutarTarih.Size = new System.Drawing.Size(778, 66);
            this.tlpTutarTarih.TabIndex = 0;
            // 
            // lblTutar
            // 
            this.lblTutar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(112, 5);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(54, 23);
            this.lblTutar.TabIndex = 6;
            this.lblTutar.Text = "Tutar:";
            this.lblTutar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTutar
            // 
            this.txtTutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTutar.Location = new System.Drawing.Point(172, 3);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(298, 29);
            this.txtTutar.TabIndex = 6;
            this.txtTutar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTutar.TextChanged += new System.EventHandler(this.TxtTutar_TextChanged);
            // 
            // pnlParaBirimi
            // 
            this.pnlParaBirimi.Controls.Add(this.chkEuro);
            this.pnlParaBirimi.Controls.Add(this.chkDolar);
            this.pnlParaBirimi.Controls.Add(this.chkTL);
            this.pnlParaBirimi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParaBirimi.Location = new System.Drawing.Point(476, 3);
            this.pnlParaBirimi.Name = "pnlParaBirimi";
            this.pnlParaBirimi.Size = new System.Drawing.Size(299, 27);
            this.pnlParaBirimi.TabIndex = 22;
            // 
            // chkEuro
            // 
            this.chkEuro.AutoSize = true;
            this.chkEuro.Location = new System.Drawing.Point(137, 4);
            this.chkEuro.Name = "chkEuro";
            this.chkEuro.Size = new System.Drawing.Size(67, 27);
            this.chkEuro.TabIndex = 9;
            this.chkEuro.Text = "Euro";
            this.chkEuro.UseVisualStyleBackColor = true;
            this.chkEuro.CheckedChanged += new System.EventHandler(this.chkEuro_CheckedChanged);
            // 
            // chkDolar
            // 
            this.chkDolar.AutoSize = true;
            this.chkDolar.Location = new System.Drawing.Point(71, 4);
            this.chkDolar.Name = "chkDolar";
            this.chkDolar.Size = new System.Drawing.Size(73, 27);
            this.chkDolar.TabIndex = 8;
            this.chkDolar.Text = "Dolar";
            this.chkDolar.UseVisualStyleBackColor = true;
            this.chkDolar.CheckedChanged += new System.EventHandler(this.chkDolar_CheckedChanged);
            // 
            // chkTL
            // 
            this.chkTL.AutoSize = true;
            this.chkTL.Checked = true;
            this.chkTL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTL.Location = new System.Drawing.Point(18, 4);
            this.chkTL.Name = "chkTL";
            this.chkTL.Size = new System.Drawing.Size(49, 27);
            this.chkTL.TabIndex = 7;
            this.chkTL.Text = "TL";
            this.chkTL.UseVisualStyleBackColor = true;
            this.chkTL.CheckedChanged += new System.EventHandler(this.chkTL_CheckedChanged);
            // 
            // lblVadeTarihi
            // 
            this.lblVadeTarihi.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblVadeTarihi.AutoSize = true;
            this.lblVadeTarihi.Location = new System.Drawing.Point(69, 38);
            this.lblVadeTarihi.Name = "lblVadeTarihi";
            this.lblVadeTarihi.Size = new System.Drawing.Size(97, 23);
            this.lblVadeTarihi.TabIndex = 10;
            this.lblVadeTarihi.Text = "Vade Tarihi:";
            this.lblVadeTarihi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtVadeTarihi
            // 
            this.dtVadeTarihi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtVadeTarihi.Location = new System.Drawing.Point(172, 36);
            this.dtVadeTarihi.Name = "dtVadeTarihi";
            this.dtVadeTarihi.Size = new System.Drawing.Size(298, 29);
            this.dtVadeTarihi.TabIndex = 9;
            this.dtVadeTarihi.ValueChanged += new System.EventHandler(this.dtVadeTarihi_ValueChanged);
            // 
            // tpMektupDagit
            // 
            this.tpMektupDagit.Controls.Add(this.tlpDagitMain);
            this.tpMektupDagit.Location = new System.Drawing.Point(4, 30);
            this.tpMektupDagit.Name = "tpMektupDagit";
            this.tpMektupDagit.Padding = new System.Windows.Forms.Padding(10);
            this.tpMektupDagit.Size = new System.Drawing.Size(824, 624);
            this.tpMektupDagit.TabIndex = 1;
            this.tpMektupDagit.Text = "Mektup Dağılımı (Alt Projeler)";
            this.tpMektupDagit.UseVisualStyleBackColor = true;
            // 
            // tlpDagitMain
            // 
            this.tlpDagitMain.ColumnCount = 1;
            this.tlpDagitMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDagitMain.Controls.Add(this.dgvMektupDagitim, 0, 0);
            this.tlpDagitMain.Controls.Add(this.tlpDagitSummary, 0, 1);
            this.tlpDagitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDagitMain.Location = new System.Drawing.Point(10, 10);
            this.tlpDagitMain.Name = "tlpDagitMain";
            this.tlpDagitMain.RowCount = 2;
            this.tlpDagitMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDagitMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpDagitMain.Size = new System.Drawing.Size(804, 604);
            this.tlpDagitMain.TabIndex = 0;
            // 
            // dgvMektupDagitim
            // 
            this.dgvMektupDagitim.AllowUserToAddRows = false;
            this.dgvMektupDagitim.AllowUserToDeleteRows = false;
            this.dgvMektupDagitim.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMektupDagitim.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMektupDagitim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMektupDagitim.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjeNo,
            this.Tutar});
            this.dgvMektupDagitim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMektupDagitim.Location = new System.Drawing.Point(3, 3);
            this.dgvMektupDagitim.Name = "dgvMektupDagitim";
            this.dgvMektupDagitim.RowHeadersWidth = 25;
            this.dgvMektupDagitim.RowTemplate.Height = 24;
            this.dgvMektupDagitim.Size = new System.Drawing.Size(798, 498);
            this.dgvMektupDagitim.TabIndex = 0;
            this.dgvMektupDagitim.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DgvMektupDagitim_CellValidating);
            this.dgvMektupDagitim.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvMektupDagitim_CellValueChanged);
            // 
            // tlpDagitSummary
            // 
            this.tlpDagitSummary.ColumnCount = 6;
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpDagitSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpDagitSummary.Controls.Add(this.lblAnaTutar, 0, 0);
            this.tlpDagitSummary.Controls.Add(this.txtAnaTutar, 1, 0);
            this.tlpDagitSummary.Controls.Add(this.lblToplamDagitilan, 2, 0);
            this.tlpDagitSummary.Controls.Add(this.txtToplamDagitilan, 3, 0);
            this.tlpDagitSummary.Controls.Add(this.lblFark, 4, 0);
            this.tlpDagitSummary.Controls.Add(this.txtFark, 5, 0);
            this.tlpDagitSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDagitSummary.Location = new System.Drawing.Point(3, 507);
            this.tlpDagitSummary.Name = "tlpDagitSummary";
            this.tlpDagitSummary.RowCount = 1;
            this.tlpDagitSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDagitSummary.Size = new System.Drawing.Size(798, 94);
            this.tlpDagitSummary.TabIndex = 1;
            // 
            // lblAnaTutar
            // 
            this.lblAnaTutar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAnaTutar.AutoSize = true;
            this.lblAnaTutar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAnaTutar.Location = new System.Drawing.Point(53, 35);
            this.lblAnaTutar.Name = "lblAnaTutar";
            this.lblAnaTutar.Size = new System.Drawing.Size(94, 23);
            this.lblAnaTutar.TabIndex = 0;
            this.lblAnaTutar.Text = "Ana Tutar:";
            this.lblAnaTutar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAnaTutar
            // 
            this.txtAnaTutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAnaTutar.BackColor = System.Drawing.Color.LightCyan;
            this.txtAnaTutar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtAnaTutar.Location = new System.Drawing.Point(153, 32);
            this.txtAnaTutar.Name = "txtAnaTutar";
            this.txtAnaTutar.ReadOnly = true;
            this.txtAnaTutar.Size = new System.Drawing.Size(110, 29);
            this.txtAnaTutar.TabIndex = 1;
            this.txtAnaTutar.TabStop = false;
            this.txtAnaTutar.Text = "0,00";
            this.txtAnaTutar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblToplamDagitilan
            // 
            this.lblToplamDagitilan.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblToplamDagitilan.AutoSize = true;
            this.lblToplamDagitilan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamDagitilan.Location = new System.Drawing.Point(324, 24);
            this.lblToplamDagitilan.Name = "lblToplamDagitilan";
            this.lblToplamDagitilan.Size = new System.Drawing.Size(89, 46);
            this.lblToplamDagitilan.TabIndex = 2;
            this.lblToplamDagitilan.Text = "Toplam Dağıtılan:";
            this.lblToplamDagitilan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtToplamDagitilan
            // 
            this.txtToplamDagitilan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToplamDagitilan.BackColor = System.Drawing.Color.MistyRose;
            this.txtToplamDagitilan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtToplamDagitilan.Location = new System.Drawing.Point(419, 32);
            this.txtToplamDagitilan.Name = "txtToplamDagitilan";
            this.txtToplamDagitilan.ReadOnly = true;
            this.txtToplamDagitilan.Size = new System.Drawing.Size(110, 29);
            this.txtToplamDagitilan.TabIndex = 3;
            this.txtToplamDagitilan.TabStop = false;
            this.txtToplamDagitilan.Text = "0,00";
            this.txtToplamDagitilan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblFark
            // 
            this.lblFark.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFark.AutoSize = true;
            this.lblFark.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblFark.Location = new System.Drawing.Point(629, 35);
            this.lblFark.Name = "lblFark";
            this.lblFark.Size = new System.Drawing.Size(50, 23);
            this.lblFark.TabIndex = 4;
            this.lblFark.Text = "Fark:";
            this.lblFark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFark
            // 
            this.txtFark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFark.BackColor = System.Drawing.Color.LightCoral;
            this.txtFark.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtFark.Location = new System.Drawing.Point(685, 32);
            this.txtFark.Name = "txtFark";
            this.txtFark.ReadOnly = true;
            this.txtFark.Size = new System.Drawing.Size(110, 29);
            this.txtFark.TabIndex = 5;
            this.txtFark.TabStop = false;
            this.txtFark.Text = "0,00";
            this.txtFark.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(689, 5);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(140, 35);
            this.btnKaydet.TabIndex = 14;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnKaydet);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 615);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 43);
            this.panel1.TabIndex = 15;
            // 
            // ProjeNo
            // 
            this.ProjeNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProjeNo.HeaderText = "Alt Proje";
            this.ProjeNo.MinimumWidth = 6;
            this.ProjeNo.Name = "ProjeNo";
            this.ProjeNo.ReadOnly = true;
            // 
            // Tutar
            // 
            this.Tutar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0,00";
            this.Tutar.DefaultCellStyle = dataGridViewCellStyle2;
            this.Tutar.HeaderText = "Dağıtılan Tutar";
            this.Tutar.MinimumWidth = 6;
            this.Tutar.Name = "Tutar";
            // 
            // frmTeminatMektubuEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 658);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tpTeminatMektubu);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmTeminatMektubuEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teminat Mektubu İşlemleri";
            this.Load += new System.EventHandler(this.frmTeminatMektubuEkle_Load);
            this.tpTeminatMektubu.ResumeLayout(false);
            this.tpTeminatMektubuEkle.ResumeLayout(false);
            this.tlpMainLayout.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbKomisyonBilgileri.ResumeLayout(false);
            this.tlpKomisyon.ResumeLayout(false);
            this.tlpKomisyon.PerformLayout();
            this.gbMektupTemelBilgileri.ResumeLayout(false);
            this.tlpTemelBilgiler.ResumeLayout(false);
            this.tlpTemelBilgiler.PerformLayout();
            this.gbTutarVeTarih.ResumeLayout(false);
            this.tlpTutarTarih.ResumeLayout(false);
            this.tlpTutarTarih.PerformLayout();
            this.pnlParaBirimi.ResumeLayout(false);
            this.pnlParaBirimi.PerformLayout();
            this.tpMektupDagit.ResumeLayout(false);
            this.tlpDagitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMektupDagitim)).EndInit();
            this.tlpDagitSummary.ResumeLayout(false);
            this.tlpDagitSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabControl tpTeminatMektubu;
        private System.Windows.Forms.TabPage tpTeminatMektubuEkle;
        private System.Windows.Forms.TabPage tpMektupDagit;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TableLayoutPanel tlpMainLayout;
        private System.Windows.Forms.GroupBox gbKomisyonBilgileri;
        private System.Windows.Forms.GroupBox gbMektupTemelBilgileri;
        private System.Windows.Forms.GroupBox gbTutarVeTarih;
        private System.Windows.Forms.TableLayoutPanel tlpTemelBilgiler;
        private System.Windows.Forms.TableLayoutPanel tlpKomisyon;
        private System.Windows.Forms.TableLayoutPanel tlpTutarTarih;
        private System.Windows.Forms.Panel pnlParaBirimi;
        private System.Windows.Forms.TableLayoutPanel tlpDagitMain;
        private System.Windows.Forms.DataGridView dgvMektupDagitim;
        private System.Windows.Forms.TableLayoutPanel tlpDagitSummary;
        private System.Windows.Forms.Label lblAnaTutar;
        private System.Windows.Forms.TextBox txtAnaTutar;
        private System.Windows.Forms.Label lblToplamDagitilan;
        private System.Windows.Forms.TextBox txtToplamDagitilan;
        private System.Windows.Forms.Label lblFark;
        private System.Windows.Forms.TextBox txtFark;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjeNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tutar;
    }
}