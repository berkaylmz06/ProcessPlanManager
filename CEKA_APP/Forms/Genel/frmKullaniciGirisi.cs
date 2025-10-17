using CEKA_APP.DataBase;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.Security;
using CEKA_APP.Services.Sistem;
using CEKA_APP.UsrControl.Interfaces;
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
        private IKullanicilarService _kullanicilarService=> _serviceProvider.GetRequiredService<IKullanicilarService>();
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
            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;

            Kullanicilar aktifKullanici = _kullanicilarService.GirisYap(kullaniciAdi, sifre);

            if (aktifKullanici != null)
            {
                if (chkBeniHatirla.Checked)
                {
                    Properties.Settings.Default.BeniHatirla = true;
                    Properties.Settings.Default.KaydedilenKullaniciAdi = aktifKullanici.kullaniciAdi;
                    Properties.Settings.Default.KaydedilenSifre = SecurityHelpers.ProtectString(sifre);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.BeniHatirla = false;
                    Properties.Settings.Default.KaydedilenKullaniciAdi = string.Empty;
                    Properties.Settings.Default.KaydedilenSifre = string.Empty;
                    Properties.Settings.Default.Save();
                }

                var userControlFactory = _serviceProvider.GetRequiredService<IUserControlFactory>();

                if (aktifKullanici.kullaniciRol == "Operatör")
                {
                    var kesimYonetimiControl = userControlFactory.CreateKesimYonetimiControl();
                    kesimYonetimiControl.Dock = DockStyle.Fill;

                    Form hostForm = new Form
                    {
                        Text = $"Kesim Yönetimi - {aktifKullanici.kullaniciAdi}",
                        StartPosition = FormStartPosition.CenterScreen,
                        WindowState = FormWindowState.Maximized,
                        Icon = this.Icon,

                        ControlBox = false,
                        FormBorderStyle = FormBorderStyle.None
                    };

                    hostForm.Controls.Add(kesimYonetimiControl);

                    hostForm.FormClosed += (s, args) => Application.Exit();

                    this.Hide();

                    hostForm.Show();
                }
                else
                {
                    var anaSayfa = ActivatorUtilities.CreateInstance<frmAnaSayfa>(
                        _serviceProvider,
                        aktifKullanici,
                        userControlFactory,
                        _serviceProvider
                    );

                    this.Hide();
                    anaSayfa.Show();
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
