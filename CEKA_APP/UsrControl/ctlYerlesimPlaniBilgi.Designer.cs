namespace CEKA_APP.UsrControl
{
    partial class ctlYerlesimPlaniBilgi
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
            this.panelKesimListesi = new System.Windows.Forms.Panel();
            this.dataGridKesimListesi = new System.Windows.Forms.DataGridView();
            this.panelKesimDetay = new System.Windows.Forms.Panel();
            this.dataGridDetay = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.panelKesimListesi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).BeginInit();
            this.panelKesimDetay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).BeginInit();
            this.SuspendLayout();
            // 
            // panelKesimListesi
            // 
            this.panelKesimListesi.Controls.Add(this.dataGridKesimListesi);
            this.panelKesimListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKesimListesi.Location = new System.Drawing.Point(0, 50);
            this.panelKesimListesi.Name = "panelKesimListesi";
            this.panelKesimListesi.Size = new System.Drawing.Size(1503, 372);
            this.panelKesimListesi.TabIndex = 152;
            // 
            // dataGridKesimListesi
            // 
            this.dataGridKesimListesi.AllowUserToAddRows = false;
            this.dataGridKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKesimListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKesimListesi.Location = new System.Drawing.Point(0, 0);
            this.dataGridKesimListesi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridKesimListesi.MultiSelect = false;
            this.dataGridKesimListesi.Name = "dataGridKesimListesi";
            this.dataGridKesimListesi.ReadOnly = true;
            this.dataGridKesimListesi.RowHeadersVisible = false;
            this.dataGridKesimListesi.RowHeadersWidth = 62;
            this.dataGridKesimListesi.RowTemplate.Height = 28;
            this.dataGridKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKesimListesi.Size = new System.Drawing.Size(1503, 372);
            this.dataGridKesimListesi.TabIndex = 146;
            this.dataGridKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKesimListesi_CellClick);
            this.dataGridKesimListesi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKesimListesi_CellContentClick);
            // 
            // panelKesimDetay
            // 
            this.panelKesimDetay.Controls.Add(this.dataGridDetay);
            this.panelKesimDetay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelKesimDetay.Location = new System.Drawing.Point(0, 422);
            this.panelKesimDetay.Name = "panelKesimDetay";
            this.panelKesimDetay.Size = new System.Drawing.Size(1503, 372);
            this.panelKesimDetay.TabIndex = 151;
            // 
            // dataGridDetay
            // 
            this.dataGridDetay.AllowUserToAddRows = false;
            this.dataGridDetay.AllowUserToDeleteRows = false;
            this.dataGridDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridDetay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDetay.Location = new System.Drawing.Point(0, 0);
            this.dataGridDetay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridDetay.MultiSelect = false;
            this.dataGridDetay.Name = "dataGridDetay";
            this.dataGridDetay.ReadOnly = true;
            this.dataGridDetay.RowHeadersVisible = false;
            this.dataGridDetay.RowHeadersWidth = 62;
            this.dataGridDetay.RowTemplate.Height = 28;
            this.dataGridDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetay.Size = new System.Drawing.Size(1503, 372);
            this.dataGridDetay.TabIndex = 138;
            this.dataGridDetay.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDetay_CellContentClick);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1503, 50);
            this.ctlBaslik1.TabIndex = 153;
            // 
            // ctlYerlesimPlaniBilgi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelKesimListesi);
            this.Controls.Add(this.panelKesimDetay);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlYerlesimPlaniBilgi";
            this.Size = new System.Drawing.Size(1503, 794);
            this.Load += new System.EventHandler(this.ctlYerlesimPlaniBilgi_Load);
            this.panelKesimListesi.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).EndInit();
            this.panelKesimDetay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelKesimListesi;
        public System.Windows.Forms.DataGridView dataGridKesimListesi;
        private System.Windows.Forms.Panel panelKesimDetay;
        private System.Windows.Forms.DataGridView dataGridDetay;
        private ctlBaslik ctlBaslik1;
    }
}
