using CEKA_APP.DataBase;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmKullaniciGirisi : Form
    {
        private readonly KullanicilarData _kullaniciService;

        public frmKullaniciGirisi()
        {
            InitializeComponent();
            _kullaniciService = new KullanicilarData();
            this.Icon = new Icon("cekalogokirmizi.ico");
        }
        private void frmKullaniciGirisi_Load(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = true;
            this.AcceptButton = btnGiris;
            KullaniciBilgileriniYukle();
        }
        private void btnGiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Kullanıcı adı ve şifre alanları boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Kullanicilar kullanici = _kullaniciService.GirisYap(kullaniciAdi, sifre);

                if (kullanici != null)
                {
                    KullaniciBilgileriniKaydet();
                    frmAnaSayfa form1 = new frmAnaSayfa(kullanici);
                    form1.FormClosed += (s, args) => Application.Exit();
                    form1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Geçersiz kullanıcı adı veya şifre.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGiris_Click(sender, e);
            }
        }

        private void KullaniciBilgileriniYukle()
        {
            if (Properties.Settings.Default.BeniHatirla)
            {
                txtKullaniciAdi.Text = Properties.Settings.Default.KaydedilenKullaniciAdi ?? string.Empty;
                txtSifre.Text = Properties.Settings.Default.KaydedilenSifre ?? string.Empty;
                chkBeniHatirla.Checked = true;
            }
        }

        private void KullaniciBilgileriniKaydet()
        {
            if (chkBeniHatirla.Checked)
            {
                Properties.Settings.Default.KaydedilenKullaniciAdi = txtKullaniciAdi.Text;
                Properties.Settings.Default.KaydedilenSifre = txtSifre.Text;
                Properties.Settings.Default.BeniHatirla = true;
            }
            else
            {
                Properties.Settings.Default.KaydedilenKullaniciAdi = string.Empty;
                Properties.Settings.Default.KaydedilenSifre = string.Empty;
                Properties.Settings.Default.BeniHatirla = false;
            }

            Properties.Settings.Default.Save();
        }
    }
}