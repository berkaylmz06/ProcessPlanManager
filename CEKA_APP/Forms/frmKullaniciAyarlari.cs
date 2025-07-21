using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmKullaniciAyarlari : Form
    {
        private string kullaniciAdi;
        private Kullanicilar mevcutKullanici;
        public frmKullaniciAyarlari(string kullaniciAdi)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;

            this.Icon = Properties.Resources.cekalogokirmizi;
        }

        private void frmKullaniciBilgileri_Load(object sender, EventArgs e)
        {
            txtSifre.PasswordChar = '*';
            txtSifreTekrar.PasswordChar = '*';

            try
            {
                mevcutKullanici = KullanicilarData.KullaniciBilgiGetir(kullaniciAdi);
                if (mevcutKullanici != null)
                {
                    txtKullaniciAdi.Text = mevcutKullanici.kullaniciAdi;
                    txtAdSoyad.Text = mevcutKullanici.adSoyad;
                    txtSifre.Text = mevcutKullanici.sifre;
                    txtSifreTekrar.Text = mevcutKullanici.sifre;
                    txtMail.Text = mevcutKullanici.email;
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
            txtKlasorYolu.Text = Properties.Settings.Default.KlasorYolu;
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            if (txtSifre.Text != txtSifreTekrar.Text)
            {
                MessageBox.Show("Şifreler eşleşmiyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Kullanicilar yeniKullanici = new Kullanicilar
            {
                kullaniciAdi = txtKullaniciAdi.Text,
                adSoyad = txtAdSoyad.Text,
                sifre = txtSifre.Text,
                email = txtMail.Text
            };

            try
            {
                if (BilgilerDegistiMi(mevcutKullanici, yeniKullanici))
                {
                    bool basarili = KullanicilarData.KullaniciGuncelleKullaniciBilgi(yeniKullanici);
                    if (basarili)
                    {
                        MessageBox.Show("Kullanıcı bilgileri güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        mevcutKullanici = yeniKullanici;
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız oldu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Herhangi bir değişiklik yapılmadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool BilgilerDegistiMi(Kullanicilar mevcut, Kullanicilar yeni)
        {
            return mevcut.adSoyad != yeni.adSoyad ||
                   mevcut.sifre != yeni.sifre ||
                   mevcut.email != yeni.email;
        }

        private void btnDosyaSec_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderDialog.SelectedPath = @"\\fileserver\proje";
                folderDialog.Description = "Lütfen bir klasör seçin";

                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtKlasorYolu.Text = folderDialog.SelectedPath;
                    Properties.Settings.Default.KlasorYolu = folderDialog.SelectedPath;
                    Properties.Settings.Default.Save();

                    MessageBox.Show($"Seçilen klasör: {folderDialog.SelectedPath}\n başarıyla kaydedildi.");
                }
                else
                {
                    MessageBox.Show("Klasör seçimi iptal edildi.");
                }
            }
        }
    }
}
