namespace KesimTakip
{
    partial class frmKesimYap
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridKesimListesi = new System.Windows.Forms.DataGridView();
            this.dataGridDetay = new System.Windows.Forms.DataGridView();
            this.btnAra = new System.Windows.Forms.Button();
            this.btnPaketKes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKesilecekPlanSayisi = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridKesimListesi
            // 
            this.dataGridKesimListesi.AllowUserToAddRows = false;
            this.dataGridKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKesimListesi.Location = new System.Drawing.Point(33, 21);
            this.dataGridKesimListesi.MultiSelect = false;
            this.dataGridKesimListesi.Name = "dataGridKesimListesi";
            this.dataGridKesimListesi.ReadOnly = true;
            this.dataGridKesimListesi.RowHeadersVisible = false;
            this.dataGridKesimListesi.RowHeadersWidth = 62;
            this.dataGridKesimListesi.RowTemplate.Height = 28;
            this.dataGridKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKesimListesi.Size = new System.Drawing.Size(1296, 364);
            this.dataGridKesimListesi.TabIndex = 117;
            this.dataGridKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKesimListesi_CellClick);
            // 
            // dataGridDetay
            // 
            this.dataGridDetay.AllowUserToAddRows = false;
            this.dataGridDetay.AllowUserToDeleteRows = false;
            this.dataGridDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridDetay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDetay.Location = new System.Drawing.Point(33, 405);
            this.dataGridDetay.MultiSelect = false;
            this.dataGridDetay.Name = "dataGridDetay";
            this.dataGridDetay.ReadOnly = true;
            this.dataGridDetay.RowHeadersVisible = false;
            this.dataGridDetay.RowHeadersWidth = 62;
            this.dataGridDetay.RowTemplate.Height = 28;
            this.dataGridDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetay.Size = new System.Drawing.Size(1678, 364);
            this.dataGridDetay.TabIndex = 118;
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(1534, 21);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(177, 45);
            this.btnAra.TabIndex = 119;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // btnPaketKes
            // 
            this.btnPaketKes.Location = new System.Drawing.Point(1534, 208);
            this.btnPaketKes.Name = "btnPaketKes";
            this.btnPaketKes.Size = new System.Drawing.Size(177, 45);
            this.btnPaketKes.TabIndex = 120;
            this.btnPaketKes.Text = "Seçili Paketi Kes";
            this.btnPaketKes.UseVisualStyleBackColor = true;
            this.btnPaketKes.Click += new System.EventHandler(this.btnPaketKes_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1358, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 121;
            this.label1.Text = "Arama Yap:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1358, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 122;
            this.label2.Text = "Kesim Yap:";
            // 
            // txtKesilecekPlanSayisi
            // 
            this.txtKesilecekPlanSayisi.Location = new System.Drawing.Point(1534, 159);
            this.txtKesilecekPlanSayisi.Name = "txtKesilecekPlanSayisi";
            this.txtKesilecekPlanSayisi.Size = new System.Drawing.Size(177, 26);
            this.txtKesilecekPlanSayisi.TabIndex = 123;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1358, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 20);
            this.label3.TabIndex = 124;
            this.label3.Text = "Kesilecek Plan Sayısı:";
            // 
            // frmKesimYap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1760, 803);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtKesilecekPlanSayisi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPaketKes);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.dataGridDetay);
            this.Controls.Add(this.dataGridKesimListesi);
            this.Name = "frmKesimYap";
            this.Text = "frmKesimYap";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridDetay;
        private System.Windows.Forms.Button btnAra;
        public System.Windows.Forms.DataGridView dataGridKesimListesi;
        private System.Windows.Forms.Button btnPaketKes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKesilecekPlanSayisi;
        private System.Windows.Forms.Label label3;
    }
}