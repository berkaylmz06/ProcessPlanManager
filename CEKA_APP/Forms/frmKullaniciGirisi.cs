using CEKA_APP.DataBase;
using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            this.Icon = Properties.Resources.cekalogokirmizi;

            this.BackColor = ColorTranslator.FromHtml("#2C3E50");
            btnGiris.BackColor = Color.FromArgb(52, 152, 219);
            pictureBox1.Image = Properties.Resources.cekalogo;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            panelCizgi.Width = this.ClientSize.Width - 80;
            panelCizgi.Height = 1;
            panelCizgi.BackColor = Color.White;
            panelCizgi.Left = 40;
            panelCizgi.Top = pictureBox1.Bottom + 50;
        }
        private void frmKullaniciGirisi_Load(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = true;
            this.AcceptButton = btnGiris;
            KullaniciBilgileriniYukle();
            //GuncellemeKontrol();
        }

        public void GuncellemeKontrol()
        {
            try
            {
                string localVersion;

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    localVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                else
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    localVersion = fileVersionInfo.ProductVersion;
                }

                string remoteVersionFile = @"\\fileserver\proje\CEKA APP\version.txt";

                if (!File.Exists(remoteVersionFile))
                {
                    MessageBox.Show("Sürüm dosyası bulunamadı:\n" + remoteVersionFile, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string latestVersionText = File.ReadAllText(remoteVersionFile).Trim();

                if (!Version.TryParse(latestVersionText, out Version remoteVer))
                {
                    MessageBox.Show("Sunucudaki sürüm formatı geçersiz:\n" + latestVersionText, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Version.TryParse(localVersion, out Version localVer))
                {
                    MessageBox.Show("Yerel sürüm formatı geçersiz:\n" + localVersion, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (localVer < remoteVer)
                {
                    DialogResult result = MessageBox.Show(
                        $"Yeni sürüm bulundu!\n\nYerel: {localVersion}\nSunucu: {latestVersionText}\n\nGüncellemeyi başlatmak istiyor musunuz?",
                        "Güncelleme Gerekli", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        //Process.Start(@"\\fileserver\proje\CEKA APP\setup.exe");
                        Process.Start(@"\\192.168.2.3\proje\CEKA APP\setup.exe");
                    }

                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme kontrolü sırasında hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void frmKullaniciGirisi_Resize(object sender, EventArgs e)
        {
            panelCizgi.Width = this.ClientSize.Width - 80;
            panelCizgi.Left = 40;
        }
    }
}