using CEKA_APP.Abstracts;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CEKA_APP.UsrControl
{
    public partial class ctlAutoCadAktarim : UserControl
    {
        private List<AutoCadAktarim> tumParcalar;
        private IFormArayuzu _formArayuzu;
        private Guid _currentYuklemeId;

        private readonly IServiceProvider _serviceProvider;

        private IAutoCadAktarimService _autoCadAktarimService => _serviceProvider.GetRequiredService<IAutoCadAktarimService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();

        public ctlAutoCadAktarim(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            DataGridViewHelper.StilUygula(dataGridIslenmisXml);
            DataGridViewHelper.StilUygula(dataGridXmlCiktisi);
        }

        public void FormArayuzuAyarla(IFormArayuzu formArayuzu)
        {
            _formArayuzu = formArayuzu;
        }

        private void ctlAutoCadAktarim_Load(object sender, EventArgs e)
        {
            ctlBaslik1.Baslik = "AutoCad Aktarım";
            btnAktar.Enabled = false;
            _currentYuklemeId = Guid.NewGuid();
        }

        private void btnXmlDosyaSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML Dosyası|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string xmlPath = ofd.FileName;
                    try
                    {
                        tumParcalar = ParcalariOku(xmlPath);
                        dataGridXmlCiktisi.DataSource = tumParcalar;
                        btnAktar.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnAktar.Enabled = false;
                    }
                }
            }
            tabloDuzenle();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            string filtre = txtAra.Text.ToLower();

            if (tumParcalar == null)
            {
                MessageBox.Show("XML okutması yapılmamış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridXmlCiktisi.DataSource = null;
                btnAktar.Enabled = false;
                return;
            }

            if (string.IsNullOrEmpty(filtre))
            {
                dataGridXmlCiktisi.DataSource = tumParcalar;
            }
            else
            {
                var filtrelenmisParcalar = tumParcalar.Where(p =>
                    (p.Grup?.ToLower().Contains(filtre) ?? false) ||
                    (p.PozNo?.ToLower().Contains(filtre) ?? false) ||
                    (p.Ad?.ToLower().Contains(filtre) ?? false) ||
                    (p.Kalite?.ToLower().Contains(filtre) ?? false)
                ).ToList();
                dataGridXmlCiktisi.DataSource = filtrelenmisParcalar;
            }
        }

        private void OzetTabloyuGuncelle()
        {
            var guncelParcalar = dataGridXmlCiktisi.DataSource as List<AutoCadAktarim>;
            if (guncelParcalar == null) return;

            var project = txtProjeNo.Text.Trim();
            var projeKodu = string.IsNullOrEmpty(project) ? "Bilinmiyor" : project;

            List<string> hataMesajlari = new List<string>();
            var ozetParcalar = new List<AutoCadAktarimDetay>();

            var gruplar = guncelParcalar
                .GroupBy(p => p.Grup)
                .ToList();

            foreach (var grup in gruplar)
            {
                string grupNo = grup.Key;
                int grupAdet = grup.First().GrupAdet;
                decimal toplamNetAgirlik = grup.Sum(p => (decimal)(p.NetAgirlik ?? 0.0));

                ozetParcalar.Add(new AutoCadAktarimDetay
                {
                    Proje = projeKodu,
                    Grup = grupNo,
                    MalzemeKod = grupNo,
                    Adet = grupAdet,
                    MalzemeAd = "",
                    Kalite = "",
                    YuklemeId = _currentYuklemeId,
                    OrjinalAdet = grupAdet,
                    NetAgirlik = toplamNetAgirlik
                });

                var altParcalar = grup.Where(p => !string.IsNullOrEmpty(p.PozNo) && !string.IsNullOrEmpty(p.Ad) && !string.IsNullOrEmpty(p.Kalite))
                                     .ToList();

                foreach (var p in altParcalar)
                {
                    string formattedPozNo = p.PozNo.Length == 1 ? $"0{p.PozNo}" : p.PozNo;

                    string ifsMalzemeAd = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeMalzeme(p.Ad);
                    if (string.IsNullOrEmpty(ifsMalzemeAd))
                    {
                        ifsMalzemeAd = p.Ad;
                    }

                    ozetParcalar.Add(new AutoCadAktarimDetay
                    {
                        Proje = projeKodu,
                        Grup = p.Grup,
                        MalzemeKod = $"{p.Grup.Substring(0, 3)}-00-{formattedPozNo}",
                        Adet = p.Adet,
                        MalzemeAd = ifsMalzemeAd,
                        Kalite = p.Kalite,
                        YuklemeId = _currentYuklemeId,
                        OrjinalAdet = p.Adet,
                        NetAgirlik = (decimal)(p.NetAgirlik ?? 0.0),
                        GrupAdet = p.GrupAdet
                    });
                }
            }

            if (hataMesajlari.Count > 0)
            {
                string hataMesaji = "Aşağıdaki uyarılar bulundu:\n\n" + string.Join("\n", hataMesajlari);
                MessageBox.Show(hataMesaji, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnAktar.Enabled = false;
            }
            
            dataGridIslenmisXml.DataSource = ozetParcalar.OrderBy(p => p.Grup).ThenBy(p => string.IsNullOrEmpty(p.MalzemeAd) ? 0 : 1).ThenBy(p => p.MalzemeKod).ToList();
            tabloDuzenleIslenmisXml();
        }


        private List<AutoCadAktarim> ParcalariOku(string xmlPath)
        {
            var liste = new List<AutoCadAktarim>();
            var doc = XDocument.Load(xmlPath);

            foreach (var mainpart in doc.Descendants("mainpart"))
            {
                string grupNo = mainpart.Attribute("num")?.Value ?? "Bilinmiyor";
                int grupAdet = int.TryParse(mainpart.Attribute("quantity")?.Value, out int qty) ? qty : 1;

                liste.Add(new AutoCadAktarim
                {
                    Grup = grupNo,
                    PozNo = "",
                    Adet = grupAdet,
                    Ad = "",
                    Kalite = "",
                    NetAgirlik = null,
                    GrupAdet = grupAdet
                });

                foreach (var sp in mainpart.Descendants("singlepart"))
                {
                    var part = sp.Element("part");
                    if (part == null)
                    {
                        continue;
                    }

                    var parca = new AutoCadAktarim
                    {
                        Grup = grupNo,
                        PozNo = sp.Attribute("num")?.Value ?? "Bilinmiyor",
                        Adet = int.TryParse(sp.Attribute("quantity")?.Value, out int adet) ? adet : 1,
                        Ad = part.Attribute("name")?.Value ?? "Bilinmiyor",
                        Kalite = part.Element("material")?.Attribute("name")?.Value ?? "Bilinmiyor",
                        NetAgirlik = double.TryParse(part.Element("exactWeight")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double net) ? net / 1000.0 : (double?)null,
                        GrupAdet = grupAdet
                    };

                    liste.Add(parca);
                }
            }

            return liste;
        }

        private void btnHazirla_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjeNo.Text))
            {
                MessageBox.Show("Lütfen proje bilgisi giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnAktar.Enabled = false;
                return;
            }

            OzetTabloyuGuncelle();
            btnAktar.Enabled = true;
        }

        public void tabloDuzenle()
        {
            if (dataGridXmlCiktisi.Columns.Contains("GrupAdet"))
                dataGridXmlCiktisi.Columns["GrupAdet"].Visible = false;
            if (dataGridXmlCiktisi.Columns.Contains("GrupAdi"))
                dataGridXmlCiktisi.Columns["GrupAdi"].Visible = false;
            if (dataGridXmlCiktisi.Columns.Contains("MalzemeKod"))
                dataGridXmlCiktisi.Columns["MalzemeKod"].Visible = false;
            if (dataGridXmlCiktisi.Columns.Contains("YuklemeId"))
                dataGridXmlCiktisi.Columns["YuklemeId"].Visible = false;
        }

        public void tabloDuzenleIslenmisXml()
        {
            if (dataGridIslenmisXml.Columns.Contains("YuklemeId"))
                dataGridIslenmisXml.Columns["YuklemeId"].Visible = false;
            if (dataGridIslenmisXml.Columns.Contains("OrjinalAdet"))
                dataGridIslenmisXml.Columns["OrjinalAdet"].Visible = false;
            if (dataGridIslenmisXml.Columns.Contains("GrupAdet"))
                dataGridIslenmisXml.Columns["GrupAdet"].Visible = false;
            if (dataGridIslenmisXml.Columns.Contains("UstGrup"))
                dataGridIslenmisXml.Columns["UstGrup"].Visible = false;
            if (dataGridIslenmisXml.Columns.Contains("UstGrupId"))
                dataGridIslenmisXml.Columns["UstGrupId"].Visible = false;
            if (dataGridIslenmisXml.Columns.Contains("GrupId"))
                dataGridIslenmisXml.Columns["GrupId"].Visible = false;
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            if (dataGridIslenmisXml == null || dataGridIslenmisXml.Rows.Count == 0)
            {
                MessageBox.Show("Kaydedilecek veri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAktar.Enabled = false;
                return;
            }

            DialogResult result = MessageBox.Show("Verileri yüklemek istiyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            string proje = "";
            HashSet<string> uniqueGruplar = new HashSet<string>();
            List<string> basariMesajlari = new List<string>();
            List<string> hataMesajlari = new List<string>();

            foreach (DataGridViewRow row in dataGridIslenmisXml.Rows)
            {
                if (row.IsNewRow) continue;

                proje = row.Cells["Proje"].Value?.ToString()?.Trim() ?? "";
                string grup = row.Cells["Grup"].Value?.ToString()?.Trim() ?? "";
                string malzemeKod = row.Cells["MalzemeKod"].Value?.ToString()?.Trim() ?? "";
                int adet = 0;
                int.TryParse(row.Cells["Adet"].Value?.ToString(), out adet);
                string malzemeAd = row.Cells["MalzemeAd"].Value?.ToString()?.Trim() ?? "";
                string kalite = row.Cells["Kalite"].Value?.ToString()?.Trim() ?? "";
                decimal netAgirlik = 0m;
                if (decimal.TryParse(row.Cells["NetAgirlik"]?.Value?.ToString(), out decimal agirlik))
                {
                    netAgirlik = agirlik;
                }
                int grupAdet = Convert.ToInt32(row.Cells["GrupAdet"].Value ?? "1");
                int takimCarpani = grupAdet;

                bool isUstGrup = string.IsNullOrEmpty(malzemeAd) ||
                                (malzemeKod == grup && string.IsNullOrEmpty(kalite));

                if (!string.IsNullOrWhiteSpace(proje) && !string.IsNullOrWhiteSpace(grup) && adet > 0)
                {
                    try
                    {
                        if (isUstGrup)
                        {
                            _autoCadAktarimService.SaveAutoCadData(proje, grup, grup, adet, "", "", _currentYuklemeId, 0m, takimCarpani);
                            basariMesajlari.Add($"Üst grup '{grup}' kaydedildi.");
                        }
                        else
                        {
                            _autoCadAktarimService.SaveAutoCadData(proje, grup, malzemeKod, adet, malzemeAd, kalite, _currentYuklemeId, netAgirlik, takimCarpani);
                            basariMesajlari.Add($"Malzeme '{malzemeAd}' kaydedildi.");
                        }
                        uniqueGruplar.Add(grup);
                    }
                    catch (Exception ex)
                    {
                        string hataDetayi = isUstGrup ?
                            $"Üst grup '{grup}' kaydedilemedi: {ex.Message}" :
                            $"Malzeme '{malzemeAd}' kaydedilemedi: {ex.Message}";

                        hataMesajlari.Add(hataDetayi);
                        continue;
                    }
                }
            }
            string gruplar = string.Join(", ", uniqueGruplar);
            var kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_formArayuzu.lblSistemKullaniciMetinAl());

            string logMesaji = $"AutoCad Aktarım - Proje: {proje}, Gruplar: {gruplar}, Yükleme ID: {_currentYuklemeId}";
            if (hataMesajlari.Count > 0)
            {
                logMesaji += $", Hatalar: {hataMesajlari.Count} adet";
            }

            _kullaniciHareketleriService.LogEkle(kullaniciId, "PaftaYuklemesiYapildi", "AutoCad Aktarım", logMesaji);

            string sonucMesaji = $"İşlem tamamlandı. {basariMesajlari.Count} kayıt işlendi.";
            if (hataMesajlari.Count > 0)
            {
                sonucMesaji += $"\n{hataMesajlari.Count} hata oluştu:\n" + string.Join("\n", hataMesajlari.Take(3));
                if (hataMesajlari.Count > 3)
                {
                    sonucMesaji += $"\n... ve {hataMesajlari.Count - 3} hata daha";
                }
                MessageBox.Show(sonucMesaji, "Kısmi Başarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(sonucMesaji, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            _currentYuklemeId = Guid.NewGuid();
            btnAktar.Enabled = false;
        }
    }
}