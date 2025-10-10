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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.kesimListesiPanel = new System.Windows.Forms.Panel();
            this.dataGridViewTamamlanmisKesimListesi = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.detailLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.hareketPanel = new System.Windows.Forms.Panel();
            this.dataGridViewTamamlanmisHareket = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.detayPanel = new System.Windows.Forms.Panel();
            this.dataGridTamamlanmisDetay = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.mainLayoutPanel.SuspendLayout();
            this.kesimListesiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.detailLayoutPanel.SuspendLayout();
            this.hareketPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).BeginInit();
            this.detayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainLayoutPanel.Controls.Add(this.kesimListesiPanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.detailLayoutPanel, 1, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 50);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 1;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1575, 890);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // kesimListesiPanel
            // 
            this.kesimListesiPanel.Controls.Add(this.dataGridViewTamamlanmisKesimListesi);
            this.kesimListesiPanel.Controls.Add(this.panel1);
            this.kesimListesiPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kesimListesiPanel.Location = new System.Drawing.Point(3, 3);
            this.kesimListesiPanel.Name = "kesimListesiPanel";
            this.kesimListesiPanel.Size = new System.Drawing.Size(624, 884);
            this.kesimListesiPanel.TabIndex = 0;
            // 
            // dataGridViewTamamlanmisKesimListesi
            // 
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisKesimListesi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTamamlanmisKesimListesi.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTamamlanmisKesimListesi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTamamlanmisKesimListesi.ColumnHeadersHeight = 35;
            this.dataGridViewTamamlanmisKesimListesi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTamamlanmisKesimListesi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTamamlanmisKesimListesi.EnableHeadersVisualStyles = false;
            this.dataGridViewTamamlanmisKesimListesi.Location = new System.Drawing.Point(0, 38);
            this.dataGridViewTamamlanmisKesimListesi.Name = "dataGridViewTamamlanmisKesimListesi";
            this.dataGridViewTamamlanmisKesimListesi.ReadOnly = true;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisKesimListesi.RowHeadersWidth = 51;
            this.dataGridViewTamamlanmisKesimListesi.RowTemplate.Height = 28;
            this.dataGridViewTamamlanmisKesimListesi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisKesimListesi.Size = new System.Drawing.Size(624, 846);
            this.dataGridViewTamamlanmisKesimListesi.TabIndex = 2;
            this.dataGridViewTamamlanmisKesimListesi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTamamlanmisKesimListesi_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 38);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(516, 38);
            this.panel3.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(516, 35);
            this.label4.TabIndex = 1;
            this.label4.Text = "Tamamlanmış Kesimler (Paket Listesi)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAra);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(516, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(108, 38);
            this.panel2.TabIndex = 0;
            // 
            // btnAra
            // 
            this.btnAra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnAra.FlatAppearance.BorderSize = 0;
            this.btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAra.ForeColor = System.Drawing.Color.White;
            this.btnAra.Location = new System.Drawing.Point(7, 3);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(98, 32);
            this.btnAra.TabIndex = 0;
            this.btnAra.Text = "ARA";
            this.btnAra.UseVisualStyleBackColor = false;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // detailLayoutPanel
            // 
            this.detailLayoutPanel.ColumnCount = 1;
            this.detailLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailLayoutPanel.Controls.Add(this.hareketPanel, 0, 0);
            this.detailLayoutPanel.Controls.Add(this.detayPanel, 0, 1);
            this.detailLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailLayoutPanel.Location = new System.Drawing.Point(633, 3);
            this.detailLayoutPanel.Name = "detailLayoutPanel";
            this.detailLayoutPanel.RowCount = 2;
            this.detailLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.93665F));
            this.detailLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.06335F));
            this.detailLayoutPanel.Size = new System.Drawing.Size(939, 884);
            this.detailLayoutPanel.TabIndex = 1;
            // 
            // hareketPanel
            // 
            this.hareketPanel.Controls.Add(this.dataGridViewTamamlanmisHareket);
            this.hareketPanel.Controls.Add(this.label2);
            this.hareketPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hareketPanel.Location = new System.Drawing.Point(3, 3);
            this.hareketPanel.Name = "hareketPanel";
            this.hareketPanel.Size = new System.Drawing.Size(933, 293);
            this.hareketPanel.TabIndex = 0;
            // 
            // dataGridViewTamamlanmisHareket
            // 
            this.dataGridViewTamamlanmisHareket.AllowUserToAddRows = false;
            this.dataGridViewTamamlanmisHareket.AllowUserToDeleteRows = false;
            this.dataGridViewTamamlanmisHareket.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTamamlanmisHareket.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTamamlanmisHareket.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTamamlanmisHareket.ColumnHeadersHeight = 35;
            this.dataGridViewTamamlanmisHareket.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewTamamlanmisHareket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTamamlanmisHareket.EnableHeadersVisualStyles = false;
            this.dataGridViewTamamlanmisHareket.Location = new System.Drawing.Point(0, 35);
            this.dataGridViewTamamlanmisHareket.Name = "dataGridViewTamamlanmisHareket";
            this.dataGridViewTamamlanmisHareket.ReadOnly = true;
            this.dataGridViewTamamlanmisHareket.RowHeadersVisible = false;
            this.dataGridViewTamamlanmisHareket.RowHeadersWidth = 51;
            this.dataGridViewTamamlanmisHareket.RowTemplate.Height = 28;
            this.dataGridViewTamamlanmisHareket.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTamamlanmisHareket.Size = new System.Drawing.Size(933, 258);
            this.dataGridViewTamamlanmisHareket.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(933, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Seçili Kesimin Hareketleri (İşlem Süreleri)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // detayPanel
            // 
            this.detayPanel.Controls.Add(this.dataGridTamamlanmisDetay);
            this.detayPanel.Controls.Add(this.label1);
            this.detayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detayPanel.Location = new System.Drawing.Point(3, 302);
            this.detayPanel.Name = "detayPanel";
            this.detayPanel.Size = new System.Drawing.Size(933, 579);
            this.detayPanel.TabIndex = 1;
            // 
            // dataGridTamamlanmisDetay
            // 
            this.dataGridTamamlanmisDetay.AllowUserToAddRows = false;
            this.dataGridTamamlanmisDetay.AllowUserToDeleteRows = false;
            this.dataGridTamamlanmisDetay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridTamamlanmisDetay.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridTamamlanmisDetay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridTamamlanmisDetay.ColumnHeadersHeight = 35;
            this.dataGridTamamlanmisDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridTamamlanmisDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridTamamlanmisDetay.EnableHeadersVisualStyles = false;
            this.dataGridTamamlanmisDetay.Location = new System.Drawing.Point(0, 35);
            this.dataGridTamamlanmisDetay.Name = "dataGridTamamlanmisDetay";
            this.dataGridTamamlanmisDetay.ReadOnly = true;
            this.dataGridTamamlanmisDetay.RowHeadersVisible = false;
            this.dataGridTamamlanmisDetay.RowHeadersWidth = 51;
            this.dataGridTamamlanmisDetay.RowTemplate.Height = 28;
            this.dataGridTamamlanmisDetay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTamamlanmisDetay.Size = new System.Drawing.Size(933, 544);
            this.dataGridTamamlanmisDetay.TabIndex = 1;
            this.dataGridTamamlanmisDetay.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridTamamlanmisDetay_CellFormatting);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(933, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Seçili Kesimin Detayları (Plan Bilgisi)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mainLayoutPanel);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlYapilanKesimleriGor";
            this.Size = new System.Drawing.Size(1575, 940);
            this.Load += new System.EventHandler(this.ctlYapilanKesimleriGor_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.kesimListesiPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisKesimListesi)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.detailLayoutPanel.ResumeLayout(false);
            this.hareketPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTamamlanmisHareket)).EndInit();
            this.detayPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTamamlanmisDetay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // Kontrol değişkenleri
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Panel kesimListesiPanel;
        private System.Windows.Forms.TableLayoutPanel detailLayoutPanel;
        private System.Windows.Forms.Panel hareketPanel;
        private System.Windows.Forms.Panel detayPanel;

        private System.Windows.Forms.Button btnAra;
        public System.Windows.Forms.DataGridView dataGridViewTamamlanmisKesimListesi;
        private System.Windows.Forms.DataGridView dataGridTamamlanmisDetay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.DataGridView dataGridViewTamamlanmisHareket;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
    }
}