using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP.Forms
{
    public partial class frmYeniKilometreTasi : Form
    {
        public string KilometreTasiAdi { get; private set; }
        public string Oran { get; private set; }
        private FiyatlandirmaKilometreTaslariData kilometreTasiData = new FiyatlandirmaKilometreTaslariData();

        public frmYeniKilometreTasi()
        {
            InitializeComponent();
            this.Icon = new Icon("cekalogokirmizi.ico");

            cmbOran.Items.AddRange(new string[] { "%10", "%20", "%30", "%40", "%50", "%60", "%70", "%80", "%90", "%100" });
            this.Controls.Add(cmbOran);

            LoadKilometreTasi();
        }

        private void LoadKilometreTasi()
        {
            var kilometreTasi = kilometreTasiData.GetFiyatlandirmaKilometreTasi();
            listKilometreTaslari.Items.Clear();
            foreach (var (Id, Adi, Tarih) in kilometreTasi)
            {
                listKilometreTaslari.Items.Add(Adi);
            }
            listKilometreTaslari.SelectedIndex = -1;
            cmbOran.SelectedIndex = -1; 
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listKilometreTaslari.SelectedIndex >= 0 && cmbOran.SelectedIndex >= 0)
            {
                KilometreTasiAdi = listKilometreTaslari.SelectedItem.ToString();
                Oran = cmbOran.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir kilometre taşı ve oran seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!txtKilometreTasi.Visible)
            {
                txtKilometreTasi.Clear();
                txtKilometreTasi.Visible = true;
                lblYeniKilometreTasi.Visible = true;
                lblYeniKilometreTasiBilgi.Visible = true;

                lblOran.Visible = false;
                cmbOran.Visible = false;

                btnEkle.Text = "Onayla";
                this.Height += 40;
            }
            else if (!string.IsNullOrEmpty(txtKilometreTasi.Text))
            {
                KilometreTasiAdi = txtKilometreTasi.Text;

                kilometreTasiData.FiyatlandirmaKilometreTasiEkle(KilometreTasiAdi);
                LoadKilometreTasi();

                txtKilometreTasi.Visible = false;
                lblYeniKilometreTasi.Visible = false;
                lblYeniKilometreTasiBilgi.Visible = false;

                lblOran.Visible = true;
                cmbOran.Visible = true;

                btnEkle.Text = "Yeni Ekle";
                this.Height -= 40;
                MessageBox.Show("Yeni kilometre taşı eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen yeni kilometre taşı adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmYeniKilometreTasi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None && (string.IsNullOrEmpty(KilometreTasiAdi) || string.IsNullOrEmpty(Oran)))
            {
                e.Cancel = true;
                MessageBox.Show("Lütfen bir kilometre taşı ve oran seçin veya yeni bir kilometre taşı ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
