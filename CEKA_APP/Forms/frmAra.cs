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
        private Dictionary<string, Control> filtreKutulari = new Dictionary<string, Control>();

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
            panelFiltreler.Controls.Clear();
            panelFiltreler.AutoScroll = true;

            int yOffset = 10;
            int xOffset = 10;
            int controlWidth = 150;
            int labelWidth = 150;

            var sortedColumns = columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            foreach (var column in sortedColumns)
            {
                if (column.HeaderText.Contains("Tarih"))
                {
                    Label labelBaslangic = new Label();
                    labelBaslangic.Text = column.HeaderText + " (Başlangıç):";
                    labelBaslangic.AutoSize = true;
                    labelBaslangic.Location = new Point(xOffset, yOffset + 5);
                    panelFiltreler.Controls.Add(labelBaslangic);

                    DateTimePicker dtpBaslangic = new DateTimePicker();
                    dtpBaslangic.Format = DateTimePickerFormat.Custom;
                    dtpBaslangic.CustomFormat = "dd.MM.yyyy";
                    dtpBaslangic.ShowCheckBox = true;
                    dtpBaslangic.Checked = false;
                    dtpBaslangic.Name = column.HeaderText + "_Baslangic";
                    dtpBaslangic.Location = new Point(xOffset + labelWidth, yOffset);
                    dtpBaslangic.Width = controlWidth;
                    panelFiltreler.Controls.Add(dtpBaslangic);
                    filtreKutulari[dtpBaslangic.Name] = dtpBaslangic;

                    if (sonFiltreKriterleri != null && sonFiltreKriterleri.ContainsKey(dtpBaslangic.Name))
                    {
                        if (DateTime.TryParse(sonFiltreKriterleri[dtpBaslangic.Name], out DateTime val))
                        {
                            dtpBaslangic.Value = val;
                            dtpBaslangic.Checked = true;
                        }
                    }

                    yOffset += Math.Max(labelBaslangic.Height, dtpBaslangic.Height) + 10;

                    Label labelBitis = new Label();
                    labelBitis.Text = column.HeaderText + " (Bitiş):";
                    labelBitis.AutoSize = true;
                    labelBitis.Location = new Point(xOffset, yOffset + 5);
                    panelFiltreler.Controls.Add(labelBitis);

                    DateTimePicker dtpBitis = new DateTimePicker();
                    dtpBitis.Format = DateTimePickerFormat.Custom;
                    dtpBitis.CustomFormat = "dd.MM.yyyy";
                    dtpBitis.ShowCheckBox = true;
                    dtpBitis.Checked = false;
                    dtpBitis.Name = column.HeaderText + "_Bitis";
                    dtpBitis.Location = new Point(xOffset + labelWidth, yOffset);
                    dtpBitis.Width = controlWidth;
                    panelFiltreler.Controls.Add(dtpBitis);
                    filtreKutulari[dtpBitis.Name] = dtpBitis;

                    if (sonFiltreKriterleri != null && sonFiltreKriterleri.ContainsKey(dtpBitis.Name))
                    {
                        if (DateTime.TryParse(sonFiltreKriterleri[dtpBitis.Name], out DateTime val))
                        {
                            dtpBitis.Value = val;
                            dtpBitis.Checked = true;
                        }
                    }

                    yOffset += Math.Max(labelBitis.Height, dtpBitis.Height) + 10;
                }
                else
                {
                    Label label = new Label();
                    label.Text = column.HeaderText + ":";
                    label.AutoSize = true;
                    label.Location = new Point(xOffset, yOffset + 5);
                    panelFiltreler.Controls.Add(label);

                    TextBox textBox = new TextBox();
                    textBox.Name = column.HeaderText;
                    textBox.Location = new Point(xOffset + labelWidth, yOffset);
                    textBox.Width = controlWidth;
                    textBox.KeyDown += new KeyEventHandler(this.btnAra_KeyDown);

                    if (sonFiltreKriterleri != null && sonFiltreKriterleri.ContainsKey(column.HeaderText))
                    {
                        textBox.Text = sonFiltreKriterleri[column.HeaderText];
                    }

                    panelFiltreler.Controls.Add(textBox);
                    filtreKutulari[column.HeaderText] = textBox;

                    yOffset += Math.Max(label.Height, textBox.Height) + 10;
                }
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            var geciciFiltreKutulari = new Dictionary<string, TextBox>();
            foreach (var item in filtreKutulari)
            {
                if (item.Value is TextBox textBox)
                {
                    geciciFiltreKutulari[item.Key] = textBox;
                }
                else if (item.Value is DateTimePicker dtp)
                {
                    var tempTextBox = new TextBox();
                    if (dtp.Checked)
                    {
                        tempTextBox.Text = dtp.Value.ToShortDateString();
                    }
                    geciciFiltreKutulari[item.Key] = tempTextBox;
                }
            }

            DataTable sonucTablo = filtrelemeFonksiyonu?.Invoke(geciciFiltreKutulari);

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

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            foreach (var control in filtreKutulari.Values)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
                else if (control is DateTimePicker dtp)
                {
                    dtp.Checked = false;
                }
            }
        }

        private void btnAra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAra_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}