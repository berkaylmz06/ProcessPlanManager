namespace CEKA_APP.UsrControl.ProjeFinans
{
    partial class ctlSevkiyat
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelUst = new System.Windows.Forms.Panel();
            this.txtProjeAra = new System.Windows.Forms.TextBox();
            this.btnAra = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.btnPaketEkle = new System.Windows.Forms.Button();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.btnSevkiyatEkle = new System.Windows.Forms.Button();
            this.panelUst.SuspendLayout();
            this.panelAlt.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.28358F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.71642F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 145);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1569, 720);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // panelUst
            // 
            this.panelUst.Controls.Add(this.txtProjeAra);
            this.panelUst.Controls.Add(this.btnAra);
            this.panelUst.Controls.Add(this.label2);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 50);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1569, 95);
            this.panelUst.TabIndex = 10;
            // 
            // txtProjeAra
            // 
            this.txtProjeAra.Location = new System.Drawing.Point(127, 26);
            this.txtProjeAra.Name = "txtProjeAra";
            this.txtProjeAra.Size = new System.Drawing.Size(163, 22);
            this.txtProjeAra.TabIndex = 2;
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(296, 22);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(149, 31);
            this.btnAra.TabIndex = 4;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnProjeAra_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Proje Ara";
            // 
            // panelAlt
            // 
            this.panelAlt.Controls.Add(this.panelRight);
            this.panelAlt.Controls.Add(this.panelLeft);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 865);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1569, 62);
            this.panelAlt.TabIndex = 9;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.btnKaydet);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1186, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(383, 62);
            this.panelRight.TabIndex = 9;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(234, 6);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.btnSevkiyatEkle);
            this.panelLeft.Controls.Add(this.btnPaketEkle);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(305, 62);
            this.panelLeft.TabIndex = 8;
            // 
            // btnPaketEkle
            // 
            this.btnPaketEkle.Location = new System.Drawing.Point(156, 11);
            this.btnPaketEkle.Name = "btnPaketEkle";
            this.btnPaketEkle.Size = new System.Drawing.Size(146, 48);
            this.btnPaketEkle.TabIndex = 4;
            this.btnPaketEkle.Text = "Paket Ekle";
            this.btnPaketEkle.UseVisualStyleBackColor = true;
            this.btnPaketEkle.Click += new System.EventHandler(this.btnPaketEkle_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1569, 50);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // btnSevkiyatEkle
            // 
            this.btnSevkiyatEkle.Location = new System.Drawing.Point(4, 11);
            this.btnSevkiyatEkle.Name = "btnSevkiyatEkle";
            this.btnSevkiyatEkle.Size = new System.Drawing.Size(146, 48);
            this.btnSevkiyatEkle.TabIndex = 5;
            this.btnSevkiyatEkle.Text = "Sevkiyat Ekle";
            this.btnSevkiyatEkle.UseVisualStyleBackColor = true;
            this.btnSevkiyatEkle.Click += new System.EventHandler(this.btnSevkiyatEkle_Click);
            // 
            // ctlSevkiyat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelAlt);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlSevkiyat";
            this.Size = new System.Drawing.Size(1569, 927);
            this.Load += new System.EventHandler(this.ctlSevkiyat_Load);
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelAlt.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.TextBox txtProjeAra;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Button btnPaketEkle;
        private System.Windows.Forms.Button btnSevkiyatEkle;
    }
}
