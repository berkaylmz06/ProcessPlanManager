namespace CEKA_APP
{
    partial class frmStandartProjeler
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.btnProjeOlustur = new System.Windows.Forms.Button();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtOlusturmaTarihi = new System.Windows.Forms.DateTimePicker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtProjeBagla = new System.Windows.Forms.TextBox();
            this.btnProjeBagla = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.txtGrupNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEkle = new System.Windows.Forms.Button();
            this.listGruplar = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Açıklama:";
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(164, 80);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(217, 22);
            this.txtAciklama.TabIndex = 2;
            // 
            // btnProjeOlustur
            // 
            this.btnProjeOlustur.Location = new System.Drawing.Point(117, 182);
            this.btnProjeOlustur.Name = "btnProjeOlustur";
            this.btnProjeOlustur.Size = new System.Drawing.Size(172, 43);
            this.btnProjeOlustur.TabIndex = 3;
            this.btnProjeOlustur.Text = "Proje Oluştur";
            this.btnProjeOlustur.UseVisualStyleBackColor = true;
            this.btnProjeOlustur.Click += new System.EventHandler(this.btnProjeOlustur_Click);
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(164, 37);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(217, 22);
            this.txtProjeNo.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Proje No:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Oluşturma Tarihi:";
            // 
            // dtOlusturmaTarihi
            // 
            this.dtOlusturmaTarihi.Enabled = false;
            this.dtOlusturmaTarihi.Location = new System.Drawing.Point(164, 125);
            this.dtOlusturmaTarihi.Name = "dtOlusturmaTarihi";
            this.dtOlusturmaTarihi.Size = new System.Drawing.Size(217, 22);
            this.dtOlusturmaTarihi.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 276);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.dtOlusturmaTarihi);
            this.tabPage1.Controls.Add(this.txtAciklama);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnProjeOlustur);
            this.tabPage1.Controls.Add(this.txtProjeNo);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(416, 247);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Standart Proje Oluştur";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.txtProjeBagla);
            this.tabPage2.Controls.Add(this.btnProjeBagla);
            this.tabPage2.Controls.Add(this.btnSil);
            this.tabPage2.Controls.Add(this.txtGrupNo);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnEkle);
            this.tabPage2.Controls.Add(this.listGruplar);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(416, 247);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Standart Grup Oluştur";
            // 
            // txtProjeBagla
            // 
            this.txtProjeBagla.Location = new System.Drawing.Point(226, 158);
            this.txtProjeBagla.Name = "txtProjeBagla";
            this.txtProjeBagla.Size = new System.Drawing.Size(172, 22);
            this.txtProjeBagla.TabIndex = 5;
            // 
            // btnProjeBagla
            // 
            this.btnProjeBagla.Location = new System.Drawing.Point(226, 186);
            this.btnProjeBagla.Name = "btnProjeBagla";
            this.btnProjeBagla.Size = new System.Drawing.Size(172, 41);
            this.btnProjeBagla.TabIndex = 4;
            this.btnProjeBagla.Text = "Projeye Bağla";
            this.btnProjeBagla.UseVisualStyleBackColor = true;
            this.btnProjeBagla.Click += new System.EventHandler(this.btnProjeBagla_Click);
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(315, 98);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(83, 41);
            this.btnSil.TabIndex = 3;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // txtGrupNo
            // 
            this.txtGrupNo.Location = new System.Drawing.Point(226, 49);
            this.txtGrupNo.Name = "txtGrupNo";
            this.txtGrupNo.Size = new System.Drawing.Size(172, 22);
            this.txtGrupNo.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(264, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Grup No Ekle";
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(226, 98);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(83, 41);
            this.btnEkle.TabIndex = 2;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // listGruplar
            // 
            this.listGruplar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listGruplar.Dock = System.Windows.Forms.DockStyle.Left;
            this.listGruplar.FormattingEnabled = true;
            this.listGruplar.ItemHeight = 16;
            this.listGruplar.Location = new System.Drawing.Point(3, 3);
            this.listGruplar.Name = "listGruplar";
            this.listGruplar.Size = new System.Drawing.Size(200, 241);
            this.listGruplar.TabIndex = 0;
            // 
            // frmStandartProjeler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 276);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.Name = "frmStandartProjeler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Standart Projeler";
            this.Load += new System.EventHandler(this.frmStandartProjeler_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Button btnProjeOlustur;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtOlusturmaTarihi;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listGruplar;
        private System.Windows.Forms.TextBox txtGrupNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnProjeBagla;
        private System.Windows.Forms.TextBox txtProjeBagla;
    }
}