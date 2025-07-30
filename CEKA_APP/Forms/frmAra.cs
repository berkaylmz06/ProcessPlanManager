using CEKA_APP.DataBase;
using CEKA_APP.UsrControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmAra : Form
    {
        private Func<Dictionary<string, TextBox>, DataTable> filtrelemeFonksiyonu;
        private Action<DataTable> aramaSonucuCallback;
        private readonly bool detayEklenecekMi;
        private readonly Dictionary<string, string> sonFiltreKriterleri; 
        private Dictionary<string, TextBox> filtreKutulari = new Dictionary<string, TextBox>();

        public frmAra(DataGridViewColumnCollection columns, Func<Dictionary<string, TextBox>, DataTable> filtreFonksiyonu,
            Action<DataTable> callback, bool detayEkle = false, Dictionary<string, string> sonKriterler = null)
        {
            InitializeComponent();
            this.filtrelemeFonksiyonu = filtreFonksiyonu;
            this.aramaSonucuCallback = callback;
            this.detayEklenecekMi = detayEkle;
            this.sonFiltreKriterleri = sonKriterler;
            AraFormDinamikLabel(columns);

            this.Icon = Properties.Resources.cekalogokirmizi;
        }

        public frmAra()
        {
        }

        private void AraFormDinamikLabel(DataGridViewColumnCollection columns)
        {
            int yOffset = 100;
            int xOffset = 40;
            int textBoxWidth = 150;
            int labelWidth = 100;

            for (int i = 0; i < columns.Count; i++)
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

                if (sonFiltreKriterleri != null && sonFiltreKriterleri.ContainsKey(column.HeaderText))
                {
                    textBox.Text = sonFiltreKriterleri[column.HeaderText];
                }

                this.Controls.Add(textBox);
                filtreKutulari[column.HeaderText] = textBox;

                yOffset += Math.Max(label.Height, textBox.Height) + 10;
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            DataTable sonucTablo = filtrelemeFonksiyonu?.Invoke(filtreKutulari);

            if (sonucTablo == null)
                return;

            if (detayEklenecekMi && !sonucTablo.Columns.Contains("Detay"))
            {
                sonucTablo.Columns.Add("Detay", typeof(string));
                foreach (DataRow row in sonucTablo.Rows)
                    row["Detay"] = "Detay Görmek İçin Tıklayınız.";
            }

            aramaSonucuCallback?.Invoke(sonucTablo);
            this.Close();
        }
    }
}