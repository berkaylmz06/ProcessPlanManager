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

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            KullanicilarData kullaniciService = new KullanicilarData();
            Kullanicilar kullanici = kullaniciService.GirisYap(kullaniciAdi, sifre);

            if (kullanici != null)
            {
                frmAnaSayfa form1 = new frmAnaSayfa(kullanici);
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
