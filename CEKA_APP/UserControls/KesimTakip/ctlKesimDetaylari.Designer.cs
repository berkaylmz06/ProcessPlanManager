namespace CEKA_APP.UsrControl
{
    partial class ctlKesimDetaylari
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelDisContainer = new System.Windows.Forms.Panel();
            this.panelList = new System.Windows.Forms.Panel();
            this.lstPozlar = new System.Windows.Forms.ListBox();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblPozListesi = new System.Windows.Forms.Label();
            this.panelSpacer2 = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelChart = new System.Windows.Forms.Panel();
            this.chartKesim = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelSpacer3 = new System.Windows.Forms.Panel();
            this.panelKartContainer = new System.Windows.Forms.Panel();
            this.lblBilgi = new System.Windows.Forms.Label();
            this.panelKart4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblToplamPozIfsKarsiligi = new System.Windows.Forms.Label();
            this.panelKart3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblToplamPoz = new System.Windows.Forms.Label();
            this.panelKart2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblKesilmisPoz = new System.Windows.Forms.Label();
            this.panelKart1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblKesilecekPoz = new System.Windows.Forms.Label();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelDisContainer.SuspendLayout();
            this.panelList.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartKesim)).BeginInit();
            this.panelKartContainer.SuspendLayout();
            this.panelKart4.SuspendLayout();
            this.panelKart3.SuspendLayout();
            this.panelKart2.SuspendLayout();
            this.panelKart1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 560F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelDisContainer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 50);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 942F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1677, 942);
            this.tableLayoutPanel1.TabIndex = 139;
            // 
            // panelDisContainer
            // 
            this.panelDisContainer.BackColor = System.Drawing.SystemColors.Control;
            this.panelDisContainer.Controls.Add(this.panelList);
            this.panelDisContainer.Controls.Add(this.panelSpacer1);
            this.panelDisContainer.Controls.Add(this.panelHeader);
            this.panelDisContainer.Controls.Add(this.panelSpacer2);
            this.panelDisContainer.Controls.Add(this.panelSearch);
            this.panelDisContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDisContainer.Location = new System.Drawing.Point(3, 3);
            this.panelDisContainer.Name = "panelDisContainer";
            this.panelDisContainer.Padding = new System.Windows.Forms.Padding(10);
            this.panelDisContainer.Size = new System.Drawing.Size(554, 936);
            this.panelDisContainer.TabIndex = 138;
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.lstPozlar);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(10, 95);
            this.panelList.Margin = new System.Windows.Forms.Padding(10);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(534, 831);
            this.panelList.TabIndex = 139;
            // 
            // lstPozlar
            // 
            this.lstPozlar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstPozlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPozlar.FormattingEnabled = true;
            this.lstPozlar.ItemHeight = 16;
            this.lstPozlar.Location = new System.Drawing.Point(0, 0);
            this.lstPozlar.Name = "lstPozlar";
            this.lstPozlar.Size = new System.Drawing.Size(534, 831);
            this.lstPozlar.TabIndex = 128;
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer1.Location = new System.Drawing.Point(10, 85);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(534, 10);
            this.panelSpacer1.TabIndex = 140;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panelHeader.Controls.Add(this.lblPozListesi);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(10, 52);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(5);
            this.panelHeader.Size = new System.Drawing.Size(534, 33);
            this.panelHeader.TabIndex = 138;
            // 
            // lblPozListesi
            // 
            this.lblPozListesi.AutoSize = true;
            this.lblPozListesi.BackColor = System.Drawing.Color.Transparent;
            this.lblPozListesi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblPozListesi.ForeColor = System.Drawing.Color.Orange;
            this.lblPozListesi.Location = new System.Drawing.Point(28, 5);
            this.lblPozListesi.Name = "lblPozListesi";
            this.lblPozListesi.Size = new System.Drawing.Size(92, 18);
            this.lblPozListesi.TabIndex = 137;
            this.lblPozListesi.Text = "Poz Listesi";
            // 
            // panelSpacer2
            // 
            this.panelSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpacer2.Location = new System.Drawing.Point(10, 42);
            this.panelSpacer2.Name = "panelSpacer2";
            this.panelSpacer2.Size = new System.Drawing.Size(534, 10);
            this.panelSpacer2.TabIndex = 139;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.txtArama);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(10, 10);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(534, 32);
            this.panelSearch.TabIndex = 137;
            // 
            // txtArama
            // 
            this.txtArama.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArama.Location = new System.Drawing.Point(0, 0);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(534, 15);
            this.txtArama.TabIndex = 136;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.panelChart);
            this.panel2.Controls.Add(this.panelSpacer3);
            this.panel2.Controls.Add(this.panelKartContainer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(563, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(1111, 936);
            this.panel2.TabIndex = 139;
            // 
            // panelChart
            // 
            this.panelChart.Controls.Add(this.chartKesim);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(10, 10);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(1091, 718);
            this.panelChart.TabIndex = 139;
            // 
            // chartKesim
            // 
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.Maximum = 5D;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.Name = "ChartArea1";
            this.chartKesim.ChartAreas.Add(chartArea1);
            this.chartKesim.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartKesim.Legends.Add(legend1);
            this.chartKesim.Location = new System.Drawing.Point(0, 0);
            this.chartKesim.Name = "chartKesim";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartKesim.Series.Add(series1);
            this.chartKesim.Size = new System.Drawing.Size(1091, 718);
            this.chartKesim.TabIndex = 137;
            this.chartKesim.TabStop = false;
            this.chartKesim.Text = "chart1";
            // 
            // panelSpacer3
            // 
            this.panelSpacer3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSpacer3.Location = new System.Drawing.Point(10, 728);
            this.panelSpacer3.Name = "panelSpacer3";
            this.panelSpacer3.Size = new System.Drawing.Size(1091, 10);
            this.panelSpacer3.TabIndex = 129;
            // 
            // panelKartContainer
            // 
            this.panelKartContainer.Controls.Add(this.lblBilgi);
            this.panelKartContainer.Controls.Add(this.panelKart4);
            this.panelKartContainer.Controls.Add(this.panelKart3);
            this.panelKartContainer.Controls.Add(this.panelKart2);
            this.panelKartContainer.Controls.Add(this.panelKart1);
            this.panelKartContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelKartContainer.Location = new System.Drawing.Point(10, 738);
            this.panelKartContainer.Name = "panelKartContainer";
            this.panelKartContainer.Size = new System.Drawing.Size(1091, 188);
            this.panelKartContainer.TabIndex = 138;
            this.panelKartContainer.Resize += new System.EventHandler(this.panel3_Resize);
            // 
            // lblBilgi
            // 
            this.lblBilgi.AutoSize = true;
            this.lblBilgi.ForeColor = System.Drawing.SystemColors.Control;
            this.lblBilgi.Location = new System.Drawing.Point(15, 172);
            this.lblBilgi.Name = "lblBilgi";
            this.lblBilgi.Size = new System.Drawing.Size(0, 16);
            this.lblBilgi.TabIndex = 5;
            // 
            // panelKart4
            // 
            this.panelKart4.BackColor = System.Drawing.Color.Orange;
            this.panelKart4.Controls.Add(this.label4);
            this.panelKart4.Controls.Add(this.lblToplamPozIfsKarsiligi);
            this.panelKart4.Location = new System.Drawing.Point(821, 38);
            this.panelKart4.Name = "panelKart4";
            this.panelKart4.Size = new System.Drawing.Size(236, 120);
            this.panelKart4.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(3, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "IFS’e göre toplam poz";
            // 
            // lblToplamPozIfsKarsiligi
            // 
            this.lblToplamPozIfsKarsiligi.AutoSize = true;
            this.lblToplamPozIfsKarsiligi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamPozIfsKarsiligi.ForeColor = System.Drawing.SystemColors.Control;
            this.lblToplamPozIfsKarsiligi.Location = new System.Drawing.Point(51, 51);
            this.lblToplamPozIfsKarsiligi.Name = "lblToplamPozIfsKarsiligi";
            this.lblToplamPozIfsKarsiligi.Size = new System.Drawing.Size(29, 20);
            this.lblToplamPozIfsKarsiligi.TabIndex = 2;
            this.lblToplamPozIfsKarsiligi.Text = "....";
            // 
            // panelKart3
            // 
            this.panelKart3.BackColor = System.Drawing.Color.Orange;
            this.panelKart3.Controls.Add(this.label3);
            this.panelKart3.Controls.Add(this.lblToplamPoz);
            this.panelKart3.Location = new System.Drawing.Point(567, 38);
            this.panelKart3.Name = "panelKart3";
            this.panelKart3.Size = new System.Drawing.Size(236, 120);
            this.panelKart3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(56, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Toplam Poz";
            // 
            // lblToplamPoz
            // 
            this.lblToplamPoz.AutoSize = true;
            this.lblToplamPoz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblToplamPoz.ForeColor = System.Drawing.SystemColors.Control;
            this.lblToplamPoz.Location = new System.Drawing.Point(56, 51);
            this.lblToplamPoz.Name = "lblToplamPoz";
            this.lblToplamPoz.Size = new System.Drawing.Size(29, 20);
            this.lblToplamPoz.TabIndex = 3;
            this.lblToplamPoz.Text = "....";
            // 
            // panelKart2
            // 
            this.panelKart2.BackColor = System.Drawing.Color.Orange;
            this.panelKart2.Controls.Add(this.label2);
            this.panelKart2.Controls.Add(this.lblKesilmisPoz);
            this.panelKart2.Location = new System.Drawing.Point(310, 38);
            this.panelKart2.Name = "panelKart2";
            this.panelKart2.Size = new System.Drawing.Size(236, 120);
            this.panelKart2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(51, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Kesilmis Poz";
            // 
            // lblKesilmisPoz
            // 
            this.lblKesilmisPoz.AutoSize = true;
            this.lblKesilmisPoz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKesilmisPoz.ForeColor = System.Drawing.SystemColors.Control;
            this.lblKesilmisPoz.Location = new System.Drawing.Point(51, 51);
            this.lblKesilmisPoz.Name = "lblKesilmisPoz";
            this.lblKesilmisPoz.Size = new System.Drawing.Size(29, 20);
            this.lblKesilmisPoz.TabIndex = 2;
            this.lblKesilmisPoz.Text = "....";
            // 
            // panelKart1
            // 
            this.panelKart1.BackColor = System.Drawing.Color.Orange;
            this.panelKart1.Controls.Add(this.label1);
            this.panelKart1.Controls.Add(this.lblKesilecekPoz);
            this.panelKart1.Location = new System.Drawing.Point(56, 38);
            this.panelKart1.Name = "panelKart1";
            this.panelKart1.Size = new System.Drawing.Size(236, 120);
            this.panelKart1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(48, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kesilecek Poz";
            // 
            // lblKesilecekPoz
            // 
            this.lblKesilecekPoz.AutoSize = true;
            this.lblKesilecekPoz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKesilecekPoz.ForeColor = System.Drawing.SystemColors.Control;
            this.lblKesilecekPoz.Location = new System.Drawing.Point(48, 51);
            this.lblKesilecekPoz.Name = "lblKesilecekPoz";
            this.lblKesilecekPoz.Size = new System.Drawing.Size(29, 20);
            this.lblKesilecekPoz.TabIndex = 0;
            this.lblKesilecekPoz.Text = "....";
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1677, 50);
            this.ctlBaslik1.TabIndex = 140;
            // 
            // ctlKesimDetaylari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ctlBaslik1);
            this.Name = "ctlKesimDetaylari";
            this.Size = new System.Drawing.Size(1677, 992);
            this.Load += new System.EventHandler(this.ctlKesimDetaylari_Load);
            this.Resize += new System.EventHandler(this.ctlKesimDetaylari_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelDisContainer.ResumeLayout(false);
            this.panelList.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartKesim)).EndInit();
            this.panelKartContainer.ResumeLayout(false);
            this.panelKartContainer.PerformLayout();
            this.panelKart4.ResumeLayout(false);
            this.panelKart4.PerformLayout();
            this.panelKart3.ResumeLayout(false);
            this.panelKart3.PerformLayout();
            this.panelKart2.ResumeLayout(false);
            this.panelKart2.PerformLayout();
            this.panelKart1.ResumeLayout(false);
            this.panelKart1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelDisContainer;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.ListBox lstPozlar;
        private System.Windows.Forms.Panel panelSpacer1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblPozListesi;
        private System.Windows.Forms.Panel panelSpacer2;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKesim;
        private System.Windows.Forms.Panel panelSpacer3;
        private System.Windows.Forms.Panel panelKartContainer;
        private System.Windows.Forms.Panel panelKart4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblToplamPozIfsKarsiligi;
        private System.Windows.Forms.Panel panelKart3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblToplamPoz;
        private System.Windows.Forms.Panel panelKart2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblKesilmisPoz;
        private System.Windows.Forms.Panel panelKart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblKesilecekPoz;
        private ctlBaslik ctlBaslik1;
        private System.Windows.Forms.Label lblBilgi;
    }
}