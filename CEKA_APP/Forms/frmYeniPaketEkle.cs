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
    public partial class frmYeniPaketEkle : Form
    {
        public string PaketAdi { get; private set; }
        private ProjeFinans_SevkiyatPaketleriData paketData = new ProjeFinans_SevkiyatPaketleriData();
        public frmYeniPaketEkle()
        {
            InitializeComponent();
            LoadPaketler();

            this.Icon = Properties.Resources.cekalogokirmizi;
        }
        private void LoadPaketler()
        {
            var paketler = paketData.GetPaketler();
            listPaketler.Items.Clear();
            foreach (var (Id, Adi, Tarih) in paketler)
            {
                listPaketler.Items.Add(Adi);
            }
            listPaketler.SelectedIndex = -1;
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (!txtPaketAdi.Visible)
            {
                txtPaketAdi.Clear();

                btnEkle.Text = "Onayla";
                this.Height += 30;
            }
            else if (!string.IsNullOrEmpty(txtPaketAdi.Text))
            {
                PaketAdi = txtPaketAdi.Text;
                paketData.PaketEkle(PaketAdi);
                LoadPaketler();
                btnEkle.Text = "Yeni Ekle";
                this.Height -= 30;
                MessageBox.Show("Yeni paket eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen yeni paket adını girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
