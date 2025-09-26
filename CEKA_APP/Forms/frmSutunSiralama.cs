using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmSutunSiralama : Form
    {
        private List<string> sutunSirasi;

        public frmSutunSiralama(List<string> mevcutSutunSirasi)
        {
            InitializeComponent();
            sutunSirasi = new List<string>(mevcutSutunSirasi);
            lstSutunlar.Items.AddRange(sutunSirasi.ToArray());

            this.Icon = Properties.Resources.cekalogokirmizi;
        }
        private void frmSutunSiralama_Load(object sender, EventArgs e)
        {
            btnYukariTasi.Image = new Bitmap(Properties.Resources.yukariOk, new Size(24, 24));
            btnAsagiTasi.Image = new Bitmap(Properties.Resources.asagiOk, new Size(24, 24));
            btnEnUsteTasi.Image = new Bitmap(Properties.Resources.enYukariOk, new Size(24, 24));
            btnEnAltaTasi.Image = new Bitmap(Properties.Resources.enAsagiOk, new Size(24, 24));

            btnYukariTasi.ImageAlign = ContentAlignment.MiddleCenter;
            btnAsagiTasi.ImageAlign = ContentAlignment.MiddleCenter;
            btnEnUsteTasi.ImageAlign = ContentAlignment.MiddleCenter;
            btnEnAltaTasi.ImageAlign = ContentAlignment.MiddleCenter;
        }
        public List<string> SutunSirasiniAl()
        {
            return sutunSirasi;
        }

        private void btnYukariTasi_Click(object sender, EventArgs e)
        {
            if (lstSutunlar.SelectedIndex > 0)
            {
                int indeks = lstSutunlar.SelectedIndex;
                string ogeler = sutunSirasi[indeks];
                sutunSirasi.RemoveAt(indeks);
                sutunSirasi.Insert(indeks - 1, ogeler);
                lstSutunlar.Items.Clear();
                lstSutunlar.Items.AddRange(sutunSirasi.ToArray());
                lstSutunlar.SelectedIndex = indeks - 1;
            }
        }

        private void btnAsagiTasi_Click(object sender, EventArgs e)
        {
            if (lstSutunlar.SelectedIndex < lstSutunlar.Items.Count - 1 && lstSutunlar.SelectedIndex >= 0)
            {
                int indeks = lstSutunlar.SelectedIndex;
                string ogeler = sutunSirasi[indeks];
                sutunSirasi.RemoveAt(indeks);
                sutunSirasi.Insert(indeks + 1, ogeler);
                lstSutunlar.Items.Clear();
                lstSutunlar.Items.AddRange(sutunSirasi.ToArray());
                lstSutunlar.SelectedIndex = indeks + 1;
            }
        }

        private void btnEnUsteTasi_Click(object sender, EventArgs e)
        {
            if (lstSutunlar.SelectedIndex >= 0)
            {
                int indeks = lstSutunlar.SelectedIndex;
                string ogeler = sutunSirasi[indeks];
                sutunSirasi.RemoveAt(indeks);
                sutunSirasi.Insert(0, ogeler);
                lstSutunlar.Items.Clear();
                lstSutunlar.Items.AddRange(sutunSirasi.ToArray());
                lstSutunlar.SelectedIndex = 0;
            }
        }

        private void btnEnAltaTasi_Click(object sender, EventArgs e)
        {
            if (lstSutunlar.SelectedIndex >= 0)
            {
                int indeks = lstSutunlar.SelectedIndex;
                string ogeler = sutunSirasi[indeks];
                sutunSirasi.RemoveAt(indeks);
                sutunSirasi.Add(ogeler);
                lstSutunlar.Items.Clear();
                lstSutunlar.Items.AddRange(sutunSirasi.ToArray());
                lstSutunlar.SelectedIndex = sutunSirasi.Count - 1;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}