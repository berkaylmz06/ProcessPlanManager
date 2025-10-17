namespace CEKA_APP.Forms.KesimTakip
{
    partial class frmKesimIptal
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvKesimAdetleri = new System.Windows.Forms.DataGridView();
            this.btnTamam = new System.Windows.Forms.Button();
            this.btnIptal = new System.Windows.Forms.Button();
            this.comboBoxIptalNedeniSec = new System.Windows.Forms.ComboBox();
            this.lblIptalNedeniSec = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKesimAdetleri)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvKesimAdetleri
            // 
            this.dgvKesimAdetleri.AllowUserToAddRows = false;
            this.dgvKesimAdetleri.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKesimAdetleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKesimAdetleri.ColumnHeadersHeight = 29;
            this.dgvKesimAdetleri.Location = new System.Drawing.Point(17, 152);
            this.dgvKesimAdetleri.Name = "dgvKesimAdetleri";
            this.dgvKesimAdetleri.RowHeadersWidth = 51;
            this.dgvKesimAdetleri.Size = new System.Drawing.Size(694, 388);
            this.dgvKesimAdetleri.TabIndex = 3;
            // 
            // btnTamam
            // 
            this.btnTamam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTamam.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTamam.Location = new System.Drawing.Point(501, 563);
            this.btnTamam.Margin = new System.Windows.Forms.Padding(4);
            this.btnTamam.Name = "btnTamam";
            this.btnTamam.Size = new System.Drawing.Size(100, 35);
            this.btnTamam.TabIndex = 5;
            this.btnTamam.Text = "Tamam";
            this.btnTamam.UseVisualStyleBackColor = true;
            this.btnTamam.Click += new System.EventHandler(this.btnTamam_Click);
            // 
            // btnIptal
            // 
            this.btnIptal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIptal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnIptal.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIptal.Location = new System.Drawing.Point(611, 563);
            this.btnIptal.Margin = new System.Windows.Forms.Padding(4);
            this.btnIptal.Name = "btnIptal";
            this.btnIptal.Size = new System.Drawing.Size(100, 35);
            this.btnIptal.TabIndex = 6;
            this.btnIptal.Text = "İptal";
            this.btnIptal.UseVisualStyleBackColor = true;
            this.btnIptal.Click += new System.EventHandler(this.btnIptal_Click);
            // 
            // comboBoxIptalNedeniSec
            // 
            this.comboBoxIptalNedeniSec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxIptalNedeniSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIptalNedeniSec.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboBoxIptalNedeniSec.FormattingEnabled = true;
            this.comboBoxIptalNedeniSec.Location = new System.Drawing.Point(113, 94);
            this.comboBoxIptalNedeniSec.Name = "comboBoxIptalNedeniSec";
            this.comboBoxIptalNedeniSec.Size = new System.Drawing.Size(598, 28);
            this.comboBoxIptalNedeniSec.TabIndex = 9;
            // 
            // lblIptalNedeniSec
            // 
            this.lblIptalNedeniSec.AutoSize = true;
            this.lblIptalNedeniSec.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblIptalNedeniSec.Location = new System.Drawing.Point(13, 97);
            this.lblIptalNedeniSec.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIptalNedeniSec.Name = "lblIptalNedeniSec";
            this.lblIptalNedeniSec.Size = new System.Drawing.Size(94, 20);
            this.lblIptalNedeniSec.TabIndex = 10;
            this.lblIptalNedeniSec.Text = "İptal Nedeni:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 80);
            this.panel1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(65, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kesim İptal";
            // 
            // frmKesimIptal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblIptalNedeniSec);
            this.Controls.Add(this.comboBoxIptalNedeniSec);
            this.Controls.Add(this.btnTamam);
            this.Controls.Add(this.btnIptal);
            this.Controls.Add(this.dgvKesimAdetleri);
            this.Name = "frmKesimIptal";
            this.Text = "Kesim İptal";
            ((System.ComponentModel.ISupportInitialize)(this.dgvKesimAdetleri)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvKesimAdetleri;
        private System.Windows.Forms.Button btnTamam;
        private System.Windows.Forms.Button btnIptal;
        private System.Windows.Forms.ComboBox comboBoxIptalNedeniSec;
        private System.Windows.Forms.Label lblIptalNedeniSec;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}