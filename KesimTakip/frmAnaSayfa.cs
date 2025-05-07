using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.DataBase;
using System.IO;
using iText.Kernel.Pdf.Canvas.Parser;
using KesimTakip.Helper;
using System.Xml;
using KesimTakip.Business;
using KesimTakip.Entitys;
using static KesimTakip.Business.VeriOkuma;

namespace KesimTakip
{
    public partial class frmAnaSayfa : Form
    {
        private readonly VeriOkuma _veriOkuma;
        private Button _seciliButon;
        private int? currentId = null;

        private Timer timer;
        private int timerCounter = 0;
        private List<Button> buttonGroup;
        public frmAnaSayfa(string adSoyad)
        {
            InitializeComponent();

            txtOlusturan.Text = adSoyad;
            lblSistemKullanici.Text = adSoyad;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            ShowCurrentDateTime();

            panelAraYuz.Dock = DockStyle.Left;
            panelAraYuz.Width = 150;

            panelSistem.Height = 300;
            panelYardim.Height = 300;

            DataGridViewHelper.StilUygula(dataGridView1);
            DataGridViewHelper.StilUygula(dataGridView2);
            DataGridViewHelper.StilUygula(dataGridView3);
            DataGridViewHelper.StilUygula(dataGridView4);
            _veriOkuma = new VeriOkuma();
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
        private void btnYeni_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentId == null)
                {
                    currentId = KesimListesiData.GetSiradakiId();
                }
                btnSec.Enabled = true;
                txtId.Text = currentId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
        private async void btnSec_Click(object sender, EventArgs e)
        {
            if (_seciliButon == null)
            {
                MessageBox.Show("Lütfen önce bir makine seçin (Ajan, Adm, Baykal).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            lblKesimId.Enabled = false;
            txtKesimId.Enabled = false;
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
                        if (_seciliButon == btnBaykal)
                        {
                            pdfText = await PdfOkuBaykal(filePath);
                        }
                        else if (_seciliButon == btnAjan)
                        {
                            pdfText = await PdfOkuAjan(filePath);
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

                        string pdfTuru = TespitEtPdfTuru(pdfText);
                        if (string.IsNullOrEmpty(pdfTuru))
                        {
                            MessageBox.Show("PDF türü tespit edilemedi. Dosya içeriğini kontrol edin.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        if (!ButonVePdfUyumluMu(pdfTuru))
                        {
                            MessageBox.Show($"Hatalı seçim: {pdfTuru} PDF'si yüklendi, ancak {_seciliButon.Text} butonu seçili.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        await PdfYukle(filePath);

                        richTextBox1.Text = pdfText;

                        string islenmisVeri = IslenmisVeri();
                        richTextBox4.Text = islenmisVeri;




                        (List<MalzemeBilgisi> validData, List<string> InvalidData) = await Task.Run(() =>
                        {
                            try
                            {
                                if (_seciliButon == btnAjan)
                                    return _veriOkuma.AjanOku(islenmisVeri);
                                else if (_seciliButon == btnAdm)
                                    return (_veriOkuma.LantekOku(pdfText), new List<string>());
                                else if (_seciliButon == btnBaykal)
                                    return _veriOkuma.BaykalOku(pdfText);
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

                        richTextBox2.Clear();
                        dataGridView1.Rows.Clear();

                        int totalItems = validData?.Count ?? 0;
                        int currentItem = 0;

                        if (validData != null)
                        {
                            foreach (var data in validData)
                            {
                                if (data != null)
                                {
                                    richTextBox2.AppendText($"{data.Kalite} - {data.Kalinlik} - {data.Kalip} - {data.Poz} - {data.Adet} - {data.Proje}\n");
                                    dataGridView1.Rows.Add(data.Kalite, data.Kalinlik, data.Kalip, data.Poz, data.Adet, data.Proje);

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
                                richTextBox3.AppendText($"{invalidLine}\n");
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

        public string IslenmisVeri()
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("richTextBox1 boş. Lütfen veri girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }

            StringBuilder sonucBuilder = new StringBuilder();
            string[] satirlar = richTextBox1.Text
                .Replace("\r\n", "\n")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            bool isFirmaAdiSection = false;
            bool veriIslendi = false;
            int currentPageNumber = 1;
            int lastFirmaPageNumber = 1;

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
                    satir.StartsWith("02Mayıs2025") ||
                    satir.StartsWith("25Nisan2025") ||
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

                string birlesikVeri = $"{parcaAdi}{eklenecekVeri} (Sayfa: {lastFirmaPageNumber})";

                sonucBuilder.AppendLine(birlesikVeri);
                veriIslendi = true;
            }

            return sonucBuilder.ToString();
        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = richTextBox3.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                richTextBox2.Clear();
                dataGridView1.Rows.Clear();

                foreach (string line in lines)
                {
                    string[] parts = line.Split('-').Select(p => p.Trim()).ToArray();

                    if (parts.Length >= 6)
                    {
                        string kalite = parts[0];
                        string kalinlik = parts[1];
                        string kalip = parts[2] + "-" + parts[3];
                        string poz = parts[4];
                        string adet = parts[5];
                        string proje = parts[6];

                        richTextBox2.AppendText($"{kalite} - {kalinlik} - {kalip} - {poz} - {adet} - {proje}\n");

                        dataGridView1.Rows.Add(kalite, kalinlik, kalip, poz, adet, proje);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
        public async Task PdfYukle(string filePath)
        {
            try
            {
                Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();
                pdfDoc.LoadFromFile(filePath);
                int pageCount = pdfDoc.Pages.Count;

                for (int i = 0; i < pageCount; i++)
                {
                    int progressValue = (i + 1) * 100 / pageCount;
                    Invoke((Action)(() => progressBar1.Value = progressValue));
                    await Task.Delay(20);
                }

                pdfViewer1.LoadFromFile(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF yükleme hatası: " + ex.Message);
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
            HashSet<string> kalinlikSet = new HashSet<string>();
            StringBuilder kalipBuilder = new StringBuilder();
            StringBuilder pozBuilder = new StringBuilder();
            StringBuilder adetBuilder = new StringBuilder();
            StringBuilder projeBuilder = new StringBuilder();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    kaliteSet.Add(row.Cells[0].Value?.ToString());
                    kalinlikSet.Add(row.Cells[1].Value?.ToString());
                    kalipBuilder.Append(row.Cells[2].Value?.ToString() + ";");
                    pozBuilder.Append(row.Cells[3].Value?.ToString() + ";");
                    adetBuilder.Append(row.Cells[4].Value?.ToString() + ";");
                    projeBuilder.Append(row.Cells[5].Value?.ToString() + ";");
                }
            }

            txtKalite.Text = string.Join(";", kaliteSet);
            txtKalinlik.Text = string.Join(";", kalinlikSet);
            txtKalipNo.Text = kalipBuilder.ToString().TrimEnd(';');
            txtKesilenPozlar.Text = pozBuilder.ToString().TrimEnd(';');
            txtKPAdet.Text = adetBuilder.ToString().TrimEnd(';');
            txtProjeNo.Text = projeBuilder.ToString().TrimEnd(';');
        }

        private void btnTumunuEkle_Click(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow seciliSatir = dataGridView1.SelectedRows[0];

                txtKalite.Text = seciliSatir.Cells.Count > 0 && seciliSatir.Cells[0].Value != null ? seciliSatir.Cells[0].Value.ToString() : "";
                txtKalinlik.Text = seciliSatir.Cells.Count > 1 && seciliSatir.Cells[1].Value != null ? seciliSatir.Cells[1].Value.ToString() : "";
                txtKalipNo.Text = seciliSatir.Cells.Count > 2 && seciliSatir.Cells[2].Value != null ? seciliSatir.Cells[2].Value.ToString() : "";
                txtKesilenPozlar.Text = seciliSatir.Cells.Count > 3 && seciliSatir.Cells[3].Value != null ? seciliSatir.Cells[3].Value.ToString() : "";
                txtKPAdet.Text = seciliSatir.Cells.Count > 4 && seciliSatir.Cells[4].Value != null ? seciliSatir.Cells[4].Value.ToString() : "";
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Text.Trim());
                string olusturan = txtOlusturan.Text.Trim();

                if (!int.TryParse(txtKesimId.Text.Trim(), out int kesimid))
                {
                    MessageBox.Show("Geçersiz Kesim ID! Lütfen geçerli bir sayı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (KesimListesiPaketData.KesimListesiPaketKesimIdVarsa(kesimid))
                {
                    MessageBox.Show("Bu Kesim ID zaten mevcut! Lütfen farklı bir Kesim ID girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string projeNo = txtProjeNo.Text.Trim();
                string kalinlik = txtKalinlik.Text.Trim();
                string kalite = txtKalite.Text.Trim();
                int kesilecekPlanSayisi = int.Parse(txtKesimPlaniTekrarSayisi.Text.Trim());
                int toplamPlanTekrari = int.Parse(txtKesimPlaniTekrarSayisi.Text.Trim());
                string eklemeTekrari = dtEklemeTarihi.Text.Trim();

                string[] kaliplar = txtKalipNo.Text.Split(';');
                string[] pozlar = txtKesilenPozlar.Text.Split(';');
                string[] adetler = txtKPAdet.Text.Split(';');

                btnSec.Enabled = false;
                KesimListesiData.SiradakiIdKaydet(currentId.Value);
                KesimListesiData.SaveKesimData(currentId.Value, olusturan, kesimid, projeNo, kalinlik, kalite, kaliplar, pozlar, adetler, eklemeTekrari);
                KesimListesiPaketData.SaveKesimDataPaket(olusturan, kesimid, kesilecekPlanSayisi, toplamPlanTekrari, eklemeTekrari);

                MessageBox.Show("Kesimler Başarıyla Kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentId = null;
                txtId.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSistem_Click(object sender, EventArgs e)
        {
            PanelGosterYardimMenu(panelSistem);
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
                    string olusturan = txtOlusturan.Text;
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
            string kesimYapanKullanici = lblSistemKullanici.Text;
            frmKesimYap kesyap = new frmKesimYap(kesimYapanKullanici);
            kesyap.Show();
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

        private void btnXmlOlustur_Click(object sender, EventArgs e)
        {
            lblKesimId.Enabled = true;
            txtKesimId.Enabled = true;
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
            XmlWriterSettings ayarlar = new XmlWriterSettings();
            ayarlar.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(dosyaYolu, ayarlar))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("YerleşimPlanıBilgileri");

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
                if (kalite == "ST37")
                    kaliteXml = "S235JR";
                else if (kalite == "ST44")
                    kaliteXml = "S275JR";
                else if (kalite == "ST52")
                    kaliteXml = "S355J2";
                writer.WriteElementString("Kalite", kaliteXml);

                writer.WriteElementString("EklemeTarihi", dtEklemeTarihi.Value.ToString("dd.MM.yyyy"));

                foreach (DataGridViewRow sayfaRow in dataGridView3.Rows)
                {
                    if (sayfaRow.IsNewRow || sayfaRow.Cells[0].Value == null) continue;

                    string sayfaId = sayfaRow.Cells[0].Value.ToString();

                    writer.WriteStartElement("Sayfa");
                    writer.WriteAttributeString("ID", sayfaId);

                    foreach (DataGridViewRow parcaRow in dataGridView2.Rows)
                    {
                        if (parcaRow.IsNewRow || parcaRow.Cells[1].Value == null) continue;

                        if (parcaRow.Cells[1].Value.ToString() == sayfaId)
                        {
                            writer.WriteStartElement("Parca");

                            string kalipVerisi = parcaRow.Cells[0].Value?.ToString() ?? "";
                            string[] parcalar = kalipVerisi.Split('-');

                            string kalip = "";
                            string poz = "";
                            string proje = "";

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
                            else
                            {
                                kalip = kalipVerisi;
                            }

                            writer.WriteElementString("Kalip", kalip);
                            writer.WriteElementString("Poz", poz);
                            writer.WriteElementString("Adet", parcaRow.Cells[2].Value?.ToString());
                            writer.WriteElementString("Proje", proje);

                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            MessageBox.Show("XML başarıyla oluşturuldu.");
        }

        private void btnKesimPlaniEkle_Click(object sender, EventArgs e)
        {
            PanelGosterAnaPaneller(panelKesimPlaniEkle);
        }

        private void PanelGosterAnaPaneller(Panel target)
        {
            panelKesimPlaniEkle.Visible = false;

            target.Visible = true;
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
        private void button1_Click(object sender, EventArgs e)
        {
            BaykalSayfaPozDagitimi();
            SayfaIDTabloVerileri();    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AjanSayfaPozDagitimi();
            SayfaIDTabloVerileri();
        }


        //private void AjanSayfaPozDagitimi()
        //{
        //    try
        //    {
        //        string id = txtId.Text.Trim();
        //        if (string.IsNullOrEmpty(id))
        //        {
        //            MessageBox.Show("Lütfen txtId alanını doldurun!");
        //            return;
        //        }

        //        if (dataGridView2.Columns.Count == 0)
        //        {
        //            dataGridView2.Columns.Add("ParcaID", "Parça ID");
        //            dataGridView2.Columns.Add("SayfaID", "Sayfa ID");
        //        }

        //        var logLines = new List<string> {
        //    "=== AjanLog.txt ===",
        //    $"İşlem Tarihi: {DateTime.Now}",
        //    $"txtId: {id}",
        //    "\nRichTextBox4 Verileri:"
        //};

        //        Regex pozRegex = new Regex(
        //            @"ST[A-Z0-9]{2,}\s*-\s*[A-Z0-9]{2,4}\s*-\s*(\d{1,3}\s*-\s*\d{1,3}\s*-\s*P\d+\s*-\s*\d+AD\s*-\s*\d{5,6}\.\d{2}(?:-\s*EK\d{1,2})?)",
        //            RegexOptions.IgnoreCase
        //        );
        //        Regex suffixRegex = new Regex(@"-(?!EK\d+$)[A-Za-z0-9]+$", RegexOptions.IgnoreCase);
        //        Regex sayfaSimpleRegex = new Regex(@"Sayfa:\s*(\d+)", RegexOptions.IgnoreCase);

        //        var lines4 = richTextBox4.Lines;
        //        Dictionary<string, int> sayfaSayaclari = new Dictionary<string, int>();
        //        HashSet<string> validSayfaNos = new HashSet<string>();

        //        foreach (var line in lines4)
        //        {
        //            if (string.IsNullOrWhiteSpace(line)) continue;

        //            var parts = line.Split(new[] { " (Sayfa: " }, StringSplitOptions.None);
        //            if (parts.Length < 2) continue;

        //            string parcaAdiFull = parts[0].Trim();
        //            string sayfaNo = parts[1].Replace(")", "").Trim();

        //            validSayfaNos.Add(sayfaNo);

        //            if (!sayfaSayaclari.ContainsKey(sayfaNo))
        //            {
        //                sayfaSayaclari[sayfaNo] = 1;
        //            }
        //            else
        //            {
        //                sayfaSayaclari[sayfaNo]++;
        //            }

        //            Match pozMatch = pozRegex.Match(parcaAdiFull);
        //            string poz = pozMatch.Success ? pozMatch.Groups[1].Value : "Poz Yok";

        //            string parcaID = suffixRegex.Replace(parcaAdiFull, "");

        //            string sayfaID = $"{id}-{sayfaNo.PadLeft(2, '0')}";
        //            string parcaIDFinal = $"{sayfaID}-{sayfaSayaclari[sayfaNo].ToString("D2")}";

        //            logLines.Add($"{parcaAdiFull} => Poz: {poz}, Parça ID: {parcaIDFinal}, Sayfa ID: {sayfaID}");
        //        }

        //        logLines.Add("\nRichTextBox1 Verileri:");
        //        sayfaSayaclari.Clear();
        //        var lines1 = richTextBox1.Lines;
        //        int currentRealSayfaNo = 0;
        //        bool isToplamParcaPage = false;

        //        foreach (var line in lines1)
        //        {
        //            if (string.IsNullOrWhiteSpace(line)) continue;

        //            var sayfaMatch = Regex.Match(line, @"--------\s*Sayfa\s*(\d+)\s*--------");
        //            if (sayfaMatch.Success)
        //            {
        //                currentRealSayfaNo = int.Parse(sayfaMatch.Groups[1].Value);
        //                isToplamParcaPage = false;
        //                continue;
        //            }

        //            var sayfaSimpleMatch = sayfaSimpleRegex.Match(line);
        //            if (sayfaSimpleMatch.Success)
        //            {
        //                currentRealSayfaNo = int.Parse(sayfaSimpleMatch.Groups[1].Value);
        //                isToplamParcaPage = false;
        //                continue;
        //            }

        //            if (line.Contains("ToplamParçaKesmeListesi"))
        //            {
        //                isToplamParcaPage = true;
        //                continue;
        //            }

        //            if (line.StartsWith("ST") && !isToplamParcaPage && validSayfaNos.Contains(currentRealSayfaNo.ToString()))
        //            {
        //                Match pozMatch = pozRegex.Match(line);
        //                string adet = "";
        //                Match adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}:\d{2}\s+(\d+)");
        //                if (!adetMatch.Success)
        //                    adetMatch = Regex.Match(line, @"\d{1,2}:\d{2}\s+(\d+)");
        //                if (adetMatch.Success)
        //                    adet = adetMatch.Groups[1].Value;

        //                string temizParca = suffixRegex.Replace(line.Trim(), "");

        //                string sayfaID = $"{id}-{currentRealSayfaNo.ToString("D2")}";
        //                int parcaIndex = sayfaSayaclari.ContainsKey(currentRealSayfaNo.ToString()) ? sayfaSayaclari[currentRealSayfaNo.ToString()] + 1 : 1;
        //                string parcaIDFinal = $"{sayfaID}-{parcaIndex.ToString("D2")}";

        //                logLines.Add($"{line} (Sayfa:{currentRealSayfaNo}) => Adet: {adet}, Parça ID: {parcaIDFinal}, Sayfa ID: {sayfaID}");

        //                sayfaSayaclari[currentRealSayfaNo.ToString()] = parcaIndex;
        //            }
        //        }

        //        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //        string logPath = Path.Combine(desktopPath, "AjanLog.txt");
        //        logLines.Add("\nSon Çıktı Verileri:");

        //        var rich4Dict = new Dictionary<string, (string Poz, string SayfaID)>();
        //        var rich1Dict = new Dictionary<string, string>();

        //        foreach (var line in logLines)
        //        {
        //            if (line.Contains("RichTextBox4 Verileri:") || line.Contains("RichTextBox1 Verileri:")) continue;

        //            var match = Regex.Match(line, @"Poz: (.+?), Parça ID: (.+?), Sayfa ID: (.+)");
        //            if (match.Success)
        //            {
        //                string poz = match.Groups[1].Value.Trim();
        //                string parcaID = match.Groups[2].Value.Trim();
        //                string sayfaID = match.Groups[3].Value.Trim();
        //                rich4Dict[parcaID] = (poz, sayfaID);
        //            }
        //        }

        //        foreach (var line in logLines)
        //        {
        //            if (!line.Contains("=> Adet:")) continue;

        //            var match = Regex.Match(line, @"Adet: (\d+), Parça ID: (.+?), Sayfa ID");
        //            if (match.Success)
        //            {
        //                string adet = match.Groups[1].Value.Trim();
        //                string parcaID = match.Groups[2].Value.Trim();
        //                rich1Dict[parcaID] = adet;
        //            }
        //        }

        //        foreach (var kvp in rich4Dict)
        //        {
        //            string parcaID = kvp.Key;
        //            var (poz, sayfaID) = kvp.Value;

        //            if (rich1Dict.ContainsKey(parcaID))
        //            {
        //                string adet = rich1Dict[parcaID];
        //                logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Sayfa ID: {sayfaID}, Adet: {adet}");
        //                dataGridView2.Rows.Add(poz, sayfaID, adet);
        //            }
        //        }

        //        File.WriteAllLines(logPath, logLines);

        //        MessageBox.Show("Veriler DataGridView2'ye eklendi ve log dosyası masaüstüne kaydedildi!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Hata oluştu: {ex.Message}");
        //    }
        //}
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

                if (dataGridView2.Columns.Count == 0)
                {
                    dataGridView2.Columns.Add("Poz", "Poz");
                    dataGridView2.Columns.Add("SayfaID", "Sayfa ID");
                    dataGridView2.Columns.Add("Adet", "Adet");
                    dataGridView2.Columns.Add("Agirlik", "Ağırlık");
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

                var lines4 = richTextBox4.Lines;
                Dictionary<string, int> sayfaSayaclari = new Dictionary<string, int>();
                HashSet<string> validSayfaNos = new HashSet<string>();

                foreach (var line in lines4)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split(new[] { " (Sayfa: " }, StringSplitOptions.None);
                    if (parts.Length < 2) continue;

                    string parcaAdiFull = parts[0].Trim();
                    string sayfaNo = parts[1].Replace(")", "").Trim();

                    validSayfaNos.Add(sayfaNo);

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

                    string sayfaID = $"{id}-{sayfaNo.PadLeft(2, '0')}";
                    string parcaIDFinal = $"{sayfaID}-{sayfaSayaclari[sayfaNo].ToString("D2")}";

                    logLines.Add($"{parcaAdiFull} => Poz: {poz}, Parça ID: {parcaIDFinal}, Sayfa ID: {sayfaID}");
                }

                logLines.Add("\nRichTextBox1 Verileri:");
                sayfaSayaclari.Clear();
                var lines1 = richTextBox1.Lines;
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
                            agirlik = parts[3].Trim(); // Ağırlık genellikle 3. indekste
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

                    var match = Regex.Match(line, @"Poz: (.+?), Parça ID: (.+?), Sayfa ID: (.+)");
                    if (match.Success)
                    {
                        string poz = match.Groups[1].Value.Trim();
                        string parcaID = match.Groups[2].Value.Trim();
                        string sayfaID = match.Groups[3].Value.Trim();
                        rich4Dict[parcaID] = (poz, sayfaID);
                    }
                }

                foreach (var line in logLines)
                {
                    if (!line.Contains("=> Adet:")) continue;

                    var match = Regex.Match(line, @"Adet: (\d+), Ağırlık: ([\d.]+), Parça ID: (.+?), Sayfa ID");
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
                    var (poz, sayfaID) = kvp.Value;

                    if (rich1Dict.ContainsKey(parcaID))
                    {
                        var (adet, agirlik) = rich1Dict[parcaID];
                        logLines.Add($"Parça ID: {parcaID} => Poz: {poz}, Sayfa ID: {sayfaID}, Adet: {adet}, Ağırlık: {agirlik}");
                        dataGridView2.Rows.Add(poz, sayfaID, adet, agirlik);
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

                string text = richTextBox1.Text.Trim();
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

                Regex layoutRegex = new Regex(@"Yerleşim: (\d+) / \d+", RegexOptions.IgnoreCase);
                Regex kesmeRegex = new Regex(@"Kesme sayısı: (\d+)", RegexOptions.IgnoreCase);
                Regex partRegex = new Regex(@"(\d+)(?: - (\d+))?\s+.*?(\d+-\d+)-P(\d+)(?:-\d+AD)?", RegexOptions.IgnoreCase);
                Regex siralamaRegex = new Regex(@"^\s*Sıralama\s*$", RegexOptions.IgnoreCase);

                var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var validSayfaNos = new HashSet<string>();
                var sayfaSayaclari = new Dictionary<string, int>();
                var partInfoDict = new Dictionary<string, (string Poz, string SayfaID, int Count)>();
                string currentPage = "";
                string baseLayout = "";
                int continuationCount = 0;
                bool isPartSection = false;
                string tekrarSayisi = "0";

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var layoutMatch = layoutRegex.Match(line);
                    if (layoutMatch.Success)
                    {
                        currentPage = layoutMatch.Groups[1].Value;
                        baseLayout = currentPage;
                        continuationCount = 0;
                        isPartSection = false;
                        validSayfaNos.Add(currentPage);
                        logLines.Add($"Sayfa tespit edildi: {currentPage}, ID: {baseId}-{currentPage}");
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
                        string sayfaID = $"{baseId}-{currentPage.PadLeft(2, '0')}";
                        if (!sayfaSayaclari.ContainsKey(currentPage))
                        {
                            sayfaSayaclari[currentPage] = 0;
                        }
                        logLines.Add($"Sıralama bölümü başladı, sayfa: {currentPage}, ID: {sayfaID}, Tekrar: {tekrarSayisi}");
                        continue;
                    }

                    if (isPartSection && partRegex.IsMatch(line))
                    {
                        var partMatch = partRegex.Match(line);
                        if (partMatch.Success)
                        {
                            string startNum = partMatch.Groups[1].Value;
                            string endNum = partMatch.Groups[2].Value;
                            string partCode = partMatch.Groups[3].Value;
                            string pozNum = partMatch.Groups[4].Value;
                            string poz = $"{partCode}-P{pozNum}";
                            int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                            string currentLayout = continuationCount == 0 ? currentPage : $"{baseLayout}-{continuationCount}";
                            string sayfaID = $"{baseId}-{currentPage.PadLeft(2, '0')}";
                            int currentCount;
                            sayfaSayaclari.TryGetValue(currentPage, out currentCount);
                            sayfaSayaclari[currentPage] = currentCount + 1;
                            string parcaID = $"{sayfaID}-{sayfaSayaclari[currentPage].ToString("D2")}";

                            partInfoDict[parcaID] = (poz, sayfaID, count);
                            logLines.Add($"{poz}, Yerleşim: {currentLayout}, Adet: {count}, Parça ID: {parcaID}, Satır: {line}");
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
                            string partCode = partMatch.Groups[3].Value;
                            string pozNum = partMatch.Groups[4].Value;
                            string poz = $"{partCode}-P{pozNum}";
                            int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                            string sayfaID = $"{baseId}-{currentPage.PadLeft(2, '0')}";
                            int currentCount;
                            sayfaSayaclari.TryGetValue(currentPage, out currentCount);
                            sayfaSayaclari[currentPage] = currentCount + 1;
                            string parcaID = $"{sayfaID}-{sayfaSayaclari[currentPage].ToString("D2")}";

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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                btnEkle.Enabled = true;
                btnTumunuEkle.Enabled = true;
                btnKaydet.Enabled = true;
            }
            else
            {
                btnEkle.Enabled = false;
                btnTumunuEkle.Enabled = false;
                btnKaydet.Enabled = false;
            }
        }
      
        private void SayfaIDTabloVerileri()
        {
            dataGridView3.Rows.Clear();
            string metin = richTextBox1.Text;

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

                        Console.WriteLine($"Sayfa: {sayfaNo}, Levha Adet: {levhaAdet}");

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

        private void button2_Click(object sender, EventArgs e)
        {
            string islenmisVeri = IslenmisVeri();
            richTextBox4.Text = islenmisVeri;
            _veriOkuma.AjanOku(islenmisVeri);
        }
        public List<(string BirlesikVeri, string Agirlik)> EKIcerenIslenmisVeriler()
        {
            List<(string, string)> sonucListesi = new List<(string, string)>();

            string[] satirlar = richTextBox1.Text
                .Replace("\r\n", "\n")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            bool isFirmaAdiSection = false;
            int currentPageNumber = 1;
            int lastFirmaPageNumber = 1;

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

                if (satir.StartsWith("ToplamParçaKesmeListesi"))
                {
                    isFirmaAdiSection = false;
                    continue;
                }

                if (satir.StartsWith("FirmaAdı"))
                {
                    isFirmaAdiSection = true;
                    lastFirmaPageNumber = currentPageNumber;
                    continue;
                }

                if (!isFirmaAdiSection || !satir.StartsWith("ST"))
                    continue;

                string[] sutunlar = Regex.Split(satir, @"\t+|\s{2,}")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                string parcaAdi = sutunlar[0];
                if (!Regex.IsMatch(parcaAdi, @"^ST\d*-.*-.*-.*-.*-$"))
                    continue;

                string nextLine = i + 1 < satirlar.Length ? satirlar[i + 1].Trim() : "";

                if (string.IsNullOrWhiteSpace(nextLine))
                    continue;

                string[] sonrakiSutunlar = Regex.Split(nextLine, @"\t+|\s{2,}")
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToArray();

                string eklenecekVeri = sonrakiSutunlar.Length > 0 ? sonrakiSutunlar[0] : "";
                string birlesikVeri = $"{parcaAdi}{eklenecekVeri} (Sayfa: {lastFirmaPageNumber})";

                if (!birlesikVeri.Contains("EK"))
                    continue;

                // Ağırlık: Süre değerinden hemen önceki sütun
                string agirlik = "";
                for (int j = sutunlar.Length - 1; j >= 0; j--)
                {
                    if (Regex.IsMatch(sutunlar[j], @"^\d+:\d{2}(:\d{2})?$") && j > 0)
                    {
                        agirlik = sutunlar[j - 1];
                        break;
                    }
                }

                sonucListesi.Add((birlesikVeri, agirlik));
            }

            return sonucListesi;
        }





        private void button5_Click(object sender, EventArgs e)
        {
            var ekliSatirlar = EKIcerenIslenmisVeriler();

            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView4.Columns.Add("Satir", "EK İçeren Satır");

            foreach (var satir in ekliSatirlar)
            {
                dataGridView4.Rows.Add(satir);
            }

            // === Log dosyası oluşturma ===
            var gruplu = ekliSatirlar
                .GroupBy(x =>
                {
                    var match = Regex.Match(x.BirlesikVeri, @"ST\d+-\d+[Mm][Mm]-(\d+-\d+-P\d+)-\w+-(\d+\.\d+)");
                    if (match.Success)
                    {
                        string grupKey = $"{match.Groups[1].Value}-{match.Groups[2].Value}";
                        return grupKey;
                    }
                    return null;
                })
                .Where(g => g.Key != null)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x =>
                    {
                        var ekMatch = Regex.Match(x.BirlesikVeri, @"(EK\d+)");
                        return ekMatch.Success ? $"{ekMatch.Value}:{x.Agirlik}kg" : null;
                    }).Where(x => x != null).ToList()
                );

            try
            {
                richTextBox5.Clear();

                foreach (var kvp in gruplu)
                {
                    if (kvp.Value.Count > 0)
                    {
                        string ilkSatir = $"Poz: {kvp.Key}  Ek bilgisi : {kvp.Value[0]}";
                        richTextBox5.AppendText(ilkSatir + Environment.NewLine);

                        int boslukSayisi = ilkSatir.IndexOf(kvp.Value[0]);
                        string bosluk = new string(' ', boslukSayisi);

                        for (int i = 1; i < kvp.Value.Count; i++)
                        {
                            richTextBox5.AppendText(bosluk + kvp.Value[i] + Environment.NewLine);
                        }
                    }
                }

                MessageBox.Show($"log.txt başarıyla yazıldı:\n{richTextBox5}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }
        private void HesaplaEkAgirlikYuzdeleri()
        {
            try
            {
                var logLines = new List<string> {
            "=== EkAgirlikYuzdeLog.txt ===",
            $"İşlem Tarihi: {DateTime.Now}",
            "\nDataGridView2 Verileri Analizi:"
        };

                // Pozları gruplamak için dictionary
                var pozGroups = new Dictionary<string, List<(string Poz, string SayfaID, int Adet, double Agirlik)>>();
                Regex ekRegex = new Regex(@"-EK\d+$", RegexOptions.IgnoreCase);

                // DataGridView2'den verileri oku
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.IsNewRow) continue;

                    string poz = row.Cells[0].Value?.ToString() ?? "";
                    string sayfaID = row.Cells[1].Value?.ToString() ?? "";
                    string adetStr = row.Cells[2].Value?.ToString() ?? "0";
                    string agirlikStr = row.Cells[3].Value?.ToString() ?? "0";


                    if (!int.TryParse(adetStr, out int adet) || !double.TryParse(agirlikStr, out double agirlik))
                    {
                        logLines.Add($"Hata: Geçersiz adet veya ağırlık değeri - Poz: {poz}");
                        continue;
                    }

                    // EK ibaresini kaldırarak temel pozu bul
                    string basePoz = ekRegex.Replace(poz, "");

                    if (!pozGroups.ContainsKey(basePoz))
                    {
                        pozGroups[basePoz] = new List<(string Poz, string SayfaID, int Adet, double Agirlik)>();
                    }

                    pozGroups[basePoz].Add((poz, sayfaID, adet, agirlik));
                }

                logLines.Add("\nEK İbaresi İçeren Pozlar ve Yüzde Hesaplamaları:");
                foreach (var group in pozGroups)
                {
                    string basePoz = group.Key;
                    var items = group.Value;

                    // Sadece EK ibaresi içeren ve birden fazla satırı olan grupları işle
                    if (items.Any(item => ekRegex.IsMatch(item.Poz)) && items.Count > 1)
                    {
                        double toplamAgirlik = items.Sum(item => item.Agirlik);
                        logLines.Add($"\nPoz: {basePoz}, Toplam Ağırlık: {toplamAgirlik:F3}");

                        foreach (var item in items)
                        {
                            double yuzde = (item.Agirlik / toplamAgirlik) * 100;
                            logLines.Add($"  - Poz: {item.Poz}, Sayfa ID: {item.SayfaID}, Adet: {item.Adet}, Ağırlık: {item.Agirlik:F3}, Yüzde: {yuzde:F2}%");
                        }
                    }
                }

                // Log dosyasını masaüstüne kaydet
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logPath = Path.Combine(desktopPath, "EkAgirlikYuzdeLog.txt");
                File.WriteAllLines(logPath, logLines);

                MessageBox.Show("EK ibaresi içeren pozların ağırlık yüzdeleri hesaplandı ve log dosyası masaüstüne kaydedildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HesaplaEkAgirlikYuzdeleri();
        }
    }
}
