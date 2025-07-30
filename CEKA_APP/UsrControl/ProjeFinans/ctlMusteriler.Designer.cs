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
            this.cmsMusteriIslemleri = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMusteriEkle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMusteriGuncelle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMusteriSil = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridMusteriler = new System.Windows.Forms.DataGridView();
            this.cmsMusteriIslemleri.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMusteriler)).BeginInit();
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
            // cmsMusteriIslemleri
            // 
            this.cmsMusteriIslemleri.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsMusteriIslemleri.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMusteriEkle,
            this.tsmiMusteriGuncelle,
            this.tsmiMusteriSil});
            this.cmsMusteriIslemleri.Name = "contextMenuStrip1";
            this.cmsMusteriIslemleri.Size = new System.Drawing.Size(189, 76);
            // 
            // tsmiMusteriEkle
            // 
            this.tsmiMusteriEkle.Name = "tsmiMusteriEkle";
            this.tsmiMusteriEkle.Size = new System.Drawing.Size(188, 24);
            this.tsmiMusteriEkle.Text = "Müşteri Ekle";
            // 
            // tsmiMusteriGuncelle
            // 
            this.tsmiMusteriGuncelle.Name = "tsmiMusteriGuncelle";
            this.tsmiMusteriGuncelle.Size = new System.Drawing.Size(188, 24);
            this.tsmiMusteriGuncelle.Text = "Müşteri Güncelle";
            // 
            // tsmiMusteriSil
            // 
            this.tsmiMusteriSil.Name = "tsmiMusteriSil";
            this.tsmiMusteriSil.Size = new System.Drawing.Size(188, 24);
            this.tsmiMusteriSil.Text = "Müşteri Sil";
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
            // ctlMusteriler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridMusteriler);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlMusteriler";
            this.Size = new System.Drawing.Size(1399, 934);
            this.Load += new System.EventHandler(this.ctlMusteriler_Load);
            this.cmsMusteriIslemleri.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMusteriler)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.ContextMenuStrip cmsMusteriIslemleri;
        private System.Windows.Forms.ToolStripMenuItem tsmiMusteriEkle;
        private System.Windows.Forms.ToolStripMenuItem tsmiMusteriGuncelle;
        private System.Windows.Forms.ToolStripMenuItem tsmiMusteriSil;
        private System.Windows.Forms.DataGridView dataGridMusteriler;
    }
}
