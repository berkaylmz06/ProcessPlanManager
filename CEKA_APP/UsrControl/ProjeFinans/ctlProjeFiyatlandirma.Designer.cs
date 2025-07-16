namespace CEKA_APP.UsrControl
{
    partial class ctlProjeFiyatlandirma
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
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.lblProjeAdi = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnProjeAra = new System.Windows.Forms.Button();
            this.panelAlt = new System.Windows.Forms.Panel();
            this.btnYeniKalemEkle = new System.Windows.Forms.Button();
            this.panelUst = new System.Windows.Forms.Panel();
            this.panelFill = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelAlt.SuspendLayout();
            this.panelUst.SuspendLayout();
            this.panelFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(84, 29);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(203, 22);
            this.txtProjeNo.TabIndex = 2;
            // 
            // lblProjeAdi
            // 
            this.lblProjeAdi.AutoSize = true;
            this.lblProjeAdi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProjeAdi.Location = new System.Drawing.Point(25, 29);
            this.lblProjeAdi.Name = "lblProjeAdi";
            this.lblProjeAdi.Size = new System.Drawing.Size(53, 23);
            this.lblProjeAdi.TabIndex = 1;
            this.lblProjeAdi.Text = "Proje:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(1616, 41);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnProjeAra
            // 
            this.btnProjeAra.Location = new System.Drawing.Point(293, 24);
            this.btnProjeAra.Name = "btnProjeAra";
            this.btnProjeAra.Size = new System.Drawing.Size(88, 32);
            this.btnProjeAra.TabIndex = 4;
            this.btnProjeAra.Text = "Ara";
            this.btnProjeAra.UseVisualStyleBackColor = true;
            this.btnProjeAra.Click += new System.EventHandler(this.btnProjeAra_Click);
            // 
            // panelAlt
            // 
            this.panelAlt.Controls.Add(this.btnYeniKalemEkle);
            this.panelAlt.Controls.Add(this.btnKaydet);
            this.panelAlt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAlt.Location = new System.Drawing.Point(0, 913);
            this.panelAlt.Name = "panelAlt";
            this.panelAlt.Size = new System.Drawing.Size(1765, 92);
            this.panelAlt.TabIndex = 5;
            // 
            // btnYeniKalemEkle
            // 
            this.btnYeniKalemEkle.Location = new System.Drawing.Point(3, 41);
            this.btnYeniKalemEkle.Name = "btnYeniKalemEkle";
            this.btnYeniKalemEkle.Size = new System.Drawing.Size(146, 48);
            this.btnYeniKalemEkle.TabIndex = 4;
            this.btnYeniKalemEkle.Text = "+ Kalem Ekle";
            this.btnYeniKalemEkle.UseVisualStyleBackColor = true;
            this.btnYeniKalemEkle.Click += new System.EventHandler(this.btnYeniKalemEkle_Click);
            // 
            // panelUst
            // 
            this.panelUst.Controls.Add(this.btnProjeAra);
            this.panelUst.Controls.Add(this.txtProjeNo);
            this.panelUst.Controls.Add(this.lblProjeAdi);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 50);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1765, 76);
            this.panelUst.TabIndex = 6;
            // 
            // panelFill
            // 
            this.panelFill.Controls.Add(this.tableLayoutPanel2);
            this.panelFill.Controls.Add(this.tableLayoutPanel1);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 126);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(1765, 787);
            this.panelFill.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.06931F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.93069F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 221F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 239F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 237F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 192F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 718);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1765, 69);
            this.tableLayoutPanel2.TabIndex = 1;
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 194F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1765, 787);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1765, 50);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // ctlProjeFiyatlandirma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelFill);
            this.Controls.Add(this.panelUst);
            this.Controls.Add(this.panelAlt);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeFiyatlandirma";
            this.Size = new System.Drawing.Size(1765, 1005);
            this.Load += new System.EventHandler(this.ctlProjeFiyatlandirma_Load);
            this.panelAlt.ResumeLayout(false);
            this.panelUst.ResumeLayout(false);
            this.panelUst.PerformLayout();
            this.panelFill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Label lblProjeAdi;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnProjeAra;
        public System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.Panel panelFill;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnYeniKalemEkle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
