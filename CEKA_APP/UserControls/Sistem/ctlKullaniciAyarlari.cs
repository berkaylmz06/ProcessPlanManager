using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKullaniciAyarlari : UserControl
    {
        private List<Kullanicilar> tumKullanicilar;
        private Timer flashTimer;
        private bool isRed = false;
        private int flashCount = 0;
        private const int maxFlashes = 3;
        private readonly IServiceProvider _serviceProvider;

        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();

        public ctlKullaniciAyarlari(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            ButonGenelHelper.KullaniciEkleButonAyari(btnYeniKullanici);
            ButonGenelHelper.KullaniciEkleButonAyari(btnRolAta);
            ButonGenelHelper.KullaniciEkleButonAyari(btnGüncelle);
            ButonGenelHelper.KullaniciEkleButonAyari(btnKullaniciSil);
            ListBoxHelper.StilUygula(lstKullanicilar);

            YukleVeListele();

            flashTimer = new Timer();
            flashTimer.Interval = 200;
            flashTimer.Tick += FlashTimer_Tick;
        }

        private void ctlKullaniciAyarlari_Load(object sender, EventArgs e)
        {
            lstKullanicilar.ClearSelected();
            ctlBaslik1.Baslik = "Kullanıcı Ayarları";
            ctlBaslik2.Baslik = "Kullanıcı Bilgileri";

            txtSifre.PasswordChar = '*';
        }

        public void YukleVeListele()
        {
            var dt = _kullaniciService.GetKullaniciListesi();
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
            lstKullanicilar.DisplayMember = "kullaniciAdi";
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
            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text))
            {
                MessageBox.Show("Lütfen güncellenecek kullanıcı adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var kullanici = new Kullanicilar
            {
                kullaniciAdi = txtKullaniciAdi.Text.Trim(),
                adSoyad = txtAdSoyad.Text.Trim(),
                sifre = txtSifre.Text.Trim(),
                kullaniciRol = cbKullaniciRol.Text.Trim(),
                email = txtEmail.Text.Trim()
            };

            var result = MessageBox.Show($"{kullanici.adSoyad} adlı kullanıcı güncellensin mi?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool guncellendi = _kullaniciService.KullaniciGuncelle(kullanici);

                if (guncellendi)
                {
                    MessageBox.Show("Kullanıcı başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    YukleVeListele();
                }
                else
                {
                    MessageBox.Show("Güncellenecek veri bulunamadı veya değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
           frmKullaniciEkle kulEkle = new frmKullaniciEkle(this, _kullaniciService);
            kulEkle.ShowDialog();
        }

        private void btnKullaniciSil_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();

            if (string.IsNullOrWhiteSpace(kullaniciAdi))
            {
                MessageBox.Show("Silinecek kullanıcı adını giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"{kullaniciAdi} adlı kullanıcıyı silmek istediğinize emin misiniz?",
                                         "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool silindi = _kullaniciService.KullaniciSil(kullaniciAdi);

                if (silindi)
                {
                    MessageBox.Show("Kullanıcı başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    YukleVeListele();
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı veya silinemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
