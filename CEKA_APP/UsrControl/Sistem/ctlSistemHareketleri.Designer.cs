namespace CEKA_APP.UsrControl
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
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
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
            this.dataGridKullaniciLog.Location = new System.Drawing.Point(0, 50);
            this.dataGridKullaniciLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridKullaniciLog.MultiSelect = false;
            this.dataGridKullaniciLog.Name = "dataGridKullaniciLog";
            this.dataGridKullaniciLog.ReadOnly = true;
            this.dataGridKullaniciLog.RowHeadersVisible = false;
            this.dataGridKullaniciLog.RowHeadersWidth = 62;
            this.dataGridKullaniciLog.RowTemplate.Height = 28;
            this.dataGridKullaniciLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKullaniciLog.Size = new System.Drawing.Size(1349, 897);
            this.dataGridKullaniciLog.TabIndex = 126;
            this.dataGridKullaniciLog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridKullaniciLog_CellFormatting);
            this.dataGridKullaniciLog.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKullaniciLog_CellMouseEnter);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1349, 50);
            this.ctlBaslik1.TabIndex = 140;
            // 
            // ctlSistemHareketleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridKullaniciLog);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlSistemHareketleri";
            this.Size = new System.Drawing.Size(1349, 947);
            this.Load += new System.EventHandler(this.ctlSistemHareketleri_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKullaniciLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridKullaniciLog;
        private ctlBaslik ctlBaslik1;
    }
}
