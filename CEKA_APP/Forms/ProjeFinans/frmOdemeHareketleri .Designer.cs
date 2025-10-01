namespace CEKA_APP.Forms
{
    partial class frmOdemeHareketleri
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridOdemeHareketleri;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dataGridOdemeHareketleri = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOdemeHareketleri)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridOdemeHareketleri
            // 
            this.dataGridOdemeHareketleri.AllowUserToAddRows = false;
            this.dataGridOdemeHareketleri.AllowUserToDeleteRows = false;
            this.dataGridOdemeHareketleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridOdemeHareketleri.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridOdemeHareketleri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOdemeHareketleri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOdemeHareketleri.Location = new System.Drawing.Point(0, 0);
            this.dataGridOdemeHareketleri.Name = "dataGridOdemeHareketleri";
            this.dataGridOdemeHareketleri.ReadOnly = true;
            this.dataGridOdemeHareketleri.RowHeadersWidth = 51;
            this.dataGridOdemeHareketleri.RowTemplate.Height = 24;
            this.dataGridOdemeHareketleri.Size = new System.Drawing.Size(800, 450);
            this.dataGridOdemeHareketleri.TabIndex = 0;
            // 
            // frmOdemeHareketleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridOdemeHareketleri);
            this.Name = "frmOdemeHareketleri";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ödeme Hareketleri";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOdemeHareketleri)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

    }
}