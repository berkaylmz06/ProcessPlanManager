using KesimTakip.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KesimTakip
{
    public partial class frmKullaniciGirisi : Form
    {
        public frmKullaniciGirisi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chkYoneticiGirisi.Checked)
            {
                string kullaniciAdi = txtKullaniciAdi.Text.Trim();
                string sifre = txtSifre.Text.Trim();

                YoneticiData kullaniciService = new YoneticiData();
                Yoneticiler yonetici = kullaniciService.GirisYapYonetici(kullaniciAdi, sifre);

                if (yonetici != null)
                {
                    frmYoneticiEkrani yonet = new frmYoneticiEkrani();
                    yonet.Show();
                    this.Hide();

                    yonet.FormClosed += (s, args) => Application.Exit();
                }
                else
                {
                    MessageBox.Show("Geçersiz kullanıcı adı veya şifre.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string kullaniciAdi = txtKullaniciAdi.Text.Trim();
                string sifre = txtSifre.Text.Trim();

                KullanicilarData kullaniciService = new KullanicilarData();
                Kullanicilar kullanici = kullaniciService.GirisYap(kullaniciAdi, sifre);

                if (kullanici != null)
                {
                    frmAnaSayfa form1 = new frmAnaSayfa(kullanici.adSoyad);
                    form1.Show();
                    this.Hide();

                    form1.FormClosed += (s, args) => Application.Exit();
                }
                else
                {
                    MessageBox.Show("Geçersiz kullanıcı adı veya şifre.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
