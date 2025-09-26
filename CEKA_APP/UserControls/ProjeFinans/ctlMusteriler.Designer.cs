namespace CEKA_APP.UsrControl
{
    partial class ctlMusteriler
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
            this.components = new System.ComponentModel.Container();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.dataGridMusteriler = new System.Windows.Forms.DataGridView();
            this.cmsMusteriIslemleri = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAra = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExcelYukle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSutunSiralama = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMusteriler)).BeginInit();
            this.cmsMusteriIslemleri.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1399, 50);
            this.ctlBaslik1.TabIndex = 3;
            // 
            // dataGridMusteriler
            // 
            this.dataGridMusteriler.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridMusteriler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMusteriler.ContextMenuStrip = this.cmsMusteriIslemleri;
            this.dataGridMusteriler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMusteriler.Location = new System.Drawing.Point(0, 50);
            this.dataGridMusteriler.Name = "dataGridMusteriler";
            this.dataGridMusteriler.RowHeadersWidth = 51;
            this.dataGridMusteriler.RowTemplate.Height = 24;
            this.dataGridMusteriler.Size = new System.Drawing.Size(1399, 884);
            this.dataGridMusteriler.TabIndex = 4;
            this.dataGridMusteriler.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridMusteriler_CellMouseDown);
            this.dataGridMusteriler.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridMusteriler_DataBindingComplete);
            // 
            // cmsMusteriIslemleri
            // 
            this.cmsMusteriIslemleri.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsMusteriIslemleri.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExcelYukle,
            this.tsmiAra,
            this.tsmiSutunSiralama});
            this.cmsMusteriIslemleri.Name = "contextMenuStrip1";
            this.cmsMusteriIslemleri.Size = new System.Drawing.Size(233, 104);
            // 
            // tsmiAra
            // 
            this.tsmiAra.Name = "tsmiAra";
            this.tsmiAra.Size = new System.Drawing.Size(232, 24);
            this.tsmiAra.Text = "Ara";
            this.tsmiAra.Click += new System.EventHandler(this.tsmiAra_Click);
            // 
            // tsmiExcelYukle
            // 
            this.tsmiExcelYukle.Name = "tsmiExcelYukle";
            this.tsmiExcelYukle.Size = new System.Drawing.Size(232, 24);
            this.tsmiExcelYukle.Text = "Excel\'den Müşteri Yükle";
            this.tsmiExcelYukle.Click += new System.EventHandler(this.tsmiExcelYukle_Click);
            // 
            // tsmiSutunSiralama
            // 
            this.tsmiSutunSiralama.Name = "tsmiSutunSiralama";
            this.tsmiSutunSiralama.Size = new System.Drawing.Size(232, 24);
            this.tsmiSutunSiralama.Text = "Sütun Sıralama";
            this.tsmiSutunSiralama.Click += new System.EventHandler(this.tsmiSutunSiralama_Click);
            // 
            // ctlMusteriler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridMusteriler);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlMusteriler";
            this.Size = new System.Drawing.Size(1399, 934);
            this.Load += new System.EventHandler(this.ctlMusteriler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMusteriler)).EndInit();
            this.cmsMusteriIslemleri.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.DataGridView dataGridMusteriler;
        private System.Windows.Forms.ContextMenuStrip cmsMusteriIslemleri;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelYukle;
        private System.Windows.Forms.ToolStripMenuItem tsmiAra;
        private System.Windows.Forms.ToolStripMenuItem tsmiSutunSiralama;
    }
}
