using CEKA_APP.DataBase;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.UsrControl.Interfaces;
using CEKA_APP.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
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
        private readonly IServiceProvider _serviceProvider;

        public frmKullaniciGirisi(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

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
            if (IsPublished())
            {
                try
                {
                    GuncellemeKontrol();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Güncelleme kontrolü başarısız: " + ex.Message);
                }
            }
        }
        private bool IsPublished()
        {
            try
            {
                return ApplicationDeployment.IsNetworkDeployed;
            }
            catch
            {
                return false;
            }
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

                string remoteVersionFile = ConfigurationManager.AppSettings["RemoteVersionFile"];
                string setupFileLocation = ConfigurationManager.AppSettings["SetupFile"];

                if (string.IsNullOrEmpty(remoteVersionFile) || string.IsNullOrEmpty(setupFileLocation))
                {
                    MessageBox.Show("Uygulama yapılandırma dosyasında (App.config) sürüm ve kurulum dosya yolları ('RemoteVersionFile', 'SetupFile') tanımlanmamış. Lütfen App.config dosyasını kontrol edin.", "Yapılandırma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                        if (File.Exists(setupFileLocation))
                        {
                            Process.Start(setupFileLocation);
                        }
                        else
                        {
                            MessageBox.Show("Kurulum dosyası bulunamadı:\n" + setupFileLocation, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
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
                var kullanciciService = _serviceProvider.GetService<IKullanicilarService>();
                Kullanicilar kullanici = kullanciciService.GirisYap(kullaniciAdi, sifre);

                if (kullanici != null)
                {
                    try
                    {
                        if (chkBeniHatirla.Checked)
                        {
                            Properties.Settings.Default.BeniHatirla = true;
                            Properties.Settings.Default.KaydedilenKullaniciAdi = kullaniciAdi;
                            Properties.Settings.Default.KaydedilenSifre = SecurityHelpers.ProtectString(sifre);
                        }
                        else
                        {
                            Properties.Settings.Default.BeniHatirla = false;
                            Properties.Settings.Default.KaydedilenKullaniciAdi = string.Empty;
                            Properties.Settings.Default.KaydedilenSifre = string.Empty;
                        }

                        Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Ayar kaydetme hatası: " + ex.Message);
                    }

                    var scope = _serviceProvider.CreateScope();

                    var userControlFactory = scope.ServiceProvider.GetRequiredService<IUserControlFactory>();
                    var formAnaSayfa = new frmAnaSayfa(kullanici, userControlFactory, scope.ServiceProvider);

                    formAnaSayfa.FormClosed += (s, args) => Application.Exit();
                    formAnaSayfa.Show();
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

                var encrypted = Properties.Settings.Default.KaydedilenSifre;
                if (!string.IsNullOrEmpty(encrypted))
                {
                    var decrypted = SecurityHelpers.UnprotectString(encrypted);
                    if (!string.IsNullOrEmpty(decrypted))
                    {
                        txtSifre.Text = decrypted;
                        chkBeniHatirla.Checked = true;
                    }
                    else
                    {
                        Properties.Settings.Default.BeniHatirla = false;
                        Properties.Settings.Default.KaydedilenKullaniciAdi = string.Empty;
                        Properties.Settings.Default.KaydedilenSifre = string.Empty;
                        Properties.Settings.Default.Save();

                        txtSifre.Text = string.Empty;
                        chkBeniHatirla.Checked = false;
                    }
                }
                else
                {
                    chkBeniHatirla.Checked = false;
                }
            }
        }

        private void frmKullaniciGirisi_Resize(object sender, EventArgs e)
        {
            panelCizgi.Width = this.ClientSize.Width - 80;
            panelCizgi.Left = 40;
        }
    }
}
