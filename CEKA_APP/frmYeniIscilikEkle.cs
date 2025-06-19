using CEKA_APP.DataBase.ProjeFinans;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmYeniIscilikEkle : Form
    {
        public string IscilikAdi { get; private set; }
        private IscilikData iscilikData = new IscilikData();

        public frmYeniIscilikEkle()
        {
            InitializeComponent();
            LoadIscilikler();

            this.Icon = new Icon("cekalogokirmizi.ico");

        }

        private void LoadIscilikler()
        {
            var iscilikler = iscilikData.GetIscilikler();
            listIscilikler.Items.Clear();
            foreach (var (Id, Adi, Tarih) in iscilikler)
            {
                listIscilikler.Items.Add(Adi);
            }
            listIscilikler.SelectedIndex = -1; 
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            if (listIscilikler.SelectedIndex >= 0)
            {
                IscilikAdi = listIscilikler.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir işçilik seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!txtYeniIscilik.Visible)
            {
                txtYeniIscilik.Clear();
                txtYeniIscilik.Visible = true;
                lblYeniIscilik.Visible = true;
                btnEkle.Text = "Onayla";
                this.Height += 30;
            }
            else if (!string.IsNullOrEmpty(txtYeniIscilik.Text))
            {
                IscilikAdi = txtYeniIscilik.Text;
                int iscilikId = iscilikData.IscilikEkle(IscilikAdi);
                LoadIscilikler();
                txtYeniIscilik.Visible = false;
                lblYeniIscilik.Visible = false;
                btnEkle.Text = "Yeni Ekle";
                this.Height -= 30;
                MessageBox.Show("Yeni işçilik eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen yeni işçilik adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmYeniIscilikEkle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None && string.IsNullOrEmpty(IscilikAdi))
            {
                e.Cancel = true;
                MessageBox.Show("Lütfen bir işçilik seçin veya yeni bir işçilik ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}