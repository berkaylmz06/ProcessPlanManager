﻿using System.Drawing;

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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMusteriNo = new System.Windows.Forms.TextBox();
            this.txtMusteriAdi = new System.Windows.Forms.TextBox();
            this.txtTeklifNo = new System.Windows.Forms.TextBox();
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
            this.label11 = new System.Windows.Forms.Label();
            this.lblAltProjeHata = new System.Windows.Forms.Label();
            this.chkEuro = new System.Windows.Forms.CheckBox();
            this.chkDolar = new System.Windows.Forms.CheckBox();
            this.chkTL = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chkCogul = new System.Windows.Forms.CheckBox();
            this.chkTekil = new System.Windows.Forms.CheckBox();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(44, 170);
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
            this.label2.Location = new System.Drawing.Point(44, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Müşteri Adı:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(44, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Teklif No:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label4.Location = new System.Drawing.Point(44, 320);
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
            this.label5.Location = new System.Drawing.Point(44, 375);
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
            this.label6.Location = new System.Drawing.Point(44, 424);
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
            this.label7.Location = new System.Drawing.Point(44, 470);
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
            this.label8.Location = new System.Drawing.Point(44, 520);
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
            this.label9.Location = new System.Drawing.Point(44, 708);
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
            this.label10.Location = new System.Drawing.Point(44, 654);
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
            this.txtMusteriNo.Location = new System.Drawing.Point(670, 166);
            this.txtMusteriNo.Name = "txtMusteriNo";
            this.txtMusteriNo.Size = new System.Drawing.Size(402, 27);
            this.txtMusteriNo.TabIndex = 10;
            this.txtMusteriNo.Leave += new System.EventHandler(this.txtMusteriNo_Leave);
            // 
            // txtMusteriAdi
            // 
            this.txtMusteriAdi.BackColor = System.Drawing.Color.White;
            this.txtMusteriAdi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMusteriAdi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMusteriAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtMusteriAdi.Location = new System.Drawing.Point(670, 216);
            this.txtMusteriAdi.Name = "txtMusteriAdi";
            this.txtMusteriAdi.Size = new System.Drawing.Size(402, 27);
            this.txtMusteriAdi.TabIndex = 11;
            // 
            // txtTeklifNo
            // 
            this.txtTeklifNo.BackColor = System.Drawing.Color.White;
            this.txtTeklifNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTeklifNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTeklifNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtTeklifNo.Location = new System.Drawing.Point(670, 266);
            this.txtTeklifNo.Name = "txtTeklifNo";
            this.txtTeklifNo.Size = new System.Drawing.Size(402, 27);
            this.txtTeklifNo.TabIndex = 12;
            // 
            // txtIsFirsatiNo
            // 
            this.txtIsFirsatiNo.BackColor = System.Drawing.Color.White;
            this.txtIsFirsatiNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIsFirsatiNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtIsFirsatiNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtIsFirsatiNo.Location = new System.Drawing.Point(670, 316);
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
            this.txtProjeNo.Location = new System.Drawing.Point(670, 371);
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
            this.txtToplamBedel.Location = new System.Drawing.Point(670, 704);
            this.txtToplamBedel.Name = "txtToplamBedel";
            this.txtToplamBedel.Size = new System.Drawing.Size(402, 27);
            this.txtToplamBedel.TabIndex = 18;
            // 
            // chkAltProjeVar
            // 
            this.chkAltProjeVar.AutoSize = true;
            this.chkAltProjeVar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAltProjeVar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkAltProjeVar.Location = new System.Drawing.Point(670, 421);
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
            this.chkAltProjeYok.Location = new System.Drawing.Point(787, 421);
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
            this.chkProjeIliskisiYok.Location = new System.Drawing.Point(787, 467);
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
            this.chkProjeIliskisiVar.Location = new System.Drawing.Point(670, 467);
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
            this.chkNakliyeYok.Location = new System.Drawing.Point(787, 655);
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
            this.chkNakliyeVar.Location = new System.Drawing.Point(670, 655);
            this.chkNakliyeVar.Name = "chkNakliyeVar";
            this.chkNakliyeVar.Size = new System.Drawing.Size(52, 24);
            this.chkNakliyeVar.TabIndex = 24;
            this.chkNakliyeVar.Text = "Var";
            this.chkNakliyeVar.UseVisualStyleBackColor = true;
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(205, 780);
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
            this.dtpSiparisSozlesmeTarihi.Location = new System.Drawing.Point(670, 517);
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
            this.txtProjeAra.Location = new System.Drawing.Point(150, 68);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(219, 27);
            this.txtProjeAra.TabIndex = 315;
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.Location = new System.Drawing.Point(375, 68);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(100, 28);
            this.btnAra.TabIndex = 316;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = false;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label11.Location = new System.Drawing.Point(28, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 23);
            this.label11.TabIndex = 317;
            this.label11.Text = "Proje No:";
            // 
            // lblAltProjeHata
            // 
            this.lblAltProjeHata.AutoSize = true;
            this.lblAltProjeHata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAltProjeHata.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lblAltProjeHata.Location = new System.Drawing.Point(666, 743);
            this.lblAltProjeHata.Name = "lblAltProjeHata";
            this.lblAltProjeHata.Size = new System.Drawing.Size(0, 25);
            this.lblAltProjeHata.TabIndex = 318;
            // 
            // chkEuro
            // 
            this.chkEuro.AutoSize = true;
            this.chkEuro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkEuro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkEuro.Location = new System.Drawing.Point(672, 567);
            this.chkEuro.Name = "chkEuro";
            this.chkEuro.Size = new System.Drawing.Size(83, 24);
            this.chkEuro.TabIndex = 321;
            this.chkEuro.Text = "Euro (€)";
            this.chkEuro.UseVisualStyleBackColor = true;
            // 
            // chkDolar
            // 
            this.chkDolar.AutoSize = true;
            this.chkDolar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkDolar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkDolar.Location = new System.Drawing.Point(815, 567);
            this.chkDolar.Name = "chkDolar";
            this.chkDolar.Size = new System.Drawing.Size(90, 24);
            this.chkDolar.TabIndex = 320;
            this.chkDolar.Text = "Dolar ($)";
            this.chkDolar.UseVisualStyleBackColor = true;
            // 
            // chkTL
            // 
            this.chkTL.AutoSize = true;
            this.chkTL.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkTL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.chkTL.Location = new System.Drawing.Point(963, 567);
            this.chkTL.Name = "chkTL";
            this.chkTL.Size = new System.Drawing.Size(119, 24);
            this.chkTL.TabIndex = 319;
            this.chkTL.Text = "Türk Lirası (₺)";
            this.chkTL.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label12.Location = new System.Drawing.Point(44, 568);
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
            this.label13.Location = new System.Drawing.Point(44, 613);
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
            this.chkCogul.Location = new System.Drawing.Point(787, 612);
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
            this.chkTekil.Location = new System.Drawing.Point(670, 612);
            this.chkTekil.Name = "chkTekil";
            this.chkTekil.Size = new System.Drawing.Size(61, 24);
            this.chkTekil.TabIndex = 324;
            this.chkTekil.Text = "Tekil";
            this.chkTekil.UseVisualStyleBackColor = true;
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
            this.ctlBaslik1.Size = new System.Drawing.Size(1297, 50);
            this.ctlBaslik1.TabIndex = 26;
            // 
            // ctlProjeKutuk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.chkCogul);
            this.Controls.Add(this.chkTekil);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkEuro);
            this.Controls.Add(this.chkDolar);
            this.Controls.Add(this.chkTL);
            this.Controls.Add(this.lblAltProjeHata);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.txtProjeAra);
            this.Controls.Add(this.dtpSiparisSozlesmeTarihi);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.ctlBaslik1);
            this.Controls.Add(this.chkNakliyeYok);
            this.Controls.Add(this.chkNakliyeVar);
            this.Controls.Add(this.chkProjeIliskisiYok);
            this.Controls.Add(this.chkProjeIliskisiVar);
            this.Controls.Add(this.chkAltProjeYok);
            this.Controls.Add(this.chkAltProjeVar);
            this.Controls.Add(this.txtToplamBedel);
            this.Controls.Add(this.txtProjeNo);
            this.Controls.Add(this.txtIsFirsatiNo);
            this.Controls.Add(this.txtTeklifNo);
            this.Controls.Add(this.txtMusteriAdi);
            this.Controls.Add(this.txtMusteriNo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ctlProjeKutuk";
            this.Size = new System.Drawing.Size(1297, 886);
            this.Load += new System.EventHandler(this.ctlProjeKutuk_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMusteriNo;
        private System.Windows.Forms.TextBox txtMusteriAdi;
        private System.Windows.Forms.TextBox txtTeklifNo;
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblAltProjeHata;
        private System.Windows.Forms.CheckBox chkEuro;
        private System.Windows.Forms.CheckBox chkDolar;
        private System.Windows.Forms.CheckBox chkTL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkCogul;
        private System.Windows.Forms.CheckBox chkTekil;
    }
}