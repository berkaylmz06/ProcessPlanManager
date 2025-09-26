namespace CEKA_APP.UsrControl
{
    partial class ctlKesimYap
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.bottomLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridDetay = new System.Windows.Forms.DataGridView();
            this.topLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridKesimListesi = new System.Windows.Forms.DataGridView();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.lblKesilecekLot = new System.Windows.Forms.Label();
            this.txtKesilecekLot = new System.Windows.Forms.TextBox();
            this.lblAramaYap = new System.Windows.Forms.Label();
            this.btnAra = new System.Windows.Forms.Button();
            this.lblKesimYap = new System.Windows.Forms.Label();
            this.btnPaketKes = new System.Windows.Forms.Button();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.bottomLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).BeginInit();
            this.topLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).BeginInit();
            this.rightPanel.SuspendLayout();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomLayoutPanel
            // 
            this.bottomLayoutPanel.ColumnCount = 1;
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Controls.Add(this.label5, 0, 0);
            this.bottomLayoutPanel.Controls.Add(this.dataGridDetay, 0, 1);
            this.bottomLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLayoutPanel.Location = new System.Drawing.Point(3, 370);
            this.bottomLayoutPanel.Name = "bottomLayoutPanel";
            this.bottomLayoutPanel.RowCount = 2;
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Size = new System.Drawing.Size(1814, 361);
            this.bottomLayoutPanel.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1808, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "Kesim Detayları";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dataGridDetay
            // 
            this.dataGridDetay.AllowUserToAddRows = false;
            this.dataGridDetay.AllowUserToDeleteRows = false;
            this.dataGridDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridDetay.BackgroundColor = System.Drawing.Color.White;
            this.dataGridDetay.ColumnHeadersHeight = 29;
            this.dataGridDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDetay.Location = new System.Drawing.Point(3, 33);
            this.dataGridDetay.Name = "dataGridDetay";
            this.dataGridDetay.ReadOnly = true;
            this.dataGridDetay.RowHeadersVisible = false;
            this.dataGridDetay.RowHeadersWidth = 51;
            this.dataGridDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetay.Size = new System.Drawing.Size(1808, 325);
            this.dataGridDetay.TabIndex = 1;
            // 
            // topLayoutPanel
            // 
            this.topLayoutPanel.ColumnCount = 2;
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.27232F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.72767F));
            this.topLayoutPanel.Controls.Add(this.label4, 0, 0);
            this.topLayoutPanel.Controls.Add(this.dataGridKesimListesi, 0, 1);
            this.topLayoutPanel.Controls.Add(this.rightPanel, 1, 1);
            this.topLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.topLayoutPanel.Name = "topLayoutPanel";
            this.topLayoutPanel.RowCount = 2;
            this.topLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.topLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topLayoutPanel.Size = new System.Drawing.Size(1814, 361);
            this.topLayoutPanel.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1432, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Kesim Listesi";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dataGridKesimListesi
            // 
            this.dataGridKesimListesi.AllowUserToAddRows = false;
            this.dataGridKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridKesimListesi.BackgroundColor = System.Drawing.Color.White;
            this.dataGridKesimListesi.ColumnHeadersHeight = 29;
            this.dataGridKesimListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKesimListesi.Location = new System.Drawing.Point(3, 33);
            this.dataGridKesimListesi.Name = "dataGridKesimListesi";
            this.dataGridKesimListesi.ReadOnly = true;
            this.dataGridKesimListesi.RowHeadersVisible = false;
            this.dataGridKesimListesi.RowHeadersWidth = 51;
            this.dataGridKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKesimListesi.Size = new System.Drawing.Size(1432, 325);
            this.dataGridKesimListesi.TabIndex = 1;
            this.dataGridKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridKesimListesi_CellClick);
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.lblKesilecekLot);
            this.rightPanel.Controls.Add(this.txtKesilecekLot);
            this.rightPanel.Controls.Add(this.lblAramaYap);
            this.rightPanel.Controls.Add(this.btnAra);
            this.rightPanel.Controls.Add(this.lblKesimYap);
            this.rightPanel.Controls.Add(this.btnPaketKes);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(1441, 33);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(370, 325);
            this.rightPanel.TabIndex = 2;
            // 
            // lblKesilecekLot
            // 
            this.lblKesilecekLot.AutoSize = true;
            this.lblKesilecekLot.Location = new System.Drawing.Point(28, 131);
            this.lblKesilecekLot.Name = "lblKesilecekLot";
            this.lblKesilecekLot.Size = new System.Drawing.Size(98, 16);
            this.lblKesilecekLot.TabIndex = 6;
            this.lblKesilecekLot.Text = "Kesilecek LOT:";
            // 
            // txtKesilecekLot
            // 
            this.txtKesilecekLot.Location = new System.Drawing.Point(188, 128);
            this.txtKesilecekLot.Name = "txtKesilecekLot";
            this.txtKesilecekLot.Size = new System.Drawing.Size(158, 22);
            this.txtKesilecekLot.TabIndex = 7;
            // 
            // lblAramaYap
            // 
            this.lblAramaYap.AutoSize = true;
            this.lblAramaYap.Location = new System.Drawing.Point(29, 79);
            this.lblAramaYap.Name = "lblAramaYap";
            this.lblAramaYap.Size = new System.Drawing.Size(78, 16);
            this.lblAramaYap.TabIndex = 0;
            this.lblAramaYap.Text = "Arama Yap:";
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(189, 69);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(157, 36);
            this.btnAra.TabIndex = 1;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // lblKesimYap
            // 
            this.lblKesimYap.AutoSize = true;
            this.lblKesimYap.Location = new System.Drawing.Point(29, 180);
            this.lblKesimYap.Name = "lblKesimYap";
            this.lblKesimYap.Size = new System.Drawing.Size(75, 16);
            this.lblKesimYap.TabIndex = 4;
            this.lblKesimYap.Text = "Kesim Yap:";
            // 
            // btnPaketKes
            // 
            this.btnPaketKes.Location = new System.Drawing.Point(189, 170);
            this.btnPaketKes.Name = "btnPaketKes";
            this.btnPaketKes.Size = new System.Drawing.Size(157, 36);
            this.btnPaketKes.TabIndex = 5;
            this.btnPaketKes.Text = "Seçili Paketi Kes";
            this.btnPaketKes.UseVisualStyleBackColor = true;
            this.btnPaketKes.Click += new System.EventHandler(this.btnPaketKes_Click);
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.topLayoutPanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.bottomLayoutPanel, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 50);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1820, 734);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1820, 50);
            this.ctlBaslik1.TabIndex = 1;
            // 
            // ctlKesimYap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutPanel);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlKesimYap";
            this.Size = new System.Drawing.Size(1820, 784);
            this.Load += new System.EventHandler(this.ctlKesimYap_Load);
            this.bottomLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetay)).EndInit();
            this.topLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesimListesi)).EndInit();
            this.rightPanel.ResumeLayout(false);
            this.rightPanel.PerformLayout();
            this.mainLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel bottomLayoutPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridDetay;
        private System.Windows.Forms.TableLayoutPanel topLayoutPanel;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.DataGridView dataGridKesimListesi;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Label lblAramaYap;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.Label lblKesimYap;
        private System.Windows.Forms.Button btnPaketKes;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Label lblKesilecekLot;
        private System.Windows.Forms.TextBox txtKesilecekLot;
    }
}