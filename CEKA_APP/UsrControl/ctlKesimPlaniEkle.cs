using CEKA_APP.Abstracts;
using CEKA_APP.Business;
using CEKA_APP.Concretes;
using CEKA_APP.DataBase;
using CEKA_APP.Entitys;
using CEKA_APP.Helper;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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

        public ctlKesimPlaniEkle()
        {
            InitializeComponent();
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
                            pdfText = await PdfOkuBaykal(filePath);

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
                                    _formArayuzu.RichTextBox2MetinEkle($"{data.Kalite} - {data.Malzeme} - {data.Kalip} - {data.Poz} - {data.Adet} - {data.Proje}\n");
                                    dataGridView1.Rows.Add(data.Kalite, data.Malzeme, data.Kalip, data.Poz, data.Adet, data.Proje);
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
                using (iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(pdfpath)))
                {
                    int pageNumbers = pdfDocument.GetNumberOfPages();
                    pageText.AppendLine($"Toplam sayfa sayısı: {pageNumbers}");

                    for (int i = 1; i <= pageNumbers; i++)
                    {
                        try
                        {

                            pageText.AppendLine($"\n-------- Sayfa {i} --------");

                            ITextExtractionStrategy strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();
                            string text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);

                            pageText.AppendLine(string.IsNullOrWhiteSpace(text) ? "[Bu sayfada metin yok]" : text);
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

        public async Task<string> PdfOkuAjan(string pdfpath)
        {
            var pageText = new StringBuilder();

            try
            {
                using (var reader = new PdfReader(pdfpath))
                using (var pdfDoc = new PdfDocument(reader))
                {
                    pageText.AppendLine($"===== {Path.GetFileName(pdfpath)} =====");
                    int pageCount = pdfDoc.GetNumberOfPages();
                    pageText.AppendLine($"Toplam sayfa sayısı: {pageCount}");

                    int? firmaAdiPageNumber = null;

                    for (int i = 1; i <= pageCount; i++)
                    {
                        try
                        {
                            var strategy = new LocationTextExtractionStrategy();
                            var text = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);

                            if (string.IsNullOrWhiteSpace(text))
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("[Bu sayfada metin yok]");
                                continue;
                            }

                            var lines = text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
                            if (lines.Count == 0)
                            {
                                pageText.AppendLine($"\n-------- Sayfa {i} --------");
                                pageText.AppendLine("[Bu sayfada geçerli satır yok]");
                                continue;
                            }

                            bool currentPageIsFirmaAdi = lines.Take(5).Any(l => l.Trim().StartsWith("FirmaAdı", StringComparison.OrdinalIgnoreCase));
                            bool currentPageIsToplamParca = lines.Take(5).Any(l => l.Trim().StartsWith("ToplamParçaKesmeListesi", StringComparison.OrdinalIgnoreCase));

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

                            for (int j = 0; j < lines.Count; j++)
                            {
                                var currentLine = Regex.Replace(lines[j].Trim(), @"\s{2,}", " ");

                                if (bufferLine == null)
                                {
                                    bufferLine = currentLine;
                                    continue;
                                }

                                if (Regex.IsMatch(currentLine, @"^\d") && Regex.IsMatch(bufferLine, @"[A-Z0-9\-]{5,}$"))
                                {
                                    var combined = bufferLine + " " + currentLine;
                                    var columns = combined.Split(' ');
                                    pageText.AppendLine(string.Join("\t", columns));
                                    bufferLine = null;
                                }
                                else
                                {
                                    var prevColumns = bufferLine.Split(' ');
                                    pageText.AppendLine(string.Join("\t", prevColumns));
                                    bufferLine = currentLine;
                                }
                            }

                            if (!string.IsNullOrEmpty(bufferLine))
                            {
                                var lastColumns = bufferLine.Split(' ');
                                pageText.AppendLine(string.Join("\t", lastColumns));
                            }

                            await Task.Delay(30);
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
            if (!currentId.HasValue)
            {
                MessageBox.Show("Lütfen önce bir kesim oluşturun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            if (string.IsNullOrEmpty(Id))
            {
                MessageBox.Show("Lütfen geçerli bir ID giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (KesimListesiPaketData.KesimIdVarMi(Id))
            {
                MessageBox.Show($"Girilen ID zaten sistemde mevcut: {Id}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Text = "";
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();
                return;
            }

            string ifsKalite = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKalite(kalite);
            if (string.IsNullOrEmpty(ifsKalite))
            {
                hataMesajlari.Add($"Kalite '{kalite}' için eşleşme bulunamadı, hata mesajlarında orijinal değer kullanılacak.");
                ifsKalite = kalite;
            }
            string ifsMalzeme = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
            if (string.IsNullOrEmpty(ifsMalzeme))
            {
                hataMesajlari.Add(hataMesaji);
                MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Aynı kalıp, poz ve proje için adetleri toplamak için bir sözlük oluştur
            Dictionary<(string, string, string), int> kalipPozProjeAdet = new Dictionary<(string, string, string), int>();

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView2.Rows[i];
                if (row.IsNewRow || row.Cells[0].Value == null) continue;

                string orijinalKod = row.Cells[0].Value?.ToString()?.Trim() ?? "";
                string dKesimId = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                string adetStr = row.Cells[2].Value?.ToString()?.Trim() ?? ""; // 3. kolondan adet al
                string paketAdetStr = "";

                foreach (DataGridViewRow d3Row in dataGridView3.Rows)
                {
                    if (d3Row.IsNewRow || d3Row.Cells[0].Value == null) continue;

                    if (d3Row.Cells[0].Value.ToString() == dKesimId)
                    {
                        paketAdetStr = d3Row.Cells[2].Value?.ToString()?.Trim() ?? "";
                        break;
                    }
                }

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
                string kalipOrj = $"{kalip}-{poz}";
                string kalipPoz = $"{kalip}-{poz}";
                string kalipPozForValidation = kalipPoz;

                if (!AutoCadAktarimData.GetirStandartGruplar(kalip))
                {
                    string kalip00 = $"{kalip.Substring(0, 3)}-00";
                    kalipPoz = $"{kalip00}-{poz}";
                    kalipPozForValidation = kalipPoz;
                }

                int tireSayisi = kalipPoz.Count(c => c == '-');
                if (tireSayisi >= 3)
                {
                    int ucuncuTireIndex = kalipPoz.IndexOf('-', kalipPoz.IndexOf('-', kalipPoz.IndexOf('-') + 1) + 1);
                    kalipPozForValidation = kalipPoz.Substring(0, ucuncuTireIndex);
                }

                if (!int.TryParse(adetStr, out int adet))
                {
                    hataMesajlari.Add($"Satır {i + 1}: Geçersiz adet formatı: {adetStr}");
                    continue;
                }

                var key = (kalip, poz, proje);
                if (kalipPozProjeAdet.ContainsKey(key))
                {
                    kalipPozProjeAdet[key] += adet;
                }
                else
                {
                    kalipPozProjeAdet[key] = adet;
                }

                if (!int.TryParse(paketAdetStr, out int paketAdet))
                {
                    hataMesajlari.Add($"Satır {i + 1}: Geçersiz paket adedi: {paketAdetStr}");
                    continue;
                }

                geciciKayitlar.Add(Tuple.Create(dKesimId, proje, kalip, poz, adetStr, adet, paketAdet));
                islenmisPaketIdSet.Add(dKesimId);
            }

            foreach (var entry in kalipPozProjeAdet)
            {
                var (kalip, poz, proje) = entry.Key;
                int toplamAdet = entry.Value;
                string kalipPozForValidation = $"{kalip}-{poz}";

                if (!AutoCadAktarimData.GetirStandartGruplar(kalip))
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

                var (isValid, toplamAdetIfs, toplamAdetYuklenen) = AutoCadAktarimData.KontrolAdeta(ifsKalite, ifsMalzeme, kalipPozForValidation, proje, toplamAdet);

                if (!isValid)
                {
                    hataMesajlari.Add(
                        $"Stok Yetersizliği: [{kalite} - {malzeme} - {kalip} - {poz} - {proje}]\n" +
                        $"Planlanan: {toplamAdet}, Yüklenen: {toplamAdetYuklenen}, Toplam Talep: {toplamAdet + toplamAdetYuklenen}, Stok: {toplamAdetIfs}\n" +
                        $"❗  Toplam ihtiyaç, mevcut stok miktarını aşmaktadır.\n"
                    );

                }
            }

            if (hataMesajlari.Count > 0)
            {
                hataMesaji = "Aşağıdaki satırlarda hata bulundu:\n\n" + string.Join("\n", hataMesajlari);
                MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool kayitYapildi = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow || row.Cells[0].Value == null) continue;

                string malzemeDg = row.Cells[1].Value?.ToString()?.Trim() ?? "";
                string kalipDg = row.Cells[2].Value?.ToString()?.Trim() ?? "";
                string pozDg = row.Cells[3].Value?.ToString()?.Trim() ?? "";
                string numericPartDg = pozDg.Replace("P", "").Replace("p", "");
                if (int.TryParse(numericPartDg, out int numberDg))
                {
                    pozDg = numberDg.ToString("D2");
                }

                string malzemeKodDg = $"{kalipDg}-{pozDg}";
                string projeDg = row.Cells[5].Value?.ToString()?.Trim() ?? "";
                string adetStrDg = row.Cells[4].Value?.ToString()?.Trim() ?? "";
                int adetDg = 0;
                if (!string.IsNullOrEmpty(adetStrDg))
                {
                    adetStrDg = adetStrDg.ToUpper().Replace("AD", "").Trim();
                    int.TryParse(adetStrDg, out adetDg);
                }

                int adetKayit = adetDg;

                HashSet<string> ekSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                Regex regex = new Regex(@"-EK(\d+)", RegexOptions.IgnoreCase);

                foreach (DataGridViewRow row2 in dataGridView2.Rows)
                {
                    if (row2.IsNewRow || row2.Cells[0].Value == null) continue;

                    string orijinalKod = row2.Cells[0].Value?.ToString() ?? "";
                    if (string.IsNullOrEmpty(orijinalKod)) continue;

                    string kontrolKod = $"{kalipDg}-P{pozDg}";
                    if (orijinalKod.StartsWith(kontrolKod, StringComparison.OrdinalIgnoreCase))
                    {
                        var match = regex.Match(orijinalKod);
                        if (match.Success)
                        {
                            string ekKodu = match.Value.ToUpper();
                            ekSet.Add(ekKodu);
                        }
                    }
                }

                if (ekSet.Count > 0)
                {
                    adetKayit = adetDg * ekSet.Count;
                }

                bool ekVar = ekSet.Count > 0;

                KesimDetaylariData.SaveKesimDetaylariData(ifsKalite, ifsMalzeme, malzemeKodDg, projeDg, adetKayit, adetKayit, ekVar);
            }

            foreach (var kayit in geciciKayitlar)
            {
                string dKesimId = kayit.Item1;
                string proje = kayit.Item2;
                string kalip = kayit.Item3;
                string poz = kayit.Item4;
                string adetStr = kayit.Item5;
                int adet = kayit.Item6;
                int paketAdet = kayit.Item7;

                if (!islenmisPaketIdSet.Contains(dKesimId)) continue;

                KesimListesiPaketData.SaveKesimDataPaket(olusturan, dKesimId, paketAdet, paketAdet, eklemeTarihi);
                IdUreticiData.SiradakiIdKaydet(currentId.Value);
                KesimListesiData.SaveKesimData(olusturan, dKesimId, proje, malzeme, kalite,
                    new string[] { kalip }, new string[] { poz }, new string[] { adetStr }, eklemeTarihi);

                kayitYapildi = true;
            }

            if (hataMesajlari.Count > 0)
            {
                hataMesaji = "Aşağıdaki uyarilar bulundu:\n\n" + string.Join("\n", hataMesajlari);
                MessageBox.Show(hataMesaji, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (islenmisPaketIdSet.Count > 0 && !kayitYapildi)
            {
                MessageBox.Show("Paket başarıyla oluşturuldu, ancak içerik eklenmedi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (kayitYapildi)
            {
                var userController = new LogEkle(_formArayuzu.lblSistemKullaniciMetinAl());
                userController.LogYap("KesimPlaniEklendi", "Kesim Planı Ekle", $"Kullanıcı {currentId.Value} numaralı kesim planını yükledi.");
                ExportToXmlWithDialog(dataGridView1);
                MessageBox.Show("Kayıt işlemi başarıyla tamamlandı ve XML dosyası oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
            }
            else
            {
                MessageBox.Show("Hiçbir geçerli satır bulunamadı, kayıt yapılmadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //private void btnKaydet_Click(object sender, EventArgs e)
        //{
        //    if (!currentId.HasValue)
        //    {
        //        MessageBox.Show("Lütfen önce bir kesim oluşturun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    string olusturan = _formArayuzu.lblSistemKullaniciMetinAl();
        //    string malzeme = txtMalzeme.Text.Trim();
        //    string kalite = txtKalite.Text.Trim();
        //    DateTime eklemeTarihi = DateTime.Now;
        //    string Id = txtId.Text.Trim();
        //    string hataMesaji;

        //    HashSet<string> islenmisPaketIdSet = new HashSet<string>();
        //    List<string> hataMesajlari = new List<string>();
        //    List<Tuple<string, string, string, string, string, int, int>> geciciKayitlar = new List<Tuple<string, string, string, string, string, int, int>>();

        //    if (string.IsNullOrEmpty(Id))
        //    {
        //        MessageBox.Show("Lütfen geçerli bir ID giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    if (KesimListesiPaketData.KesimIdVarMi(Id))
        //    {
        //        MessageBox.Show($"Girilen ID zaten sistemde mevcut: {Id}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        txtId.Text = "";
        //        dataGridView1.Rows.Clear();
        //        dataGridView2.Rows.Clear();
        //        dataGridView3.Rows.Clear();
        //        return;
        //    }

        //    string ifsKalite = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKalite(kalite);
        //    if (string.IsNullOrEmpty(ifsKalite))
        //    {
        //        hataMesajlari.Add($"Kalite '{kalite}' için eşleşme bulunamadı, hata mesajlarında orijinal değer kullanılacak.");
        //        ifsKalite = kalite;
        //    }
        //    string ifsMalzeme = KarsilastirmaTablosuData.GetIfsCodeByAutoCadCodeKesim(malzeme, out hataMesaji);
        //    if (string.IsNullOrEmpty(ifsMalzeme))
        //    {
        //        hataMesajlari.Add(hataMesaji);
        //        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    for (int i = 0; i < dataGridView2.Rows.Count; i++)
        //    {
        //        DataGridViewRow row = dataGridView2.Rows[i];
        //        if (row.IsNewRow || row.Cells[0].Value == null) continue;

        //        string orijinalKod = row.Cells[0].Value?.ToString()?.Trim() ?? "";
        //        string dKesimId = row.Cells[1].Value?.ToString()?.Trim() ?? "";
        //        string adetStr = row.Cells[2].Value?.ToString()?.Trim() ?? "";
        //        string paketAdetStr = "";

        //        foreach (DataGridViewRow d3Row in dataGridView3.Rows)
        //        {
        //            if (d3Row.IsNewRow || d3Row.Cells[0].Value == null) continue;

        //            if (d3Row.Cells[0].Value.ToString() == dKesimId)
        //            {
        //                paketAdetStr = d3Row.Cells[2].Value?.ToString()?.Trim() ?? "";
        //                break;
        //            }
        //        }

        //        string[] parcalar = orijinalKod.Split('-');
        //        int adet = 0;
        //        string proje = "", kalip = "", poz = "", ekBilgi = "";

        //        if (parcalar.Length >= 5)
        //        {
        //            kalip = $"{parcalar[0]}-{parcalar[1]}";
        //            poz = parcalar[2];
        //            string adetToplam = parcalar[3];
        //            proje = parcalar[4].Trim();

        //            if (parcalar.Length >= 6)
        //            {
        //                ekBilgi = parcalar[5];
        //            }

        //            if (!string.IsNullOrEmpty(adetToplam))
        //            {
        //                adetToplam = adetToplam.ToUpper().Replace("AD", "");
        //                int.TryParse(adetToplam, out adet);
        //            }
        //        }
        //        else
        //        {
        //            hataMesajlari.Add($"Satır {i + 1}: Geçersiz veri formatı: {orijinalKod}");
        //            continue;
        //        }

        //        string numericPart = poz.Replace("P", "").Replace("p", "");
        //        if (int.TryParse(numericPart, out int number))
        //        {
        //            poz = number.ToString("D2");
        //            if (!string.IsNullOrEmpty(ekBilgi))
        //            {
        //                poz = $"{poz}-{ekBilgi}";
        //            }
        //        }
        //        string kalipOrj = $"{kalip}-{poz}";
        //        string kalipPoz = $"{kalip}-{poz}";
        //        string kalipPozForValidation = kalipPoz;

        //        if (!AutoCadAktarimData.GetirStandartGruplar(kalip))
        //        {
        //            string kalip00 = $"{kalip.Substring(0, 3)}-00";
        //            kalipPoz = $"{kalip00}-{poz}";
        //            kalipPozForValidation = kalipPoz;
        //        }

        //        int tireSayisi = kalipPoz.Count(c => c == '-');
        //        if (tireSayisi >= 3)
        //        {
        //            int ucuncuTireIndex = kalipPoz.IndexOf('-', kalipPoz.IndexOf('-', kalipPoz.IndexOf('-') + 1) + 1);
        //            kalipPozForValidation = kalipPoz.Substring(0, ucuncuTireIndex);
        //        }

        //        var (isValid, toplamAdetIfs, toplamAdetYuklenen) = AutoCadAktarimData.KontrolAdeta(ifsKalite, ifsMalzeme, kalipPozForValidation, proje, adet);

        //        if (!isValid)
        //        {
        //            hataMesajlari.Add($"Satır {i + 1} - Stok Aşımı: [{kalite}-{malzeme}-{kalipOrj}-{proje}]\n" +
        //                              $"Planlanan: {adet}, Yüklenmiş: {toplamAdetYuklenen}, Toplam: {adet + toplamAdetYuklenen}, Stok: {toplamAdetIfs}\n" +
        //                              $"❗ Toplam ihtiyaç, mevcut stok miktarını aşmaktadır.\n");
        //            continue;
        //        }

        //        if (!int.TryParse(paketAdetStr, out int paketAdet))
        //        {
        //            hataMesajlari.Add($"Satır {i + 1}: Geçersiz paket adedi: {paketAdetStr}");
        //            continue;
        //        }

        //        geciciKayitlar.Add(Tuple.Create(dKesimId, proje, kalip, poz, adetStr, adet, paketAdet));
        //        islenmisPaketIdSet.Add(dKesimId);
        //    }

        //    if (hataMesajlari.Count > 0)
        //    {
        //        hataMesaji = "Aşağıdaki satırlarda hata bulundu:\n\n" + string.Join("\n", hataMesajlari);
        //        MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    bool kayitYapildi = false;

        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        if (row.IsNewRow || row.Cells[0].Value == null) continue;

        //        string malzemeDg = row.Cells[1].Value?.ToString()?.Trim() ?? "";
        //        string kalipDg = row.Cells[2].Value?.ToString()?.Trim() ?? "";
        //        string pozDg = row.Cells[3].Value?.ToString()?.Trim() ?? "";
        //        string numericPartDg = pozDg.Replace("P", "").Replace("p", "");
        //        if (int.TryParse(numericPartDg, out int numberDg))
        //        {
        //            pozDg = numberDg.ToString("D2");
        //        }


        //        string malzemeKodDg = $"{kalipDg}-{pozDg}";
        //        string projeDg = row.Cells[5].Value?.ToString()?.Trim() ?? "";
        //        string adetStrDg = row.Cells[4].Value?.ToString()?.Trim() ?? "";
        //        int adetDg = 0;
        //        if (!string.IsNullOrEmpty(adetStrDg))
        //        {
        //            adetStrDg = adetStrDg.ToUpper().Replace("AD", "").Trim();
        //            int.TryParse(adetStrDg, out adetDg);
        //        }

        //        int adetKayit = adetDg;

        //        HashSet<string> ekSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //        Regex regex = new Regex(@"-EK(\d+)", RegexOptions.IgnoreCase);

        //        foreach (DataGridViewRow row2 in dataGridView2.Rows)
        //        {
        //            if (row2.IsNewRow || row2.Cells[0].Value == null) continue;

        //            string orijinalKod = row2.Cells[0].Value?.ToString() ?? "";
        //            if (string.IsNullOrEmpty(orijinalKod)) continue;

        //            string kontrolKod = $"{kalipDg}-P{pozDg}";
        //            if (orijinalKod.StartsWith(kontrolKod, StringComparison.OrdinalIgnoreCase))
        //            {
        //                var match = regex.Match(orijinalKod);
        //                if (match.Success)
        //                {
        //                    string ekKodu = match.Value.ToUpper();
        //                    ekSet.Add(ekKodu);
        //                }
        //            }
        //        }

        //        if (ekSet.Count > 0)
        //        {
        //            adetKayit = adetDg * ekSet.Count;
        //        }

        //        bool ekVar = ekSet.Count > 0;

        //        KesimDetaylariData.SaveKesimDetaylariData(ifsKalite, ifsMalzeme, malzemeKodDg, projeDg, adetKayit, adetKayit, ekVar);

        //    }

        //    foreach (var kayit in geciciKayitlar)
        //    {
        //        string dKesimId = kayit.Item1;
        //        string proje = kayit.Item2;
        //        string kalip = kayit.Item3;
        //        string poz = kayit.Item4;
        //        string adetStr = kayit.Item5;
        //        int adet = kayit.Item6;
        //        int paketAdet = kayit.Item7;

        //        if (!islenmisPaketIdSet.Contains(dKesimId)) continue;

        //        KesimListesiPaketData.SaveKesimDataPaket(olusturan, dKesimId, paketAdet, paketAdet, eklemeTarihi);
        //        IdUreticiData.SiradakiIdKaydet(currentId.Value);
        //        KesimListesiData.SaveKesimData(olusturan, dKesimId, proje, malzeme, kalite,
        //            new string[] { kalip }, new string[] { poz }, new string[] { adetStr }, eklemeTarihi);

        //        kayitYapildi = true;
        //    }

        //    if (hataMesajlari.Count > 0)
        //    {
        //        hataMesaji = "Aşağıdaki uyarilar bulundu:\n\n" + string.Join("\n", hataMesajlari);
        //        MessageBox.Show(hataMesaji, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //    if (islenmisPaketIdSet.Count > 0 && !kayitYapildi)
        //    {
        //        MessageBox.Show("Paket başarıyla oluşturuldu, ancak içerik eklenmedi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    else if (kayitYapildi)
        //    {
        //        var userController = new LogEkle(_formArayuzu.lblSistemKullaniciMetinAl());
        //        userController.LogYap("KesimPlaniEklendi", "Kesim Planı Ekle", $"Kullanıcı {currentId.Value} numaralı kesim planını yükledi.");
        //        ExportToXmlWithDialog(dataGridView1);
        //        MessageBox.Show("Kayıt işlemi başarıyla tamamlandı ve XML dosyası oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        Temizle();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Hiçbir geçerli satır bulunamadı, kayıt yapılmadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        private void Temizle()
        {
            txtId.Clear();
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
            currentId = null;
            _xmlDosyaYolu = null;

            _seciliButon = null;
            ButonMakinaSecHelper.NötrStilUygula(buttonGroup);

            if (_seciliButon != null)
            {
                MessageBox.Show("Hata: _seciliButon null olmadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            int lastFirmaPageNumber = 1;
            int firmaSayac = 0;
            for (int i = 0; i < satirlar.Length; i++)
            {
                string satir = satirlar[i].Trim();

                if (satir.StartsWith("-------- Sayfa"))
                {
                    string[] parts = satir.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3 && int.TryParse(parts[2].Replace("--------", "").Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }

                if (satir.StartsWith("Sayfa:"))
                {
                    if (int.TryParse(satir.Replace("Sayfa:", "").Trim(), out int pageNum))
                    {
                        currentPageNumber = pageNum;
                    }
                    continue;
                }

                if (satir.StartsWith("--------") ||
                    satir.StartsWith("Parça") ||
                    satir.StartsWith("Not:") ||
                    satir.StartsWith("EkstraKesim") ||
                    satir.StartsWith("Toplam"))
                    continue;

                if (satir.StartsWith("ToplamParçaKesmeListesi"))
                {
                    isFirmaAdiSection = false;
                    continue;
                }
                if (satir.StartsWith("FirmaAdı"))
                {
                    isFirmaAdiSection = true;
                    firmaSayac++;
                    lastFirmaPageNumber = firmaSayac;
                    continue;
                }
                if (satir.StartsWith("FirmaAdı"))
                {
                    isFirmaAdiSection = true;
                    lastFirmaPageNumber = currentPageNumber;
                    continue;
                }

                if (!isFirmaAdiSection)
                    continue;

                if (!satir.StartsWith("ST"))
                    continue;

                string[] sutunlar = Regex.Split(satir, @"\t+|\s{2,}")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                string parcaAdi = sutunlar[0].Trim();
                if (!Regex.IsMatch(parcaAdi, @"^ST\d*-.*-.*-.*-.*-$"))
                {
                    continue;
                }

                string nextLine = "";
                if (i + 1 < satirlar.Length)
                {
                    nextLine = satirlar[i + 1].Trim();
                }

                if (string.IsNullOrWhiteSpace(nextLine) || Regex.IsMatch(nextLine, @"^\s+$"))
                    continue;

                string[] sonrakiSutunlar = Regex.Split(nextLine, @"\t+|\s{2,}")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                string eklenecekVeri = sonrakiSutunlar.Length > 0 ? sonrakiSutunlar[0] : "";

                string birlesikVeri = $"{parcaAdi}{eklenecekVeri} (Yerleşim: {firmaSayac})(Sayfa: {currentPageNumber})";

                sonucBuilder.AppendLine(birlesikVeri);
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
            string pattern = @"^\s*(?:(?:\d+\s*-\s*\d+\s*)|\d+\s*)?(ST\d*-.*-.*-.*-.*-\S*)";

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
            Dictionary<string, int> pozToplamAdet = new Dictionary<string, int>();
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
                            int adet = int.Parse(miktar) * int.Parse(currentProgramTekrari);

                            if (!pozToplamAdet.ContainsKey(pozKey))
                            {
                                pozToplamAdet[pozKey] = 0;
                            }
                            pozToplamAdet[pozKey] += adet;

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
                string sonuc1 = $"{currentMalzeme}{(string.IsNullOrEmpty(kalip) ? "" : "-")}{kalip}{(string.IsNullOrEmpty(kalip) ? "" : "-")}{pozKey}-{pozToplamAdet[pozKey]}AD-{proje}";
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

                currentId = IdUreticiData.GetSiradakiId();

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

        public void ExportToXmlWithDialog(DataGridView dgv)
        {
            string targetFolder = Properties.Settings.Default.KlasorYolu;

            if (string.IsNullOrWhiteSpace(targetFolder) || !Directory.Exists(targetFolder))
            {
                MessageBox.Show("Klasör seçimi yapılmamış veya geçersiz bir klasör yolu. Lütfen önce bir klasör seçin.",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string yerlesimPlaniId = txtId.Text.Trim();
            string tarihSaat = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"{yerlesimPlaniId}_YerlesimPlani_{tarihSaat}.xml";
            string dosyaYolu = Path.Combine(targetFolder, fileName);

            ExportToXml(dgv, dosyaYolu);
        }
        public void ExportToXml(DataGridView dgv, string dosyaYolu)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtSite.Text))
            {
                MessageBox.Show("ID veya Site alanı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgv.Rows.Count == 0 || dgv.Rows[0].IsNewRow)
            {
                MessageBox.Show("DataGridView'de veri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XmlWriterSettings ayarlar = new XmlWriterSettings { Indent = true };

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

                    string stokKodu = (dgv.Rows[0].Cells[1].Value?.ToString() ?? "").Trim();
                    string stokKoduXml = stokKodu;
                    if (!string.IsNullOrEmpty(stokKodu) && Regex.IsMatch(stokKodu, @"MM$", RegexOptions.IgnoreCase))
                    {
                        string sayiKismi = Regex.Replace(stokKodu, @"MM$", "", RegexOptions.IgnoreCase).Trim();
                        if (int.TryParse(sayiKismi, out int sayi))
                        {
                            stokKoduXml = $"KPL{sayi:D3}";
                        }
                    }
                    writer.WriteElementString("StokKodu", stokKoduXml);

                    string kalite = (dgv.Rows[0].Cells[0].Value?.ToString() ?? "").Trim();
                    string kaliteXml = kalite;
                    var kaliteDonusumleri = new Dictionary<string, string>
            {
                { "ST37", "S235JR" },
                { "ST44", "S275JR" },
                { "ST52", "S355J2" }
            };
                    if (kaliteDonusumleri.TryGetValue(kalite, out var donusum))
                    {
                        kaliteXml = donusum;
                    }
                    writer.WriteElementString("Kalite", kaliteXml);

                    writer.WriteElementString("EklemeTarihi", dtEklemeTarihi.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture).Trim());

                    var parcaBySayfaId = dataGridView2.Rows.Cast<DataGridViewRow>()
                        .Where(row => !row.IsNewRow && row.Cells[1].Value != null)
                        .GroupBy(row => (SayfaId: row.Cells[1].Value.ToString().Trim(), Poz: ExtractPoz(row.Cells[0].Value?.ToString()?.Trim())))
                        .Select(g => new
                        {
                            g.Key.SayfaId,
                            g.Key.Poz,
                            ParcaRows = g.ToList(),
                            ToplamAdet = g.Sum(row => double.TryParse(row.Cells[2].Value?.ToString()?.Trim(), out double adet) ? adet : 0),
                            ToplamEkOran = g.Sum(row => double.TryParse(row.Cells[4].Value?.ToString()?.Trim(), out double oran) ? oran : 0),
                            EkAdi = g.FirstOrDefault(row => row.Cells[0].Value?.ToString().Contains("EK") == true)?.Cells[0].Value?.ToString().Split('-').LastOrDefault(p => p.Contains("EK")) ?? "-"
                        })
                        .GroupBy(x => x.SayfaId)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (DataGridViewRow sayfaRow in dataGridView3.Rows)
                    {
                        if (sayfaRow.IsNewRow || sayfaRow.Cells[0].Value == null) continue;

                        string sayfaId = sayfaRow.Cells[0].Value.ToString().Trim();
                        writer.WriteStartElement("Sayfa");
                        writer.WriteElementString("ID", sayfaId);

                        string tekrarAdeti = (sayfaRow.Cells[2].Value?.ToString() ?? "1").Trim();
                        writer.WriteElementString("TekrarAdeti", tekrarAdeti);

                        if (parcaBySayfaId.TryGetValue(sayfaId, out var parcaGroups))
                        {
                            foreach (var parcaGroup in parcaGroups)
                            {
                                writer.WriteStartElement("Parca");

                                string kalipVerisi = parcaGroup.ParcaRows.First().Cells[0].Value?.ToString() ?? "";
                                string[] parcalar = kalipVerisi.Split('-');
                                string kalip = kalipVerisi, poz = parcaGroup.Poz, proje = "";

                                if (parcalar.Length >= 5)
                                {
                                    string sifirParca = parcalar[0].Trim();
                                    string birParca = parcalar[1].Trim();

                                    kalip = $"{sifirParca}-{birParca}";
                                    if (!AutoCadAktarimData.GetirStandartGruplar(kalip))
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

                                string ekAdi = parcaGroup.EkAdi;
                                string ekOran = parcaGroup.ToplamEkOran > 0 ? parcaGroup.ToplamEkOran.ToString("F6", CultureInfo.InvariantCulture) : "-";
                                string adetToWrite = parcaGroup.ToplamEkOran > 0 ? "0" : parcaGroup.ToplamAdet.ToString(CultureInfo.InvariantCulture);

                                writer.WriteElementString("Adet", adetToWrite);
                                writer.WriteElementString("EkAdi", ekAdi);
                                writer.WriteElementString("EkOran", ekOran);

                                writer.WriteEndElement();
                            }
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                var userController = new LogEkle(_formArayuzu.lblSistemKullaniciMetinAl());
                userController.LogYap("XmlDosyasiOlusturuldu", "Kesim Planı Ekle", $"Kullanıcı {txtId.Text.Trim()} numaralı kesim planı XML dosyası oluşturdu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML oluşturma sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ExtractPoz(string kalipVerisi)
        {
            if (string.IsNullOrEmpty(kalipVerisi)) return "";
            var parcalar = kalipVerisi.Split('-');
            if (parcalar.Length >= 3)
            {
                string poz = parcalar[2].Trim();
                return poz.StartsWith("P") ? poz : $"P{poz}";
            }
            return "";
        }

        //public void ExportToXml(DataGridView dgv, string dosyaYolu)
        //{
        //    if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtSite.Text))
        //    {
        //        MessageBox.Show("ID veya Site alanı boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    if (dgv.Rows.Count == 0 || dgv.Rows[0].IsNewRow)
        //    {
        //        MessageBox.Show("DataGridView'de veri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    XmlWriterSettings ayarlar = new XmlWriterSettings { Indent = true };

        //    try
        //    {
        //        string directory = Path.GetDirectoryName(dosyaYolu);
        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }

        //        using (XmlWriter writer = XmlWriter.Create(dosyaYolu, ayarlar))
        //        {
        //            writer.WriteStartDocument();
        //            writer.WriteStartElement("YerlesimPlaniBilgileri");

        //            writer.WriteElementString("ID", txtId.Text.Trim());
        //            writer.WriteElementString("Site", txtSite.Text.Trim());

        //            string stokKodu = (dgv.Rows[0].Cells[1].Value?.ToString() ?? "").Trim();
        //            string stokKoduXml = stokKodu;
        //            if (!string.IsNullOrEmpty(stokKodu) && Regex.IsMatch(stokKodu, @"MM$", RegexOptions.IgnoreCase))
        //            {
        //                string sayiKismi = Regex.Replace(stokKodu, @"MM$", "", RegexOptions.IgnoreCase).Trim();
        //                if (int.TryParse(sayiKismi, out int sayi))
        //                {
        //                    stokKoduXml = $"KPL{sayi:D3}";
        //                }
        //            }
        //            writer.WriteElementString("StokKodu", stokKoduXml);

        //            string kalite = (dgv.Rows[0].Cells[0].Value?.ToString() ?? "").Trim();
        //            string kaliteXml = kalite;
        //            var kaliteDonusumleri = new Dictionary<string, string>
        //    {
        //        { "ST37", "S235JR" },
        //        { "ST44", "S275JR" },
        //        { "ST52", "S355J2" }
        //    };
        //            if (kaliteDonusumleri.TryGetValue(kalite, out var donusum))
        //            {
        //                kaliteXml = donusum;
        //            }
        //            writer.WriteElementString("Kalite", kaliteXml);

        //            writer.WriteElementString("EklemeTarihi", dtEklemeTarihi.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture).Trim());

        //            var parcaBySayfaId = dataGridView2.Rows.Cast<DataGridViewRow>()
        //                .Where(row => !row.IsNewRow && row.Cells[1].Value != null)
        //                .GroupBy(row => row.Cells[1].Value.ToString().Trim())
        //                .ToDictionary(g => g.Key, g => g.ToList());

        //            foreach (DataGridViewRow sayfaRow in dataGridView3.Rows)
        //            {
        //                if (sayfaRow.IsNewRow || sayfaRow.Cells[0].Value == null) continue;

        //                string sayfaId = sayfaRow.Cells[0].Value.ToString().Trim();
        //                writer.WriteStartElement("Sayfa");
        //                writer.WriteElementString("ID", sayfaId);

        //                string tekrarAdeti = (sayfaRow.Cells[2].Value?.ToString() ?? "1").Trim();
        //                writer.WriteElementString("TekrarAdeti", tekrarAdeti);

        //                if (parcaBySayfaId.TryGetValue(sayfaId, out var parcaRows))
        //                {
        //                    foreach (var parcaRow in parcaRows)
        //                    {
        //                        writer.WriteStartElement("Parca");

        //                        string kalipVerisi = (parcaRow.Cells[0].Value?.ToString() ?? "").Trim();
        //                        string[] parcalar = kalipVerisi.Split('-');
        //                        string kalip = kalipVerisi, poz = "", proje = "";

        //                        if (parcalar.Length >= 5)
        //                        {
        //                            string sifirParca = parcalar[0].Trim();
        //                            string birParca = parcalar[1].Trim();

        //                            kalip = $"{sifirParca}-{birParca}";
        //                            if (!AutoCadAktarimData.GetirStandartGruplar(kalip))
        //                            {
        //                                birParca = "00";
        //                                kalip = $"{sifirParca}-{birParca}";
        //                            }

        //                            poz = parcalar[2].Trim();
        //                            proje = parcalar[4].Trim();

        //                            if (poz.StartsWith("P"))
        //                            {
        //                                poz = poz.Substring(1);
        //                                if (poz.Length == 1 && int.TryParse(poz, out int pozSayi))
        //                                {
        //                                    poz = pozSayi.ToString("D2");
        //                                }
        //                            }
        //                        }

        //                        writer.WriteElementString("Kalip", kalip);
        //                        writer.WriteElementString("Poz", poz);
        //                        writer.WriteElementString("Proje", proje);

        //                        string ekAdi = "-";
        //                        string ekOran = "-";
        //                        string adetToWrite = (parcaRow.Cells[2].Value?.ToString() ?? "0").Trim();

        //                        if (parcaRow.Cells[4].Value != null &&
        //                            double.TryParse(parcaRow.Cells[4].Value.ToString().Trim(), out double oran) &&
        //                            parcaRow.Cells[2].Value != null &&
        //                            double.TryParse(parcaRow.Cells[2].Value.ToString().Trim(), out double adet))
        //                        {
        //                            ekOran = oran.ToString(CultureInfo.InvariantCulture).Trim();
        //                            adetToWrite = "0";

        //                            List<string> parcaList = parcaRow.Cells[0].Value?.ToString()
        //                                .Split('-')
        //                                .Select(p => p.Trim())
        //                                .ToList() ?? new List<string>();

        //                            if (parcaList.Any(p => p.Contains("EK")))
        //                            {
        //                                ekAdi = parcaList.First(p => p.Contains("EK"));
        //                            }
        //                        }

        //                        writer.WriteElementString("Adet", adetToWrite);
        //                        writer.WriteElementString("EkAdi", ekAdi);
        //                        writer.WriteElementString("EkOran", ekOran);

        //                        writer.WriteEndElement();
        //                    }
        //                }

        //                writer.WriteEndElement();
        //            }

        //            writer.WriteEndElement();
        //            writer.WriteEndDocument();
        //        }

        //        var userController = new LogEkle(_formArayuzu.lblSistemKullaniciMetinAl());
        //        userController.LogYap("XmlDosyasiOlusturuldu", "Kesim Planı Ekle", $"Kullanıcı {txtId.Text.Trim()} numaralı kesim planı XML dosyası oluşturdu.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"XML oluşturma sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

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

                //                Regex pozRegex = new Regex(
                //    @"ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,4}\s*-\s*(\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2}(?:-\s*EK\d{1,2})?)",
                //    RegexOptions.IgnoreCase
                //);
                Regex pozRegex = new Regex(
                    @"ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,}\s*-\s*(\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2,}(?:-\s*EK\d{1,2})?)",
                    RegexOptions.IgnoreCase);

                Regex suffixRegex = new Regex(@"-(?!EK\d+$)[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
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

                    if (line.StartsWith("ST") && !isToplamParcaPage && validSayfaNos.Contains(currentRealSayfaNo.ToString()))
                    {
                        Match pozMatch = pozRegex.Match(line);
                        string adet = "";
                        Match adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}:\d{2}\s+(\d+)");
                        if (!adetMatch.Success)
                            adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}\s+(\d+)");
                        if (adetMatch.Success)
                            adet = adetMatch.Groups[1].Value;

                        string agirlik = "";
                        var parts = line.Split('\t');
                        if (parts.Length >= 4)
                        {
                            agirlik = parts[3].Trim();
                        }

                        string temizParca = suffixRegex.Replace(line.Trim(), "");

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

                var rich4Dict = new Dictionary<string, (string Poz, string SayfaID)>();
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
                        string sayfaID = match.Groups[4].Value.Trim();
                        rich4Dict[parcaID] = (poz, yerlesimID);
                    }
                }

                foreach (var line in logLines)
                {
                    if (!line.Contains("=> Adet:")) continue;

                    var match = Regex.Match(line, @"Adet: (\d+), Ağırlık: ([\d,]*\.?\d*), Parça ID: (.+?), Sayfa ID");
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
            try
            {
                string baseId = txtId.Text.Trim();
                if (string.IsNullOrEmpty(baseId))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string text1 = _formArayuzu.RichTextBox1MetniAl().Trim();
                if (string.IsNullOrEmpty(text1))
                {
                    MessageBox.Show("Lütfen richTextBox1 alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string text4 = _formArayuzu.RichTextBox4MetniAl().Trim();
                if (string.IsNullOrEmpty(text4))
                {
                    MessageBox.Show("Lütfen richTextBox4 alanını doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var logLines = new List<string>
        {
            "=== ProNest_Log.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            $"BaseId: {baseId}"
        };

                Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*--------", RegexOptions.IgnoreCase);
                Regex partRegex = new Regex(@"(\d+)(?:\s*-\s*(\d+))?\s+.*?(\d+-\d+-P\d+(?:-[\w\.]+)*)\s.*?(\d{1,3}(?:,\d{1,2})?)\s*kg", RegexOptions.IgnoreCase);
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
                    MessageBox.Show("Hiçbir poz bulunamadı. Lütfen veri formatını kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "CEKA_APP");
                string logPath = Path.Combine(appPath, "ProNest_Log.txt");
                File.WriteAllLines(logPath, logLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu, lütfen veri formatını kontrol edin veya destek ekibiyle iletişime geçin: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                Regex partRegex = new Regex(@"(\d{5}\.\d{2,3}-\d{3}-00-P\d+)\s+(\d+)\s+([\d,.]+)\s+([\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)\s+(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase);
                Regex cncRegex = new Regex(@"^\s*(\d+)\s+(\d+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)\s+([\d.,]+)", RegexOptions.IgnoreCase);
                Regex rich4Regex = new Regex(@"ST\d+-[A-Za-z0-9]+-(?<poz>\d+-\d+-P\d+)-\d*AD-(?<pozPrefix>[\d.]+).*\(Sayfa\s*[:]\s*(?<sayfa>\d+).*M[iı]ktar\s*[:\s]\s*(?<adet>\d+).*Program\s*[:\s]\s*(?<program>\d+)", RegexOptions.IgnoreCase);

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
                            partInfoDict[parcaId] = (poz, $"{baseId}-{programId}", adet, agirlik, uniqueId);
                            logLines.Add($"Parça Bulundu: {parcaId} => Poz: {poz}, Program: {currentCncProgram}, Adet: {adet}, Ağırlık: {agirlik:F2}, Id: {uniqueId}");
                        }
                    }
                }

                logLines.Add("\nRichTextBox4 Verileri:");
                programSequenceDict.Clear();

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string cleanedLine = Regex.Replace(line.Trim(), @"\s+", " ").Replace("\r", "").Replace("\n", "");
                    cleanedLine = Regex.Replace(cleanedLine, @"\p{C}", "");

                    var match = rich4Regex.Match(cleanedLine);
                    if (match.Success)
                    {
                        string pozSimple = match.Groups["poz"].Value;
                        string pozPrefix = match.Groups["pozPrefix"].Value;
                        string programNo = match.Groups["program"].Value;
                        string adetStr = match.Groups["adet"].Value;

                        string programId = programIdMap.ContainsKey(programNo) ? programIdMap[programNo] : "00";

                        int sequence = programSequenceDict.ContainsKey(programNo) ? programSequenceDict[programNo] + 1 : 1;
                        programSequenceDict[programNo] = sequence;
                        string sequenceStr = sequence.ToString("D2");
                        string uniqueId = $"{baseId}-{programId}-{sequenceStr}";

                        string formattedPoz = $"{pozSimple}-2AD-{pozPrefix}";
                        string parcaId = $"{pozPrefix}-{pozSimple}";

                        if (partInfoDict.TryGetValue(parcaId, out var partInfo))
                        {
                            logLines.Add($"Eşleşme Bulundu: {cleanedLine},Id: {partInfo.Id}");
                            finalOutputList.Add((partInfo.Id, formattedPoz, $"{baseId}-{programId}", partInfo.Adet, partInfo.Agirlik));
                        }
                        else
                        {
                            logLines.Add($"Eşleşme Bulunamadı: {cleanedLine}");
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

                // btnAdm için ayrı bir işlem mantığı
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

                                int rowIndexAdm = dataGridView3.Rows.Add();
                                dataGridView3.Rows[rowIndexAdm].Cells[0].Value = birlesikIDAdm;
                                dataGridView3.Rows[rowIndexAdm].Cells[1].Value = sayfaNo4;
                                dataGridView3.Rows[rowIndexAdm].Cells[2].Value = adet;
                                log.WriteLine($"[{DateTime.Now}] Satır eklendi: {birlesikIDAdm}, Sayfa: {sayfaNo4}, Miktar: {adet}, Program: {programNo}");
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

                            int rowIndex = dataGridView3.Rows.Add();
                            dataGridView3.Rows[rowIndex].Cells[0].Value = birlesikID;
                            dataGridView3.Rows[rowIndex].Cells[1].Value = sayfaNo;
                            log.WriteLine($"[{DateTime.Now}] Satır eklendi: {birlesikID}, Sayfa: {sayfaNo}");

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
                                    Match levhaMatch = Regex.Match(sayfa, @"LevhaÖlçüleri\s+\d+X\d+\w*\s+(\d+)\s+Kalınlık", RegexOptions.IgnoreCase);
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
                                dataGridView3.Rows[rowIndex].Cells[2].Value = yerlesimTekrar[yerlesimNo];
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

                var pozGroups = new Dictionary<string, List<(string Poz, string SayfaID, int Adet, double Agirlik, DataGridViewRow Row)>>();

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

                    string basePoz = ekRegex.Replace(poz, "");

                    if (!pozGroups.ContainsKey(basePoz))
                        pozGroups[basePoz] = new List<(string, string, int, double, DataGridViewRow)>();

                    pozGroups[basePoz].Add((poz, sayfaID, adet, agirlik, row));
                }

                logLines.Add("\nEK İbaresi İçeren Pozlar ve Yüzde Hesaplamaları:");

                foreach (var group in pozGroups)
                {
                    string basePoz = group.Key;
                    var items = group.Value;

                    if (items.Any(item => ekRegex.IsMatch(item.Poz)))
                    {
                        double toplamAgirlik = items.Where(item => ekRegex.IsMatch(item.Poz))
                                                    .Sum(item => item.Agirlik * item.Adet);

                        logLines.Add($"\nPoz: {basePoz}, Toplam Ağırlık: {toplamAgirlik}");

                        foreach (var item in items.Where(i => ekRegex.IsMatch(i.Poz)))
                        {
                            double oran = (item.Agirlik * item.Adet) / toplamAgirlik;
                            logLines.Add($"  - Poz: {item.Poz}, Sayfa ID: {item.SayfaID}, Adet: {item.Adet}, Ağırlık: {item.Agirlik}, Parçanın Toplam Ağırlığa Oranı: {oran:F6}");

                            item.Row.Cells["Oran"].Value = $"{oran:F6}";
                        }
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

            if (dataGridView3.Rows.Count == 0 || dataGridView3.Rows[0].IsNewRow)
            {
                MessageBox.Show("DataGridView3'te veri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"ModifiedPDF_{Guid.NewGuid()}.pdf");

                using (var memoryStream = new MemoryStream())
                {
                    using (var input = PdfSharp.Pdf.IO.PdfReader.Open(txtDosya.Text, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                    {
                        foreach (DataGridViewRow row in dataGridView3.Rows)
                        {
                            if (row.IsNewRow || row.Cells[1].Value == null || row.Cells[0].Value == null) continue;

                            string sayfaNoStr = row.Cells[1].Value.ToString().Trim();
                            string metin = row.Cells[0].Value.ToString().Trim();

                            if (!int.TryParse(sayfaNoStr, out int sayfaNo) || sayfaNo < 1 || sayfaNo > input.PageCount)
                            {
                                MessageBox.Show($"Geçersiz sayfa numarası: {sayfaNoStr}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }

                            var gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(input.Pages[sayfaNo - 1], PdfSharp.Drawing.XGraphicsPdfPageOptions.Append);
                            var font = new PdfSharp.Drawing.XFont("Arial", 20);
                            gfx.DrawString(metin, font, PdfSharp.Drawing.XBrushes.Black, new PdfSharp.Drawing.XPoint(400, 40));

                        }

                        input.Save(memoryStream);
                        memoryStream.Position = 0;
                    }

                    File.WriteAllBytes(tempFilePath, memoryStream.ToArray());
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


    }
}
