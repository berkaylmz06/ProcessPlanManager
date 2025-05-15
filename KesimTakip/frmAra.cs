using KesimTakip.DataBase;
using KesimTakip.UsrControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KesimTakip
{
    public partial class frmAra : Form
    {
        public frmAra(DataGridViewColumnCollection columns)
        {
            InitializeComponent();
            AraFormDinamikLabel(columns);
        }

        public frmAra()
        {
        }

        private Dictionary<string, TextBox> filtreKutulari = new Dictionary<string, TextBox>();

        private void AraFormDinamikLabel(DataGridViewColumnCollection columns)
        {
            int yOffset = 100;
            int xOffset = 40;
            int textBoxWidth = 150;
            int labelWidth = 100;

            for (int i = 0; i < columns.Count - 1; i++)  
            {
                DataGridViewColumn column = columns[i];

                Label label = new Label();
                label.Text = column.HeaderText + ":";
                label.AutoSize = true;
                label.Location = new Point(xOffset, yOffset);
                this.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = column.HeaderText;
                textBox.Location = new Point(xOffset + labelWidth + 40, yOffset);
                textBox.Width = textBoxWidth;
                this.Controls.Add(textBox);

                filtreKutulari[column.HeaderText] = textBox;

                yOffset += Math.Max(label.Height, textBox.Height) + 10;
            }
        }


        private void btnAra_Click(object sender, EventArgs e)
        {
            DataTable sonucTablo = KesimListesiPaketData.KesimListesiniPaketFiltrele(filtreKutulari);

            if (!sonucTablo.Columns.Contains("Detay"))
            {
                sonucTablo.Columns.Add("Detay", typeof(string));
            }

            foreach (DataRow row in sonucTablo.Rows)
            {
                row["Detay"] = "Detay Görmek İçin Tıklayınız.";
            }

            var ctl = Application.OpenForms["frmAnaSayfa"]
                            ?.Controls.Find("panelAnaSayfaContainer", true)
                            .FirstOrDefault()
                            ?.Controls
                            .OfType<ctlKesimYap>()
                            .FirstOrDefault();

            if (ctl != null)
            {
                ctl.dataGridKesimListesi.DataSource = sonucTablo;

                if (ctl.dataGridKesimListesi.Columns.Contains("id"))
                {
                    ctl.dataGridKesimListesi.Columns["id"].Visible = false;
                }

                ctl.tabloDuzenle();
            }

            this.Close();
        }
    }
}

