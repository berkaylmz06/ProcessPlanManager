using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.DataBase;
using CEKA_APP.UsrControl;

namespace CEKA_APP
{
    public partial class frmKullaniciEkle : Form
    {
        private ctlKullaniciAyarlari kullaniciAyar;
        public frmKullaniciEkle(ctlKullaniciAyarlari kullaniciAyarKontrol)
        {
            InitializeComponent();
            kullaniciAyar = kullaniciAyarKontrol;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAdSoyad.Text) ||
       string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
       string.IsNullOrWhiteSpace(txtSifre.Text) ||
       string.IsNullOrWhiteSpace(txtMail.Text))
            {
                MessageBox.Show("Kullanıcının tüm bilgilerini giriniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(txtMail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Geçerli bir e-posta adresi giriniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var yeniKullanici = new Kullanicilar
            {
                adSoyad = txtAdSoyad.Text.Trim(),
                kullaniciAdi = txtKullaniciAdi.Text.Trim(),
                sifre = txtSifre.Text.Trim(),
                email = txtMail.Text.Trim()
            };

            if (KullanicilarData.KullaniciAdiVarMi(yeniKullanici.kullaniciAdi))
            {
                MessageBox.Show("Bu kullanıcı adı zaten mevcut.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            KullanicilarData.KullaniciEkle(yeniKullanici);
            MessageBox.Show("Kullanıcı başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

            kullaniciAyar?.YukleVeListele();
        }
    }
}
