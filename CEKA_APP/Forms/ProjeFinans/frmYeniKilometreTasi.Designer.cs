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
            this.listKilometreTaslari = new System.Windows.Forms.ListBox();
            this.btnSec = new System.Windows.Forms.Button();
            this.btnEkle = new System.Windows.Forms.Button();
            this.lblOran = new System.Windows.Forms.Label();
            this.cmbOran = new System.Windows.Forms.ComboBox();
            this.panelYeniKilometreTasi = new System.Windows.Forms.Panel();
            this.txtKilometreTasi = new System.Windows.Forms.TextBox();
            this.lblYeniKilometreTasiBilgi = new System.Windows.Forms.Label();
            this.lblYeniKilometreTasi = new System.Windows.Forms.Label();
            this.chkTutarIleGir = new System.Windows.Forms.CheckBox();
            this.txtTutar = new System.Windows.Forms.TextBox();
            this.lblTutar = new System.Windows.Forms.Label();
            this.panelYeniKilometreTasi.SuspendLayout();
            this.SuspendLayout();
            // 
            // listKilometreTaslari
            // 
            this.listKilometreTaslari.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listKilometreTaslari.FormattingEnabled = true;
            this.listKilometreTaslari.ItemHeight = 16;
            this.listKilometreTaslari.Location = new System.Drawing.Point(20, 20);
            this.listKilometreTaslari.Name = "listKilometreTaslari";
            this.listKilometreTaslari.Size = new System.Drawing.Size(345, 212);
            this.listKilometreTaslari.TabIndex = 8;
            // 
            // btnSec
            // 
            this.btnSec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSec.FlatAppearance.BorderSize = 0;
            this.btnSec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSec.ForeColor = System.Drawing.Color.White;
            this.btnSec.Location = new System.Drawing.Point(198, 305);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(167, 45);
            this.btnSec.TabIndex = 9;
            this.btnSec.Text = "Seç";
            this.btnSec.UseVisualStyleBackColor = false;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.Gray;
            this.btnEkle.FlatAppearance.BorderSize = 0;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.ForeColor = System.Drawing.Color.White;
            this.btnEkle.Location = new System.Drawing.Point(20, 305);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(167, 45);
            this.btnEkle.TabIndex = 10;
            this.btnEkle.Text = "Yeni Ekle";
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // lblOran
            // 
            this.lblOran.AutoSize = true;
            this.lblOran.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOran.Location = new System.Drawing.Point(17, 273);
            this.lblOran.Name = "lblOran";
            this.lblOran.Size = new System.Drawing.Size(51, 20);
            this.lblOran.TabIndex = 15;
            this.lblOran.Text = "Oran:";
            // 
            // cmbOran
            // 
            this.cmbOran.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOran.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOran.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cmbOran.FormattingEnabled = true;
            this.cmbOran.Location = new System.Drawing.Point(198, 270);
            this.cmbOran.Name = "cmbOran";
            this.cmbOran.Size = new System.Drawing.Size(167, 28);
            this.cmbOran.TabIndex = 16;
            // 
            // panelYeniKilometreTasi
            // 
            this.panelYeniKilometreTasi.Controls.Add(this.txtKilometreTasi);
            this.panelYeniKilometreTasi.Controls.Add(this.lblYeniKilometreTasiBilgi);
            this.panelYeniKilometreTasi.Controls.Add(this.lblYeniKilometreTasi);
            this.panelYeniKilometreTasi.Location = new System.Drawing.Point(10, 356);
            this.panelYeniKilometreTasi.Name = "panelYeniKilometreTasi";
            this.panelYeniKilometreTasi.Size = new System.Drawing.Size(365, 80);
            this.panelYeniKilometreTasi.TabIndex = 17;
            this.panelYeniKilometreTasi.Visible = false;
            // 
            // txtKilometreTasi
            // 
            this.txtKilometreTasi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKilometreTasi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtKilometreTasi.Location = new System.Drawing.Point(188, 44);
            this.txtKilometreTasi.Name = "txtKilometreTasi";
            this.txtKilometreTasi.Size = new System.Drawing.Size(167, 26);
            this.txtKilometreTasi.TabIndex = 11;
            // 
            // lblYeniKilometreTasiBilgi
            // 
            this.lblYeniKilometreTasiBilgi.AutoSize = true;
            this.lblYeniKilometreTasiBilgi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblYeniKilometreTasiBilgi.ForeColor = System.Drawing.Color.DimGray;
            this.lblYeniKilometreTasiBilgi.Location = new System.Drawing.Point(85, 12);
            this.lblYeniKilometreTasiBilgi.Name = "lblYeniKilometreTasiBilgi";
            this.lblYeniKilometreTasiBilgi.Size = new System.Drawing.Size(250, 20);
            this.lblYeniKilometreTasiBilgi.TabIndex = 12;
            this.lblYeniKilometreTasiBilgi.Text = "Lütfen yeni kilometre taşı giriniz.";
            // 
            // lblYeniKilometreTasi
            // 
            this.lblYeniKilometreTasi.AutoSize = true;
            this.lblYeniKilometreTasi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblYeniKilometreTasi.Location = new System.Drawing.Point(10, 47);
            this.lblYeniKilometreTasi.Name = "lblYeniKilometreTasi";
            this.lblYeniKilometreTasi.Size = new System.Drawing.Size(151, 20);
            this.lblYeniKilometreTasi.TabIndex = 13;
            this.lblYeniKilometreTasi.Text = "Kilometre Taşı Adı:";
            // 
            // chkTutarIleGir
            // 
            this.chkTutarIleGir.AutoSize = true;
            this.chkTutarIleGir.Location = new System.Drawing.Point(20, 241);
            this.chkTutarIleGir.Name = "chkTutarIleGir";
            this.chkTutarIleGir.Size = new System.Drawing.Size(170, 25);
            this.chkTutarIleGir.TabIndex = 18;
            this.chkTutarIleGir.Text = "Tutar bilgisi ile gir.";
            this.chkTutarIleGir.UseVisualStyleBackColor = true;
            this.chkTutarIleGir.CheckedChanged += new System.EventHandler(this.chkTutarIleGir_CheckedChanged);
            // 
            // txtTutar
            // 
            this.txtTutar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTutar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtTutar.Location = new System.Drawing.Point(198, 270);
            this.txtTutar.Name = "txtTutar";
            this.txtTutar.Size = new System.Drawing.Size(167, 26);
            this.txtTutar.TabIndex = 19;
            this.txtTutar.Visible = false;
            // 
            // lblTutar
            // 
            this.lblTutar.AutoSize = true;
            this.lblTutar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTutar.Location = new System.Drawing.Point(17, 273);
            this.lblTutar.Name = "lblTutar";
            this.lblTutar.Size = new System.Drawing.Size(53, 20);
            this.lblTutar.TabIndex = 14;
            this.lblTutar.Text = "Tutar:";
            this.lblTutar.Visible = false;
            // 
            // frmYeniKilometreTasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(391, 356);
            this.Controls.Add(this.lblTutar);
            this.Controls.Add(this.txtTutar);
            this.Controls.Add(this.chkTutarIleGir);
            this.Controls.Add(this.panelYeniKilometreTasi);
            this.Controls.Add(this.cmbOran);
            this.Controls.Add(this.lblOran);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.listKilometreTaslari);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "frmYeniKilometreTasi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kilometre Taşı Ekle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniKilometreTasi_FormClosing);
            this.panelYeniKilometreTasi.ResumeLayout(false);
            this.panelYeniKilometreTasi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listKilometreTaslari;
        private System.Windows.Forms.Button btnSec;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Label lblOran;
        private System.Windows.Forms.ComboBox cmbOran;
        private System.Windows.Forms.Panel panelYeniKilometreTasi;
        private System.Windows.Forms.TextBox txtKilometreTasi;
        private System.Windows.Forms.Label lblYeniKilometreTasiBilgi;
        private System.Windows.Forms.Label lblYeniKilometreTasi;
        private System.Windows.Forms.CheckBox chkTutarIleGir;
        private System.Windows.Forms.TextBox txtTutar;
        private System.Windows.Forms.Label lblTutar;
    }
}