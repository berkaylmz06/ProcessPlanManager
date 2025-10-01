namespace CEKA_APP.UsrControl
{
    partial class ctlKarsilastirmaTablosu
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridKalite = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridMalzeme = new System.Windows.Forms.DataGridView();
            this.btnEkle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtIfsCode = new System.Windows.Forms.TextBox();
            this.txtAciklama = new System.Windows.Forms.TextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelIfsCode = new System.Windows.Forms.Label();
            this.labelAciklama = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridKesim = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKalite)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMalzeme)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesim)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(810, 995);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridKalite);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(802, 966);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kalite Karşılaştırma";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridKalite
            // 
            this.dataGridKalite.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKalite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKalite.Location = new System.Drawing.Point(3, 3);
            this.dataGridKalite.Name = "dataGridKalite";
            this.dataGridKalite.ReadOnly = true;
            this.dataGridKalite.RowHeadersWidth = 51;
            this.dataGridKalite.RowTemplate.Height = 24;
            this.dataGridKalite.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKalite.Size = new System.Drawing.Size(796, 960);
            this.dataGridKalite.TabIndex = 2;
            this.dataGridKalite.SelectionChanged += new System.EventHandler(this.dataGridKalite_SelectionChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridMalzeme);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(802, 966);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Malzeme Karşılaştırma";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridMalzeme
            // 
            this.dataGridMalzeme.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMalzeme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMalzeme.Location = new System.Drawing.Point(3, 3);
            this.dataGridMalzeme.Name = "dataGridMalzeme";
            this.dataGridMalzeme.ReadOnly = true;
            this.dataGridMalzeme.RowHeadersWidth = 51;
            this.dataGridMalzeme.RowTemplate.Height = 24;
            this.dataGridMalzeme.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridMalzeme.Size = new System.Drawing.Size(796, 960);
            this.dataGridMalzeme.TabIndex = 2;
            this.dataGridMalzeme.SelectionChanged += new System.EventHandler(this.dataGridMalzeme_SelectionChanged);
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(848, 237);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(123, 45);
            this.btnEkle.TabIndex = 2;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(996, 237);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(123, 47);
            this.btnSil.TabIndex = 3;
            this.btnSil.Text = "Sil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(996, 109);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(144, 22);
            this.txtCode.TabIndex = 5;
            // 
            // txtIfsCode
            // 
            this.txtIfsCode.Location = new System.Drawing.Point(996, 149);
            this.txtIfsCode.Name = "txtIfsCode";
            this.txtIfsCode.Size = new System.Drawing.Size(144, 22);
            this.txtIfsCode.TabIndex = 6;
            // 
            // txtAciklama
            // 
            this.txtAciklama.Location = new System.Drawing.Point(996, 188);
            this.txtAciklama.Name = "txtAciklama";
            this.txtAciklama.Size = new System.Drawing.Size(144, 22);
            this.txtAciklama.TabIndex = 7;
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(860, 112);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(54, 20);
            this.labelCode.TabIndex = 8;
            this.labelCode.Text = "Code:";
            // 
            // labelIfsCode
            // 
            this.labelIfsCode.AutoSize = true;
            this.labelIfsCode.Location = new System.Drawing.Point(860, 152);
            this.labelIfsCode.Name = "labelIfsCode";
            this.labelIfsCode.Size = new System.Drawing.Size(64, 16);
            this.labelIfsCode.TabIndex = 9;
            this.labelIfsCode.Text = "IFS Kodu:";
            // 
            // labelAciklama
            // 
            this.labelAciklama.AutoSize = true;
            this.labelAciklama.Location = new System.Drawing.Point(860, 191);
            this.labelAciklama.Name = "labelAciklama";
            this.labelAciklama.Size = new System.Drawing.Size(66, 16);
            this.labelAciklama.TabIndex = 10;
            this.labelAciklama.Text = "Açıklama:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 995);
            this.panel1.TabIndex = 11;
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1759, 50);
            this.ctlBaslik1.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridKesim);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(802, 966);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Kesim Karşılaştırma";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridKesim
            // 
            this.dataGridKesim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridKesim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKesim.Location = new System.Drawing.Point(0, 0);
            this.dataGridKesim.Name = "dataGridKesim";
            this.dataGridKesim.ReadOnly = true;
            this.dataGridKesim.RowHeadersWidth = 51;
            this.dataGridKesim.RowTemplate.Height = 24;
            this.dataGridKesim.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKesim.Size = new System.Drawing.Size(802, 966);
            this.dataGridKesim.TabIndex = 3;
            this.dataGridKesim.SelectionChanged += new System.EventHandler(this.dataGridKesim_SelectionChanged);
            // 
            // ctlKarsilastirmaTablosu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelAciklama);
            this.Controls.Add(this.labelIfsCode);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.txtAciklama);
            this.Controls.Add(this.txtIfsCode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlKarsilastirmaTablosu";
            this.Size = new System.Drawing.Size(1759, 1045);
            this.Load += new System.EventHandler(this.ctlKarsilastirmaTablosu_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKalite)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMalzeme)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridKesim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.DataGridView dataGridKalite;
        private System.Windows.Forms.DataGridView dataGridMalzeme;
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtIfsCode;
        private System.Windows.Forms.TextBox txtAciklama;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelIfsCode;
        private System.Windows.Forms.Label labelAciklama;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridKesim;
    }
}
