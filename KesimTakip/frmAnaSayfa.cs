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
using ProNestParser;
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

                        string pdfText = await PdfOku(filePath);
                        string pdfTextYeni = await PdfOkuAjan(filePath);


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
                        richTextBox4.Text = pdfTextYeni;

                        (List<MalzemeBilgisi> validData, List<string> InvalidData) = await Task.Run(() =>
                        {
                            try
                            {
                                if (_seciliButon == btnAjan)
                                    return _veriOkuma.AjanOku(pdfTextYeni);
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
                        if (_seciliButon ==btnBaykal )
                        {
                            BaykalTekSayfaDetay();
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

        private string IslenmisVeriyiAlAjan()
        {
            if (_seciliButon != btnAjan)
                return string.Empty;

            string[] lines = richTextBox1.Lines;
            StringBuilder output = new StringBuilder();
            bool isPartSection = false; // Parça bölümünde miyiz?

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                // Parça bölümü başlıyor
                if (line == "Parça")
                {
                    isPartSection = true;
                    output.AppendLine(line);
                    continue;
                }

                // Parça bölümü bitti
                if (isPartSection && line.StartsWith("Toplam"))
                {
                    isPartSection = false;
                    output.AppendLine(line);
                    continue;
                }

                // Boş satır, sayfa ayırıcı veya diğer satırları olduğu gibi ekle
                if (!isPartSection || string.IsNullOrEmpty(line) || line.StartsWith("-------- Sayfa") || line.StartsWith("Sayfa:") || line == "Poz" || line == "No" || line == "(Kg)")
                {
                    output.AppendLine(line);
                    continue;
                }

                // ST ile başlayan satır (Parça bölümü içindeyiz)
                if (isPartSection && line.StartsWith("ST"))
                {
                    string partName = line; // Parça adı (örneğin, ST37-2MM-178-20-P30-)

                    // Sonraki satırları kontrol et, Not1 satırını bul
                    i++;
                    bool foundNot1 = false;
                    string extraInfo = string.Empty; // Ø14 gibi ek bilgiler

                    while (i < lines.Length && !foundNot1)
                    {
                        string nextLine = lines[i].Trim();

                        // Ölçü satırı (sayısal veri içerir, örneğin, 1 1945X412 ...)
                        if (Regex.IsMatch(nextLine, @"^\d+\s+\d+X\d+"))
                        {
                            i++; // Ölçü satırını atla
                            continue;
                        }

                        // Not1 satırı: AD- içerir veya BKM ile biter
                        if (nextLine.Contains("AD-") || nextLine.EndsWith("BKM"))
                        {
                            string not1Value = nextLine;

                            // Ø14 gibi ek bilgi varsa, ayır
                            if (not1Value.Contains("Ø"))
                            {
                                var parts = not1Value.Split(new[] { "Ø" }, StringSplitOptions.None);
                                not1Value = parts[0].Trim();
                                extraInfo = "Ø" + parts[1].Trim();
                            }

                            // ST ve Not1 satırlarını birleştir
                            output.AppendLine($"{partName}{not1Value}");
                            if (!string.IsNullOrEmpty(extraInfo))
                            {
                                output.AppendLine(extraInfo);
                            }
                            foundNot1 = true;
                        }

                        i++;
                    }

                    // Not1 satırı bulunamadıysa, geri sar
                    if (!foundNot1)
                    {
                        i--;
                    }
                }
            }

            return output.ToString();
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
       
        public async Task<string> PdfOku(string pdfpath)
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

                    for (int i = 1; i <= pageCount; i++)
                    {
                        pageText.AppendLine($"\n-------- Sayfa {i} --------");

                        try
                        {
                            var strategy = new LocationTextExtractionStrategy();
                            var text = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);

                            if (string.IsNullOrWhiteSpace(text))
                            {
                                pageText.AppendLine("[Bu sayfada metin yok]");
                                continue;
                            }

                            var lines = text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
                            string bufferLine = null;

                            for (int j = 0; j < lines.Count; j++)
                            {
                                var currentLine = Regex.Replace(lines[j].Trim(), @"\s{2,}", " ");

                                if (bufferLine == null)
                                {
                                    bufferLine = currentLine;
                                    continue;
                                }

                                // Eğer önceki satır ürün kodu gibi alfanumerikse ve bu satır sayılarla başlıyorsa, birleştir
                                if (Regex.IsMatch(currentLine, @"^\d") && Regex.IsMatch(bufferLine, @"[A-Z0-9\-]{5,}$"))
                                {
                                    var combined = bufferLine + " " + currentLine;
                                    var columns = combined.Split(' ');
                                    pageText.AppendLine(string.Join("\t", columns));
                                    bufferLine = null;
                                }
                                else
                                {
                                    // Önceki satırı yazdır, bunu sonraya al
                                    var prevColumns = bufferLine.Split(' ');
                                    pageText.AppendLine(string.Join("\t", prevColumns));
                                    bufferLine = currentLine;
                                }
                            }

                            // Son satır kaldıysa ekle
                            if (!string.IsNullOrEmpty(bufferLine))
                            {
                                var lastColumns = bufferLine.Split(' ');
                                pageText.AppendLine(string.Join("\t", lastColumns));
                            }

                            await Task.Delay(30);
                        }
                        catch (Exception exPage)
                        {
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
                KesimListesiPaketData.SaveKesimDataPaket(olusturan, kesimid, kesilecekPlanSayisi, toplamPlanTekrari, eklemeTekrari);
                KesimListesiData.SaveKesimData(currentId.Value, olusturan, kesimid, projeNo, kalinlik, kalite, kaliplar, pozlar, adetler, eklemeTekrari);

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
                sfd.FileName = "kesimlistesi.xml";

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
                writer.WriteStartElement("KesimListesi");

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].IsNewRow) continue;

                    writer.WriteStartElement("Parca");

                    string kaliteDegeri = dgv.Rows[i].Cells[0].Value?.ToString();
                    string kaliteXmlDegeri = kaliteDegeri;
                    if (kaliteDegeri == "ST37")
                        kaliteXmlDegeri = "S235JR";
                    else if (kaliteDegeri == "ST52")
                        kaliteXmlDegeri = "S355J2";

                    writer.WriteElementString("Kalite", kaliteXmlDegeri);

                    writer.WriteElementString("Kalınlık", dgv.Rows[i].Cells[1].Value?.ToString());

                    writer.WriteElementString("Kalıp", dgv.Rows[i].Cells[2].Value?.ToString());

                    string pozDegeri = dgv.Rows[i].Cells[3].Value?.ToString();
                    string pozXmlDegeri = pozDegeri;
                    if (!string.IsNullOrEmpty(pozDegeri) && pozDegeri.StartsWith("P") && pozDegeri.Length == 2)
                    {
                        pozXmlDegeri = "P0" + pozDegeri.Substring(1);
                    }
                    writer.WriteElementString("Poz", pozXmlDegeri);

                    writer.WriteElementString("Adet", dgv.Rows[i].Cells[4].Value?.ToString());

                    writer.WriteElementString("Proje", dgv.Rows[i].Cells[5].Value?.ToString());

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
            BaykalTekSayfaDetay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var parser = new AjanParser();
            parser.AjanTekSayfaDetay(richTextBox1, richTextBox4, txtId, dataGridView2, dataGridView3);
        
        }
        public void BaykalTekSayfaDetay() 
        {
            try
            {
                var parser = new VeriOkuma.ProNestParser();

                var (partInfoList, pageInfoList, logFilePath) = parser.ParseProNestOutput(richTextBox1.Text, txtId.Text.Trim());

                var uniquePozs = partInfoList.Select(p => p.Poz).Distinct().OrderBy(p => p).ToList();
                foreach (var poz in uniquePozs)
                {
                    var layouts = partInfoList.Where(p => p.Poz == poz).Select(p => p.Layout).Distinct().OrderBy(l => l).ToList();
                    foreach (var layout in layouts)
                    {
                        int totalCount = partInfoList.Where(p => p.Poz == poz && p.Layout == layout).Sum(p => p.Count);
                        dataGridView2.Rows.Add(poz, layout, totalCount);
                    }
                }

                foreach (var page in pageInfoList)
                {
                    dataGridView3.Rows.Add(page.Id, page.YerlesimSayisi, page.TekrarSayisi);
                }

                MessageBox.Show($"Veri işleme tamamlandı. Log dosyası: {logFilePath}");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PDF Dosyaları|*.pdf";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;


            foreach (var pdfPath in openFileDialog1.FileNames)
            {
                richTextBox5.Clear();
                richTextBox5.AppendText($"===== {System.IO.Path.GetFileName(pdfPath)} =====\n");

                using (var reader = new PdfReader(pdfPath))
                using (var pdfDoc = new PdfDocument(reader))
                {
                    List<string[]> allRows = new List<string[]>();
                    int maxColumns = 0;

                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        var strategy = new SimpleTextExtractionStrategy();
                        var text = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), strategy);
                        var lines = text.Split('\n');

                        foreach (var line in lines)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            // Tüm boşluklara göre sütun ayır
                            var columns = Regex.Split(line.Trim(), @"\s+");
                            allRows.Add(columns);

                            if (columns.Length > maxColumns)
                                maxColumns = columns.Length;
                        }
                    }

                    // Her satırı eşit sütun sayısına getir, sonra yazdır
                    foreach (var row in allRows)
                    {
                        var filled = row.Concat(Enumerable.Repeat("", maxColumns - row.Length));
                        richTextBox5.AppendText(string.Join("\t", filled) + "\n");
                    }

                    richTextBox5.AppendText("\n");
                }
            }
        }
    }
}

