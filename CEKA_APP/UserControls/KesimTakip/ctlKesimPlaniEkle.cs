using CEKA_APP.Abstracts;
using CEKA_APP.Business;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ERP;
using CEKA_APP.Interfaces.KesimTakip;
using CEKA_APP.Interfaces.Sistem;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using UglyToad.PdfPig.Content;

namespace CEKA_APP.UsrControl
{
    public partial class ctlKesimPlaniEkle : UserControl
    {
        private Button _seciliButon;
        private List<Button> buttonGroup;
        private int? currentId = null;
        private readonly VeriOkuma _veriOkuma;
        private IFormArayuzu _formArayuzu;
        private string _xmlDosyaYolu;
        private readonly IServiceProvider _serviceProvider;

        private IIdUreticiService _idUreticiService => _serviceProvider.GetRequiredService<IIdUreticiService>();
        private IKesimDetaylariService _kesimDetaylariService => _serviceProvider.GetRequiredService<IKesimDetaylariService>();
        private IKesimListesiService _kesimListesiService => _serviceProvider.GetRequiredService<IKesimListesiService>();
        private IKesimListesiPaketService _kesimListesiPaketService => _serviceProvider.GetRequiredService<IKesimListesiPaketService>();
        private IAutoCadAktarimService _autoCadAktarimService => _serviceProvider.GetRequiredService<IAutoCadAktarimService>();
        private IKarsilastirmaTablosuService _karsilastirmaTablosuService => _serviceProvider.GetRequiredService<IKarsilastirmaTablosuService>();
        private IKullanicilarService _kullaniciService => _serviceProvider.GetRequiredService<IKullanicilarService>();
        private IKullaniciHareketLogService _kullaniciHareketleriService => _serviceProvider.GetRequiredService<IKullaniciHareketLogService>();


        public ctlKesimPlaniEkle(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _veriOkuma = new VeriOkuma();

            DataGridViewHelper.StilUygula(dataGridView1);
            DataGridViewHelper.StilUygula(dataGridView2);
            DataGridViewHelper.StilUygula(dataGridView3);
        }
        public void FormArayuzuAyarla(IFormArayuzu formArayuzu)
        {
            _formArayuzu = formArayuzu;
        }
        private void ctlKesimPlaniEkle_Load(object sender, EventArgs e)
        {
            buttonGroup = new List<Button> { btnAjan, btnAdm, btnBaykal };

            btnAjan.Click += ExclusiveButton_Click;
            btnAdm.Click += ExclusiveButton_Click;
            btnBaykal.Click += ExclusiveButton_Click;

            ButonMakinaSecHelper.NötrStilUygula(buttonGroup);

            _seciliButon = null;

            ctlBaslik1.Baslik = "Kesim Planı Ekle";

            pdfViewer1.ShowToolbar = false;
        }
        private void ExclusiveButton_Click(object sender, EventArgs e)
        {
            Temizle();
            var clickedButton = sender as Button;
            if (clickedButton != null)
            {
                _seciliButon = clickedButton;
                ButonMakinaSecHelper.ButonSekli(clickedButton, buttonGroup);
            }
        }
        private string TespitEtPdfTuru(string pdfText)
        {
            string lowerText = pdfText.ToLower();

            string firstPageText = pdfText.Substring(0, Math.Min(2000, pdfText.Length)).ToLower();

            if (firstPageText.Contains("pronest"))
                return "Baykal";
            if (firstPageText.Contains("toplamparçakesmelistesi"))
                return "Ajan";
            if (firstPageText.Contains("firmaadı"))
                return "Ajan";
            if (firstPageText.Contains("nestings list"))
                return "Lantek";
            if (firstPageText.Contains("detailed list of nestings"))
                return "Lantek";


            MessageBox.Show("PDF türü tespit edilemedi. İlk 500 karakter: " + firstPageText.Substring(0, Math.Min(500, firstPageText.Length)), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }

        private bool ButonVePdfUyumluMu(string pdfTuru)
        {
            if (pdfTuru == "Ajan" && _seciliButon == btnAjan)
                return true;
            if (pdfTuru == "Lantek" && _seciliButon == btnAdm)
                return true;
            if (pdfTuru == "Baykal" && _seciliButon == btnBaykal)
                return true;

            return false;
        }
        private async void btnSec_Click(object sender, EventArgs e)
        {
            if (_seciliButon == null)
            {
                MessageBox.Show("Lütfen önce bir makine seçin (Ajan, Adm, Baykal).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnKaydet.Enabled = true;
            lblDurum.Text = "";
            _formArayuzu.RichTextBox1Temizle();
            _formArayuzu.RichTextBox2Temizle();
            _formArayuzu.RichTextBox3Temizle();
            _formArayuzu.RichTextBox4Temizle();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            txtDosya.Clear();
            txtKalite.Clear();
            txtMalzeme.Clear();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "PDF File|*.pdf";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string filePath = open.FileName;
                txtDosya.Text = filePath;
                if (File.Exists(filePath))
                {

                    try
                    {
                        progressBar1.Value = 0;
                        progressBar1.Visible = true;
                        string pdfText;
                        string islenmisVeriBaykal = null;
                        string islenmisVeriAjan = null;
                        string islenmisVeriAdm = null;

                        if (_seciliButon == btnBaykal)
                        {
                            pdfText = await PdfOkuBaykal(filePath);

                            _formArayuzu.RichTextBox1Yaz(pdfText);

                            islenmisVeriBaykal = IslenmisVeriBaykal();
                            _formArayuzu.RichTextBox4Yaz(islenmisVeriBaykal);
                        }
                        else if (_seciliButon == btnAjan)
                        {
                            pdfText = await PdfOkuAjan(filePath);

                            _formArayuzu.RichTextBox1Yaz(pdfText);


                            islenmisVeriAjan = IslenmisVeriAjan();
                            _formArayuzu.RichTextBox4Yaz(islenmisVeriAjan);
                        }
                        else if (_seciliButon == btnAdm)
                        {
                            pdfText = await PdfOkuADM(filePath);

                            _formArayuzu.RichTextBox1Yaz(pdfText);


                            islenmisVeriAdm = IslenmisVeriADM();
                            _formArayuzu.RichTextBox4Yaz(islenmisVeriAdm);
                        }

                        else
                        {
                            MessageBox.Show("Geçersiz buton seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        if (string.IsNullOrEmpty(pdfText))
                        {
                            MessageBox.Show("PDF metni okunamadı. PDF dosyasının içeriğini kontrol edin.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }
                        //Burada PdfOkuAjan da pdf metni işlendiği için hangi buton seçili hangisi değil sistem anlamıyor bu yüzden alttaki iki satır pdf türü tespit etmek ve okumak için
                        string pdf = await PdfOkuBaykal(filePath);
                        string pftTuruTespitEt = TespitEtPdfTuru(pdf);

                        if (string.IsNullOrEmpty(pftTuruTespitEt))
                        {
                            MessageBox.Show("PDF türü tespit edilemedi. Dosya içeriğini kontrol edin.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }


                        if (!ButonVePdfUyumluMu(pftTuruTespitEt))
                        {
                            MessageBox.Show($"Hatalı seçim: {pftTuruTespitEt} PDF yüklendi, ancak {_seciliButon.Text} butonu seçili.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        await PdfYukle(filePath);

                        (List<MalzemeBilgisi> validData, List<string> InvalidData) = await Task.Run(() =>
                        {
                            try
                            {
                                if (_seciliButon == btnAjan)
                                    return _veriOkuma.PlazmaOku(islenmisVeriAjan);
                                else if (_seciliButon == btnBaykal)
                                    return _veriOkuma.PlazmaOku(islenmisVeriBaykal);
                                else if (_seciliButon == btnAdm)
                                    return _veriOkuma.AdmOku(islenmisVeriAdm);
                                else
                                    throw new InvalidOperationException("Geçersiz buton seçimi.");
                            }
                            catch (Exception ex)
                            {
                                return (null, new List<string> { $"Veri işleme hatası: {ex.Message}" });
                            }
                        });

                        if ((validData == null || validData.Count == 0) && (InvalidData == null || InvalidData.Count == 0))
                        {
                            MessageBox.Show("Hiçbir veri işlenemedi. PDF içeriğini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }
                        _formArayuzu.RichTextBox2Temizle();
                        dataGridView1.Rows.Clear();

                        int totalItems = validData?.Count ?? 0;
                        int currentItem = 0;

                        if (validData != null)
                        {
                            foreach (var data in validData)
                            {
                                if (data != null)
                                {
                                    _formArayuzu.RichTextBox2MetinEkle($"{data.Kalite} - {data.Malzeme} - {data.Kalip} - {data.Poz} - {data.Adet} - {data.Proje}- {data.EkDurumu}\n");
                                    dataGridView1.Rows.Add(data.Kalite, data.Malzeme, data.Kalip, data.Poz, data.Adet, data.Proje, data.EkDurumu);
                                    UpdateTextBoxes();
                                    currentItem++;
                                    int progressValue = totalItems > 0 ? (int)((currentItem / (float)totalItems) * 100) : 0;
                                    progressBar1.Value = progressValue;
                                }
                            }
                        }

                        if (InvalidData != null && InvalidData.Any())
                        {
                            foreach (var invalidLine in InvalidData)
                            {
                                _formArayuzu.RichTextBox3MetinEkle($"{invalidLine}\n");
                            }
                            MessageBox.Show("Bazı veriler formata uymadığı için 'Sistem > Hatalı Format' bölümüne eklendi. Lütfen kontrol edin ve düzenleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        List<string> duzenlenmisSatirlar = new List<string>();
                        foreach (var satir in InvalidData)
                        {
                            string duzenlenmis = DuzeltRegexIle(satir);
                            duzenlenmisSatirlar.Add(duzenlenmis);
                        }


                        if (_seciliButon == btnBaykal)
                        {
                            BaykalSayfaPozDagitimi();
                            SayfaIDTabloVerileri();
                            HesaplaEkAgirlikYuzdeleri();
                        }
                        else if (_seciliButon == btnAjan)
                        {
                            AjanSayfaPozDagitimi();
                            SayfaIDTabloVerileri();
                            HesaplaEkAgirlikYuzdeleri();
                        }
                        else if (_seciliButon == btnAdm)
                        {
                            AdmSayfaPozDagitimi();
                            SayfaIDTabloVerileri();
                            HesaplaEkAgirlikYuzdeleri();
                        }

                        AdetKarsilastirma();

                        progressBar1.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        progressBar1.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Dosya bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressBar1.Visible = false;
                }
            }
        }
        private void AdetKarsilastirma()
        {
            Dictionary<string, int> normalDg1Quantities = new Dictionary<string, int>();
            Dictionary<string, int> ekDg1Quantities = new Dictionary<string, int>();
            Dictionary<string, int> ekParcaDg1Quantities = new Dictionary<string, int>();
            Dictionary<string, List<string>> pozTypes = new Dictionary<string, List<string>>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string kalite = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                    string malzeme = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                    string kalip = row.Cells[2].Value?.ToString()?.Trim() ?? "";
                    string poz = row.Cells[3].Value?.ToString()?.Trim() ?? "";
                    string adetStr = row.Cells[4].Value?.ToString()?.Trim() ?? "";
                    string proje = row.Cells[5].Value?.ToString()?.Trim() ?? "";
                    string ekBilgi = row.Cells[6].Value?.ToString()?.Trim() ?? "";

                    string numericPozPartDg1 = poz.Replace("P", "").Replace("p", "");
                    if (int.TryParse(numericPozPartDg1, out int numberDg1))
                    {
                        poz = numberDg1.ToString("D2");
                    }

                    int adet = 0;
                    adetStr = Regex.Replace(adetStr, @"AD$", "", RegexOptions.IgnoreCase);
                    if (int.TryParse(adetStr, out adet))
                    {
                        string key = $"{kalip}-{poz}-{proje}";
                        string type = ekBilgi.Equals("EK", StringComparison.OrdinalIgnoreCase) ? "EK" :
                                      ekBilgi.Equals("EK Parça", StringComparison.OrdinalIgnoreCase) ? "EK Parça" : "Normal";

                        if (!pozTypes.ContainsKey(key))
                        {
                            pozTypes[key] = new List<string>();
                        }
                        if (!pozTypes[key].Contains(type))
                        {
                            pozTypes[key].Add(type);
                        }

                        if (ekBilgi.Equals("EK", StringComparison.OrdinalIgnoreCase))
                        {
                            ekDg1Quantities[key] = ekDg1Quantities.ContainsKey(key) ? ekDg1Quantities[key] + adet : adet;
                        }
                        else if (ekBilgi.Equals("EK Parça", StringComparison.OrdinalIgnoreCase))
                        {
                            ekParcaDg1Quantities[key] = ekParcaDg1Quantities.ContainsKey(key) ? ekParcaDg1Quantities[key] + adet : adet;
                        }
                        else
                        {
                            normalDg1Quantities[key] = normalDg1Quantities.ContainsKey(key) ? normalDg1Quantities[key] + adet : adet;
                        }
                    }
                }
            }

            Dictionary<string, int> pageRepetitions = new Dictionary<string, int>();
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (!row.IsNewRow && row.Cells.Count > 2)
                {
                    string pageId = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(pageId))
                    {
                        string[] parts = pageId.Split('-');
                        if (parts.Length >= 2)
                        {
                            string basePageId = $"{parts[0]}-{parts[1]}";
                            pageRepetitions[basePageId] = pageRepetitions.ContainsKey(basePageId) ? pageRepetitions[basePageId] + 1 : 1;
                        }
                    }
                }
            }

            Dictionary<string, (int TotalCalculatedAdet, List<string> OriginalCodes)> dg2GroupedByFullKey = new Dictionary<string, (int, List<string>)>();
            Dictionary<string, int> ekOnlyQuantities = new Dictionary<string, int>();
            Dictionary<string, int> ekSayiliQuantities = new Dictionary<string, int>();
            Dictionary<string, List<string>> ekRelatedKeys = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dg2PozTypes = new Dictionary<string, List<string>>();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow && row.Cells.Count > 2)
                {
                    string orijinalKod = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                    string pageIdFromDg2 = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                    int rawAdet = 0;
                    if (!int.TryParse(row.Cells[2].Value?.ToString()?.Trim(), out rawAdet))
                    {
                        continue;
                    }

                    int calculatedAdet = rawAdet;
                    if (pageRepetitions.ContainsKey(pageIdFromDg2))
                    {
                        calculatedAdet *= pageRepetitions[pageIdFromDg2];
                    }

                    string[] parcalar = orijinalKod.Split('-');
                    if (parcalar.Length >= 5)
                    {
                        string pozFromDg2 = parcalar[2].Replace("P", "").Replace("p", "");
                        if (int.TryParse(pozFromDg2, out int numberDg2))
                        {
                            pozFromDg2 = numberDg2.ToString("D2");
                        }

                        string proje = "";
                        if (parcalar[4].Contains(" "))
                        {
                            var projeParts = parcalar[4].Split(' ');
                            proje = projeParts.Length > 1 ? projeParts[1] : parcalar[4];
                        }
                        else
                        {
                            proje = parcalar[4];
                        }

                        string baseKey = $"{parcalar[0]}-{parcalar[1]}-{pozFromDg2}-{proje}";

                        string type = "Normal";
                        if (parcalar.Length > 5)
                        {
                            if (Regex.IsMatch(parcalar[5], @"^EK\d+$", RegexOptions.IgnoreCase))
                            {
                                type = "EK Sayılı";
                            }
                            else if (parcalar[5].Equals("EK", StringComparison.OrdinalIgnoreCase))
                            {
                                type = "EK";
                            }
                        }

                        if (!dg2PozTypes.ContainsKey(baseKey))
                        {
                            dg2PozTypes[baseKey] = new List<string>();
                        }
                        if (!dg2PozTypes[baseKey].Contains(type))
                        {
                            dg2PozTypes[baseKey].Add(type);
                        }

                        if (parcalar.Length > 5 && Regex.IsMatch(parcalar[5], @"^EK\d+$", RegexOptions.IgnoreCase))
                        {
                            string ekNumber = parcalar[5];
                            string ekParcaKey = $"{baseKey}-{ekNumber}";
                            ekSayiliQuantities[ekParcaKey] = ekSayiliQuantities.ContainsKey(ekParcaKey) ? ekSayiliQuantities[ekParcaKey] + calculatedAdet : calculatedAdet;

                            string ekBaseKey = $"{baseKey}-EK";
                            if (!ekRelatedKeys.ContainsKey(ekBaseKey))
                            {
                                ekRelatedKeys[ekBaseKey] = new List<string>();
                            }
                            ekRelatedKeys[ekBaseKey].Add(orijinalKod);
                            continue;
                        }

                        if (parcalar.Length > 5 && parcalar[5].Equals("EK", StringComparison.OrdinalIgnoreCase))
                        {
                            string ekKey = $"{baseKey}-EK";
                            ekOnlyQuantities[ekKey] = ekOnlyQuantities.ContainsKey(ekKey) ? ekOnlyQuantities[ekKey] + calculatedAdet : calculatedAdet;
                            if (!ekRelatedKeys.ContainsKey(ekKey))
                            {
                                ekRelatedKeys[ekKey] = new List<string>();
                            }
                            ekRelatedKeys[ekKey].Add(orijinalKod);
                            continue;
                        }

                        string fullKeyForGrouping = baseKey;
                        if (dg2GroupedByFullKey.ContainsKey(fullKeyForGrouping))
                        {
                            var existingEntry = dg2GroupedByFullKey[fullKeyForGrouping];
                            existingEntry.TotalCalculatedAdet += calculatedAdet;
                            existingEntry.OriginalCodes.Add(orijinalKod);
                            dg2GroupedByFullKey[fullKeyForGrouping] = existingEntry;
                        }
                        else
                        {
                            dg2GroupedByFullKey[fullKeyForGrouping] = (calculatedAdet, new List<string> { orijinalKod });
                        }
                    }
                }
            }

            StringBuilder statusMessage = new StringBuilder();
            bool allQuantitiesMatch = true;
            HashSet<string> processedNormalKeys = new HashSet<string>();
            HashSet<string> processedEkKeys = new HashSet<string>();
            HashSet<string> processedEkParcaKeys = new HashSet<string>();
            List<string> mismatchKeys = new List<string>();
            List<string> missingOrExtraKeys = new List<string>();
            List<string> duplicateTypeKeys = new List<string>();

            var allPozKeys = pozTypes.Keys.Union(dg2PozTypes.Keys).Distinct();
            foreach (var key in allPozKeys)
            {
                var dg1Types = pozTypes.ContainsKey(key) ? pozTypes[key] : new List<string>();
                var dg2Types = dg2PozTypes.ContainsKey(key) ? dg2PozTypes[key] : new List<string>();
                var allTypes = dg1Types.Union(dg2Types).Distinct().ToList();

                if (allTypes.Contains("Normal") && (allTypes.Contains("EK") || allTypes.Contains("EK Sayılı")))
                {
                    duplicateTypeKeys.Add(key);
                }
            }

            foreach (var dg1Entry in normalDg1Quantities)
            {
                string dg1Key = dg1Entry.Key;
                int dg1Adet = dg1Entry.Value;
                int dg2TotalForDg1Key = 0;

                foreach (var dg2GroupedEntry in dg2GroupedByFullKey)
                {
                    string fullDg2Key = dg2GroupedEntry.Key;
                    int dg2TotalCalculatedAdet = dg2GroupedEntry.Value.TotalCalculatedAdet;
                    if (dg1Key == fullDg2Key)
                    {
                        dg2TotalForDg1Key += dg2TotalCalculatedAdet;
                        processedNormalKeys.Add(dg1Key);
                    }
                }

                if (dg2TotalForDg1Key != dg1Adet)
                {
                    allQuantitiesMatch = false;
                    if (!mismatchKeys.Contains(dg1Key))
                    {
                        mismatchKeys.Add(dg1Key);
                    }
                }
            }

            foreach (var dg1Entry in ekDg1Quantities)
            {
                string dg1Key = dg1Entry.Key;
                int dg1Adet = dg1Entry.Value;
                int dg2TotalForDg1Key = 0;
                string ekKey = $"{dg1Key}-EK";
                if (ekOnlyQuantities.ContainsKey(ekKey))
                {
                    dg2TotalForDg1Key = ekOnlyQuantities[ekKey];
                    processedEkKeys.Add(dg1Key);
                }

                if (dg2TotalForDg1Key != dg1Adet)
                {
                    allQuantitiesMatch = false;
                    if (!mismatchKeys.Contains(dg1Key))
                    {
                        mismatchKeys.Add(dg1Key);
                    }
                }
            }

            foreach (var dg1Entry in ekParcaDg1Quantities)
            {
                string dg1Key = dg1Entry.Key;
                int dg1Adet = dg1Entry.Value;
                bool allEkMatch = true;

                foreach (var ekEntry in ekSayiliQuantities)
                {
                    string ekKey = ekEntry.Key;
                    if (ekKey.StartsWith(dg1Key + "-"))
                    {
                        int ekTotalAdet = ekEntry.Value;
                        if (ekTotalAdet != dg1Adet)
                        {
                            allEkMatch = false;
                        }
                    }
                }

                if (!allEkMatch)
                {
                    allQuantitiesMatch = false;
                    if (!mismatchKeys.Contains(dg1Key))
                    {
                        mismatchKeys.Add(dg1Key);
                    }
                }
                processedEkParcaKeys.Add(dg1Key);
            }

            bool hasMissingOrExtra = false;

            foreach (var dg1Entry in normalDg1Quantities)
            {
                if (!processedNormalKeys.Contains(dg1Entry.Key))
                {
                    hasMissingOrExtra = true;
                    if (!missingOrExtraKeys.Contains(dg1Entry.Key))
                    {
                        missingOrExtraKeys.Add(dg1Entry.Key);
                    }
                }
            }
            foreach (var dg1Entry in ekDg1Quantities)
            {
                if (!processedEkKeys.Contains(dg1Entry.Key))
                {
                    hasMissingOrExtra = true;
                    if (!missingOrExtraKeys.Contains(dg1Entry.Key))
                    {
                        missingOrExtraKeys.Add(dg1Entry.Key);
                    }
                }
            }
            foreach (var dg1Entry in ekParcaDg1Quantities)
            {
                if (!processedEkParcaKeys.Contains(dg1Entry.Key))
                {
                    hasMissingOrExtra = true;
                    if (!missingOrExtraKeys.Contains(dg1Entry.Key))
                    {
                        missingOrExtraKeys.Add(dg1Entry.Key);
                    }
                }
            }
            foreach (var dg2GroupedEntry in dg2GroupedByFullKey)
            {
                string fullDg2Key = dg2GroupedEntry.Key;
                if (!normalDg1Quantities.ContainsKey(fullDg2Key))
                {
                    hasMissingOrExtra = true;
                    if (!missingOrExtraKeys.Contains(fullDg2Key))
                    {
                        missingOrExtraKeys.Add(fullDg2Key);
                    }
                }
            }

            bool hasDuplicateTypes = duplicateTypeKeys.Any();
            if (hasDuplicateTypes)
            {
                statusMessage.Append("⚠️ Normal ve EK/EK Sayılı aynı pozda: ");
                statusMessage.Append(string.Join(", ", duplicateTypeKeys));
                lblDurum.Text = statusMessage.ToString();
                lblDurum.ForeColor = Color.OrangeRed;
                btnKaydet.Enabled = false;
            }
            else if (!allQuantitiesMatch)
            {
                statusMessage.Append("⚠️ Adetler eşleşmiyor: ");
                statusMessage.Append(string.Join(", ", mismatchKeys));
                lblDurum.Text = statusMessage.ToString();
                lblDurum.ForeColor = Color.OrangeRed;
                btnKaydet.Enabled = false;
            }
            else
            {
                statusMessage.Append("✅ Tüm adetler eşleşiyor.");
                lblDurum.Text = statusMessage.ToString();
                lblDurum.ForeColor = Color.Green;
                btnKaydet.Enabled = true;
            }

            if (hasMissingOrExtra)
            {
                if (hasDuplicateTypes || !allQuantitiesMatch)
                {
                    statusMessage.Append("\n⚠️ Bazı kayıtlar eksik veya fazla: ");
                }
                else
                {
                    statusMessage.Clear();
                    statusMessage.Append("⚠️ Bazı kayıtlar eksik veya fazla: ");
                }
                statusMessage.Append(string.Join(", ", missingOrExtraKeys));
                lblDurum.Text = statusMessage.ToString();
                lblDurum.ForeColor = Color.OrangeRed;
                btnKaydet.Enabled = false;
            }

            if (!normalDg1Quantities.Any() && !ekDg1Quantities.Any() && !ekParcaDg1Quantities.Any() && !dg2GroupedByFullKey.Any())
            {
                lblDurum.Text = "Karşılaştırılacak veri bulunamadı.";
                lblDurum.ForeColor = Color.Gray;
                btnKaydet.Enabled = false;
            }
        }

        private string DuzeltRegexIle(string hataliSatir)
        {
            var match = Regex.Match(hataliSatir, @"(ST[^\s]*)(\d{5}\.\d{2})");
            if (match.Success)
            {
                string baslangic = match.Groups[1].Value;
                string bitis = match.Groups[2].Value;

                string duzenliSatir = $"{baslangic}-DZN-{bitis}";
                return duzenliSatir;
            }
            return hataliSatir;
        }
        public async Task PdfYukle(string filePath)
        {
            try
            {
                progressBar1.Visible = true;
                progressBar1.Value = 0;

                PdfiumViewer.PdfDocument pdfDoc = null;
                await Task.Run(() =>
                {
                    pdfDoc = PdfiumViewer.PdfDocument.Load(filePath);
                    int pageCount = pdfDoc.PageCount;

                    for (int i = 0; i < pageCount; i++)
                    {
                        int progressValue = (i + 1) * 100 / pageCount;
                        Invoke(new Action(() => progressBar1.Value = progressValue));
                    }
                });

                pdfViewer1.Document = pdfDoc;

                pdfViewer1.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitWidth;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF yükleme hatası: {ex.ToString()}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar1.Visible = false;
            }
        }
        public async Task<string> PdfOkuBaykal(string pdfpath)
        {
            var pageText = new StringBuilder();

            try
            {
                using (UglyToad.PdfPig.PdfDocument pdfDocument = UglyToad.PdfPig.PdfDocument.Open(pdfpath))
                {
                    int pageNumbers = pdfDocument.NumberOfPages;
                    pageText.AppendLine($"Toplam sayfa sayısı: {pageNumbers}");

                    for (int i = 1; i <= pageNumbers; i++)
                    {
                        try
                        {
                            pageText.AppendLine($"\n-------- Sayfa {i} --------");

                            var page = pdfDocument.GetPage(i);
                            var words = page.GetWords().Select(w => new { Text = w.Text, Y = w.BoundingBox.Bottom }).ToList();

                            var groupedByY = words.GroupBy(w => Math.Round(w.Y, 2))
                                                 .OrderByDescending(g => g.Key)
                                                 .Select(g => string.Join(" ", g.Select(w => w.Text).ToList()))
                                                 .ToList();

                            string text = groupedByY.Any() ? string.Join("\n", groupedByY) : "[Bu sayfada metin yok]";
                            pageText.AppendLine(text);
                            pageText.AppendLine();

                            int progressValue = (int)((i / (float)pageNumbers) * 100);
                            BeginInvoke((Action)(() => progressBar1.Value = progressValue));

                            await Task.Delay(100);
                        }
                        catch (Exception pageEx)
                        {
                            pageText.AppendLine($"\n-------- Sayfa {i} --------");
                            pageText.AppendLine($"[Sayfa okunamadı: {pageEx.Message}]");
                            pageText.AppendLine();

                            MessageBox.Show($"Sayfa {i} okunamadı: {pageEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pageText.AppendLine($"[PDF genel okuma hatası: {ex.Message}]");
                MessageBox.Show("PDF genel okuma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return pageText.ToString();
        }
        public async Task<string> PdfOkuAjan(string pdfPath)
        {
            var pageText = new StringBuilder();

            try
            {
                using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(pdfPath))
                {
                    pageText.AppendLine($"===== {System.IO.Path.GetFileName(pdfPath)} =====");
                    pageText.AppendLine($"Toplam sayfa sayısı: {document.GetPages().Count()}");

                    int? firmaAdiPageNumber = null;

                    for (int i = 1; i <= document.GetPages().Count(); i++)
                    {
                        try
                        {
                            var page = document.GetPage(i);
                            var words = page.GetWords().OrderByDescending(w => w.BoundingBox.Bottom).ToList();

                            if (!words.Any())
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("Bu sayfada metin bulunamadı.");
                                continue;
                            }

                            var lines = new List<List<Word>>();
                            double lastY = -1;
                            double yThreshold = 5.0;
                            foreach (var word in words)
                            {
                                if (lastY == -1 || Math.Abs(word.BoundingBox.Bottom - lastY) > yThreshold)
                                {
                                    lines.Add(new List<Word>());
                                }
                                lines.Last().Add(word);
                                lastY = word.BoundingBox.Bottom;
                            }

                            var textLines = lines.Select(line =>
                                string.Join(" ", line.OrderBy(w => w.BoundingBox.Left).Select(w => w.Text).ToList()).Trim())
                                .Where(l => !string.IsNullOrWhiteSpace(l))
                                .ToList();

                            if (!textLines.Any())
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("Bu sayfada geçerli satır yok.");
                                continue;
                            }

                            bool currentPageIsFirmaAdi = textLines.Take(5).Any(l => l.Trim().StartsWith("FirmaAdı", StringComparison.OrdinalIgnoreCase));
                            bool currentPageIsToplamParca = textLines.Take(5).Any(l => l.Trim().StartsWith("ToplamParçaKesmeListesi", StringComparison.OrdinalIgnoreCase));

                            if (currentPageIsToplamParca)
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("[Sayfa atlandı - ToplamParçaKesmeListesi]");
                                firmaAdiPageNumber = null;
                                continue;
                            }

                            if (!currentPageIsFirmaAdi && firmaAdiPageNumber == null)
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("[Sayfa atlandı - FirmaAdı değil]");
                                continue;
                            }

                            if (currentPageIsFirmaAdi)
                            {
                                firmaAdiPageNumber = i;
                            }

                            string sayfaBaslik = currentPageIsFirmaAdi
                                ? $"-------- Sayfa {firmaAdiPageNumber} --------"
                                : $"-------- Sayfa {firmaAdiPageNumber} -------- Devam";
                            pageText.AppendLine($"\n{sayfaBaslik}");

                            string bufferLine = null;

                            for (int j = 0; j < textLines.Count; j++)
                            {
                                var currentLine = Regex.Replace(textLines[j].Trim(), @"\s{2,}", " ");

                                if (bufferLine == null)
                                {
                                    bufferLine = currentLine;
                                    continue;
                                }

                                if (Regex.IsMatch(currentLine, @"^\d") && Regex.IsMatch(bufferLine, @"^ST[A-Z0-9\-]{5,}$"))
                                {
                                    var combined = bufferLine + " " + currentLine;
                                    var columns = Regex.Split(combined, @"\s+").Where(c => !string.IsNullOrEmpty(c)).ToList();
                                    pageText.AppendLine(string.Join("\t", columns));
                                    bufferLine = null;
                                }
                                else
                                {
                                    var prevColumns = Regex.Split(bufferLine, @"\s+").Where(c => !string.IsNullOrEmpty(c)).ToList();
                                    pageText.AppendLine(string.Join(" ", prevColumns));
                                    bufferLine = currentLine;
                                }
                            }

                            if (!string.IsNullOrEmpty(bufferLine))
                            {
                                var lastColumns = Regex.Split(bufferLine, @"\s+").Where(c => !string.IsNullOrEmpty(c)).ToList();
                                pageText.AppendLine(string.Join(" ", lastColumns));
                            }
                        }
                        catch (Exception exPage)
                        {
                            pageText.AppendLine($"\n-------- Sayfa {i} --------");
                            pageText.AppendLine($"[Sayfa {i} okunamadı: {exPage.Message}]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pageText.AppendLine($"[PDF genel okuma hatası: {ex.Message}]");
            }

            return pageText.ToString();
        }
        public async Task<string> PdfOkuADM(string pdfpath)
        {
            var pageText = new StringBuilder();
            var logLines = new List<string> { "=== PdfOkuADM_Log.txt ===", $"İşlem Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm:ss}" };
            int? lastProgramNo = null;
            bool checkNextId = false;

            try
            {
                using (UglyToad.PdfPig.PdfDocument pdfDocument = UglyToad.PdfPig.PdfDocument.Open(pdfpath))
                {
                    int pageNumbers = pdfDocument.NumberOfPages;
                    pageText.AppendLine($"Toplam sayfa sayısı: {pageNumbers}");
                    logLines.Add($"Toplam sayfa sayısı: {pageNumbers}");

                    for (int i = 1; i <= pageNumbers; i++)
                    {
                        try
                        {
                            pageText.AppendLine($"\n-------- Sayfa {i} --------");
                            logLines.Add($"\nSayfa {i} işleniyor...");

                            var page = pdfDocument.GetPage(i);
                            var words = page.GetWords().Select(w => new { Text = w.Text, Y = w.BoundingBox.Bottom }).ToList();

                            var groupedByY = words.GroupBy(w => Math.Round(w.Y, 2))
                                                 .OrderByDescending(g => g.Key)
                                                 .Select(g => string.Join(" ", g.Select(w => w.Text).ToList()))
                                                 .ToList();

                            string[] lines = groupedByY.ToArray();
                            bool idFound = false;

                            for (int j = 0; j < lines.Length; j++)
                            {
                                string line = lines[j].Trim();
                                if (string.IsNullOrWhiteSpace(line))
                                {
                                    logLines.Add($"Boş satır atlandı: {j + 1}");
                                    continue;
                                }

                                logLines.Add($"Satır {j + 1}: {line}");

                                if (Regex.IsMatch(line, @"^\d+,\d+\s*"))
                                {
                                    idFound = true;
                                    if (lastProgramNo.HasValue)
                                    {
                                        checkNextId = true;
                                    }
                                    string idPart = Regex.Match(line, @"^\d+,\d+").Value;
                                    string remainingPart = line.Length > idPart.Length ? line.Substring(idPart.Length).Trim() : null;
                                    pageText.AppendLine(idPart);
                                    logLines.Add($"ID satırı bulundu: {idPart}");

                                    if (!string.IsNullOrEmpty(remainingPart) && Regex.IsMatch(remainingPart, @"^\d+\s+\d+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+"))
                                    {
                                        string[] parts = Regex.Split(remainingPart, @"\s+").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                                        if (parts.Length >= 7)
                                        {
                                            string programNo = parts[0];
                                            if (int.TryParse(programNo, out int currentProgramNo))
                                            {
                                                if (checkNextId && lastProgramNo.HasValue)
                                                {
                                                    int expectedProgramNo = lastProgramNo.Value + 10;
                                                    logLines.Add($"ID satırından sonraki satır bulundu satır başı yapıldı: {remainingPart}");
                                                    pageText.AppendLine();
                                                    pageText.AppendLine(remainingPart);
                                                    checkNextId = false;
                                                    lastProgramNo = currentProgramNo;
                                                }
                                                else
                                                {
                                                    int expectedProgramNo = lastProgramNo.HasValue ? lastProgramNo.Value + 10 : currentProgramNo;
                                                    if (currentProgramNo != expectedProgramNo)
                                                    {
                                                        logLines.Add($"Ardışık program kontrolü: Beklenen: {expectedProgramNo}, Bulunan: {programNo}, satır başı yapılıyor: {remainingPart}");
                                                        pageText.AppendLine();
                                                        pageText.AppendLine(remainingPart);
                                                    }
                                                    else
                                                    {
                                                        pageText.AppendLine(remainingPart);
                                                        logLines.Add($"Program satırı eklendi: {programNo}");
                                                    }
                                                    lastProgramNo = currentProgramNo;
                                                    checkNextId = false;
                                                }
                                            }
                                            idFound = false;
                                            continue;
                                        }
                                    }
                                    continue;
                                }

                                if (idFound && Regex.IsMatch(line, @"^\d+\s+\d+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+\s+[\d,.]+"))
                                {
                                    string[] parts = Regex.Split(line, @"\s+").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                                    if (parts.Length >= 7)
                                    {
                                        string programNo = parts[0];
                                        if (int.TryParse(programNo, out int currentProgramNo))
                                        {
                                            if (checkNextId && lastProgramNo.HasValue)
                                            {
                                                int expectedProgramNo = lastProgramNo.Value + 10;
                                                logLines.Add($"ID satırından sonraki satır bulundu satır başı yapıldı: {line}");
                                                pageText.AppendLine();
                                                pageText.AppendLine(line);
                                                checkNextId = false;
                                            }
                                            else
                                            {
                                                int expectedProgramNo = lastProgramNo.HasValue ? lastProgramNo.Value + 10 : currentProgramNo;
                                                if (currentProgramNo != expectedProgramNo)
                                                {
                                                    logLines.Add($"Ardışık program kontrolü: Beklenen: {expectedProgramNo}, Bulunan: {programNo}, satır başı yapılıyor: {line}");
                                                    pageText.AppendLine();
                                                    pageText.AppendLine(line);
                                                }
                                                else
                                                {
                                                    pageText.AppendLine(line);
                                                    logLines.Add($"Program satırı eklendi: {programNo}");
                                                }
                                                checkNextId = false;
                                            }
                                            lastProgramNo = currentProgramNo;
                                        }
                                        idFound = false;
                                        continue;
                                    }
                                }

                                pageText.AppendLine(line);
                                idFound = false;
                            }

                            int progressValue = (int)((i / (float)pageNumbers) * 100);
                            BeginInvoke((Action)(() => progressBar1.Value = progressValue));

                            await Task.Delay(100);
                        }
                        catch (Exception pageEx)
                        {
                            pageText.AppendLine($"\n-------- Sayfa {i} --------");
                            pageText.AppendLine($"[Sayfa okunamadı: {pageEx.Message}]");
                            logLines.Add($"Sayfa {i} hatası: {pageEx.Message}");
                            MessageBox.Show($"Sayfa {i} okunamadı: {pageEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pageText.AppendLine($"[PDF genel okuma hatası: {ex.Message}]");
                logLines.Add($"PDF genel okuma hatası: {ex.Message}");
                MessageBox.Show("PDF genel okuma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
            string logPath = Path.Combine(appPath, "PdfOkuADM_Log.txt");
            File.WriteAllLines(logPath, logLines, Encoding.UTF8);

            return pageText.ToString();
        }
        private void UpdateTextBoxes()
        {
            HashSet<string> kaliteSet = new HashSet<string>();
            HashSet<string> malzemeSet = new HashSet<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    kaliteSet.Add(row.Cells[0].Value?.ToString());
                    malzemeSet.Add(row.Cells[1].Value?.ToString());
                }
            }

            txtKalite.Text = kaliteSet.First();
            txtMalzeme.Text = malzemeSet.First();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (!currentId.HasValue)
                    {
                        MessageBox.Show("Lütfen önce bir kesim oluşturun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (dataGridView1.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow) || dataGridView2.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow) || dataGridView3.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
                    {
                        MessageBox.Show("Okuma işlemi başarısız: Tablolar boş!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string olusturan = _formArayuzu.lblSistemKullaniciMetinAl();
                    string malzeme = txtMalzeme.Text.Trim();
                    string kalite = txtKalite.Text.Trim();
                    DateTime eklemeTarihi = DateTime.Now;
                    string Id = txtId.Text.Trim();
                    string hataMesaji;

                    HashSet<string> islenmisPaketIdSet = new HashSet<string>();
                    List<string> hataMesajlari = new List<string>();
                    List<Tuple<string, string, string, string, string, int, int>> geciciKayitlar = new List<Tuple<string, string, string, string, string, int, int>>();
                    Dictionary<string, decimal> kesimIdToplamOran = new Dictionary<string, decimal>();
                    Dictionary<string, string> kesimIdAnaPozKeyMap = new Dictionary<string, string>();

                    if (string.IsNullOrEmpty(Id))
                    {
                        MessageBox.Show("Lütfen geçerli bir ID giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (_kesimListesiPaketService.KesimIdVarMi(Id))
                    {
                        MessageBox.Show($"Girilen ID zaten sistemde mevcut: {Id}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string ifsKalite = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kalite);
                    if (string.IsNullOrEmpty(ifsKalite))
                    {
                        hataMesajlari.Add($"Kalite '{kalite}' için eşleşme bulunamadı, hata mesajlarında orijinal değer kullanılacak.");
                        ifsKalite = kalite;
                    }

                    string ifsMalzeme = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
                    if (string.IsNullOrEmpty(ifsMalzeme))
                    {
                        hataMesajlari.Add(hataMesaji);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Dictionary<string, int> pageRepetitions = new Dictionary<string, int>();
                    Dictionary<string, List<(string pageId, int paketAdet)>> pageIdMap = new Dictionary<string, List<(string, int)>>();
                    foreach (DataGridViewRow d3Row in dataGridView3.Rows)
                    {
                        if (!d3Row.IsNewRow && d3Row.Cells.Count > 2 && d3Row.Cells[0].Value != null)
                        {
                            string pageId = d3Row.Cells[0].Value.ToString().Trim();
                            string[] parts = pageId.Split('-');
                            if (parts.Length >= 2)
                            {
                                string basePageId = $"{parts[0]}-{parts[1]}";
                                int paketAdet = 1;
                                if (d3Row.Cells[2].Value != null && int.TryParse(d3Row.Cells[2].Value.ToString().Trim(), out int adet))
                                {
                                    paketAdet = adet;
                                }
                                if (!pageIdMap.ContainsKey(basePageId))
                                {
                                    pageIdMap[basePageId] = new List<(string, int)>();
                                }
                                pageIdMap[basePageId].Add((pageId, paketAdet));
                                pageRepetitions[basePageId] = pageRepetitions.ContainsKey(basePageId) ? pageRepetitions[basePageId] + 1 : 1;
                            }
                        }
                    }

                    Regex regex = new Regex(@"-EK(\d*)$", RegexOptions.IgnoreCase);

                    Dictionary<string, decimal> anaPozToplamOran = new Dictionary<string, decimal>();
                    Dictionary<string, Tuple<string, string, string, string, string>> anaPozBilgileri = new Dictionary<string, Tuple<string, string, string, string, string>>();
                    Dictionary<(string kalip, string poz, string proje), int> kalipPozProjeToplamAdet = new Dictionary<(string, string, string), int>();
                    List<Tuple<string, string, string, string, int, bool>> eksizDetayKayitBilgileri = new List<Tuple<string, string, string, string, int, bool>>();

                    Dictionary<(string pageId, string tamPoz), decimal> ekBirimAgirlikMap = new Dictionary<(string, string), decimal>();
                    Dictionary<(string pageId, string tamPoz), decimal> ekOranMap = new Dictionary<(string, string), decimal>();

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        DataGridViewRow row = dataGridView2.Rows[i];
                        if (row.IsNewRow || row.Cells[0].Value == null) continue;

                        string orijinalKod = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                        string dKesimId = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                        string adetStr = row.Cells[2].Value?.ToString()?.Trim() ?? "";

                        string[] parcalar = orijinalKod.Split('-');
                        string proje = "", kalip = "", poz = "", ekBilgi = "";
                        if (parcalar.Length >= 5)
                        {
                            kalip = $"{parcalar[0]}-{parcalar[1]}";
                            poz = parcalar[2];
                            proje = parcalar[4].Trim();
                            if (parcalar.Length >= 6)
                            {
                                ekBilgi = parcalar[5];
                            }
                        }
                        else
                        {
                            hataMesajlari.Add($"Satır {i + 1}: Geçersiz veri formatı: {orijinalKod}");
                            continue;
                        }

                        string numericPart = poz.Replace("P", "").Replace("p", "");
                        if (int.TryParse(numericPart, out int number))
                        {
                            poz = number.ToString("D2");
                            if (!string.IsNullOrEmpty(ekBilgi))
                            {
                                poz = $"{poz}-{ekBilgi}";
                            }
                        }

                        decimal oran = 0;
                        if (row.Cells[4].Value != null && decimal.TryParse(row.Cells[4].Value.ToString().Trim(), out decimal parsedOran))
                        {
                            oran = parsedOran;
                        }

                        decimal birimAgirlik = 0;
                        if (row.Cells[3].Value != null && decimal.TryParse(row.Cells[3].Value.ToString().Trim(), out decimal parsedBirim))
                        {
                            birimAgirlik = parsedBirim;
                        }
                        else
                        {
                            hataMesajlari.Add($"Satır {i + 1}: Birim ağırlık bulunamadı: {orijinalKod}");
                            continue;
                        }

                        int totalPaketAdet = pageRepetitions.ContainsKey(dKesimId) ? pageRepetitions[dKesimId] : 1;

                        if (!int.TryParse(adetStr, out int adet))
                        {
                            hataMesajlari.Add($"Satır {i + 1}: Geçersiz adet formatı: {adetStr}");
                            continue;
                        }

                        int adetKayitFinal = adet * totalPaketAdet;
                        bool ekVar = regex.IsMatch(orijinalKod);
                        string basePoz = Regex.Replace(poz, @"\-EK\d*$", "", RegexOptions.IgnoreCase);
                        string kalipPozForValidation = $"{kalip}-{basePoz}";
                        if (!_autoCadAktarimService.GetirStandartGruplar(kalip))
                        {
                            string kalip00 = $"{kalip.Substring(0, 3)}-00";
                            kalipPozForValidation = $"{kalip00}-{basePoz}";
                        }

                        string anaPozKey = $"{kalip}-{basePoz}-{proje}";

                        if (ekVar)
                        {
                            string uniqueKey = $"{dKesimId}-{poz}";
                            kesimIdAnaPozKeyMap[uniqueKey] = anaPozKey;
                            kesimIdToplamOran[uniqueKey] = oran;

                            if (pageIdMap.ContainsKey(dKesimId))
                            {
                                foreach (var (pageId, paketAdet) in pageIdMap[dKesimId])
                                {
                                    var keyTuple = (pageId, poz);
                                    ekOranMap[keyTuple] = oran;
                                    ekBirimAgirlikMap[keyTuple] = birimAgirlik;
                                }
                            }
                            else
                            {
                                string pageId = dKesimId;
                                var keyTuple = (pageId, poz);
                                ekOranMap[keyTuple] = oran;
                                ekBirimAgirlikMap[keyTuple] = birimAgirlik;
                            }

                            if (!anaPozToplamOran.ContainsKey(anaPozKey))
                                anaPozToplamOran[anaPozKey] = 0;
                            anaPozToplamOran[anaPozKey] += oran;

                            if (!anaPozBilgileri.ContainsKey(anaPozKey))
                            {
                                anaPozBilgileri[anaPozKey] = new Tuple<string, string, string, string, string>(ifsKalite, ifsMalzeme, proje, kalip, basePoz);
                            }
                        }
                        else
                        {
                            var key = (kalip, poz, proje);
                            if (kalipPozProjeToplamAdet.ContainsKey(key))
                            {
                                kalipPozProjeToplamAdet[key] += adetKayitFinal;
                            }
                            else
                            {
                                kalipPozProjeToplamAdet[key] = adetKayitFinal;
                            }

                            string malzemeKodForDetails = $"{kalip}-{basePoz}";
                            int firstHyphenIndex = malzemeKodForDetails.IndexOf('-');
                            int secondHyphenIndex = -1;
                            if (firstHyphenIndex != -1)
                            {
                                secondHyphenIndex = malzemeKodForDetails.IndexOf('-', firstHyphenIndex + 1);
                            }

                            if (secondHyphenIndex != -1 && secondHyphenIndex + 1 < malzemeKodForDetails.Length)
                            {
                                int thirdHyphenIndex = malzemeKodForDetails.IndexOf('-', secondHyphenIndex + 1);
                                if (thirdHyphenIndex != -1)
                                {
                                    malzemeKodForDetails = malzemeKodForDetails.Substring(0, thirdHyphenIndex);
                                }
                            }

                            eksizDetayKayitBilgileri.Add(Tuple.Create(ifsKalite, ifsMalzeme, malzemeKodForDetails, proje, adetKayitFinal, ekVar));
                        }

                        if (pageIdMap.ContainsKey(dKesimId))
                        {
                            foreach (var (pageId, paketAdet) in pageIdMap[dKesimId])
                            {
                                geciciKayitlar.Add(Tuple.Create(pageId, proje, kalip, poz, adetStr, adet, paketAdet));
                                islenmisPaketIdSet.Add(pageId);
                            }
                        }
                        else
                        {
                            int paketAdet = 1;
                            geciciKayitlar.Add(Tuple.Create(dKesimId, proje, kalip, poz, adetStr, adet, paketAdet));
                            islenmisPaketIdSet.Add(dKesimId);
                        }
                    }

                    Dictionary<string, decimal> kesimIdOranPayi = new Dictionary<string, decimal>();
                    foreach (var kvp in kesimIdToplamOran)
                    {
                        string uniqueKey = kvp.Key;
                        decimal kesimToplamOran = kvp.Value;
                        if (kesimIdAnaPozKeyMap.ContainsKey(uniqueKey) && anaPozToplamOran.ContainsKey(kesimIdAnaPozKeyMap[uniqueKey]))
                        {
                            decimal anaToplamOran = anaPozToplamOran[kesimIdAnaPozKeyMap[uniqueKey]];
                            decimal oranPayi = anaToplamOran > 0 ? kesimToplamOran / anaToplamOran : 0;
                            oranPayi = Math.Min(1.0m, oranPayi);
                            kesimIdOranPayi[uniqueKey] = oranPayi;
                        }
                    }

                    Dictionary<string, decimal> anaPozHesaplananAdet = new Dictionary<string, decimal>();
                    foreach (var oranKvp in anaPozToplamOran)
                    {
                        string anaPozKey = oranKvp.Key;
                        decimal toplamOran = oranKvp.Value;

                        if (anaPozBilgileri.ContainsKey(anaPozKey))
                        {
                            var bilgi = anaPozBilgileri[anaPozKey];
                            string detayIfsKalite = bilgi.Item1;
                            string detayIfsMalzeme = bilgi.Item2;
                            string detayProje = bilgi.Item3;
                            string kalip = bilgi.Item4;
                            string basePoz = bilgi.Item5;

                            string kalipPozForValidation = $"{kalip}-{basePoz}";
                            if (!_autoCadAktarimService.GetirStandartGruplar(kalip))
                            {
                                string kalip00 = $"{kalip.Substring(0, 3)}-00";
                                kalipPozForValidation = $"{kalip00}-{basePoz}";
                            }

                            decimal netAgirlik = _autoCadAktarimService.GetNetAgirlik(detayIfsKalite, detayIfsMalzeme, kalipPozForValidation, detayProje);
                            var (stokYeterli, toplamAdetIfs) = _autoCadAktarimService.AdetGetir(detayIfsKalite, detayIfsMalzeme, kalipPozForValidation, detayProje, 0);
                            decimal toplamAgirlik = toplamAdetIfs * netAgirlik;
                            decimal hesaplananAgirlik = toplamAgirlik * toplamOran;
                            decimal hesaplananAdet = netAgirlik > 0 ? Math.Round(hesaplananAgirlik / netAgirlik, 3, MidpointRounding.AwayFromZero) : 0;

                            anaPozHesaplananAdet[anaPozKey] = hesaplananAdet;

                            if (netAgirlik <= 0)
                            {
                                hataMesajlari.Add($"Net ağırlık bulunamadı: [{detayIfsKalite} - {detayIfsMalzeme} - {kalip} - {basePoz} - {detayProje}]");
                            }
                            else if (toplamAdetIfs <= 0)
                            {
                                hataMesajlari.Add($"Stok adet bulunamadı: [{detayIfsKalite} - {detayIfsMalzeme} - {kalip} - {basePoz} - {detayProje}]");
                            }
                            else if (hesaplananAdet > toplamAdetIfs)
                            {
                                hataMesajlari.Add(
                                      $"EK Hesaplama Hatası (EK'li Poz): [{kalite} - {malzeme} - {kalip} - {basePoz} - {detayProje}]\n" +
                                      $"Stok Adet: {toplamAdetIfs}, Hesaplanan Adet: {hesaplananAdet:F2}\n" +
                                      $"❗ Hesaplanan adet stok adedini aşıyor (toplam oran: {toplamOran:F2}).\n"
                                  );
                            }
                        }
                    }

                    foreach (var entry in kalipPozProjeToplamAdet)
                    {
                        var (kalip, poz, proje) = entry.Key;
                        int toplamAdet = entry.Value;

                        string kalipPozForValidation = $"{kalip}-{poz}";
                        if (!_autoCadAktarimService.GetirStandartGruplar(kalip))
                        {
                            string kalip00 = $"{kalip.Substring(0, 3)}-00";
                            kalipPozForValidation = $"{kalip00}-{poz}";
                        }

                        int tireSayisi = kalipPozForValidation.Count(c => c == '-');
                        if (tireSayisi >= 3)
                        {
                            int ucuncuTireIndex = kalipPozForValidation.IndexOf('-', kalipPozForValidation.IndexOf('-', kalipPozForValidation.IndexOf('-') + 1) + 1);
                            kalipPozForValidation = kalipPozForValidation.Substring(0, ucuncuTireIndex);
                        }

                        var (isValid, toplamAdetIfs, toplamAdetYuklenen) = _autoCadAktarimService.KontrolAdeta(ifsKalite, ifsMalzeme, kalipPozForValidation, proje, toplamAdet);

                        if (!isValid)
                        {
                            hataMesajlari.Add(
                                $"Stok Yetersizliği: [{kalite} - {malzeme} - {kalip} - {poz} - {proje}]\n" +
                                $"Planlanan: {toplamAdet}, Yüklenen: {toplamAdetYuklenen}, Toplam Talep: {toplamAdet + toplamAdetYuklenen}, Stok: {toplamAdetIfs}\n" +
                                $"❗ Toplam ihtiyaç, mevcut stok miktarını aşmaktadır.\n"
                            );
                        }
                    }

                    if (hataMesajlari.Count > 0)
                    {
                        hataMesaji = "Aşağıdaki satırlarda hata bulundu:\n\n" + string.Join("\n", hataMesajlari);
                        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    List<Tuple<string, string, string, string, int, bool>> detayKayitBilgileri = new List<Tuple<string, string, string, string, int, bool>>();
                    foreach (var eksizDetay in eksizDetayKayitBilgileri)
                    {
                        detayKayitBilgileri.Add(eksizDetay);
                    }
                    foreach (var anaPozKey in anaPozHesaplananAdet.Keys)
                    {
                        if (anaPozBilgileri.ContainsKey(anaPozKey))
                        {
                            var bilgi = anaPozBilgileri[anaPozKey];
                            string detayIfsKalite = bilgi.Item1;
                            string detayIfsMalzeme = bilgi.Item2;
                            string detayProje = bilgi.Item3;
                            string kalip = bilgi.Item4;
                            string basePoz = bilgi.Item5;

                            string malzemeKodForDetails = $"{kalip}-{basePoz}";
                            int firstHyphenIndex = malzemeKodForDetails.IndexOf('-');
                            int secondHyphenIndex = -1;
                            if (firstHyphenIndex != -1)
                            {
                                secondHyphenIndex = malzemeKodForDetails.IndexOf('-', firstHyphenIndex + 1);
                            }
                            if (secondHyphenIndex != -1 && secondHyphenIndex + 1 < malzemeKodForDetails.Length)
                            {
                                int thirdHyphenIndex = malzemeKodForDetails.IndexOf('-', secondHyphenIndex + 1);
                                if (thirdHyphenIndex != -1)
                                {
                                    malzemeKodForDetails = malzemeKodForDetails.Substring(0, thirdHyphenIndex);
                                }
                            }

                            decimal hesaplananAdet = anaPozHesaplananAdet[anaPozKey];
                            int detayAdetKayit = (int)Math.Round(hesaplananAdet, 0);

                            detayKayitBilgileri.Add(Tuple.Create(detayIfsKalite, detayIfsMalzeme, malzemeKodForDetails, detayProje, detayAdetKayit, true));
                        }
                    }

                    bool kayitYapildi = false;

                    foreach (var detayBilgi in detayKayitBilgileri)
                    {
                        string detayIfsKalite = detayBilgi.Item1;
                        string detayIfsMalzeme = detayBilgi.Item2;
                        string detayMalzemeKod = detayBilgi.Item3;
                        string detayProje = detayBilgi.Item4;
                        int detayAdetKayit = detayBilgi.Item5;
                        bool detayEkVar = detayBilgi.Item6;

                        _kesimDetaylariService.SaveKesimDetaylariData(detayIfsKalite, detayIfsMalzeme, detayMalzemeKod, detayProje, detayAdetKayit, detayAdetKayit, detayEkVar);
                        kayitYapildi = true;
                    }

                    foreach (var kayit in geciciKayitlar)
                    {
                        string pageId = kayit.Item1;
                        string proje = kayit.Item2;
                        string kalip = kayit.Item3;
                        string poz = kayit.Item4;
                        string adetStr = kayit.Item5;
                        int adet = kayit.Item6;
                        int paketAdet = kayit.Item7;

                        decimal kesimHesaplananAdet = adet;

                        bool ekVar = regex.IsMatch(poz);
                        if (ekVar)
                        {
                            string basePoz = Regex.Replace(poz, @"\-EK\d*$", "");
                            string kalipPozForValidation = $"{kalip}-{basePoz}";

                            if (!_autoCadAktarimService.GetirStandartGruplar(kalip))
                            {
                                string kalip00 = $"{kalip.Substring(0, 3)}-00";
                                kalipPozForValidation = $"{kalip00}-{basePoz}";
                            }

                            string anaPozKey = $"{kalip}-{basePoz}-{proje}";
                            if (anaPozBilgileri.ContainsKey(anaPozKey))
                            {
                                var bilgi = anaPozBilgileri[anaPozKey];
                                string detayIfsKalite = bilgi.Item1;
                                string detayIfsMalzeme = bilgi.Item2;
                                string detayProje = bilgi.Item3;

                                var keyTuple = (pageId, poz);

                                if (ekOranMap.ContainsKey(keyTuple) && ekBirimAgirlikMap.ContainsKey(keyTuple))
                                {
                                    decimal netAgirlik = _autoCadAktarimService.GetNetAgirlik(detayIfsKalite, detayIfsMalzeme, kalipPozForValidation, detayProje);
                                    var (stokYeterli, toplamAdetIfs) = _autoCadAktarimService.AdetGetir(detayIfsKalite, detayIfsMalzeme, kalipPozForValidation, detayProje, 0);

                                    decimal anaToplamAgirlik = toplamAdetIfs * netAgirlik;

                                    decimal ekOran = ekOranMap[keyTuple];

                                    decimal hesaplananAgirlik = anaToplamAgirlik * ekOran;
                                    kesimHesaplananAdet = netAgirlik > 0 ? hesaplananAgirlik / netAgirlik : 0;

                                    kesimHesaplananAdet = Math.Round(kesimHesaplananAdet, 2, MidpointRounding.AwayFromZero);

                                    adetStr = kesimHesaplananAdet.ToString("F2");
                                }
                            }
                        }

                        _kesimListesiPaketService.SaveKesimDataPaket(olusturan, pageId, paketAdet, paketAdet, eklemeTarihi);
                        _kesimListesiService.SaveKesimData(olusturan, pageId, proje, malzeme, kalite,
                            new string[] { kalip }, new string[] { poz }, new decimal[] { kesimHesaplananAdet }, eklemeTarihi);
                    }

                    if (kayitYapildi)
                    {
                        _idUreticiService.SiradakiIdKaydet(currentId.Value);
                        var kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_formArayuzu.lblSistemKullaniciMetinAl());
                        _kullaniciHareketleriService.LogEkle(kullaniciId, "KesimPlaniEklendi", "Kesim Planı Ekle", $"Kullanıcı {currentId.Value} numaralı kesim planını yükledi.");

                        if (ExportToXmlWithDialog(dataGridView1))
                        {
                            MessageBox.Show("Kayıt işlemi başarıyla tamamlandı ve XML dosyası oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnYazdir.Enabled = true;
                            scope.Complete();
                        }
                        else
                        {
                            MessageBox.Show("XML oluşturulamadı, kayıt işlemi iptal edildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hiçbir geçerli satır bulunamadı, kayıt yapılmadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            lblDurum.Text = "";
            txtMalzeme.Clear();
            txtKalite.Clear();
            txtDosya.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            _formArayuzu.RichTextBox1Temizle();
            _formArayuzu.RichTextBox2Temizle();
            _formArayuzu.RichTextBox3Temizle();
            _formArayuzu.RichTextBox4Temizle();
            _xmlDosyaYolu = null;
            btnYazdir.Enabled = false;
            ButonMakinaSecHelper.NötrStilUygula(buttonGroup);

        }

        public string IslenmisVeriAjan()

        {
            if (_formArayuzu.RichTextBox1BosMu())
            {
                MessageBox.Show("richTextBox1 boş. Lütfen veri girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }

            StringBuilder sonucBuilder = new StringBuilder();
            string[] satirlar = _formArayuzu.RichTextBox1SatirlariGetir();
            bool isFirmaAdiSection = false;
            int currentPageNumber = 1;
            int firmaSayac = 0;

            foreach (string satir in satirlar)
            {
                string temizSatir = satir.Trim();

                if (temizSatir.StartsWith("-------- Sayfa"))
                {
                    string[] parts = temizSatir.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3 && int.TryParse(parts[2].Replace("--------", "").Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }

                if (temizSatir.StartsWith("Sayfa:"))
                {
                    if (int.TryParse(temizSatir.Replace("Sayfa:", "").Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }
                if (temizSatir.StartsWith("--------") ||
                  temizSatir.StartsWith("Parça") ||
                  temizSatir.StartsWith("Not:") ||
                  temizSatir.StartsWith("EkstraKesim") ||
                  temizSatir.StartsWith("Toplam"))
                    continue;

                if (temizSatir.StartsWith("ToplamParçaKesmeListesi"))
                {
                    isFirmaAdiSection = false;
                    continue;
                }

                if (temizSatir.StartsWith("FirmaAdı"))
                {
                    isFirmaAdiSection = true;
                    firmaSayac++;
                    continue;
                }

                if (!isFirmaAdiSection)
                    continue;
                Match match = Regex.Match(temizSatir, @"(ST\d+\s*-(?:\s*[^\s-]*\s*(?:-+\s*[^\s-]*\s*)*-+\s*)\S+)");

                if (match.Success)
                {
                    string parcaVeri = match.Groups[1].Value.Trim();
                    string birlesikVeri = $"{parcaVeri} (Yerleşim: {firmaSayac})(Sayfa: {currentPageNumber})";
                    sonucBuilder.AppendLine(birlesikVeri);
                }
            }

            return sonucBuilder.ToString();
        }
        public string IslenmisVeriBaykal()
        {
            if (_formArayuzu.RichTextBox1BosMu())
            {
                MessageBox.Show("richTextBox1 boş. Lütfen veri girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }

            StringBuilder sonucBuilder = new StringBuilder();
            string[] satirlar = _formArayuzu.RichTextBox1SatirlariGetir();

            int currentYerlesim = 1;
            int currentSayfa = 1;
            Regex yerlesimRegex = new Regex(@"Yerleşim:\s*(\d+)\s*/\s*\d+", RegexOptions.IgnoreCase);
            Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*--------", RegexOptions.IgnoreCase);
            string pattern = @"^(?:(?:\d+\s*-\s*\d+\s*)|\d+\s*)?(ST\d+\s*-(?:\s*[^\s-]*\s*(?:-+\s*[^\s-]*\s*)*-+\s*)\S+)";

            foreach (string satir in satirlar)
            {
                string trimmedSatir = satir.Trim();

                Match yerlesimMatch = yerlesimRegex.Match(trimmedSatir);
                if (yerlesimMatch.Success && int.TryParse(yerlesimMatch.Groups[1].Value, out int yerlesimFromLayout))
                {
                    currentYerlesim = yerlesimFromLayout;
                    continue;
                }
                Match sayfaMatch = sayfaRegex.Match(trimmedSatir);
                if (sayfaMatch.Success && int.TryParse(sayfaMatch.Groups[1].Value, out int sayfaFromLayout))
                {
                    currentSayfa = sayfaFromLayout;
                    continue;
                }

                if (trimmedSatir.IndexOf("ST", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Match match = Regex.Match(trimmedSatir, pattern);
                    if (match.Success)
                    {
                        string parcaAdi = match.Groups[1].Value.Trim();
                        sonucBuilder.AppendLine($"{parcaAdi} (Yerleşim: {currentYerlesim})(Sayfa: {currentSayfa})");
                    }
                }
            }

            return sonucBuilder.ToString();
        }
        public string IslenmisVeriADM()
        {
            if (_formArayuzu.RichTextBox1BosMu())
            {
                MessageBox.Show("Lütfen bir format seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }

            StringBuilder sonucBuilder = new StringBuilder();
            string[] satirlar = _formArayuzu.RichTextBox1SatirlariAl();
            int currentPageNumber = 1;
            string currentProgram = "";
            string currentProgramTekrari = "";
            string currentMalzeme = "";
            Dictionary<string, int> parcaToplamAdet = new Dictionary<string, int>();
            List<(string Parca, string Miktar, int PageNumber, string Program, string ProgramTekrari)> parcaListesi = new List<(string, string, int, string, string)>();

            string malzemeRegex = @"^ST\d+-[A-Za-z0-9-]+$";
            string programRegex = @"^\s*(\d+)\s+(\d+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)";
            string parcaRegex = @"^\d{5}\.\d{2,3}(?:-\d+)*-P\d+(?:-EK\d+)?\s+\d{1,3}";

            foreach (string satir in satirlar)
            {
                string temizSatir = Regex.Replace(satir, @"\s+", " ").Trim();
                if (string.IsNullOrEmpty(temizSatir))
                    continue;

                if (temizSatir.StartsWith("-------- Sayfa"))
                {
                    string[] parts = temizSatir.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3 && int.TryParse(parts[2].Replace("--------", "").Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }

                if (temizSatir.StartsWith("Sayfa:") || temizSatir.StartsWith("Sayfa "))
                {
                    string pagePart = temizSatir.Replace("Sayfa:", "").Replace("Sayfa ", "").Trim();
                    if (int.TryParse(pagePart.Split('/')[0].Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }

                if (Regex.IsMatch(temizSatir, malzemeRegex))
                {
                    string[] malzemeParts = temizSatir.Split('-');
                    if (malzemeParts.Length >= 2)
                    {
                        currentMalzeme = $"{malzemeParts[0]}-{malzemeParts[1]}";
                    }
                    continue;
                }

                if (Regex.IsMatch(temizSatir, programRegex))
                {
                    string[] programSutun = Regex.Split(temizSatir, @"\s+")
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();

                    if (programSutun.Length >= 2)
                    {
                        currentProgram = programSutun[0];
                        currentProgramTekrari = programSutun[1];
                    }
                    continue;
                }

                if (Regex.IsMatch(temizSatir, parcaRegex))
                {
                    string[] parcaSutun = Regex.Split(temizSatir, @"\s+")
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();

                    if (parcaSutun.Length >= 2)
                    {
                        string parca = parcaSutun[0];
                        string miktar = parcaSutun[1];

                        string[] parcaParts = parca.Split('-');
                        if (parcaParts.Length >= 2 && !string.IsNullOrEmpty(currentMalzeme))
                        {
                            string proje = parcaParts[0];
                            string kalip = parcaParts.Length > 2 ? string.Join("-", parcaParts.Skip(1).Take(parcaParts.Length - (parcaParts[parcaParts.Length - 1].StartsWith("EK") ? 3 : 2))) : "";
                            string poz = parcaParts[parcaParts.Length - 1].StartsWith("P") ? parcaParts[parcaParts.Length - 1] : parcaParts[parcaParts.Length - 2];
                            string ek = parcaParts[parcaParts.Length - 1].StartsWith("EK") ? parcaParts[parcaParts.Length - 1] : "";
                            string pozKey = string.IsNullOrEmpty(ek) ? poz : $"{poz}-{ek}";

                            string parcaKey = $"{proje}-{kalip}-{pozKey}";
                            int adet = int.Parse(miktar) * int.Parse(currentProgramTekrari);

                            if (!parcaToplamAdet.ContainsKey(parcaKey))
                            {
                                parcaToplamAdet[parcaKey] = 0;
                            }
                            parcaToplamAdet[parcaKey] += adet;

                            parcaListesi.Add((parca, miktar, currentPageNumber, currentProgram, currentProgramTekrari));
                        }
                    }
                }
            }

            foreach (var (parca, miktar, pageNumber, program, programTekrari) in parcaListesi)
            {
                string[] parcaParts = parca.Split('-');
                string proje = parcaParts[0];
                string kalip = parcaParts.Length > 2 ? string.Join("-", parcaParts.Skip(1).Take(parcaParts.Length - (parcaParts[parcaParts.Length - 1].StartsWith("EK") ? 3 : 2))) : "";
                string poz = parcaParts[parcaParts.Length - 1].StartsWith("P") ? parcaParts[parcaParts.Length - 1] : parcaParts[parcaParts.Length - 2];
                string ek = parcaParts[parcaParts.Length - 1].StartsWith("EK") ? parcaParts[parcaParts.Length - 1] : "";
                string pozKey = string.IsNullOrEmpty(ek) ? poz : $"{poz}-{ek}";

                string parcaKey = $"{proje}-{kalip}-{pozKey}";
                string sonuc1 = $"{currentMalzeme}{(string.IsNullOrEmpty(kalip) ? "" : "-")}{kalip}{(string.IsNullOrEmpty(kalip) ? "" : "-")}{pozKey}-{parcaToplamAdet[parcaKey]}AD-{proje}";
                sonucBuilder.AppendLine($"{sonuc1} (Sayfa: {pageNumber}, Miktar: {miktar}, Program: {program}, Program Tekrarı: {programTekrari})");
            }

            string sonuc = sonucBuilder.ToString();
            return sonuc;
        }
        public void IDVer()
        {
            try
            {
                currentId = null;

                currentId = _idUreticiService.GetSiradakiId();

                if (currentId == null)
                {
                    MessageBox.Show("Yeni bir ID alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                panelVeriYonetim.Enabled = true;
                txtId.Text = currentId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (Control control in panelVeriYonetim.Controls)
            {
                if (control != null)
                {
                    control.Enabled = true;
                }
            }
        }

        public bool ExportToXmlWithDialog(DataGridView dgv)
        {
            string targetFolder = Properties.Settings.Default.KlasorYolu;

            if (string.IsNullOrWhiteSpace(targetFolder) || !Directory.Exists(targetFolder))
            {
                MessageBox.Show("Klasör seçimi yapılmamış veya geçersiz bir klasör yolu. Lütfen önce bir klasör seçin.",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string yerlesimPlaniId = txtId.Text.Trim();
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"{yerlesimPlaniId}_YerlesimPlani_{tarihSaat}.xml";
            string dosyaYolu = Path.Combine(targetFolder, fileName);

            return ExportToXml(dgv, dosyaYolu);
        }
        public bool ExportToXml(DataGridView dgv, string dosyaYolu)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtSite.Text))
            {
                MessageBox.Show("ID veya Site alanı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dgv.Rows.Count == 0 || dgv.Rows[0].IsNewRow)
            {
                MessageBox.Show("DataGridView'de veri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            XmlWriterSettings ayarlar = new XmlWriterSettings { Indent = true };
            Regex ekRegex = new Regex(@"-EK\d*(?=\s|$)", RegexOptions.IgnoreCase);

            try
            {
                string directory = Path.GetDirectoryName(dosyaYolu);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (XmlWriter writer = XmlWriter.Create(dosyaYolu, ayarlar))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("YerlesimPlaniBilgileri");

                    writer.WriteElementString("ID", txtId.Text.Trim());
                    writer.WriteElementString("Site", txtSite.Text.Trim());

                    string stokKoduInput = txtMalzeme.Text?.Trim();
                    string stokKoduXml = _karsilastirmaTablosuService.GetIfsCodeByKesimCode(stokKoduInput);
                    if (string.IsNullOrEmpty(stokKoduXml))
                    {
                        MessageBox.Show("Kesim karşılaştırma tablosunda stok kodu bulunamadı veya geçersiz!");
                        return false;
                    }
                    writer.WriteElementString("StokKodu", $"K{stokKoduXml}");

                    string kaliteInput = txtKalite.Text?.Trim();
                    string kaliteXml = _karsilastirmaTablosuService.GetIfsCodeByAutoCadCodeKalite(kaliteInput);
                    if (string.IsNullOrEmpty(kaliteXml))
                    {
                        MessageBox.Show("Kalite karşılaştırma tablosunda stok kodu bulunamadı veya geçersiz!");
                        return false;
                    }
                    writer.WriteElementString("Kalite", kaliteXml);

                    writer.WriteElementString("EklemeTarihi", dtEklemeTarihi.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture).Trim());

                    var parcaBySayfaId = dataGridView2.Rows.Cast<DataGridViewRow>()
                        .Where(row => !row.IsNewRow && row.Cells[1].Value != null)
                        .GroupBy(row => new
                        {
                            SayfaId = row.Cells[1].Value.ToString().Trim(),
                            BasePoz = ekRegex.Replace(row.Cells[0].Value?.ToString()?.Trim() ?? "", "")
                        })
                        .Select(g => new
                        {
                            g.Key.SayfaId,
                            g.Key.BasePoz,
                            Poz = ExtractPoz(g.Key.BasePoz),
                            ParcaRows = g.ToList(),
                            ToplamAdet = g.Sum(row => double.TryParse(row.Cells[2].Value?.ToString()?.Trim(), out double adet) ? adet : 0),
                            ToplamEkOran = g.Sum(row => double.TryParse(row.Cells[4].Value?.ToString()?.Trim(), out double oran) ? oran : 0),
                            EkAdi = string.Join(",", g.Where(row => row.Cells[0].Value?.ToString().Contains("EK") == true)
                                                     .Select(row => row.Cells[0].Value?.ToString().Split('-').LastOrDefault(p => p.Contains("EK")))
                                                     .Distinct())
                        })
                        .ToList();

                    foreach (DataGridViewRow sayfaRow in dataGridView3.Rows)
                    {
                        if (sayfaRow.IsNewRow || sayfaRow.Cells[0].Value == null) continue;

                        string pageId = sayfaRow.Cells[0].Value.ToString().Trim();
                        string[] pageIdParts = pageId.Split('-');
                        if (pageIdParts.Length < 2) continue;
                        string basePageId = $"{pageIdParts[0]}-{pageIdParts[1]}";

                        writer.WriteStartElement("Sayfa");
                        writer.WriteElementString("ID", pageId);

                        string tekrarAdeti = (sayfaRow.Cells[2].Value?.ToString() ?? "1").Trim();
                        writer.WriteElementString("TekrarAdeti", tekrarAdeti);

                        var matchingParcaGroups = parcaBySayfaId.Where(x => x.SayfaId == basePageId).ToList();

                        foreach (var parcaGroup in matchingParcaGroups)
                        {
                            writer.WriteStartElement("Parca");

                            string kalipVerisi = parcaGroup.BasePoz;
                            string[] parcalar = kalipVerisi.Split('-');
                            string kalip = kalipVerisi, poz = parcaGroup.Poz, proje = "";

                            if (parcalar.Length >= 5)
                            {
                                string sifirParca = parcalar[0].Trim();
                                string birParca = parcalar[1].Trim();

                                kalip = $"{sifirParca}-{birParca}";
                                if (!_autoCadAktarimService.GetirStandartGruplar(kalip))
                                {
                                    birParca = "00";
                                    kalip = $"{sifirParca}-{birParca}";
                                }

                                proje = parcalar[4].Trim();

                                if (poz.StartsWith("P"))
                                {
                                    poz = poz.Substring(1);
                                    if (poz.Length == 1 && int.TryParse(poz, out int pozSayi))
                                    {
                                        poz = pozSayi.ToString("D2");
                                    }
                                }
                            }

                            writer.WriteElementString("Kalip", kalip);
                            writer.WriteElementString("Poz", poz);
                            writer.WriteElementString("Proje", proje);

                            string ekAdi = string.IsNullOrEmpty(parcaGroup.EkAdi) ? "-" : parcaGroup.EkAdi;
                            string ekOran = parcaGroup.ToplamEkOran > 0 ? parcaGroup.ToplamEkOran.ToString("F6", CultureInfo.InvariantCulture) : "-";
                            string adetToWrite = parcaGroup.ToplamEkOran > 0 ? "0" : parcaGroup.ToplamAdet.ToString(CultureInfo.InvariantCulture);

                            writer.WriteElementString("Adet", adetToWrite);
                            writer.WriteElementString("EkAdi", ekAdi);
                            writer.WriteElementString("EkOran", ekOran);

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                var kullaniciId = _kullaniciService.GetKullaniciIdByKullaniciAdi(_formArayuzu.lblSistemKullaniciMetinAl());
                _kullaniciHareketleriService.LogEkle(kullaniciId, "XmlDosyasiOlusturuldu", "Kesim Planı Ekle", $"Kullanıcı {txtId.Text.Trim()} numaralı kesim planı XML dosyası oluşturdu.");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML oluşturma sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
        }

        private string ExtractPoz(string kalipVerisi)
        {
            if (string.IsNullOrEmpty(kalipVerisi)) return "";
            var parcalar = kalipVerisi.Split('-');
            if (parcalar.Length >= 3)
            {
                string poz = parcalar[2].Trim();
                return poz.StartsWith("P", StringComparison.OrdinalIgnoreCase) ? poz : $"P{poz}";
            }
            return "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string islenmisVeri = IslenmisVeriAjan();
            _formArayuzu.RichTextBox4Yaz(islenmisVeri);
            _veriOkuma.PlazmaOku(islenmisVeri);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BaykalSayfaPozDagitimi();
            SayfaIDTabloVerileri();
            HesaplaEkAgirlikYuzdeleri();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AjanSayfaPozDagitimi();
            SayfaIDTabloVerileri();
            HesaplaEkAgirlikYuzdeleri();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            AdmSayfaPozDagitimi();
            SayfaIDTabloVerileri();
            HesaplaEkAgirlikYuzdeleri();
        }

        private void AjanSayfaPozDagitimi()
        {
            try
            {
                string id = txtId.Text.Trim();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var logLines = new List<string> {
            "=== AjanLog.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            $"txtId: {id}",
            "\nRichTextBox4 Verileri:"
        };

                Regex pozRegex = new Regex(
     @"ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,}\s*-\s*(\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2,}(?:\s*-\s*EK[A-Za-z0-9]*)?)",
     RegexOptions.IgnoreCase);

                Regex suffixRegex = new Regex(@"-(?!EK[A-Za-z0-9]*$)[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
                Regex sayfaSimpleRegex = new Regex(@"Sayfa:\s*(\d+)", RegexOptions.IgnoreCase);

                var lines4 = _formArayuzu.RichTextBox4SatirlariAl();
                Dictionary<string, int> sayfaSayaclari = new Dictionary<string, int>();
                HashSet<string> validSayfaNos = new HashSet<string>();

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int sayfaStart = line.IndexOf("(Sayfa:");
                    int sayfaEnd = sayfaStart >= 0 ? line.IndexOf(')', sayfaStart) : -1;

                    string parcaAdiFull = (sayfaStart >= 0 && sayfaEnd > sayfaStart)
                        ? line.Substring(0, sayfaEnd + 1).Trim()
                        : line.Trim();

                    var parts = line.Split(new[] { "(Sayfa: " }, StringSplitOptions.None);
                    if (parts.Length < 2) continue;

                    string sayfaNo = parts[1].Replace(")", "").Trim();
                    validSayfaNos.Add(sayfaNo);

                    string yerlesimNo = "00";
                    var yerlesimParts = line.Split(new[] { "(Yerleşim: " }, StringSplitOptions.None);
                    if (yerlesimParts.Length >= 2)
                    {
                        yerlesimNo = yerlesimParts[1].Split(')')[0].Trim();
                    }

                    if (!sayfaSayaclari.ContainsKey(sayfaNo))
                    {
                        sayfaSayaclari[sayfaNo] = 1;
                    }
                    else
                    {
                        sayfaSayaclari[sayfaNo]++;
                    }

                    Match pozMatch = pozRegex.Match(parcaAdiFull);
                    string poz = pozMatch.Success ? pozMatch.Groups[1].Value : "Poz Yok";

                    string parcaID = suffixRegex.Replace(parcaAdiFull, "");

                    string yerlesimID = $"{id}-{yerlesimNo.PadLeft(2, '0')}";
                    string sayfaID = $"{id}-{sayfaNo.PadLeft(2, '0')}";
                    string parcaIDFinal = $"{sayfaID}-{sayfaSayaclari[sayfaNo].ToString("D2")}";

                    logLines.Add($"{parcaAdiFull} => Poz: {poz}, Parça ID: {parcaIDFinal}, Yerleşim ID: {yerlesimID}, Sayfa ID: {sayfaID}");
                }


                logLines.Add("\nRichTextBox1 Verileri:");
                sayfaSayaclari.Clear();
                var lines1 = _formArayuzu.RichTextBox1SatirlariAl();
                int currentRealSayfaNo = 0;
                bool isToplamParcaPage = false;

                foreach (var line in lines1)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var sayfaMatch = Regex.Match(line, @"--------\s*Sayfa\s*(\d+)\s*--------");
                    if (sayfaMatch.Success)
                    {
                        currentRealSayfaNo = int.Parse(sayfaMatch.Groups[1].Value);
                        isToplamParcaPage = false;
                        continue;
                    }

                    var sayfaSimpleMatch = sayfaSimpleRegex.Match(line);
                    if (sayfaSimpleMatch.Success)
                    {
                        currentRealSayfaNo = int.Parse(sayfaSimpleMatch.Groups[1].Value);
                        isToplamParcaPage = false;
                        continue;
                    }

                    if (line.Contains("ToplamParçaKesmeListesi"))
                    {
                        isToplamParcaPage = true;
                        continue;
                    }

                    var parcaMatch = Regex.Match(line, @"^\d+\s+(ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,}\s*-\s*\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2,}(?:\s*-\s*EK[A-Za-z0-9]*)?)");
                    if (parcaMatch.Success && !isToplamParcaPage && validSayfaNos.Contains(currentRealSayfaNo.ToString()))
                    {
                        string parcaAdi = parcaMatch.Groups[1].Value;
                        Match pozMatch = pozRegex.Match(parcaAdi);
                        string poz = pozMatch.Success ? pozMatch.Groups[1].Value : "Poz Yok";

                        string adet = "";
                        Match adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}:\d{2}\s+(\d+)");
                        if (!adetMatch.Success)
                            adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}\s+(\d+)");
                        if (adetMatch.Success)
                            adet = adetMatch.Groups[1].Value;

                        string agirlik = "";
                        var agirlikMatch = Regex.Match(line, @"([0-9]{1,6}\.[0-9]{2,3})\s+(?:0:|\d{1,2}:)\d{2}:\d{2}");
                        if (agirlikMatch.Success)
                        {
                            agirlik = agirlikMatch.Groups[1].Value.Trim();
                        }
                        else
                        {
                            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length > 2 && double.TryParse(parts[parts.Length - 2].Replace(".", ","), out _))
                            {
                                agirlik = parts[parts.Length - 2];
                            }
                            else if (parts.Length > 3 && double.TryParse(parts[parts.Length - 3].Replace(".", ","), out _))
                            {
                                agirlik = parts[parts.Length - 3];
                            }
                        }

                        string temizParca = suffixRegex.Replace(parcaAdi, "");
                        string sayfaID = $"{id}-{currentRealSayfaNo.ToString("D2")}";
                        int parcaIndex = sayfaSayaclari.ContainsKey(currentRealSayfaNo.ToString()) ? sayfaSayaclari[currentRealSayfaNo.ToString()] + 1 : 1;
                        string parcaIDFinal = $"{sayfaID}-{parcaIndex.ToString("D2")}";

                        logLines.Add($"{line} (Sayfa:{currentRealSayfaNo}) => Adet: {adet}, Ağırlık: {agirlik}, Parça ID: {parcaIDFinal}, Sayfa ID: {sayfaID}");

                        sayfaSayaclari[currentRealSayfaNo.ToString()] = parcaIndex;
                    }
                }

                string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
                string logPath = Path.Combine(appPath, "AjanLog.txt");
                logLines.Add("\nSon Çıktı Verileri:");

                var rich4Dict = new Dictionary<string, (string Poz, string YerlesimID)>();
                var rich1Dict = new Dictionary<string, (string Adet, string Agirlik)>();

                foreach (var line in logLines)
                {
                    if (line.Contains("RichTextBox4 Verileri:") || line.Contains("RichTextBox1 Verileri:")) continue;

                    var match = Regex.Match(line, @"Poz: (.+?), Parça ID: (.+?), Yerleşim ID: (.+), Sayfa ID: (.+)");
                    if (match.Success)
                    {
                        string poz = match.Groups[1].Value.Trim();
                        string parcaID = match.Groups[2].Value.Trim();
                        string yerlesimID = match.Groups[3].Value.Trim();
                        rich4Dict[parcaID] = (poz, yerlesimID);
                    }
                }

                foreach (var line in logLines)
                {
                    if (!line.Contains("=> Adet:")) continue;

                    var match = Regex.Match(line, @"Adet: (\d+), Ağırlık: (.+?), Parça ID: (.+?), Sayfa ID");
                    if (match.Success)
                    {
                        string adet = match.Groups[1].Value.Trim();
                        string agirlik = match.Groups[2].Value.Trim();
                        string parcaID = match.Groups[3].Value.Trim();
                        rich1Dict[parcaID] = (adet, agirlik);
                    }
                }

                foreach (var kvp in rich4Dict)
                {
                    string parcaID = kvp.Key;
                    var (poz, yerlesimID) = kvp.Value;

                    if (rich1Dict.ContainsKey(parcaID))
                    {
                        var (adet, agirlik) = rich1Dict[parcaID];
                        logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Yerleşim ID: {yerlesimID}, Adet: {adet}, Ağırlık: {agirlik}");
                        dataGridView2.Rows.Add(poz, yerlesimID, adet, agirlik);
                    }
                }

                File.WriteAllLines(logPath, logLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BaykalSayfaPozDagitimi()
        {
            string baseId = txtId.Text.Trim();
            var logLines = new List<string>
    {
        "=== ProNest_Log.txt ===",
        $"İşlem Tarihi: {DateTime.Now}",
        $"BaseId: {baseId}"
    };
            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
            string logPath = Path.Combine(appPath, "ProNest_Log.txt");

            try
            {
                if (string.IsNullOrEmpty(baseId))
                {
                    logLines.Add("Hata: txtId alanı boş!");
                    MessageBox.Show("Lütfen txtId alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string text1 = _formArayuzu.RichTextBox1MetniAl().Trim();
                if (string.IsNullOrEmpty(text1))
                {
                    logLines.Add("Hata: richTextBox1 alanı boş!");
                    MessageBox.Show("Lütfen richTextBox1 alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string text4 = _formArayuzu.RichTextBox4MetniAl().Trim();
                if (string.IsNullOrEmpty(text4))
                {
                    logLines.Add("Hata: richTextBox4 alanı boş!");
                    MessageBox.Show("Lütfen richTextBox4 alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*--------", RegexOptions.IgnoreCase);
                Regex partRegex = new Regex(@"(\d+)(?:\s*-\s*(\d+))?\s+.*?(\d+-\d+-P\d+(?:-[\w\.\s]+)*)\s.*?(\d{1,3}(?:,\d{1,2})?)\s*kg", RegexOptions.IgnoreCase);
                Regex richTextBox4SayfaRegex = new Regex(@"\(Sayfa:\s*(\d+)\)", RegexOptions.IgnoreCase);
                Regex richTextBox4YerlesimRegex = new Regex(@"\(Yerleşim:\s*(\d+)\)", RegexOptions.IgnoreCase);

                var lines1 = text1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var lines4 = text4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                var richTextBox1Matches = new Dictionary<string, (int Adet, double Agirlik)>();
                var partInfoDict = new Dictionary<string, (string Poz, string SayfaID, int Adet, double Agirlik)>();
                string currentSayfa = "";
                Dictionary<string, int> sayfaSiraSayaclari = new Dictionary<string, int>();

                logLines.Add("\nRichTextBox1 Verileri:");
                foreach (var line in lines1)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var sayfaMatch = sayfaRegex.Match(line);
                    if (sayfaMatch.Success)
                    {
                        currentSayfa = sayfaMatch.Groups[1].Value.PadLeft(2, '0');
                        sayfaSiraSayaclari[currentSayfa] = 1;
                        continue;
                    }

                    var partMatch = partRegex.Match(line);
                    if (partMatch.Success)
                    {
                        if (!sayfaSiraSayaclari.ContainsKey(currentSayfa))
                            sayfaSiraSayaclari[currentSayfa] = 1;

                        string id = $"{baseId}-{currentSayfa}-{sayfaSiraSayaclari[currentSayfa].ToString("D2")}";
                        string startNum = partMatch.Groups[1].Value;
                        string endNum = partMatch.Groups[2].Value;
                        string agirlikStr = partMatch.Groups[4].Value.Trim();

                        if (!double.TryParse(agirlikStr, NumberStyles.Any, new CultureInfo("tr-TR"), out double agirlik))
                        {
                            logLines.Add($"Hata: Ağırlık değeri parse edilemedi: {agirlikStr}");
                            MessageBox.Show($"Ağırlık değeri parse edilemedi: {agirlikStr}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                        richTextBox1Matches[id] = (count, agirlik);
                        logLines.Add($"{id}: {line}, Adet: {count}, Ağırlık: {agirlik.ToString("F2", CultureInfo.InvariantCulture)} (Sayfa: {currentSayfa})");
                        sayfaSiraSayaclari[currentSayfa]++;
                    }
                }

                logLines.Add("\nRichTextBox4 Verileri:");
                string previousSayfa = "";
                string currentYerlesim = "";
                sayfaSiraSayaclari.Clear();

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var sayfaMatch = richTextBox4SayfaRegex.Match(line);
                    var yerlesimMatch = richTextBox4YerlesimRegex.Match(line);

                    if (sayfaMatch.Success)
                    {
                        currentSayfa = sayfaMatch.Groups[1].Value.PadLeft(2, '0');
                        if (!sayfaSiraSayaclari.ContainsKey(currentSayfa))
                            sayfaSiraSayaclari[currentSayfa] = 1;
                        previousSayfa = currentSayfa;
                    }
                    else
                    {
                        currentSayfa = previousSayfa;
                    }

                    currentYerlesim = yerlesimMatch.Success ? yerlesimMatch.Groups[1].Value.PadLeft(2, '0') : "00";
                    string id = $"{baseId}-{currentSayfa}-{sayfaSiraSayaclari[currentSayfa].ToString("D2")}";
                    string yerlesimId = $"{baseId}-{currentYerlesim}";
                    var parcaKoduMatch = Regex.Match(line, @"^(ST\d{2}-\d+(?:MM|mm)-)(.+)(?=\(Yerleşim:)");

                    if (!parcaKoduMatch.Success)
                    {
                        logLines.Add($"[Atlandı] {id}: Parça kodu bulunamadı -> {line}");
                        continue;
                    }

                    string pozbilgi = parcaKoduMatch.Groups[1].Value;
                    string poz = parcaKoduMatch.Groups[2].Value;

                    if (richTextBox1Matches.TryGetValue(id, out var info))
                    {
                        partInfoDict[id] = (poz, yerlesimId, info.Adet, info.Agirlik);
                        logLines.Add($"{id} => {poz}, {sayfaMatch}, {yerlesimMatch}, Yerleşim ID: {yerlesimId}");
                    }
                    else
                    {
                        logLines.Add($"[Uyarı] {id}: RichTextBox1 ile eşleşmedi -> {poz}");
                    }

                    sayfaSiraSayaclari[currentSayfa]++;
                }

                logLines.Add("\nSon Çıktı Verileri:");
                foreach (var kvp in partInfoDict)
                {
                    string parcaID = kvp.Key;
                    var (poz, yerlesimID, count, agirlik) = kvp.Value;
                    logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Yerleşim ID: {yerlesimID}, Adet: {count}, Ağırlık: {agirlik.ToString("F2", CultureInfo.InvariantCulture)}");
                    dataGridView2.Rows.Add(poz, yerlesimID, count, agirlik.ToString("F2", CultureInfo.InvariantCulture));
                }

                dataGridView2.Columns[3].DefaultCellStyle.Format = "F2";
                dataGridView2.Columns[3].DefaultCellStyle.FormatProvider = CultureInfo.InvariantCulture;

                var uniquePozs = partInfoDict.Values.Select(p => p.Poz).Distinct().OrderBy(p => p).ToList();
                logLines.Add($"\nBenzersiz pozlar: {string.Join(", ", uniquePozs)}");

                if (!uniquePozs.Any())
                {
                    logLines.Add("Hata: Hiçbir poz bulunamadı!");
                    MessageBox.Show("Hiçbir poz bulunamadı. Lütfen veri formatını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                logLines.Add($"Hata oluştu: {ex.Message}");
                MessageBox.Show($"Bir hata oluştu, lütfen veri formatını kontrol edin veya destek ekibiyle iletişime geçin: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                File.WriteAllLines(logPath, logLines);
            }
        }
        private void AdmSayfaPozDagitimi()
        {
            try
            {
                string baseId = txtId.Text.Trim();
                if (string.IsNullOrEmpty(baseId))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string text1 = _formArayuzu.RichTextBox1MetniAl().Trim();
                if (string.IsNullOrEmpty(text1))
                {
                    MessageBox.Show("Lütfen richTextBox1 alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string text4 = _formArayuzu.RichTextBox4MetniAl().Trim();
                if (string.IsNullOrEmpty(text4))
                {
                    MessageBox.Show("Lütfen richTextBox4 alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var logLines = new List<string>
        {
            "=== ADM_Log.txt ===",
            $"İşlem Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm:ss}",
            $"BaseId: {baseId}"
        };

                Regex partRegex = new Regex(@"(\d{5}\.\d{2,4}-\d+(?:-\d+)*-(?:P|00-)?P\d+)\s+(\d+)\s+([\d,.]+)\s+([\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase);
                Regex cncRegex = new Regex(@"^\s*(\d+)\s+(\d+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)", RegexOptions.IgnoreCase);
                Regex rich4Regex = new Regex(@"(?i)(ST\d+).*?(P\d+).*?(\d+AD).*?(\d{5}\.\d{2,4})", RegexOptions.IgnoreCase);

                var lines1 = text1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var lines4 = text4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                var partInfoDict = new Dictionary<string, (string Poz, string YerlesimID, int Adet, double Agirlik, string Id)>();
                var programSequenceDict = new Dictionary<string, int>();
                var programIdMap = new Dictionary<string, string>();
                var finalOutputList = new List<(string ParcaId, string Poz, string YerlesimId, int Adet, double Agirlik)>();
                int programCounter = 1;

                logLines.Add("\nRichTextBox1 Verileri:");
                string currentCncProgram = "";

                foreach (var line in lines1)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Contains("Part Quantity Length Weight") ||
                        line.Contains("Sayfa") || line.Contains("Nestings list") || line.Contains("Akyapak") ||
                        line.StartsWith("ID CNC") || line.Contains("Toplam sayfa sayısı") || line.Contains("%")) continue;

                    var cncMatch = cncRegex.Match(line);
                    if (cncMatch.Success)
                    {
                        currentCncProgram = cncMatch.Groups[1].Value;
                        if (!programIdMap.ContainsKey(currentCncProgram))
                        {
                            programIdMap[currentCncProgram] = programCounter.ToString("D2");
                            programCounter++;
                        }
                        programSequenceDict[currentCncProgram] = 0;
                        logLines.Add($"CNC Program Bulundu: {currentCncProgram}");
                        continue;
                    }

                    var partMatch = partRegex.Match(line.Trim());
                    if (partMatch.Success)
                    {
                        string poz = partMatch.Groups[1].Value;
                        string adetStr = partMatch.Groups[2].Value;
                        string agirlikStr = partMatch.Groups[4].Value;

                        if (int.TryParse(adetStr, out int adet) &&
                            double.TryParse(agirlikStr.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double agirlik))
                        {
                            string programId = programIdMap[currentCncProgram];
                            int sequence = programSequenceDict.ContainsKey(currentCncProgram) ? programSequenceDict[currentCncProgram] + 1 : 1;
                            programSequenceDict[currentCncProgram] = sequence;
                            string sequenceStr = sequence.ToString("D2");
                            string parcaId = poz;
                            string uniqueId = $"{baseId}-{programId}-{sequenceStr}";
                            string dictKey = $"{parcaId}|{currentCncProgram}";

                            partInfoDict[dictKey] = (poz, $"{baseId}-{programId}", adet, agirlik, uniqueId);
                            logLines.Add($"Parça Bulundu: {parcaId} => Poz: {poz}, Program: {currentCncProgram}, Adet: {adet}, Ağırlık: {agirlik:F2}, Id: {uniqueId}");
                        }
                    }
                }

                logLines.Add($"partInfoDict: {string.Join(", ", partInfoDict.Select(kvp => $"{kvp.Key}:{kvp.Value.Id}"))}");

                logLines.Add("\nRichTextBox4 Verileri:");
                programSequenceDict.Clear();

                logLines.Add($"programIdMap: {string.Join(", ", programIdMap.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string cleanedLine = Regex.Replace(line.Trim(), @"\s+", " ").Replace("\r", "").Replace("\n", "");
                    cleanedLine = Regex.Replace(cleanedLine, @"\p{C}", "");

                    var match = rich4Regex.Match(cleanedLine);
                    if (match.Success)
                    {
                        // ANAHTAR BİLGİLERİ YAKALA
                        string materialType = match.Groups[1].Value; // ST37
                        string pNumber = match.Groups[2].Value; // P10
                        string adType = match.Groups[3].Value; // 2AD
                        string pozPrefix = match.Groups[4].Value; // 25166.01

                        // POZ SIMPLE'I MANUEL OLARAK ÇIKAR
                        string pozBeforeP = Regex.Match(cleanedLine, $@"{Regex.Escape(materialType)}.*?-(.*?)-{Regex.Escape(pNumber)}").Groups[1].Value;
                        string[] pozParts = pozBeforeP.Split('-');

                        // ESNEK POZ SIMPLE - Son iki parçayı al
                        string pozSimple;
                        if (pozParts.Length >= 2)
                        {
                            pozSimple = $"{pozParts[pozParts.Length - 2]}-{pozParts[pozParts.Length - 1]}";
                        }
                        else if (pozParts.Length == 1)
                        {
                            pozSimple = pozParts[0];
                        }
                        else
                        {
                            pozSimple = "";
                        }

                        string fullPozSimple = $"{pozSimple}-{pNumber}"; // 211-00-P10
                        string programNo = Regex.Match(cleanedLine, @"Program\s*[:\s]\s*(\d+)").Groups[1].Value;
                        string adetStr = Regex.Match(cleanedLine, @"Miktar\s*[:\s]\s*(\d+)").Groups[1].Value;
                        string tekrarStr = Regex.Match(cleanedLine, @"Program Tekrarı\s*[:\s]\s*(\d+)").Groups[1].Value;

                        string programId = programIdMap.ContainsKey(programNo) ? programIdMap[programNo] : "00";

                        int sequence = programSequenceDict.ContainsKey(programNo) ? programSequenceDict[programNo] + 1 : 1;
                        programSequenceDict[programNo] = sequence;
                        string sequenceStr = sequence.ToString("D2");
                        string parcaId = $"{pozPrefix}-{fullPozSimple}"; // 25166.01-211-00-P10

                        // DİNAMİK formattedPoz - AD tipini kullan
                        string formattedPoz = $"{fullPozSimple}-{adType}-{pozPrefix}"; // 211-00-P10-2AD-25166.01

                        if (int.TryParse(adetStr, out int miktar) && int.TryParse(tekrarStr, out int tekrar))
                        {
                            string dictKey = $"{parcaId}|{programNo}";
                            if (partInfoDict.TryGetValue(dictKey, out var partInfo))
                            {
                                logLines.Add($"Eşleşme Bulundu: {cleanedLine}, Id: {partInfo.Id}");
                                finalOutputList.Add((partInfo.Id, formattedPoz, $"{baseId}-{programId}", miktar, partInfo.Agirlik));
                            }
                            else
                            {
                                logLines.Add($"Eşleşme Bulunamadı: {cleanedLine}");
                                logLines.Add($"Hata Detayı: dictKey={dictKey}, parcaId={parcaId}, programId={programId}, programNo={programNo}");
                                var relatedParts = partInfoDict.Keys.Where(k => k.StartsWith(parcaId + "|")).Select(k => $"{k}:{partInfoDict[k].Id}");
                                logLines.Add($"partInfoDict'teki ilgili pozlar: {string.Join(", ", relatedParts)}");
                            }
                        }
                        else
                        {
                            logLines.Add($"Hata: Adet veya Tekrar parse edilemedi: {cleanedLine}");
                        }
                    }
                    else
                    {
                        logLines.Add($"Eşleşme Bulunamadı: {cleanedLine}");
                        string possibleProgram = Regex.Match(cleanedLine, @"Program\s*[:\s]\s*(\d+)").Groups[1].Value;
                        if (!string.IsNullOrEmpty(possibleProgram))
                        {
                            int sequence = programSequenceDict.ContainsKey(possibleProgram) ? programSequenceDict[possibleProgram] + 1 : 1;
                            programSequenceDict[possibleProgram] = sequence;
                        }
                    }
                }

                dataGridView2.Rows.Clear();
                foreach (var output in finalOutputList)
                {
                    dataGridView2.Rows.Add(
                        output.Poz,
                        output.YerlesimId,
                        output.Adet,
                        output.Agirlik.ToString("F2", CultureInfo.InvariantCulture)
                    );
                }

                logLines.Add("\nSon Çıktı Verileri:");
                if (finalOutputList.Count == 0)
                {
                    logLines.Add("Uyarı: Son Çıktı Verileri boş! Eşleşme bulunamadı veya veri işlenemedi.");
                }
                else
                {
                    logLines.AddRange(finalOutputList.Select(output =>
                        $"Parça ID: {output.ParcaId} => Poz: {output.Poz}, Yerleşim ID: {output.YerlesimId}, Adet: {output.Adet}, Ağırlık: {output.Agirlik:F2}"));
                }

                string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
                string logPath = Path.Combine(appPath, "ADM_Log.txt");
                File.WriteAllLines(logPath, logLines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                string errorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ADM_Hata.txt");
                File.WriteAllText(errorPath, $"Hata: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n{ex.ToString()}", Encoding.UTF8);
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}\nDetaylar masaüstündeki ADM_Hata.txt dosyasına kaydedildi.",
                               "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void AdmSayfaPozDagitimi()
        //{
        //    try
        //    {
        //        string baseId = txtId.Text.Trim();
        //        if (string.IsNullOrEmpty(baseId))
        //        {
        //            MessageBox.Show("Lütfen txtId alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        string text1 = _formArayuzu.RichTextBox1MetniAl().Trim();
        //        if (string.IsNullOrEmpty(text1))
        //        {
        //            MessageBox.Show("Lütfen richTextBox1 alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        string text4 = _formArayuzu.RichTextBox4MetniAl().Trim();
        //        if (string.IsNullOrEmpty(text4))
        //        {
        //            MessageBox.Show("Lütfen richTextBox4 alanını doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        var logLines = new List<string>
        //{
        //    "=== ADM_Log.txt ===",
        //    $"İşlem Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm:ss}",
        //    $"BaseId: {baseId}"
        //};

        //        Regex partRegex = new Regex(@"(\d{5}\.\d{2,3}-\d{3}-00-P\d+)\s+(\d+)\s+([\d,.]+)\s+([\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase);
        //        Regex cncRegex = new Regex(@"^\s*(\d+)\s+(\d+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)", RegexOptions.IgnoreCase);
        //        Regex rich4Regex = new Regex(@"ST\d+-[A-Za-z0-9]+-(?<poz>\d+-\d+-P\d+)-\d*AD-(?<pozPrefix>[\d.]+).*\(Sayfa\s*[:]\s*(?<sayfa>\d+).*M[iı]ktar\s*[:\s]\s*(?<adet>\d+).*Program\s*[:\s]\s*(?<program>\d+)\s*,\s*Program Tekrarı\s*[:\s]\s*(?<tekrar>\d+)\)", RegexOptions.IgnoreCase);

        //        var lines1 = text1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        //        var lines4 = text4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        //        var partInfoDict = new Dictionary<string, (string Poz, string YerlesimID, int Adet, double Agirlik, string Id)>();
        //        var programSequenceDict = new Dictionary<string, int>();
        //        var programIdMap = new Dictionary<string, string>();
        //        var finalOutputList = new List<(string ParcaId, string Poz, string YerlesimId, int Adet, double Agirlik)>();
        //        int programCounter = 1;

        //        logLines.Add("\nRichTextBox1 Verileri:");
        //        string currentCncProgram = "";

        //        foreach (var line in lines1)
        //        {
        //            if (string.IsNullOrWhiteSpace(line) || line.Contains("Part Quantity Length Weight") ||
        //                line.Contains("Sayfa") || line.Contains("Nestings list") || line.Contains("Akyapak") ||
        //                line.StartsWith("ID CNC") || line.Contains("Toplam sayfa sayısı") || line.Contains("%")) continue;

        //            var cncMatch = cncRegex.Match(line);
        //            if (cncMatch.Success)
        //            {
        //                currentCncProgram = cncMatch.Groups[1].Value;
        //                if (!programIdMap.ContainsKey(currentCncProgram))
        //                {
        //                    programIdMap[currentCncProgram] = programCounter.ToString("D2");
        //                    programCounter++;
        //                }
        //                programSequenceDict[currentCncProgram] = 0;
        //                logLines.Add($"CNC Program Bulundu: {currentCncProgram}");
        //                continue;
        //            }

        //            var partMatch = partRegex.Match(line.Trim());
        //            if (partMatch.Success)
        //            {
        //                string poz = partMatch.Groups[1].Value;
        //                string adetStr = partMatch.Groups[2].Value;
        //                string agirlikStr = partMatch.Groups[4].Value;

        //                if (int.TryParse(adetStr, out int adet) &&
        //                    double.TryParse(agirlikStr.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double agirlik))
        //                {
        //                    string programId = programIdMap[currentCncProgram];
        //                    int sequence = programSequenceDict.ContainsKey(currentCncProgram) ? programSequenceDict[currentCncProgram] + 1 : 1;
        //                    programSequenceDict[currentCncProgram] = sequence;
        //                    string sequenceStr = sequence.ToString("D2");
        //                    string parcaId = poz;
        //                    string uniqueId = $"{baseId}-{programId}-{sequenceStr}";
        //                    string dictKey = $"{parcaId}|{currentCncProgram}";

        //                    partInfoDict[dictKey] = (poz, $"{baseId}-{programId}", adet, agirlik, uniqueId);
        //                    logLines.Add($"Parça Bulundu: {parcaId} => Poz: {poz}, Program: {currentCncProgram}, Adet: {adet}, Ağırlık: {agirlik:F2}, Id: {uniqueId}");
        //                }
        //            }
        //        }

        //        logLines.Add($"partInfoDict: {string.Join(", ", partInfoDict.Select(kvp => $"{kvp.Key}:{kvp.Value.Id}"))}");

        //        logLines.Add("\nRichTextBox4 Verileri:");
        //        programSequenceDict.Clear();

        //        logLines.Add($"programIdMap: {string.Join(", ", programIdMap.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");

        //        foreach (var line in lines4)
        //        {
        //            if (string.IsNullOrWhiteSpace(line)) continue;

        //            string cleanedLine = Regex.Replace(line.Trim(), @"\s+", " ").Replace("\r", "").Replace("\n", "");
        //            cleanedLine = Regex.Replace(cleanedLine, @"\p{C}", "");

        //            var match = rich4Regex.Match(cleanedLine);
        //            if (match.Success)
        //            {
        //                string pozSimple = match.Groups["poz"].Value;
        //                string pozPrefix = match.Groups["pozPrefix"].Value;
        //                string programNo = match.Groups["program"].Value;
        //                string adetStr = match.Groups["adet"].Value;
        //                string tekrarStr = match.Groups["tekrar"].Value;

        //                string programId = programIdMap.ContainsKey(programNo) ? programIdMap[programNo] : "00";

        //                int sequence = programSequenceDict.ContainsKey(programNo) ? programSequenceDict[programNo] + 1 : 1;
        //                programSequenceDict[programNo] = sequence;
        //                string sequenceStr = sequence.ToString("D2");
        //                string parcaId = $"{pozPrefix}-{pozSimple}";

        //                string formattedPoz = $"{pozSimple}-2AD-{pozPrefix}";

        //                if (int.TryParse(adetStr, out int miktar) && int.TryParse(tekrarStr, out int tekrar))
        //                {
        //                    string dictKey = $"{parcaId}|{programNo}";
        //                    if (partInfoDict.TryGetValue(dictKey, out var partInfo))
        //                    {
        //                        logLines.Add($"Eşleşme Bulundu: {cleanedLine}, Id: {partInfo.Id}");
        //                        finalOutputList.Add((partInfo.Id, formattedPoz, $"{baseId}-{programId}", miktar, partInfo.Agirlik));
        //                    }
        //                    else
        //                    {
        //                        logLines.Add($"Eşleşme Bulunamadı: {cleanedLine}");
        //                        logLines.Add($"Hata Detayı: dictKey={dictKey}, parcaId={parcaId}, programId={programId}, programNo={programNo}");
        //                        var relatedParts = partInfoDict.Keys.Where(k => k.StartsWith(parcaId + "|")).Select(k => $"{k}:{partInfoDict[k].Id}");
        //                        logLines.Add($"partInfoDict'teki ilgili pozlar: {string.Join(", ", relatedParts)}");
        //                    }
        //                }
        //                else
        //                {
        //                    logLines.Add($"Hata: Adet veya Tekrar parse edilemedi: {cleanedLine}");
        //                }
        //            }
        //            else
        //            {
        //                logLines.Add($"Eşleşme Bulunamadı: {cleanedLine}");
        //                string possibleProgram = Regex.Match(cleanedLine, @"Program\s*[:\s]\s*(\d+)").Groups[1].Value;
        //                if (!string.IsNullOrEmpty(possibleProgram))
        //                {
        //                    int sequence = programSequenceDict.ContainsKey(possibleProgram) ? programSequenceDict[possibleProgram] + 1 : 1;
        //                    programSequenceDict[possibleProgram] = sequence;
        //                }
        //            }
        //        }

        //        dataGridView2.Rows.Clear();
        //        foreach (var output in finalOutputList)
        //        {
        //            dataGridView2.Rows.Add(
        //                output.Poz,
        //                output.YerlesimId,
        //                output.Adet,
        //                output.Agirlik.ToString("F2", CultureInfo.InvariantCulture)
        //            );
        //        }

        //        logLines.Add("\nSon Çıktı Verileri:");
        //        if (finalOutputList.Count == 0)
        //        {
        //            logLines.Add("Uyarı: Son Çıktı Verileri boş! Eşleşme bulunamadı veya veri işlenemedi.");
        //        }
        //        else
        //        {
        //            logLines.AddRange(finalOutputList.Select(output =>
        //                $"Parça ID: {output.ParcaId} => Poz: {output.Poz}, Yerleşim ID: {output.YerlesimId}, Adet: {output.Adet}, Ağırlık: {output.Agirlik:F2}"));
        //        }

        //        string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
        //        string logPath = Path.Combine(appPath, "ADM_Log.txt");
        //        File.WriteAllLines(logPath, logLines, Encoding.UTF8);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ADM_Hata.txt");
        //        File.WriteAllText(errorPath, $"Hata: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n{ex.ToString()}", Encoding.UTF8);
        //        MessageBox.Show($"Bir hata oluştu:\n{ex.Message}\nDetaylar masaüstündeki ADM_Hata.txt dosyasına kaydedildi.",
        //                       "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void SayfaIDTabloVerileri()
        {
            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
            string logPath = Path.Combine(appPath, "SayfaIDlog.txt");

            File.WriteAllText(logPath, $"[{DateTime.Now}] Yeni işlem başladı\n");

            using (StreamWriter log = new StreamWriter(logPath, true))
            {
                log.WriteLine($"[{DateTime.Now}] İşlem başladı");

                dataGridView3.Rows.Clear();
                string metin4 = _formArayuzu.RichTextBox4MetniAl();
                string metin1 = _formArayuzu.RichTextBox1MetniAl();
                string idValue = txtId.Text;

                if (string.IsNullOrWhiteSpace(metin4))
                {
                    MessageBox.Show("richTextBox4 boş!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.WriteLine($"[{DateTime.Now}] Hata: richTextBox4 boş");
                    return;
                }

                if (string.IsNullOrWhiteSpace(idValue) || !int.TryParse(idValue, out int id))
                {
                    MessageBox.Show("txtID boş veya geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.WriteLine($"[{DateTime.Now}] Hata: txtID boş veya geçersiz: {idValue}");
                    return;
                }

                log.WriteLine($"[{DateTime.Now}] txtID: {idValue}");
                log.WriteLine($"[{DateTime.Now}] Seçili buton: {_seciliButon}");

                Dictionary<string, string> yerlesimSayfa = new Dictionary<string, string>();
                Dictionary<string, string> yerlesimTekrar = new Dictionary<string, string>();

                if (_seciliButon == btnAdm)
                {
                    log.WriteLine($"[{DateTime.Now}] btnAdm dalına girildi");

                    Regex rich4Regex = new Regex(
        @"\(Sayfa\s*[:]\s*(?<sayfa>\d+)\s*,\s*M[iı]ktar\s*[:]\s*(?<adet>\d+)\s*,\s*Program\s*[:]\s*(?<program>\d+)\s*,\s*Program Tekrarı\s*[:]\s*(?<tekrar>\d+)\)",
        RegexOptions.IgnoreCase);

                    var lines4 = metin4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    Dictionary<string, int> programSequence = new Dictionary<string, int>();
                    Dictionary<string, string> programIdMap = new Dictionary<string, string>();
                    int programCounter = 1;

                    Regex cncRegex = new Regex(@"^\s*(\d+)\s+(\d+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)", RegexOptions.IgnoreCase);
                    var lines1 = metin1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    foreach (var line in lines1)
                    {
                        var cncMatch = cncRegex.Match(line);
                        if (cncMatch.Success)
                        {
                            string programNo = cncMatch.Groups[1].Value;
                            if (!programIdMap.ContainsKey(programNo))
                            {
                                programIdMap[programNo] = programCounter.ToString("D2");
                                programCounter++;
                            }
                        }
                    }

                    log.WriteLine($"[{DateTime.Now}] RichTextBox4 işleniyor, satır sayısı: {lines4.Length}");

                    foreach (var line in lines4)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            log.WriteLine($"[{DateTime.Now}] Boş satır atlandı");
                            continue;
                        }

                        string cleanedLine = Regex.Replace(line.Trim(), @"\s+", " ");
                        log.WriteLine($"[{DateTime.Now}] İşlenen satır: {cleanedLine}");

                        var match4 = rich4Regex.Match(cleanedLine);
                        if (match4.Success)
                        {
                            string sayfaNo4 = match4.Groups["sayfa"].Value;
                            string adet = match4.Groups["tekrar"].Value;
                            string programNo = match4.Groups["program"].Value;

                            string programId = programIdMap.ContainsKey(programNo) ? programIdMap[programNo] : programNo.PadLeft(2, '0');
                            string birlesikIDAdm = $"{idValue}-{programId}";

                            int sequence = programSequence.ContainsKey(programNo) ? programSequence[programNo] + 1 : 1;
                            programSequence[programNo] = sequence;

                            if (!yerlesimSayfa.ContainsKey(programNo))
                            {
                                yerlesimSayfa[programNo] = sayfaNo4;
                                yerlesimTekrar[programNo] = adet;

                                int tekrarSayisi = int.Parse(adet);
                                for (int t = 1; t <= tekrarSayisi; t++)
                                {
                                    string tekrarID = $"{birlesikIDAdm}-{t.ToString("D2")}";
                                    int newRowIndex = dataGridView3.Rows.Add();
                                    dataGridView3.Rows[newRowIndex].Cells[0].Value = tekrarID;
                                    dataGridView3.Rows[newRowIndex].Cells[1].Value = sayfaNo4;
                                    dataGridView3.Rows[newRowIndex].Cells[2].Value = "1";
                                    log.WriteLine($"[{DateTime.Now}] Tekrar satırı eklendi: {tekrarID}, Sayfa: {sayfaNo4}, Miktar: 1");
                                }
                            }
                            else
                            {
                                log.WriteLine($"[{DateTime.Now}] Uyarı: Program {programNo} zaten işlendi, atlanıyor");
                            }
                        }
                        else
                        {
                            log.WriteLine($"[{DateTime.Now}] Hata: Satır ayrıştırılamadı: {cleanedLine}");
                        }
                    }
                }
                else
                {
                    MatchCollection matches = Regex.Matches(metin4, @"\(Yerleşim:\s*(\d+)\)\(Sayfa:\s*(\d+)\)");
                    foreach (Match match in matches)
                    {
                        string yerlesimNo = match.Groups[1].Value;
                        string sayfaNo = match.Groups[2].Value;

                        if (!yerlesimSayfa.ContainsKey(yerlesimNo))
                        {
                            yerlesimSayfa[yerlesimNo] = sayfaNo;

                            string formatliYerlesim = int.Parse(yerlesimNo).ToString("D2");
                            string birlesikID = $"{idValue}-{formatliYerlesim}";

                            string[] sayfalar = metin1.Split(new[] { "-------- Sayfa" }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sayfa in sayfalar)
                            {
                                if (!Regex.IsMatch(sayfa, $@"{sayfaNo}\s+--------")) continue;

                                if (_seciliButon == btnBaykal)
                                {
                                    string[] lines = sayfa.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    bool bulundu = false;
                                    foreach (var line in lines)
                                    {
                                        Match kesmeMatch = Regex.Match(line, @"Kesme sayısı[:\s]*?(\d+)", RegexOptions.IgnoreCase);
                                        if (kesmeMatch.Success)
                                        {
                                            yerlesimTekrar[yerlesimNo] = kesmeMatch.Groups[1].Value;
                                            log.WriteLine($"[{DateTime.Now}] Yerleşim {yerlesimNo} için Kesme sayısı: {kesmeMatch.Groups[1].Value}");
                                            bulundu = true;
                                            break;
                                        }
                                    }
                                    if (!bulundu)
                                    {
                                        yerlesimTekrar[yerlesimNo] = "0";
                                        log.WriteLine($"[{DateTime.Now}] Hata: Sayfa {sayfaNo} için Kesme sayısı bulunamadı");
                                    }
                                }
                                else if (_seciliButon == btnAjan)
                                {
                                    Match levhaMatch = Regex.Match(sayfa, @"ProgramTekrarı\s*\(Sacadet\)\s*(\d+)", RegexOptions.IgnoreCase);

                                    if (levhaMatch.Success)
                                    {
                                        yerlesimTekrar[yerlesimNo] = levhaMatch.Groups[1].Value;
                                        log.WriteLine($"[{DateTime.Now}] Yerleşim {yerlesimNo} için Kesme sayısı: {levhaMatch.Groups[1].Value}");
                                    }
                                    else
                                    {
                                        yerlesimTekrar[yerlesimNo] = "0";
                                        log.WriteLine($"[{DateTime.Now}] Hata: Sayfa {sayfaNo} için Kesme sayısı bulunamadı");
                                    }
                                }
                                break;
                            }

                            if (yerlesimTekrar.ContainsKey(yerlesimNo))
                            {
                                int tekrarSayisi = int.Parse(yerlesimTekrar[yerlesimNo]);
                                for (int t = 1; t <= tekrarSayisi; t++)
                                {
                                    string tekrarID = $"{birlesikID}-{t.ToString("D2")}";
                                    int newRowIndex = dataGridView3.Rows.Add();
                                    dataGridView3.Rows[newRowIndex].Cells[0].Value = tekrarID;
                                    dataGridView3.Rows[newRowIndex].Cells[1].Value = sayfaNo;
                                    dataGridView3.Rows[newRowIndex].Cells[2].Value = "1";
                                    log.WriteLine($"[{DateTime.Now}] Tekrar satırı eklendi: {tekrarID}, Sayfa: {sayfaNo}, Miktar: 1");
                                }
                            }
                        }
                    }

                    if (matches.Count == 0)
                    {
                        log.WriteLine($"[{DateTime.Now}] Hata: RichTextBox4'te yerleşim/sayfa eşleşmesi bulunamadı");
                    }
                }

                if (_seciliButon != btnBaykal && _seciliButon != btnAjan && _seciliButon != btnAdm)
                {
                    log.WriteLine($"[{DateTime.Now}] Hata: Geçersiz buton seçimi: {_seciliButon}");
                }

                log.WriteLine($"[{DateTime.Now}] DataGridView3 İçeriği:");
                if (dataGridView3.Rows.Count == 0)
                {
                    log.WriteLine($"[{DateTime.Now}] Uyarı: DataGridView3 boş, hiçbir satır eklenmedi");
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                    {
                        if (row.IsNewRow) continue;
                        log.WriteLine($"[{DateTime.Now}] Satır: ID={row.Cells[0].Value}, Sayfa={row.Cells[1].Value}, Miktar/Kesme Sayısı={row.Cells[2].Value}");
                    }
                }

                log.WriteLine($"[{DateTime.Now}] İşlem tamamlandı");
            }
        }
        private void HesaplaEkAgirlikYuzdeleri()
        {
            try
            {
                var logLines = new List<string> {
            "=== EkAdetAgirlikYuzdeLog.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            "\nDataGridView2 Veri Analizi:"
        };

                Regex ekRegex = new Regex(@"-EK\d*(?=\s|$)", RegexOptions.IgnoreCase);

                if (!dataGridView2.Columns.Contains("Oran"))
                {
                    dataGridView2.Columns.Add("Oran", "Toplam Ağırlık Oranı");
                }

                var pozGroups = new Dictionary<string, List<(string Poz, string SayfaID, int Adet, double Agirlik, DataGridViewRow Row, string D3PageId)>>();

                var pageIdMap = dataGridView3.Rows
                    .Cast<DataGridViewRow>()
                    .Where(row => !row.IsNewRow && row.Cells[0].Value != null)
                    .Select(row => row.Cells[0].Value.ToString().Trim())
                    .GroupBy(pageId => pageId.Split('-').Take(2).Aggregate((a, b) => $"{a}-{b}"))
                    .ToDictionary(g => g.Key, g => g.Select(id => id).ToList());

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.IsNewRow) continue;

                    string poz = row.Cells[0].Value?.ToString() ?? "";
                    string sayfaID = row.Cells[1].Value?.ToString() ?? "";
                    string adetStr = row.Cells[2].Value?.ToString() ?? "0";
                    string agirlikStr = row.Cells[3].Value?.ToString() ?? "0";

                    agirlikStr = agirlikStr.Replace(",", ".");

                    if (!int.TryParse(adetStr, out int adet) ||
                        !double.TryParse(agirlikStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double agirlik))
                    {
                        logLines.Add($"Hata: Geçersiz adet veya ağırlık değeri - Poz: {poz}");
                        continue;
                    }

                    var d3PageIds = pageIdMap.ContainsKey(sayfaID) ? pageIdMap[sayfaID] : new List<string> { sayfaID };

                    if (ekRegex.IsMatch(poz))
                    {
                        string basePoz = ekRegex.Replace(poz, "");
                        string[] parts = basePoz.Split('-');
                        string key;
                        if (parts.Length >= 5)
                        {
                            string kalip_prefix = parts[0];
                            string poz_part = parts[2];
                            string proje = parts[4];
                            key = $"{kalip_prefix},{poz_part},{proje}";
                        }
                        else
                        {
                            key = basePoz;
                        }
                        if (!pozGroups.ContainsKey(key))
                            pozGroups[key] = new List<(string, string, int, double, DataGridViewRow, string)>();

                        foreach (var d3PageId in d3PageIds)
                        {
                            pozGroups[key].Add((poz, sayfaID, adet, agirlik, row, d3PageId));
                        }
                    }
                }

                logLines.Add("\nEK İbaresi İçeren Pozlar ve Yüzde Hesaplamaları:");

                foreach (var group in pozGroups)
                {
                    string key = group.Key;
                    var items = group.Value;

                    double toplamAgirlik = items.Sum(item => item.Agirlik * item.Adet);

                    logLines.Add($"\nPoz: {key}, Toplam Ağırlık: {toplamAgirlik.ToString().Replace('.', ',')}");

                    foreach (var item in items)
                    {
                        double oran = toplamAgirlik != 0 ? (item.Agirlik * item.Adet) / toplamAgirlik : 0;
                        logLines.Add($"  - Poz: {item.Poz}, Sayfa ID: {item.D3PageId}, Adet: {item.Adet}, Ağırlık: {item.Agirlik.ToString().Replace('.', ',')}, Parçanın Toplam Ağırlığa Oranı: {oran.ToString("F6").Replace('.', ',')}");
                        item.Row.Cells["Oran"].Value = $"{oran:F6}";
                    }
                }

                string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
                string logPath = Path.Combine(appPath, "EkAdetAgirlikYuzdeLog.txt");
                File.WriteAllLines(logPath, logLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            string islenmisVeri = IslenmisVeriADM();
            _formArayuzu.RichTextBox4Yaz(islenmisVeri);
            _veriOkuma.AdmOku(islenmisVeri);
        }

        private void btnAjan_Click(object sender, EventArgs e)
        {
            IDVer();
        }

        private void btnBaykal_Click(object sender, EventArgs e)
        {
            IDVer();
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            IDVer();
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Yazdırma işlemine başlamadan önce IFS'e yüklediğiniz XML'in hatasız olduğundan emin olun. İşleme devam etmek istiyor musunuz?",
                "Yazdırma Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            GlobalFontSettings.FontResolver = new CEKA_APP.Helper.PlatformFontResolver();

            if (string.IsNullOrEmpty(txtDosya.Text) || !File.Exists(txtDosya.Text))
            {
                MessageBox.Show("Önce PDF seçiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Lütfen txtId alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_seciliButon != btnAdm && (dataGridView3.Rows.Count == 0 || dataGridView3.Rows[0].IsNewRow))
            {
                MessageBox.Show("DataGridView3'te veri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"ModifiedPDF_{Guid.NewGuid()}.pdf");

                if (_seciliButon == btnAdm)
                {
                    using (var pdfDocument = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(txtDosya.Text), new iText.Kernel.Pdf.PdfWriter(tempFilePath)))
                    {
                        var uniqueBaseNumbers = dataGridView2.Rows
                            .Cast<DataGridViewRow>()
                            .Where(row => !row.IsNewRow && row.Cells[1].Value != null)
                            .Select(row => row.Cells[1].Value.ToString().Trim())
                            .Distinct()
                            .OrderBy(num => num)
                            .ToList();

                        var pageIdMap = dataGridView3.Rows
                            .Cast<DataGridViewRow>()
                            .Where(row => !row.IsNewRow && row.Cells[0].Value != null)
                            .Select(row => row.Cells[0].Value.ToString().Trim())
                            .GroupBy(pageId => pageId.Split('-').Take(2).Aggregate((a, b) => $"{a}-{b}"))
                            .ToDictionary(g => g.Key, g => g.OrderBy(id => id).ToList());

                        int numberIndex = 0;

                        var textLocations = FindCncLocations(txtDosya.Text);

                        var locationsByPage = textLocations
                            .GroupBy(loc => loc.PageNumber)
                            .OrderBy(g => g.Key);

                        foreach (var pageGroup in locationsByPage)
                        {
                            if (numberIndex >= uniqueBaseNumbers.Count) break;

                            int pageNumber = pageGroup.Key;
                            if (pageNumber < 1 || pageNumber > pdfDocument.GetNumberOfPages()) continue;

                            var page = pdfDocument.GetPage(pageNumber);
                            var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                            var pageSize = page.GetPageSize();
                            double pageWidth = pageSize.GetWidth();
                            double marginPoint = 2 * 28.3465;

                            var sortedLocations = pageGroup.OrderBy(loc => loc.Y).Reverse().ToList();

                            foreach (var location in sortedLocations)
                            {
                                if (numberIndex >= uniqueBaseNumbers.Count) break;

                                string baseNumber = uniqueBaseNumbers[numberIndex];
                                string textToWrite;
                                if (pageIdMap.ContainsKey(baseNumber))
                                {
                                    var ids = pageIdMap[baseNumber];
                                    if (ids.Count > 1)
                                    {
                                        var firstId = ids.First();
                                        var shortIds = ids.Select(id => id.Split('-').Last()).ToList();
                                        textToWrite = $"{firstId}/{string.Join("/", shortIds.Skip(1))}";
                                    }
                                    else
                                    {
                                        textToWrite = ids.First();
                                    }
                                }
                                else
                                {
                                    textToWrite = baseNumber;
                                }

                                double xOffset = 600;
                                double yOffset = 20;
                                float fontSize = 14f;
                                var font = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
                                float textWidth = font.GetWidth(textToWrite, fontSize);

                                if (location.X + xOffset + textWidth > pageWidth - marginPoint)
                                {
                                    xOffset = pageWidth - textWidth - marginPoint - location.X;
                                }

                                canvas.BeginText()
                                      .SetFontAndSize(font, fontSize)
                                      .SetTextMatrix((float)(location.X + xOffset), (float)(location.Y + yOffset))
                                      .ShowText(textToWrite)
                                      .EndText();

                                numberIndex++;
                            }
                        }
                    }
                }
                else
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var input = PdfSharp.Pdf.IO.PdfReader.Open(txtDosya.Text, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                        {
                            var pageGroups = dataGridView3.Rows
                                .Cast<DataGridViewRow>()
                                .Where(row => !row.IsNewRow && row.Cells[0].Value != null && row.Cells[1].Value != null)
                                .GroupBy(row => row.Cells[1].Value.ToString().Trim())
                                .ToDictionary(g => g.Key, g => g.Select(row => row.Cells[0].Value.ToString().Trim()).OrderBy(id => id).ToList());

                            foreach (var group in pageGroups)
                            {
                                string sayfaNoStr = group.Key;
                                if (!int.TryParse(sayfaNoStr, out int sayfaNo) || sayfaNo < 1 || sayfaNo > input.PageCount)
                                {
                                    MessageBox.Show($"Geçersiz sayfa numarası: {sayfaNoStr}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }

                                var ids = group.Value;
                                string metin;
                                if (ids.Count > 1)
                                {
                                    var firstId = ids.First();
                                    var shortIds = ids.Select(id => id.Split('-').Last()).ToList();
                                    metin = $"{firstId}/{string.Join("/", shortIds.Skip(1))}";
                                }
                                else
                                {
                                    metin = ids.First();
                                }
                                float fontSize = metin.Length > 50 ? 16f : 20f;

                                var gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(input.Pages[sayfaNo - 1], PdfSharp.Drawing.XGraphicsPdfPageOptions.Append);
                                var font = new PdfSharp.Drawing.XFont("Arial", fontSize);

                                double pageWidth = gfx.PageSize.Width;
                                double marginPoint = 2 * 28.3465;
                                double xPosition = 400;
                                double textWidth = gfx.MeasureString(metin, font).Width;
                                if (xPosition + textWidth > pageWidth - marginPoint)
                                {
                                    xPosition = pageWidth - textWidth - marginPoint;
                                }

                                gfx.DrawString(metin, font, PdfSharp.Drawing.XBrushes.Black, new PdfSharp.Drawing.XPoint(xPosition, 40));
                            }

                            input.Save(memoryStream);
                            memoryStream.Position = 0;
                        }

                        File.WriteAllBytes(tempFilePath, memoryStream.ToArray());
                    }
                }

                using (var document = PdfiumViewer.PdfDocument.Load(tempFilePath))
                using (var printDoc = document.CreatePrintDocument())
                {
                    using (var printDialog = new PrintDialog())
                    {
                        printDialog.Document = printDoc;
                        printDialog.AllowSomePages = true;
                        printDialog.UseEXDialog = true;

                        if (printDialog.ShowDialog() == DialogResult.OK)
                        {
                            printDoc.PrinterSettings = printDialog.PrinterSettings;
                            printDoc.Print();
                            MessageBox.Show("PDF başarıyla yazdırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnYazdir.Enabled = false;
                        }
                    }
                }

                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "YazdirmaHata.txt");
                File.AppendAllText(logPath, $"[{DateTime.Now}] Hata: {ex.Message}\n{ex.StackTrace}\n");
                MessageBox.Show($"Hata oluştu: {ex.Message}\nDetaylar: {logPath}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<(int PageNumber, double X, double Y)> FindCncLocations(string pdfPath)
        {
            var locations = new List<(int PageNumber, double X, double Y)>();

            using (var document = UglyToad.PdfPig.PdfDocument.Open(pdfPath))
            {
                for (int pageNum = 1; pageNum <= document.NumberOfPages; pageNum++)
                {
                    var page = document.GetPage(pageNum);
                    var words = page.GetWords();

                    foreach (var word in words)
                    {
                        if (word.Text.Contains("CNC"))
                        {
                            var location = word.BoundingBox.BottomLeft;
                            locations.Add((pageNum, location.X, location.Y));
                        }
                    }
                }
            }
            return locations;
        }
    }
}
