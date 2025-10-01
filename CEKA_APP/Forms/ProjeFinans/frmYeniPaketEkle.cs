using CEKA_APP.Abstracts.ProjeFinans;
using CEKA_APP.Concretes.ProjeFinans;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmYeniPaketEkle : Form
    {
        public string PaketAdi { get; private set; }

        private const int VerticalMargin = 12;

        private ISevkiyatPaketleriService _sevkiyatPaketleriService;
        public frmYeniPaketEkle(ISevkiyatPaketleriService sevkiyatPaketleriService)
        {
            InitializeComponent();

            _sevkiyatPaketleriService = sevkiyatPaketleriService ?? throw new ArgumentNullException(nameof(sevkiyatPaketleriService));

            LoadPaketler();

            panelYeniPaket.Visible = false;

            SetFormSize(false);

            this.Icon = Properties.Resources.cekalogokirmizi;

            txtPaketAdi.TextChanged += txtPaketAdi_TextChanged;
        }

        private void LoadPaketler()
        {
            var paketler = _sevkiyatPaketleriService.GetPaketler();
            listPaketler.Items.Clear();
            foreach (var (Id, Adi, Tarih) in paketler)
            {
                listPaketler.Items.Add(Adi);
            }
            listPaketler.SelectedIndex = -1;
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listPaketler.SelectedIndex >= 0)
            {
                PaketAdi = listPaketler.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir paket seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (btnEkle.Text == "Yeni Ekle")
            {
                panelYeniPaket.Visible = true;
                txtPaketAdi.Clear();

                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Red;

                SetFormSize(true);
            }
            else if (btnEkle.Text == "İptal")
            {
                GeriDon();
            }
            else if (btnEkle.Text == "Onayla")
            {
                if (!string.IsNullOrEmpty(txtPaketAdi.Text))
                {
                    PaketAdi = txtPaketAdi.Text;
                    _sevkiyatPaketleriService.PaketEkle(PaketAdi);
                    LoadPaketler();

                    GeriDon();

                    MessageBox.Show("Yeni paket başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lütfen yeni paket adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtPaketAdi_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPaketAdi.Text))
            {
                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Red;
            }
            else
            {
                btnEkle.Text = "Onayla";
                btnEkle.BackColor = Color.FromArgb(46, 204, 113); // Onay için yeşil
            }
        }

        private void SetFormSize(bool expanded)
        {
            if (expanded)
            {
                this.ClientSize = new Size(this.ClientSize.Width, panelYeniPaket.Bottom + VerticalMargin);
            }
            else
            {
                this.ClientSize = new Size(this.ClientSize.Width, btnEkle.Bottom + VerticalMargin);
            }
        }

        private void GeriDon()
        {
            panelYeniPaket.Visible = false;
            btnEkle.Text = "Yeni Ekle";
            btnEkle.BackColor = Color.Gray;

            SetFormSize(false);
        }

        private void frmYeniPaketEkle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None && string.IsNullOrEmpty(PaketAdi))
            {
                e.Cancel = true;
                MessageBox.Show("Lütfen bir paket seçin veya yeni bir paket ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}