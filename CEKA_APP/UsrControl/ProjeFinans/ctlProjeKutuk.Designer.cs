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
            this.label9 = new System.Windows.Forms.Label();
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
            this.chkCogul = new System.Windows.Forms.CheckBox();
            this.chkTekil = new System.Windows.Forms.CheckBox();
            this.btnSil = new System.Windows.Forms.Button();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.cmbParaBirimi = new System.Windows.Forms.ComboBox();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.panelUstSag = new System.Windows.Forms.Panel();
            this.txtDurum = new System.Windows.Forms.TextBox();
            this.lblDurum = new System.Windows.Forms.Label();
            this.lblToplamBedelBilgi = new System.Windows.Forms.Label();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelContainer.SuspendLayout();
            this.panelAlt.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.panelUstSag.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(53, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Müşteri No:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label2.Location = new System.Drawing.Point(53, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Müşteri Adı:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label4.Location = new System.Drawing.Point(53, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "İş Fırsatı No:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label5.Location = new System.Drawing.Point(53, 327);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Proje No:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label6.Location = new System.Drawing.Point(53, 376);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Alt proje var mı ?";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(53, 422);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(244, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Diğer projelerle ilişkisi var mı ? ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label8.Location = new System.Drawing.Point(53, 472);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Sipariş Sözleşme Tarihi:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label9.Location = new System.Drawing.Point(53, 680);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "Toplam Bedel:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label10.Location = new System.Drawing.Point(53, 626);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Nakliye var mı ?";
            // 
            // txtMusteriNo
            // 
            this.txtMusteriNo.BackColor = System.Drawing.Color.White;
            this.txtMusteriNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMusteriNo.Location = new System.Drawing.Point(679, 162);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(402, 27);
            this.txtMusteriNo.TabIndex = 10;
            this.txtMusteriNo.Leave += new System.EventHandler(this.txtMusteriNo_Leave);
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.BackColor = System.Drawing.Color.White;
            this.txtMusteriAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriAdi.Enabled = false;
            this.txtMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMusteriAdi.Location = new System.Drawing.Point(679, 212);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(402, 27);
            this.txtMusteriAdi.TabIndex = 11;
            // 
            // txtIsFirsatiNo
            // 
            this.txtIsFirsatiNo.BackColor = System.Drawing.Color.White;
            this.txtIsFirsatiNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIsFirsatiNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtIsFirsatiNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtIsFirsatiNo.Location = new System.Drawing.Point(679, 268);
            this.txtIsFirsatiNo.Name = "txtIsFirsatiNo";
            this.txtIsFirsatiNo.Size = new System.Drawing.Size(402, 27);
            this.txtIsFirsatiNo.TabIndex = 13;
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.BackColor = System.Drawing.Color.White;
            this.txtProjeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProjeNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtProjeNo.Location = new System.Drawing.Point(679, 323);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(402, 27);
            this.txtProjeNo.TabIndex = 14;
            // 
            // txtToplamBedel
            // 
            this.txtToplamBedel.BackColor = System.Drawing.Color.White;
            this.txtToplamBedel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToplamBedel.Enabled = false;
            this.txtToplamBedel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtToplamBedel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtToplamBedel.Location = new System.Drawing.Point(679, 676);
            this.txtToplamBedel.Name = "txtToplamBedel";
            this.txtToplamBedel.Size = new System.Drawing.Size(402, 27);
            this.txtToplamBedel.TabIndex = 18;
            // 
            // chkAltProjeVar
            // 
            this.chkAltProjeVar.AutoSize = true;
            this.chkAltProjeVar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAltProjeVar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkAltProjeVar.Location = new System.Drawing.Point(679, 373);
            this.chkAltProjeVar.Name = "chkAltProjeVar";
            this.chkAltProjeVar.Size = new System.Drawing.Size(52, 24);
            this.chkAltProjeVar.TabIndex = 20;
            this.chkAltProjeVar.Text = "Var";
            this.chkAltProjeVar.UseVisualStyleBackColor = true;
            // 
            // chkAltProjeYok
            // 
            this.chkAltProjeYok.AutoSize = true;
            this.chkAltProjeYok.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAltProjeYok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkAltProjeYok.Location = new System.Drawing.Point(796, 373);
            this.chkAltProjeYok.Name = "chkAltProjeYok";
            this.chkAltProjeYok.Size = new System.Drawing.Size(54, 24);
            this.chkAltProjeYok.TabIndex = 21;
            this.chkAltProjeYok.Text = "Yok";
            this.chkAltProjeYok.UseVisualStyleBackColor = true;
            // 
            // chkProjeIliskisiYok
            // 
            this.chkProjeIliskisiYok.AutoSize = true;
            this.chkProjeIliskisiYok.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkProjeIliskisiYok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkProjeIliskisiYok.Location = new System.Drawing.Point(796, 419);
            this.chkProjeIliskisiYok.Name = "chkProjeIliskisiYok";
            this.chkProjeIliskisiYok.Size = new System.Drawing.Size(54, 24);
            this.chkProjeIliskisiYok.TabIndex = 23;
            this.chkProjeIliskisiYok.Text = "Yok";
            this.chkProjeIliskisiYok.UseVisualStyleBackColor = true;
            // 
            // chkProjeIliskisiVar
            // 
            this.chkProjeIliskisiVar.AutoSize = true;
            this.chkProjeIliskisiVar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkProjeIliskisiVar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkProjeIliskisiVar.Location = new System.Drawing.Point(679, 419);
            this.chkProjeIliskisiVar.Name = "chkProjeIliskisiVar";
            this.chkProjeIliskisiVar.Size = new System.Drawing.Size(52, 24);
            this.chkProjeIliskisiVar.TabIndex = 22;
            this.chkProjeIliskisiVar.Text = "Var";
            this.chkProjeIliskisiVar.UseVisualStyleBackColor = true;
            // 
            // chkNakliyeYok
            // 
            this.chkNakliyeYok.AutoSize = true;
            this.chkNakliyeYok.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkNakliyeYok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkNakliyeYok.Location = new System.Drawing.Point(796, 627);
            this.chkNakliyeYok.Name = "chkNakliyeYok";
            this.chkNakliyeYok.Size = new System.Drawing.Size(54, 24);
            this.chkNakliyeYok.TabIndex = 25;
            this.chkNakliyeYok.Text = "Yok";
            this.chkNakliyeYok.UseVisualStyleBackColor = true;
            // 
            // chkNakliyeVar
            // 
            this.chkNakliyeVar.AutoSize = true;
            this.chkNakliyeVar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkNakliyeVar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkNakliyeVar.Location = new System.Drawing.Point(679, 627);
            this.chkNakliyeVar.Name = "chkNakliyeVar";
            this.chkNakliyeVar.Size = new System.Drawing.Size(52, 24);
            this.chkNakliyeVar.TabIndex = 24;
            this.chkNakliyeVar.Text = "Var";
            this.chkNakliyeVar.UseVisualStyleBackColor = true;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.dtpSiparisSozlesmeTarihi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpSiparisSozlesmeTarihi.Location = new System.Drawing.Point(679, 469);
            this.dtpSiparisSozlesmeTarihi.Name = "dtpSiparisSozlesmeTarihi";
            this.dtpSiparisSozlesmeTarihi.Size = new System.Drawing.Size(402, 27);
            this.dtpSiparisSozlesmeTarihi.TabIndex = 314;
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
            this.txtProjeAra.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeAra_KeyDown);
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
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
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
            // lblAltProjeHata
            // 
            this.lblAltProjeHata.AutoSize = true;
            this.lblAltProjeHata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAltProjeHata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblAltProjeHata.Location = new System.Drawing.Point(675, 706);
            this.lblAltProjeHata.Name = "lblAltProjeHata";
            this.lblAltProjeHata.Size = new System.Drawing.Size(0, 20);
            this.lblAltProjeHata.TabIndex = 318;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label12.Location = new System.Drawing.Point(53, 520);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 23);
            this.label12.TabIndex = 322;
            this.label12.Text = "Para Birimi:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label13.Location = new System.Drawing.Point(53, 574);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(558, 23);
            this.label13.TabIndex = 323;
            this.label13.Text = "Ödeme şartları, tekil mi yoksa çoklu faturalandırma için mi uygulanacak?\r\n";
            // 
            // chkCogul
            // 
            this.chkCogul.AutoSize = true;
            this.chkCogul.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkCogul.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkCogul.Location = new System.Drawing.Point(796, 573);
            this.chkCogul.Name = "chkCogul";
            this.chkCogul.Size = new System.Drawing.Size(70, 24);
            this.chkCogul.TabIndex = 325;
            this.chkCogul.Text = "Çoğul";
            this.chkCogul.UseVisualStyleBackColor = true;
            // 
            // chkTekil
            // 
            this.chkTekil.AutoSize = true;
            this.chkTekil.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTekil.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkTekil.Location = new System.Drawing.Point(679, 573);
            this.chkTekil.Name = "chkTekil";
            this.chkTekil.Size = new System.Drawing.Size(61, 24);
            this.chkTekil.TabIndex = 324;
            this.chkTekil.Text = "Tekil";
            this.chkTekil.UseVisualStyleBackColor = true;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(24, 43);
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
            this.panelContainer.Controls.Add(this.panelAlt);
            this.panelContainer.Controls.Add(this.cmbParaBirimi);
            this.panelContainer.Controls.Add(this.label1);
            this.panelContainer.Controls.Add(this.label2);
            this.panelContainer.Controls.Add(this.chkCogul);
            this.panelContainer.Controls.Add(this.chkTekil);
            this.panelContainer.Controls.Add(this.label4);
            this.panelContainer.Controls.Add(this.label13);
            this.panelContainer.Controls.Add(this.label5);
            this.panelContainer.Controls.Add(this.label12);
            this.panelContainer.Controls.Add(this.label6);
            this.panelContainer.Controls.Add(this.label7);
            this.panelContainer.Controls.Add(this.label8);
            this.panelContainer.Controls.Add(this.label9);
            this.panelContainer.Controls.Add(this.lblAltProjeHata);
            this.panelContainer.Controls.Add(this.label10);
            this.panelContainer.Controls.Add(this.txtMusteriNo);
            this.panelContainer.Controls.Add(this.txtMusteriAdi);
            this.panelContainer.Controls.Add(this.dtpSiparisSozlesmeTarihi);
            this.panelContainer.Controls.Add(this.txtIsFirsatiNo);
            this.panelContainer.Controls.Add(this.txtProjeNo);
            this.panelContainer.Controls.Add(this.txtToplamBedel);
            this.panelContainer.Controls.Add(this.chkNakliyeYok);
            this.panelContainer.Controls.Add(this.chkAltProjeVar);
            this.panelContainer.Controls.Add(this.chkNakliyeVar);
            this.panelContainer.Controls.Add(this.chkAltProjeYok);
            this.panelContainer.Controls.Add(this.chkProjeIliskisiYok);
            this.panelContainer.Controls.Add(this.chkProjeIliskisiVar);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 50);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1578, 882);
            this.panelContainer.TabIndex = 327;
            // 
            // panelAlt
            // 
            this.panelAlt.BackColor = System.Drawing.Color.White;
            this.panelAlt.Controls.Add(this.btnKaydet);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 828);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1578, 54);
            this.panelAlt.TabIndex = 331;
            // 
            // cmbParaBirimi
            // 
            this.cmbParaBirimi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParaBirimi.FormattingEnabled = true;
            this.cmbParaBirimi.Items.AddRange(new object[] {
            "Euro (€)",
            "Dolar ($)",
            "Türk Lirası (₺)"});
            this.cmbParaBirimi.Location = new System.Drawing.Point(679, 522);
            this.cmbParaBirimi.Name = "cmbParaBirimi";
            this.cmbParaBirimi.Size = new System.Drawing.Size(402, 24);
            this.cmbParaBirimi.TabIndex = 327;
            // 
            // btnTemizle
            // 
            this.btnTemizle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnTemizle.FlatAppearance.BorderSize = 0;
            this.btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemizle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTemizle.ForeColor = System.Drawing.Color.White;
            this.btnTemizle.Location = new System.Drawing.Point(24, 77);
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
            this.panelUst.Controls.Add(this.panelUstSag);
            this.panelUst.Controls.Add(this.txtDurum);
            this.panelUst.Controls.Add(this.lblDurum);
            this.panelUst.Controls.Add(this.lblToplamBedelBilgi);
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.lblAraProje);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 50);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1578, 115);
            this.panelUst.TabIndex = 329;
            // 
            // panelUstSag
            // 
            this.panelUstSag.Controls.Add(this.btnTemizle);
            this.panelUstSag.Controls.Add(this.btnSil);
            this.panelUstSag.Controls.Add(this.btnAra);
            this.panelUstSag.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelUstSag.Location = new System.Drawing.Point(1428, 0);
            this.panelUstSag.Name = "panelUstSag";
            this.panelUstSag.Size = new System.Drawing.Size(148, 113);
            this.panelUstSag.TabIndex = 331;
            // 
            // txtDurum
            // 
            this.txtDurum.BackColor = System.Drawing.Color.White;
            this.txtDurum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDurum.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDurum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtDurum.Location = new System.Drawing.Point(235, 39);
            this.txtDurum.Name = "txtDurum";
            this.txtDurum.Size = new System.Drawing.Size(177, 27);
            this.txtDurum.TabIndex = 329;
            // 
            // lblDurum
            // 
            this.lblDurum.AutoSize = true;
            this.lblDurum.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDurum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblDurum.Location = new System.Drawing.Point(231, 13);
            this.lblDurum.Name = "lblDurum";
            this.lblDurum.Size = new System.Drawing.Size(67, 23);
            this.lblDurum.TabIndex = 330;
            this.lblDurum.Text = "Durum:";
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
            // ctlBaslik1
            // 
            this.ctlBaslik1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.ctlBaslik1.ForeColor = System.Drawing.Color.White;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1578, 50);
            this.ctlBaslik1.TabIndex = 26;
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
            this.Size = new System.Drawing.Size(1578, 932);
            this.Load += new System.EventHandler(this.ctlProjeKutuk_Load);
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            this.panelAlt.ResumeLayout(false);
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
        private System.Windows.Forms.Label label9;
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
        private System.Windows.Forms.CheckBox chkCogul;
        private System.Windows.Forms.CheckBox chkTekil;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.ComboBox cmbParaBirimi;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.TextBox txtDurum;
        private System.Windows.Forms.Label lblDurum;
        private System.Windows.Forms.Label lblToplamBedelBilgi;
        private System.Windows.Forms.Panel panelUstSag;
        private System.Windows.Forms.Panel panelAlt;
    }
}