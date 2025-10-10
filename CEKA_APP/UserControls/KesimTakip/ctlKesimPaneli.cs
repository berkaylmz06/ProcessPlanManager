using CEKA_APP.Abstracts;
using CEKA_APP.Forms.KesimTakip;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace CEKA_APP.UserControls.KesimTakip
{
    public partial class ctlKesimPaneli : UserControl
    {
        private Timer uiTimer;
        private Timer dbTimer;
        private DateTime startTime;
        private TimeSpan elapsedTime;
        private bool isTimerRunning;
        private Dictionary<string, int> _kesimIdToSureId; 
        public event EventHandler KesimTamamlandi;
        public bool IsTimerRunning => isTimerRunning;

        private IKullaniciAdiOgren _kullaniciAdi;
        public string KesimEmriNo { get => txtKesimPlaniNo.Text; set => txtKesimPlaniNo.Text = value; }
        public string LotNo { get => txtLotNo.Text; set => txtLotNo.Text = value; }
        public string OperatorAd { get => txtOperatorAd.Text; set => txtOperatorAd.Text = value; }

        private readonly IServiceProvider _serviceProvider;
        private IKesimSureService _kesimSureService => _serviceProvider.GetRequiredService<IKesimSureService>();
        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private IKesimTamamlanmisService _kesimTamamlanmisService => _serviceProvider.GetRequiredService<IKesimTamamlanmisService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();

        public ctlKesimPaneli(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();
            InitializeTimers();
            InitializeElapsedTimeLabel();

            _kesimIdToSureId = new Dictionary<string, int>();
            btnKesimDurdur.Enabled = false;
            btnKesimBitir.Enabled = false;
        }
        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }
        private void InitializeTimers()
        {
            uiTimer = new Timer();
            uiTimer.Interval = 1000;
            uiTimer.Tick += UiTimer_Tick;

            dbTimer = new Timer();
            dbTimer.Interval = 10000;
            dbTimer.Tick += DbTimer_Tick;

            elapsedTime = TimeSpan.Zero;
            isTimerRunning = false;
        }

        private void InitializeElapsedTimeLabel()
        {
            lblElapsedTime = new Label
            {
                Text = "Geçen Süre: 00:00:00",
                AutoSize = true
            };
            groupBox1.Controls.Add(lblElapsedTime);
        }

        private void btnKesimPlaniSec_Click(object sender, EventArgs e)
        {
            using (var form = new frmKesimPlaniSec(_serviceProvider))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    KesimEmriNo = form.SelectedKesimIds;
                    ProcessSelectedKesimPlans();
                }
            }
        }

        private void ProcessSelectedKesimPlans()
        {
            if (string.IsNullOrEmpty(KesimEmriNo))
            {
                MessageBox.Show("Hiçbir kesim planı seçilmedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (kesimIds.Count == 0)
            {
                MessageBox.Show("Geçerli kesim planı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Seçilen Kesim Planları: {string.Join(", ", kesimIds)}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKesimBaslat_Click(object sender, EventArgs e)
        {
            if (!isTimerRunning)
            {
                if (string.IsNullOrEmpty(KesimEmriNo))
                {
                    MessageBox.Show("Lütfen bir kesim planı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(LotNo))
                {
                    MessageBox.Show("Lütfen Lot No girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (kesimIds.Count == 0)
                {
                    MessageBox.Show("Geçerli kesim planı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _kesimIdToSureId.Clear();
                foreach (var kesimId in kesimIds)
                {
                    try
                    {
                        int sureId = _kesimSureService.Baslat(kesimId, OperatorAd);
                        _kesimIdToSureId[kesimId] = sureId;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kesim planı {kesimId} başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                startTime = DateTime.Now;
                uiTimer.Start();
                dbTimer.Start();
                isTimerRunning = true;

                btnKesimBaslat.Enabled = false;
                btnKesimDurdur.Enabled = true;
                btnKesimBitir.Enabled = true;
            }
        }

        private void btnKesimDurdur_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                uiTimer.Stop();
                dbTimer.Stop();
                elapsedTime += DateTime.Now - startTime;
                isTimerRunning = false;

                DialogResult result = MessageBox.Show("Kesim işlemine devam edilecek mi?", "Kesim Durdur", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    btnKesimBaslat.Enabled = true;
                    btnKesimDurdur.Enabled = false;
                    return;
                }
                else if (result == DialogResult.No)
                {
                    using (var form = new frmKesimIptal(_serviceProvider, KesimEmriNo, OperatorAd))
                    {
                        var dialogResult = form.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            string iptalNedeni = form.IptalNedeni;
                            var kesimAdetleri = form.KesimAdetleri;

                            int toplamSaniye = (int)elapsedTime.TotalSeconds;
                            int kesimSayisi = _kesimIdToSureId.Count;
                            int paylasilanSaniye = kesimSayisi > 0 ? toplamSaniye / kesimSayisi : 0;

                            DateTime currentDateTime = DateTime.Now;
                            DateTime tarih = currentDateTime.Date;
                            TimeSpan saat = currentDateTime.TimeOfDay;
                            string kesilenLot = LotNo.Trim();

                            var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            try
                            {
                                foreach (var kesimId in kesimIds)
                                {
                                    if (string.IsNullOrEmpty(kesimId) || kesimId == "0")
                                    {
                                        MessageBox.Show($"Kesim ID'si bulunamadı: {kesimId}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                    var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                                    if (dt.Rows.Count == 0)
                                    {
                                        MessageBox.Show($"KesimListesi tablosunda ilgili kesimId bulunamadı: {kesimId}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continue;
                                    }

                                    string olusturan = dt.Rows[0]["olusturan"]?.ToString();
                                    if (string.IsNullOrEmpty(olusturan))
                                    {
                                        MessageBox.Show($"Oluşturan bilgisi eksik: {kesimId}. Lütfen gerekli alanları doldurun.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        continue;
                                    }

                                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                                        Timeout = TimeSpan.FromMinutes(3)
                                    }))
                                    {
                                        StringBuilder pozVeSondurumMesaj = new StringBuilder();
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            string kalite = row["kalite"].ToString();
                                            string malzeme = row["malzeme"].ToString();
                                            string kalipNo = row["kalipNo"].ToString();
                                            string poz = row["kesilecekPozlar"].ToString();
                                            string proje = row["projeNo"].ToString();
                                            string pozKey = $"{kalite}-{malzeme}-{kalipNo}-{poz}-{proje}";

                                            if (!kesimAdetleri.TryGetValue(pozKey, out decimal sondurum))
                                            {
                                                sondurum = 0;
                                            }

                                            string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                                            if (string.IsNullOrEmpty(ifsKalite))
                                                throw new Exception($"Kalite kodu '{kalite}' için eşleşme bulunamadı.");

                                            string hataMesaji;
                                            string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                                            if (string.IsNullOrEmpty(ifsMalzeme))
                                                throw new Exception(hataMesaji);

                                            string[] pozParcalari = poz.Split('-');
                                            string pozIlkKisim = pozParcalari.Length > 0 ? pozParcalari[0] : poz;
                                            string kalipNoPoz = $"{kalipNo}-{pozIlkKisim}";
                                            string kalipNoPozForValidation = kalipNoPoz.Contains("-EK")
                                                ? kalipNoPoz.Substring(0, kalipNoPoz.IndexOf("-EK"))
                                                : kalipNoPoz;

                                            string pozbilgileri = $"{ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje}";
                                            pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Kesilen Adet: {sondurum}");

                                            if (!_kesimDetaylariService.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje))
                                                throw new Exception($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.");

                                            bool updateSuccess = _kesimDetaylariService.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, sondurum);
                                            if (!updateSuccess)
                                                throw new Exception($"Poz: {pozbilgileri} için kesilmisAdet güncellenemedi.");
                                        }

                                        string hata;
                                        bool paketSonuc = _kesimListesiPaketService.KesimListesiPaketKontrolluDusme(kesimId, 1, out hata);
                                        if (!paketSonuc)
                                            throw new Exception(hata);

                                        bool iptalSonuc = _kesimListesiPaketService.KesimListesiPaketIptalEt(kesimId, iptalNedeni);
                                        if (!iptalSonuc)
                                            throw new Exception($"Kesim planı {kesimId} için iptal işlemi başarısız.");

                                        bool sonuc1 = _kesimTamamlanmisService.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, 1, tarih, saat, kesilenLot);

                                        if (!sonuc1)
                                            throw new Exception("Kayıt işlemi sırasında hata oluştu.");

                                        int kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                                        _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniIptalEdildi", "Kesim Iptal",
                                            $"Kullanıcı {kesimId} numaralı kesim planını iptal etti. Iptal Nedeni: {iptalNedeni}, Kesilen Lot: {kesilenLot}");

                                        foreach (var kvp in _kesimIdToSureId.Where(kvp => kvp.Key == kesimId))
                                        {
                                            try
                                            {
                                                _kesimSureService.IptalEt(kvp.Value, paylasilanSaniye);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception($"Kesim planı {kvp.Key} iptal edilirken hata oluştu: {ex.Message}");
                                            }
                                        }

                                        scope.Complete();
                                    }
                                }

                                MessageBox.Show($"Kesim iptal edildi.\nIptal Nedeni: {iptalNedeni}\nToplam Geçen Süre: {elapsedTime:hh\\:mm\\:ss}", "Kesim Iptal Edildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Bir hata oluştu, işlem geri alındı: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            elapsedTime = TimeSpan.Zero;
                            lblElapsedTime.Text = "Geçen Süre: 00:00:00";
                            _kesimIdToSureId.Clear();

                            btnKesimBaslat.Enabled = true;
                            btnKesimDurdur.Enabled = false;
                            btnKesimBitir.Enabled = false;

                            KesimTamamlandi?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            uiTimer.Start();
                            dbTimer.Start();
                            startTime = DateTime.Now;
                            isTimerRunning = true;
                            btnKesimBaslat.Enabled = false;
                            btnKesimDurdur.Enabled = true;
                        }
                    }
                }
                else
                {
                    uiTimer.Start();
                    dbTimer.Start();
                    startTime = DateTime.Now;
                    isTimerRunning = true;
                    btnKesimBaslat.Enabled = false;
                    btnKesimDurdur.Enabled = true;
                }
            }
        }
        private void btnKesimBitir_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                uiTimer.Stop();
                dbTimer.Stop();
                elapsedTime += DateTime.Now - startTime;
                isTimerRunning = false;
            }

            int toplamSaniye = (int)elapsedTime.TotalSeconds;
            int kesimSayisi = _kesimIdToSureId.Count;
            int paylasilanSaniye = kesimSayisi > 0 ? toplamSaniye / kesimSayisi : 0;

            DateTime currentDateTime = DateTime.Now;
            DateTime tarih = currentDateTime.Date;
            TimeSpan saat = currentDateTime.TimeOfDay;
            string kesilenLot = LotNo.Trim();
            int carpan = 1;

            var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            try
            {
                foreach (var kesimId in kesimIds)
                {
                    if (string.IsNullOrEmpty(kesimId) || kesimId == "0")
                    {
                        MessageBox.Show($"Kesim ID'si bulunamadı: {kesimId}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show($"KesimListesi tablosunda ilgili kesimId bulunamadı: {kesimId}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    string olusturan = dt.Rows[0]["olusturan"]?.ToString();
                    if (string.IsNullOrEmpty(olusturan))
                    {
                        MessageBox.Show($"Oluşturan bilgisi eksik: {kesimId}. Lütfen gerekli alanları doldurun.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }

                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TimeSpan.FromMinutes(3)
                    }))
                    {
                        StringBuilder pozVeSondurumMesaj = new StringBuilder();
                        foreach (DataRow row in dt.Rows)
                        {
                            string kalite = row["kalite"].ToString();
                            string malzeme = row["malzeme"].ToString();
                            string kalipNo = row["kalipNo"].ToString();
                            string poz = row["kesilecekPozlar"].ToString();
                            string proje = row["projeNo"].ToString();
                            string adetSatır = row["kpAdetSayilari"].ToString();

                            string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                            if (string.IsNullOrEmpty(ifsKalite))
                                throw new Exception($"Kalite kodu '{kalite}' için eşleşme bulunamadı.");

                            string hataMesaji;
                            string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                            if (string.IsNullOrEmpty(ifsMalzeme))
                                throw new Exception(hataMesaji);

                            if (!decimal.TryParse(adetSatır, out decimal kpAdet))
                                throw new Exception("Veritabanındaki bazı adet değerleri geçerli değil.");

                            decimal sondurum = kpAdet * carpan;

                            string[] pozParcalari = poz.Split('-');
                            string pozIlkKisim = pozParcalari.Length > 0 ? pozParcalari[0] : poz;
                            string kalipNoPoz = $"{kalipNo}-{pozIlkKisim}";
                            string kalipNoPozForValidation = kalipNoPoz;

                            int tireSayisi = kalipNoPoz.Count(c => c == '-');
                            if (tireSayisi >= 3)
                            {
                                int ucuncuTireIndex = kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-') + 1) + 1);
                                kalipNoPozForValidation = kalipNoPoz.Substring(0, ucuncuTireIndex);
                            }

                            string pozbilgileri = $"{ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje}";
                            pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Sondurum: {sondurum}");

                            if (!_kesimDetaylariService.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje))
                                throw new Exception($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.");
                        }

                        string hata;
                        bool paketSonuc = _kesimListesiPaketService.KesimListesiPaketKontrolluDusme(kesimId, carpan, out hata);
                        if (!paketSonuc)
                            throw new Exception(hata);

                        foreach (DataRow row in dt.Rows)
                        {
                            string kalite = row["kalite"].ToString();
                            string malzeme = row["malzeme"].ToString();
                            string kalipNo = row["kalipNo"].ToString();
                            string poz = row["kesilecekPozlar"].ToString();
                            string proje = row["projeNo"].ToString();
                            string adetSatır = row["kpAdetSayilari"].ToString();

                            string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                            string hataMesaji;
                            string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                            decimal kpAdet = decimal.Parse(adetSatır);
                            decimal sondurum = kpAdet * carpan;

                            string[] pozParcalari = poz.Split('-');
                            string pozIlkKisim = pozParcalari.Length > 0 ? pozParcalari[0] : poz;
                            string kalipNoPoz = $"{kalipNo}-{pozIlkKisim}";
                            string kalipNoPozForValidation = kalipNoPoz.Contains("-EK")
                                ? kalipNoPoz.Substring(0, kalipNoPoz.IndexOf("-EK"))
                                : kalipNoPoz;

                            bool updateSuccess = _kesimDetaylariService.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, sondurum);
                            if (!updateSuccess)
                                throw new Exception($"Poz: {ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje} için kesilmisAdet güncellenemedi.");
                        }

                        bool sonuc1 = _kesimTamamlanmisService.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, carpan, tarih, saat, kesilenLot);

                        if (!sonuc1)
                            throw new Exception("Kayıt işlemi sırasında hata oluştu.");

                        int kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                        _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniKesildi", "Kesim Yap",
                            $"Kullanıcı {kesimId} numaralı kesim planının kesimini tamamladı. Kesilen Lot: {kesilenLot}");

                        scope.Complete();
                    }

                    foreach (var kvp in _kesimIdToSureId.Where(kvp => kvp.Key == kesimId))
                    {
                        try
                        {
                            _kesimSureService.Bitir(kvp.Value, paylasilanSaniye);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Kesim planı {kvp.Key} bitirilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                MessageBox.Show($"Kesim başarıyla tamamlandı.\nToplam Geçen Süre: {elapsedTime:hh\\:mm\\:ss}\nHer bir kesim planına paylaştırılan süre: {TimeSpan.FromSeconds(paylasilanSaniye):hh\\:mm\\:ss}", "Kesim Tamamlandı");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu, işlem geri alındı: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            elapsedTime = TimeSpan.Zero;
            lblElapsedTime.Text = "Geçen Süre: 00:00:00";
            _kesimIdToSureId.Clear();

            btnKesimBaslat.Enabled = true;
            btnKesimDurdur.Enabled = false;
            btnKesimBitir.Enabled = false;

            KesimTamamlandi?.Invoke(this, EventArgs.Empty);
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentElapsed = DateTime.Now - startTime + elapsedTime;
            lblElapsedTime.Text = $"Geçen Süre: {currentElapsed:hh\\:mm\\:ss}";
        }

        private void DbTimer_Tick(object sender, EventArgs e)
        {
            if (_kesimIdToSureId.Count > 0 && isTimerRunning)
            {
                int toplamSaniye = (int)((DateTime.Now - startTime + elapsedTime).TotalSeconds);
                int kesimSayisi = _kesimIdToSureId.Count;
                int paylasilanSaniye = kesimSayisi > 0 ? toplamSaniye / kesimSayisi : 0;

                foreach (var kvp in _kesimIdToSureId)
                {
                    try
                    {
                        _kesimSureService.GuncelleToplamSure(kvp.Value, paylasilanSaniye);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Kesim planı {kvp.Key} süre güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ctlKesimPaneli_Resize(object sender, EventArgs e)
        {
            int width = this.Width;
            int height = this.Height;

            int groupBoxWidth = (int)(width * 0.90);
            int groupBoxHeight = 220;

            int groupBoxX = (width - groupBoxWidth) / 2;
            int groupBoxY = (int)(height * 0.05);

            groupBox1.Size = new Size(groupBoxWidth, groupBoxHeight);
            groupBox1.Location = new Point(groupBoxX, groupBoxY);

            int innerPadding = 10;

            int btnSelectWidth = (int)(groupBoxWidth * 0.30);
            int btnSelectHeight = 56;

            btnKesimPlaniSec.Location = new Point(innerPadding, (groupBoxHeight - btnSelectHeight) / 2);
            btnKesimPlaniSec.Size = new Size(btnSelectWidth, btnSelectHeight);

            int labelStartX = btnKesimPlaniSec.Right + innerPadding * 2;

            int labelWidth = 145;

            int remainingWidth = groupBoxWidth - labelStartX - labelWidth - (innerPadding * 2);
            int textBoxWidth = remainingWidth;

            int verticalSpacing = 20;
            int firstLineY = 30;
            int textBoxYCorrection = 4;

            lblKesimEmriNo.Location = new Point(labelStartX, firstLineY);
            lblKesimEmriNo.Size = new Size(labelWidth, lblKesimEmriNo.Height);
            txtKesimPlaniNo.Location = new Point(lblKesimEmriNo.Right + innerPadding, firstLineY - textBoxYCorrection);
            txtKesimPlaniNo.Size = new Size(textBoxWidth, txtKesimPlaniNo.Height);

            int secondLineY = lblKesimEmriNo.Bottom + verticalSpacing;
            lblLotNo.Location = new Point(labelStartX, secondLineY);
            lblLotNo.Size = new Size(labelWidth, lblLotNo.Height);
            txtLotNo.Location = new Point(txtKesimPlaniNo.Location.X, secondLineY - textBoxYCorrection);
            txtLotNo.Size = new Size(textBoxWidth, txtLotNo.Height);

            int thirdLineY = lblLotNo.Bottom + verticalSpacing;
            lblOperatorAd.Location = new Point(labelStartX, thirdLineY);
            lblOperatorAd.Size = new Size(labelWidth, lblOperatorAd.Height);
            txtOperatorAd.Location = new Point(txtKesimPlaniNo.Location.X, thirdLineY - textBoxYCorrection);
            txtOperatorAd.Size = new Size(textBoxWidth, txtOperatorAd.Height);

            int fourthLineY = lblOperatorAd.Bottom + verticalSpacing;
            lblElapsedTime.Location = new Point(labelStartX, fourthLineY);

            int buttonWidth = width / 4;
            int buttonHeight = height / 3;

            int buttonY = groupBox1.Bottom + (height - groupBox1.Bottom - buttonHeight) / 2;

            int spacing = (int)(width - (3 * buttonWidth)) / 4;
            if (spacing < 5) spacing = 5;

            btnKesimBaslat.Location = new Point(spacing, buttonY);
            btnKesimBaslat.Size = new Size(buttonWidth, buttonHeight);

            btnKesimDurdur.Location = new Point(btnKesimBaslat.Right + spacing, buttonY);
            btnKesimDurdur.Size = new Size(buttonWidth, buttonHeight);

            btnKesimBitir.Location = new Point(btnKesimDurdur.Right + spacing, buttonY);
            btnKesimBitir.Size = new Size(buttonWidth, buttonHeight);
        }
    }
}