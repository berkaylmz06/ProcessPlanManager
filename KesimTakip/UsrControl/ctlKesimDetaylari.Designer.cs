namespace KesimTakip.UsrControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lstPozlar = new System.Windows.Forms.ListBox();
            this.txtArama = new System.Windows.Forms.TextBox();
            this.chartKesim = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chartKesim)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstPozlar
            // 
            this.lstPozlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPozlar.FormattingEnabled = true;
            this.lstPozlar.ItemHeight = 16;
            this.lstPozlar.Location = new System.Drawing.Point(0, 22);
            this.lstPozlar.Name = "lstPozlar";
            this.lstPozlar.Size = new System.Drawing.Size(229, 947);
            this.lstPozlar.TabIndex = 128;
            // 
            // txtArama
            // 
            this.txtArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtArama.Location = new System.Drawing.Point(0, 0);
            this.txtArama.Name = "txtArama";
            this.txtArama.Size = new System.Drawing.Size(229, 22);
            this.txtArama.TabIndex = 136;
            // 
            // chartKesim
            // 
            chartArea4.Name = "ChartArea1";
            this.chartKesim.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartKesim.Legends.Add(legend4);
            this.chartKesim.Location = new System.Drawing.Point(89, 46);
            this.chartKesim.Name = "chartKesim";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartKesim.Series.Add(series4);
            this.chartKesim.Size = new System.Drawing.Size(963, 555);
            this.chartKesim.TabIndex = 137;
            this.chartKesim.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstPozlar);
            this.panel1.Controls.Add(this.txtArama);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 969);
            this.panel1.TabIndex = 138;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.chartKesim);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(229, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1433, 969);
            this.panel2.TabIndex = 139;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 776);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1433, 193);
            this.panel3.TabIndex = 138;
            // 
            // ctlKesimDetaylari
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ctlKesimDetaylari";
            this.Size = new System.Drawing.Size(1662, 969);
            ((System.ComponentModel.ISupportInitialize)(this.chartKesim)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lstPozlar;
        private System.Windows.Forms.TextBox txtArama;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKesim;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}
