namespace KesimTakip.UsrControl
{
    partial class ctlYapilanKesimleriGor
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
            this.btnAra = new System.Windows.Forms.Button();
            this.dataGridViewTamamlanmisKesimListesi = new System.Windows.Forms.DataGridView();
            this.dataGridTamamlanmisDetay = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewTamamlanmisHareket = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new KesimTakip.UsrControl.ctlBaslik();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(557, 80);
            this.btnAra.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(157, 36);
            this.btnAra.TabIndex = 139;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // dataGridViewTamamlanmisKesimListesi
            // 
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewTamamlanmisKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewTamamlanmisKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTamamlanmisKesimListesi.Location = new System.Drawing.Point(30, 123);
            this.dataGridViewTamamlanmisKesimListesi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewTamamlanmisKesimListesi.MultiSelect = false;
            this.dataGridViewTamamlanmisKesimListesi.Name = "dataGridViewTamamlanmisKesimListesi";
            this.dataGridViewTamamlanmisKesimListesi.ReadOnly = true;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersWidth = 62;
            this.dataGridViewTamamlanmisKesimListesi.RowTemplate.Height = 28;
            this.dataGridViewTamamlanmisKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisKesimListesi.Size = new System.Drawing.Size(684, 294);
            this.dataGridViewTamamlanmisKesimListesi.TabIndex = 140;
            this.dataGridViewTamamlanmisKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTamamlanmisKesimListesi_CellClick);
            // 
            // dataGridTamamlanmisDetay
            // 
            this.dataGridTamamlanmisDetay.AllowUserToAddRows = false;
            this.dataGridTamamlanmisDetay.AllowUserToDeleteRows = false;
            this.dataGridTamamlanmisDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridTamamlanmisDetay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridTamamlanmisDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTamamlanmisDetay.Location = new System.Drawing.Point(30, 457);
            this.dataGridTamamlanmisDetay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridTamamlanmisDetay.MultiSelect = false;
            this.dataGridTamamlanmisDetay.Name = "dataGridTamamlanmisDetay";
            this.dataGridTamamlanmisDetay.ReadOnly = true;
            this.dataGridTamamlanmisDetay.RowHeadersVisible = false;
            this.dataGridTamamlanmisDetay.RowHeadersWidth = 62;
            this.dataGridTamamlanmisDetay.RowTemplate.Height = 28;
            this.dataGridTamamlanmisDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTamamlanmisDetay.Size = new System.Drawing.Size(1460, 316);
            this.dataGridTamamlanmisDetay.TabIndex = 141;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(37, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 20);
            this.label4.TabIndex = 142;
            this.label4.Text = "Tamamlanmış Kesimler Paket";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(37, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 20);
            this.label1.TabIndex = 143;
            this.label1.Text = "Tamamlanmış Kesim Detay";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(813, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(319, 20);
            this.label2.TabIndex = 145;
            this.label2.Text = "Tamamlanmış Kesimlerin Hareketleri";
            // 
            // dataGridViewTamamlanmisHareket
            // 
            this.dataGridViewTamamlanmisHareket.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisHareket.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisHareket.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewTamamlanmisHareket.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewTamamlanmisHareket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTamamlanmisHareket.Location = new System.Drawing.Point(806, 123);
            this.dataGridViewTamamlanmisHareket.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewTamamlanmisHareket.MultiSelect = false;
            this.dataGridViewTamamlanmisHareket.Name = "dataGridViewTamamlanmisHareket";
            this.dataGridViewTamamlanmisHareket.ReadOnly = true;
            this.dataGridViewTamamlanmisHareket.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisHareket.RowHeadersWidth = 62;
            this.dataGridViewTamamlanmisHareket.RowTemplate.Height = 28;
            this.dataGridViewTamamlanmisHareket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisHareket.Size = new System.Drawing.Size(684, 294);
            this.dataGridViewTamamlanmisHareket.TabIndex = 144;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1575, 50);
            this.ctlBaslik1.TabIndex = 146;
            // 
            // ctlYapilanKesimleriGor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlBaslik1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridViewTamamlanmisHareket);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridTamamlanmisDetay);
            this.Controls.Add(this.dataGridViewTamamlanmisKesimListesi);
            this.Controls.Add(this.btnAra);
            this.Name = "ctlYapilanKesimleriGor";
            this.Size = new System.Drawing.Size(1575, 940);
            this.Load += new System.EventHandler(this.ctlYapilanKesimleriGor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAra;
        public System.Windows.Forms.DataGridView dataGridViewTamamlanmisKesimListesi;
        private System.Windows.Forms.DataGridView dataGridTamamlanmisDetay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataGridView dataGridViewTamamlanmisHareket;
        private ctlBaslik ctlBaslik1;
    }
}
