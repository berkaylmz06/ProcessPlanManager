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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.lblProjeAdi = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnProjeAra = new System.Windows.Forms.Button();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(33, 136);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(894, 505);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(111, 83);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(203, 22);
            this.txtProjeNo.TabIndex = 2;
            // 
            // lblProjeAdi
            // 
            this.lblProjeAdi.AutoSize = true;
            this.lblProjeAdi.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProjeAdi.Location = new System.Drawing.Point(52, 83);
            this.lblProjeAdi.Name = "lblProjeAdi";
            this.lblProjeAdi.Size = new System.Drawing.Size(53, 23);
            this.lblProjeAdi.TabIndex = 1;
            this.lblProjeAdi.Text = "Proje:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(44, 803);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(146, 48);
            this.btnKaydet.TabIndex = 3;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnProjeAra
            // 
            this.btnProjeAra.Location = new System.Drawing.Point(320, 78);
            this.btnProjeAra.Name = "btnProjeAra";
            this.btnProjeAra.Size = new System.Drawing.Size(88, 32);
            this.btnProjeAra.TabIndex = 4;
            this.btnProjeAra.Text = "Ara";
            this.btnProjeAra.UseVisualStyleBackColor = true;
            this.btnProjeAra.Click += new System.EventHandler(this.btnProjeAra_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1525, 50);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // ctlProjeFiyatlandirma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnProjeAra);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.lblProjeAdi);
            this.Controls.Add(this.txtProjeNo);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeFiyatlandirma";
            this.Size = new System.Drawing.Size(1525, 871);
            this.Load += new System.EventHandler(this.ctlProjeFiyatlandirma_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblProjeAdi;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnProjeAra;
        public System.Windows.Forms.TextBox txtProjeNo;
    }
}
