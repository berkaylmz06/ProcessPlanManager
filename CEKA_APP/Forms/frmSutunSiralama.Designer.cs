namespace CEKA_APP.Forms
{
    partial class frmSutunSiralama
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
            this.lstSutunlar = new System.Windows.Forms.ListBox();
            this.btnYukariTasi = new System.Windows.Forms.Button();
            this.btnAsagiTasi = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.btnEnAltaTasi = new System.Windows.Forms.Button();
            this.btnEnUsteTasi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstSutunlar
            // 
            this.lstSutunlar.FormattingEnabled = true;
            this.lstSutunlar.ItemHeight = 16;
            this.lstSutunlar.Location = new System.Drawing.Point(12, 12);
            this.lstSutunlar.Name = "lstSutunlar";
            this.lstSutunlar.Size = new System.Drawing.Size(314, 420);
            this.lstSutunlar.TabIndex = 0;
            // 
            // btnYukariTasi
            // 
            this.btnYukariTasi.Location = new System.Drawing.Point(333, 163);
            this.btnYukariTasi.Name = "btnYukariTasi";
            this.btnYukariTasi.Size = new System.Drawing.Size(56, 37);
            this.btnYukariTasi.TabIndex = 1;
            this.btnYukariTasi.Text = "▲";
            this.btnYukariTasi.UseVisualStyleBackColor = true;
            this.btnYukariTasi.Click += new System.EventHandler(this.btnYukariTasi_Click);
            // 
            // btnAsagiTasi
            // 
            this.btnAsagiTasi.Location = new System.Drawing.Point(333, 218);
            this.btnAsagiTasi.Name = "btnAsagiTasi";
            this.btnAsagiTasi.Size = new System.Drawing.Size(56, 40);
            this.btnAsagiTasi.TabIndex = 2;
            this.btnAsagiTasi.Text = "▼";
            this.btnAsagiTasi.UseVisualStyleBackColor = true;
            this.btnAsagiTasi.Click += new System.EventHandler(this.btnAsagiTasi_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(405, 383);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(66, 49);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Tamam";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Location = new System.Drawing.Point(333, 383);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(66, 49);
            this.btnIptal.TabIndex = 4;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // btnEnAltaTasi
            // 
            this.btnEnAltaTasi.Location = new System.Drawing.Point(333, 264);
            this.btnEnAltaTasi.Name = "btnEnAltaTasi";
            this.btnEnAltaTasi.Size = new System.Drawing.Size(56, 40);
            this.btnEnAltaTasi.TabIndex = 5;
            this.btnEnAltaTasi.Text = "⯯";
            this.btnEnAltaTasi.UseVisualStyleBackColor = true;
            this.btnEnAltaTasi.Click += new System.EventHandler(this.btnEnAltaTasi_Click);
            // 
            // btnEnUsteTasi
            // 
            this.btnEnUsteTasi.Location = new System.Drawing.Point(333, 120);
            this.btnEnUsteTasi.Name = "btnEnUsteTasi";
            this.btnEnUsteTasi.Size = new System.Drawing.Size(56, 37);
            this.btnEnUsteTasi.TabIndex = 6;
            this.btnEnUsteTasi.Text = "⯭";
            this.btnEnUsteTasi.UseVisualStyleBackColor = true;
            this.btnEnUsteTasi.Click += new System.EventHandler(this.btnEnUsteTasi_Click);
            // 
            // frmSutunSiralama
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 450);
            this.Controls.Add(this.btnEnUsteTasi);
            this.Controls.Add(this.btnEnAltaTasi);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.btnAsagiTasi);
            this.Controls.Add(this.btnYukariTasi);
            this.Controls.Add(this.lstSutunlar);
            this.MaximizeBox = false;
            this.Name = "frmSutunSiralama";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sütun Sıralama";
            this.Load += new System.EventHandler(this.frmSutunSiralama_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstSutunlar;
        private System.Windows.Forms.Button btnYukariTasi;
        private System.Windows.Forms.Button btnAsagiTasi;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.Button btnEnAltaTasi;
        private System.Windows.Forms.Button btnEnUsteTasi;
    }
}