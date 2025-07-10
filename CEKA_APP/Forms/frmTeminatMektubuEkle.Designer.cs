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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblMusteriNo = new System.Windows.Forms.Label();
            this.lblMusteri = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblTutar = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.lblBanka = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.lblMektupTur = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.chkTL = new System.Windows.Forms.CheckBox();
            this.chkDolar = new System.Windows.Forms.CheckBox();
            this.chkEuro = new System.Windows.Forms.CheckBox();
            this.lblVade = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lblIadeTarihi = new System.Windows.Forms.Label();
            this.lblKomisyonTutari = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.lblKomisyonOrani = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.lblKomisyonVadesi = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(192, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(279, 22);
            this.textBox1.TabIndex = 0;
            // 
            // lblMusteriNo
            // 
            this.lblMusteriNo.AutoSize = true;
            this.lblMusteriNo.Location = new System.Drawing.Point(68, 59);
            this.lblMusteriNo.Name = "lblMusteriNo";
            this.lblMusteriNo.Size = new System.Drawing.Size(77, 16);
            this.lblMusteriNo.TabIndex = 1;
            this.lblMusteriNo.Text = "Mektup NO:";
            // 
            // lblMusteri
            // 
            this.lblMusteri.AutoSize = true;
            this.lblMusteri.Location = new System.Drawing.Point(68, 106);
            this.lblMusteri.Name = "lblMusteri";
            this.lblMusteri.Size = new System.Drawing.Size(53, 16);
            this.lblMusteri.TabIndex = 3;
            this.lblMusteri.Text = "Müşteri:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(192, 103);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(279, 22);
            this.textBox2.TabIndex = 2;
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Location = new System.Drawing.Point(68, 209);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(41, 16);
            this.lblTutar.TabIndex = 5;
            this.lblTutar.Text = "Tutar:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(192, 206);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(279, 22);
            this.textBox3.TabIndex = 4;
            // 
            // lblBanka
            // 
            this.lblBanka.AutoSize = true;
            this.lblBanka.Location = new System.Drawing.Point(68, 266);
            this.lblBanka.Name = "lblBanka";
            this.lblBanka.Size = new System.Drawing.Size(49, 16);
            this.lblBanka.TabIndex = 7;
            this.lblBanka.Text = "Banka:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(192, 263);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(279, 22);
            this.textBox4.TabIndex = 6;
            // 
            // lblMektupTur
            // 
            this.lblMektupTur.AutoSize = true;
            this.lblMektupTur.Location = new System.Drawing.Point(68, 325);
            this.lblMektupTur.Name = "lblMektupTur";
            this.lblMektupTur.Size = new System.Drawing.Size(84, 16);
            this.lblMektupTur.TabIndex = 9;
            this.lblMektupTur.Text = "Mektup Türü:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(192, 322);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(279, 22);
            this.textBox5.TabIndex = 8;
            // 
            // chkTL
            // 
            this.chkTL.AutoSize = true;
            this.chkTL.Location = new System.Drawing.Point(362, 158);
            this.chkTL.Name = "chkTL";
            this.chkTL.Size = new System.Drawing.Size(109, 20);
            this.chkTL.TabIndex = 10;
            this.chkTL.Text = "Türk Lirası (₺)";
            this.chkTL.UseVisualStyleBackColor = true;
            // 
            // chkDolar
            // 
            this.chkDolar.AutoSize = true;
            this.chkDolar.Location = new System.Drawing.Point(214, 158);
            this.chkDolar.Name = "chkDolar";
            this.chkDolar.Size = new System.Drawing.Size(80, 20);
            this.chkDolar.TabIndex = 11;
            this.chkDolar.Text = "Dolar ($)";
            this.chkDolar.UseVisualStyleBackColor = true;
            // 
            // chkEuro
            // 
            this.chkEuro.AutoSize = true;
            this.chkEuro.Location = new System.Drawing.Point(71, 158);
            this.chkEuro.Name = "chkEuro";
            this.chkEuro.Size = new System.Drawing.Size(75, 20);
            this.chkEuro.TabIndex = 12;
            this.chkEuro.Text = "Euro (€)";
            this.chkEuro.UseVisualStyleBackColor = true;
            // 
            // lblVade
            // 
            this.lblVade.AutoSize = true;
            this.lblVade.Location = new System.Drawing.Point(68, 386);
            this.lblVade.Name = "lblVade";
            this.lblVade.Size = new System.Drawing.Size(43, 16);
            this.lblVade.TabIndex = 14;
            this.lblVade.Text = "Vade:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(192, 380);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(279, 22);
            this.dateTimePicker1.TabIndex = 15;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(192, 440);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(279, 22);
            this.dateTimePicker2.TabIndex = 17;
            // 
            // lblIadeTarihi
            // 
            this.lblIadeTarihi.AutoSize = true;
            this.lblIadeTarihi.Location = new System.Drawing.Point(68, 446);
            this.lblIadeTarihi.Name = "lblIadeTarihi";
            this.lblIadeTarihi.Size = new System.Drawing.Size(74, 16);
            this.lblIadeTarihi.TabIndex = 16;
            this.lblIadeTarihi.Text = "İade Tarihi:";
            // 
            // lblKomisyonTutari
            // 
            this.lblKomisyonTutari.AutoSize = true;
            this.lblKomisyonTutari.Location = new System.Drawing.Point(68, 514);
            this.lblKomisyonTutari.Name = "lblKomisyonTutari";
            this.lblKomisyonTutari.Size = new System.Drawing.Size(106, 16);
            this.lblKomisyonTutari.TabIndex = 19;
            this.lblKomisyonTutari.Text = "Komisyon Tutarı:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(192, 511);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(279, 22);
            this.textBox6.TabIndex = 18;
            // 
            // lblKomisyonOrani
            // 
            this.lblKomisyonOrani.AutoSize = true;
            this.lblKomisyonOrani.Location = new System.Drawing.Point(68, 574);
            this.lblKomisyonOrani.Name = "lblKomisyonOrani";
            this.lblKomisyonOrani.Size = new System.Drawing.Size(104, 16);
            this.lblKomisyonOrani.TabIndex = 21;
            this.lblKomisyonOrani.Text = "Komisyon Oranı:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(192, 571);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(279, 22);
            this.textBox7.TabIndex = 20;
            // 
            // lblKomisyonVadesi
            // 
            this.lblKomisyonVadesi.AutoSize = true;
            this.lblKomisyonVadesi.Location = new System.Drawing.Point(68, 636);
            this.lblKomisyonVadesi.Name = "lblKomisyonVadesi";
            this.lblKomisyonVadesi.Size = new System.Drawing.Size(115, 16);
            this.lblKomisyonVadesi.TabIndex = 23;
            this.lblKomisyonVadesi.Text = "Komisyon Vadesi:";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(192, 633);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(279, 22);
            this.textBox8.TabIndex = 22;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(158, 695);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(202, 51);
            this.btnKaydet.TabIndex = 24;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            // 
            // frmTeminatMektubuEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 781);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.lblKomisyonVadesi);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.lblKomisyonOrani);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.lblKomisyonTutari);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.lblIadeTarihi);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.lblVade);
            this.Controls.Add(this.chkEuro);
            this.Controls.Add(this.chkDolar);
            this.Controls.Add(this.chkTL);
            this.Controls.Add(this.lblMektupTur);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.lblBanka);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.lblTutar);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.lblMusteri);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.lblMusteriNo);
            this.Controls.Add(this.textBox1);
            this.Name = "frmTeminatMektubuEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Teminat Mektubu Ekle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblMusteriNo;
        private System.Windows.Forms.Label lblMusteri;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblTutar;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label lblBanka;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label lblMektupTur;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.CheckBox chkTL;
        private System.Windows.Forms.CheckBox chkDolar;
        private System.Windows.Forms.CheckBox chkEuro;
        private System.Windows.Forms.Label lblVade;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label lblIadeTarihi;
        private System.Windows.Forms.Label lblKomisyonTutari;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label lblKomisyonOrani;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label lblKomisyonVadesi;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button btnKaydet;
    }
}