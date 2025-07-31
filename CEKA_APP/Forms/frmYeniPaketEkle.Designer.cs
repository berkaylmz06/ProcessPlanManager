namespace CEKA_APP.Forms
{
    partial class frmYeniPaketEkle
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
            this.lblPaketAdi = new System.Windows.Forms.Label();
            this.lblPaketAdiBilgi = new System.Windows.Forms.Label();
            this.txtPaketAdi = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.listPaketler = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblPaketAdi
            // 
            this.lblPaketAdi.AutoSize = true;
            this.lblPaketAdi.Location = new System.Drawing.Point(19, 318);
            this.lblPaketAdi.Name = "lblPaketAdi";
            this.lblPaketAdi.Size = new System.Drawing.Size(68, 16);
            this.lblPaketAdi.TabIndex = 22;
            this.lblPaketAdi.Text = "Paket Adı:";
            // 
            // lblPaketAdiBilgi
            // 
            this.lblPaketAdiBilgi.AutoSize = true;
            this.lblPaketAdiBilgi.Location = new System.Drawing.Point(9, 279);
            this.lblPaketAdiBilgi.Name = "lblPaketAdiBilgi";
            this.lblPaketAdiBilgi.Size = new System.Drawing.Size(296, 16);
            this.lblPaketAdiBilgi.TabIndex = 21;
            this.lblPaketAdiBilgi.Text = "Lütfen eklemek istediğiniz yeni paket adını giriniz.";
            // 
            // txtPaketAdi
            // 
            this.txtPaketAdi.Location = new System.Drawing.Point(98, 315);
            this.txtPaketAdi.Name = "txtPaketAdi";
            this.txtPaketAdi.Size = new System.Drawing.Size(176, 22);
            this.txtPaketAdi.TabIndex = 20;
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(98, 366);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(116, 38);
            this.btnEkle.TabIndex = 19;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // listPaketler
            // 
            this.listPaketler.FormattingEnabled = true;
            this.listPaketler.ItemHeight = 16;
            this.listPaketler.Location = new System.Drawing.Point(12, 23);
            this.listPaketler.Name = "listPaketler";
            this.listPaketler.Size = new System.Drawing.Size(291, 244);
            this.listPaketler.TabIndex = 17;
            // 
            // frmYeniPaketEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 416);
            this.Controls.Add(this.lblPaketAdi);
            this.Controls.Add(this.lblPaketAdiBilgi);
            this.Controls.Add(this.txtPaketAdi);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.listPaketler);
            this.Name = "frmYeniPaketEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paket Ekle";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPaketAdi;
        private System.Windows.Forms.Label lblPaketAdiBilgi;
        private System.Windows.Forms.TextBox txtPaketAdi;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.ListBox listPaketler;
    }
}