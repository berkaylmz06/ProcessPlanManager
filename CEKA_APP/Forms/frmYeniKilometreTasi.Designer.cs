namespace CEKA_APP.Forms
{
    partial class frmYeniKilometreTasi
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
            this.lblYeniKilometreTasi = new System.Windows.Forms.Label();
            this.lblYeniKilometreTasiBilgi = new System.Windows.Forms.Label();
            this.txtKilometreTasi = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.btnSec = new System.Windows.Forms.Button();
            this.listKilometreTaslari = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOran = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblYeniKilometreTasi
            // 
            this.lblYeniKilometreTasi.AutoSize = true;
            this.lblYeniKilometreTasi.Location = new System.Drawing.Point(19, 312);
            this.lblYeniKilometreTasi.Name = "lblYeniKilometreTasi";
            this.lblYeniKilometreTasi.Size = new System.Drawing.Size(119, 16);
            this.lblYeniKilometreTasi.TabIndex = 13;
            this.lblYeniKilometreTasi.Text = "Kilometre Taşı Adı:";
            this.lblYeniKilometreTasi.Visible = false;
            // 
            // lblYeniKilometreTasiBilgi
            // 
            this.lblYeniKilometreTasiBilgi.AutoSize = true;
            this.lblYeniKilometreTasiBilgi.Location = new System.Drawing.Point(95, 277);
            this.lblYeniKilometreTasiBilgi.Name = "lblYeniKilometreTasiBilgi";
            this.lblYeniKilometreTasiBilgi.Size = new System.Drawing.Size(192, 16);
            this.lblYeniKilometreTasiBilgi.TabIndex = 12;
            this.lblYeniKilometreTasiBilgi.Text = "Lütfen yeni kilometre taşı giriniz.";
            this.lblYeniKilometreTasiBilgi.Visible = false;
            // 
            // txtKilometreTasi
            // 
            this.txtKilometreTasi.Location = new System.Drawing.Point(191, 309);
            this.txtKilometreTasi.Name = "txtKilometreTasi";
            this.txtKilometreTasi.Size = new System.Drawing.Size(176, 22);
            this.txtKilometreTasi.TabIndex = 11;
            this.txtKilometreTasi.Visible = false;
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(216, 386);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(116, 38);
            this.btnEkle.TabIndex = 10;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // btnSec
            // 
            this.btnSec.Location = new System.Drawing.Point(42, 386);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(116, 38);
            this.btnSec.TabIndex = 9;
            this.btnSec.Text = "Seç";
            this.btnSec.UseVisualStyleBackColor = true;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // listKilometreTaslari
            // 
            this.listKilometreTaslari.FormattingEnabled = true;
            this.listKilometreTaslari.ItemHeight = 16;
            this.listKilometreTaslari.Location = new System.Drawing.Point(22, 22);
            this.listKilometreTaslari.Name = "listKilometreTaslari";
            this.listKilometreTaslari.Size = new System.Drawing.Size(345, 244);
            this.listKilometreTaslari.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Oran:";
            this.label1.Visible = false;
            // 
            // cmbOran
            // 
            this.cmbOran.FormattingEnabled = true;
            this.cmbOran.Location = new System.Drawing.Point(191, 344);
            this.cmbOran.Name = "cmbOran";
            this.cmbOran.Size = new System.Drawing.Size(176, 24);
            this.cmbOran.TabIndex = 16;
            // 
            // frmYeniKilometreTasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 436);
            this.Controls.Add(this.cmbOran);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblYeniKilometreTasi);
            this.Controls.Add(this.lblYeniKilometreTasiBilgi);
            this.Controls.Add(this.txtKilometreTasi);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.listKilometreTaslari);
            this.Name = "frmYeniKilometreTasi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kilometre Taşı Ekle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniKilometreTasi_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblYeniKilometreTasi;
        private System.Windows.Forms.Label lblYeniKilometreTasiBilgi;
        private System.Windows.Forms.TextBox txtKilometreTasi;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnSec;
        private System.Windows.Forms.ListBox listKilometreTaslari;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbOran;
    }
}