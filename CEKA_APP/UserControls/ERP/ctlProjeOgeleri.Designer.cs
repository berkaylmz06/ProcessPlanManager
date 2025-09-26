namespace CEKA_APP.UsrControl
{
    partial class ctlProjeOgeleri
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelList = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSpacer2 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridOgeDetay = new System.Windows.Forms.DataGridView();
            this.panelBilgi = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnYeni = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnStandartProje = new System.Windows.Forms.Button();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 50);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2348, 1206);
            this.tableLayoutPanel1.TabIndex = 142;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelList);
            this.panel1.Controls.Add(this.panelSpacer1);
            this.panel1.Controls.Add(this.panelSearch);
            this.panel1.Controls.Add(this.panelSpacer2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.MinimumSize = new System.Drawing.Size(375, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 1198);
            this.panel1.TabIndex = 0;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.treeView1);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(0, 113);
            this.panelList.Margin = new System.Windows.Forms.Padding(4);
            this.panelList.MinimumSize = new System.Drawing.Size(375, 375);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(696, 1085);
            this.panelList.TabIndex = 143;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4);
            this.treeView1.MinimumSize = new System.Drawing.Size(374, 374);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(696, 1085);
            this.treeView1.TabIndex = 9;
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer1.Location = new System.Drawing.Point(0, 101);
            this.panelSpacer1.Margin = new System.Windows.Forms.Padding(4);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(696, 12);
            this.panelSpacer1.TabIndex = 145;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelSearch.Controls.Add(this.btnAra);
            this.panelSearch.Controls.Add(this.txtProjeNo);
            this.panelSearch.Controls.Add(this.label1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 12);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(4);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(696, 89);
            this.panelSearch.TabIndex = 142;
            // 
            // btnAra
            // 
            this.btnAra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAra.Location = new System.Drawing.Point(569, 25);
            this.btnAra.Margin = new System.Windows.Forms.Padding(4);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(112, 41);
            this.btnAra.TabIndex = 141;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjeNo.Location = new System.Drawing.Point(25, 34);
            this.txtProjeNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(536, 22);
            this.txtProjeNo.TabIndex = 140;
            this.txtProjeNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeNo_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(22, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 140;
            this.label1.Text = "Proje No";
            // 
            // panelSpacer2
            // 
            this.panelSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer2.Location = new System.Drawing.Point(0, 0);
            this.panelSpacer2.Margin = new System.Windows.Forms.Padding(4);
            this.panelSpacer2.Name = "panelSpacer2";
            this.panelSpacer2.Size = new System.Drawing.Size(696, 12);
            this.panelSpacer2.TabIndex = 144;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Controls.Add(this.panelButtons);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(708, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.MinimumSize = new System.Drawing.Size(800, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1636, 1198);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 81);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridOgeDetay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelBilgi);
            this.splitContainer1.Size = new System.Drawing.Size(1636, 1117);
            this.splitContainer1.SplitterDistance = 632;
            this.splitContainer1.TabIndex = 141;
            // 
            // dataGridOgeDetay
            // 
            this.dataGridOgeDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOgeDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOgeDetay.Location = new System.Drawing.Point(0, 0);
            this.dataGridOgeDetay.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridOgeDetay.Name = "dataGridOgeDetay";
            this.dataGridOgeDetay.RowHeadersWidth = 51;
            this.dataGridOgeDetay.RowTemplate.Height = 24;
            this.dataGridOgeDetay.Size = new System.Drawing.Size(1636, 632);
            this.dataGridOgeDetay.TabIndex = 139;
            // 
            // panelBilgi
            // 
            this.panelBilgi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBilgi.Location = new System.Drawing.Point(0, 0);
            this.panelBilgi.Margin = new System.Windows.Forms.Padding(4);
            this.panelBilgi.Name = "panelBilgi";
            this.panelBilgi.Size = new System.Drawing.Size(1636, 481);
            this.panelBilgi.TabIndex = 140;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.panel4);
            this.panelButtons.Controls.Add(this.panel3);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(4);
            this.panelButtons.MinimumSize = new System.Drawing.Size(800, 81);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1636, 81);
            this.panelButtons.TabIndex = 141;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnKaydet);
            this.panel4.Controls.Add(this.btnYeni);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1259, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(377, 81);
            this.panel4.TabIndex = 145;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKaydet.Location = new System.Drawing.Point(193, 14);
            this.btnKaydet.Margin = new System.Windows.Forms.Padding(4);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(170, 54);
            this.btnKaydet.TabIndex = 143;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnYeni
            // 
            this.btnYeni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYeni.Location = new System.Drawing.Point(15, 14);
            this.btnYeni.Margin = new System.Windows.Forms.Padding(4);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new System.Drawing.Size(170, 54);
            this.btnYeni.TabIndex = 142;
            this.btnYeni.Text = "Yeni Ekle";
            this.btnYeni.UseVisualStyleBackColor = true;
            this.btnYeni.Click += new System.EventHandler(this.btnYeni_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnStandartProje);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.MinimumSize = new System.Drawing.Size(200, 81);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 81);
            this.panel3.TabIndex = 144;
            // 
            // btnStandartProje
            // 
            this.btnStandartProje.Location = new System.Drawing.Point(14, 14);
            this.btnStandartProje.Margin = new System.Windows.Forms.Padding(4);
            this.btnStandartProje.MinimumSize = new System.Drawing.Size(150, 40);
            this.btnStandartProje.Name = "btnStandartProje";
            this.btnStandartProje.Size = new System.Drawing.Size(170, 54);
            this.btnStandartProje.TabIndex = 141;
            this.btnStandartProje.Text = "Standart Proje";
            this.btnStandartProje.UseVisualStyleBackColor = true;
            this.btnStandartProje.Click += new System.EventHandler(this.btnStandartProje_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(2348, 50);
            this.ctlBaslik1.TabIndex = 144;
            // 
            // ctlProjeOgeleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ctlBaslik1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ctlProjeOgeleri";
            this.Size = new System.Drawing.Size(2348, 1256);
            this.Load += new System.EventHandler(this.ctlProjeOgeleri_Load);
            this.Resize += new System.EventHandler(this.ctlProjeOgeleri_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelSpacer1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSpacer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridOgeDetay;
        private System.Windows.Forms.Panel panelBilgi;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnYeni;
        private System.Windows.Forms.Button btnStandartProje;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private ctlBaslik ctlBaslik1;
    }
}
