namespace CEKA_APP.UserControls.ProjeTakip
{
    partial class ctlProjeKarti
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Yeni eklenen TableLayoutPanel değişkeni
        private System.Windows.Forms.TableLayoutPanel tlpProjeBilgileri;

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
            this.panelUst = new System.Windows.Forms.Panel();
            this.panelUstSag = new System.Windows.Forms.Panel();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnStatuBilgisi = new System.Windows.Forms.Button();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblToplamBedelBilgi = new System.Windows.Forms.Label();
            this.txtProjeAra = new System.Windows.Forms.TextBox();
            this.lblAraProje = new System.Windows.Forms.Label();
            this.txtProjeBasTarihi = new System.Windows.Forms.TextBox();
            this.lblProjeBasTarihi = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbProjeBilgileri = new System.Windows.Forms.TabPage();
            this.tlpProjeBilgileri = new System.Windows.Forms.TableLayoutPanel(); // YENİ EKLENDİ
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRefProje = new System.Windows.Forms.TextBox();
            this.lblProjeMuh = new System.Windows.Forms.Label();
            this.txtProjeMuh = new System.Windows.Forms.TextBox();
            this.lblReferansProje = new System.Windows.Forms.Label();
            this.chkRefProjeYok = new System.Windows.Forms.CheckBox();
            this.chkRefProjeVar = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.lblMusteriNo = new System.Windows.Forms.Label();
            this.txtMusteriNo = new System.Windows.Forms.TextBox();
            this.lblMusteriAdi = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProjeBitisTarihi = new System.Windows.Forms.TextBox();
            this.lblProjeBitisTarihi = new System.Windows.Forms.Label();
            this.txtProjeAdi = new System.Windows.Forms.TextBox();
            this.lblProjeAdi = new System.Windows.Forms.Label();
            this.lblProjeNo = new System.Windows.Forms.Label();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.btnUrunGrubuEkle = new System.Windows.Forms.Button();
            this.tbUrunGruplari = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridUstGruplar = new System.Windows.Forms.DataGridView();
            this.dataGridUrunGruplari = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelUst.SuspendLayout();
            this.panelUstSag.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbProjeBilgileri.SuspendLayout();
            this.tlpProjeBilgileri.SuspendLayout(); // YENİ EKLENDİ
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbUrunGruplari.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUstGruplar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUrunGruplari)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUst
            // 
            this.panelUst.BackColor = System.Drawing.Color.White;
            this.panelUst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUst.Controls.Add(this.panelUstSag);
            this.panelUst.Controls.Add(this.txtStatus);
            this.panelUst.Controls.Add(this.lblStatus);
            this.panelUst.Controls.Add(this.lblToplamBedelBilgi);
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.lblAraProje);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 63);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1640, 84);
            this.panelUst.TabIndex = 330;
            // 
            // panelUstSag
            // 
            this.panelUstSag.Controls.Add(this.btnKaydet);
            this.panelUstSag.Controls.Add(this.btnStatuBilgisi);
            this.panelUstSag.Controls.Add(this.btnTemizle);
            this.panelUstSag.Controls.Add(this.btnSil);
            this.panelUstSag.Controls.Add(this.btnAra);
            this.panelUstSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelUstSag.Location = new System.Drawing.Point(1291, 0);
            this.panelUstSag.Name = "panelUstSag";
            this.panelUstSag.Size = new System.Drawing.Size(347, 82);
            this.panelUstSag.TabIndex = 331;
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(236, 8);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(100, 28);
            this.btnKaydet.TabIndex = 330;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            // 
            // btnStatuBilgisi
            // 
            this.btnStatuBilgisi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStatuBilgisi.FlatAppearance.BorderSize = 0;
            this.btnStatuBilgisi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatuBilgisi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStatuBilgisi.ForeColor = System.Drawing.Color.White;
            this.btnStatuBilgisi.Location = new System.Drawing.Point(24, 42);
            this.btnStatuBilgisi.Name = "btnStatuBilgisi";
            this.btnStatuBilgisi.Size = new System.Drawing.Size(100, 28);
            this.btnStatuBilgisi.TabIndex = 329;
            this.btnStatuBilgisi.Text = "Statü Bilgi";
            this.btnStatuBilgisi.UseVisualStyleBackColor = false;
            // 
            // btnTemizle
            // 
            this.btnTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTemizle.FlatAppearance.BorderSize = 0;
            this.btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemizle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTemizle.ForeColor = System.Drawing.Color.White;
            this.btnTemizle.Location = new System.Drawing.Point(130, 42);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(100, 28);
            this.btnTemizle.TabIndex = 328;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = false;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(130, 8);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(100, 28);
            this.btnSil.TabIndex = 326;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.Location = new System.Drawing.Point(24, 8);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(100, 28);
            this.btnAra.TabIndex = 316;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = false;
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.White;
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatus.Enabled = false;
            this.txtStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtStatus.Location = new System.Drawing.Point(235, 39);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(177, 27);
            this.txtStatus.TabIndex = 329;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblStatus.Location = new System.Drawing.Point(231, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 23);
            this.lblStatus.TabIndex = 330;
            this.lblStatus.Text = "Statü:";
            // 
            // lblToplamBedelBilgi
            // 
            this.lblToplamBedelBilgi.AutoSize = true;
            this.lblToplamBedelBilgi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblToplamBedelBilgi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblToplamBedelBilgi.Location = new System.Drawing.Point(1187, 60);
            this.lblToplamBedelBilgi.Name = "lblToplamBedelBilgi";
            this.lblToplamBedelBilgi.Size = new System.Drawing.Size(0, 20);
            this.lblToplamBedelBilgi.TabIndex = 5;
            // 
            // txtProjeAra
            // 
            this.txtProjeAra.BackColor = System.Drawing.Color.White;
            this.txtProjeAra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeAra.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeAra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeAra.Location = new System.Drawing.Point(37, 39);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(177, 27);
            this.txtProjeAra.TabIndex = 315;
            // 
            // lblAraProje
            // 
            this.lblAraProje.AutoSize = true;
            this.lblAraProje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAraProje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblAraProje.Location = new System.Drawing.Point(33, 13);
            this.lblAraProje.Name = "lblAraProje";
            this.lblAraProje.Size = new System.Drawing.Size(81, 23);
            this.lblAraProje.TabIndex = 317;
            this.lblAraProje.Text = "Proje No:";
            // 
            // txtProjeBasTarihi
            // 
            this.txtProjeBasTarihi.BackColor = System.Drawing.Color.White;
            this.txtProjeBasTarihi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeBasTarihi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeBasTarihi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeBasTarihi.Location = new System.Drawing.Point(320, 157);
            this.txtProjeBasTarihi.Name = "txtProjeBasTarihi";
            this.txtProjeBasTarihi.Size = new System.Drawing.Size(369, 27);
            this.txtProjeBasTarihi.TabIndex = 331;
            // 
            // lblProjeBasTarihi
            // 
            this.lblProjeBasTarihi.AutoSize = true;
            this.lblProjeBasTarihi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjeBasTarihi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblProjeBasTarihi.Location = new System.Drawing.Point(30, 157);
            this.lblProjeBasTarihi.Name = "lblProjeBasTarihi";
            this.lblProjeBasTarihi.Size = new System.Drawing.Size(173, 23);
            this.lblProjeBasTarihi.TabIndex = 332;
            this.lblProjeBasTarihi.Text = "Proje başlangıç tarihi:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbProjeBilgileri);
            this.tabControl1.Controls.Add(this.tbUrunGruplari);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 147);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1640, 875);
            this.tabControl1.TabIndex = 333;
            // 
            // tbProjeBilgileri
            // 
            this.tbProjeBilgileri.Controls.Add(this.tlpProjeBilgileri); // tlpProjeBilgileri'ni ekle
            this.tbProjeBilgileri.Location = new System.Drawing.Point(4, 25);
            this.tbProjeBilgileri.Name = "tbProjeBilgileri";
            this.tbProjeBilgileri.Padding = new System.Windows.Forms.Padding(3);
            this.tbProjeBilgileri.Size = new System.Drawing.Size(1632, 846);
            this.tbProjeBilgileri.TabIndex = 0;
            this.tbProjeBilgileri.Text = "Proje Bilgileri";
            this.tbProjeBilgileri.UseVisualStyleBackColor = true;
            // 
            // tlpProjeBilgileri
            // 
            this.tlpProjeBilgileri.ColumnCount = 1;
            this.tlpProjeBilgileri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProjeBilgileri.Controls.Add(this.groupBox1, 0, 0);
            this.tlpProjeBilgileri.Controls.Add(this.groupBox2, 0, 1);
            this.tlpProjeBilgileri.Controls.Add(this.groupBox3, 0, 2);
            this.tlpProjeBilgileri.Controls.Add(this.btnUrunGrubuEkle, 0, 3);
            this.tlpProjeBilgileri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProjeBilgileri.Location = new System.Drawing.Point(3, 3);
            this.tlpProjeBilgileri.Name = "tlpProjeBilgileri";
            this.tlpProjeBilgileri.RowCount = 4;
            this.tlpProjeBilgileri.RowStyles.Add(new System.Windows.Forms.RowStyle()); // groupBox1 için otomatik boyut
            this.tlpProjeBilgileri.RowStyles.Add(new System.Windows.Forms.RowStyle()); // groupBox2 için otomatik boyut
            this.tlpProjeBilgileri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // groupBox3 için kalan alanı kullan
            this.tlpProjeBilgileri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F)); // buton için sabit yükseklik
            this.tlpProjeBilgileri.Size = new System.Drawing.Size(1626, 840);
            this.tlpProjeBilgileri.TabIndex = 352;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtRefProje);
            this.groupBox3.Controls.Add(this.lblProjeMuh);
            this.groupBox3.Controls.Add(this.txtProjeMuh);
            this.groupBox3.Controls.Add(this.lblReferansProje);
            this.groupBox3.Controls.Add(this.chkRefProjeYok);
            this.groupBox3.Controls.Add(this.chkRefProjeVar);
            this.groupBox3.Location = new System.Drawing.Point(3, 497);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(715, 172); // Size'ı koruyoruz, ancak yükseklik dinamikleşecek
            this.groupBox3.TabIndex = 351;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Genel";
            // 
            // txtRefProje
            // 
            this.txtRefProje.BackColor = System.Drawing.Color.White;
            this.txtRefProje.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRefProje.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRefProje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtRefProje.Location = new System.Drawing.Point(512, 95);
            this.txtRefProje.Name = "txtRefProje";
            this.txtRefProje.Size = new System.Drawing.Size(177, 27);
            this.txtRefProje.TabIndex = 339;
            this.txtRefProje.Visible = false;
            // 
            // lblProjeMuh
            // 
            this.lblProjeMuh.AutoSize = true;
            this.lblProjeMuh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjeMuh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblProjeMuh.Location = new System.Drawing.Point(30, 52);
            this.lblProjeMuh.Name = "lblProjeMuh";
            this.lblProjeMuh.Size = new System.Drawing.Size(137, 23);
            this.lblProjeMuh.TabIndex = 338;
            this.lblProjeMuh.Text = "Proje Mühendisi:";
            // 
            // txtProjeMuh
            // 
            this.txtProjeMuh.BackColor = System.Drawing.Color.White;
            this.txtProjeMuh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeMuh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeMuh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeMuh.Location = new System.Drawing.Point(320, 52);
            this.txtProjeMuh.Name = "txtProjeMuh";
            this.txtProjeMuh.Size = new System.Drawing.Size(369, 27);
            this.txtProjeMuh.TabIndex = 337;
            // 
            // lblReferansProje
            // 
            this.lblReferansProje.AutoSize = true;
            this.lblReferansProje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblReferansProje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblReferansProje.Location = new System.Drawing.Point(30, 102);
            this.lblReferansProje.Name = "lblReferansProje";
            this.lblReferansProje.Size = new System.Drawing.Size(179, 23);
            this.lblReferansProje.TabIndex = 340;
            this.lblReferansProje.Text = "Referans proje var mı?";
            // 
            // chkRefProjeYok
            // 
            this.chkRefProjeYok.AutoSize = true;
            this.chkRefProjeYok.Location = new System.Drawing.Point(440, 102);
            this.chkRefProjeYok.Name = "chkRefProjeYok";
            this.chkRefProjeYok.Size = new System.Drawing.Size(53, 20);
            this.chkRefProjeYok.TabIndex = 342;
            this.chkRefProjeYok.Text = "Yok";
            this.chkRefProjeYok.UseVisualStyleBackColor = true;
            // 
            // chkRefProjeVar
            // 
            this.chkRefProjeVar.AutoSize = true;
            this.chkRefProjeVar.Location = new System.Drawing.Point(340, 102);
            this.chkRefProjeVar.Name = "chkRefProjeVar";
            this.chkRefProjeVar.Size = new System.Drawing.Size(50, 20);
            this.chkRefProjeVar.TabIndex = 341;
            this.chkRefProjeVar.Text = "Var";
            this.chkRefProjeVar.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMusteriAdi);
            this.groupBox2.Controls.Add(this.lblMusteriNo);
            this.groupBox2.Controls.Add(this.txtMusteriNo);
            this.groupBox2.Controls.Add(this.lblMusteriAdi);
            this.groupBox2.Location = new System.Drawing.Point(3, 315);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(715, 176);
            this.groupBox2.TabIndex = 350;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Müşteri Bilgileri";
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.BackColor = System.Drawing.Color.White;
            this.txtMusteriAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMusteriAdi.Location = new System.Drawing.Point(320, 104);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(369, 27);
            this.txtMusteriAdi.TabIndex = 335;
            // 
            // lblMusteriNo
            // 
            this.lblMusteriNo.AutoSize = true;
            this.lblMusteriNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMusteriNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblMusteriNo.Location = new System.Drawing.Point(30, 54);
            this.lblMusteriNo.Name = "lblMusteriNo";
            this.lblMusteriNo.Size = new System.Drawing.Size(99, 23);
            this.lblMusteriNo.TabIndex = 334;
            this.lblMusteriNo.Text = "Müşteri No:";
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.BackColor = System.Drawing.Color.White;
            this.txtMusteriNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMusteriNo.Location = new System.Drawing.Point(320, 54);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(369, 27);
            this.txtMusteriNo.TabIndex = 333;
            // 
            // lblMusteriAdi
            // 
            this.lblMusteriAdi.AutoSize = true;
            this.lblMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMusteriAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblMusteriAdi.Location = new System.Drawing.Point(30, 104);
            this.lblMusteriAdi.Name = "lblMusteriAdi";
            this.lblMusteriAdi.Size = new System.Drawing.Size(101, 23);
            this.lblMusteriAdi.TabIndex = 336;
            this.lblMusteriAdi.Text = "Müşteri Adı:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProjeBasTarihi);
            this.groupBox1.Controls.Add(this.txtProjeBitisTarihi);
            this.groupBox1.Controls.Add(this.lblProjeBitisTarihi);
            this.groupBox1.Controls.Add(this.txtProjeAdi);
            this.groupBox1.Controls.Add(this.lblProjeBasTarihi);
            this.groupBox1.Controls.Add(this.lblProjeAdi);
            this.groupBox1.Controls.Add(this.lblProjeNo);
            this.groupBox1.Controls.Add(this.txtProjeNo);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(715, 306); // Size'ı koruyoruz
            this.groupBox1.TabIndex = 349;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Proje Bilgileri";
            // 
            // txtProjeBitisTarihi
            // 
            this.txtProjeBitisTarihi.BackColor = System.Drawing.Color.White;
            this.txtProjeBitisTarihi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeBitisTarihi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeBitisTarihi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeBitisTarihi.Location = new System.Drawing.Point(320, 213);
            this.txtProjeBitisTarihi.Name = "txtProjeBitisTarihi";
            this.txtProjeBitisTarihi.Size = new System.Drawing.Size(369, 27);
            this.txtProjeBitisTarihi.TabIndex = 345;
            // 
            // lblProjeBitisTarihi
            // 
            this.lblProjeBitisTarihi.AutoSize = true;
            this.lblProjeBitisTarihi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjeBitisTarihi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblProjeBitisTarihi.Location = new System.Drawing.Point(30, 213);
            this.lblProjeBitisTarihi.Name = "lblProjeBitisTarihi";
            this.lblProjeBitisTarihi.Size = new System.Drawing.Size(133, 23);
            this.lblProjeBitisTarihi.TabIndex = 346;
            this.lblProjeBitisTarihi.Text = "Proje bitiş tarihi:";
            // 
            // txtProjeAdi
            // 
            this.txtProjeAdi.BackColor = System.Drawing.Color.White;
            this.txtProjeAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeAdi.Location = new System.Drawing.Point(320, 105);
            this.txtProjeAdi.Name = "txtProjeAdi";
            this.txtProjeAdi.Size = new System.Drawing.Size(369, 27);
            this.txtProjeAdi.TabIndex = 347;
            // 
            // lblProjeAdi
            // 
            this.lblProjeAdi.AutoSize = true;
            this.lblProjeAdi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjeAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblProjeAdi.Location = new System.Drawing.Point(30, 105);
            this.lblProjeAdi.Name = "lblProjeAdi";
            this.lblProjeAdi.Size = new System.Drawing.Size(83, 23);
            this.lblProjeAdi.TabIndex = 348;
            this.lblProjeAdi.Text = "Proje Adı:";
            // 
            // lblProjeNo
            // 
            this.lblProjeNo.AutoSize = true;
            this.lblProjeNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProjeNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblProjeNo.Location = new System.Drawing.Point(30, 54);
            this.lblProjeNo.Name = "lblProjeNo";
            this.lblProjeNo.Size = new System.Drawing.Size(81, 23);
            this.lblProjeNo.TabIndex = 344;
            this.lblProjeNo.Text = "Proje No:";
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.BackColor = System.Drawing.Color.White;
            this.txtProjeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeNo.Location = new System.Drawing.Point(320, 54);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(369, 27);
            this.txtProjeNo.TabIndex = 343;
            // 
            // btnUrunGrubuEkle
            // 
            this.btnUrunGrubuEkle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))); // Butonu sola ve alta sabitle
            this.btnUrunGrubuEkle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnUrunGrubuEkle.FlatAppearance.BorderSize = 0;
            this.btnUrunGrubuEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunGrubuEkle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnUrunGrubuEkle.ForeColor = System.Drawing.Color.White;
            this.btnUrunGrubuEkle.Location = new System.Drawing.Point(3, 809);
            this.btnUrunGrubuEkle.Name = "btnUrunGrubuEkle";
            this.btnUrunGrubuEkle.Size = new System.Drawing.Size(143, 28);
            this.btnUrunGrubuEkle.TabIndex = 330;
            this.btnUrunGrubuEkle.Text = "Ürün Grubu Ekle";
            this.btnUrunGrubuEkle.UseVisualStyleBackColor = false;
            this.btnUrunGrubuEkle.Click += new System.EventHandler(this.btnUrunGrubuEkle_Click);
            // 
            // tbUrunGruplari
            // 
            this.tbUrunGruplari.Controls.Add(this.tableLayoutPanel1);
            this.tbUrunGruplari.Location = new System.Drawing.Point(4, 25);
            this.tbUrunGruplari.Name = "tbUrunGruplari";
            this.tbUrunGruplari.Padding = new System.Windows.Forms.Padding(3);
            this.tbUrunGruplari.Size = new System.Drawing.Size(1632, 846);
            this.tbUrunGruplari.TabIndex = 1;
            this.tbUrunGruplari.Text = "Ürün Grupları";
            this.tbUrunGruplari.UseVisualStyleBackColor = true;
            this.tbUrunGruplari.Resize += new System.EventHandler(this.tbUrunGruplari_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridUstGruplar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridUrunGruplari, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1626, 840);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridUstGruplar
            // 
            this.dataGridUstGruplar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUstGruplar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUstGruplar.Location = new System.Drawing.Point(3, 3);
            this.dataGridUstGruplar.Name = "dataGridUstGruplar";
            this.dataGridUstGruplar.RowHeadersWidth = 51;
            this.dataGridUstGruplar.RowTemplate.Height = 24;
            this.dataGridUstGruplar.Size = new System.Drawing.Size(1620, 414);
            this.dataGridUstGruplar.TabIndex = 332;
            // 
            // dataGridUrunGruplari
            // 
            this.dataGridUrunGruplari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUrunGruplari.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUrunGruplari.Location = new System.Drawing.Point(3, 423);
            this.dataGridUrunGruplari.Name = "dataGridUrunGruplari";
            this.dataGridUrunGruplari.RowHeadersWidth = 51;
            this.dataGridUrunGruplari.RowTemplate.Height = 24;
            this.dataGridUrunGruplari.Size = new System.Drawing.Size(1620, 414);
            this.dataGridUrunGruplari.TabIndex = 0;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1640, 63);
            this.ctlBaslik1.TabIndex = 28;
            // 
            // ctlProjeKarti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeKarti";
            this.Size = new System.Drawing.Size(1640, 1022);
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelUstSag.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbProjeBilgileri.ResumeLayout(false);
            this.tlpProjeBilgileri.ResumeLayout(false); // YENİ EKLENDİ
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbUrunGruplari.ResumeLayout(false);
            this.tbUrunGruplari.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUstGruplar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUrunGruplari)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private UsrControl.ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.Panel panelUstSag;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnStatuBilgisi;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblToplamBedelBilgi;
        private System.Windows.Forms.TextBox txtProjeAra;
        private System.Windows.Forms.Label lblAraProje;
        private System.Windows.Forms.TextBox txtProjeBasTarihi;
        private System.Windows.Forms.Label lblProjeBasTarihi;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbProjeBilgileri;
        private System.Windows.Forms.Button btnUrunGrubuEkle;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Label lblProjeNo;
        private System.Windows.Forms.CheckBox chkRefProjeYok;
        private System.Windows.Forms.CheckBox chkRefProjeVar;
        private System.Windows.Forms.TextBox txtRefProje;
        private System.Windows.Forms.Label lblReferansProje;
        private System.Windows.Forms.TextBox txtProjeMuh;
        private System.Windows.Forms.Label lblProjeMuh;
        private System.Windows.Forms.TextBox txtMusteriAdi;
        private System.Windows.Forms.Label lblMusteriAdi;
        private System.Windows.Forms.TextBox txtMusteriNo;
        private System.Windows.Forms.Label lblMusteriNo;
        private System.Windows.Forms.TabPage tbUrunGruplari;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridUstGruplar;
        private System.Windows.Forms.DataGridView dataGridUrunGruplari;
        private System.Windows.Forms.TextBox txtProjeBitisTarihi;
        private System.Windows.Forms.Label lblProjeBitisTarihi;
        private System.Windows.Forms.TextBox txtProjeAdi;
        private System.Windows.Forms.Label lblProjeAdi;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        // YENİ EKLENEN KONTROL
        private System.Windows.Forms.TableLayoutPanel xtlpProjeBilgileri;
    }
}