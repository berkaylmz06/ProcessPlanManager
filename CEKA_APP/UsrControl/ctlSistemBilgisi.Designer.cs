namespace CEKA_APP.UsrControl
{
    partial class ctlSistemBilgisi
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
            this.richPdfCiktisi = new System.Windows.Forms.RichTextBox();
            this.richIslenmisVeri = new System.Windows.Forms.RichTextBox();
            this.richValidData = new System.Windows.Forms.RichTextBox();
            this.richInvalidData = new System.Windows.Forms.RichTextBox();
            this.ctlBaslik1 = new CEKA_APP.UsrControl.ctlBaslik();
            this.SuspendLayout();
            // 
            // richPdfCiktisi
            // 
            this.richPdfCiktisi.Location = new System.Drawing.Point(62, 85);
            this.richPdfCiktisi.Name = "richPdfCiktisi";
            this.richPdfCiktisi.Size = new System.Drawing.Size(631, 363);
            this.richPdfCiktisi.TabIndex = 0;
            this.richPdfCiktisi.Text = "";
            // 
            // richIslenmisVeri
            // 
            this.richIslenmisVeri.Location = new System.Drawing.Point(745, 85);
            this.richIslenmisVeri.Name = "richIslenmisVeri";
            this.richIslenmisVeri.Size = new System.Drawing.Size(631, 363);
            this.richIslenmisVeri.TabIndex = 1;
            this.richIslenmisVeri.Text = "";
            // 
            // richValidData
            // 
            this.richValidData.Location = new System.Drawing.Point(62, 535);
            this.richValidData.Name = "richValidData";
            this.richValidData.Size = new System.Drawing.Size(631, 363);
            this.richValidData.TabIndex = 2;
            this.richValidData.Text = "";
            // 
            // richInvalidData
            // 
            this.richInvalidData.Location = new System.Drawing.Point(745, 535);
            this.richInvalidData.Name = "richInvalidData";
            this.richInvalidData.Size = new System.Drawing.Size(631, 363);
            this.richInvalidData.TabIndex = 3;
            this.richInvalidData.Text = "";
            // 
            // ctlBaslik1
            // 
            this.ctlBaslik1.Baslik = "Başlık";
            this.ctlBaslik1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlBaslik1.Location = new System.Drawing.Point(0, 0);
            this.ctlBaslik1.Name = "ctlBaslik1";
            this.ctlBaslik1.Size = new System.Drawing.Size(1469, 50);
            this.ctlBaslik1.TabIndex = 4;
            // 
            // ctlSistemBilgisi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlBaslik1);
            this.Controls.Add(this.richInvalidData);
            this.Controls.Add(this.richValidData);
            this.Controls.Add(this.richIslenmisVeri);
            this.Controls.Add(this.richPdfCiktisi);
            this.Name = "ctlSistemBilgisi";
            this.Size = new System.Drawing.Size(1469, 940);
            this.Load += new System.EventHandler(this.ctlSistemBilgisi_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richIslenmisVeri;
        private System.Windows.Forms.RichTextBox richValidData;
        private System.Windows.Forms.RichTextBox richInvalidData;
        public System.Windows.Forms.RichTextBox richPdfCiktisi;
        private ctlBaslik ctlBaslik1;
    }
}
