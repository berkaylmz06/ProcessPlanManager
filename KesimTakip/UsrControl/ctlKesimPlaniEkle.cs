using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf;
using KesimTakip.Business;
using KesimTakip.Entitys;
using KesimTakip.Helper;
using KesimTakip.DataBase;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Globalization;
using System.Xml;
using KesimTakip.Abstracts;
using System.Threading;

namespace KesimTakip.UsrControl
{
    public partial class ctlKesimPlaniEkle : UserControl
    {
        private Button _seciliButon;
        private List<Button> buttonGroup;
        private int? currentId = null;
        private readonly VeriOkuma _veriOkuma;
        private IFormArayuzu _formArayuzu;

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
            //if (_seciliButon == null)
            //{
            //    MessageBox.Show("Lütfen önce bir makine seçin (Ajan, Adm, Baykal).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //_formArayuzu.RichTextBox1Temizle();
            //_formArayuzu.RichTextBox2Temizle();
            //_formArayuzu.RichTextBox3Temizle();
            //_formArayuzu.RichTextBox4Temizle();
            //dataGridView1.Rows.Clear();
            //dataGridView2.Rows.Clear();
            //dataGridView3.Rows.Clear();
            //OpenFileDialog open = new OpenFileDialog();
            //open.Filter = "PDF File|*.pdf";
            //if (open.ShowDialog() == DialogResult.OK)
            //{
            //    string filePath = open.FileName;
            //    txtDosya.Text = filePath;
            //    if (File.Exists(filePath))
            //    {

            //        try
            //        {
            //            progressBar1.Value = 0;
            //            progressBar1.Visible = true;

            //            string pdfText;

            //            if (_seciliButon == btnBaykal)
            //            {
            //                pdfText = await PdfOkuBaykal(filePath);
            //            }
            //            else if (_seciliButon == btnAjan)
            //            {
            //                pdfText = await PdfOkuAjan(filePath);

            //            }
            //            else
            //            {
            //                MessageBox.Show("Geçersiz buton seçimi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                progressBar1.Visible = false;
            //                return;
            //            }

            //            if (string.IsNullOrEmpty(pdfText))
            //            {
            //                MessageBox.Show("PDF metni okunamadı. PDF dosyasının içeriğini kontrol edin.",
            //                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                progressBar1.Visible = false;
            //                return;
            //            }

            //            string pdfTuru = TespitEtPdfTuru(pdfText);
            //            if (string.IsNullOrEmpty(pdfTuru))
            //            {
            //                MessageBox.Show("PDF türü tespit edilemedi. Dosya içeriğini kontrol edin.",
            //                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                progressBar1.Visible = false;
            //                return;
            //            }
            //            //Burada PdfOkuAjan da pdf metni işlendiği için hangi buton seçili hangisi değil sistem anlamıyor bu yüzden alttaki iki satır pdf türü tespit etmek ve okumak için
            //            string pdf = await PdfOkuBaykal(filePath);
            //            string pftTuruTespitEt = TespitEtPdfTuru(pdf);

            //            if (!ButonVePdfUyumluMu(pftTuruTespitEt))
            //            {
            //                MessageBox.Show($"Hatalı seçim: {pftTuruTespitEt} PDF yüklendi, ancak {_seciliButon.Text} butonu seçili.",
            //                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                progressBar1.Visible = false;
            //                return;
            //            }

            //            await PdfYukle(filePath);

            //            _formArayuzu.RichTextBox1Yaz(pdfText);
            //            string islenmisVeri = IslenmisVeri();
            //            _formArayuzu.RichTextBox4Yaz(islenmisVeri);

            //            string islenmisVeri2 = IslenmisVeri2();
            //            _formArayuzu.RichTextBox4Yaz(islenmisVeri2);


            //            (List<MalzemeBilgisi> validData, List<string> InvalidData) = await Task.Run(() =>
            //            {
            //                try
            //                {
            //                    if (_seciliButon == btnAjan)
            //                        return _veriOkuma.AjanOku(islenmisVeri);
            //                    else if (_seciliButon == btnBaykal)
            //                        return _veriOkuma.BaykalOku(islenmisVeri2);
            //                    else if (_seciliButon == btnAdm)
            //                        return (_veriOkuma.LantekOku(pdfText), new List<string>());

            //                    else
            //                        throw new InvalidOperationException("Geçersiz buton seçimi.");
            //                }
            //                catch (Exception ex)
            //                {
            //                    return (null, new List<string> { $"Veri işleme hatası: {ex.Message}" });
            //                }
            //            });

            //            if ((validData == null || validData.Count == 0) && (InvalidData == null || InvalidData.Count == 0))
            //            {
            //                MessageBox.Show("Hiçbir veri işlenemedi. PDF içeriğini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                progressBar1.Visible = false;
            //                return;
            //            }
            //            _formArayuzu.RichTextBox2Temizle();
            //            dataGridView1.Rows.Clear();

            //            int totalItems = validData?.Count ?? 0;
            //            int currentItem = 0;

            //            if (validData != null)
            //            {
            //                foreach (var data in validData)
            //                {
            //                    if (data != null)
            //                    {
            //                        _formArayuzu.RichTextBox2MetinEkle($"{data.Kalite} - {data.Malzeme} - {data.Kalip} - {data.Poz} - {data.Adet} - {data.Proje}\n");
            //                        dataGridView1.Rows.Add(data.Kalite, data.Malzeme, data.Kalip, data.Poz, data.Adet, data.Proje);
            //                        UpdateTextBoxes();
            //                        currentItem++;
            //                        int progressValue = totalItems > 0 ? (int)((currentItem / (float)totalItems) * 100) : 0;
            //                        progressBar1.Value = progressValue;
            //                    }
            //                }
            //            }

            //            if (InvalidData != null && InvalidData.Any())
            //            {
            //                foreach (var invalidLine in InvalidData)
            //                {
            //                    _formArayuzu.RichTextBox3MetinEkle($"{invalidLine}\n");
            //                }
            //                MessageBox.Show("Bazı veriler formata uymadığı için 'Sistem > Hatalı Format' bölümüne eklendi. Lütfen kontrol edin ve düzenleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            }
            //            if (_seciliButon == btnBaykal)
            //            {
            //                //BaykalTekSayfaDetay();
            //            }
            //            else if (_seciliButon == btnAjan)
            //            {
            //            }

            //            progressBar1.Visible = false;
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            progressBar1.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Dosya bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        progressBar1.Visible = false;
            //    }
            //}

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
                        string islenmisVeri2 = null;
                        string islenmisVeri = null;

                        if (_seciliButon == btnBaykal)
                        {
                            pdfText = await PdfOkuBaykal(filePath);

                            _formArayuzu.RichTextBox1Yaz(pdfText);

                            islenmisVeri2 = IslenmisVeriBaykal();
                            _formArayuzu.RichTextBox4Yaz(islenmisVeri2);
                        }
                        else if (_seciliButon == btnAjan)
                        {
                            pdfText = await PdfOkuAjan(filePath);

                            _formArayuzu.RichTextBox1Yaz(pdfText);


                            islenmisVeri = IslenmisVeriAjan();
                            _formArayuzu.RichTextBox4Yaz(islenmisVeri);
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
                                    return _veriOkuma.PlazmaOku(islenmisVeri);
                                else if (_seciliButon == btnBaykal)
                                    return _veriOkuma.PlazmaOku(islenmisVeri2);
                                else if (_seciliButon == btnAdm)
                                    return (_veriOkuma.LantekOku(pdfText), new List<string>());

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
                        if (_seciliButon == btnBaykal)
                        {
                            //BaykalTekSayfaDetay();
                        }
                        else if (_seciliButon == btnAjan)
                        {
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

        public async Task PdfYukle(string filePath)
        {
            try
            {
                progressBar1.Visible = true;
                progressBar1.Value = 0;

                int pageCount = 0;

                await Task.Run(() =>
                {
                    Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();
                    pdfDoc.LoadFromFile(filePath);
                    pageCount = pdfDoc.Pages.Count;

                    for (int i = 0; i < pageCount; i++)
                    {
                        int progressValue = (i + 1) * 100 / pageCount;
                        Invoke(new Action(() => progressBar1.Value = progressValue));
                        Thread.Sleep(20);
                    }
                });

                pdfViewer1.LoadFromFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF yükleme hatası: " + ex.Message);
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
                            pageText.AppendLine($"-------- Sayfa {i} işleniyor ---------------");

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
                            pageText.AppendLine($"-------- Sayfa {i} ---------------");
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
                MessageBox.Show("PDF genel okuma hatası: " + ex.Message);
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
            string malzeme = txtMalzeme.Text;
            string kalite = txtKalite.Text;
            DateTime eklemeTarihi = DateTime.Now;


            HashSet<string> islenmisPaketIdSet = new HashSet<string>();
            bool kayitYapildi = false;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView2.Rows[i];
                if (row.IsNewRow || row.Cells[0].Value == null) continue;

                string orijinalKod = row.Cells[0].Value?.ToString() ?? "";
                string dKesimId = row.Cells[1].Value?.ToString() ?? "";
                string adetStr = row.Cells[2].Value?.ToString() ?? "";
                string paketAdetStr = "";

                foreach (DataGridViewRow d3Row in dataGridView3.Rows)
                {
                    if (d3Row.IsNewRow || d3Row.Cells[0].Value == null) continue;

                    if (d3Row.Cells[0].Value.ToString() == dKesimId)
                    {
                        paketAdetStr = d3Row.Cells[2].Value?.ToString() ?? "";
                        break;
                    }
                }

                if (!islenmisPaketIdSet.Contains(dKesimId) && int.TryParse(paketAdetStr, out int paketAdet))
                {
                    KesimListesiPaketData.SaveKesimDataPaket(
                        olusturan,
                        dKesimId,
                        paketAdet,
                        paketAdet,
                        eklemeTarihi
                    );
                    islenmisPaketIdSet.Add(dKesimId);
                }

                if (!islenmisPaketIdSet.Contains(dKesimId)) continue;

                string[] parcalar = orijinalKod.Split('-');

                int adet = 0;
                string proje = "", kalip = "", poz = "";

                if (parcalar.Length >= 4)
                {
                    kalip = $"{parcalar[0]}-{parcalar[1]}";
                    poz = parcalar[2];

                    string adetToplam = parcalar.LastOrDefault(p => p.Contains("AD"));

                    if (!string.IsNullOrEmpty(adetToplam))
                    {
                        adetToplam = adetToplam.Replace("AD", "");
                        if (!int.TryParse(adetToplam, out adet))
                        {
                            adet = 0;
                        }
                    }

                    string sonParca = parcalar.LastOrDefault(p => p.Contains("."));
                    if (!string.IsNullOrEmpty(sonParca))
                    {
                        proje = sonParca;
                    }
                }

                KesimListesiData.SiradakiIdKaydet(currentId.Value);
                KesimListesiData.SaveKesimData(
                    currentId.Value,
                    olusturan,
                    dKesimId,
                    proje,
                    malzeme,
                    kalite,
                    new string[] { kalip },
                    new string[] { poz },
                    new string[] { adetStr },
                    eklemeTarihi
                );
                string kesimDetaylari = kalite + "-" + malzeme + "-" + kalip + "-" + poz + "-" + proje;
                KesimDetaylariData.SaveKesimDetaylariData(kesimDetaylari, dKesimId, adet, adet);

                kayitYapildi = true;
            }

            if (islenmisPaketIdSet.Count > 0 && !kayitYapildi)
            {
                MessageBox.Show("Paket başarıyla oluşturuldu, ancak herhangi bir içerik eklenmedi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (kayitYapildi)
            {
                var userController = new UserController(_formArayuzu.lblSistemKullaniciMetinAl());
                userController.LogYap("KesimPlaniEklendi", "Kesim Planı Ekle", $"Kullanıcı {currentId.Value} numaralı kesim planını yükledi.");
                MessageBox.Show("Kayıt işlemi tamamlandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Hiçbir geçerli satır bulunamadı, kayıt yapılmadı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*işleniyor\s*---------------", RegexOptions.IgnoreCase);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                currentId = null;

                currentId = KesimListesiData.GetSiradakiId();

                if (currentId == null)
                {
                    MessageBox.Show("Yeni bir ID alınamadı.");
                    return;
                }
                panelVeriYonetim.Enabled = true;
                txtId.Text = currentId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }

            foreach (Control control in panelVeriYonetim.Controls)
            {
                if (control != null)
                {
                    control.Enabled = true;
                }
            }
        }

        private void btnXmlOlustur_Click(object sender, EventArgs e)
        {
            ExportToXmlWithDialog(dataGridView1);
        }
        public void ExportToXmlWithDialog(DataGridView dgv)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "XML Dosyası (*.xml)|*.xml";
                sfd.Title = "XML Dosyasını Kaydet";
                sfd.FileName = "yerlesim_plani.xml";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToXml(dgv, sfd.FileName);
                }
            }
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
                using (XmlWriter writer = XmlWriter.Create(dosyaYolu, ayarlar))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("YerlesimPlaniBilgileri");

                    writer.WriteElementString("ID", txtId.Text);
                    writer.WriteElementString("Site", txtSite.Text);

                    string stokKodu = dgv.Rows[0].Cells[1].Value?.ToString() ?? "";
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

                    string kalite = dgv.Rows[0].Cells[0].Value?.ToString() ?? "";
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

                    writer.WriteElementString("EklemeTarihi", dtEklemeTarihi.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));

                    var parcaBySayfaId = dataGridView2.Rows.Cast<DataGridViewRow>()
                        .Where(row => !row.IsNewRow && row.Cells[1].Value != null)
                        .GroupBy(row => row.Cells[1].Value.ToString())
                        .ToDictionary(g => g.Key, g => g.ToList());

                    foreach (DataGridViewRow sayfaRow in dataGridView3.Rows)
                    {
                        if (sayfaRow.IsNewRow || sayfaRow.Cells[0].Value == null) continue;

                        string sayfaId = sayfaRow.Cells[0].Value.ToString();
                        writer.WriteStartElement("Sayfa");
                        writer.WriteElementString("ID", sayfaId);

                        string tekrarAdeti = sayfaRow.Cells[2].Value?.ToString() ?? "1";
                        writer.WriteElementString("TekrarAdeti", tekrarAdeti);

                        if (parcaBySayfaId.TryGetValue(sayfaId, out var parcaRows))
                        {
                            foreach (var parcaRow in parcaRows)
                            {
                                writer.WriteStartElement("Parca");

                                string kalipVerisi = parcaRow.Cells[0].Value?.ToString() ?? "";
                                string[] parcalar = kalipVerisi.Split('-');
                                string kalip = kalipVerisi, poz = "", proje = "";

                                if (parcalar.Length >= 5)
                                {
                                    kalip = $"{parcalar[0]}-{parcalar[1]}";
                                    poz = parcalar[2];
                                    proje = parcalar[4];

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

                                string ekAdi = "-";
                                string ekOran = "-";
                                string adetToWrite = parcaRow.Cells[2].Value?.ToString() ?? "0";

                                if (parcaRow.Cells[4].Value != null &&
                                    double.TryParse(parcaRow.Cells[4].Value.ToString(), out double oran) &&
                                    parcaRow.Cells[2].Value != null &&
                                    double.TryParse(parcaRow.Cells[2].Value.ToString(), out double adet))
                                {
                                    ekOran = (oran).ToString(CultureInfo.InvariantCulture);
                                    adetToWrite = "0";

                                    List<string> parcaList = parcaRow.Cells[0].Value?.ToString()
                                        .Split('-')
                                        .ToList() ?? new List<string>();

                                    if (parcaList.Any(p => p.Contains("EK")))
                                    {
                                        ekAdi = parcaList.First(p => p.Contains("EK"));
                                    }
                                }

                                writer.WriteElementString("Adet", adetToWrite);
                                writer.WriteElementString("Proje", proje);
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

                var userController = new UserController(_formArayuzu.lblSistemKullaniciMetinAl());
                userController.LogYap("XmlDosyasiOlusturuldu", "Kesim Planı Ekle", $"Kullanıcı {txtId.Text} numaralı kesim planı XML dosyası oluşturdu.");

                MessageBox.Show("XML başarıyla oluşturuldu.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML oluşturma sırasında hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string islenmisVeri = IslenmisVeriAjan();
            _formArayuzu.RichTextBox4Yaz(islenmisVeri);
            _veriOkuma.AjanOku(islenmisVeri);
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
        private void AjanSayfaPozDagitimi()
        {
            try
            {
                string id = txtId.Text.Trim();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!");
                    return;
                }

                var logLines = new List<string> {
            "=== AjanLog.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            $"txtId: {id}",
            "\nRichTextBox4 Verileri:"
        };

                Regex pozRegex = new Regex(
    @"ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,4}\s*-\s*(\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2}(?:-\s*EK\d{1,2})?)",
    RegexOptions.IgnoreCase
);
                Regex suffixRegex = new Regex(@"-(?!EK\d+$)[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
                Regex sayfaSimpleRegex = new Regex(@"Sayfa:\s*(\d+)", RegexOptions.IgnoreCase);

                var lines4 = _formArayuzu.RichTextBox4SatirlariAl();
                Dictionary<string, int> sayfaSayaclari = new Dictionary<string, int>();
                HashSet<string> validSayfaNos = new HashSet<string>();

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // --- BURASI DÜZENLENDİ ---
                    int sayfaStart = line.IndexOf("(Sayfa:");
                    int sayfaEnd = sayfaStart >= 0 ? line.IndexOf(')', sayfaStart) : -1;

                    string parcaAdiFull = (sayfaStart >= 0 && sayfaEnd > sayfaStart)
                        ? line.Substring(0, sayfaEnd + 1).Trim()  // (Sayfa: ...) kısmı dahil
                        : line.Trim();
                    // --- DÜZENLEME BİTTİ ---

                    // Sayfa numarasını almak için orijinal satırdan kullanmaya devam
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

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logPath = Path.Combine(desktopPath, "AjanLog.txt");
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

                MessageBox.Show("Veriler DataGridView2'ye eklendi ve log dosyası masaüstüne kaydedildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void BaykalSayfaPozDagitimi2()
        {
            try
            {
                string baseId = txtId.Text.Trim();
                if (string.IsNullOrEmpty(baseId))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!");
                    return;
                }

                string text = _formArayuzu.RichTextBox1MetniAl().Trim();
                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("Lütfen richTextBox1 alanını doldurun!");
                    return;
                }

                if (dataGridView2.Columns.Count == 0)
                {
                    dataGridView2.Columns.Add("Poz", "Poz");
                    dataGridView2.Columns.Add("SayfaID", "Sayfa ID");
                    dataGridView2.Columns.Add("Adet", "Adet");
                }

                var logLines = new List<string>
        {
            "=== ProNest_Log.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            $"BaseId: {baseId}",
            "\nRichTextBox1 Verileri:"
        };
                Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*işleniyor\s*---------------", RegexOptions.IgnoreCase);
                Regex yerlesimRegex = new Regex(@"Yerleşim: (\d+) / \d+", RegexOptions.IgnoreCase);
                Regex kesmeRegex = new Regex(@"Kesme sayısı: (\d+)", RegexOptions.IgnoreCase);
                Regex partRegex = new Regex(@"(\d+)(?:\s*-\s*(\d+))?\s+.*?(\d+-\d+-P\d+(?:-[\w\.]+)*) .*?(\d{1,3}(?:,\d{1,2})?)\s*kg", RegexOptions.IgnoreCase);
                Regex siralamaRegex = new Regex(@"^\s*Sıralama\s*$", RegexOptions.IgnoreCase);

                var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var validYerlesimNo = new HashSet<string>();
                var validSayfaNo = new HashSet<string>();
                var sayfaSayaclari = new Dictionary<string, int>();
                var partInfoDict = new Dictionary<string, (string Poz, string SayfaID, int Count)>();
                string currentSayfa = "";
                string currentYerlesim = "";
                string baseLayout = "";
                int continuationCount = 0;
                bool isPartSection = false;
                string tekrarSayisi = "0";

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var yerlesimMatch = yerlesimRegex.Match(line);
                    if (yerlesimMatch.Success)
                    {
                        currentYerlesim = yerlesimMatch.Groups[1].Value;
                        baseLayout = currentYerlesim;
                        continuationCount = 0;
                        isPartSection = false;
                        validYerlesimNo.Add(currentYerlesim);
                        logLines.Add($"Yerleşim tespit edildi: {currentYerlesim}, ID: {baseId}-{currentYerlesim}");
                        continue;
                    }
                    var sayfaMatch = sayfaRegex.Match(line);
                    if (sayfaMatch.Success)
                    {
                        currentSayfa = sayfaMatch.Groups[1].Value;
                        baseLayout = currentSayfa;
                        continuationCount = 0;
                        isPartSection = false;
                        validSayfaNo.Add(currentSayfa);
                        logLines.Add($"Sayfa tespit edildi: {currentSayfa}, ID: {baseId}-{currentSayfa}");
                        continue;
                    }

                    var kesmeMatch = kesmeRegex.Match(line);
                    if (kesmeMatch.Success)
                    {
                        tekrarSayisi = kesmeMatch.Groups[1].Value;
                        logLines.Add($"Kesme sayısı bulundu: {tekrarSayisi}");
                        continue;
                    }

                    if (siralamaRegex.IsMatch(line))
                    {
                        isPartSection = true;
                        string sayfaID = $"{baseId}-{currentSayfa.PadLeft(2, '0')}";
                        if (!sayfaSayaclari.ContainsKey(currentSayfa))
                        {
                            sayfaSayaclari[currentSayfa] = 0;
                        }
                        logLines.Add($"Sıralama bölümü başladı, sayfa: {currentSayfa}, yerleşim:{currentYerlesim}, ID: {sayfaID}, Tekrar: {tekrarSayisi}");
                        continue;
                    }

                    if (isPartSection && partRegex.IsMatch(line))
                    {
                        var partMatch = partRegex.Match(line);
                        if (partMatch.Success)
                        {
                            string startNum = partMatch.Groups[1].Value;
                            string endNum = partMatch.Groups[2].Value;
                            string poz = partMatch.Groups[3].Value;
                            int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                            string currentLayout = continuationCount == 0 ? currentSayfa : $"{baseLayout}-{continuationCount}";
                            string sayfaID = $"{baseId}-{currentSayfa.PadLeft(2, '0')}";
                            int currentCount;
                            sayfaSayaclari.TryGetValue(currentSayfa, out currentCount);
                            sayfaSayaclari[currentSayfa] = currentCount + 1;
                            string parcaID = $"{sayfaID}-{sayfaSayaclari[currentSayfa].ToString("D2")}";

                            partInfoDict[parcaID] = (poz, sayfaID, count);
                            logLines.Add($"{poz}, Yerleşim: {currentLayout}, Sayfa: {currentSayfa}, Adet: {count}, Parça ID: {parcaID}, Satır: {line}");
                        }
                        else
                        {
                            logLines.Add($"Parça satırı eşleşmedi: {line}");
                        }
                    }
                    else if (isPartSection && !siralamaRegex.IsMatch(line) && partRegex.IsMatch(line))
                    {
                        continuationCount++;
                        string currentLayout = $"{baseLayout}-{continuationCount}";
                        logLines.Add($"Devam eden yerleşim: {currentLayout}");

                        var partMatch = partRegex.Match(line);
                        if (partMatch.Success)
                        {
                            string startNum = partMatch.Groups[1].Value;
                            string endNum = partMatch.Groups[2].Value;
                            string poz = partMatch.Groups[3].Value;
                            int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                            string sayfaID = $"{baseId}-{currentSayfa.PadLeft(2, '0')}";
                            int currentCount;
                            sayfaSayaclari.TryGetValue(currentSayfa, out currentCount);
                            sayfaSayaclari[currentSayfa] = currentCount + 1;
                            string parcaID = $"{sayfaID}-{sayfaSayaclari[currentSayfa].ToString("D2")}";

                            partInfoDict[parcaID] = (poz, sayfaID, count);
                            logLines.Add($"Poz bulundu: {poz}, Yerleşim: {currentLayout}, Adet: {count}, Parça ID: {parcaID}, Satır: {line}");
                        }
                        else
                        {
                            logLines.Add($"Parça satırı eşleşmedi: {line}");
                        }
                    }
                }

                logLines.Add("\nSon Çıktı Verileri:");
                foreach (var kvp in partInfoDict)
                {
                    string parcaID = kvp.Key;
                    var (poz, sayfaID, count) = kvp.Value;
                    logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Sayfa ID: {sayfaID}, Adet: {count}");
                    dataGridView2.Rows.Add(poz, sayfaID, count);
                }

                var uniquePozs = partInfoDict.Values.Select(p => p.Poz).Distinct().OrderBy(p => p).ToList();
                logLines.Add($"\nBenzersiz pozlar: {string.Join(", ", uniquePozs)}");

                if (!uniquePozs.Any())
                {
                    MessageBox.Show("Hiçbir poz bulunamadı. Lütfen veri formatını kontrol edin.");
                    return;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logPath = Path.Combine(desktopPath, "ProNest_Log.txt");
                File.WriteAllLines(logPath, logLines);

                MessageBox.Show("Veriler DataGridView2'ye eklendi ve log dosyası masaüstüne kaydedildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void BaykalSayfaPozDagitimi()
        {
            try
            {
                string baseId = txtId.Text.Trim();
                if (string.IsNullOrEmpty(baseId))
                {
                    MessageBox.Show("Lütfen txtId alanını doldurun!");
                    return;
                }

                string text1 = _formArayuzu.RichTextBox1MetniAl().Trim();
                if (string.IsNullOrEmpty(text1))
                {
                    MessageBox.Show("Lütfen richTextBox1 alanını doldurun!");
                    return;
                }

                string text4 = _formArayuzu.RichTextBox4MetniAl().Trim();
                if (string.IsNullOrEmpty(text4))
                {
                    MessageBox.Show("Lütfen richTextBox4 alanını doldurun!");
                    return;
                }

                var logLines = new List<string>
        {
            "=== ProNest_Log.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            $"BaseId: {baseId}",
            "\nRichTextBox1 Verileri:"
        };

                Regex sayfaRegex = new Regex(@"--------\s*Sayfa\s*(\d+)\s*işleniyor\s*---------------", RegexOptions.IgnoreCase);
                Regex partRegex = new Regex(@"(\d+)(?:\s*-\s*(\d+))?\s+.*?(\d+-\d+-P\d+(?:-[\w\.]+)*)\s.*?(\d{1,3}(?:,\d{1,2})?)\s*kg", RegexOptions.IgnoreCase);
                Regex richTextBox4SayfaRegex = new Regex(@"\(Sayfa:\s*(\d+)\)", RegexOptions.IgnoreCase);
                Regex richTextBox4YerlesimRegex = new Regex(@"\(Yerleşim:\s*(\d+)\)", RegexOptions.IgnoreCase);

                var lines1 = text1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var lines4 = text4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                var richTextBox1Matches = new Dictionary<string, (int Adet, double Agirlik)>();
                var partInfoDict = new Dictionary<string, (string Poz, string SayfaID, int Adet, double Agirlik)>();

                string currentSayfa = "";
                Dictionary<string, int> sayfaSiraSayaclari = new Dictionary<string, int>();

                // RichTextBox1 işleme
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
                        double agirlik =double.Parse(partMatch.Groups[4].Value);
                        int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                        richTextBox1Matches[id] = (count, agirlik);
                        logLines.Add($"{id}: {line} (Sayfa: {currentSayfa}), Adet: {count}, Ağırlık: {agirlik}");
                        sayfaSiraSayaclari[currentSayfa]++;
                    }
                }

                // RichTextBox4 işleme
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

                    var parcaKoduMatch = Regex.Match(line, @"(ST\d{2}-\d+(?:MM|mm)-.*?)(?=\(Yerleşim:)");

                    if (!parcaKoduMatch.Success)
                    {
                        logLines.Add($"[Atlandı] {id}: Parça kodu bulunamadı -> {line}");
                        continue;
                    }

                    string poz = parcaKoduMatch.Groups[1].Value;

                    if (richTextBox1Matches.TryGetValue(id, out var info))
                    {
                        partInfoDict[id] = (poz, yerlesimId, info.Adet, info.Agirlik);
                        logLines.Add($"{id} => {poz} | Sayfa: {currentSayfa}, Yerleşim: {yerlesimId}");
                    }
                    else
                    {
                        logLines.Add($"[Uyarı] {id}: RichTextBox1 ile eşleşmedi -> {poz}");
                    }

                    sayfaSiraSayaclari[currentSayfa]++;
                }

                // Sonuçları log ve grid'e yaz
                logLines.Add("\nSon Çıktı Verileri:");
                foreach (var kvp in partInfoDict)
                {
                    string parcaID = kvp.Key;
                    var (poz, sayfaID, count, agirlik) = kvp.Value;
                    logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Sayfa ID: {sayfaID}, Adet: {count}, Ağırlık: {agirlik}");
                    dataGridView2.Rows.Add(poz, sayfaID, count, agirlik);
                }

                var uniquePozs = partInfoDict.Values.Select(p => p.Poz).Distinct().OrderBy(p => p).ToList();
                logLines.Add($"\nBenzersiz pozlar: {string.Join(", ", uniquePozs)}");

                if (!uniquePozs.Any())
                {
                    MessageBox.Show("Hiçbir poz bulunamadı. Lütfen veri formatını kontrol edin.");
                    return;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logPath = Path.Combine(desktopPath, "ProNest_Log.txt");
                File.WriteAllLines(logPath, logLines);

                MessageBox.Show("Veriler işlendi, log dosyasına yazıldı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }



        private void SayfaIDTabloVerileri()
        {
            dataGridView3.Rows.Clear();
            string metin = _formArayuzu.RichTextBox1MetniAl();


            if (string.IsNullOrWhiteSpace(metin))
            {
                MessageBox.Show("richTextBox1 boş!");
                return;
            }

            HashSet<string> benzersizDegerler = new HashSet<string>();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow && row.Cells[1].Value != null)
                {
                    string tamDeger = row.Cells[1].Value.ToString();
                    if (benzersizDegerler.Add(tamDeger))
                    {
                        int rowIndex = dataGridView3.Rows.Add();
                        dataGridView3.Rows[rowIndex].Cells[0].Value = tamDeger;

                        string[] parcalar = tamDeger.Split('-');
                        if (parcalar.Length == 2 && int.TryParse(parcalar[1], out int sayi))
                        {
                            dataGridView3.Rows[rowIndex].Cells[1].Value = sayi.ToString();
                        }
                    }
                }
            }

            if (_seciliButon == btnBaykal)
            {
                string[] sayfalar = metin.Split(new[] { "-------- Sayfa" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sayfa in sayfalar)
                {
                    Match yerlesimMatch = Regex.Match(sayfa, @"Yerleşim:\s*(\d+)\s*/\s*\d+");
                    Match kesmeMatch = Regex.Match(sayfa, @"Kesme sayısı:\s*(\d+)");
                    Match sayfaNoMatch = Regex.Match(sayfa, @"\d{2}\.\d{2}\.\d{4}\s+\d{2}:\d{2}:\d{2}\s+(\d+)\s*/\s*\d+");

                    if (yerlesimMatch.Success && kesmeMatch.Success && sayfaNoMatch.Success)
                    {
                        string yerlesimNo = yerlesimMatch.Groups[1].Value;
                        string kesmeSayisi = kesmeMatch.Groups[1].Value;
                        string sayfaNo = sayfaNoMatch.Groups[1].Value;

                        foreach (DataGridViewRow drow in dataGridView3.Rows)
                        {
                            if (drow.Cells[1].Value != null && drow.Cells[1].Value.ToString() == yerlesimNo)
                            {
                                drow.Cells[1].Value = sayfaNo;
                                drow.Cells[2].Value = kesmeSayisi;
                                break;
                            }
                        }
                    }
                }
            }
            else if (_seciliButon == btnAjan)
            {
                string[] sayfalar = metin.Split(new[] { "-------- Sayfa" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sayfa in sayfalar)
                {
                    Match sayfaHeaderMatch = Regex.Match(sayfa, @"^\s*(\d+)\s*--------", RegexOptions.Multiline);
                    Match levhaOlcuMatch = Regex.Match(sayfa, @"LevhaÖlçüleri\s+\d+X\d+\w*\s+(\d+)\s+Kalınlık\s*\(mm\)", RegexOptions.IgnoreCase);

                    if (sayfaHeaderMatch.Success && levhaOlcuMatch.Success)
                    {
                        string sayfaNo = sayfaHeaderMatch.Groups[1].Value;
                        string levhaAdet = levhaOlcuMatch.Groups[1].Value;

                        foreach (DataGridViewRow drow in dataGridView3.Rows)
                        {
                            if (drow.Cells[1].Value != null && drow.Cells[1].Value.ToString() == sayfaNo)
                            {
                                drow.Cells[2].Value = levhaAdet;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!sayfaHeaderMatch.Success) Console.WriteLine("Sayfa başlığı eşleşmedi!");
                        if (!levhaOlcuMatch.Success) Console.WriteLine("LevhaÖlçüleri eşleşmedi!");
                    }
                }
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

                Regex ekRegex = new Regex(@"-EK\d+$", RegexOptions.IgnoreCase);

                if (!dataGridView2.Columns.Contains("Oran"))
                {
                    dataGridView2.Columns.Add("Oran", "Toplam Ağırlık Oranı");
                }

                var pozGroups = new Dictionary<string, List<(string Poz, string SayfaID, int Adet, double Agirlik)>>();

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
                        pozGroups[basePoz] = new List<(string, string, int, double)>();

                    pozGroups[basePoz].Add((poz, sayfaID, adet, agirlik));
                }

                logLines.Add("\nEK İbaresi İçeren Pozlar ve Yüzde Hesaplamaları:");

                foreach (var group in pozGroups)
                {
                    string basePoz = group.Key;
                    var items = group.Value;

                    if (items.Any(item => ekRegex.IsMatch(item.Poz)) && items.Count > 1)
                    {
                        double toplamAgirlik = items.Sum(item => item.Agirlik * item.Adet);

                        logLines.Add($"\nPoz: {basePoz}, Toplam Ağırlık: {toplamAgirlik}");

                        foreach (var item in items)
                        {
                            double oran = (item.Agirlik * item.Adet) / toplamAgirlik;
                            logLines.Add($"  - Poz: {item.Poz}, Sayfa ID: {item.SayfaID}, Adet: {item.Adet}, Ağırlık: {item.Agirlik}, Parçanın Toplam Ağırlığa Oranı {oran}");

                            var matchingRow = dataGridView2.Rows
                                .Cast<DataGridViewRow>()
                                .FirstOrDefault(r => !r.IsNewRow &&
                                                     (r.Cells[0].Value?.ToString() ?? "") == item.Poz &&
                                                     (r.Cells[1].Value?.ToString() ?? "") == item.SayfaID);

                            if (matchingRow != null)
                            {
                                matchingRow.Cells["Oran"].Value = $"{oran}";
                            }
                        }
                    }
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logPath = Path.Combine(desktopPath, "EkAdetAgirlikYuzdeLog.txt");
                File.WriteAllLines(logPath, logLines);

                MessageBox.Show("EK ibaresi içeren pozların ağırlık yüzdeleri hesaplandı ve tabloya yazıldı. Log dosyası masaüstüne kaydedildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
