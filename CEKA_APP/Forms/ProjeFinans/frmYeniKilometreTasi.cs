using CEKA_APP.Interfaces.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmYeniKilometreTasi : Form
    {
        public string KilometreTasiAdi { get; private set; }
        public string Oran { get; private set; }
        private List<string> _alreadySelectedMilestones;

        private const int VerticalMargin = 12;
        private readonly IKilometreTaslariService _kilometreTaslariService;
        public bool TutarIleGirildi => chkTutarIleGir.Checked;
        public string GirilenDeger
        {
            get
            {
                if (TutarIleGirildi)
                    return txtTutar.Text;
                return Oran;
            }
        }
        public frmYeniKilometreTasi(IKilometreTaslariService kilometreTaslariService, List<string> alreadySelectedMilestones = null)
        {
            InitializeComponent();

            _kilometreTaslariService = kilometreTaslariService ?? throw new ArgumentNullException(nameof(kilometreTaslariService));
            this.Icon = Properties.Resources.cekalogokirmizi;

            _alreadySelectedMilestones = alreadySelectedMilestones ?? new List<string>();

            cmbOran.Items.AddRange(new string[] { "%5", "%10", "%15", "%20", "%25", "%30", "%35", "%40", "%45", "%50", "%55", "%60", "%65", "%70", "%75", "%80", "%85", "%90", "%95", "%100" });

            panelYeniKilometreTasi.Visible = false;

            LoadKilometreTasi();

            SetFormSize(false);

            txtKilometreTasi.TextChanged += txtKilometreTasi_TextChanged;
        }

        private void LoadKilometreTasi()
        {
            var allKilometreTaslari = _kilometreTaslariService.GetFiyatlandirmaKilometreTasi();
            listKilometreTaslari.Items.Clear();

            foreach (var (Id, Adi, Tarih) in allKilometreTaslari)
            {
                if (!_alreadySelectedMilestones.Any(selectedAdi => selectedAdi.Equals(Adi, StringComparison.OrdinalIgnoreCase)))
                {
                    listKilometreTaslari.Items.Add(Adi);
                }
            }
            listKilometreTaslari.SelectedIndex = -1;
            cmbOran.SelectedIndex = -1;
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listKilometreTaslari.SelectedIndex >= 0)
            {
                KilometreTasiAdi = listKilometreTaslari.SelectedItem.ToString();

                if (TutarIleGirildi)
                {
                    if (string.IsNullOrWhiteSpace(txtTutar.Text))
                    {
                        MessageBox.Show("Lütfen bir tutar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Oran = ""; 
                }
                else
                {
                    if (cmbOran.SelectedIndex < 0)
                    {
                        MessageBox.Show("Lütfen bir oran seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Oran = cmbOran.SelectedItem.ToString();
                }

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir kilometre taşı seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (btnEkle.Text == "Yeni Ekle")
            {
                cmbOran.Visible = false;
                lblOran.Visible = false;
                btnSec.Visible = false;

                panelYeniKilometreTasi.Visible = true;
                txtKilometreTasi.Clear();

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
                if (!string.IsNullOrEmpty(txtKilometreTasi.Text.Trim()))
                {
                    string yeniKilometreTasiAdi = txtKilometreTasi.Text.Trim();

                    var mevcutVeritabaniKilometreTaslari = _kilometreTaslariService.GetFiyatlandirmaKilometreTasi();
                    if (mevcutVeritabaniKilometreTaslari.Any(kt => kt.Adi.Equals(yeniKilometreTasiAdi, StringComparison.OrdinalIgnoreCase)))
                    {
                        MessageBox.Show("Bu kilometre taşı veritabanında zaten mevcut. Lütfen farklı bir ad girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    KilometreTasiAdi = yeniKilometreTasiAdi;
                    _kilometreTaslariService.FiyatlandirmaKilometreTasiEkle(KilometreTasiAdi);
                    LoadKilometreTasi();

                    GeriDon();

                    MessageBox.Show("Yeni kilometre taşı eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lütfen yeni kilometre taşı adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtKilometreTasi_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKilometreTasi.Text))
            {
                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Red;
            }
            else
            {
                btnEkle.Text = "Onayla";
                btnEkle.BackColor = Color.FromArgb(46, 204, 113);
            }
        }

        private void SetFormSize(bool expanded)
        {
            if (expanded)
            {
                this.ClientSize = new Size(this.ClientSize.Width, panelYeniKilometreTasi.Bottom + VerticalMargin);
            }
            else
            {
                this.ClientSize = new Size(this.ClientSize.Width, btnEkle.Bottom + VerticalMargin);
            }
        }

        private void GeriDon()
        {
            panelYeniKilometreTasi.Visible = false;

            listKilometreTaslari.Visible = true;
            cmbOran.Visible = true;
            lblOran.Visible = true;
            btnSec.Visible = true;

            btnEkle.Text = "Yeni Ekle";
            btnEkle.BackColor = Color.Gray;

            SetFormSize(false);
        }

        private void frmYeniKilometreTasi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None &&
                (string.IsNullOrEmpty(KilometreTasiAdi) ||
                 (!TutarIleGirildi && string.IsNullOrEmpty(Oran)) ||
                 (TutarIleGirildi && string.IsNullOrEmpty(txtTutar.Text))))
            {
                e.Cancel = true;
                MessageBox.Show("Lütfen bir kilometre taşı ve oran/tutar seçin veya yeni bir kilometre taşı ekleyin.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void chkTutarIleGir_CheckedChanged(object sender, EventArgs e)
        {
            bool tutarSecili = chkTutarIleGir.Checked;

            cmbOran.Visible = !tutarSecili;
            lblOran.Visible = !tutarSecili;

            lblTutar.Visible = tutarSecili;
            txtTutar.Visible = tutarSecili;
        }
    }
}