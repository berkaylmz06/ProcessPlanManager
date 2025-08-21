namespace CEKA_APP.UsrControl.ProjeFinans
{
    partial class ctlTakipTakvimi
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
            this.tlpTakvim = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTarih = new System.Windows.Forms.Label();
            this.btnSonrakiAy = new System.Windows.Forms.Button();
            this.btnOncekiAy = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTakvim
            // 
            this.tlpTakvim.ColumnCount = 7;
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpTakvim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTakvim.Location = new System.Drawing.Point(0, 50);
            this.tlpTakvim.Name = "tlpTakvim";
            this.tlpTakvim.RowCount = 6;
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpTakvim.Size = new System.Drawing.Size(1598, 932);
            this.tlpTakvim.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.panel1);
            this.panelHeader.Controls.Add(this.panelRight);
            this.panelHeader.Controls.Add(this.panelLeft);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1598, 50);
            this.panelHeader.TabIndex = 1;
            // 
            // lblTarih
            // 
            this.lblTarih.AutoSize = true;
            this.lblTarih.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblTarih.Location = new System.Drawing.Point(648, 5);
            this.lblTarih.Name = "lblTarih";
            this.lblTarih.Size = new System.Drawing.Size(96, 33);
            this.lblTarih.TabIndex = 2;
            this.lblTarih.Text = "Ay, Yıl";
            this.lblTarih.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSonrakiAy
            // 
            this.btnSonrakiAy.Location = new System.Drawing.Point(21, 10);
            this.btnSonrakiAy.Name = "btnSonrakiAy";
            this.btnSonrakiAy.Size = new System.Drawing.Size(75, 30);
            this.btnSonrakiAy.TabIndex = 1;
            this.btnSonrakiAy.Text = ">";
            this.btnSonrakiAy.UseVisualStyleBackColor = true;
            this.btnSonrakiAy.Click += new System.EventHandler(this.btnSonrakiAy_Click);
            // 
            // btnOncekiAy
            // 
            this.btnOncekiAy.Location = new System.Drawing.Point(10, 10);
            this.btnOncekiAy.Name = "btnOncekiAy";
            this.btnOncekiAy.Size = new System.Drawing.Size(75, 30);
            this.btnOncekiAy.TabIndex = 0;
            this.btnOncekiAy.Text = "<";
            this.btnOncekiAy.UseVisualStyleBackColor = true;
            this.btnOncekiAy.Click += new System.EventHandler(this.btnOncekiAy_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.btnOncekiAy);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(96, 50);
            this.panelLeft.TabIndex = 3;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.btnSonrakiAy);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1488, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(110, 50);
            this.panelRight.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTarih);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(96, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1392, 50);
            this.panel1.TabIndex = 4;
            // 
            // ctlTakipTakvimi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpTakvim);
            this.Controls.Add(this.panelHeader);
            this.Name = "ctlTakipTakvimi";
            this.Size = new System.Drawing.Size(1598, 982);
            this.Load += new System.EventHandler(this.ctlTakipTakvimi_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTakvim;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnSonrakiAy;
        private System.Windows.Forms.Button btnOncekiAy;
        private System.Windows.Forms.Label lblTarih;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
    }
}