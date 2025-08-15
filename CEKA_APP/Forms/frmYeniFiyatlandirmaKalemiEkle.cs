using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmYeniFiyatlandirmaKalemiEkle : Form
    {
        public string KalemAdi { get; private set; }
        private ProjeFinans_FiyatlandirmaKalemleriData kalemData = new ProjeFinans_FiyatlandirmaKalemleriData();

        private const int VerticalMargin = 12;

        public frmYeniFiyatlandirmaKalemiEkle()
        {
            InitializeComponent();
            LoadKalemler();

            panelYeniKalem.Visible = false;

            this.ClientSize = new Size(this.ClientSize.Width, btnEkle.Bottom + VerticalMargin);

            this.Icon = Properties.Resources.cekalogokirmizi;

            txtYeniKalem.TextChanged += txtYeniKalem_TextChanged;
        }

        private void LoadKalemler()
        {
            var kalemler = kalemData.GetFiyatlandirmaKalemleri();
            listKalemler.Items.Clear();
            foreach (var (Id, Adi, Tarih) in kalemler)
            {
                listKalemler.Items.Add(Adi);
            }
            listKalemler.SelectedIndex = -1;
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listKalemler.SelectedIndex >= 0)
            {
                KalemAdi = listKalemler.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir kalem seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (btnEkle.Text == "Yeni Ekle")
            {
                panelYeniKalem.Visible = true;
                this.ClientSize = new Size(this.ClientSize.Width, panelYeniKalem.Bottom + VerticalMargin);

                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Red;
            }
            else if (btnEkle.Text == "İptal")
            {
                GeriDon();
            }
            else if (btnEkle.Text == "Onayla")
            {
                if (!string.IsNullOrEmpty(txtYeniKalem.Text))
                {
                    KalemAdi = txtYeniKalem.Text;
                    kalemData.FiyatlandirmaKalemleriEkle(KalemAdi);
                    LoadKalemler();

                    GeriDon();

                    MessageBox.Show("Yeni kalem başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lütfen yeni kalem adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtYeniKalem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtYeniKalem.Text))
            {
                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Gray;
            }
            else
            {
                btnEkle.Text = "Onayla";
                btnEkle.BackColor = Color.FromArgb(46, 204, 113); // Onay için yeşil
            }
        }

        private void GeriDon()
        {
            panelYeniKalem.Visible = false;
            btnSec.Visible = true;
            txtYeniKalem.Clear();
            btnEkle.Text = "Yeni Ekle";
            btnEkle.BackColor = Color.Gray;

            this.ClientSize = new Size(this.ClientSize.Width, btnEkle.Bottom + VerticalMargin);
        }

        private void frmYeniFiyatlandirmaKalemiEkle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None && string.IsNullOrEmpty(KalemAdi))
            {
                e.Cancel = true;
                MessageBox.Show("Lütfen bir kalem seçin veya yeni bir kalem ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}