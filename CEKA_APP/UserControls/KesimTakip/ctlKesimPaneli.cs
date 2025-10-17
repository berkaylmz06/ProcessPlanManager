using CEKA_APP.Abstracts;
using CEKA_APP.Forms.KesimTakip;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
        public string MalzemeEn { get => txtMalzemeEn.Text; set => txtMalzemeEn.Text = value; }
        public string MalzemeBoy { get => txtMalzemeBoy.Text; set => txtMalzemeBoy.Text = value; }


        private readonly IServiceProvider _serviceProvider;
        private IKesimSureService _kesimSureService => _serviceProvider.GetRequiredService<IKesimSureService>();
        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private IKesimTamamlanmisService _kesimTamamlanmisService => _serviceProvider.GetRequiredService<IKesimTamamlanmisService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();
        private IAutoCadAktarimService _autoCadAktarimService => _serviceProvider.GetRequiredService<IAutoCadAktarimService>();

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
        private ctlKesimYonetimi GetParentKesimYonetimi()
        {
            Control parentControl = this.Parent;
            while (parentControl != null && !(parentControl is ctlKesimYonetimi))
            {
                parentControl = parentControl.Parent;
            }
            return parentControl as ctlKesimYonetimi;
        }
        private void btnKesimPlaniSec_Click(object sender, EventArgs e)
        {
            ctlKesimYonetimi parentYonetim = GetParentKesimYonetimi();
            List<string> usedKesimIds = parentYonetim?.GetirKullanilanKesimIds() ?? new List<string>();

            if (!string.IsNullOrEmpty(KesimEmriNo))
            {
                var currentPanelIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                usedKesimIds = usedKesimIds.Except(currentPanelIds).ToList();
            }

            using (var form = new frmKesimPlaniSec(_serviceProvider, usedKesimIds))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    KesimEmriNo = form.SelectedKesimIds;
                    LotNo = form.SelectedLotNo;
                    MalzemeEn = form.SelectedEn;
                    MalzemeBoy = form.SelectedBoy;

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

                bool mustStartNew = _kesimIdToSureId.Count == 0 || !_kesimIdToSureId.Any(kvp => kvp.Value > 0);

                if (mustStartNew)
                {
                    _kesimIdToSureId.Clear();
                    foreach (var kesimId in kesimIds)
                    {
                        try
                        {
                            int sureId = _kesimSureService.Baslat(kesimId, OperatorAd, LotNo);
                            _kesimIdToSureId[kesimId] = sureId;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Kesim planı {kesimId} başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
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

                int toplamSaniye = (int)elapsedTime.TotalSeconds;
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

                DialogResult result = MessageBox.Show("Kesim işlemine devam edilecek mi?", "Kesim Durdur", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                bool resumeTimer = false;

                if (result == DialogResult.Yes)
                {
                    btnKesimBaslat.Enabled = true;
                    btnKesimDurdur.Enabled = false;
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
                            var yanUrunVerileriByKesimId = form.YanUrunVerileri; 

                            string kesilenLot = LotNo.Trim().Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? LotNo.Trim();
                            int carpan = 1;

                            var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            try
                            {
                                string malzemeEnStr = MalzemeEn.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                                string malzemeBoyStr = MalzemeBoy.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                                if (!int.TryParse(malzemeEnStr, out int mevcutMalzemeEnInt))
                                {
                                    mevcutMalzemeEnInt = 0;
                                }

                                if (!int.TryParse(malzemeBoyStr, out int mevcutMalzemeBoyInt))
                                {
                                    mevcutMalzemeBoyInt = 0;
                                }

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

                                            string adetSatır = row["kpAdetSayilari"].ToString();
                                            string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                                            if (string.IsNullOrEmpty(ifsKalite)) throw new Exception($"Kalite kodu '{kalite}' için eşleşme bulunamadı.");
                                            string hataMesaji;
                                            string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                                            if (string.IsNullOrEmpty(ifsMalzeme)) throw new Exception(hataMesaji);
                                            if (!decimal.TryParse(adetSatır, out decimal kpAdet)) throw new Exception("Veritabanındaki bazı adet değerleri geçerli değil.");
                                            decimal gerceklesecekSondurum = sondurum * carpan;
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
                                            pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Kesilen Adet: {gerceklesecekSondurum}");
                                            if (!_kesimDetaylariService.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje)) throw new Exception($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.");
                                            bool updateSuccess = _kesimDetaylariService.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, gerceklesecekSondurum);
                                            if (!updateSuccess) throw new Exception($"Poz: {pozbilgileri} için kesilmisAdet güncellenemedi.");
                                        }

                                        bool iptalSonuc = _kesimListesiPaketService.KesimListesiPaketIptalEt(kesimId, iptalNedeni);
                                        if (!iptalSonuc) throw new Exception($"Kesim planı {kesimId} için iptal işlemi başarısız.");

                                        int kesimTamamlanmisId = _kesimTamamlanmisService.TablodanKesimTamamlanmisEkleme(
                                            olusturan,
                                            kesimId,
                                            carpan,
                                            kesilenLot,
                                            mevcutMalzemeEnInt,
                                            mevcutMalzemeBoyInt);

                                        if (kesimTamamlanmisId <= 0) throw new Exception("Kayıt işlemi sırasında KesimTamamlanmisId alınamadı.");

                                        List<YanUrunDetay> yanUrunDetaylari = yanUrunVerileriByKesimId.ContainsKey(kesimId) ? yanUrunVerileriByKesimId[kesimId] : new List<YanUrunDetay>();
                                        foreach (var detay in yanUrunDetaylari)
                                        {
                                            bool yanUrunKayitBasarili = _kesimTamamlanmisService.YanUrunDetayEkleme(
                                                kesimTamamlanmisId,
                                                detay.En,
                                                detay.Boy,
                                                detay.Adet);

                                            if (!yanUrunKayitBasarili)
                                                throw new Exception($"Kesim Tamamlanmış ID: {kesimTamamlanmisId} için yan ürün detayı ({detay.En}x{detay.Boy} - {detay.Adet} adet) kaydedilemedi.");
                                        }

                                        foreach (var kvp in _kesimIdToSureId.Where(kvp => kvp.Key == kesimId))
                                        {
                                            try
                                            {
                                                _kesimSureService.Bitir(kvp.Value, paylasilanSaniye);
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception($"Kesim planı {kvp.Key} süre bitirilirken hata oluştu: {ex.Message}");
                                            }
                                        }

                                        int kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                                        string yanUrunDurumu = yanUrunDetaylari.Any() ? $"Girildi ({yanUrunDetaylari.Count} adet kayıt)" : "Girilmedi";
                                        _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniIptalEdildi", "Kesim İptal Edildi", $"Kullanıcı {kesimId} numaralı kesim planını iptal etti. İptal Nedeni: {iptalNedeni}. Yan Ürün Durumu: {yanUrunDurumu}");

                                        scope.Complete();

                                        lblElapsedTime.Text = "Geçen Süre: 00:00:00";
                                        elapsedTime = TimeSpan.Zero;
                                        _kesimIdToSureId.Clear();
                                        btnKesimBaslat.Enabled = true;
                                        btnKesimDurdur.Enabled = false;
                                        btnKesimBitir.Enabled = false;
                                        KesimEmriNo = string.Empty;
                                        LotNo = string.Empty;
                                        MalzemeEn = string.Empty;
                                        MalzemeBoy = string.Empty;
                                        KesimTamamlandi?.Invoke(this, EventArgs.Empty);
                                    }
                                }

                                MessageBox.Show("Kesim başarıyla iptal edildi, adetler ve yan ürün bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Kesim iptal, adet güncelleme veya yan ürün kaydetme işleminde hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            resumeTimer = true;
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    resumeTimer = true;
                }

                if (resumeTimer)
                {
                    startTime = DateTime.Now;
                    uiTimer.Start();
                    dbTimer.Start();
                    isTimerRunning = true;

                    btnKesimBaslat.Enabled = false;
                    btnKesimDurdur.Enabled = true;
                    btnKesimBitir.Enabled = true;
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

            string kesilenLot = LotNo.Trim().Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? LotNo.Trim();
            int carpan = 1;
            var kesimIds = KesimEmriNo.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Dictionary<string, List<YanUrunDetay>> yanUrunVerileriByKesimId = new Dictionary<string, List<YanUrunDetay>>();
            string yanUrunDurumuGenel = "Girilmedi";

            if (kesimIds.Count > 0)
            {
                DialogResult yanUrunSoru = MessageBox.Show(
                    $"Kesim işlemi {kesimIds.Count} plan ile tamamlandı. Yan ürün bilgisi girilecek mi?",
                    "Kesim Bitir",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (yanUrunSoru == DialogResult.Yes)
                {
                    using (var yanUrunForm = new frmYanUrunGiris(_serviceProvider, kesimIds))
                    {
                        if (yanUrunForm.ShowDialog() == DialogResult.OK)
                        {
                            yanUrunVerileriByKesimId = yanUrunForm.YanUrunVerileriByKesimId;
                            int toplamYanUrunAdet = yanUrunVerileriByKesimId.Sum(kvp => kvp.Value?.Count ?? 0);
                            yanUrunDurumuGenel = $"Girildi ({toplamYanUrunAdet} adet kayıt)";

                            foreach (var id in kesimIds.Where(id => !yanUrunVerileriByKesimId.ContainsKey(id)))
                            {
                                yanUrunVerileriByKesimId.Add(id, new List<YanUrunDetay>());
                            }
                        }
                        else
                        {
                            foreach (var id in kesimIds)
                            {
                                yanUrunVerileriByKesimId.Add(id, new List<YanUrunDetay>());
                            }
                            yanUrunDurumuGenel = "Girilmedi (İptal edildi)";
                        }
                    }
                }
                else
                {
                    foreach (var id in kesimIds)
                    {
                        yanUrunVerileriByKesimId.Add(id, new List<YanUrunDetay>());
                    }
                }
            }

            int toplamSaniye = (int)elapsedTime.TotalSeconds;
            int kesimSayisi = _kesimIdToSureId.Count;
            int paylasilanSaniye = kesimSayisi > 0 ? toplamSaniye / kesimSayisi : 0;

            try
            {
                string malzemeEnStr = MalzemeEn.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                string malzemeBoyStr = MalzemeBoy.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                if (!int.TryParse(malzemeEnStr, out int mevcutMalzemeEnInt))
                {
                    mevcutMalzemeEnInt = 0;
                }

                if (!int.TryParse(malzemeBoyStr, out int mevcutMalzemeBoyInt))
                {
                    mevcutMalzemeBoyInt = 0;
                }

                foreach (var kesimId in kesimIds)
                {
                    if (string.IsNullOrEmpty(kesimId) || kesimId == "0")
                    {
                        MessageBox.Show($"Kesim ID'si bulunamadı: {kesimId}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    List<YanUrunDetay> yanUrunDetaylari = yanUrunVerileriByKesimId.ContainsKey(kesimId) ? yanUrunVerileriByKesimId[kesimId] : new List<YanUrunDetay>();

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
                            pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Kesilen Adet: {sondurum}");

                            if (!_kesimDetaylariService.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje))
                                throw new Exception($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.");

                            bool updateSuccess = _kesimDetaylariService.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, sondurum);
                            if (!updateSuccess)
                                throw new Exception($"Poz: {pozbilgileri} için kesilmisAdet güncellenemedi.");
                        }

                        string hata;
                        bool paketSonuc = _kesimListesiPaketService.KesimListesiPaketKontrolluDusme(kesimId, carpan, out hata);
                        if (!paketSonuc)
                            throw new Exception(hata);

                        int kesimTamamlanmisId = _kesimTamamlanmisService.TablodanKesimTamamlanmisEkleme(
                            olusturan,
                            kesimId,
                            carpan,
                            kesilenLot,
                            mevcutMalzemeEnInt,
                            mevcutMalzemeBoyInt);

                        if (kesimTamamlanmisId <= 0)
                            throw new Exception("Kayıt işlemi sırasında KesimTamamlanmisId alınamadı.");

                        foreach (var detay in yanUrunDetaylari)
                        {
                            bool yanUrunKayitBasarili = _kesimTamamlanmisService.YanUrunDetayEkleme(
                                kesimTamamlanmisId,
                                detay.En,
                                detay.Boy,
                                detay.Adet);

                            if (!yanUrunKayitBasarili)
                                throw new Exception($"Kesim Tamamlanmış ID: {kesimTamamlanmisId} için yan ürün detayı ({detay.En}x{detay.Boy} - {detay.Adet} adet) kaydedilemedi.");
                        }

                        int kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());

                        string logDetail = $"Kullanıcı {kesimId} numaralı kesim planını tamamladı. Kesilen Lot: {kesilenLot}. Yan Ürün Durumu: {yanUrunDurumuGenel}";
                        _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniTamamlandi", "Kesim Tamamlandı", logDetail);

                        foreach (var kvp in _kesimIdToSureId.Where(kvp => kvp.Key == kesimId))
                        {
                            try
                            {
                                _kesimSureService.Bitir(kvp.Value, paylasilanSaniye);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Kesim planı {kvp.Key} tamamlanırken hata oluştu: {ex.Message}");
                            }
                        }

                        scope.Complete();
                    }
                }

                const string YAZICI_ADI = "Argox OS-214 plus series PPLA";
                foreach (var kesimId in kesimIds)
                {
                    var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                    if (dt.Rows.Count == 0)
                        continue;

                    try
                    {
                        PrintDocument pdFeed = new PrintDocument();
                        pdFeed.PrinterSettings.PrinterName = YAZICI_ADI;
                        pdFeed.DefaultPageSettings.PaperSize = new PaperSize("Custom", MmToHundredthsInch(80), MmToHundredthsInch(40));
                        pdFeed.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                        pdFeed.PrintPage += (s, ev) => { ev.HasMorePages = false; };
                        pdFeed.Print();

                        List<(DataRow Row, int CopyIndex, int ToplamAdetIfs, int ToplamAdetYuklenen)> labelsToPrint = new List<(DataRow, int, int, int)>();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (decimal.TryParse(row["kpAdetSayilari"].ToString(), out decimal kpAdet))
                            {
                                int adet = (int)kpAdet;
                                string kalite = row["kalite"].ToString();
                                string malzeme = row["malzeme"].ToString();
                                string kalipNo = row["kalipNo"].ToString();
                                string poz = row["kesilecekPozlar"].ToString();
                                string proje = row["projeNo"].ToString();
                                string kalip = $"{kalipNo}-{poz}";
                                string hataMesaji;
                                string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                                if (string.IsNullOrEmpty(ifsKalite))
                                {
                                    MessageBox.Show($"Kalite '{kalite}' için eşleşme bulunamadı, hata mesajlarında orijinal değer kullanılacak.");
                                    ifsKalite = kalite;
                                }

                                string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                                if (string.IsNullOrEmpty(ifsMalzeme))
                                {
                                    MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) = _autoCadAktarimService.KontrolAdeta(
                                    ifsKalite,
                                    ifsMalzeme,
                                    kalip,
                                    proje,
                                    0
                                );

                                for (int i = 0; i < adet; i++)
                                {
                                    labelsToPrint.Add((row, i + 1, toplamAdetIfs, toplamAdetYuklenen));
                                }
                            }
                        }

                        int currentLabelIndex = 0;
                        int totalLabels = labelsToPrint.Count;

                        if (totalLabels > 0)
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = YAZICI_ADI;
                            pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", MmToHundredthsInch(80), MmToHundredthsInch(40));
                            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                            pd.PrintController = new StandardPrintController();

                            pd.PrintPage += (s, ev) =>
                            {
                                float mmToPx = ev.Graphics.DpiX / 25.4f;
                                float x = 2 * mmToPx;
                                float y = 3 * mmToPx;
                                float satirAraligi = 14;

                                var (row, copyIndex, toplamAdetIfs, toplamAdetYuklenen) = labelsToPrint[currentLabelIndex];
                                string adetFormatted = decimal.Parse(row["kpAdetSayilari"].ToString()).ToString("G29", System.Globalization.CultureInfo.CurrentCulture);

                                Font font = new Font("Arial", 10);
                                Brush brush = Brushes.Black;

                                ev.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                ev.Graphics.DrawString($"Kesim No: {kesimId}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Kalite: {row["kalite"]}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Malzeme: {row["malzeme"]}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Grup: {row["kalipNo"]}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Poz: {row["kesilecekPozlar"]}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Proje: {row["projeNo"]}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Adet: {adetFormatted} (Kopya: {copyIndex}/{adetFormatted})", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Operator: {OperatorAd}", font, brush, x, y);
                                y += satirAraligi;
                                ev.Graphics.DrawString($"Toplam Adet: {toplamAdetIfs}", font, brush, x, y);

                                Image logo = Properties.Resources.cekalogosiyah;
                                if (logo != null)
                                {
                                    int logoWidth = MmToHundredthsInch(15);
                                    int logoHeight = MmToHundredthsInch(10);
                                    int etiketGenislik = MmToHundredthsInch(80);
                                    int logoX = etiketGenislik - logoWidth - MmToHundredthsInch(5);
                                    int logoY = 5;
                                    ev.Graphics.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);
                                }
                                else
                                {
                                    int logoWidth = MmToHundredthsInch(15);
                                    int logoHeight = MmToHundredthsInch(15);
                                    int etiketGenislik = MmToHundredthsInch(80);
                                    int logoX = etiketGenislik - logoWidth - MmToHundredthsInch(15);
                                    int logoY = 5;
                                    ev.Graphics.DrawRectangle(Pens.Red, logoX, logoY, logoWidth, logoHeight);
                                    ev.Graphics.DrawString("Logo Yok", new Font("Arial", 6), Brushes.Red, logoX, logoY);
                                }

                                currentLabelIndex++;
                                ev.HasMorePages = currentLabelIndex < totalLabels;
                            };

                            pd.Print();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Yazdırma sırasında hata (Kesim ID: {kesimId}): {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (kesimIds.Any())
                {
                    MessageBox.Show("Etiketler basıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

            KesimEmriNo = string.Empty;
            LotNo = string.Empty;
            MalzemeEn = string.Empty;
            MalzemeBoy = string.Empty;

            KesimTamamlandi?.Invoke(this, EventArgs.Empty);
        }

        private int MmToHundredthsInch(int mm)
        {
            return (int)(mm * 100 / 25.4);
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
            int groupBoxHeight = 270;

            int groupBoxX = (width - groupBoxWidth) / 2;
            int groupBoxY = (int)(height * 0.05);

            groupBox1.Size = new Size(groupBoxWidth, groupBoxHeight);
            groupBox1.Location = new Point(groupBoxX, groupBoxY);

            int innerPadding = 10;

            int btnSelectWidth = (int)(groupBoxWidth * 0.30);
            int btnSelectHeight = 56;

            btnKesimPlaniSec.Location = new Point(innerPadding, (int)(groupBoxHeight / 2.0 - btnSelectHeight / 2.0));
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
            lblMalzemeEn.Location = new Point(labelStartX, thirdLineY);
            lblMalzemeEn.Size = new Size(labelWidth, lblMalzemeEn.Height);
            txtMalzemeEn.Location = new Point(txtKesimPlaniNo.Location.X, thirdLineY - textBoxYCorrection);
            txtMalzemeEn.Size = new Size(textBoxWidth, txtMalzemeEn.Height);

            int fourthLineY = lblMalzemeEn.Bottom + verticalSpacing;
            lblMalzemeBoy.Location = new Point(labelStartX, fourthLineY);
            lblMalzemeBoy.Size = new Size(labelWidth, lblMalzemeBoy.Height);
            txtMalzemeBoy.Location = new Point(txtKesimPlaniNo.Location.X, fourthLineY - textBoxYCorrection);
            txtMalzemeBoy.Size = new Size(textBoxWidth, txtMalzemeBoy.Height);

            int fifthLineY = lblMalzemeBoy.Bottom + verticalSpacing;
            lblOperatorAd.Location = new Point(labelStartX, fifthLineY);
            lblOperatorAd.Size = new Size(labelWidth, lblOperatorAd.Height);
            txtOperatorAd.Location = new Point(txtKesimPlaniNo.Location.X, fifthLineY - textBoxYCorrection);
            txtOperatorAd.Size = new Size(textBoxWidth, txtOperatorAd.Height);

            int sixthLineY = lblOperatorAd.Bottom + verticalSpacing;
            lblElapsedTime.Location = new Point(labelStartX, sixthLineY);

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

        public void LoadDevamEdenKesim(
           (string KesimId, string LotNo, int En, int Boy, int ToplamSureSaniye, string KesimYapan) kesimData)
        {
            KesimEmriNo = kesimData.KesimId;
            LotNo = kesimData.LotNo;
            MalzemeEn = kesimData.En.ToString();
            MalzemeBoy = kesimData.Boy.ToString();
            OperatorAd = kesimData.KesimYapan;

            elapsedTime = TimeSpan.FromSeconds(kesimData.ToplamSureSaniye);

            _kesimIdToSureId.Clear();

            try
            {
                int sureId = _kesimSureService.GetirSureId(kesimData.KesimId);
                _kesimIdToSureId[kesimData.KesimId] = sureId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Devam eden kesimin SureId'si alınırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            lblElapsedTime.Text = $"Geçen Süre: {elapsedTime:hh\\:mm\\:ss}";
            startTime = DateTime.Now;

            isTimerRunning = false;
            uiTimer.Stop();
            dbTimer.Stop();

            if (kesimData.ToplamSureSaniye > 0)
            {
                btnKesimBaslat.Enabled = true;
                btnKesimDurdur.Enabled = false;
                btnKesimBitir.Enabled = true;
            }
            else
            {
                btnKesimBaslat.Enabled = true;
                btnKesimDurdur.Enabled = false;
                btnKesimBitir.Enabled = false;
            }
        }
    }
}