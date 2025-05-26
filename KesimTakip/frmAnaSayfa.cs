using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iText.StyledXmlParser.Jsoup.Helper;
using KesimTakip.Abstracts;
using KesimTakip.Business;
using KesimTakip.Concretes;
using KesimTakip.DataBase;
using KesimTakip.Helper;
using KesimTakip.UsrControl;

namespace KesimTakip
{
    public partial class frmAnaSayfa : Form
    {
        private Timer timer;
        private int timerCounter = 0;
        public IFormArayuzu FormArayuzuInterface { get; private set; }
        public IKullaniciAdiOgren KullaniciAdiInterface { get; private set; }

        private ctlKesimPlaniEkle kesimPlaniEkle;
        private ctlSistemBilgisi sistemBilgisiAnaSayfa;

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

            panelAraYuz.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelYardimCubugu.BackColor = ColorTranslator.FromHtml("#E67E22");

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
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnSistem);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnYardim);
            MenuStripGenelHelper.StilUygula(menuStrip1);

            lblSistemKullanici.Text = aktifKullanici.adSoyad;

            btnKesimPlaniEkle.Visible = false;
            btnKesimYap.Visible = false;
            btnYapilanKesimleriGor.Visible = false;
            btnKesimDetaylari.Visible = false;
            btnKullaniciAyarlari.Visible = false;
            btnIletilenSorunlar.Visible = false;
            btnSistemHareketleri.Visible = false;
            lblKullaniciBilgi.Visible = false;
            btnSistemBilgisiAnaSayfa.Visible = false;

            switch (aktifKullanici.kullaniciRol)
            {
                case "Yönetici":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnKullaniciAyarlari.Visible = true;
                    btnSistemHareketleri.Visible = true;
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
                    break;

                case "İş Hazırlama":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    break;

                case "Muhasebe":
                    btnKesimPlaniEkle.Visible = true;
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    btnKesimDetaylari.Visible = true;
                    btnKullaniciAyarlari.Visible = true;
                    btnIletilenSorunlar.Visible = true;
                    btnSistemHareketleri.Visible = true;
                    break;

                case "Operatör":
                    btnKesimYap.Visible = true;
                    btnYapilanKesimleriGor.Visible = true;
                    break;

                case "Kullanıcı":
                    lblKullaniciBilgi.Visible = true;
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
              btnOturumuKapat
              );
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
            frmKullaniciGirisi kullanicigiris = new frmKullaniciGirisi();
            kullanicigiris.Show();

            this.Hide();

            kullanicigiris.FormClosed += (s, args) => Application.Exit();
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
                        MessageBox.Show("Bildiriminiz başarıyla iletildi.");
                        txtSorun.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Gönderim iptal edildi.");
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
    }
}
