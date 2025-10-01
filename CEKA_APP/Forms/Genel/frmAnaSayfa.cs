using CEKA_APP.Abstracts;
using CEKA_APP.Concretes;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.Sistem;
using CEKA_APP.UsrControl;
using CEKA_APP.UsrControl.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmAnaSayfa : Form
    {
        private Timer timer;
        private int timerCounter = 0;
        public IFormArayuzu FormArayuzuInterface { get; private set; }
        public IKullaniciAdiOgren KullaniciAdiInterface { get; private set; }

        private Kullanicilar aktifKullanici;
        private Stack<UserControl> geriYigin = new Stack<UserControl>();
        private Stack<UserControl> ileriYigin = new Stack<UserControl>();

        private ctlKesimPlaniEkle kesimPlaniEkle;
        private ctlSistemBilgisi sistemBilgisiAnaSayfa;
        private ctlAutoCadAktarim autoCadAktarim;
        private ctlProjeOgeleri projeOgeleri;
        private ctlKarsilastirmaTablosu karsilastirmaTablosu;
        public ctlProjeFiyatlandirma projeFiyatlandirma;
        public ctlProjeBilgileri projeBilgileri;
        private ctlKullaniciAyarlari kullaniciAyarlari;

        private Dictionary<string, List<(string Text, Action ClickAction, bool Visible)>> buttonGroups;

        private readonly IUserControlFactory _userControlFactory;
        private readonly IServiceProvider _serviceProvider;

        public frmAnaSayfa(Kullanicilar kullanici, IUserControlFactory userControlFactory, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userControlFactory = userControlFactory ?? throw new ArgumentNullException(nameof(userControlFactory));

            aktifKullanici = kullanici ?? throw new ArgumentNullException(nameof(kullanici));
            FormArayuzuInterface = new FormArayuzu(this);
            KullaniciAdiInterface = new KullaniciAdiOgren(this);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            ShowCurrentDateTime();

            panelAraYuz.Dock = DockStyle.Left;
            panelAraYuz.Width = 250;
            panelSistem.Height = 300;
            panelYardim.Height = 300;
            panelDuyuru.Height = 300;

            panelAraYuz.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelYardimCubugu.BackColor = ColorTranslator.FromHtml("#E67E22");
            panelNavigasyon.BackColor = ColorTranslator.FromHtml("#E67E22");

            this.Icon = Properties.Resources.cekalogokirmizi;

            pictureBoxGeri.Enabled = false;
            pictureBoxIleri.Enabled = false;

            InitializeButtonGroups();
            SetupDynamicMenu();
        }

        private frmAnaSayfa() { }

        private void ShowCurrentDateTime()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm");
            lblSistemTarih.Text = currentDate;
            lblSistemSaat.Text = currentTime;
        }

        private void InitializeButtonGroups()
        {
            buttonGroups = new Dictionary<string, List<(string Text, Action ClickAction, bool Visible)>>
            {
                {
                    "Kesim İşlemleri", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Kesim Planı Ekle", () => btnKesimPlaniEkle_Click(null, null), false),
                        ("Kesim Yap", () => btnKesimYap_Click(null, null), false),
                        ("Yapılan Kesimleri Gör", () => btnYapilanKesimleriGor_Click(null, null), false),
                        ("Kesim Detayları", () => btnKesimDetaylari_Click(null, null), false),
                        ("Yerleşim Planı Bilgileri", () => btnYerlesimPlaniBilgileri_Click(null, null), false)
                    }
                },
                {
                    "Proje Finans", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Müşteriler", () => btnMusteriler_Click(null, null), false),
                        ("Proje Kütük", () => btnProjeKutuk_Click(null, null), false),
                        ("Proje Fiyatlandırma", () => btnProjeFiyatlandirma_Click(null, null), false),
                        ("Ödeme Şartları", () => btnOdemeSartlari_Click(null, null), false),
                        ("Ödeme Şartları Liste", () => btnOdemeSarlariListe_Click(null, null), false),
                        ("Teminat Mektupları", () => btnTeminatMektuplari_Click(null, null), false),
                        ("Sevkiyat", () => btnSevkiyat_Click(null, null), false),
                        ("Takip Takvimi", () => btnTakipTakvimi_Click(null, null), false)
                    }
                },
                {
                    "Proje Takip", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Proje Kartı", () => btnProjeKarti_Click(null, null), false),
                    }
                },
                {
                    "ERP", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Proje Öğeleri", () => btnProjeOgeleri_Click(null, null), false),
                        ("AutoCAD Aktarım", () => btnAutoCad_Click(null, null), false),
                        ("Karşılaştırma Tabloları", () => btnKarsilastirmaTablolari_Click(null, null), false)
                    }
                },
                {
                    "Sistem ve Ayarlar", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Kullanıcı Ayarları", () => btnKullaniciAyarlari_Click(null, null), false),
                        ("İletilen Sorunlar", () => btnIletilenSorunlar_Click(null, null), false),
                        ("Sistem Hareketleri", () => btnSistemHareketleri_Click(null, null), false),
                        ("Sistem Bilgisi", () => btnSistemBilgisiAnaSayfa_Click(null, null), false)
                    }
                },
                {
                    "Genel", new List<(string Text, Action ClickAction, bool Visible)>
                    {
                        ("Oturumu Kapat", () => btnOturumuKapat_Click(null, null), true)
                    }
                }
            };

            switch (aktifKullanici.kullaniciRol)
            {
                case "Yönetici":
                    SetButtonVisibility("Kesim İşlemleri", true);
                    SetButtonVisibility("Proje Finans", true);
                    SetButtonVisibility("ERP", true);
                    SetButtonVisibility("Sistem ve Ayarlar", true);
                    break;
                case "Destek":
                    SetButtonVisibility("Kesim İşlemleri", true);
                    SetButtonVisibility("Proje Finans", true);
                    SetButtonVisibility("ERP", true);
                    SetButtonVisibility("Sistem ve Ayarlar", true);
                    SetButtonVisibility("Proje Takip", true);
                    richTextBox1.Visible = true;
                    richTextBox2.Visible = true;
                    richTextBox3.Visible = true;
                    richTextBox4.Visible = true;
                    btnAktar.Visible = true;
                    break;
                case "İş Hazırlama":
                    SetButtonVisibility("Kesim İşlemleri", new List<string> { "Kesim Planı Ekle", "Kesim Yap", "Yapılan Kesimleri Gör", "Kesim Detayları", "Yerleşim Planı Bilgileri" });
                    break;
                case "Muhasebe":
                    SetButtonVisibility("Proje Finans", true);
                    break;
                case "Operatör":
                    SetButtonVisibility("Kesim İşlemleri", new List<string> { "Kesim Yap", "Yapılan Kesimleri Gör" });
                    break;
                case "Kullanıcı":
                    lblKullaniciBilgi.Visible = true;
                    break;
                case "Ressam":
                case "Erp":
                    SetButtonVisibility("ERP", true);
                    break;
            }
        }

        private void SetButtonVisibility(string groupName, bool visible)
        {
            if (buttonGroups.ContainsKey(groupName))
            {
                var buttons = buttonGroups[groupName];
                for (int i = 0; i < buttons.Count; i++)
                {
                    var button = buttons[i];
                    button.Visible = visible;
                    buttons[i] = button;
                }
                buttonGroups[groupName] = buttons;
            }
        }

        private void SetButtonVisibility(string groupName, List<string> buttonNames)
        {
            if (buttonGroups.ContainsKey(groupName))
            {
                var buttons = buttonGroups[groupName];
                for (int i = 0; i < buttons.Count; i++)
                {
                    var button = buttons[i];
                    button.Visible = buttonNames.Contains(button.Text);
                    buttons[i] = button;
                }
                buttonGroups[groupName] = buttons;
            }
        }

        private void SetupDynamicMenu()
        {
            panelAraYuz.Controls.Clear();
            panelAraYuz.AutoScroll = true;
            int y = 15;

            foreach (var group in buttonGroups)
            {
                if (group.Value.Any(b => b.Visible))
                {
                    var groupButton = new Button
                    {
                        Text = group.Key,
                        Size = new Size(220, 40),
                        Location = new Point(15, y),
                        BackColor = ColorTranslator.FromHtml("#34495E"),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    groupButton.Click += (s, e) => ShowSubMenu(group.Key);
                    panelAraYuz.Controls.Add(groupButton);
                    y += 50;
                }
            }

            var logoutButton = new Button
            {
                Text = "Oturumu Kapat",
                Size = new Size(220, 30),
                Location = new Point(15, y),
                BackColor = ColorTranslator.FromHtml("#E74C3C"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            logoutButton.Click += btnOturumuKapat_Click;
            panelAraYuz.Controls.Add(logoutButton);

            panelAraYuz.Refresh();
        }

        private void ShowSubMenu(string groupName)
        {
            panelAraYuz.Controls.Clear();
            panelAraYuz.AutoScroll = true;
            int y = 15;
            List<Button> subButtons = new List<Button>();

            foreach (var button in buttonGroups[groupName])
            {
                if (button.Visible)
                {
                    var subButton = new Button
                    {
                        Text = button.Text,
                        Size = new Size(220, 30),
                        Location = new Point(15, y),
                        FlatStyle = FlatStyle.Flat
                    };
                    ButonGenelHelper.StilUygula(subButton);
                    subButton.Click += (s, e) => button.ClickAction.Invoke();
                    subButtons.Add(subButton);
                    y += 35;
                }
            }

            var backButton = new Button
            {
                Text = "Geri",
                Size = new Size(220, 30),
                Location = new Point(15, y),
                BackColor = ColorTranslator.FromHtml("#7F8C8D"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            backButton.Click += (s, e) => SetupDynamicMenu();

            foreach (var subButton in subButtons)
            {
                panelAraYuz.Controls.Add(subButton);
            }

            panelAraYuz.Controls.Add(backButton);

            panelAraYuz.Refresh();
        }
        public void NavigateToUserControl(UserControl uc)
        {
            UserControlEkle(uc);
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            panelContainer.Size = new Size(1696, 197);
            MenuStripGenelHelper.StilUygula(menuStrip1);

            ButonGenelHelper.TuruncuZeminButonStilUygula(btnSistem);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnYardim);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnDuyuru);

            lblSistemKullanici.Text = aktifKullanici.kullaniciAdi;

            pictureBoxKullanici.Image = Properties.Resources.kullanici;
            pictureBoxKullanici.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxKullanici.Cursor = Cursors.Hand;

            pictureBoxAnaSayfa.Image = Properties.Resources.anasayfa;
            pictureBoxAnaSayfa.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAnaSayfa.Cursor = Cursors.Hand;

            pictureBoxGeri.Image = Properties.Resources.navigationBack;
            pictureBoxGeri.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxGeri.Cursor = Cursors.Hand;

            pictureBoxIleri.Image = Properties.Resources.navigationForward;
            pictureBoxIleri.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxIleri.Cursor = Cursors.Hand;

            panelNavigasyon.Padding = new Padding(5);
            panelAraYuz.Width = 250;

            var duyuruService = _serviceProvider.GetService<IDuyurularService>();
            Duyurular sonDuyuru = duyuruService.GetSonDuyuru();
            lblDuyuru.Text = sonDuyuru != null ? $"🗨️ {sonDuyuru.duyuru}" : "Henüz bir duyuru yok.";
            ctlBaslik1.Baslik = "Ana Sayfa";

            SetupDynamicMenu();
        }

        private void btnSistem_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelSistem);
        }

        private void btnOturumuKapat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Oturumu kapatmak için onaylıyor musunuz?",
                "Bilgi",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (result != DialogResult.OK)
                return;

            var scope = _serviceProvider.CreateScope();

            var kullaniciGirisi = scope.ServiceProvider.GetService<frmKullaniciGirisi>();
            if (kullaniciGirisi == null)
            {
                kullaniciGirisi = ActivatorUtilities.CreateInstance<frmKullaniciGirisi>(scope.ServiceProvider);
            }

            kullaniciGirisi.FormClosed += (s, args) =>
            {
                scope.Dispose();
                Application.Exit();
            };

            kullaniciGirisi.Show();
            this.Hide();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnYardim_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelYardim);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerCounter++;
            lblTimer.Text = TimeSpan.FromSeconds(timerCounter).ToString(@"hh\:mm\:ss") + "\n";
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSorun.Text))
            {
                MessageBox.Show("Bildiriyi doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Bildiriyi göndermek istiyor musunuz?", "Onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string olusturan = lblSistemKullanici.Text;
                string sorun = txtSorun.Text;
                DateTime sistemTarihSaat = DateTime.Parse($"{lblSistemTarih.Text} {lblSistemSaat.Text}");

                var sorunService = _serviceProvider.GetService<ISorunBildirimleriService>();

                if (sorunService.SorunBildirimEkle(olusturan, sorun, sistemTarihSaat))
                {
                    MessageBox.Show("Bildiriminiz iletildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSorun.Clear();
                }
            }
            else
            {
                MessageBox.Show("Gönderim iptal edildi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UserControlEkle(UserControl uc)
        {
            if (panelAnaSayfaContainer.Controls.Count > 0)
            {
                var mevcut = panelAnaSayfaContainer.Controls[0] as UserControl;
                if (mevcut != null)
                {
                    geriYigin.Push(mevcut);
                    pictureBoxGeri.Enabled = true;
                    Console.WriteLine($"Geri yığınına eklendi: {mevcut.GetType().Name}");
                }
            }

            panelAnaSayfaContainer.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(uc);
            ileriYigin.Clear();
            pictureBoxIleri.Enabled = false;

            Console.WriteLine($"Yeni UserControl eklendi: {uc.GetType().Name}");
        }

        private void pictureBoxGeri_Click(object sender, EventArgs e)
        {
            if (geriYigin.Count > 0)
            {
                var mevcut = panelAnaSayfaContainer.Controls[0] as UserControl;
                if (mevcut != null)
                {
                    ileriYigin.Push(mevcut);
                    Console.WriteLine($"İleri yığınına eklendi: {mevcut.GetType().Name}");
                }

                var onceki = geriYigin.Pop();
                panelAnaSayfaContainer.Controls.Clear();
                panelAnaSayfaContainer.Controls.Add(onceki);

                pictureBoxGeri.Enabled = geriYigin.Count > 0;
                pictureBoxIleri.Enabled = true;

                Console.WriteLine($"Geri gidildi: {onceki.GetType().Name}");
            }
        }

        private void pictureBoxIleri_Click(object sender, EventArgs e)
        {
            if (ileriYigin.Count > 0)
            {
                var mevcut = panelAnaSayfaContainer.Controls[0] as UserControl;
                if (mevcut != null)
                {
                    geriYigin.Push(mevcut);
                    Console.WriteLine($"Geri yığınına eklendi: {mevcut.GetType().Name}");
                }

                var sonraki = ileriYigin.Pop();
                panelAnaSayfaContainer.Controls.Clear();
                panelAnaSayfaContainer.Controls.Add(sonraki);

                pictureBoxIleri.Enabled = ileriYigin.Count > 0;
                pictureBoxGeri.Enabled = true;

                Console.WriteLine($"İleri gidildi: {sonraki.GetType().Name}");
            }
        }

        private void btnKesimYap_Click(object sender, EventArgs e)
        {
            var yeniKesimYap = _userControlFactory.CreateKesimYapControl();
            yeniKesimYap.FormKullaniciAdiGetir(KullaniciAdiInterface);
            UserControlEkle(yeniKesimYap);
        }

        private void yardımCubugunuKaldirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelYardimCubugu.Visible = !panelYardimCubugu.Visible;
            yardımCubugunuKaldirToolStripMenuItem.Text = panelYardimCubugu.Visible ? "Yardım çubuğu kapat" : "Yardım çubuğu aç";
            if (!panelYardimCubugu.Visible)
            {
                panelSistem.Visible = false;
                panelYardim.Visible = false;
            }
        }

        private void btnKesimPlaniEkle_Click(object sender, EventArgs e)
        {
            if (kesimPlaniEkle == null)
            {
                kesimPlaniEkle = _userControlFactory.CreateKesimPlaniEkleControl();
                kesimPlaniEkle.FormArayuzuAyarla(FormArayuzuInterface);
            }

            UserControlEkle(kesimPlaniEkle);
        }

        private void PanelGosterYardimMenu(Panel hedefPanel)
        {
            panelContainer.Visible = !hedefPanel.Visible;
            foreach (Control ctrl in panelContainer.Controls)
            {
                if (ctrl is Panel) ctrl.Visible = false;
            }
            hedefPanel.Visible = !hedefPanel.Visible;
        }

        private void btnYapilanKesimleriGor_Click(object sender, EventArgs e)
        {
            var yapilanKesimleriGor = _userControlFactory.CreateYapilanKesimleriGorControl();
            yapilanKesimleriGor.FormKullaniciAdiGetir(KullaniciAdiInterface);
            UserControlEkle(yapilanKesimleriGor);
        }

        private void btnKesimDetaylari_Click(object sender, EventArgs e)
        {
            var kesimDetaylarControl = _userControlFactory.CreateKesimDetaylariControl();
            UserControlEkle(kesimDetaylarControl);
        }

        private void btnIletilenSorunlar_Click(object sender, EventArgs e)
        {
            var sorunlar = _userControlFactory.CreateSorunlarService();
            UserControlEkle(sorunlar);
        }

        private void btnSistemHareketleri_Click(object sender, EventArgs e)
        {
            var sistemHareketleri = _userControlFactory.CreateSistemHareketleriService();
            UserControlEkle(sistemHareketleri);
        }

        private void btnKullaniciAyarlari_Click(object sender, EventArgs e)
        {
            if (kullaniciAyarlari == null)
            {
                kullaniciAyarlari = _userControlFactory.CreateKullaniciAyarlariService();
            }
            UserControlEkle(kullaniciAyarlari);
        }

        private void btnSistemBilgisiAnaSayfa_Click(object sender, EventArgs e)
        {
            if (sistemBilgisiAnaSayfa == null)
            {
                sistemBilgisiAnaSayfa = new ctlSistemBilgisi();
                sistemBilgisiAnaSayfa.FormArayuzuAyarla(FormArayuzuInterface);
            }
            UserControlEkle(sistemBilgisiAnaSayfa);
        }

        private void btnAutoCad_Click(object sender, EventArgs e)
        {
            if (autoCadAktarim == null)
            {
                autoCadAktarim = _userControlFactory.CreateAutoCadAktarimControl();
                autoCadAktarim.FormArayuzuAyarla(FormArayuzuInterface);
            }
            UserControlEkle(autoCadAktarim);
        }

        private void btnProjeOgeleri_Click(object sender, EventArgs e)
        {
            if (projeOgeleri == null)
            {
                projeOgeleri = _userControlFactory.CreateProjeOgeleriControl();
            }
            UserControlEkle(projeOgeleri);
        }

        private void btnKarsilastirmaTablolari_Click(object sender, EventArgs e)
        {
            if (karsilastirmaTablosu == null)
            {
                karsilastirmaTablosu = _userControlFactory.CreateKarsilastirmaTablosuService();
            }
            UserControlEkle(karsilastirmaTablosu);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var frm = ActivatorUtilities.CreateInstance<frmKullaniciAyarlari>(
                    scope.ServiceProvider,
                    aktifKullanici.kullaniciAdi
                );
                frm.ShowDialog();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            geriYigin.Clear();
            ileriYigin.Clear();
            pictureBoxGeri.Enabled = false;
            pictureBoxIleri.Enabled = false;
            if (panelAnaSayfa != null)
            {
                panelAnaSayfaContainer.Controls.Add(panelAnaSayfa);
                Console.WriteLine("Ana sayfa eklendi, gezinme geçmişine dahil değil.");
            }
        }

        private void btnDuyuru_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelDuyuru);
        }

        private void btnYayınla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextDuyuru.Text))
            {
                MessageBox.Show("Duyuru metni giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Duyuruyu yayınlamak istiyor musunuz?", "Onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string olusturan = lblSistemKullanici.Text;
                string duyuru = richTextDuyuru.Text;
                string tarihStr = lblSistemSaat.Text + " " + lblSistemTarih.Text;
                DateTime tarih = DateTime.ParseExact(tarihStr, "HH:mm yyyy-MM-dd", CultureInfo.InvariantCulture);
                var duyuruService = _serviceProvider.GetService<IDuyurularService>();
                if (duyuruService.DuyuruEkle(olusturan, duyuru, tarih))
                {
                    MessageBox.Show("Duyurunuz yayınlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextDuyuru.Clear();
                }
            }
            else
            {
                MessageBox.Show("Yayınlama iptal edildi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnProjeKutuk_Click(object sender, EventArgs e)
        {
            var projeKutukControl = _userControlFactory.CreateProjeKutukControl();
            UserControlEkle(projeKutukControl);
        }

        private void btnProjeFiyatlandirma_Click(object sender, EventArgs e)
        {
            var projeFiyatlandirmaControl = _userControlFactory.CreateProjeFiyatlandirmaControl();
            UserControlEkle(projeFiyatlandirmaControl);
        }

        private void btnYerlesimPlaniBilgileri_Click(object sender, EventArgs e)
        {
            var yerlesimPlaniBilgileri = _userControlFactory.CreateYerlesimPlaniBilgiControl();
            yerlesimPlaniBilgileri.FormKullaniciAdiGetir(KullaniciAdiInterface);
            UserControlEkle(yerlesimPlaniBilgileri);
        }

        private void btnMusteriler_Click(object sender, EventArgs e)
        {
            var musterilerControl = _userControlFactory.CreateMusterilerControl();
            UserControlEkle(musterilerControl);
        }

        private void btnOdemeSartlari_Click(object sender, EventArgs e)
        {
            var odemeSartlariControl = _userControlFactory.CreateOdemeSartlariControl();
            UserControlEkle(odemeSartlariControl);
        }

        private void btnTeminatMektuplari_Click(object sender, EventArgs e)
        {
            var teminatMektuplari = _userControlFactory.CreateTeminatMektuplariControl();
            UserControlEkle(teminatMektuplari);
        }

        private void btnOdemeSarlariListe_Click(object sender, EventArgs e)
        {
            var odemeSartlariListeControl = _userControlFactory.CreateOdemeSartlariListeControl();
            UserControlEkle(odemeSartlariListeControl);
        }

        private void btnSevkiyat_Click(object sender, EventArgs e)
        {
            var sevkiyatControl = _userControlFactory.CreateSevkiyatControl();
            UserControlEkle(sevkiyatControl);
        }

        private void btnTakipTakvimi_Click(object sender, EventArgs e)
        {
            var takipTakvimControl = _userControlFactory.CreateTakipTakvimiControl();
            UserControlEkle(takipTakvimControl);
        }

        private void btnProjeKarti_Click(object sender, EventArgs e)
        {
            var projeKartiControl = _userControlFactory.CreateProjeKartiControl();
            UserControlEkle(projeKartiControl);
        }
    }
}