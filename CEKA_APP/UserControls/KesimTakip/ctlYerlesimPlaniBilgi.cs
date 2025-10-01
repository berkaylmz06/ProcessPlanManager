using CEKA_APP.Abstracts;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CEKA_APP.UsrControl
{
    public partial class ctlYerlesimPlaniBilgi : UserControl
    {
        private IKullaniciAdiOgren _kullaniciAdi;
        private List<string> hataMesajlari = new List<string>();

        private readonly IServiceProvider _serviceProvider;
        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();
        public ctlYerlesimPlaniBilgi(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            DataTable dt = _kesimListesiPaketService.GetKesimListesiPaket();

            dataGridKesimListesi.DataSource = dt;

            DataGridViewHelper.StilUygula(dataGridDetay);
            DataGridViewHelper.StilUygula(dataGridKesimListesi);
            tabloDuzenle();

            VerileriYukle();
            EkleSilmeButonlari();

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

        private void ctlYerlesimPlaniBilgi_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "Yerleşim Planı Bilgi";
            EkleSilmeButonlari();
        }

        public void FormKullaniciAdiGetir(IKullaniciAdiOgren kullaniciAdi)
        {
            _kullaniciAdi = kullaniciAdi;
        }

        private void EkleSilmeButonlari()
        {
            if (!dataGridKesimListesi.Columns.Contains("Sil"))
            {
                DataGridViewButtonColumn silButonu = new DataGridViewButtonColumn
                {
                    Name = "Sil",
                    HeaderText = "Sil",
                    Text = "Sil",
                    UseColumnTextForButtonValue = true
                };
                dataGridKesimListesi.Columns.Add(silButonu);
            }

            if (!dataGridDetay.Columns.Contains("Sil") && dataGridDetay.DataSource != null)
            {
                DataGridViewButtonColumn silButonu = new DataGridViewButtonColumn
                {
                    Name = "Sil",
                    HeaderText = "Sil",
                    Text = "Sil",
                    UseColumnTextForButtonValue = true
                };
                dataGridDetay.Columns.Add(silButonu);
            }
        }
        private void dataGridKesimListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridKesimListesi.Columns[e.ColumnIndex].Name == "Sil")
            {
                var kesimId = dataGridKesimListesi.Rows[e.RowIndex].Cells["kesimId"].Value?.ToString();
                if (kesimId != null)
                {
                    DialogResult result = MessageBox.Show(
                        $"Kesim ID: {kesimId} olan veri ve ilgili tüm kesim listesi verilerinin adetleri düşürülecek. Onaylıyor musunuz?",
                        "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        hataMesajlari.Clear();
                        bool tumDetaylarGuncellendi = true;

                        foreach (DataGridViewRow row in dataGridDetay.Rows)
                        {
                            string kalite = row.Cells["kalite"].Value?.ToString();
                            string malzeme = row.Cells["malzeme"].Value?.ToString();
                            string kalipNo = row.Cells["kalipNo"].Value?.ToString();
                            string kesilecekPozlar = row.Cells["kesilecekPozlar"].Value?.ToString();
                            string proje = row.Cells["projeNo"].Value?.ToString();
                            decimal silinecekAdet = 1;

                            if (kalite != null && malzeme != null && kalipNo != null && kesilecekPozlar != null && proje != null)
                            {
                                if (row.Cells["kpAdetSayilari"].Value != null && decimal.TryParse(row.Cells["kpAdetSayilari"].Value.ToString(), out decimal adet))
                                {
                                    silinecekAdet = adet > 0 ? adet : 1;
                                }
                                else
                                {
                                    hataMesajlari.Add($"Satır için kpAdetSayilari değeri geçersiz veya null: {row.Cells["kpAdetSayilari"].Value}");
                                    tumDetaylarGuncellendi = false;
                                    break;
                                }

                                string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                                if (string.IsNullOrEmpty(ifsKalite))
                                {
                                    hataMesajlari.Add($"Kalite '{kalite}' için eşleşme bulunamadı, orijinal değer '{kalite}' kullanılacak.");
                                    ifsKalite = kalite;
                                }

                                string hataMesaji;
                                string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                                if (string.IsNullOrEmpty(ifsMalzeme))
                                {
                                    hataMesajlari.Add(hataMesaji);
                                    tumDetaylarGuncellendi = false;
                                    break;
                                }

                                string kesilecekPozlarForValidation = kesilecekPozlar;
                                if (kesilecekPozlar.Contains("-"))
                                {
                                    kesilecekPozlarForValidation = kesilecekPozlar.Substring(0, kesilecekPozlar.IndexOf("-"));
                                }

                                if (ifsKalite != null && ifsMalzeme != null)
                                {
                                    if (!_kesimDetaylariService.GuncelleKesimDetaylari(ifsKalite, ifsMalzeme, kalipNo, kesilecekPozlarForValidation, proje, silinecekAdet, false))
                                    {
                                        hataMesajlari.Add($"Kesim detayı adet düşürme işlemi başarısız. {kesimId} numaralı yerleşim planında silinecek yeteri kadar poz bulunamadı.");
                                        tumDetaylarGuncellendi = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                hataMesajlari.Add("Eksik veri: Kalite, malzeme, kalıp no, kesilecek pozlar veya proje null.");
                                tumDetaylarGuncellendi = false;
                                break;
                            }
                        }

                        if (tumDetaylarGuncellendi && hataMesajlari.Count == 0)
                        {
                            bool paketSilindi = _kesimListesiPaketService.KesimListesiPaketSil(kesimId);
                            if (paketSilindi)
                            {
                                MessageBox.Show($"Kesim ID: {kesimId} olan veri ve ilgili tüm kesim detaylarının adetleri başarıyla düşürüldü ve tablodan silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                VerileriYukle();
                                dataGridDetay.DataSource = null;
                                dataGridDetay.Columns.Clear();

                                var kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                                _kullaniciHareketleriService.LogEkle(kullaniciId, "YerlesimPlaniSilindi", "Yerleşim Planı Bilgi", $"Kullanıcı {kesimId} numaralı yerleşim planını ve içeriğini tamamen sildi.");
                            }
                            else
                            {
                                hataMesajlari.Add("Paket silme işlemi başarısız oldu.");
                            }
                        }

                        if (hataMesajlari.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void dataGridDetay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridDetay.Columns[e.ColumnIndex].Name == "Sil")
            {
                var id = dataGridDetay.Rows[e.RowIndex].Cells["id"].Value?.ToString();
                if (id != null && int.TryParse(id, out int detayId))
                {
                    DialogResult result = MessageBox.Show($"Seçilen kesim detayı adetleri düşürülecek. Onaylıyor musunuz?", "Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        hataMesajlari.Clear();
                        string kalite = dataGridDetay.Rows[e.RowIndex].Cells["kalite"].Value?.ToString();
                        string malzeme = dataGridDetay.Rows[e.RowIndex].Cells["malzeme"].Value?.ToString();
                        string kalipNo = dataGridDetay.Rows[e.RowIndex].Cells["kalipNo"].Value?.ToString();
                        string kesilecekPozlar = dataGridDetay.Rows[e.RowIndex].Cells["kesilecekPozlar"].Value?.ToString();
                        string proje = dataGridDetay.Rows[e.RowIndex].Cells["projeNo"].Value?.ToString();
                        decimal silinecekAdet = 1;

                        if (kalite != null && malzeme != null && kalipNo != null && kesilecekPozlar != null && proje != null)
                        {
                            if (dataGridDetay.Rows[e.RowIndex].Cells["kpAdetSayilari"].Value != null && decimal.TryParse(dataGridDetay.Rows[e.RowIndex].Cells["kpAdetSayilari"].Value.ToString(), out decimal adet))
                            {
                                silinecekAdet = adet > 0 ? adet : 1;
                            }
                            else
                            {
                                hataMesajlari.Add($"Seçilen satır için kpAdetSayilari değeri geçersiz veya null: {dataGridDetay.Rows[e.RowIndex].Cells["kpAdetSayilari"].Value}");
                            }


                            string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                            if (string.IsNullOrEmpty(ifsKalite))
                            {
                                hataMesajlari.Add($"Kalite '{kalite}' için eşleşme bulunamadı, orijinal değer '{kalite}' kullanılacak.");
                                ifsKalite = kalite;
                            }

                            string hataMesaji;
                            string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                            if (string.IsNullOrEmpty(ifsMalzeme))
                            {
                                hataMesajlari.Add(hataMesaji);
                            }

                            string kesilecekPozlarForValidation = kesilecekPozlar;
                            if (kesilecekPozlar.Contains("-"))
                            {
                                kesilecekPozlarForValidation = kesilecekPozlar.Substring(0, kesilecekPozlar.IndexOf("-"));
                            }

                            if (hataMesajlari.Count > 0)
                            {
                                MessageBox.Show(string.Join(Environment.NewLine, hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (ifsKalite != null && ifsMalzeme != null)
                            {
                                bool listeSilindi = _kesimListesiService.KesimListesiSil(detayId);
                                if (listeSilindi && _kesimDetaylariService.GuncelleKesimDetaylari(ifsKalite, ifsMalzeme, kalipNo, kesilecekPozlarForValidation, proje, silinecekAdet, false))
                                {
                                    MessageBox.Show($"Kesim detayında {silinecekAdet} adet düşürüldü.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    var kesimId = dataGridKesimListesi.CurrentRow?.Cells["kesimId"].Value?.ToString();
                                    if (kesimId != null)
                                    {
                                        YenileDetayTablosu(kesimId);

                                        var kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_kullaniciAdi.lblSistemKullaniciMetinAl());
                                        _kullaniciHareketleriService.LogEkle(kullaniciId, "YerlesimPlaniIcerigiSilindi", "Yerleşim Planı Bilgi",
                                            $"Kullanıcı {kesimId} numaralı kesimden içerik sildi. Kalite: {ifsKalite}, Malzeme: {ifsMalzeme}, MalzemeKod: {kalipNo}-{kesilecekPozlarForValidation}, Proje: {proje} Silinen Adet: {silinecekAdet}");
                                    }
                                }
                                else
                                {
                                    hataMesajlari.Add($"Kesim detayı satırı adet düşürme işlemi başarısız. Kalite: {ifsKalite}, Malzeme: {ifsMalzeme}, MalzemeKod: {kalipNo}-{kesilecekPozlarForValidation}, Silinecek Adet: {silinecekAdet.ToString("G29", System.Globalization.CultureInfo.InvariantCulture)}");
                                    MessageBox.Show(string.Join(Environment.NewLine, hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            hataMesajlari.Add("Eksik veri: Kalite, malzeme, kalıp no, kesilecek pozlar veya proje null.");
                            MessageBox.Show(string.Join(Environment.NewLine, hataMesajlari), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void dataGridKesimListesi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var satir = dataGridKesimListesi.Rows[e.RowIndex];
                if (satir.Cells["kesimId"].Value != null)
                {
                    string kesimId = satir.Cells["kesimId"].Value.ToString();
                    YenileDetayTablosu(kesimId);
                }
            }
        }

        private void YenileDetayTablosu(string kesimId)
        {
            try
            {
                var dt = _kesimListesiService.GetirKesimListesi(kesimId);

                int toplamPlanTekrari = 1;
                foreach (DataGridViewRow row in dataGridKesimListesi.Rows)
                {
                    if (row.Cells["kesimId"].Value?.ToString() == kesimId)
                    {
                        if (row.Cells["toplamPlanTekrari"].Value != null && int.TryParse(row.Cells["toplamPlanTekrari"].Value.ToString(), out int planTekrari))
                        {
                            toplamPlanTekrari = planTekrari;
                        }
                        break;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["kpAdetSayilari"] != null && int.TryParse(row["kpAdetSayilari"].ToString(), out int kpAdet))
                        {
                            row["kpAdetSayilari"] = kpAdet * toplamPlanTekrari;
                        }
                    }
                }

                dataGridDetay.Columns.Clear();
                dataGridDetay.DataSource = dt;

                if (dt != null && dt.Rows.Count > 0)
                {
                    Dictionary<string, string> kolonBasliklari = new Dictionary<string, string>
                    {
                        { "olusturan", "Planı Oluşturan" },
                        { "kesimId", "Kesim ID" },
                        { "projeNo", "Proje No" },
                        { "kalite", "Kalite" },
                        { "malzeme", "Malzeme" },
                        { "kalipNo", "Kalıp No" },
                        { "kesilenPozlar", "Kesilen Pozlar" },
                        { "kesilenPozAdetSayilari", "Kesilen Pozların Adet Sayıları" },
                        { "eklemeTarihi", "Ekleme Tarihi" },
                        { "kpAdetSayilari", "KP Adet Sayıları (Toplam)" }
                    };

                    foreach (var kolon in kolonBasliklari)
                    {
                        if (dataGridDetay.Columns.Contains(kolon.Key))
                        {
                            dataGridDetay.Columns[kolon.Key].HeaderText = kolon.Value;
                        }
                    }

                    if (dataGridDetay.Columns.Contains("id"))
                    {
                        dataGridDetay.Columns["id"].Visible = false;
                        dataGridDetay.Columns["id"].DisplayIndex = 0;
                    }

                    if (dataGridDetay.Columns.Contains("Sil"))
                    {
                        int silIndex = dataGridDetay.Columns.Count - 1;
                        dataGridDetay.Columns["Sil"].DisplayIndex = silIndex > 0 ? silIndex : 0;
                    }
                }
                else
                {
                    dataGridDetay.Columns.Clear();
                    dataGridDetay.DataSource = null;
                }

                EkleSilmeButonlari();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            EkleSilmeButonlari();
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

            if (dataGridKesimListesi.Columns.Contains("Sil"))
            {
                dataGridKesimListesi.Columns["Sil"].DisplayIndex = dataGridKesimListesi.Columns.Count - 1;
            }
        }
    }
}