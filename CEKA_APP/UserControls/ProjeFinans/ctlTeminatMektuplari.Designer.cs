namespace CEKA_APP.UsrControl
{
    partial class ctlTeminatMektuplari
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
            this.dataGridTeminatMektuplari = new System.Windows.Forms.DataGridView();
            this.cmsTeminatMektubuIslemleri = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiMektupEkle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMektupGuncelle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMektupSil = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAra = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSutunSirala = new System.Windows.Forms.ToolStripMenuItem();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTeminatMektuplari)).BeginInit();
            this.cmsTeminatMektubuIslemleri.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridTeminatMektuplari
            // 
            this.dataGridTeminatMektuplari.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridTeminatMektuplari.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTeminatMektuplari.ContextMenuStrip = this.cmsTeminatMektubuIslemleri;
            this.dataGridTeminatMektuplari.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridTeminatMektuplari.Location = new System.Drawing.Point(0, 50);
            this.dataGridTeminatMektuplari.Name = "dataGridTeminatMektuplari";
            this.dataGridTeminatMektuplari.RowHeadersWidth = 51;
            this.dataGridTeminatMektuplari.RowTemplate.Height = 24;
            this.dataGridTeminatMektuplari.Size = new System.Drawing.Size(1665, 962);
            this.dataGridTeminatMektuplari.TabIndex = 6;
            this.dataGridTeminatMektuplari.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridTeminatMektuplari_CellMouseDown);
            this.dataGridTeminatMektuplari.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridTeminatMektuplari_ColumnWidthChanged);
            this.dataGridTeminatMektuplari.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridTeminatMektuplari_DataBindingComplete);
            // 
            // cmsTeminatMektubuIslemleri
            // 
            this.cmsTeminatMektubuIslemleri.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTeminatMektubuIslemleri.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiMektupEkle,
            this.tsmiMektupGuncelle,
            this.tsmiMektupSil,
            this.tsmiAra,
            this.tsmiSutunSirala});
            this.cmsTeminatMektubuIslemleri.Name = "cmsTeminatMektubuIslemleri";
            this.cmsTeminatMektubuIslemleri.Size = new System.Drawing.Size(255, 124);
            this.cmsTeminatMektubuIslemleri.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTeminatMektubuIslemleri_Opening);
            // 
            // tsmiMektupEkle
            // 
            this.tsmiMektupEkle.Name = "tsmiMektupEkle";
            this.tsmiMektupEkle.Size = new System.Drawing.Size(254, 24);
            this.tsmiMektupEkle.Text = "Teminat Mektubu Ekle";
            this.tsmiMektupEkle.Click += new System.EventHandler(this.tsmiMektupEkle_Click);
            // 
            // tsmiMektupGuncelle
            // 
            this.tsmiMektupGuncelle.Name = "tsmiMektupGuncelle";
            this.tsmiMektupGuncelle.Size = new System.Drawing.Size(254, 24);
            this.tsmiMektupGuncelle.Text = "Teminat Mektubu Güncelle";
            this.tsmiMektupGuncelle.Click += new System.EventHandler(this.tsmiMektupGuncelle_Click);
            // 
            // tsmiMektupSil
            // 
            this.tsmiMektupSil.Name = "tsmiMektupSil";
            this.tsmiMektupSil.Size = new System.Drawing.Size(254, 24);
            this.tsmiMektupSil.Text = "Teminat Mektubu Sil";
            this.tsmiMektupSil.Click += new System.EventHandler(this.tsmiMektupSil_Click);
            // 
            // tsmiAra
            // 
            this.tsmiAra.Name = "tsmiAra";
            this.tsmiAra.Size = new System.Drawing.Size(254, 24);
            this.tsmiAra.Text = "Ara";
            this.tsmiAra.Click += new System.EventHandler(this.tsmiAra_Click);
            // 
            // tsmiSutunSirala
            // 
            this.tsmiSutunSirala.Name = "tsmiSutunSirala";
            this.tsmiSutunSirala.Size = new System.Drawing.Size(254, 24);
            this.tsmiSutunSirala.Text = "Sütun Sıralama";
            this.tsmiSutunSirala.Click += new System.EventHandler(this.tsmiSutunSirala_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1665, 50);
            this.ctlBaslik1.TabIndex = 5;
            // 
            // ctlTeminatMektuplari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridTeminatMektuplari);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlTeminatMektuplari";
            this.Size = new System.Drawing.Size(1665, 1012);
            this.Load += new System.EventHandler(this.ctlTeminatMektuplari_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTeminatMektuplari)).EndInit();
            this.cmsTeminatMektubuIslemleri.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridTeminatMektuplari;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.ContextMenuStrip cmsTeminatMektubuIslemleri;
        private System.Windows.Forms.ToolStripMenuItem tsmiMektupEkle;
        private System.Windows.Forms.ToolStripMenuItem tsmiMektupGuncelle;
        private System.Windows.Forms.ToolStripMenuItem tsmiMektupSil;
        private System.Windows.Forms.ToolStripMenuItem tsmiAra;
        private System.Windows.Forms.ToolStripMenuItem tsmiSutunSirala;
    }
}
