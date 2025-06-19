namespace CEKA_APP
{
    partial class frmYeniIscilikEkle
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
            this.listIscilikler = new System.Windows.Forms.ListBox();
            this.btnSec = new System.Windows.Forms.Button();
            this.btnEkle = new System.Windows.Forms.Button();
            this.txtYeniIscilik = new System.Windows.Forms.TextBox();
            this.lblYeniIscilik = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listIscilikler
            // 
            this.listIscilikler.FormattingEnabled = true;
            this.listIscilikler.ItemHeight = 16;
            this.listIscilikler.Location = new System.Drawing.Point(12, 27);
            this.listIscilikler.Name = "listIscilikler";
            this.listIscilikler.Size = new System.Drawing.Size(345, 244);
            this.listIscilikler.TabIndex = 0;
            // 
            // btnSec
            // 
            this.btnSec.Location = new System.Drawing.Point(40, 351);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(116, 38);
            this.btnSec.TabIndex = 1;
            this.btnSec.Text = "Seç";
            this.btnSec.UseVisualStyleBackColor = true;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(199, 351);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(116, 38);
            this.btnEkle.TabIndex = 2;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // txtYeniIscilik
            // 
            this.txtYeniIscilik.Location = new System.Drawing.Point(12, 309);
            this.txtYeniIscilik.Name = "txtYeniIscilik";
            this.txtYeniIscilik.Size = new System.Drawing.Size(345, 22);
            this.txtYeniIscilik.TabIndex = 3;
            this.txtYeniIscilik.Visible = false;
            // 
            // lblYeniIscilik
            // 
            this.lblYeniIscilik.AutoSize = true;
            this.lblYeniIscilik.Location = new System.Drawing.Point(12, 290);
            this.lblYeniIscilik.Name = "lblYeniIscilik";
            this.lblYeniIscilik.Size = new System.Drawing.Size(146, 16);
            this.lblYeniIscilik.TabIndex = 4;
            this.lblYeniIscilik.Text = "Lütfen yeni işçilik giriniz.";
            this.lblYeniIscilik.Visible = false;
            // 
            // frmYeniIscilikEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(369, 413);
            this.Controls.Add(this.lblYeniIscilik);
            this.Controls.Add(this.txtYeniIscilik);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.listIscilikler);
            this.Name = "frmYeniIscilikEkle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İşçilik Ekle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmYeniIscilikEkle_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listIscilikler;
        private System.Windows.Forms.Button btnSec;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.TextBox txtYeniIscilik;
        private System.Windows.Forms.Label lblYeniIscilik;
    }
}