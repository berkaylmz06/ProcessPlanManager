namespace KesimTakip.UsrControl
{
    partial class ctlAutoCadAktarim
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
            this.txtAra = new System.Windows.Forms.TextBox();
            this.dataGridXmlCiktisi = new System.Windows.Forms.DataGridView();
            this.btnHazirla = new System.Windows.Forms.Button();
            this.dataGridIslenmisXml = new System.Windows.Forms.DataGridView();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnXmlDosyaSec = new System.Windows.Forms.Button();
            this.btnAktar = new System.Windows.Forms.Button();
            this.ctlBaslik1 = new KesimTakip.UsrControl.ctlBaslik();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridXmlCiktisi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIslenmisXml)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAra
            // 
            this.txtAra.Location = new System.Drawing.Point(725, 174);
            this.txtAra.Name = "txtAra";
            this.txtAra.Size = new System.Drawing.Size(199, 22);
            this.txtAra.TabIndex = 2;
            this.txtAra.TextChanged += new System.EventHandler(this.txtAra_TextChanged);
            // 
            // dataGridXmlCiktisi
            // 
            this.dataGridXmlCiktisi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridXmlCiktisi.Location = new System.Drawing.Point(26, 202);
            this.dataGridXmlCiktisi.Name = "dataGridXmlCiktisi";
            this.dataGridXmlCiktisi.ReadOnly = true;
            this.dataGridXmlCiktisi.RowHeadersWidth = 51;
            this.dataGridXmlCiktisi.RowTemplate.Height = 24;
            this.dataGridXmlCiktisi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridXmlCiktisi.Size = new System.Drawing.Size(898, 628);
            this.dataGridXmlCiktisi.TabIndex = 1;
            // 
            // btnHazirla
            // 
            this.btnHazirla.Location = new System.Drawing.Point(1649, 167);
            this.btnHazirla.Name = "btnHazirla";
            this.btnHazirla.Size = new System.Drawing.Size(94, 29);
            this.btnHazirla.TabIndex = 4;
            this.btnHazirla.Text = "Hazırla";
            this.btnHazirla.UseVisualStyleBackColor = true;
            this.btnHazirla.Click += new System.EventHandler(this.btnHazirla_Click);
            // 
            // dataGridIslenmisXml
            // 
            this.dataGridIslenmisXml.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridIslenmisXml.Location = new System.Drawing.Point(945, 202);
            this.dataGridIslenmisXml.Name = "dataGridIslenmisXml";
            this.dataGridIslenmisXml.ReadOnly = true;
            this.dataGridIslenmisXml.RowHeadersWidth = 51;
            this.dataGridIslenmisXml.RowTemplate.Height = 24;
            this.dataGridIslenmisXml.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridIslenmisXml.Size = new System.Drawing.Size(898, 628);
            this.dataGridIslenmisXml.TabIndex = 3;
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(35, 174);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(199, 22);
            this.txtProjeNo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Proje No";
            // 
            // btnXmlDosyaSec
            // 
            this.btnXmlDosyaSec.Location = new System.Drawing.Point(35, 87);
            this.btnXmlDosyaSec.Name = "btnXmlDosyaSec";
            this.btnXmlDosyaSec.Size = new System.Drawing.Size(187, 32);
            this.btnXmlDosyaSec.TabIndex = 0;
            this.btnXmlDosyaSec.Text = "Xml Dosya Seç";
            this.btnXmlDosyaSec.UseVisualStyleBackColor = true;
            this.btnXmlDosyaSec.Click += new System.EventHandler(this.btnXmlDosyaSec_Click);
            // 
            // btnAktar
            // 
            this.btnAktar.Location = new System.Drawing.Point(1749, 167);
            this.btnAktar.Name = "btnAktar";
            this.btnAktar.Size = new System.Drawing.Size(94, 29);
            this.btnAktar.TabIndex = 7;
            this.btnAktar.Text = "Aktar";
            this.btnAktar.UseVisualStyleBackColor = true;
            this.btnAktar.Click += new System.EventHandler(this.btnAktar_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1869, 50);
            this.ctlBaslik1.TabIndex = 8;
            // 
            // ctlAutoCadAktarim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlBaslik1);
            this.Controls.Add(this.btnAktar);
            this.Controls.Add(this.dataGridIslenmisXml);
            this.Controls.Add(this.dataGridXmlCiktisi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnXmlDosyaSec);
            this.Controls.Add(this.txtAra);
            this.Controls.Add(this.txtProjeNo);
            this.Controls.Add(this.btnHazirla);
            this.Name = "ctlAutoCadAktarim";
            this.Size = new System.Drawing.Size(1869, 1033);
            this.Load += new System.EventHandler(this.ctlAutoCadAktarim_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridXmlCiktisi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridIslenmisXml)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtAra;
        private System.Windows.Forms.DataGridView dataGridXmlCiktisi;
        private System.Windows.Forms.DataGridView dataGridIslenmisXml;
        private System.Windows.Forms.Button btnXmlDosyaSec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Button btnHazirla;
        private System.Windows.Forms.Button btnAktar;
        private ctlBaslik ctlBaslik1;
    }
}
