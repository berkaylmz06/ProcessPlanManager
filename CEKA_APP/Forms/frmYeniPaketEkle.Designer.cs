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
            this.listPaketler = new System.Windows.Forms.ListBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.panelYeniPaket = new System.Windows.Forms.Panel();
            this.lblPaketAdiBilgi = new System.Windows.Forms.Label();
            this.lblPaketAdi = new System.Windows.Forms.Label();
            this.txtPaketAdi = new System.Windows.Forms.TextBox();
            this.panelYeniPaket.SuspendLayout();
            this.SuspendLayout();
            // 
            // listPaketler
            // 
            this.listPaketler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPaketler.FormattingEnabled = true;
            this.listPaketler.ItemHeight = 16;
            this.listPaketler.Location = new System.Drawing.Point(20, 20);
            this.listPaketler.Name = "listPaketler";
            this.listPaketler.Size = new System.Drawing.Size(345, 196);
            this.listPaketler.TabIndex = 17;
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.Gray;
            this.btnEkle.FlatAppearance.BorderSize = 0;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.ForeColor = System.Drawing.Color.White;
            this.btnEkle.Location = new System.Drawing.Point(20, 237);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(345, 45);
            this.btnEkle.TabIndex = 21;
            this.btnEkle.Text = "Yeni Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // panelYeniPaket
            // 
            this.panelYeniPaket.Controls.Add(this.lblPaketAdiBilgi);
            this.panelYeniPaket.Controls.Add(this.lblPaketAdi);
            this.panelYeniPaket.Controls.Add(this.txtPaketAdi);
            this.panelYeniPaket.Location = new System.Drawing.Point(10, 290);
            this.panelYeniPaket.Name = "panelYeniPaket";
            this.panelYeniPaket.Size = new System.Drawing.Size(365, 80);
            this.panelYeniPaket.TabIndex = 23;
            this.panelYeniPaket.Visible = false;
            // 
            // lblPaketAdiBilgi
            // 
            this.lblPaketAdiBilgi.AutoSize = true;
            this.lblPaketAdiBilgi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPaketAdiBilgi.ForeColor = System.Drawing.Color.DimGray;
            this.lblPaketAdiBilgi.Location = new System.Drawing.Point(125, 12);
            this.lblPaketAdiBilgi.Name = "lblPaketAdiBilgi";
            this.lblPaketAdiBilgi.Size = new System.Drawing.Size(163, 20);
            this.lblPaketAdiBilgi.TabIndex = 20;
            this.lblPaketAdiBilgi.Text = "Yeni paket adını girin";
            // 
            // lblPaketAdi
            // 
            this.lblPaketAdi.AutoSize = true;
            this.lblPaketAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPaketAdi.Location = new System.Drawing.Point(13, 47);
            this.lblPaketAdi.Name = "lblPaketAdi";
            this.lblPaketAdi.Size = new System.Drawing.Size(85, 20);
            this.lblPaketAdi.TabIndex = 19;
            this.lblPaketAdi.Text = "Paket Adı:";
            // 
            // txtPaketAdi
            // 
            this.txtPaketAdi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPaketAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtPaketAdi.Location = new System.Drawing.Point(123, 44);
            this.txtPaketAdi.Name = "txtPaketAdi";
            this.txtPaketAdi.Size = new System.Drawing.Size(222, 26);
            this.txtPaketAdi.TabIndex = 18;
            // 
            // frmYeniPaketEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(385, 290);
            this.Controls.Add(this.panelYeniPaket);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.listPaketler);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmYeniPaketEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paket Ekle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniPaketEkle_FormClosing);
            this.panelYeniPaket.ResumeLayout(false);
            this.panelYeniPaket.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listPaketler;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Panel panelYeniPaket;
        private System.Windows.Forms.Label lblPaketAdiBilgi;
        private System.Windows.Forms.Label lblPaketAdi;
        private System.Windows.Forms.TextBox txtPaketAdi;
    }
}