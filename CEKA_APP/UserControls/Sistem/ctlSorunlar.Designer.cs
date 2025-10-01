namespace CEKA_APP.UsrControl
{
    partial class ctlSorunlar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBildiriZamani = new System.Windows.Forms.TextBox();
            this.txtBildiriYapanKullanici = new System.Windows.Forms.TextBox();
            this.txtSorunBildirimi = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridSorunBildirimleri = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSorunBildirimleri)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBildiriZamani
            // 
            this.txtBildiriZamani.Enabled = false;
            this.txtBildiriZamani.Location = new System.Drawing.Point(1060, 350);
            this.txtBildiriZamani.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBildiriZamani.Name = "txtBildiriZamani";
            this.txtBildiriZamani.Size = new System.Drawing.Size(394, 22);
            this.txtBildiriZamani.TabIndex = 130;
            // 
            // txtBildiriYapanKullanici
            // 
            this.txtBildiriYapanKullanici.Enabled = false;
            this.txtBildiriYapanKullanici.Location = new System.Drawing.Point(1060, 133);
            this.txtBildiriYapanKullanici.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBildiriYapanKullanici.Name = "txtBildiriYapanKullanici";
            this.txtBildiriYapanKullanici.Size = new System.Drawing.Size(394, 22);
            this.txtBildiriYapanKullanici.TabIndex = 129;
            // 
            // txtSorunBildirimi
            // 
            this.txtSorunBildirimi.Enabled = false;
            this.txtSorunBildirimi.Location = new System.Drawing.Point(1060, 184);
            this.txtSorunBildirimi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSorunBildirimi.Name = "txtSorunBildirimi";
            this.txtSorunBildirimi.Size = new System.Drawing.Size(394, 140);
            this.txtSorunBildirimi.TabIndex = 128;
            this.txtSorunBildirimi.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(899, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 127;
            this.label4.Text = "Bildiri Zamanı:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(903, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 126;
            this.label3.Text = "Sorun:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(903, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 16);
            this.label2.TabIndex = 125;
            this.label2.Text = "Bildiri Yapan Kullanıcı:";
            // 
            // dataGridSorunBildirimleri
            // 
            this.dataGridSorunBildirimleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridSorunBildirimleri.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridSorunBildirimleri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSorunBildirimleri.Location = new System.Drawing.Point(68, 103);
            this.dataGridSorunBildirimleri.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridSorunBildirimleri.Name = "dataGridSorunBildirimleri";
            this.dataGridSorunBildirimleri.RowHeadersVisible = false;
            this.dataGridSorunBildirimleri.RowHeadersWidth = 62;
            this.dataGridSorunBildirimleri.RowTemplate.Height = 28;
            this.dataGridSorunBildirimleri.Size = new System.Drawing.Size(752, 291);
            this.dataGridSorunBildirimleri.TabIndex = 124;
            this.dataGridSorunBildirimleri.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridSorunBildirimleri_CellClick);
            this.dataGridSorunBildirimleri.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridSorunBildirimleri_CellFormatting);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1588, 50);
            this.ctlBaslik1.TabIndex = 131;
            // 
            // ctlSorunlar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlBaslik1);
            this.Controls.Add(this.txtBildiriZamani);
            this.Controls.Add(this.txtBildiriYapanKullanici);
            this.Controls.Add(this.txtSorunBildirimi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridSorunBildirimleri);
            this.Name = "ctlSorunlar";
            this.Size = new System.Drawing.Size(1588, 900);
            this.Load += new System.EventHandler(this.ctlSorunlar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSorunBildirimleri)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBildiriZamani;
        private System.Windows.Forms.TextBox txtBildiriYapanKullanici;
        private System.Windows.Forms.RichTextBox txtSorunBildirimi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridSorunBildirimleri;
        private ctlBaslik ctlBaslik1;
    }
}
