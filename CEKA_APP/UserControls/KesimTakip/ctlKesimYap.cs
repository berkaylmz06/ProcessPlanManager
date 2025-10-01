using CEKA_APP.Abstracts;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Genel;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKesimYap : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;

        private readonly IServiceProvider _serviceProvider;

        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private IKesimTamamlanmisService _kesimTamamlanmisService => _serviceProvider.GetRequiredService<IKesimTamamlanmisService>();
        private IKesimTamamlanmisHareketService _kesimTamamlanmisHareketService => _serviceProvider.GetRequiredService<IKesimTamamlanmisHareketService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();
        private ITabloFiltreleService _tabloFiltreleService => _serviceProvider.GetRequiredService<ITabloFiltreleService>();

        public ctlKesimYap(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataTable dt = _kesimListesiPaketService.GetKesimListesiPaket();

            dataGridKesimListesi.DataSource = dt;

            DataGridViewHelper.StilUygula(dataGridDetay);
            DataGridViewHelper.StilUygula(dataGridKesimListesi);
            tabloDuzenle();

            VerileriYukle();
        }

        private void ctlKesimYap_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Kesim Yap";
        }

        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }

        private void dataGridKesimListesi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var satir = dataGridKesimListesi.Rows[e.RowIndex];
                if (satir.Cells["kesimId"].Value != null)
                {
                    string kesimId = satir.Cells["kesimId"].Value.ToString();

                    try
                    {
                        var dt = _kesimListesiService.GetirKesimListesi(kesimId);

                        dataGridDetay.DataSource = dt;
                        dataGridDetay.Columns[0].Visible = false;
                        dataGridDetay.Columns[1].HeaderText = "Planı Oluşturan";
                        dataGridDetay.Columns[2].HeaderText = "Kesim ID";
                        dataGridDetay.Columns[3].HeaderText = "Proje No";
                        dataGridDetay.Columns[4].HeaderText = "Kalite";
                        dataGridDetay.Columns[5].HeaderText = "Malzeme";
                        dataGridDetay.Columns[6].HeaderText = "Kalıp No";
                        dataGridDetay.Columns[7].HeaderText = "Kesilen Pozlar";
                        dataGridDetay.Columns[8].HeaderText = "Kesilen Pozların Adet Sayıları";
                        dataGridDetay.Columns[9].HeaderText = "Ekleme Tarihi";

                        dataGridDetay.CellFormatting += (s, ev) =>
                        {
                            if (ev.ColumnIndex == dataGridDetay.Columns["kpAdetSayilari"].Index && ev.Value != null)
                            {
                                if (decimal.TryParse(ev.Value.ToString(), out decimal val))
                                {
                                    ev.Value = val.ToString("G29", System.Globalization.CultureInfo.CurrentCulture);
                                    ev.FormattingApplied = true;
                                }
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void VerileriYukle()
        {
            DataTable dt = _kesimListesiPaketService.GetKesimListesiPaket();

            dt.Columns.Add("Detay", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["Detay"] = "Detay Görmek İçin Tıklayınız.";
            }

            dataGridKesimListesi.DataSource = dt;
        }

        public void tabloDuzenle()
        {
            if (dataGridKesimListesi.Columns.Contains("id"))
                dataGridKesimListesi.Columns["id"].Visible = false;

            if (dataGridKesimListesi.Columns.Contains("olusturan"))
                dataGridKesimListesi.Columns["olusturan"].HeaderText = "Planı Oluşturan";

            if (dataGridKesimListesi.Columns.Contains("kesimId"))
                dataGridKesimListesi.Columns["kesimId"].HeaderText = "Kesim ID";

            if (dataGridKesimListesi.Columns.Contains("kesilecekPlanSayisi"))
                dataGridKesimListesi.Columns["kesilecekPlanSayisi"].HeaderText = "Kesilecek Plan Sayısı";

            if (dataGridKesimListesi.Columns.Contains("kesilmisPlanSayisi"))
                dataGridKesimListesi.Columns["kesilmisPlanSayisi"].HeaderText = "Kesilmiş Plan Sayısı";

            if (dataGridKesimListesi.Columns.Contains("toplamPlanTekrari"))
                dataGridKesimListesi.Columns["toplamPlanTekrari"].HeaderText = "Toplam Plan Tekrarı";

            if (dataGridKesimListesi.Columns.Contains("eklemeTarihi"))
                dataGridKesimListesi.Columns["eklemeTarihi"].HeaderText = "Ekleme Tarihi";
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string baseSql = _kesimListesiPaketService.GetKesimListesiPaketQuery();

            var frm = new frmAra(
                dataGridKesimListesi,
                _tabloFiltreleService,
                (dt) => { dataGridKesimListesi.DataSource = dt; },
                baseSql,
                _serviceProvider,
                detayEkle: true
            );

            frm.ShowDialog();
        }

        private void btnPaketKes_Click(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime tarih = currentDateTime.Date;
            TimeSpan saat = currentDateTime.TimeOfDay;

            if (dataGridKesimListesi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridKesimListesi.SelectedRows[0];

            try
            {
                string kesimId = selectedRow.Cells["kesimId"].Value?.ToString();
                string olusturan = selectedRow.Cells["olusturan"].Value?.ToString();
                string kesilenLot = txtKesilecekLot.Text.Trim();
                int carpan = 1;

                if (string.IsNullOrEmpty(olusturan))
                {
                    MessageBox.Show("Oluşturan bilgisi eksik. Lütfen gerekli alanları doldurun.", "Dikkat!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (string.IsNullOrEmpty(kesimId) || kesimId == "0")
                {
                    MessageBox.Show("Kesim ID'si bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dt = _kesimListesiService.GetirKesimListesi(kesimId);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("KesimListesi tablosunda ilgili kesimId bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                StringBuilder pozVeSondurumMesaj = new StringBuilder();
                StringBuilder hataAyrintilari = new StringBuilder();
                List<string> hataMesajlari = new List<string>();

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
                    {
                        MessageBox.Show($"Kalite kodu '{kalite}' için eşleşme bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hataMesaji;
                    string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                    if (string.IsNullOrEmpty(ifsMalzeme))
                    {
                        hataMesajlari.Add(hataMesaji);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(adetSatır, out decimal kpAdet))
                    {
                        MessageBox.Show("Veritabanındaki bazı adet değerleri geçerli değil.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    decimal sondurum = kpAdet * carpan;

                    string kalipNoPoz = $"{kalipNo}-{poz}";
                    string kalipNoPozForValidation = kalipNoPoz;

                    int tireSayisi = kalipNoPoz.Count(c => c == '-');
                    if (tireSayisi >= 3)
                    {
                        int ucuncuTireIndex = kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-', kalipNoPoz.IndexOf('-') + 1) + 1);
                        kalipNoPozForValidation = kalipNoPoz.Substring(0, ucuncuTireIndex);
                    }

                    string pozbilgileri = $"{ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje}";
                    pozVeSondurumMesaj.AppendLine($"Poz: {pozbilgileri}, Sondurum: {sondurum}");

                    hataAyrintilari.AppendLine($"Kontrol edilen pozbilgileri: {pozbilgileri}");

                    if (!_kesimDetaylariService.PozExists(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje))
                    {
                        MessageBox.Show($"Poz: {pozbilgileri} KesimDetaylari tablosunda bulunamadı.\nAyrıntılar:\n{hataAyrintilari.ToString()}",
                            "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string hata;
                bool paketSonuc = _kesimListesiPaketService.KesimListesiPaketKontrolluDusme(kesimId, carpan, out hata);
                if (!paketSonuc)
                {
                    MessageBox.Show(hata, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                    {
                        MessageBox.Show($"Kalite kodu '{kalite}' için eşleşme bulunamadı.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hataMesaji;
                    string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                    if (string.IsNullOrEmpty(ifsMalzeme))
                    {
                        hataMesajlari.Add(hataMesaji);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!decimal.TryParse(adetSatır, out decimal kpAdet))
                    {
                        MessageBox.Show("Veritabanındaki bazı adet değerleri geçerli değil.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    decimal sondurum = kpAdet * carpan;
                    string kalipNoPoz = $"{kalipNo}-{poz}";
                    string kalipNoPozForValidation = kalipNoPoz;
                    if (kalipNoPoz.Contains("-EK"))
                    {
                        kalipNoPozForValidation = kalipNoPoz.Substring(0, kalipNoPoz.IndexOf("-EK"));
                    }

                    bool updateSuccess = _kesimDetaylariService.UpdateKesilmisAdet(ifsKalite, ifsMalzeme, kalipNoPozForValidation, proje, sondurum);
                    if (!updateSuccess)
                    {
                        MessageBox.Show($"Poz: {ifsKalite}-{ifsMalzeme}-{kalipNoPozForValidation}-{proje} için kesilmisAdet veya kesilecekAdet güncellenemedi. Kesilecek adet yetersiz olabilir.\nSondurum: {sondurum}",
                            "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                bool sonuc1 = _kesimTamamlanmisService.TablodanKesimTamamlanmisEkleme(olusturan, kesimId, carpan, tarih, saat, kesilenLot);
                bool sonuc2 = _kesimTamamlanmisHareketService.TablodanKesimTamamlanmisHareketEkleme(olusturan, kesimId, carpan, tarih, saat);

                if (sonuc1 && sonuc2)
                {
                    int kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                    _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniKesildi", "Kesim Yap", $"Kullanıcı {kesimId} numaralı kesim planının kesimini tamamladı. Kesilen Lot: {kesilenLot}");
                }
                else
                {
                    MessageBox.Show("Kayıt işlemi sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Kesim başarıyla tamamlandı.",
                    "Başarılı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _kesimListesiPaketService.VerileriYenile(dataGridKesimListesi);
                tabloDuzenle();
                VerileriYukle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}