namespace KesimTakip.UsrControl
{
    partial class ctlKesimYap
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtKesilecekPlanSayisi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPaketKes = new System.Windows.Forms.Button();
            this.btnAra = new System.Windows.Forms.Button();
            this.dataGridDetay = new System.Windows.Forms.DataGridView();
            this.dataGridKesimListesi = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelBaslik = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).BeginInit();
            this.panelBaslik.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1422, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 16);
            this.label3.TabIndex = 132;
            this.label3.Text = "Kesilecek Plan Sayısı:";
            // 
            // txtKesilecekPlanSayisi
            // 
            this.txtKesilecekPlanSayisi.Location = new System.Drawing.Point(1579, 273);
            this.txtKesilecekPlanSayisi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKesilecekPlanSayisi.Name = "txtKesilecekPlanSayisi";
            this.txtKesilecekPlanSayisi.Size = new System.Drawing.Size(158, 22);
            this.txtKesilecekPlanSayisi.TabIndex = 131;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1422, 322);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 130;
            this.label2.Text = "Kesim Yap:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1422, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 16);
            this.label1.TabIndex = 129;
            this.label1.Text = "Arama Yap:";
            // 
            // btnPaketKes
            // 
            this.btnPaketKes.Location = new System.Drawing.Point(1579, 312);
            this.btnPaketKes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPaketKes.Name = "btnPaketKes";
            this.btnPaketKes.Size = new System.Drawing.Size(157, 36);
            this.btnPaketKes.TabIndex = 128;
            this.btnPaketKes.Text = "Seçili Paketi Kes";
            this.btnPaketKes.UseVisualStyleBackColor = true;
            this.btnPaketKes.Click += new System.EventHandler(this.btnPaketKes_Click);
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(1579, 163);
            this.btnAra.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(157, 36);
            this.btnAra.TabIndex = 127;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // dataGridDetay
            // 
            this.dataGridDetay.AllowUserToAddRows = false;
            this.dataGridDetay.AllowUserToDeleteRows = false;
            this.dataGridDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridDetay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDetay.Location = new System.Drawing.Point(26, 463);
            this.dataGridDetay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridDetay.MultiSelect = false;
            this.dataGridDetay.Name = "dataGridDetay";
            this.dataGridDetay.ReadOnly = true;
            this.dataGridDetay.RowHeadersVisible = false;
            this.dataGridDetay.RowHeadersWidth = 62;
            this.dataGridDetay.RowTemplate.Height = 28;
            this.dataGridDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetay.Size = new System.Drawing.Size(1775, 300);
            this.dataGridDetay.TabIndex = 126;
            // 
            // dataGridKesimListesi
            // 
            this.dataGridKesimListesi.AllowUserToAddRows = false;
            this.dataGridKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKesimListesi.Location = new System.Drawing.Point(26, 111);
            this.dataGridKesimListesi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridKesimListesi.MultiSelect = false;
            this.dataGridKesimListesi.Name = "dataGridKesimListesi";
            this.dataGridKesimListesi.ReadOnly = true;
            this.dataGridKesimListesi.RowHeadersVisible = false;
            this.dataGridKesimListesi.RowHeadersWidth = 62;
            this.dataGridKesimListesi.RowTemplate.Height = 28;
            this.dataGridKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKesimListesi.Size = new System.Drawing.Size(1329, 291);
            this.dataGridKesimListesi.TabIndex = 125;
            this.dataGridKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKesimListesi_CellClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(31, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 25);
            this.label4.TabIndex = 134;
            this.label4.Text = "Kesim Paket";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(31, 432);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(211, 25);
            this.label5.TabIndex = 135;
            this.label5.Text = "Kesim Paket Detay";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(3, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 29);
            this.label6.TabIndex = 136;
            this.label6.Text = "Kesim Yap";
            // 
            // panelBaslik
            // 
            this.panelBaslik.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelBaslik.Controls.Add(this.label6);
            this.panelBaslik.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBaslik.Location = new System.Drawing.Point(0, 0);
            this.panelBaslik.Name = "panelBaslik";
            this.panelBaslik.Size = new System.Drawing.Size(1820, 55);
            this.panelBaslik.TabIndex = 137;
            // 
            // ctlKesimYap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBaslik);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtKesilecekPlanSayisi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPaketKes);
            this.Controls.Add(this.btnAra);
            this.Controls.Add(this.dataGridDetay);
            this.Controls.Add(this.dataGridKesimListesi);
            this.Name = "ctlKesimYap";
            this.Size = new System.Drawing.Size(1820, 784);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).EndInit();
            this.panelBaslik.ResumeLayout(false);
            this.panelBaslik.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKesilecekPlanSayisi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPaketKes;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.DataGridView dataGridDetay;
        public System.Windows.Forms.DataGridView dataGridKesimListesi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelBaslik;
    }
}
