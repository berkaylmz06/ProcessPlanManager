namespace CEKA_APP.Forms.KesimTakip
{
    partial class frmYanUrunGiris
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblAdet;
        private System.Windows.Forms.TextBox txtAdet;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.DataGridView dataGridYanUrunler; // Yeni eklendi

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
            this.lblEn = new System.Windows.Forms.Label();
            this.txtEn = new System.Windows.Forms.TextBox();
            this.lblBoy = new System.Windows.Forms.Label();
            this.txtBoy = new System.Windows.Forms.TextBox();
            this.btnSiradaki = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.lblKesimId = new System.Windows.Forms.Label();
            this.lblAdet = new System.Windows.Forms.Label();
            this.txtAdet = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.dataGridYanUrunler = new System.Windows.Forms.DataGridView(); // Yeni eklendi
            ((System.ComponentModel.ISupportInitialize)(this.dataGridYanUrunler)).BeginInit(); // Yeni eklendi
            this.SuspendLayout();
            // 
            // lblEn
            // 
            this.lblEn.AutoSize = true;
            this.lblEn.Location = new System.Drawing.Point(30, 70);
            this.lblEn.Name = "lblEn";
            this.lblEn.Size = new System.Drawing.Size(29, 16);
            this.lblEn.TabIndex = 0;
            this.lblEn.Text = "EN:";
            // 
            // txtEn
            // 
            this.txtEn.Location = new System.Drawing.Point(130, 70);
            this.txtEn.Name = "txtEn";
            this.txtEn.Size = new System.Drawing.Size(120, 22);
            this.txtEn.TabIndex = 1;
            // 
            // lblBoy
            // 
            this.lblBoy.AutoSize = true;
            this.lblBoy.Location = new System.Drawing.Point(30, 100);
            this.lblBoy.Name = "lblBoy";
            this.lblBoy.Size = new System.Drawing.Size(38, 16);
            this.lblBoy.TabIndex = 2;
            this.lblBoy.Text = "BOY:";
            // 
            // txtBoy
            // 
            this.txtBoy.Location = new System.Drawing.Point(130, 100);
            this.txtBoy.Name = "txtBoy";
            this.txtBoy.Size = new System.Drawing.Size(120, 22);
            this.txtBoy.TabIndex = 3;
            // 
            // btnSiradaki
            // 
            this.btnSiradaki.Location = new System.Drawing.Point(230, 430); // Konum güncellendi
            this.btnSiradaki.Name = "btnSiradaki";
            this.btnSiradaki.Size = new System.Drawing.Size(100, 30); // Boyut güncellendi
            this.btnSiradaki.TabIndex = 6; // Index güncellendi
            this.btnSiradaki.Text = "Sıradaki >";
            this.btnSiradaki.UseVisualStyleBackColor = true;
            this.btnSiradaki.Click += new System.EventHandler(this.btnSiradaki_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(120, 430); // Konum güncellendi
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(100, 30); // Boyut güncellendi
            this.btnIptal.TabIndex = 7; // Index güncellendi
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // lblKesimId
            // 
            this.lblKesimId.AutoSize = true;
            this.lblKesimId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKesimId.Location = new System.Drawing.Point(15, 20);
            this.lblKesimId.Name = "lblKesimId";
            this.lblKesimId.Size = new System.Drawing.Size(129, 18);
            this.lblKesimId.TabIndex = 6;
            this.lblKesimId.Text = "Kesim Planı No:";
            // 
            // lblAdet
            // 
            this.lblAdet.AutoSize = true;
            this.lblAdet.Location = new System.Drawing.Point(30, 130);
            this.lblAdet.Name = "lblAdet";
            this.lblAdet.Size = new System.Drawing.Size(41, 16);
            this.lblAdet.TabIndex = 8;
            this.lblAdet.Text = "Adet:";
            // 
            // txtAdet
            // 
            this.txtAdet.Location = new System.Drawing.Point(130, 130);
            this.txtAdet.Name = "txtAdet";
            this.txtAdet.Size = new System.Drawing.Size(120, 22);
            this.txtAdet.TabIndex = 4;
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(260, 100);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(70, 52); // Boyut güncellendi
            this.btnEkle.TabIndex = 5;
            this.btnEkle.Text = "EKLE";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // dataGridYanUrunler
            // 
            this.dataGridYanUrunler.AllowUserToAddRows = false;
            this.dataGridYanUrunler.AllowUserToDeleteRows = false;
            this.dataGridYanUrunler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridYanUrunler.Location = new System.Drawing.Point(15, 170);
            this.dataGridYanUrunler.Name = "dataGridYanUrunler";
            this.dataGridYanUrunler.RowHeadersWidth = 51;
            this.dataGridYanUrunler.RowTemplate.Height = 24;
            this.dataGridYanUrunler.Size = new System.Drawing.Size(315, 240);
            this.dataGridYanUrunler.TabIndex = 9;
            // 
            // frmYanUrunGiris
            // 
            this.ClientSize = new System.Drawing.Size(340, 470); // Boyut güncellendi
            this.Controls.Add(this.dataGridYanUrunler); // Yeni eklendi
            this.Controls.Add(this.btnEkle); // Yeni eklendi
            this.Controls.Add(this.txtAdet); // Yeni eklendi
            this.Controls.Add(this.lblAdet); // Yeni eklendi
            this.Controls.Add(this.lblKesimId);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnSiradaki);
            this.Controls.Add(this.txtBoy);
            this.Controls.Add(this.lblBoy);
            this.Controls.Add(this.txtEn);
            this.Controls.Add(this.lblEn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmYanUrunGiris";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yan Ürün Girişi";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridYanUrunler)).EndInit(); // Yeni eklendi
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEn;
        private System.Windows.Forms.TextBox txtEn;
        private System.Windows.Forms.Label lblBoy;
        private System.Windows.Forms.TextBox txtBoy;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Button btnSiradaki;
        private System.Windows.Forms.Label lblKesimId;
    }
}