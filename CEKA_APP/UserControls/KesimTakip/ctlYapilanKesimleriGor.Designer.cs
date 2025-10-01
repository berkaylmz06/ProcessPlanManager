namespace CEKA_APP.UsrControl
{
    partial class ctlYapilanKesimleriGor
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
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.topLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewTamamlanmisKesimListesi = new System.Windows.Forms.DataGridView();
            this.dataGridViewTamamlanmisHareket = new System.Windows.Forms.DataGridView();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.bottomLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridTamamlanmisDetay = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.mainLayoutPanel.SuspendLayout();
            this.topLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).BeginInit();
            this.rightPanel.SuspendLayout();
            this.bottomLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).BeginInit();
            this.SuspendLayout();
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
            this.mainLayoutPanel.Size = new System.Drawing.Size(1575, 890);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // topLayoutPanel
            // 
            this.topLayoutPanel.ColumnCount = 3;
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.topLayoutPanel.Controls.Add(this.label4, 0, 0);
            this.topLayoutPanel.Controls.Add(this.label2, 2, 0);
            this.topLayoutPanel.Controls.Add(this.dataGridViewTamamlanmisKesimListesi, 0, 1);
            this.topLayoutPanel.Controls.Add(this.dataGridViewTamamlanmisHareket, 2, 1);
            this.topLayoutPanel.Controls.Add(this.rightPanel, 1, 1);
            this.topLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.topLayoutPanel.Name = "topLayoutPanel";
            this.topLayoutPanel.RowCount = 2;
            this.topLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.topLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topLayoutPanel.Size = new System.Drawing.Size(1569, 439);
            this.topLayoutPanel.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(700, 30);
            this.label4.TabIndex = 0;
            this.label4.Text = "Tamamlanmış Kesimler Paket";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(865, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(701, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tamamlanmış Kesimlerin Hareketleri";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dataGridViewTamamlanmisKesimListesi
            // 
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTamamlanmisKesimListesi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewTamamlanmisKesimListesi.ColumnHeadersHeight = 29;
            this.dataGridViewTamamlanmisKesimListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTamamlanmisKesimListesi.Location = new System.Drawing.Point(3, 33);
            this.dataGridViewTamamlanmisKesimListesi.Name = "dataGridViewTamamlanmisKesimListesi";
            this.dataGridViewTamamlanmisKesimListesi.ReadOnly = true;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersWidth = 51;
            this.dataGridViewTamamlanmisKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisKesimListesi.Size = new System.Drawing.Size(700, 403);
            this.dataGridViewTamamlanmisKesimListesi.TabIndex = 2;
            this.dataGridViewTamamlanmisKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTamamlanmisKesimListesi_CellClick);
            // 
            // dataGridViewTamamlanmisHareket
            // 
            this.dataGridViewTamamlanmisHareket.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisHareket.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisHareket.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTamamlanmisHareket.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewTamamlanmisHareket.ColumnHeadersHeight = 29;
            this.dataGridViewTamamlanmisHareket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTamamlanmisHareket.Location = new System.Drawing.Point(865, 33);
            this.dataGridViewTamamlanmisHareket.Name = "dataGridViewTamamlanmisHareket";
            this.dataGridViewTamamlanmisHareket.ReadOnly = true;
            this.dataGridViewTamamlanmisHareket.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisHareket.RowHeadersWidth = 51;
            this.dataGridViewTamamlanmisHareket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisHareket.Size = new System.Drawing.Size(701, 403);
            this.dataGridViewTamamlanmisHareket.TabIndex = 3;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.btnAra);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanel.Location = new System.Drawing.Point(709, 33);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(150, 403);
            this.rightPanel.TabIndex = 4;
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(5, 5);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(157, 36);
            this.btnAra.TabIndex = 0;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // bottomLayoutPanel
            // 
            this.bottomLayoutPanel.ColumnCount = 1;
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.bottomLayoutPanel.Controls.Add(this.dataGridTamamlanmisDetay, 0, 1);
            this.bottomLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLayoutPanel.Location = new System.Drawing.Point(3, 448);
            this.bottomLayoutPanel.Name = "bottomLayoutPanel";
            this.bottomLayoutPanel.RowCount = 2;
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Size = new System.Drawing.Size(1569, 439);
            this.bottomLayoutPanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1563, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tamamlanmış Kesim Detay";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dataGridTamamlanmisDetay
            // 
            this.dataGridTamamlanmisDetay.AllowUserToAddRows = false;
            this.dataGridTamamlanmisDetay.AllowUserToDeleteRows = false;
            this.dataGridTamamlanmisDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridTamamlanmisDetay.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridTamamlanmisDetay.ColumnHeadersHeight = 29;
            this.dataGridTamamlanmisDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridTamamlanmisDetay.Location = new System.Drawing.Point(3, 33);
            this.dataGridTamamlanmisDetay.Name = "dataGridTamamlanmisDetay";
            this.dataGridTamamlanmisDetay.ReadOnly = true;
            this.dataGridTamamlanmisDetay.RowHeadersVisible = false;
            this.dataGridTamamlanmisDetay.RowHeadersWidth = 51;
            this.dataGridTamamlanmisDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTamamlanmisDetay.Size = new System.Drawing.Size(1563, 403);
            this.dataGridTamamlanmisDetay.TabIndex = 1;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1575, 50);
            this.ctlBaslik1.TabIndex = 1;
            // 
            // ctlYapilanKesimleriGor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutPanel);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlYapilanKesimleriGor";
            this.Size = new System.Drawing.Size(1575, 940);
            this.Load += new System.EventHandler(this.ctlYapilanKesimleriGor_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.topLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).EndInit();
            this.rightPanel.ResumeLayout(false);
            this.bottomLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // Kontrol değişkenleri
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel topLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel bottomLayoutPanel;
        private System.Windows.Forms.Panel rightPanel;

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