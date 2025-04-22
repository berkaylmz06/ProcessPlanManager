namespace KesimTakip
{
    partial class frmYoneticiEkrani
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anaSayfaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kullanıcıAyarlarıToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sorunBildirimeriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelKullaniciAyar = new System.Windows.Forms.Panel();
            this.btnKullaniciEkle = new System.Windows.Forms.Button();
            this.panelAnaSayfa = new System.Windows.Forms.Panel();
            this.dataGridKesimListesi = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSorunlar = new System.Windows.Forms.Button();
            this.btnKullaniciAyarlari = new System.Windows.Forms.Button();
            this.panelSorunBildirimleri = new System.Windows.Forms.Panel();
            this.txtBildiriZamani = new System.Windows.Forms.TextBox();
            this.txtBildiriYapanKullanici = new System.Windows.Forms.TextBox();
            this.txtSorunBildirimi = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridSorunBildirimleri = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnKesimDetaylari = new System.Windows.Forms.Button();
            this.panelKesimDetaylari = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.panelKullaniciAyar.SuspendLayout();
            this.panelAnaSayfa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).BeginInit();
            this.panelSorunBildirimleri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSorunBildirimleri)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1473, 36);
            this.menuStrip1.TabIndex = 152;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.anaSayfaToolStripMenuItem,
            this.kullanıcıAyarlarıToolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // anaSayfaToolStripMenuItem
            // 
            this.anaSayfaToolStripMenuItem.Name = "anaSayfaToolStripMenuItem";
            this.anaSayfaToolStripMenuItem.Size = new System.Drawing.Size(241, 34);
            this.anaSayfaToolStripMenuItem.Text = "Ana Sayfa";
            this.anaSayfaToolStripMenuItem.Click += new System.EventHandler(this.anaSayfaToolStripMenuItem_Click);
            // 
            // kullanıcıAyarlarıToolStripMenuItem1
            // 
            this.kullanıcıAyarlarıToolStripMenuItem1.Name = "kullanıcıAyarlarıToolStripMenuItem1";
            this.kullanıcıAyarlarıToolStripMenuItem1.Size = new System.Drawing.Size(241, 34);
            this.kullanıcıAyarlarıToolStripMenuItem1.Text = "Kullanıcı Ayarları";
            this.kullanıcıAyarlarıToolStripMenuItem1.Click += new System.EventHandler(this.kullanıcıAyarlarıToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(241, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sorunBildirimeriToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // sorunBildirimeriToolStripMenuItem
            // 
            this.sorunBildirimeriToolStripMenuItem.Name = "sorunBildirimeriToolStripMenuItem";
            this.sorunBildirimeriToolStripMenuItem.Size = new System.Drawing.Size(244, 34);
            this.sorunBildirimeriToolStripMenuItem.Text = "Sorun Bildirimeri";
            this.sorunBildirimeriToolStripMenuItem.Click += new System.EventHandler(this.sorunBildirimeriToolStripMenuItem_Click);
            // 
            // panelKullaniciAyar
            // 
            this.panelKullaniciAyar.BackColor = System.Drawing.SystemColors.Window;
            this.panelKullaniciAyar.Controls.Add(this.btnKullaniciEkle);
            this.panelKullaniciAyar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKullaniciAyar.Location = new System.Drawing.Point(0, 0);
            this.panelKullaniciAyar.Name = "panelKullaniciAyar";
            this.panelKullaniciAyar.Size = new System.Drawing.Size(1473, 876);
            this.panelKullaniciAyar.TabIndex = 0;
            // 
            // btnKullaniciEkle
            // 
            this.btnKullaniciEkle.Location = new System.Drawing.Point(275, 86);
            this.btnKullaniciEkle.Name = "btnKullaniciEkle";
            this.btnKullaniciEkle.Size = new System.Drawing.Size(130, 34);
            this.btnKullaniciEkle.TabIndex = 0;
            this.btnKullaniciEkle.Text = "Kullanıcı Ekle";
            this.btnKullaniciEkle.UseVisualStyleBackColor = true;
            this.btnKullaniciEkle.Click += new System.EventHandler(this.btnKullaniciEkle_Click);
            // 
            // panelAnaSayfa
            // 
            this.panelAnaSayfa.BackColor = System.Drawing.SystemColors.Window;
            this.panelAnaSayfa.Controls.Add(this.dataGridKesimListesi);
            this.panelAnaSayfa.Controls.Add(this.label1);
            this.panelAnaSayfa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAnaSayfa.Location = new System.Drawing.Point(0, 0);
            this.panelAnaSayfa.Name = "panelAnaSayfa";
            this.panelAnaSayfa.Size = new System.Drawing.Size(1473, 876);
            this.panelAnaSayfa.TabIndex = 0;
            // 
            // dataGridKesimListesi
            // 
            this.dataGridKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKesimListesi.Location = new System.Drawing.Point(265, 89);
            this.dataGridKesimListesi.Name = "dataGridKesimListesi";
            this.dataGridKesimListesi.RowHeadersVisible = false;
            this.dataGridKesimListesi.RowHeadersWidth = 62;
            this.dataGridKesimListesi.RowTemplate.Height = 28;
            this.dataGridKesimListesi.Size = new System.Drawing.Size(821, 384);
            this.dataGridKesimListesi.TabIndex = 115;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 20);
            this.label1.TabIndex = 114;
            this.label1.Text = "Kesim Listesi Aşağıda Listelenmektedir.";
            // 
            // btnSorunlar
            // 
            this.btnSorunlar.Location = new System.Drawing.Point(41, 201);
            this.btnSorunlar.Name = "btnSorunlar";
            this.btnSorunlar.Size = new System.Drawing.Size(157, 66);
            this.btnSorunlar.TabIndex = 117;
            this.btnSorunlar.Text = "İletilen Sorunlar";
            this.btnSorunlar.UseVisualStyleBackColor = true;
            this.btnSorunlar.Click += new System.EventHandler(this.btnSorunlar_Click);
            // 
            // btnKullaniciAyarlari
            // 
            this.btnKullaniciAyarlari.Location = new System.Drawing.Point(41, 121);
            this.btnKullaniciAyarlari.Name = "btnKullaniciAyarlari";
            this.btnKullaniciAyarlari.Size = new System.Drawing.Size(157, 65);
            this.btnKullaniciAyarlari.TabIndex = 116;
            this.btnKullaniciAyarlari.Text = "Kullanıcı Ayarları";
            this.btnKullaniciAyarlari.UseVisualStyleBackColor = true;
            this.btnKullaniciAyarlari.Click += new System.EventHandler(this.btnKullaniciAyarlari_Click);
            // 
            // panelSorunBildirimleri
            // 
            this.panelSorunBildirimleri.BackColor = System.Drawing.SystemColors.Window;
            this.panelSorunBildirimleri.Controls.Add(this.txtBildiriZamani);
            this.panelSorunBildirimleri.Controls.Add(this.txtBildiriYapanKullanici);
            this.panelSorunBildirimleri.Controls.Add(this.txtSorunBildirimi);
            this.panelSorunBildirimleri.Controls.Add(this.label4);
            this.panelSorunBildirimleri.Controls.Add(this.label3);
            this.panelSorunBildirimleri.Controls.Add(this.label2);
            this.panelSorunBildirimleri.Controls.Add(this.dataGridSorunBildirimleri);
            this.panelSorunBildirimleri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSorunBildirimleri.Location = new System.Drawing.Point(0, 0);
            this.panelSorunBildirimleri.Name = "panelSorunBildirimleri";
            this.panelSorunBildirimleri.Size = new System.Drawing.Size(1473, 876);
            this.panelSorunBildirimleri.TabIndex = 116;
            // 
            // txtBildiriZamani
            // 
            this.txtBildiriZamani.Enabled = false;
            this.txtBildiriZamani.Location = new System.Drawing.Point(1403, 397);
            this.txtBildiriZamani.Name = "txtBildiriZamani";
            this.txtBildiriZamani.Size = new System.Drawing.Size(443, 26);
            this.txtBildiriZamani.TabIndex = 123;
            // 
            // txtBildiriYapanKullanici
            // 
            this.txtBildiriYapanKullanici.Enabled = false;
            this.txtBildiriYapanKullanici.Location = new System.Drawing.Point(1403, 126);
            this.txtBildiriYapanKullanici.Name = "txtBildiriYapanKullanici";
            this.txtBildiriYapanKullanici.Size = new System.Drawing.Size(443, 26);
            this.txtBildiriYapanKullanici.TabIndex = 122;
            // 
            // txtSorunBildirimi
            // 
            this.txtSorunBildirimi.Enabled = false;
            this.txtSorunBildirimi.Location = new System.Drawing.Point(1403, 190);
            this.txtSorunBildirimi.Name = "txtSorunBildirimi";
            this.txtSorunBildirimi.Size = new System.Drawing.Size(443, 174);
            this.txtSorunBildirimi.TabIndex = 121;
            this.txtSorunBildirimi.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1222, 400);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 119;
            this.label4.Text = "Bildiri Zamanı:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1226, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 118;
            this.label3.Text = "Sorun:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1226, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 20);
            this.label2.TabIndex = 117;
            this.label2.Text = "Bildiri Yapan Kullanıcı:";
            // 
            // dataGridSorunBildirimleri
            // 
            this.dataGridSorunBildirimleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridSorunBildirimleri.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridSorunBildirimleri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSorunBildirimleri.Location = new System.Drawing.Point(287, 89);
            this.dataGridSorunBildirimleri.Name = "dataGridSorunBildirimleri";
            this.dataGridSorunBildirimleri.RowHeadersVisible = false;
            this.dataGridSorunBildirimleri.RowHeadersWidth = 62;
            this.dataGridSorunBildirimleri.RowTemplate.Height = 28;
            this.dataGridSorunBildirimleri.Size = new System.Drawing.Size(846, 364);
            this.dataGridSorunBildirimleri.TabIndex = 116;
            this.dataGridSorunBildirimleri.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridSorunBildirimleri_CellClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Controls.Add(this.btnKesimDetaylari);
            this.panel1.Controls.Add(this.btnSorunlar);
            this.panel1.Controls.Add(this.btnKullaniciAyarlari);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 840);
            this.panel1.TabIndex = 124;
            // 
            // btnKesimDetaylari
            // 
            this.btnKesimDetaylari.Location = new System.Drawing.Point(41, 37);
            this.btnKesimDetaylari.Name = "btnKesimDetaylari";
            this.btnKesimDetaylari.Size = new System.Drawing.Size(157, 66);
            this.btnKesimDetaylari.TabIndex = 118;
            this.btnKesimDetaylari.Text = "Kesim Detayları";
            this.btnKesimDetaylari.UseVisualStyleBackColor = true;
            this.btnKesimDetaylari.Click += new System.EventHandler(this.btnKesimDetaylari_Click);
            // 
            // panelKesimDetaylari
            // 
            this.panelKesimDetaylari.BackColor = System.Drawing.SystemColors.Window;
            this.panelKesimDetaylari.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKesimDetaylari.Location = new System.Drawing.Point(0, 0);
            this.panelKesimDetaylari.Name = "panelKesimDetaylari";
            this.panelKesimDetaylari.Size = new System.Drawing.Size(1473, 876);
            this.panelKesimDetaylari.TabIndex = 1;
            // 
            // frmYoneticiEkrani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1473, 876);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelAnaSayfa);
            this.Controls.Add(this.panelKesimDetaylari);
            this.Controls.Add(this.panelKullaniciAyar);
            this.Controls.Add(this.panelSorunBildirimleri);
            this.Name = "frmYoneticiEkrani";
            this.Text = "Yönet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmYoneticiEkrani_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelKullaniciAyar.ResumeLayout(false);
            this.panelAnaSayfa.ResumeLayout(false);
            this.panelAnaSayfa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).EndInit();
            this.panelSorunBildirimleri.ResumeLayout(false);
            this.panelSorunBildirimleri.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSorunBildirimleri)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sorunBildirimeriToolStripMenuItem;
        private System.Windows.Forms.Panel panelKullaniciAyar;
        private System.Windows.Forms.Panel panelAnaSayfa;
        private System.Windows.Forms.Button btnKullaniciEkle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem kullanıcıAyarlarıToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem anaSayfaToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridKesimListesi;
        private System.Windows.Forms.Panel panelSorunBildirimleri;
        private System.Windows.Forms.DataGridView dataGridSorunBildirimleri;
        private System.Windows.Forms.Button btnSorunlar;
        private System.Windows.Forms.Button btnKullaniciAyarlari;
        private System.Windows.Forms.TextBox txtBildiriZamani;
        private System.Windows.Forms.TextBox txtBildiriYapanKullanici;
        private System.Windows.Forms.RichTextBox txtSorunBildirimi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnKesimDetaylari;
        private System.Windows.Forms.Panel panelKesimDetaylari;
    }
}