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
            this.btnSil = new System.Windows.Forms.Button();
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
            this.txtProjeNo.BackColor = System.Drawing.Color.White;
            this.txtProjeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjeNo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtProjeNo.Location = new System.Drawing.Point(90, 27);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(163, 30);
            this.txtProjeNo.TabIndex = 2;
            this.txtProjeNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeNo_KeyDown);
            // 
            // lblProjeAdi
            // 
            this.lblProjeAdi.AutoSize = true;
            this.lblProjeAdi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProjeAdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblProjeAdi.Location = new System.Drawing.Point(25, 30);
            this.lblProjeAdi.Name = "lblProjeAdi";
            this.lblProjeAdi.Size = new System.Drawing.Size(56, 23);
            this.lblProjeAdi.TabIndex = 1;
            this.lblProjeAdi.Text = "Proje:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnKaydet.FlatAppearance.BorderSize = 0;
            this.btnKaydet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKaydet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKaydet.ForeColor = System.Drawing.Color.White;
            this.btnKaydet.Location = new System.Drawing.Point(1616, 41);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = false;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnProjeAra
            // 
            this.btnProjeAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnProjeAra.FlatAppearance.BorderSize = 0;
            this.btnProjeAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProjeAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnProjeAra.ForeColor = System.Drawing.Color.White;
            this.btnProjeAra.Location = new System.Drawing.Point(260, 27);
            this.btnProjeAra.Name = "btnProjeAra";
            this.btnProjeAra.Size = new System.Drawing.Size(149, 31);
            this.btnProjeAra.TabIndex = 4;
            this.btnProjeAra.Text = "Ara";
            this.btnProjeAra.UseVisualStyleBackColor = false;
            this.btnProjeAra.Click += new System.EventHandler(this.btnProjeAra_Click);
            // 
            // panelAlt
            // 
            this.panelAlt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
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
            this.btnYeniKalemEkle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnYeniKalemEkle.FlatAppearance.BorderSize = 0;
            this.btnYeniKalemEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYeniKalemEkle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnYeniKalemEkle.ForeColor = System.Drawing.Color.White;
            this.btnYeniKalemEkle.Location = new System.Drawing.Point(3, 41);
            this.btnYeniKalemEkle.Name = "btnYeniKalemEkle";
            this.btnYeniKalemEkle.Size = new System.Drawing.Size(146, 48);
            this.btnYeniKalemEkle.TabIndex = 4;
            this.btnYeniKalemEkle.Text = "+ Kalem Ekle";
            this.btnYeniKalemEkle.UseVisualStyleBackColor = false;
            this.btnYeniKalemEkle.Click += new System.EventHandler(this.btnYeniKalemEkle_Click);
            // 
            // panelUst
            // 
            this.panelUst.BackColor = System.Drawing.Color.White;
            this.panelUst.Controls.Add(this.btnSil);
            this.panelUst.Controls.Add(this.btnProjeAra);
            this.panelUst.Controls.Add(this.txtProjeNo);
            this.panelUst.Controls.Add(this.lblProjeAdi);
            this.panelUst.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUst.Location = new System.Drawing.Point(0, 50);
            this.panelUst.Name = "panelUst";
            this.panelUst.Size = new System.Drawing.Size(1765, 90);
            this.panelUst.TabIndex = 6;
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnSil.FlatAppearance.BorderSize = 0;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(415, 27);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(149, 31);
            this.btnSil.TabIndex = 5;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // panelFill
            // 
            this.panelFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelFill.Controls.Add(this.tableLayoutPanel2);
            this.panelFill.Controls.Add(this.tableLayoutPanel1);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 140);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(1765, 773);
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 704);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1765, 773);
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
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
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
        public System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Panel panelAlt;
        private System.Windows.Forms.Panel panelUst;
        private System.Windows.Forms.Panel panelFill;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnYeniKalemEkle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnProjeAra;
    }
}