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
            this.lblYeniKalemAdi = new System.Windows.Forms.Label();
            this.panelYeniKalem = new System.Windows.Forms.Panel();
            this.panelYeniKalem.SuspendLayout();
            this.SuspendLayout();
            // 
            // listKalemler
            // 
            this.listKalemler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listKalemler.FormattingEnabled = true;
            this.listKalemler.ItemHeight = 16;
            this.listKalemler.Location = new System.Drawing.Point(20, 20);
            this.listKalemler.Name = "listKalemler";
            this.listKalemler.Size = new System.Drawing.Size(345, 196);
            this.listKalemler.TabIndex = 0;
            // 
            // btnSec
            // 
            this.btnSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSec.FlatAppearance.BorderSize = 0;
            this.btnSec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSec.ForeColor = System.Drawing.Color.White;
            this.btnSec.Location = new System.Drawing.Point(198, 232);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(167, 45);
            this.btnSec.TabIndex = 1;
            this.btnSec.Text = "Seç";
            this.btnSec.UseVisualStyleBackColor = false;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEkle.BackColor = System.Drawing.Color.Gray;
            this.btnEkle.FlatAppearance.BorderSize = 0;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.ForeColor = System.Drawing.Color.White;
            this.btnEkle.Location = new System.Drawing.Point(20, 232);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(167, 45);
            this.btnEkle.TabIndex = 2;
            this.btnEkle.Text = "Yeni Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // txtYeniKalem
            // 
            this.txtYeniKalem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtYeniKalem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtYeniKalem.Location = new System.Drawing.Point(123, 44);
            this.txtYeniKalem.Name = "txtYeniKalem";
            this.txtYeniKalem.Size = new System.Drawing.Size(222, 22);
            this.txtYeniKalem.TabIndex = 3;
            // 
            // lblYeniKalemBilgi
            // 
            this.lblYeniKalemBilgi.AutoSize = true;
            this.lblYeniKalemBilgi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblYeniKalemBilgi.ForeColor = System.Drawing.Color.DimGray;
            this.lblYeniKalemBilgi.Location = new System.Drawing.Point(135, 12);
            this.lblYeniKalemBilgi.Name = "lblYeniKalemBilgi";
            this.lblYeniKalemBilgi.Size = new System.Drawing.Size(126, 16);
            this.lblYeniKalemBilgi.TabIndex = 4;
            this.lblYeniKalemBilgi.Text = "Yeni kalem adını girin";
            // 
            // lblYeniKalemAdi
            // 
            this.lblYeniKalemAdi.AutoSize = true;
            this.lblYeniKalemAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblYeniKalemAdi.Location = new System.Drawing.Point(13, 47);
            this.lblYeniKalemAdi.Name = "lblYeniKalemAdi";
            this.lblYeniKalemAdi.Size = new System.Drawing.Size(74, 16);
            this.lblYeniKalemAdi.TabIndex = 7;
            this.lblYeniKalemAdi.Text = "Kalem Adı:";
            // 
            // panelYeniKalem
            // 
            this.panelYeniKalem.Controls.Add(this.lblYeniKalemBilgi);
            this.panelYeniKalem.Controls.Add(this.lblYeniKalemAdi);
            this.panelYeniKalem.Controls.Add(this.txtYeniKalem);
            this.panelYeniKalem.Location = new System.Drawing.Point(10, 285);
            this.panelYeniKalem.Name = "panelYeniKalem";
            this.panelYeniKalem.Size = new System.Drawing.Size(360, 80);
            this.panelYeniKalem.TabIndex = 8;
            this.panelYeniKalem.Visible = false;
            // 
            // frmYeniFiyatlandirmaKalemiEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(385, 300); // Formun başlangıç boyutu artırıldı
            this.Controls.Add(this.panelYeniKalem);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.listKalemler);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmYeniFiyatlandirmaKalemiEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fiyatlandırma Kalemi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniFiyatlandirmaKalemiEkle_FormClosing);
            this.panelYeniKalem.ResumeLayout(false);
            this.panelYeniKalem.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listKalemler;
        private System.Windows.Forms.Button btnSec;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.TextBox txtYeniKalem;
        private System.Windows.Forms.Label lblYeniKalemBilgi;
        private System.Windows.Forms.Label lblYeniKalemAdi;
        private System.Windows.Forms.Panel panelYeniKalem;
    }
}