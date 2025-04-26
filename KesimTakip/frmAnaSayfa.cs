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

namespace KesimTakip
{
    public partial class frmAnaSayfa : Form
    {
        private readonly VeriOkuma _veriOkuma;
        private Button _seciliButon;

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
            // Büyük-küçük harf duyarsız kontrol için metni küçük harfe çevir
            string lowerText = pdfText.ToLower();

            // İlk sayfayı kontrol etmek için metni sınırlayalım
            string firstPageText = pdfText.Substring(0, Math.Min(2000, pdfText.Length)).ToLower();

            if (firstPageText.Contains("pronest"))
                return "Baykal";
            if (firstPageText.Contains("toplamparçakesmelistesi"))
                return "Ajan";
            if (firstPageText.Contains("firmaadı"))
                return "Ajan";
            if (firstPageText.Contains("nestings list"))
                return "Lantek";

            // Hata ayıklama için
            MessageBox.Show("PDF türü tespit edilemedi. İlk 500 karakter: " + firstPageText.Substring(0, Math.Min(500, firstPageText.Length)), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null; // Tespit edilemedi
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

            // Ortak UI temizleme işlemleri
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            lblKesimId.Enabled = false;
            txtKesimId.Enabled = false;
            dataGridView1.Rows.Clear();

            // PDF seçme işlemi
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

                        // PDF'yi yükle
                        await PdfYukle(filePath);

                        // PDF metnini oku
                        string pdfText = await PdfOku(filePath);

                        if (string.IsNullOrEmpty(pdfText))
                        {
                            MessageBox.Show("PDF metni okunamadı. PDF dosyasının içeriğini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        // PDF metnini RichTextBox1'e yaz
                        richTextBox1.Text = pdfText;

                        // İşlenmiş veriyi al ve RichTextBox4'e yaz
                        string islenmisVeri = IslenmisVeriyiAlAjan();
                        richTextBox4.Text = islenmisVeri;

                        // PDF türünü tespit et
                        string pdfTuru = TespitEtPdfTuru(pdfText);
                        if (string.IsNullOrEmpty(pdfTuru))
                        {
                            MessageBox.Show("PDF türü tespit edilemedi. Dosya içeriğini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        // Seçili butonla PDF türünü kontrol et
                        if (!ButonVePdfUyumluMu(pdfTuru))
                        {
                            MessageBox.Show($"Hatalı seçim: {pdfTuru} PDF'si yüklendi, ancak {_seciliButon.Text} butonu seçili.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }

                        // Seçili butona göre ilgili metodu çalıştır
                        (List<MalzemeBilgisi> parsedData, List<string> invalidData) = await Task.Run(() =>
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

                        if ((parsedData == null || parsedData.Count == 0) && (invalidData == null || invalidData.Count == 0))
                        {
                            MessageBox.Show("Hiçbir veri işlenemedi. PDF içeriğini richTextBox1 ve richTextBox4'te kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressBar1.Visible = false;
                            return;
                        }


                        // UI'yı güncelle
                        richTextBox2.Clear();
                        dataGridView1.Rows.Clear();

                        int totalItems = parsedData?.Count ?? 0;
                        int currentItem = 0;

                        if (parsedData != null)
                        {
                            foreach (var data in parsedData)
                            {
                                if (data != null)
                                {
                                    richTextBox2.AppendText($"{data.Kalite} - {data.Kalınlık} - {data.Kalıp} - {data.Poz} - {data.Adet} - {data.Proje}\n");
                                    dataGridView1.Rows.Add(data.Kalite, data.Kalınlık, data.Kalıp, data.Poz, data.Adet, data.Proje);

                                    currentItem++;
                                    int progressValue = totalItems > 0 ? (int)((currentItem / (float)totalItems) * 100) : 0;
                                    progressBar1.Value = progressValue;
                                }
                            }
                        }

                        if (invalidData != null && invalidData.Any())
                        {
                            foreach (var invalidLine in invalidData)
                            {
                                richTextBox3.AppendText($"{invalidLine}\n");
                            }
                            MessageBox.Show("Bazı veriler formata uymuyor ve richTextBox3'ye eklendi. Lütfen kontrol edin ve düzenleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        YerlestirmeTekrarSayisi(pdfText);
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
            if (_seciliButon == btnAjan)
            {
                string[] lines = richTextBox1.Lines;
                StringBuilder output = new StringBuilder();
                int i = 0;

                while (i < lines.Length)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrEmpty(line) || line.StartsWith("-------- Sayfa"))
                    {
                        output.AppendLine();
                        i++;
                        continue;
                    }

                    if (line.StartsWith("ST"))
                    {
                        output.Append(line);
                        i++;

                        if (i < lines.Length)
                        {
                            i++;
                            if (i < lines.Length)
                            {
                                string nextLine = lines[i].Trim();
                                if (!string.IsNullOrEmpty(nextLine) && !nextLine.StartsWith("-------- Sayfa"))
                                {
                                    output.Append("").Append(nextLine).AppendLine();
                                }
                                else
                                {
                                    output.AppendLine();
                                }
                                i++;
                            }
                            else
                            {
                                output.AppendLine();
                            }
                        }
                    }
                    else
                    {
                        output.AppendLine(line);
                        i++;
                    }
                }

                return output.ToString();
            }
            else
            {
                return string.Empty; 
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // richTextBox3'teki verileri satır satır al
                string[] lines = richTextBox3.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // richTextBox2'yi ve tabloyu temizle
                richTextBox2.Clear();
                dataGridView1.Rows.Clear();

                foreach (string line in lines)
                {
                    // '-' ile ayır ve trimle
                    string[] parts = line.Split('-').Select(p => p.Trim()).ToArray();

                    // Parça sayısı en az 6 ise işleme al
                    if (parts.Length >= 6)
                    {
                        string kalite = parts[0];
                        string kalinlik = parts[1];
                        string kalip = parts[2] + "-" + parts[3];
                        string poz = parts[4];
                        string adet = parts[5];
                        string proje = parts[6];

                        // richTextBox2'ye yaz
                        richTextBox2.AppendText($"{kalite} - {kalinlik} - {kalip} - {poz} - {adet} - {proje}\n");

                        // dataGridView1'e ekle
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
                            // LocationTextExtractionStrategy kullan
                            ITextExtractionStrategy strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy();
                            string text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);

                            // Sayfa içeriğini ekle
                            pageText.AppendLine(string.IsNullOrWhiteSpace(text) ? "[Bu sayfada metin yok]" : text);
                            pageText.AppendLine();

                            // ProgressBar güncelleme
                            int progressValue = (int)((i / (float)pageNumbers) * 100);
                            BeginInvoke((Action)(() => progressBar1.Value = progressValue));

                            // Task.Delay kaldırıldı, gerekirse ekleyebilirsiniz
                            // await Task.Delay(100);
                        }
                        catch (Exception pageEx)
                        {
                            pageText.AppendLine($"-------- Sayfa {i} ---------------");
                            pageText.AppendLine($"[Sayfa okunamadı: {pageEx.Message}]");
                            pageText.AppendLine();
                            // Hata mesajını anında göster
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

            // RichTextBox'a yazdır
            BeginInvoke((Action)(() => richTextBox1.Text = pageText.ToString()));

            return pageText.ToString();
        }


        private void UpdateTextBoxes()
        {
            HashSet<string> kaliteSet = new HashSet<string>();
            HashSet<string> kalinlikSet = new HashSet<string>();
            StringBuilder kalipBuilder = new StringBuilder();
            StringBuilder pozBuilder = new StringBuilder();
            StringBuilder adetBuilder = new StringBuilder();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    kaliteSet.Add(row.Cells[0].Value?.ToString());
                    kalinlikSet.Add(row.Cells[1].Value?.ToString());
                    kalipBuilder.Append(row.Cells[2].Value?.ToString() + ";");
                    pozBuilder.Append(row.Cells[3].Value?.ToString() + ";");
                    adetBuilder.Append(row.Cells[4].Value?.ToString() + ";");
                }
            }

            txtKalite.Text = string.Join(";", kaliteSet);
            txtKalinlik.Text = string.Join(";", kalinlikSet);
            txtKalipNo.Text = kalipBuilder.ToString().TrimEnd(';');
            txtKesilenPozlar.Text = pozBuilder.ToString().TrimEnd(';');
            txtKPAdet.Text = adetBuilder.ToString().TrimEnd(';');
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

                KesimListesiPaketData.SaveKesimDataPaket(olusturan, kesimid, kesilecekPlanSayisi, toplamPlanTekrari, eklemeTekrari);
                KesimListesiData.SaveKesimData(olusturan, kesimid, projeNo, kalinlik, kalite, kaliplar, pozlar, adetler, eklemeTekrari);

                MessageBox.Show("Kesimler Başarıyla Kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void YerlestirmeTekrarSayisi(string pdfText)
        {
            if (pdfText.Contains("ProNest"))
            {
                string kesmeSayisiPattern = @"Kesme sayısı:\s*(\d+)";
                Regex kesmeSayisiRegex = new Regex(kesmeSayisiPattern);
                Match kesmeSayisiMatch = kesmeSayisiRegex.Match(pdfText);

                if (kesmeSayisiMatch.Success)
                {
                    string kesmeSayisi = kesmeSayisiMatch.Groups[1].Value;
                    Invoke((Action)(() => txtKesimPlaniTekrarSayisi.Text = kesmeSayisi));
                    return;
                }
            }
            else if (pdfText.Contains("FirmaAdı"))
            {
                string tekrarPattern = @"ProgramTekrarı\s+(.*?)\s+Kalınlık";
                Regex tekrarRegex = new Regex(tekrarPattern);
                Match tekrarMatch = tekrarRegex.Match(pdfText);

                if (tekrarMatch.Success)
                {
                    string[] kelimeler = tekrarMatch.Groups[1].Value.Split(' ');
                    string tekrarSayisi = kelimeler.Reverse().FirstOrDefault(k => int.TryParse(k, out _));

                    if (!string.IsNullOrEmpty(tekrarSayisi))
                    {
                        Invoke((Action)(() => txtKesimPlaniTekrarSayisi.Text = tekrarSayisi));
                    }
                }
            }
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
    }
}
