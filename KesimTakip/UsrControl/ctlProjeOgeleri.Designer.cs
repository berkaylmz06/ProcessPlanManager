namespace KesimTakip.UsrControl
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelDisContainer = new System.Windows.Forms.Panel();
            this.panelList = new System.Windows.Forms.Panel();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnAra = new System.Windows.Forms.Button();
            this.txtProjeNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSpacer2 = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblProjeOgeleri = new System.Windows.Forms.Label();
            this.panelDetayContainer = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnStandartProje = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnYeni = new System.Windows.Forms.Button();
            this.dataGridOgeDetay = new System.Windows.Forms.DataGridView();
            this.ctlBaslik1 = new KesimTakip.UsrControl.ctlBaslik();
            this.panelDisContainer.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelDetayContainer.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(425, 787);
            this.treeView1.TabIndex = 9;
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panelDisContainer
            // 
            this.panelDisContainer.BackColor = System.Drawing.SystemColors.Control;
            this.panelDisContainer.Controls.Add(this.panelList);
            this.panelDisContainer.Controls.Add(this.panelSpacer1);
            this.panelDisContainer.Controls.Add(this.panelSearch);
            this.panelDisContainer.Controls.Add(this.panelSpacer2);
            this.panelDisContainer.Controls.Add(this.panelHeader);
            this.panelDisContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDisContainer.Location = new System.Drawing.Point(0, 50);
            this.panelDisContainer.Name = "panelDisContainer";
            this.panelDisContainer.Padding = new System.Windows.Forms.Padding(10);
            this.panelDisContainer.Size = new System.Drawing.Size(445, 954);
            this.panelDisContainer.TabIndex = 139;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.treeView1);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(10, 157);
            this.panelList.Margin = new System.Windows.Forms.Padding(10);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(425, 787);
            this.panelList.TabIndex = 139;
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer1.Location = new System.Drawing.Point(10, 141);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(425, 16);
            this.panelSpacer1.TabIndex = 140;
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelSearch.Controls.Add(this.btnAra);
            this.panelSearch.Controls.Add(this.txtProjeNo);
            this.panelSearch.Controls.Add(this.label1);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(10, 76);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(5);
            this.panelSearch.Size = new System.Drawing.Size(425, 65);
            this.panelSearch.TabIndex = 138;
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
            this.panelSpacer2.Location = new System.Drawing.Point(10, 60);
            this.panelSpacer2.Name = "panelSpacer2";
            this.panelSpacer2.Size = new System.Drawing.Size(425, 16);
            this.panelSpacer2.TabIndex = 139;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelHeader.Controls.Add(this.lblProjeOgeleri);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(10, 10);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(425, 50);
            this.panelHeader.TabIndex = 137;
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
            // panelDetayContainer
            // 
            this.panelDetayContainer.Controls.Add(this.panelButtons);
            this.panelDetayContainer.Controls.Add(this.dataGridOgeDetay);
            this.panelDetayContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetayContainer.Location = new System.Drawing.Point(0, 50);
            this.panelDetayContainer.Name = "panelDetayContainer";
            this.panelDetayContainer.Size = new System.Drawing.Size(1619, 954);
            this.panelDetayContainer.TabIndex = 140;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnStandartProje);
            this.panelButtons.Controls.Add(this.btnKaydet);
            this.panelButtons.Controls.Add(this.btnYeni);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1619, 70);
            this.panelButtons.TabIndex = 14;
            // 
            // btnStandartProje
            // 
            this.btnStandartProje.Location = new System.Drawing.Point(471, 16);
            this.btnStandartProje.Name = "btnStandartProje";
            this.btnStandartProje.Size = new System.Drawing.Size(156, 38);
            this.btnStandartProje.TabIndex = 13;
            this.btnStandartProje.Text = "Standart Projeler";
            this.btnStandartProje.UseVisualStyleBackColor = true;
            this.btnStandartProje.Click += new System.EventHandler(this.btnStandartProje_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(643, 16);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(156, 38);
            this.btnKaydet.TabIndex = 12;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnYeni
            // 
            this.btnYeni.Location = new System.Drawing.Point(820, 16);
            this.btnYeni.Name = "btnYeni";
            this.btnYeni.Size = new System.Drawing.Size(156, 38);
            this.btnYeni.TabIndex = 11;
            this.btnYeni.Text = "Yeni";
            this.btnYeni.UseVisualStyleBackColor = true;
            this.btnYeni.Click += new System.EventHandler(this.btnYeni_Click);
            // 
            // dataGridOgeDetay
            // 
            this.dataGridOgeDetay.AllowUserToAddRows = false;
            this.dataGridOgeDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridOgeDetay.Location = new System.Drawing.Point(471, 97);
            this.dataGridOgeDetay.Name = "dataGridOgeDetay";
            this.dataGridOgeDetay.RowHeadersWidth = 51;
            this.dataGridOgeDetay.RowTemplate.Height = 24;
            this.dataGridOgeDetay.Size = new System.Drawing.Size(966, 488);
            this.dataGridOgeDetay.TabIndex = 10;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1619, 50);
            this.ctlBaslik1.TabIndex = 0;
            // 
            // ctlProjeOgeleri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDisContainer);
            this.Controls.Add(this.panelDetayContainer);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlProjeOgeleri";
            this.Size = new System.Drawing.Size(1619, 1004);
            this.Load += new System.EventHandler(this.ctlProjeOgeleri_Load);
            this.panelDisContainer.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelDetayContainer.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridOgeDetay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelDisContainer;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Panel panelSpacer1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblProjeOgeleri;
        private System.Windows.Forms.Panel panelSpacer2;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnAra;
        private System.Windows.Forms.TextBox txtProjeNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelDetayContainer;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.DataGridView dataGridOgeDetay;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnYeni;
        private System.Windows.Forms.Button btnStandartProje;
        private System.Windows.Forms.Panel panelButtons;
    }
}
