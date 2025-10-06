using System.Drawing;

namespace CEKA_APP.UsrControl
{
    partial class ctlProjeKutuk
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMusteriNo = new System.Windows.Forms.TextBox();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.txtIsFirsatiNo = new System.Windows.Forms.TextBox();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.txtToplamBedel = new System.Windows.Forms.TextBox();
            this.chkAltProjeVar = new System.Windows.Forms.CheckBox();
            this.chkAltProjeYok = new System.Windows.Forms.CheckBox();
            this.chkProjeIliskisiYok = new System.Windows.Forms.CheckBox();
            this.chkProjeIliskisiVar = new System.Windows.Forms.CheckBox();
            this.chkNakliyeYok = new System.Windows.Forms.CheckBox();
            this.chkNakliyeVar = new System.Windows.Forms.CheckBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.dtpSiparisSozlesmeTarihi = new System.Windows.Forms.DateTimePicker();
            this.txtProjeAra = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.lblAraProje = new System.Windows.Forms.Label();
            this.lblAltProjeHata = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chkCoklu = new System.Windows.Forms.CheckBox();
            this.chkTekil = new System.Windows.Forms.CheckBox();
            this.btnSil = new System.Windows.Forms.Button();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMontajTamamlanmadi = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkMontajTamamlandi = new System.Windows.Forms.CheckBox();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.btnMektupEkle = new System.Windows.Forms.Button();
            this.cmbParaBirimi = new System.Windows.Forms.ComboBox();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.panelUstSag = new System.Windows.Forms.Panel();
            this.btnStatuBilgisi = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblToplamBedelBilgi = new System.Windows.Forms.Label();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.lblToplamBedel = new System.Windows.Forms.Label();
            this.panelContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelAlt.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.panelUstSag.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Müşteri No:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Müşteri Adı:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(26, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "İş Fırsatı No:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(26, 292);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Proje No:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(26, 341);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Alt proje var mı ?";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(26, 387);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(244, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Diğer projelerle ilişkisi var mı ? ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(26, 437);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Sipariş Sözleşme Tarihi:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(26, 591);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Nakliye var mı ?";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.BackColor = System.Drawing.Color.White;
            this.txtMusteriNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriNo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMusteriNo.Location = new System.Drawing.Point(632, 23);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(402, 29);
            this.txtMusteriNo.TabIndex = 10;
            this.txtMusteriNo.Leave += new System.EventHandler(this.txtMusteriNo_Leave);
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.BackColor = System.Drawing.Color.White;
            this.txtMusteriAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriAdi.Enabled = false;
            this.txtMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtMusteriAdi.Location = new System.Drawing.Point(632, 73);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(402, 29);
            this.txtMusteriAdi.TabIndex = 11;
            // 
            // txtIsFirsatiNo
            // 
            this.txtIsFirsatiNo.BackColor = System.Drawing.Color.White;
            this.txtIsFirsatiNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIsFirsatiNo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtIsFirsatiNo.Location = new System.Drawing.Point(652, 233);
            this.txtIsFirsatiNo.Name = "txtIsFirsatiNo";
            this.txtIsFirsatiNo.Size = new System.Drawing.Size(402, 29);
            this.txtIsFirsatiNo.TabIndex = 12;
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.BackColor = System.Drawing.Color.White;
            this.txtProjeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeNo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtProjeNo.Location = new System.Drawing.Point(652, 288);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(402, 29);
            this.txtProjeNo.TabIndex = 13;
            this.txtProjeNo.Leave += new System.EventHandler(this.txtProjeNo_Leave);
            // 
            // txtToplamBedel
            // 
            this.txtToplamBedel.BackColor = System.Drawing.Color.White;
            this.txtToplamBedel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToplamBedel.Enabled = false;
            this.txtToplamBedel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtToplamBedel.Location = new System.Drawing.Point(433, 39);
            this.txtToplamBedel.Name = "txtToplamBedel";
            this.txtToplamBedel.Size = new System.Drawing.Size(177, 29);
            this.txtToplamBedel.TabIndex = 18;
            this.txtToplamBedel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkAltProjeVar
            // 
            this.chkAltProjeVar.AutoSize = true;
            this.chkAltProjeVar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkAltProjeVar.Location = new System.Drawing.Point(652, 338);
            this.chkAltProjeVar.Name = "chkAltProjeVar";
            this.chkAltProjeVar.Size = new System.Drawing.Size(57, 27);
            this.chkAltProjeVar.TabIndex = 14;
            this.chkAltProjeVar.Text = "Var";
            this.chkAltProjeVar.UseVisualStyleBackColor = true;
            // 
            // chkAltProjeYok
            // 
            this.chkAltProjeYok.AutoSize = true;
            this.chkAltProjeYok.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkAltProjeYok.Location = new System.Drawing.Point(769, 338);
            this.chkAltProjeYok.Name = "chkAltProjeYok";
            this.chkAltProjeYok.Size = new System.Drawing.Size(58, 27);
            this.chkAltProjeYok.TabIndex = 15;
            this.chkAltProjeYok.Text = "Yok";
            this.chkAltProjeYok.UseVisualStyleBackColor = true;
            // 
            // chkProjeIliskisiYok
            // 
            this.chkProjeIliskisiYok.AutoSize = true;
            this.chkProjeIliskisiYok.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkProjeIliskisiYok.Location = new System.Drawing.Point(769, 384);
            this.chkProjeIliskisiYok.Name = "chkProjeIliskisiYok";
            this.chkProjeIliskisiYok.Size = new System.Drawing.Size(58, 27);
            this.chkProjeIliskisiYok.TabIndex = 17;
            this.chkProjeIliskisiYok.Text = "Yok";
            this.chkProjeIliskisiYok.UseVisualStyleBackColor = true;
            // 
            // chkProjeIliskisiVar
            // 
            this.chkProjeIliskisiVar.AutoSize = true;
            this.chkProjeIliskisiVar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkProjeIliskisiVar.Location = new System.Drawing.Point(652, 384);
            this.chkProjeIliskisiVar.Name = "chkProjeIliskisiVar";
            this.chkProjeIliskisiVar.Size = new System.Drawing.Size(57, 27);
            this.chkProjeIliskisiVar.TabIndex = 16;
            this.chkProjeIliskisiVar.Text = "Var";
            this.chkProjeIliskisiVar.UseVisualStyleBackColor = true;
            // 
            // chkNakliyeYok
            // 
            this.chkNakliyeYok.AutoSize = true;
            this.chkNakliyeYok.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkNakliyeYok.Location = new System.Drawing.Point(769, 592);
            this.chkNakliyeYok.Name = "chkNakliyeYok";
            this.chkNakliyeYok.Size = new System.Drawing.Size(58, 27);
            this.chkNakliyeYok.TabIndex = 23;
            this.chkNakliyeYok.Text = "Yok";
            this.chkNakliyeYok.UseVisualStyleBackColor = true;
            // 
            // chkNakliyeVar
            // 
            this.chkNakliyeVar.AutoSize = true;
            this.chkNakliyeVar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkNakliyeVar.Location = new System.Drawing.Point(652, 592);
            this.chkNakliyeVar.Name = "chkNakliyeVar";
            this.chkNakliyeVar.Size = new System.Drawing.Size(57, 27);
            this.chkNakliyeVar.TabIndex = 22;
            this.chkNakliyeVar.Text = "Var";
            this.chkNakliyeVar.UseVisualStyleBackColor = true;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(3, 4);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(219, 47);
            this.btnKaydet.TabIndex = 27;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // dtpSiparisSozlesmeTarihi
            // 
            this.dtpSiparisSozlesmeTarihi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dtpSiparisSozlesmeTarihi.Location = new System.Drawing.Point(652, 434);
            this.dtpSiparisSozlesmeTarihi.Name = "dtpSiparisSozlesmeTarihi";
            this.dtpSiparisSozlesmeTarihi.Size = new System.Drawing.Size(402, 29);
            this.dtpSiparisSozlesmeTarihi.TabIndex = 18;
            // 
            // txtProjeAra
            // 
            this.txtProjeAra.BackColor = System.Drawing.Color.White;
            this.txtProjeAra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeAra.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtProjeAra.Location = new System.Drawing.Point(37, 39);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(177, 29);
            this.txtProjeAra.TabIndex = 315;
            this.txtProjeAra.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeAra_KeyDown);
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.Location = new System.Drawing.Point(24, 8);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(100, 28);
            this.btnAra.TabIndex = 316;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = false;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // lblAraProje
            // 
            this.lblAraProje.AutoSize = true;
            this.lblAraProje.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAraProje.Location = new System.Drawing.Point(33, 13);
            this.lblAraProje.Name = "lblAraProje";
            this.lblAraProje.Size = new System.Drawing.Size(81, 23);
            this.lblAraProje.TabIndex = 317;
            this.lblAraProje.Text = "Proje No:";
            this.lblAraProje.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAltProjeHata
            // 
            this.lblAltProjeHata.AutoSize = true;
            this.lblAltProjeHata.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblAltProjeHata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblAltProjeHata.Location = new System.Drawing.Point(619, 43);
            this.lblAltProjeHata.Name = "lblAltProjeHata";
            this.lblAltProjeHata.Size = new System.Drawing.Size(0, 23);
            this.lblAltProjeHata.TabIndex = 318;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(26, 485);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 23);
            this.label12.TabIndex = 322;
            this.label12.Text = "Para Birimi:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(26, 539);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(558, 23);
            this.label13.TabIndex = 323;
            this.label13.Text = "Ödeme şartları, tekil mi yoksa çoklu faturalandırma için mi uygulanacak?";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCoklu
            // 
            this.chkCoklu.AutoSize = true;
            this.chkCoklu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkCoklu.Location = new System.Drawing.Point(769, 538);
            this.chkCoklu.Name = "chkCoklu";
            this.chkCoklu.Size = new System.Drawing.Size(75, 27);
            this.chkCoklu.TabIndex = 21;
            this.chkCoklu.Text = "Çoklu";
            this.chkCoklu.UseVisualStyleBackColor = true;
            // 
            // chkTekil
            // 
            this.chkTekil.AutoSize = true;
            this.chkTekil.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkTekil.Location = new System.Drawing.Point(652, 538);
            this.chkTekil.Name = "chkTekil";
            this.chkTekil.Size = new System.Drawing.Size(64, 27);
            this.chkTekil.TabIndex = 20;
            this.chkTekil.Text = "Tekil";
            this.chkTekil.UseVisualStyleBackColor = true;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(130, 8);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(100, 28);
            this.btnSil.TabIndex = 326;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.White;
            this.panelContainer.Controls.Add(this.groupBox2);
            this.panelContainer.Controls.Add(this.groupBox1);
            this.panelContainer.Controls.Add(this.panelAlt);
            this.panelContainer.Controls.Add(this.cmbParaBirimi);
            this.panelContainer.Controls.Add(this.chkCoklu);
            this.panelContainer.Controls.Add(this.chkTekil);
            this.panelContainer.Controls.Add(this.label4);
            this.panelContainer.Controls.Add(this.label13);
            this.panelContainer.Controls.Add(this.label5);
            this.panelContainer.Controls.Add(this.label12);
            this.panelContainer.Controls.Add(this.label6);
            this.panelContainer.Controls.Add(this.label7);
            this.panelContainer.Controls.Add(this.label8);
            this.panelContainer.Controls.Add(this.label10);
            this.panelContainer.Controls.Add(this.dtpSiparisSozlesmeTarihi);
            this.panelContainer.Controls.Add(this.txtIsFirsatiNo);
            this.panelContainer.Controls.Add(this.txtProjeNo);
            this.panelContainer.Controls.Add(this.chkNakliyeYok);
            this.panelContainer.Controls.Add(this.chkAltProjeVar);
            this.panelContainer.Controls.Add(this.chkNakliyeVar);
            this.panelContainer.Controls.Add(this.chkAltProjeYok);
            this.panelContainer.Controls.Add(this.chkProjeIliskisiYok);
            this.panelContainer.Controls.Add(this.chkProjeIliskisiVar);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 63);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1108, 783);
            this.panelContainer.TabIndex = 327;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMusteriAdi);
            this.groupBox2.Controls.Add(this.txtMusteriNo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.Location = new System.Drawing.Point(20, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1047, 116);
            this.groupBox2.TabIndex = 336;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Müsteri Bilgileri";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMontajTamamlanmadi);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkMontajTamamlandi);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(20, 634);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1047, 73);
            this.groupBox1.TabIndex = 335;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sevkiyat Sonrası";
            // 
            // chkMontajTamamlanmadi
            // 
            this.chkMontajTamamlanmadi.AutoSize = true;
            this.chkMontajTamamlanmadi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkMontajTamamlanmadi.Location = new System.Drawing.Point(748, 31);
            this.chkMontajTamamlanmadi.Name = "chkMontajTamamlanmadi";
            this.chkMontajTamamlanmadi.Size = new System.Drawing.Size(71, 27);
            this.chkMontajTamamlanmadi.TabIndex = 334;
            this.chkMontajTamamlanmadi.Text = "Hayır";
            this.chkMontajTamamlanmadi.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(5, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 23);
            this.label3.TabIndex = 332;
            this.label3.Text = "Montaj tamamlandı mı?";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkMontajTamamlandi
            // 
            this.chkMontajTamamlandi.AutoSize = true;
            this.chkMontajTamamlandi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkMontajTamamlandi.Location = new System.Drawing.Point(631, 31);
            this.chkMontajTamamlandi.Name = "chkMontajTamamlandi";
            this.chkMontajTamamlandi.Size = new System.Drawing.Size(64, 27);
            this.chkMontajTamamlandi.TabIndex = 335;
            this.chkMontajTamamlandi.Text = "Evet";
            this.chkMontajTamamlandi.UseVisualStyleBackColor = true;
            // 
            // panelAlt
            // 
            this.panelAlt.BackColor = System.Drawing.Color.White;
            this.panelAlt.Controls.Add(this.panelLeft);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 729);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1108, 54);
            this.panelAlt.TabIndex = 331;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.btnMektupEkle);
            this.panelLeft.Controls.Add(this.btnKaydet);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(546, 54);
            this.panelLeft.TabIndex = 28;
            // 
            // btnMektupEkle
            // 
            this.btnMektupEkle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMektupEkle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnMektupEkle.FlatAppearance.BorderSize = 0;
            this.btnMektupEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMektupEkle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnMektupEkle.ForeColor = System.Drawing.Color.White;
            this.btnMektupEkle.Location = new System.Drawing.Point(228, 4);
            this.btnMektupEkle.Name = "btnMektupEkle";
            this.btnMektupEkle.Size = new System.Drawing.Size(219, 47);
            this.btnMektupEkle.TabIndex = 28;
            this.btnMektupEkle.Text = "Teminat Mektubu Ekle";
            this.btnMektupEkle.UseVisualStyleBackColor = false;
            this.btnMektupEkle.Click += new System.EventHandler(this.btnMektupEkle_Click);
            // 
            // cmbParaBirimi
            // 
            this.cmbParaBirimi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParaBirimi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbParaBirimi.FormattingEnabled = true;
            this.cmbParaBirimi.Items.AddRange(new object[] {
            "Euro (€)",
            "Dolar ($)",
            "Türk Lirası (₺)"});
            this.cmbParaBirimi.Location = new System.Drawing.Point(652, 487);
            this.cmbParaBirimi.Name = "cmbParaBirimi";
            this.cmbParaBirimi.Size = new System.Drawing.Size(402, 29);
            this.cmbParaBirimi.TabIndex = 19;
            // 
            // btnTemizle
            // 
            this.btnTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTemizle.FlatAppearance.BorderSize = 0;
            this.btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemizle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTemizle.ForeColor = System.Drawing.Color.White;
            this.btnTemizle.Location = new System.Drawing.Point(130, 42);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(100, 28);
            this.btnTemizle.TabIndex = 328;
            this.btnTemizle.Text = "Temizle";
            this.btnTemizle.UseVisualStyleBackColor = false;
            this.btnTemizle.Click += new System.EventHandler(this.btnTemizle_Click);
            // 
            // panelUst
            // 
            this.panelUst.BackColor = System.Drawing.Color.White;
            this.panelUst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUst.Controls.Add(this.lblToplamBedel);
            this.panelUst.Controls.Add(this.panelUstSag);
            this.panelUst.Controls.Add(this.txtStatus);
            this.panelUst.Controls.Add(this.lblAltProjeHata);
            this.panelUst.Controls.Add(this.lblStatus);
            this.panelUst.Controls.Add(this.lblToplamBedelBilgi);
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.lblAraProje);
            this.panelUst.Controls.Add(this.txtToplamBedel);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 63);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1108, 82);
            this.panelUst.TabIndex = 329;
            // 
            // panelUstSag
            // 
            this.panelUstSag.Controls.Add(this.btnStatuBilgisi);
            this.panelUstSag.Controls.Add(this.btnTemizle);
            this.panelUstSag.Controls.Add(this.btnSil);
            this.panelUstSag.Controls.Add(this.btnAra);
            this.panelUstSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelUstSag.Location = new System.Drawing.Point(854, 0);
            this.panelUstSag.Name = "panelUstSag";
            this.panelUstSag.Size = new System.Drawing.Size(252, 80);
            this.panelUstSag.TabIndex = 331;
            // 
            // btnStatuBilgisi
            // 
            this.btnStatuBilgisi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStatuBilgisi.FlatAppearance.BorderSize = 0;
            this.btnStatuBilgisi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatuBilgisi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStatuBilgisi.ForeColor = System.Drawing.Color.White;
            this.btnStatuBilgisi.Location = new System.Drawing.Point(24, 42);
            this.btnStatuBilgisi.Name = "btnStatuBilgisi";
            this.btnStatuBilgisi.Size = new System.Drawing.Size(100, 28);
            this.btnStatuBilgisi.TabIndex = 329;
            this.btnStatuBilgisi.Text = "Statü Bilgi";
            this.btnStatuBilgisi.UseVisualStyleBackColor = false;
            this.btnStatuBilgisi.Click += new System.EventHandler(this.btnStatuBilgisi_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.White;
            this.txtStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatus.Enabled = false;
            this.txtStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtStatus.Location = new System.Drawing.Point(235, 39);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(177, 29);
            this.txtStatus.TabIndex = 329;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblStatus.Location = new System.Drawing.Point(231, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 23);
            this.lblStatus.TabIndex = 330;
            this.lblStatus.Text = "Statü:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToplamBedelBilgi
            // 
            this.lblToplamBedelBilgi.AutoSize = true;
            this.lblToplamBedelBilgi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamBedelBilgi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblToplamBedelBilgi.Location = new System.Drawing.Point(1187, 60);
            this.lblToplamBedelBilgi.Name = "lblToplamBedelBilgi";
            this.lblToplamBedelBilgi.Size = new System.Drawing.Size(0, 23);
            this.lblToplamBedelBilgi.TabIndex = 5;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ctlBaslik1.ForeColor = System.Drawing.Color.White;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1108, 63);
            this.ctlBaslik1.TabIndex = 26;
            // 
            // lblToplamBedel
            // 
            this.lblToplamBedel.AutoSize = true;
            this.lblToplamBedel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamBedel.Location = new System.Drawing.Point(429, 13);
            this.lblToplamBedel.Name = "lblToplamBedel";
            this.lblToplamBedel.Size = new System.Drawing.Size(116, 23);
            this.lblToplamBedel.TabIndex = 332;
            this.lblToplamBedel.Text = "Toplam Bedel:";
            this.lblToplamBedel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ctlProjeKutuk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeKutuk";
            this.Size = new System.Drawing.Size(1108, 846);
            this.Load += new System.EventHandler(this.ctlProjeKutuk_Load);
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelAlt.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelUstSag.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMusteriNo;
        private System.Windows.Forms.TextBox txtMusteriAdi;
        private System.Windows.Forms.TextBox txtIsFirsatiNo;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.TextBox txtToplamBedel;
        private System.Windows.Forms.CheckBox chkAltProjeVar;
        private System.Windows.Forms.CheckBox chkAltProjeYok;
        private System.Windows.Forms.CheckBox chkProjeIliskisiYok;
        private System.Windows.Forms.CheckBox chkProjeIliskisiVar;
        private System.Windows.Forms.CheckBox chkNakliyeYok;
        private System.Windows.Forms.CheckBox chkNakliyeVar;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.DateTimePicker dtpSiparisSozlesmeTarihi;
        private System.Windows.Forms.TextBox txtProjeAra;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Label lblAraProje;
        private System.Windows.Forms.Label lblAltProjeHata;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkCoklu;
        private System.Windows.Forms.CheckBox chkTekil;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.ComboBox cmbParaBirimi;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblToplamBedelBilgi;
        private System.Windows.Forms.Panel panelUstSag;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.Button btnStatuBilgisi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkMontajTamamlanmadi;
        private System.Windows.Forms.CheckBox chkMontajTamamlandi;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Button btnMektupEkle;
        private System.Windows.Forms.Label lblToplamBedel;
    }
}