namespace CEKA_APP
{
    partial class frmYeniFiyatlandirmaKalemiEkle
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
            this.listKalemler = new System.Windows.Forms.ListBox();
            this.btnSec = new System.Windows.Forms.Button();
            this.btnEkle = new System.Windows.Forms.Button();
            this.txtYeniKalem = new System.Windows.Forms.TextBox();
            this.lblYeniKalemBilgi = new System.Windows.Forms.Label();
            this.txtYeniKalemBirim = new System.Windows.Forms.TextBox();
            this.lblYeniKalemBirim = new System.Windows.Forms.Label();
            this.lblYeniKalemAdi = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listKalemler
            // 
            this.listKalemler.FormattingEnabled = true;
            this.listKalemler.ItemHeight = 16;
            this.listKalemler.Location = new System.Drawing.Point(12, 27);
            this.listKalemler.Name = "listKalemler";
            this.listKalemler.Size = new System.Drawing.Size(345, 244);
            this.listKalemler.TabIndex = 0;
            // 
            // btnSec
            // 
            this.btnSec.Location = new System.Drawing.Point(37, 378);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(116, 38);
            this.btnSec.TabIndex = 1;
            this.btnSec.Text = "Seç";
            this.btnSec.UseVisualStyleBackColor = true;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(196, 378);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(116, 38);
            this.btnEkle.TabIndex = 2;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // txtYeniKalem
            // 
            this.txtYeniKalem.Location = new System.Drawing.Point(164, 305);
            this.txtYeniKalem.Name = "txtYeniKalem";
            this.txtYeniKalem.Size = new System.Drawing.Size(176, 22);
            this.txtYeniKalem.TabIndex = 3;
            this.txtYeniKalem.Visible = false;
            // 
            // lblYeniKalemBilgi
            // 
            this.lblYeniKalemBilgi.AutoSize = true;
            this.lblYeniKalemBilgi.Location = new System.Drawing.Point(108, 283);
            this.lblYeniKalemBilgi.Name = "lblYeniKalemBilgi";
            this.lblYeniKalemBilgi.Size = new System.Drawing.Size(150, 16);
            this.lblYeniKalemBilgi.TabIndex = 4;
            this.lblYeniKalemBilgi.Text = "Lütfen yeni kalem giriniz.";
            this.lblYeniKalemBilgi.Visible = false;
            // 
            // txtYeniKalemBirim
            // 
            this.txtYeniKalemBirim.Location = new System.Drawing.Point(164, 344);
            this.txtYeniKalemBirim.Name = "txtYeniKalemBirim";
            this.txtYeniKalemBirim.Size = new System.Drawing.Size(176, 22);
            this.txtYeniKalemBirim.TabIndex = 5;
            this.txtYeniKalemBirim.Visible = false;
            // 
            // lblYeniKalemBirim
            // 
            this.lblYeniKalemBirim.AutoSize = true;
            this.lblYeniKalemBirim.Location = new System.Drawing.Point(23, 350);
            this.lblYeniKalemBirim.Name = "lblYeniKalemBirim";
            this.lblYeniKalemBirim.Size = new System.Drawing.Size(84, 16);
            this.lblYeniKalemBirim.TabIndex = 6;
            this.lblYeniKalemBirim.Text = "Kalem Birimi:";
            this.lblYeniKalemBirim.Visible = false;
            // 
            // lblYeniKalemAdi
            // 
            this.lblYeniKalemAdi.AutoSize = true;
            this.lblYeniKalemAdi.Location = new System.Drawing.Point(23, 317);
            this.lblYeniKalemAdi.Name = "lblYeniKalemAdi";
            this.lblYeniKalemAdi.Size = new System.Drawing.Size(71, 16);
            this.lblYeniKalemAdi.TabIndex = 7;
            this.lblYeniKalemAdi.Text = "Kalem Adı:";
            this.lblYeniKalemAdi.Visible = false;
            // 
            // frmYeniFiyatlandirmaKalemiEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(369, 428);
            this.Controls.Add(this.lblYeniKalemAdi);
            this.Controls.Add(this.lblYeniKalemBirim);
            this.Controls.Add(this.txtYeniKalemBirim);
            this.Controls.Add(this.lblYeniKalemBilgi);
            this.Controls.Add(this.txtYeniKalem);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.listKalemler);
            this.Name = "frmYeniFiyatlandirmaKalemiEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kalem Ekle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniFiyatlandirmaKalemiEkle_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listKalemler;
        private System.Windows.Forms.Button btnSec;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.TextBox txtYeniKalem;
        private System.Windows.Forms.Label lblYeniKalemBilgi;
        private System.Windows.Forms.TextBox txtYeniKalemBirim;
        private System.Windows.Forms.Label lblYeniKalemBirim;
        private System.Windows.Forms.Label lblYeniKalemAdi;
    }
}