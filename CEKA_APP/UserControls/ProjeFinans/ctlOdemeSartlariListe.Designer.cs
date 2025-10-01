namespace CEKA_APP.UsrControl.ProjeFinans
{
    partial class ctlOdemeSartlariListe
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
            this.dataGridOdemeSartlari = new System.Windows.Forms.DataGridView();
            this.cmsOdemeSartlari = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAra = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSutunSiralama = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOdemeSartlari)).BeginInit();
            this.cmsOdemeSartlari.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1535, 50);
            this.ctlBaslik1.TabIndex = 7;
            // 
            // dataGridOdemeSartlari
            // 
            this.dataGridOdemeSartlari.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridOdemeSartlari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOdemeSartlari.ContextMenuStrip = this.cmsOdemeSartlari;
            this.dataGridOdemeSartlari.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOdemeSartlari.Location = new System.Drawing.Point(0, 50);
            this.dataGridOdemeSartlari.Name = "dataGridOdemeSartlari";
            this.dataGridOdemeSartlari.RowHeadersWidth = 51;
            this.dataGridOdemeSartlari.RowTemplate.Height = 24;
            this.dataGridOdemeSartlari.Size = new System.Drawing.Size(1535, 1001);
            this.dataGridOdemeSartlari.TabIndex = 8;
            this.dataGridOdemeSartlari.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridOdemeSartlari_CellClick);
            this.dataGridOdemeSartlari.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridOdemeSartlari_ColumnWidthChanged);
            this.dataGridOdemeSartlari.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridOdemeSartlari_DataBindingComplete);
            // 
            // cmsOdemeSartlari
            // 
            this.cmsOdemeSartlari.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsOdemeSartlari.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAra,
            this.tsmiSutunSiralama});
            this.cmsOdemeSartlari.Name = "cmsOdemeSartlari";
            this.cmsOdemeSartlari.Size = new System.Drawing.Size(178, 52);
            // 
            // tsmiAra
            // 
            this.tsmiAra.Name = "tsmiAra";
            this.tsmiAra.Size = new System.Drawing.Size(177, 24);
            this.tsmiAra.Text = "Ara";
            this.tsmiAra.Click += new System.EventHandler(this.tsmiAra_Click);
            // 
            // tsmiSutunSiralama
            // 
            this.tsmiSutunSiralama.Name = "tsmiSutunSiralama";
            this.tsmiSutunSiralama.Size = new System.Drawing.Size(177, 24);
            this.tsmiSutunSiralama.Text = "Sütun Sıralama";
            this.tsmiSutunSiralama.Click += new System.EventHandler(this.tsmiSutunSiralama_Click);
            // 
            // ctlOdemeSartlariListe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridOdemeSartlari);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlOdemeSartlariListe";
            this.Size = new System.Drawing.Size(1535, 1051);
            this.Load += new System.EventHandler(this.ctlOdemeSartlariListe_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOdemeSartlari)).EndInit();
            this.cmsOdemeSartlari.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.DataGridView dataGridOdemeSartlari;
        private System.Windows.Forms.ContextMenuStrip cmsOdemeSartlari;
        private System.Windows.Forms.ToolStripMenuItem tsmiAra;
        private System.Windows.Forms.ToolStripMenuItem tsmiSutunSiralama;
    }
}
