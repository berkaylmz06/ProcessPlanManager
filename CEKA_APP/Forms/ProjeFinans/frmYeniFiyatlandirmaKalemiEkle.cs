using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmYeniFiyatlandirmaKalemiEkle : Form
    {
        public string KalemAdi { get; set; }
        public string KalemBirimi { get; set; }

        private const int VerticalMargin = 12;

        private readonly IFiyatlandirmaKalemleriService _fiyatlandirmaKalemleriService;
        public frmYeniFiyatlandirmaKalemiEkle(IFiyatlandirmaKalemleriService fiyatlandirmaKalemleriService)
        {
            InitializeComponent();

            _fiyatlandirmaKalemleriService = fiyatlandirmaKalemleriService ?? throw new ArgumentNullException(nameof(fiyatlandirmaKalemleriService));

            LoadKalemler();

            panelYeniKalem.Visible = false;

            this.ClientSize = new Size(this.ClientSize.Width, btnEkle.Bottom + VerticalMargin);

            this.Icon = Properties.Resources.cekalogokirmizi;

            txtYeniKalem.TextChanged += txtYeniKalem_TextChanged;
            cmbBirim.SelectedIndexChanged += txtYeniKalem_TextChanged; 
        }

        private void LoadKalemler()
        {
            var kalemler = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemleri();
            listKalemler.Items.Clear();
            foreach (var (Id, Adi, Birimi, Tarih) in kalemler)
            {
                listKalemler.Items.Add(Adi);
            }
            listKalemler.SelectedIndex = -1;
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listKalemler.SelectedIndex >= 0)
            {
                string secilenKalemAdi = listKalemler.SelectedItem.ToString();
                var kalemDetay = _fiyatlandirmaKalemleriService.GetFiyatlandirmaKalemByAdi(secilenKalemAdi);

                if (kalemDetay != null)
                {
                    KalemAdi = kalemDetay.kalemAdi;
                    KalemBirimi = kalemDetay.kalemBirimi;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Seçilen kalem veritabanında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (!string.IsNullOrEmpty(txtYeniKalem.Text) && cmbBirim.SelectedItem != null)
                {
                    KalemAdi = txtYeniKalem.Text;
                    KalemBirimi = cmbBirim.SelectedItem.ToString(); 
                    _fiyatlandirmaKalemleriService.FiyatlandirmaKalemleriEkle(KalemAdi, KalemBirimi);
                    LoadKalemler();

                    GeriDon();

                    MessageBox.Show("Yeni kalem başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lütfen yeni kalem adını ve birimini seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtYeniKalem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtYeniKalem.Text) || cmbBirim.SelectedItem == null)
            {
                btnEkle.Text = "İptal";
                btnEkle.BackColor = Color.Gray;
            }
            else
            {
                btnEkle.Text = "Onayla";
                btnEkle.BackColor = Color.FromArgb(46, 204, 113); 
            }
        }

        private void GeriDon()
        {
            panelYeniKalem.Visible = false;
            btnSec.Visible = true;
            txtYeniKalem.Clear();
            cmbBirim.SelectedIndex = -1; 
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