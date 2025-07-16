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

        public frmYeniFiyatlandirmaKalemiEkle()
        {
            InitializeComponent();
            LoadKalemler();

            this.Icon = new Icon("cekalogokirmizi.ico");

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
            if (!txtYeniKalem.Visible)
            {
                txtYeniKalem.Clear();
                txtYeniKalem.Visible = true;
                lblYeniKalemAdi.Visible = true;
                lblYeniKalemBilgi.Visible = true;
                
                btnEkle.Text = "Onayla";
                this.Height += 30;
            }
            else if (!string.IsNullOrEmpty(txtYeniKalem.Text))
            {
                KalemAdi = txtYeniKalem.Text;
                kalemData.FiyatlandirmaKalemleriEkle(KalemAdi);
                LoadKalemler();
                txtYeniKalem.Visible = false;
                lblYeniKalemAdi.Visible = false;
                lblYeniKalemBilgi.Visible = false;
                btnEkle.Text = "Yeni Ekle";
                this.Height -= 30;
                MessageBox.Show("Yeni kalem eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen yeni kalem adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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