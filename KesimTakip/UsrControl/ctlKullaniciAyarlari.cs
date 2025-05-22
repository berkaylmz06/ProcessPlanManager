using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using KesimTakip.DataBase;
using KesimTakip.Entitys;
using KesimTakip.Helper;

namespace KesimTakip.UsrControl
{
    public partial class ctlKullaniciAyarlari : UserControl
    {
        private List<Kullanicilar> tumKullanicilar;
        private Timer flashTimer;
        private bool isRed = false;
        private int flashCount = 0;
        private const int maxFlashes = 3;
        public ctlKullaniciAyarlari()
        {
            InitializeComponent();

            ButonGenelHelper.KullaniciEkleButonAyari(btnYeniKullanici);
            ButonGenelHelper.KullaniciEkleButonAyari(btnRolAta);
            ButonGenelHelper.KullaniciEkleButonAyari(btnGüncelle);
            ListBoxHelper.StilUygula(lstKullanicilar);

            YukleVeListele();

            flashTimer = new Timer();
            flashTimer.Interval = 200;
            flashTimer.Tick += FlashTimer_Tick;
        }

        private void ctlKullaniciAyarlari_Load(object sender, EventArgs e)
        {
            lstKullanicilar.ClearSelected();
        }

        private void YukleVeListele()
        {
            var dt = KullanicilarData.GetKullaniciListesi();
            if (dt == null || dt.Rows.Count == 0)
                return;

            tumKullanicilar = dt.AsEnumerable()
                .GroupBy(row => row["adSoyad"].ToString())
                .Select(gr => new Kullanicilar
                {
                    adSoyad = gr.Key,
                    kullaniciAdi = gr.First()["kullaniciAdi"].ToString(),
                    sifre = gr.First()["sifre"].ToString(),
                    kullaniciRol = gr.First()["kullaniciRol"].ToString(),
                    email = gr.First()["email"].ToString()

                }).OrderBy(x => x.adSoyad).ToList();

            KullanicilariListele(tumKullanicilar);
        }

        private void KullanicilariListele(List<Kullanicilar> kullanicilar)
        {
            lstKullanicilar.DataSource = null;
            lstKullanicilar.DataSource = kullanicilar;
            lstKullanicilar.DisplayMember = "poz";
        }

        private void lstKullanicilar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstKullanicilar.SelectedItem == null)
            {
                txtAdSoyad.Clear();
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
                cbKullaniciRol.Text = "";
                txtEmail.Clear();
                return;
            }

            if (lstKullanicilar.SelectedItem is Kullanicilar secilen)
            {
                txtAdSoyad.Text = secilen.adSoyad.ToString();
                txtKullaniciAdi.Text = secilen.kullaniciAdi.ToString();
                txtSifre.Text = secilen.sifre.ToString();
                cbKullaniciRol.Text = secilen.kullaniciRol.ToString();
                txtEmail.Text = secilen.email.ToString();
            }
        }
       
        private void btnGüncelle_Click(object sender, EventArgs e)
        {

        }

        private void btnRolAta_Click(object sender, EventArgs e)
        {
            if (lstKullanicilar.SelectedItem is Kullanicilar secilen)
            {
                flashCount = 0;
                isRed = false;
                flashTimer.Start();
            }
            else
            {
                MessageBox.Show("Lütfen bir kullanıcı seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void FlashTimer_Tick(object sender, EventArgs e)
        {
            if (flashCount >= maxFlashes * 2)
            {
                flashTimer.Stop();
                panelRolAtaCizgi.BackColor = Color.White;
                return;
            }

            if (isRed)
            {
                panelRolAtaCizgi.BackColor = Color.White;
            }
            else
            {
                panelRolAtaCizgi.BackColor = Color.Red;
            }

            isRed = !isRed;
            flashCount++;
        }

        private void btnYeniKullanici_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) || string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) || string.IsNullOrWhiteSpace(txtSifre.Text) || string.IsNullOrWhiteSpace(cbKullaniciRol.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Kullanıcının tüm bilgilerini giriniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var yeniKullanici = new Kullanicilar
                {
                    adSoyad = txtAdSoyad.Text.Trim(),
                    kullaniciAdi = txtKullaniciAdi.Text.Trim(),
                    sifre = txtSifre.Text.Trim(),
                    kullaniciRol = cbKullaniciRol.Text.Trim(),
                    email = txtEmail.Text.Trim()
                };

                KullanicilarData.KullaniciEkle(yeniKullanici);
            }

        }
    }
}
