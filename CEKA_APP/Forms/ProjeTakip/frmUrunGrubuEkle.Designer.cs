namespace CEKA_APP.Forms.ProjeTakip
{
    partial class frmUrunGrubuEkle
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSecilenleriEkle = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnAra = new System.Windows.Forms.Button();
            this.dataGridUrunGruplari = new System.Windows.Forms.DataGridView();
            this.btnSil = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUrunGruplari)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1132, 80);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(65, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ürün Grubu Ekle";
            // 
            // btnSecilenleriEkle
            // 
            this.btnSecilenleriEkle.Location = new System.Drawing.Point(949, 107);
            this.btnSecilenleriEkle.Name = "btnSecilenleriEkle";
            this.btnSecilenleriEkle.Size = new System.Drawing.Size(151, 34);
            this.btnSecilenleriEkle.TabIndex = 2;
            this.btnSecilenleriEkle.Text = "Seçilen Grupları Ekle";
            this.btnSecilenleriEkle.UseVisualStyleBackColor = true;
            this.btnSecilenleriEkle.Click += new System.EventHandler(this.btnSecilenleriEkle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(949, 773);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(151, 34);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(949, 813);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(151, 34);
            this.btnIptal.TabIndex = 4;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(949, 227);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(151, 34);
            this.btnAra.TabIndex = 5;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // dataGridUrunGruplari
            // 
            this.dataGridUrunGruplari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUrunGruplari.Location = new System.Drawing.Point(12, 95);
            this.dataGridUrunGruplari.Name = "dataGridUrunGruplari";
            this.dataGridUrunGruplari.RowHeadersWidth = 51;
            this.dataGridUrunGruplari.RowTemplate.Height = 24;
            this.dataGridUrunGruplari.Size = new System.Drawing.Size(910, 752);
            this.dataGridUrunGruplari.TabIndex = 6;
            this.dataGridUrunGruplari.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridUrunGruplari_ColumnHeaderMouseClick);
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(949, 147);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(151, 34);
            this.btnSil.TabIndex = 7;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // frmUrunGrubuEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 861);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.dataGridUrunGruplari);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.btnSecilenleriEkle);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "frmUrunGrubuEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ürün Grubu Ekle";
            this.Load += new System.EventHandler(this.frmUrunGrubuEkle_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUrunGruplari)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSecilenleriEkle;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.DataGridView dataGridUrunGruplari;
        private System.Windows.Forms.Button btnSil;
    }
}