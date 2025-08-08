namespace CEKA_APP.UsrControl
{
    partial class ctlProjeOgeleri
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelList = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSpacer2 = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblProjeOgeleri = new System.Windows.Forms.Label();
            this.panelBilgi = new System.Windows.Forms.Panel();
            this.dataGridOgeDetay = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnYeni = new System.Windows.Forms.Button();
            this.btnStandartProje = new System.Windows.Forms.Button();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 63);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelList);
            this.splitContainer1.Panel1.Controls.Add(this.panelSpacer1);
            this.splitContainer1.Panel1.Controls.Add(this.panelSearch);
            this.splitContainer1.Panel1.Controls.Add(this.panelSpacer2);
            this.splitContainer1.Panel1.Controls.Add(this.panelHeader);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelBilgi);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridOgeDetay);
            this.splitContainer1.Panel2.Controls.Add(this.panelButtons);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(1878, 928);
            this.splitContainer1.SplitterDistance = 442;
            this.splitContainer1.TabIndex = 142;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.treeView1);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(3, 148);
            this.panelList.Margin = new System.Windows.Forms.Padding(10);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(436, 777);
            this.panelList.TabIndex = 143;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(436, 777);
            this.treeView1.TabIndex = 9;
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer1.Location = new System.Drawing.Point(3, 138);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(436, 10);
            this.panelSpacer1.TabIndex = 145;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelSearch.Controls.Add(this.btnAra);
            this.panelSearch.Controls.Add(this.txtProjeNo);
            this.panelSearch.Controls.Add(this.label1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(3, 67);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(5);
            this.panelSearch.Size = new System.Drawing.Size(436, 71);
            this.panelSearch.TabIndex = 142;
            // 
            // btnAra
            // 
            this.btnAra.Location = new System.Drawing.Point(328, 21);
            this.btnAra.Name = "btnAra";
            this.btnAra.Size = new System.Drawing.Size(92, 33);
            this.btnAra.TabIndex = 141;
            this.btnAra.Text = "Ara";
            this.btnAra.UseVisualStyleBackColor = true;
            this.btnAra.Click += new System.EventHandler(this.btnAra_Click);
            // 
            // txtProjeNo
            // 
            this.txtProjeNo.Location = new System.Drawing.Point(6, 25);
            this.txtProjeNo.Name = "txtProjeNo";
            this.txtProjeNo.Size = new System.Drawing.Size(316, 22);
            this.txtProjeNo.TabIndex = 140;
            this.txtProjeNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProjeNo_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 140;
            this.label1.Text = "Proje No";
            // 
            // panelSpacer2
            // 
            this.panelSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer2.Location = new System.Drawing.Point(3, 57);
            this.panelSpacer2.Name = "panelSpacer2";
            this.panelSpacer2.Size = new System.Drawing.Size(436, 10);
            this.panelSpacer2.TabIndex = 144;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelHeader.Controls.Add(this.lblProjeOgeleri);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(436, 54);
            this.panelHeader.TabIndex = 141;
            // 
            // lblProjeOgeleri
            // 
            this.lblProjeOgeleri.AutoSize = true;
            this.lblProjeOgeleri.BackColor = System.Drawing.Color.Transparent;
            this.lblProjeOgeleri.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProjeOgeleri.ForeColor = System.Drawing.Color.Orange;
            this.lblProjeOgeleri.Location = new System.Drawing.Point(3, 15);
            this.lblProjeOgeleri.Name = "lblProjeOgeleri";
            this.lblProjeOgeleri.Size = new System.Drawing.Size(107, 18);
            this.lblProjeOgeleri.TabIndex = 137;
            this.lblProjeOgeleri.Text = "Proje Öğeleri";
            // 
            // panelBilgi
            // 
            this.panelBilgi.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBilgi.Location = new System.Drawing.Point(3, 670);
            this.panelBilgi.Name = "panelBilgi";
            this.panelBilgi.Size = new System.Drawing.Size(1426, 255);
            this.panelBilgi.TabIndex = 18;
            // 
            // dataGridOgeDetay
            // 
            this.dataGridOgeDetay.AllowUserToAddRows = false;
            this.dataGridOgeDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOgeDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridOgeDetay.Location = new System.Drawing.Point(3, 122);
            this.dataGridOgeDetay.Name = "dataGridOgeDetay";
            this.dataGridOgeDetay.RowHeadersWidth = 51;
            this.dataGridOgeDetay.RowTemplate.Height = 24;
            this.dataGridOgeDetay.Size = new System.Drawing.Size(1426, 803);
            this.dataGridOgeDetay.TabIndex = 16;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnKaydet);
            this.panelButtons.Controls.Add(this.btnYeni);
            this.panelButtons.Controls.Add(this.btnStandartProje);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(3, 3);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1426, 119);
            this.panelButtons.TabIndex = 17;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(255, 16);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(210, 38);
            this.btnKaydet.TabIndex = 12;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnYeni
            // 
            this.btnYeni.Location = new System.Drawing.Point(485, 16);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new System.Drawing.Size(210, 38);
            this.btnYeni.TabIndex = 11;
            this.btnYeni.Text = "Yeni";
            this.btnYeni.UseVisualStyleBackColor = true;
            this.btnYeni.Click += new System.EventHandler(this.btnYeni_Click);
            // 
            // btnStandartProje
            // 
            this.btnStandartProje.Location = new System.Drawing.Point(25, 16);
            this.btnStandartProje.Name = "btnStandartProje";
            this.btnStandartProje.Size = new System.Drawing.Size(210, 38);
            this.btnStandartProje.TabIndex = 13;
            this.btnStandartProje.Text = "Standart Projeler";
            this.btnStandartProje.UseVisualStyleBackColor = true;
            this.btnStandartProje.Click += new System.EventHandler(this.btnStandartProje_Click);
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1878, 63);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // ctlProjeOgeleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeOgeleri";
            this.Size = new System.Drawing.Size(1878, 991);
            this.Load += new System.EventHandler(this.ctlProjeOgeleri_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelSpacer1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSpacer2;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblProjeOgeleri;
        private System.Windows.Forms.Panel panelBilgi;
        private System.Windows.Forms.DataGridView dataGridOgeDetay;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnYeni;
        private System.Windows.Forms.Button btnStandartProje;
    }
}
