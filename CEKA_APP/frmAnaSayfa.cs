using CEKA_APP.Abstracts;
using CEKA_APP.Business;
using CEKA_APP.Concretes;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.UsrControl;
using iText.StyledXmlParser.Jsoup.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CEKA_APP
{
    public partial class frmAnaSayfa : Form
    {
        private Timer timer;
        private int timerCounter = 0;
        public IFormArayuzu FormArayuzuInterface { get; private set; }
        public IKullaniciAdiOgren KullaniciAdiInterface { get; private set; }

        private ctlKesimPlaniEkle kesimPlaniEkle;
        private ctlSistemBilgisi sistemBilgisiAnaSayfa;
        private ctlAutoCadAktarim autoCadAktarim;
        private ctlProjeOgeleri projeOgeleri;
        private ctlKarsilastirmaTablosu karsilastirmaTablosu;
        private ctlProjeKutuk projeKutuk;

        private Kullanicilar aktifKullanici;

        public frmAnaSayfa(Kullanicilar kullanici)
        {
            InitializeComponent();

            aktifKullanici = kullanici ?? throw new ArgumentNullException(nameof(kullanici));
            FormArayuzuInterface = new FormArayuzu(this);
            KullaniciAdiInterface = new KullaniciAdiOgren(this);

            aktifKullanici = kullanici;

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

            this.Icon = new Icon("cekalogokirmizi.ico");

        }
        frmAnaSayfa()
        {
        }
        private void ShowCurrentDateTime()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string currentTime = DateTime.Now.ToString("HH:mm");

            lblSistemTarih.Text = currentDate;
            lblSistemSaat.Text = currentTime;
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            panelContainer.Size = new System.Drawing.Size(1696, 197);

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

            switch (aktifKullanici.kullaniciRol)
            {
                case "Yönetici":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnKullaniciAyarlari.Visible = true;
                    btnSistemHareketleri.Visible = true;
                    panelKesimPlaniEkleVeri.Visible=true;
                    btnKarsilastirmaTablolari.Visible = true;
                    btnDuyuru.Visible=true;
                    btnProjeKutuk.Visible = true;
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
                    break;

                case "İş Hazırlama":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    panelKesimPlaniEkleVeri.Visible = true;
                    break;

                case "Muhasebe":
                    btnProjeKutuk.Visible = true;
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
            }
            DuzenliButonGoster
              (panelAraYuz,
              btnKesimPlaniEkle,
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
              btnProjeKutuk,
              btnOturumuKapat
              );


            pictureBox1.Image = Properties.Resources.kullanici;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; 
            pictureBox1.Cursor = Cursors.Hand;

            pictureBox2.Image = Properties.Resources.anasayfa;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Cursor = Cursors.Hand;

            panelAraYuz.Width = 170;

            DuyurularData duyurularData = new DuyurularData();
            Duyurular sonDuyuru = duyurularData.GetSonDuyuru();

            if (sonDuyuru != null)
            {
                lblDuyuru.Text = $"🗨️ {sonDuyuru.duyuru}";
            }
            else
            {
                lblDuyuru.Text = "Henüz bir duyuru yok.";
            }
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
            DialogResult result = MessageBox.Show("Oturumu kapatmak istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
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

            TimeSpan time = TimeSpan.FromSeconds(timerCounter);

            string timeFormatted = time.ToString(@"hh\:mm\:ss");

            lblTimer.Text = $"{timeFormatted}\n";
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSorun.Text))
            {
                MessageBox.Show("Bildiri göndermeden önce lütfen bildiri metnini doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show(
            "Bildiriyi göndermek istediğinize emin misiniz?",
            "Onayla",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

                if (result == DialogResult.Yes)
                {
                    string olusturan = lblSistemKullanici.Text;
                    string sorun = txtSorun.Text;
                    string tarih = lblSistemSaat.Text + " " + lblSistemTarih.Text;

                    SorunBildirimleriData datas = new SorunBildirimleriData();
                    bool basariliMi = datas.SorunBildirimEkle(olusturan, sorun, tarih);

                    if (basariliMi)
                    {
                        MessageBox.Show("Bildiriminiz başarıyla iletildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtSorun.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Gönderim iptal edildi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnKesimYap_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var kesimYap = new ctlKesimYap();
            kesimYap.FormKullaniciAdiGetir(KullaniciAdiInterface);
            kesimYap.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(kesimYap);
        }


        private void yardımCubugunuKaldirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelYardimCubugu.Visible = !panelYardimCubugu.Visible;

            if (panelYardimCubugu.Visible == true)
            {
                yardımCubugunuKaldirToolStripMenuItem.Text = "Yardım çubuğu kapat";
            }
            else
            {
                yardımCubugunuKaldirToolStripMenuItem.Text = "Yardım çubuğu aç";
                panelSistem.Visible = false;
                panelYardim.Visible = false;
            }
        }
        private void btnKesimPlaniEkle_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (kesimPlaniEkle == null)
            {
                kesimPlaniEkle = new ctlKesimPlaniEkle();
                kesimPlaniEkle.FormArayuzuAyarla(FormArayuzuInterface);
                kesimPlaniEkle.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(kesimPlaniEkle);
        }


        private void PanelGosterYardimMenu(Panel hedefPanel)
        {
            if (hedefPanel.Visible)
            {
                panelContainer.Visible = false;
                foreach (Control ctrl in panelContainer.Controls)
                {
                    if (ctrl is Panel)
                    {
                        ctrl.Visible = false;
                    }
                }
            }
            else
            {
                panelContainer.Visible = true;

                foreach (Control ctrl in panelContainer.Controls)
                {
                    if (ctrl is Panel)
                    {
                        ctrl.Visible = false;
                    }
                }
                hedefPanel.Visible = true;
            }
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
           
        }
        private void btnYapilanKesimleriGor_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var yapilanKesimleriGor = new ctlYapilanKesimleriGor();
            yapilanKesimleriGor.FormKullaniciAdiGetir(KullaniciAdiInterface);
            yapilanKesimleriGor.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(yapilanKesimleriGor);
        }

        private void bntKesimDetaylari_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var kesimDetaylari = new ctlKesimDetaylari();
            kesimDetaylari.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(kesimDetaylari);
        }

        private void btnIletilenSorunlar_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var sorunlar = new ctlSorunlar();
            sorunlar.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(sorunlar);
        }

        private void btnSistemHareketleri_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var sistemHareket = new ctlSistemHareketleri();
            sistemHareket.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(sistemHareket);
        }

        private void btnKullaniciAyarlari_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            var kullaniciAyar = new ctlKullaniciAyarlari();
            kullaniciAyar.Dock = DockStyle.Fill;
            panelAnaSayfaContainer.Controls.Add(kullaniciAyar);
        }

        private void btnSistemBilgisiAnaSayfa_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (sistemBilgisiAnaSayfa == null)
            {
                sistemBilgisiAnaSayfa = new ctlSistemBilgisi();
                sistemBilgisiAnaSayfa.FormArayuzuAyarla(FormArayuzuInterface);
                sistemBilgisiAnaSayfa.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(sistemBilgisiAnaSayfa);
        }

        private void btnAutoCad_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (autoCadAktarim == null)
            {
                autoCadAktarim = new ctlAutoCadAktarim();
                autoCadAktarim.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(autoCadAktarim);
        }

        private void btnProjeOgeleri_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (projeOgeleri == null)
            {
                projeOgeleri = new ctlProjeOgeleri();
                projeOgeleri.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(projeOgeleri);
        }

        private void btnKarsilastirmaTablolari_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (karsilastirmaTablosu == null)
            {
                karsilastirmaTablosu = new ctlKarsilastirmaTablosu();
                karsilastirmaTablosu.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(karsilastirmaTablosu);
        }

        private void btnKullaniciAyar_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmKullaniciBilgileri kulEkle = new frmKullaniciBilgileri(lblSistemKullanici.Text);
            kulEkle.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();
            panelAnaSayfaContainer.Controls.Add(panelAnaSayfaİcerik);
        }

        private void btnDuyuru_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelDuyuru);
        }

        private void btnYayınla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextDuyuru.Text))
            {
                MessageBox.Show("Lütfen önce duyuru metni giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show(
            "Duyuruyu yayınlamak istediğinize emin misiniz?",
            "Onayla",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

                if (result == DialogResult.Yes)
                {
                    string olusturan = lblSistemKullanici.Text;
                    string duyuru = richTextDuyuru.Text;
                    string tarihStr = lblSistemSaat.Text + " " + lblSistemTarih.Text; // "14:40 2025-06-16"
                    DateTime tarih = DateTime.ParseExact(tarihStr, "HH:mm yyyy-MM-dd", CultureInfo.InvariantCulture);


                    DuyurularData datas = new DuyurularData();
                    bool basariliMi = datas.DuyuruEkle(olusturan, duyuru, tarih);

                    if (basariliMi)
                    {
                        MessageBox.Show("Duyurunuz başarıyla yayınlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        richTextDuyuru.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Yayınlama iptal edildi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnProjeKutuk_Click(object sender, EventArgs e)
        {
            panelAnaSayfaContainer.Controls.Clear();

            if (projeKutuk == null)
            {
                projeKutuk = new ctlProjeKutuk();
                projeKutuk.Dock = DockStyle.Fill;
            }

            panelAnaSayfaContainer.Controls.Add(projeKutuk);
        }
    }
}
