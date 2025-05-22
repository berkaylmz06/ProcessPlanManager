namespace KesimTakip.UsrControl
{
    partial class ctlSistemHareketleri
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
            this.dataGridKullaniciLog = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKullaniciLog)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridKullaniciLog
            // 
            this.dataGridKullaniciLog.AllowUserToAddRows = false;
            this.dataGridKullaniciLog.AllowUserToDeleteRows = false;
            this.dataGridKullaniciLog.AllowUserToResizeColumns = false;
            this.dataGridKullaniciLog.AllowUserToResizeRows = false;
            this.dataGridKullaniciLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridKullaniciLog.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridKullaniciLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKullaniciLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKullaniciLog.Location = new System.Drawing.Point(0, 0);
            this.dataGridKullaniciLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridKullaniciLog.MultiSelect = false;
            this.dataGridKullaniciLog.Name = "dataGridKullaniciLog";
            this.dataGridKullaniciLog.ReadOnly = true;
            this.dataGridKullaniciLog.RowHeadersVisible = false;
            this.dataGridKullaniciLog.RowHeadersWidth = 62;
            this.dataGridKullaniciLog.RowTemplate.Height = 28;
            this.dataGridKullaniciLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKullaniciLog.Size = new System.Drawing.Size(1346, 939);
            this.dataGridKullaniciLog.TabIndex = 126;
            this.dataGridKullaniciLog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridKullaniciLog_CellFormatting);
            this.dataGridKullaniciLog.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKullaniciLog_CellMouseEnter);
            // 
            // ctlSistemHareketleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridKullaniciLog);
            this.Name = "ctlSistemHareketleri";
            this.Size = new System.Drawing.Size(1346, 939);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKullaniciLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridKullaniciLog;
    }
}
