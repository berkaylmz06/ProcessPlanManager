using CEKA_APP.Abstracts;
using CEKA_APP.Concretes;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.UsrControl;
using CEKA_APP.UsrControl.ProjeFinans;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        private ctlProjeKutuk projeKutuk;
        public ctlProjeFiyatlandirma projeFiyatlandirma;
        public ctlProjeBilgileri projeBilgileri;
        private ctlKullaniciAyarlari kullaniciAyarlari;

        public frmAnaSayfa(Kullanicilar kullanici)
        {
            InitializeComponent();

            aktifKullanici = kullanici ?? throw new ArgumentNullException(nameof(kullanici));
            FormArayuzuInterface = new FormArayuzu(this);
            KullaniciAdiInterface = new KullaniciAdiOgren(this);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            ShowCurrentDateTime();

            panelAraYuz.Dock = DockStyle.Left;
            panelAraYuz.Width = 150;
            panelSistem.Height = 300;
            panelYardim.Height = 300;
            panelDuyuru.Height = 300;

            panelAraYuz.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelYardimCubugu.BackColor = ColorTranslator.FromHtml("#E67E22");
            panelNavigasyon.BackColor = ColorTranslator.FromHtml("#E67E22");

            this.Icon = Properties.Resources.cekalogokirmizi;

            pictureBoxGeri.Enabled = false;
            pictureBoxIleri.Enabled = false;
        }

        private frmAnaSayfa() { }

        private void ShowCurrentDateTime()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm");
            lblSistemTarih.Text = currentDate;
            lblSistemSaat.Text = currentTime;
        }

        public void NavigateToUserControl(UserControl uc)
        {
            UserControlEkle(uc);
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            panelContainer.Size = new Size(1696, 197);

            ButonGenelHelper.StilUygula(btnKesimPlaniEkle);
            ButonGenelHelper.StilUygula(btnKesimYap);
            ButonGenelHelper.StilUygula(btnYapilanKesimleriGor);
            ButonGenelHelper.StilUygula(btnKesimDetaylari);
            ButonGenelHelper.StilUygula(btnKullaniciAyarlari);
            ButonGenelHelper.StilUygula(btnIletilenSorunlar);
            ButonGenelHelper.StilUygula(btnOturumuKapat);
            ButonGenelHelper.StilUygula(btnSistemHareketleri);
            ButonGenelHelper.StilUygula(btnSistemBilgisiAnaSayfa);
            ButonGenelHelper.StilUygula(btnAutoCad);
            ButonGenelHelper.StilUygula(btnProjeOgeleri);
            ButonGenelHelper.StilUygula(btnKarsilastirmaTablolari);
            ButonGenelHelper.StilUygula(btnProjeKutuk);
            ButonGenelHelper.StilUygula(btnProjeFiyatlandirma);
            ButonGenelHelper.StilUygula(btnYerlesimPlaniBilgileri);
            ButonGenelHelper.StilUygula(btnOdemeSartlari);
            ButonGenelHelper.StilUygula(btnTeminatMektuplari);
            ButonGenelHelper.StilUygula(btnMusteriler);
            ButonGenelHelper.StilUygula(btnOdemeSarlariListe);
            ButonGenelHelper.StilUygula(btnSevkiyat);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnSistem);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnYardim);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnDuyuru);
            MenuStripGenelHelper.StilUygula(menuStrip1);

            lblSistemKullanici.Text = aktifKullanici.kullaniciAdi;

            btnKesimPlaniEkle.Visible = false;
            btnKesimYap.Visible = false;
            btnYapilanKesimleriGor.Visible = false;
            btnKesimDetaylari.Visible = false;
            btnKullaniciAyarlari.Visible = false;
            btnIletilenSorunlar.Visible = false;
            btnSistemHareketleri.Visible = false;
            lblKullaniciBilgi.Visible = false;
            btnSistemBilgisiAnaSayfa.Visible = false;
            btnAutoCad.Visible = false;
            btnProjeOgeleri.Visible = false;
            btnKarsilastirmaTablolari.Visible = false;
            panelKesimPlaniEkleVeri.Visible = false;
            btnDuyuru.Visible = false;
            btnProjeKutuk.Visible = false;
            btnProjeFiyatlandirma.Visible = false;
            btnYerlesimPlaniBilgileri.Visible = false;
            btnOdemeSartlari.Visible = false;
            btnTeminatMektuplari.Visible = false;
            btnMusteriler.Visible = false;
            btnOdemeSarlariListe.Visible = false;
            btnSevkiyat.Visible = false;

            switch (aktifKullanici.kullaniciRol)
            {
                case "Yönetici":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnKullaniciAyarlari.Visible = true;
                    btnSistemHareketleri.Visible = true;
                    btnProjeOgeleri.Visible = true;
                    panelKesimPlaniEkleVeri.Visible = true;
                    btnKarsilastirmaTablolari.Visible = true;
                    btnDuyuru.Visible = true;
                    btnProjeKutuk.Visible = true;
                    btnProjeFiyatlandirma.Visible = true;
                    btnYerlesimPlaniBilgileri.Visible = true;
                    btnOdemeSartlari.Visible = true;
                    btnTeminatMektuplari.Visible = true;
                    btnMusteriler.Visible = true;
                    btnOdemeSarlariListe.Visible = true;
                    btnSevkiyat.Visible = true;
                    break;
                case "Destek":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnKullaniciAyarlari.Visible = true;
                    btnIletilenSorunlar.Visible = true;
                    btnSistemHareketleri.Visible = true;
                    btnSistemBilgisiAnaSayfa.Visible = true;
                    btnAutoCad.Visible = true;
                    btnProjeOgeleri.Visible = true;
                    panelKesimPlaniEkleVeri.Visible = true;
                    btnKarsilastirmaTablolari.Visible = true;
                    btnDuyuru.Visible = true;
                    btnProjeKutuk.Visible = true;
                    btnProjeFiyatlandirma.Visible = true;
                    btnYerlesimPlaniBilgileri.Visible = true;
                    btnOdemeSartlari.Visible = true;
                    btnTeminatMektuplari.Visible = true;
                    btnMusteriler.Visible = true;
                    btnOdemeSarlariListe.Visible = true;
                    btnSevkiyat.Visible = true;
                    break;
                case "İş Hazırlama":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    panelKesimPlaniEkleVeri.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnYerlesimPlaniBilgileri.Visible = true;
                    break;
                case "Muhasebe":
                    btnProjeKutuk.Visible = true;
                    btnProjeFiyatlandirma.Visible = true;
                    btnOdemeSartlari.Visible = true;
                    btnTeminatMektuplari.Visible = true;
                    btnMusteriler.Visible = true;
                    btnOdemeSarlariListe.Visible = true;
                    btnSevkiyat.Visible = true;
                    break;
                case "Operatör":
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    break;
                case "Kullanıcı":
                    lblKullaniciBilgi.Visible = true;
                    break;
                case "Ressam":
                    btnAutoCad.Visible = true;
                    btnProjeOgeleri.Visible = true;
                    break;
                case "Erp":
                    btnAutoCad.Visible = true;
                    btnProjeOgeleri.Visible = true;
                    break;

            }

            DuzenliButonGoster(
                panelAraYuz,
                btnKesimPlaniEkle,
                btnYerlesimPlaniBilgileri,
                btnKesimYap,
                btnYapilanKesimleriGor,
                btnKesimDetaylari,
                btnKullaniciAyarlari,
                btnIletilenSorunlar,
                btnSistemHareketleri,
                btnSistemBilgisiAnaSayfa,
                btnAutoCad,
                btnProjeOgeleri,
                btnKarsilastirmaTablolari,
                btnMusteriler,
                btnProjeKutuk,
                btnProjeFiyatlandirma,
                btnOdemeSartlari,
                btnTeminatMektuplari,
                btnOdemeSarlariListe,
                btnSevkiyat,
                btnOturumuKapat
            );

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
            panelAraYuz.Width = 170;

            DuyurularData duyurularData = new DuyurularData();
            Duyurular sonDuyuru = duyurularData.GetSonDuyuru();
            lblDuyuru.Text = sonDuyuru != null ? $"🗨️ {sonDuyuru.duyuru}" : "Henüz bir duyuru yok.";
            ctlBaslik1.Baslik = "Ana Sayfa";
        }
        private void btnSistem_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelSistem);
        }

        private void DuzenliButonGoster(Panel panel, params Button[] butonlar)
        {
            int y = 15;
            foreach (var btn in butonlar)
            {
                if (btn.Visible)
                {
                    int x = (panel.Width - btn.Width) / 2;
                    btn.Location = new Point(x, y);
                    y += btn.Height + 10;
                }
            }
        }

        private void btnOturumuKapat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Oturumu kapatmak için onaylıyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                frmKullaniciGirisi kullanicigiris = new frmKullaniciGirisi();
                kullanicigiris.FormClosed += (s, args) => Application.Exit();
                kullanicigiris.Show();
                this.Hide();
            }
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
                MessageBox.Show("Bildiri metnini doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Bildiriyi göndermek istiyor musunuz?", "Onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string olusturan = lblSistemKullanici.Text;
                string sorun = txtSorun.Text;
                string tarih = lblSistemSaat.Text + " " + lblSistemTarih.Text;

                SorunBildirimleriData datas = new SorunBildirimleriData();
                if (datas.SorunBildirimEkle(olusturan, sorun, tarih))
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
            var yeniKesimYap = new ctlKesimYap();
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
                kesimPlaniEkle = new ctlKesimPlaniEkle();
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
            var yapilanKesimleriGor = new ctlYapilanKesimleriGor();
            yapilanKesimleriGor.FormKullaniciAdiGetir(KullaniciAdiInterface);
            UserControlEkle(yapilanKesimleriGor);
        }

        private void btnKesimDetaylari_Click(object sender, EventArgs e)
        {
            var kesimDetaylari = new ctlKesimDetaylari();
            UserControlEkle(kesimDetaylari);
        }

        private void btnIletilenSorunlar_Click(object sender, EventArgs e)
        {
            var sorunlar = new ctlSorunlar();
            UserControlEkle(sorunlar);
        }

        private void btnSistemHareketleri_Click(object sender, EventArgs e)
        {
            var sistemHareketleri = new ctlSistemHareketleri();
            UserControlEkle(sistemHareketleri);
        }

        private void btnKullaniciAyarlari_Click(object sender, EventArgs e)
        {
            if (kullaniciAyarlari == null)
            {
                kullaniciAyarlari = new ctlKullaniciAyarlari();
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
                autoCadAktarim = new ctlAutoCadAktarim();
                autoCadAktarim.FormArayuzuAyarla(FormArayuzuInterface);
            }
            UserControlEkle(autoCadAktarim);
        }

        private void btnProjeOgeleri_Click(object sender, EventArgs e)
        {
            if (projeOgeleri == null)
            {
                projeOgeleri = new ctlProjeOgeleri();
            }
            UserControlEkle(projeOgeleri);
        }

        private void btnKarsilastirmaTablolari_Click(object sender, EventArgs e)
        {
            if (karsilastirmaTablosu == null)
            {
                karsilastirmaTablosu = new ctlKarsilastirmaTablosu();
            }
            UserControlEkle(karsilastirmaTablosu);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmKullaniciAyarlari kulEkle = new frmKullaniciAyarlari(lblSistemKullanici.Text);
            kulEkle.ShowDialog();
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
                Console.WriteLine("btnMusteriler eklendi, gezinme geçmişine dahil değil.");
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

                DuyurularData datas = new DuyurularData();
                if (datas.DuyuruEkle(olusturan, duyuru, tarih))
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
            if (projeKutuk == null)
            {
                projeKutuk = new ctlProjeKutuk();
            }
            UserControlEkle(projeKutuk);
        }

        private void btnProjeFiyatlandirma_Click(object sender, EventArgs e)
        {
            var projeFiyatlandirma = new ctlProjeFiyatlandirma();
            UserControlEkle(projeFiyatlandirma);
        }

        private void btnYerlesimPlaniBilgileri_Click(object sender, EventArgs e)
        {
            var yerlesimPlaniBilgileri = new ctlYerlesimPlaniBilgi();
            yerlesimPlaniBilgileri.FormKullaniciAdiGetir(KullaniciAdiInterface);
            UserControlEkle(yerlesimPlaniBilgileri);
        }

        private void btnMusteriler_Click(object sender, EventArgs e)
        {
            var musteriler = new ctlMusteriler();
            UserControlEkle(musteriler);
        }

        private void btnOdemeSartlari_Click(object sender, EventArgs e)
        {
            var projeFiyatlandirmaOdemeSartlari = new ctlOdemeSartlari();
            UserControlEkle(projeFiyatlandirmaOdemeSartlari);
        }

        private void btnTeminatMektuplari_Click(object sender, EventArgs e)
        {
            var teminatMektuplari = new ctlTeminatMektuplari();
            UserControlEkle(teminatMektuplari);
        }

        private void btnOdemeSarlariListe_Click(object sender, EventArgs e)
        {
            var odemeSartlariListe = new ctlOdemeSartlariListe();
            UserControlEkle(odemeSartlariListe);
        }

        private void btnSevkiyat_Click(object sender, EventArgs e)
        {
            var sevkiyat = new ctlSevkiyat();
            UserControlEkle(sevkiyat);
        }
    }
}