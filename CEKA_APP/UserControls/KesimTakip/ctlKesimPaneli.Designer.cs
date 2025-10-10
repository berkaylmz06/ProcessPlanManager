namespace CEKA_APP.UserControls.KesimTakip
{
    partial class ctlKesimPaneli
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
            this.btnKesimBaslat = new System.Windows.Forms.Button();
            this.btnKesimDurdur = new System.Windows.Forms.Button();
            this.btnKesimBitir = new System.Windows.Forms.Button();
            this.btnKesimPlaniSec = new System.Windows.Forms.Button();
            this.txtKesimPlaniNo = new System.Windows.Forms.TextBox();
            this.lblLotNo = new System.Windows.Forms.Label();
            this.txtLotNo = new System.Windows.Forms.TextBox();
            this.lblKesimEmriNo = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.lblOperatorAd = new System.Windows.Forms.Label();
            this.txtOperatorAd = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKesimBaslat
            // 
            this.btnKesimBaslat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKesimBaslat.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnKesimBaslat.FlatAppearance.BorderSize = 0;
            this.btnKesimBaslat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKesimBaslat.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKesimBaslat.ForeColor = System.Drawing.Color.White;
            this.btnKesimBaslat.Location = new System.Drawing.Point(85, 250);
            this.btnKesimBaslat.Name = "btnKesimBaslat";
            this.btnKesimBaslat.Size = new System.Drawing.Size(248, 202);
            this.btnKesimBaslat.TabIndex = 0;
            this.btnKesimBaslat.Text = "KESİM\r\nBAŞLAT";
            this.btnKesimBaslat.UseVisualStyleBackColor = false;
            this.btnKesimBaslat.Click += new System.EventHandler(this.btnKesimBaslat_Click);
            // 
            // btnKesimDurdur
            // 
            this.btnKesimDurdur.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnKesimDurdur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnKesimDurdur.FlatAppearance.BorderSize = 0;
            this.btnKesimDurdur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKesimDurdur.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKesimDurdur.ForeColor = System.Drawing.Color.White;
            this.btnKesimDurdur.Location = new System.Drawing.Point(409, 250);
            this.btnKesimDurdur.Name = "btnKesimDurdur";
            this.btnKesimDurdur.Size = new System.Drawing.Size(248, 202);
            this.btnKesimDurdur.TabIndex = 1;
            this.btnKesimDurdur.Text = "KESİM\r\nDURDUR";
            this.btnKesimDurdur.UseVisualStyleBackColor = false;
            this.btnKesimDurdur.Click += new System.EventHandler(this.btnKesimDurdur_Click);
            // 
            // btnKesimBitir
            // 
            this.btnKesimBitir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKesimBitir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnKesimBitir.FlatAppearance.BorderSize = 0;
            this.btnKesimBitir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKesimBitir.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKesimBitir.ForeColor = System.Drawing.Color.White;
            this.btnKesimBitir.Location = new System.Drawing.Point(723, 250);
            this.btnKesimBitir.Name = "btnKesimBitir";
            this.btnKesimBitir.Size = new System.Drawing.Size(248, 202);
            this.btnKesimBitir.TabIndex = 2;
            this.btnKesimBitir.Text = "KESİM\r\nBİTİR";
            this.btnKesimBitir.UseVisualStyleBackColor = false;
            this.btnKesimBitir.Click += new System.EventHandler(this.btnKesimBitir_Click);
            // 
            // btnKesimPlaniSec
            // 
            this.btnKesimPlaniSec.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKesimPlaniSec.Location = new System.Drawing.Point(12, 34);
            this.btnKesimPlaniSec.Name = "btnKesimPlaniSec";
            this.btnKesimPlaniSec.Size = new System.Drawing.Size(158, 56);
            this.btnKesimPlaniSec.TabIndex = 3;
            this.btnKesimPlaniSec.Text = "Kesim Planı Seç";
            this.btnKesimPlaniSec.UseVisualStyleBackColor = true;
            this.btnKesimPlaniSec.Click += new System.EventHandler(this.btnKesimPlaniSec_Click);
            // 
            // txtKesimPlaniNo
            // 
            this.txtKesimPlaniNo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtKesimPlaniNo.Location = new System.Drawing.Point(349, 34);
            this.txtKesimPlaniNo.Name = "txtKesimPlaniNo";
            this.txtKesimPlaniNo.ReadOnly = true;
            this.txtKesimPlaniNo.Size = new System.Drawing.Size(253, 26);
            this.txtKesimPlaniNo.TabIndex = 4;
            // 
            // lblLotNo
            // 
            this.lblLotNo.AutoSize = true;
            this.lblLotNo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLotNo.Location = new System.Drawing.Point(206, 72);
            this.lblLotNo.Name = "lblLotNo";
            this.lblLotNo.Size = new System.Drawing.Size(94, 19);
            this.lblLotNo.TabIndex = 5;
            this.lblLotNo.Text = "Lot Numarası:";
            // 
            // txtLotNo
            // 
            this.txtLotNo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtLotNo.Location = new System.Drawing.Point(349, 69);
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Size = new System.Drawing.Size(253, 26);
            this.txtLotNo.TabIndex = 6;
            // 
            // lblKesimEmriNo
            // 
            this.lblKesimEmriNo.AutoSize = true;
            this.lblKesimEmriNo.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKesimEmriNo.Location = new System.Drawing.Point(206, 37);
            this.lblKesimEmriNo.Name = "lblKesimEmriNo";
            this.lblKesimEmriNo.Size = new System.Drawing.Size(103, 19);
            this.lblKesimEmriNo.TabIndex = 7;
            this.lblKesimEmriNo.Text = "Kesim Planı No:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblElapsedTime);
            this.groupBox1.Controls.Add(this.lblOperatorAd);
            this.groupBox1.Controls.Add(this.txtOperatorAd);
            this.groupBox1.Controls.Add(this.lblKesimEmriNo);
            this.groupBox1.Controls.Add(this.txtLotNo);
            this.groupBox1.Controls.Add(this.btnKesimPlaniSec);
            this.groupBox1.Controls.Add(this.lblLotNo);
            this.groupBox1.Controls.Add(this.txtKesimPlaniNo);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(180, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 155);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Aktif Kesim Bilgileri";
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblElapsedTime.Location = new System.Drawing.Point(608, 109);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(0, 19);
            this.lblElapsedTime.TabIndex = 10;
            // 
            // lblOperatorAd
            // 
            this.lblOperatorAd.AutoSize = true;
            this.lblOperatorAd.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblOperatorAd.Location = new System.Drawing.Point(206, 107);
            this.lblOperatorAd.Name = "lblOperatorAd";
            this.lblOperatorAd.Size = new System.Drawing.Size(68, 19);
            this.lblOperatorAd.TabIndex = 9;
            this.lblOperatorAd.Text = "Operatör:";
            // 
            // txtOperatorAd
            // 
            this.txtOperatorAd.BackColor = System.Drawing.Color.PaleTurquoise;
            this.txtOperatorAd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtOperatorAd.ForeColor = System.Drawing.Color.DarkBlue;
            this.txtOperatorAd.Location = new System.Drawing.Point(349, 104);
            this.txtOperatorAd.Name = "txtOperatorAd";
            this.txtOperatorAd.ReadOnly = true;
            this.txtOperatorAd.Size = new System.Drawing.Size(253, 29);
            this.txtOperatorAd.TabIndex = 10;
            // 
            // ctlKesimPaneli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnKesimBitir);
            this.Controls.Add(this.btnKesimDurdur);
            this.Controls.Add(this.btnKesimBaslat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Name = "ctlKesimPaneli";
            this.Size = new System.Drawing.Size(1072, 551);
            this.Resize += new System.EventHandler(this.ctlKesimPaneli_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnKesimBaslat;
        private System.Windows.Forms.Button btnKesimDurdur;
        private System.Windows.Forms.Button btnKesimBitir;
        private System.Windows.Forms.Button btnKesimPlaniSec;
        private System.Windows.Forms.TextBox txtKesimPlaniNo;
        private System.Windows.Forms.Label lblLotNo;
        private System.Windows.Forms.TextBox txtLotNo;
        private System.Windows.Forms.Label lblKesimEmriNo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblOperatorAd;
        private System.Windows.Forms.TextBox txtOperatorAd;
        private System.Windows.Forms.Label lblElapsedTime;
    }
}