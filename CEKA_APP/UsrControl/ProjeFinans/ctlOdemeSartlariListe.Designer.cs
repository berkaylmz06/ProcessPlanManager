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
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.dataGridOdemeSartlari = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOdemeSartlari)).BeginInit();
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
            this.dataGridOdemeSartlari.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOdemeSartlari.Location = new System.Drawing.Point(0, 50);
            this.dataGridOdemeSartlari.Name = "dataGridOdemeSartlari";
            this.dataGridOdemeSartlari.RowHeadersWidth = 51;
            this.dataGridOdemeSartlari.RowTemplate.Height = 24;
            this.dataGridOdemeSartlari.Size = new System.Drawing.Size(1535, 1001);
            this.dataGridOdemeSartlari.TabIndex = 8;
            this.dataGridOdemeSartlari.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridOdemeSartlari_CellClick);
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
            this.ResumeLayout(false);

        }

        #endregion

        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.DataGridView dataGridOdemeSartlari;
    }
}
