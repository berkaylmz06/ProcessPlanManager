using CEKA_APP.Helper;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;

namespace CEKA_APP.Forms
{
    public partial class frmFaturaOlustur : Form
    {
        private Dictionary<string, string> configValues;
        private string tutar;
        private string aciklama;
        private string tarih;
        private string musteriAdi;
        private int notSayisi = 0;
        private List<RichTextBox> notRichTextBoxes = new List<RichTextBox>();
        private List<Label> notLabels = new List<Label>();
        private List<Button> notKapatButonlari = new List<Button>();
        private Panel notPanel;

        public frmFaturaOlustur(string tutar, string aciklama, string tarih, string musteriAdi)
        {
            this.tutar = tutar ?? "0";
            this.aciklama = aciklama ?? "";
            this.tarih = tarih ?? "Belirtilmemiş";
            this.musteriAdi = musteriAdi ?? "Bilinmiyor";
            InitializeComponent();

            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new PlatformFontResolver();

            ReadConfigFile();
        }

        private void frmFaturaOlustur_Load(object sender, EventArgs e)
        {
            txtTutar.Text = tutar;
            txtAciklama.Text = aciklama;
            txtTarih.Text = tarih;

            this.Icon = new Icon("cekalogokirmizi.ico");

            var btnNotEkle = this.Controls.Find("btnNotEkle", true).FirstOrDefault() as Button;
            int baseX = btnNotEkle != null ? btnNotEkle.Left : 10;
            int baseY = btnNotEkle != null ? btnNotEkle.Bottom + 10 : 620;
            notPanel = new Panel
            {
                Location = new Point(baseX, baseY),
                Size = new Size(330, 200),
                AutoScroll = true
            };
            this.Controls.Add(notPanel);
        }

        private void btnNotEkle_Click(object sender, EventArgs e)
        {
            notSayisi++;
            int notIndex = notSayisi;

            int baseTop = notPanel.Controls.Count > 0 ?
                notRichTextBoxes.Last().Bottom + 10 : 10;

            var notLabel = new Label
            {
                Text = $"Not {notIndex}:",
                Width = 100,
                Top = baseTop,
                Left = 0
            };
            notPanel.Controls.Add(notLabel);
            notLabels.Add(notLabel);

            var notRichTextBox = new RichTextBox
            {
                Name = $"richTextBoxNot{notIndex}",
                Width = 300,
                Height = 60,
                Top = baseTop + 20,
                Left = 0
            };
            notPanel.Controls.Add(notRichTextBox);
            notRichTextBoxes.Add(notRichTextBox);

            var kapatButonu = new Button
            {
                Text = "X",
                Width = 20,
                Height = 20,
                Top = baseTop,
                Left = 110,
                Tag = notIndex
            };
            kapatButonu.Click += KapatButonu_Click;
            notPanel.Controls.Add(kapatButonu);
            notKapatButonlari.Add(kapatButonu);
        }

        private void KapatButonu_Click(object sender, EventArgs e)
        {
            var buton = sender as Button;
            int notIndex = (int)buton.Tag;

            var notLabel = notLabels.FirstOrDefault(l => l.Text == $"Not {notIndex}:");
            var notRichTextBox = notRichTextBoxes.FirstOrDefault(r => r.Name == $"richTextBoxNot{notIndex}");
            var kapatButonu = notKapatButonlari.FirstOrDefault(b => (int)b.Tag == notIndex);

            if (notLabel != null)
            {
                notPanel.Controls.Remove(notLabel);
                notLabels.Remove(notLabel);
                notLabel.Dispose();
            }

            if (notRichTextBox != null)
            {
                notPanel.Controls.Remove(notRichTextBox);
                notRichTextBoxes.Remove(notRichTextBox);
                notRichTextBox.Dispose();
            }

            if (kapatButonu != null)
            {
                notPanel.Controls.Remove(kapatButonu);
                notKapatButonlari.Remove(kapatButonu);
                kapatButonu.Dispose();
            }

            notSayisi--;

            if (notLabels.Count > 0)
            {
                int baseTop = 10;
                for (int i = 0; i < notLabels.Count; i++)
                {
                    int yeniIndex = i + 1;
                    notLabels[i].Text = $"Not {yeniIndex}:";
                    notLabels[i].Top = baseTop + (i * 80);
                    notLabels[i].Left = 0;
                    notRichTextBoxes[i].Name = $"richTextBoxNot{yeniIndex}";
                    notRichTextBoxes[i].Top = baseTop + 20 + (i * 80);
                    notRichTextBoxes[i].Left = 0;
                    notKapatButonlari[i].Top = baseTop + (i * 80);
                    notKapatButonlari[i].Left = 110;
                    notKapatButonlari[i].Tag = yeniIndex;
                }
            }
        }

        private void chkTurkish_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                var chkEnglish = this.Controls.Find("chkEnglish", true).FirstOrDefault() as CheckBox;
                if (chkEnglish != null)
                    chkEnglish.Checked = false;
            }
        }

        private void chkEnglish_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                var chkTurkish = this.Controls.Find("chkTurkish", true).FirstOrDefault() as CheckBox;
                if (chkTurkish != null)
                    chkTurkish.Checked = false;
            }
        }

        private void ReadConfigFile()
        {
            configValues = new Dictionary<string, string>();
            string configFilePath = @"\\192.168.2.3\proje\CEKA APP\config.txt";

            if (File.Exists(configFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(configFilePath);
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                            continue;

                        int equalsIndex = trimmedLine.IndexOf('=');
                        if (equalsIndex > 0)
                        {
                            string key = trimmedLine.Substring(0, equalsIndex).Trim();
                            string value = trimmedLine.Substring(equalsIndex + 1).Trim();
                            configValues[key] = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Config dosyası okunurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("config.txt belirtilen yolda bulunamadı. Varsayılan değerler kullanılıyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private double MeasureTextHeightWithFormatter(XGraphics gfx, string text, XFont font, double desiredWidth)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            string[] paragraphs = text.Replace("\r\n", "\n").Split('\n');
            double totalHeight = 0;
            double lineHeight = font.GetHeight();

            foreach (string paragraph in paragraphs)
            {
                if (string.IsNullOrEmpty(paragraph.Trim()))
                {
                    totalHeight += lineHeight;
                    continue;
                }

                string[] words = paragraph.Split(' ');
                var currentLine = new System.Text.StringBuilder();

                foreach (string word in words)
                {
                    if (string.IsNullOrEmpty(word)) continue;

                    string testLine = currentLine.ToString();
                    if (testLine.Length > 0) testLine += " ";
                    testLine += word;

                    if (gfx.MeasureString(testLine, font).Width > desiredWidth)
                    {
                        totalHeight += lineHeight;
                        currentLine.Clear();
                        currentLine.Append(word);
                    }
                    else
                    {
                        if (currentLine.Length > 0) currentLine.Append(" ");
                        currentLine.Append(word);
                    }
                }

                totalHeight += lineHeight;
            }

            return totalHeight;
        }

        private string ForceWordWrap(XGraphics gfx, string text, XFont font, double maxWidth)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var result = new System.Text.StringBuilder();
            string[] words = text.Split(' ');

            double currentLineLength = 0;
            double spaceWidth = gfx.MeasureString(" ", font).Width;

            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    if (currentLineLength + spaceWidth > maxWidth)
                    {
                        result.Append(Environment.NewLine);
                        currentLineLength = 0;
                    }
                    else
                    {
                        result.Append(" ");
                        currentLineLength += spaceWidth;
                    }
                    continue;
                }

                double wordWidth = gfx.MeasureString(word, font).Width;

                if (currentLineLength + wordWidth + (currentLineLength > 0 ? spaceWidth : 0) > maxWidth)
                {
                    // Word itself is too long for a single line, or it causes overflow
                    if (wordWidth > maxWidth)
                    {
                        var currentSegment = new System.Text.StringBuilder();
                        foreach (char c in word)
                        {
                            currentSegment.Append(c);
                            if (gfx.MeasureString(currentSegment.ToString(), font).Width > maxWidth)
                            {
                                result.Append(Environment.NewLine);
                                result.Append(currentSegment.ToString(0, currentSegment.Length - 1));
                                result.Append(Environment.NewLine);
                                currentSegment.Clear();
                                currentSegment.Append(c);
                                currentLineLength = gfx.MeasureString(currentSegment.ToString(), font).Width;
                            }
                        }
                        if (currentSegment.Length > 0)
                        {
                            if (currentLineLength + gfx.MeasureString(currentSegment.ToString(), font).Width > maxWidth && currentLineLength > 0)
                            {
                                result.Append(Environment.NewLine);
                                result.Append(currentSegment.ToString());
                                currentLineLength = gfx.MeasureString(currentSegment.ToString(), font).Width;
                            }
                            else
                            {
                                result.Append(currentSegment.ToString());
                                currentLineLength += gfx.MeasureString(currentSegment.ToString(), font).Width;
                            }
                        }
                    }
                    else // Word fits on a line, but not with current line content
                    {
                        result.Append(Environment.NewLine);
                        result.Append(word);
                        currentLineLength = wordWidth;
                    }
                }
                else
                {
                    if (currentLineLength > 0)
                    {
                        result.Append(" ");
                        currentLineLength += spaceWidth;
                    }
                    result.Append(word);
                    currentLineLength += wordWidth;
                }
            }
            return result.ToString().Trim();
        }


        private double MeasureTextWidth(XGraphics gfx, string text, XFont font)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
            return gfx.MeasureString(text, font).Width;
        }

        private string SayiyiYaziyaCevir(string tutar, string dil)
        {
            string[] birler, onlar, binler;
            string currencyMain, currencySub, hundredWord, zeroWord, andWord;

            if (dil == "tr")
            {
                birler = new string[] { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };
                onlar = new string[] { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan" };
                binler = new string[] { "", "Bin", "Milyon", "Milyar", "Trilyon", "Katrilyon" };
                currencyMain = "Lira";
                currencySub = "Kuruş";
                hundredWord = "Yüz";
                zeroWord = "Sıfır";
                andWord = "";
            }
            else
            {
                birler = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
                onlar = new string[] { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                binler = new string[] { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion" };
                currencyMain = "Dollars";
                currencySub = "Cents";
                hundredWord = "Hundred";
                zeroWord = "Zero";
                andWord = "and";
            }

            tutar = tutar.Replace('.', ',');

            string tamKisimStr = "";
            string ondalikKisimStr = "";

            if (tutar.Contains(','))
            {
                string[] parcalar = tutar.Split(',');
                tamKisimStr = parcalar[0];
                ondalikKisimStr = (parcalar.Length > 1 ? parcalar[1] : "").PadRight(2, '0').Substring(0, 2);
            }
            else
            {
                tamKisimStr = tutar;
            }

            if (string.IsNullOrEmpty(tamKisimStr)) tamKisimStr = "0";

            if (tamKisimStr.Length > 18)
            {
                MessageBox.Show(dil == "tr" ? "Sayı katrilyondan büyük olamaz." : "Number cannot be larger than quadrillion.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "HATA: Sayı çok büyük!";
            }

            string sonuc = "";

            if (tamKisimStr == "0")
            {
                sonuc = zeroWord;
            }
            else
            {
                sonuc = ConvertNumberGroup(tamKisimStr, dil, birler, onlar, binler, hundredWord);
            }

            sonuc += " " + currencyMain;

            if (!string.IsNullOrEmpty(ondalikKisimStr) && int.Parse(ondalikKisimStr) > 0)
            {
                if (dil == "en" && tamKisimStr != "0")
                {
                    sonuc += " " + andWord;
                }

                string ondalikMetin = ConvertNumberGroup(ondalikKisimStr.TrimStart('0'), dil, birler, onlar, binler, hundredWord);
                sonuc += " " + ondalikMetin + " " + currencySub;
            }

            return sonuc.Trim();
        }

        private string ConvertNumberGroup(string number, string dil, string[] birler, string[] onlar, string[] binler, string hundredWord)
        {
            if (string.IsNullOrEmpty(number) || long.Parse(number) == 0) return "";

            number = number.PadLeft(((number.Length - 1) / 3 + 1) * 3, '0');
            string sonuc = "";

            for (int i = 0; i < number.Length; i += 3)
            {
                string grup = number.Substring(i, 3);
                if (grup == "000") continue;

                string grupMetin = "";
                int yuzler = int.Parse(grup[0].ToString());
                int onlarBas = int.Parse(grup[1].ToString());
                int birlerBas = int.Parse(grup[2].ToString());

                if (yuzler > 0)
                {
                    if (dil == "tr")
                    {
                        if (yuzler > 1) grupMetin += birler[yuzler] + " ";
                        grupMetin += hundredWord + " ";
                    }
                    else
                    {
                        grupMetin += birler[yuzler] + " " + hundredWord + " ";
                    }
                }

                if (dil == "en" && onlarBas == 1)
                {
                    switch (birlerBas)
                    {
                        case 0: grupMetin += "Ten "; break;
                        case 1: grupMetin += "Eleven "; break;
                        case 2: grupMetin += "Twelve "; break;
                        case 3: grupMetin += "Thirteen "; break;
                        case 4: grupMetin += "Fourteen "; break;
                        case 5: grupMetin += "Fifteen "; break;
                        case 6: grupMetin += "Sixteen "; break;
                        case 7: grupMetin += "Seventeen "; break;
                        case 8: grupMetin += "Eighteen "; break;
                        case 9: grupMetin += "Nineteen "; break;
                    }
                }
                else
                {
                    if (onlarBas > 0)
                    {
                        grupMetin += onlar[onlarBas];
                        if (dil == "en" && birlerBas > 0) grupMetin += "-"; else grupMetin += " ";
                    }

                    if (birlerBas > 0)
                    {
                        grupMetin += birler[birlerBas] + " ";
                    }
                }

                grupMetin = grupMetin.Trim();

                int grupIndex = (number.Length - i - 1) / 3;
                if (grupIndex > 0)
                {
                    if (dil == "tr" && grupMetin == "Bir" && binler[grupIndex] == "Bin")
                    {
                        grupMetin = binler[grupIndex];
                    }
                    else if (!string.IsNullOrEmpty(grupMetin))
                    {
                        grupMetin += " " + binler[grupIndex];
                    }
                }

                sonuc += grupMetin.Trim() + " ";
            }

            return System.Text.RegularExpressions.Regex.Replace(sonuc.Trim(), @"\s+", " ");
        }

        private string GenerateInvoiceNumber(string projeNo)
        {
            if (string.IsNullOrEmpty(projeNo))
                return "N/A";

            string yearLastTwo = DateTime.Now.Year.ToString().Substring(2);
            string baseInvoiceNumber = $"{yearLastTwo}P{projeNo}_";

            // Config dosyasından invoiceCounter değerini oku
            int lastCounter = 0;
            string configFilePath = @"\\192.168.2.3\proje\CEKA APP\config.txt";

            if (configValues.TryGetValue("invoiceCounter", out string counterValue) && int.TryParse(counterValue, out int parsedCounter))
            {
                lastCounter = parsedCounter;
            }

            int counter = lastCounter + 1; // Her PDF oluşturmada 1 artır
            string finalInvoiceNumber = $"{baseInvoiceNumber}{counter:D3}";

            // Dosyayı kilitleyip güncelle
            try
            {
                using (var fileStream = new FileStream(configFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    fileStream.Lock(0, fileStream.Length); // Dosyayı kilitle

                    var updatedConfig = new Dictionary<string, string>(configValues);
                    updatedConfig["invoiceCounter"] = counter.ToString();

                    streamWriter.BaseStream.Seek(0, SeekOrigin.Begin); // Dosyanın başına git
                    streamWriter.Write(string.Join(Environment.NewLine, updatedConfig.Select(kv => $"{kv.Key}={kv.Value}")));
                    streamWriter.Flush();
                    fileStream.SetLength(fileStream.Position); // Fazlalıkları temizle
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Config dosyasına erişim sırasında hata oluştu: {ex.Message}. Varsayılan sayaç kullanılacak.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Config dosyasına yazma izni yok: {ex.Message}. Lütfen ağ izinlerini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return $"{baseInvoiceNumber}001"; // İzin yoksa varsayılan bir değer döndür
            }

            // Invoice klasöründe dosya çakışmasını kontrol et
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string invoiceFolderPath = Path.Combine(desktopPath, "Invoice");
            if (!Directory.Exists(invoiceFolderPath))
            {
                Directory.CreateDirectory(invoiceFolderPath);
            }

            string fullPath = Path.Combine(invoiceFolderPath, $"{finalInvoiceNumber}.pdf");
            while (File.Exists(fullPath))
            {
                counter++;
                finalInvoiceNumber = $"{baseInvoiceNumber}{counter:D3}";
                fullPath = Path.Combine(invoiceFolderPath, $"{finalInvoiceNumber}.pdf");
            }

            // Eğer counter değiştiyse, config'i tekrar güncelle
            if (counter > lastCounter)
            {
                try
                {
                    using (var fileStream = new FileStream(configFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        fileStream.Lock(0, fileStream.Length);
                        var updatedConfig = new Dictionary<string, string>(configValues);
                        updatedConfig["invoiceCounter"] = counter.ToString();
                        streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                        streamWriter.Write(string.Join(Environment.NewLine, updatedConfig.Select(kv => $"{kv.Key}={kv.Value}")));
                        streamWriter.Flush();
                        fileStream.SetLength(fileStream.Position);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Config dosyası güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return finalInvoiceNumber;
        }

        private void btnPdfOlustur_Click(object sender, EventArgs e)
        {
            var chkTurkish = this.Controls.Find("chkTurkish", true).FirstOrDefault() as CheckBox;
            var chkEnglish = this.Controls.Find("chkEnglish", true).FirstOrDefault() as CheckBox;

            if (chkTurkish == null || chkEnglish == null || (!chkTurkish.Checked && !chkEnglish.Checked))
            {
                MessageBox.Show("Lütfen bir dil seçin (Türkçe veya İngilizce).", "Dil Seçimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Invoice Example";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            XFont fontNormal = new XFont("Arial", 9, XFontStyleEx.Regular);
            XFont fontBold = new XFont("Arial", 12, XFontStyleEx.Regular);
            XFont fontInvoice = new XFont("Arial", 24, XFontStyleEx.Regular);
            XFont dynamicFont = new XFont("Arial", 7, XFontStyleEx.Regular);
            XFont panelFont = new XFont("Arial", 8, XFontStyleEx.Regular);
            XFont boldHeaderFont = new XFont("Arial", 10, XFontStyleEx.Regular);
            XFont labelFont = new XFont("Arial", 6, XFontStyleEx.Regular);
            XFont infoLabelFont = new XFont("Arial", 7, XFontStyleEx.Regular);
            XFont largeLabelFont = new XFont("Arial", 8, XFontStyleEx.Regular);

            double pageWidth = page.Width;
            double pageHeight = page.Height;
            double margin = 40;
            double yPosition = margin;

            XRect logoRect = new XRect(margin, yPosition, 100, 60);
            string logoPath = configValues.TryGetValue("logoPath", out string path) ? path : null;
            XImage logoImage = null;

            if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
            {
                try
                {
                    logoImage = XImage.FromFile(logoPath);
                    double imageAspectRatio = (double)logoImage.PixelWidth / logoImage.PixelHeight;
                    double rectAspectRatio = logoRect.Width / logoRect.Height;

                    if (imageAspectRatio > rectAspectRatio)
                    {
                        logoRect.Height = logoRect.Width / imageAspectRatio;
                        logoRect.Y = yPosition + (60 - logoRect.Height) / 2;
                    }
                    else
                    {
                        logoRect.Width = logoRect.Height * imageAspectRatio;
                        logoRect.X = margin + (100 - logoRect.Width) / 2;
                    }
                    gfx.DrawImage(logoImage, logoRect);
                }
                catch (Exception ex)
                {
                    gfx.DrawRectangle(XPens.Black, logoRect);
                    gfx.DrawString("LOGO", fontBold, XBrushes.Gray,
                        new XRect(logoRect.X, logoRect.Y + 15, logoRect.Width, logoRect.Height),
                        XStringFormats.Center);
                    MessageBox.Show($"Logo yüklenirken hata oluştu: {ex.Message}. Varsayılan logo kullanılıyor.", "Logo Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                gfx.DrawRectangle(XPens.Black, logoRect);
                gfx.DrawString("LOGO", fontBold, XBrushes.Gray,
                    new XRect(logoRect.X, logoRect.Y + 15, logoRect.Width, logoRect.Height),
                    XStringFormats.Center);
            }

            string invoiceText = "INVOICE";
            XSize invoiceSize = gfx.MeasureString(invoiceText, fontInvoice);
            double invoiceX = pageWidth - margin - invoiceSize.Width;
            double rightBlockStartX = invoiceX - (1 * 72 / 2.54);

            double leftBoxesX = margin;
            double leftBoxesYStart = logoRect.Bottom + 10;
            double lineHeight = dynamicFont.Height;
            double lineSpacing = 2;

            double maxBlockWidth = rightBlockStartX - leftBoxesX;
            double minCellWidth = 20;
            double cellPadding = 5;

            string textMainBlock1Info = configValues.TryGetValue("textBoxMainBlock1Info", out string val1) ? val1 : "Bu, ana bloğun ilk satırı için örnek uzun bir metindir.";
            string textMainBlock2Info = configValues.TryGetValue("textBoxMainBlock2Info", out string val2) ? val2 : "Bu da ikinci satır için örnek bir metin.";

            string text3_1 = configValues.TryGetValue("textBox3_1", out string val3_1) ? val3_1 : "Metin 3.1";
            string text3_2 = configValues.TryGetValue("textBox3_2", out string val3_2) ? val3_2 : "Metin 3.2";
            string text3_3 = configValues.TryGetValue("textBox3_3", out string val3_3) ? val3_3 : "Metin 3.3";
            string text3_4 = configValues.TryGetValue("textBox3_4", out string val3_4) ? val3_4 : "Metin 3.4";

            string text4_1 = configValues.TryGetValue("textBox4_1", out string val4_1) ? val4_1 : "Metin 4.1";
            string text4_2 = configValues.TryGetValue("textBox4_2", out string val4_2) ? val4_2 : "Metin 4.2";
            string text4_3 = configValues.TryGetValue("textBox4_3", out string val4_3) ? val4_3 : "Metin 4.3";
            string text4_4 = configValues.TryGetValue("textBox4_4", out string val4_4) ? val4_4 : "Metin 4.4";

            string textOnly;
            string singleAmountValue = txtTutar?.Text ?? "0.00";

            if (string.IsNullOrEmpty(singleAmountValue) || !double.TryParse(singleAmountValue.Replace(',', '.'), out _))
            {
                MessageBox.Show("Lütfen geçerli bir sayı girin.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textOnly = "N/A";
            }
            else if (chkTurkish.Checked)
            {
                textOnly = SayiyiYaziyaCevir(singleAmountValue, "tr");
            }
            else if (chkEnglish.Checked)
            {
                textOnly = SayiyiYaziyaCevir(singleAmountValue, "en");
            }
            else
            {
                textOnly = "N/A";
            }

            string textShippingNo = configValues.TryGetValue("textBoxShippingNo", out string shippingNo) ? shippingNo : "";
            string textShippingDate = configValues.TryGetValue("textBoxShippingDate", out string shippingDate) ? shippingDate : "";
            string textInvoiceDateValue = txtTarih?.Text ?? "N/A";
            string textDescriptionData = txtAciklama?.Text ?? "";
            string textToValue = txtProjeNo?.Text ?? "N/A";
            string textInvoiceNumber = GenerateInvoiceNumber(txtProjeNo.Text);

            string textTotalValue = singleAmountValue;
            string textGrandTotalValue = singleAmountValue;

            string leftInfoText = configValues.TryGetValue("textBoxLeftInfo", out string valLeftInfo) ? valLeftInfo : "Sol Bilgi Metni";
            string rightInfoText = configValues.TryGetValue("textBoxRightInfo", out string valRightInfo) ? valRightInfo : "Sağ Bilgi Metni";

            string[,,] footerTexts = new string[3, 2, 3];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    for (int innerRow = 0; innerRow < 3; innerRow++)
                    {
                        string configKey = $"S{row + 1}C{col + 1}İçS{innerRow + 1}";
                        footerTexts[row, col, innerRow] = configValues.TryGetValue(configKey, out string value) ? value : "";
                    }
                }
            }

            string[] texts3rdRow = { text3_1, text3_2, text3_3, text3_4 };
            string[] texts4thRow = { text4_1, text4_2, text4_3, text4_4 };

            double actualFirstRowHeight = MeasureTextHeightWithFormatter(gfx, textMainBlock1Info, dynamicFont, maxBlockWidth - (2 * cellPadding));
            double actualSecondRowHeight = MeasureTextHeightWithFormatter(gfx, textMainBlock2Info, dynamicFont, maxBlockWidth - (2 * cellPadding));

            Func<string[], Tuple<double[], double[]>> calculateRowDimensions = (textsInRow) =>
            {
                double[] cellWidths = new double[4];
                double[] cellHeights = new double[4];

                for (int i = 0; i < 4; i++)
                {
                    cellWidths[i] = Math.Max(minCellWidth, MeasureTextWidth(gfx, textsInRow[i], dynamicFont) + (2 * cellPadding));
                }

                double totalIdealWidth = cellWidths.Sum();

                if (totalIdealWidth > maxBlockWidth)
                {
                    double compressionFactor = maxBlockWidth / totalIdealWidth;
                    for (int i = 0; i < 4; i++)
                    {
                        cellWidths[i] *= compressionFactor;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        double actualTextWidthForWrap = cellWidths[i] - (2 * cellPadding);
                        cellHeights[i] = MeasureTextHeightWithFormatter(gfx, textsInRow[i], dynamicFont, Math.Max(0, actualTextWidthForWrap));
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double actualTextWidthForWrap = cellWidths[i] - (2 * cellPadding);
                        cellHeights[i] = MeasureTextHeightWithFormatter(gfx, textsInRow[i], dynamicFont, Math.Max(0, actualTextWidthForWrap));
                    }
                }

                return Tuple.Create(cellWidths, cellHeights);
            };

            var thirdRowDims = calculateRowDimensions(texts3rdRow);
            double[] widths3rdRow = thirdRowDims.Item1;
            double[] heights3rdRow = thirdRowDims.Item2;
            double actualThirdRowHeight = Math.Max(lineHeight + lineSpacing, heights3rdRow.Max() + (2 * cellPadding));

            var fourthRowDims = calculateRowDimensions(texts4thRow);
            double[] widths4thRow = fourthRowDims.Item1;
            double[] heights4thRow = fourthRowDims.Item2;
            double actualFourthRowHeight = Math.Max(lineHeight + lineSpacing, heights4thRow.Max() + (2 * cellPadding));

            double currentYOffset = leftBoxesYStart;

            double row0YStart = currentYOffset;
            double row0YEnd = row0YStart + actualFirstRowHeight + (2 * cellPadding);
            XRect rectRow0 = new XRect(leftBoxesX + cellPadding, row0YStart + cellPadding, maxBlockWidth - (2 * cellPadding), actualFirstRowHeight);
            tf.DrawString(textMainBlock1Info, dynamicFont, XBrushes.Black, rectRow0, XStringFormats.TopLeft);
            currentYOffset = row0YEnd;

            double row1YStart = currentYOffset;
            double row1YEnd = row1YStart + actualSecondRowHeight + (2 * cellPadding);
            XRect rectRow1 = new XRect(leftBoxesX + cellPadding, row1YStart + cellPadding, maxBlockWidth - (2 * cellPadding), actualSecondRowHeight);
            tf.DrawString(textMainBlock2Info, dynamicFont, XBrushes.Black, rectRow1, XStringFormats.TopLeft);
            currentYOffset = row1YEnd;

            double row2YStart = currentYOffset;
            double row2YEnd = row2YStart + actualThirdRowHeight;
            currentYOffset = row2YEnd;

            double row3YStart = currentYOffset;
            double row3YEnd = row3YStart + actualFourthRowHeight;
            currentYOffset = row3YEnd;

            double actualBlockWidth = widths3rdRow.Sum();
            double totalBlockHeight = currentYOffset - leftBoxesYStart;

            XPen thinPen = new XPen(XColors.Black, 0.5);

            gfx.DrawLine(thinPen, leftBoxesX, leftBoxesYStart, leftBoxesX, leftBoxesYStart + totalBlockHeight);
            gfx.DrawLine(thinPen, leftBoxesX, row0YEnd, leftBoxesX + maxBlockWidth, row0YEnd);
            gfx.DrawLine(thinPen, leftBoxesX, row1YEnd, leftBoxesX + maxBlockWidth, row1YEnd);
            gfx.DrawLine(thinPen, leftBoxesX, row2YEnd, leftBoxesX + actualBlockWidth, row2YEnd);

            double invoiceY_local = yPosition + 10;
            gfx.DrawString(invoiceText, fontInvoice, XBrushes.Black, new XPoint(invoiceX, invoiceY_local + invoiceSize.Height));

            string[] invoiceLines = {
                "Serial:",
                "Invoice No:",
                "Invoice Date:",
                "Shipping Date:",
                "Shipping No:"
            };

            double singleLineInvoiceDetailHeight = new XFont("Arial", 8, XFontStyleEx.Regular).Height + lineSpacing;
            double targetInvoiceDetailsBlockHeight = totalBlockHeight - (invoiceY_local + invoiceSize.Height + 20 - leftBoxesYStart);
            int numberOfInvoiceLines = invoiceLines.Length;
            double totalRequiredTextHeight = numberOfInvoiceLines * new XFont("Arial", 8, XFontStyleEx.Regular).Height;
            double baseLineSpacing = 10;
            double textStartX_local = invoiceX;
            double textStartY = invoiceY_local + invoiceSize.Height + 20;

            for (int i = 0; i < invoiceLines.Length; i++)
            {
                double currentY = textStartY + (i * (new XFont("Arial", 8, XFontStyleEx.Regular).Height + baseLineSpacing));
                gfx.DrawString(invoiceLines[i], new XFont("Arial", 8, XFontStyleEx.Regular), XBrushes.Black,
                    new XPoint(textStartX_local, currentY));

                if (invoiceLines[i] == "Invoice Date:")
                {
                    double valueX = textStartX_local + gfx.MeasureString(invoiceLines[i], new XFont("Arial", 8, XFontStyleEx.Regular)).Width + 5;
                    gfx.DrawString(textInvoiceDateValue, new XFont("Arial", 8, XFontStyleEx.Regular), XBrushes.Black, new XPoint(valueX, currentY));
                }
                else if (invoiceLines[i] == "Invoice No:")
                {
                    double valueX = textStartX_local + gfx.MeasureString(invoiceLines[i], new XFont("Arial", 8, XFontStyleEx.Regular)).Width + 5;
                    gfx.DrawString(textInvoiceNumber, new XFont("Arial", 8, XFontStyleEx.Regular), XBrushes.Black, new XPoint(valueX, currentY));
                }
            }

            double currentSubdivisionX;

            currentSubdivisionX = leftBoxesX;
            for (int i = 0; i < 4; i++)
            {
                if (i < 3)
                {
                    gfx.DrawLine(thinPen, currentSubdivisionX + widths3rdRow[i], row2YStart, currentSubdivisionX + widths3rdRow[i], row2YEnd);
                }

                XRect textBounds = new XRect(currentSubdivisionX + cellPadding, row2YStart + cellPadding,
                                              widths3rdRow[i] - (2 * cellPadding), actualThirdRowHeight - (2 * cellPadding));
                tf.DrawString(texts3rdRow[i], dynamicFont, XBrushes.Black, textBounds, XStringFormats.TopLeft);
                currentSubdivisionX += widths3rdRow[i];
            }

            currentSubdivisionX = leftBoxesX;
            for (int i = 0; i < 4; i++)
            {
                if (i < 3)
                {
                    gfx.DrawLine(thinPen, currentSubdivisionX + widths4thRow[i], row3YStart, currentSubdivisionX + widths4thRow[i], row3YEnd);
                }

                XRect textBounds = new XRect(currentSubdivisionX + cellPadding, row3YStart + cellPadding,
                                              widths4thRow[i] - (2 * cellPadding), actualFourthRowHeight - (2 * cellPadding));
                tf.DrawString(texts4thRow[i], dynamicFont, XBrushes.Black, textBounds, XStringFormats.TopLeft);
                currentSubdivisionX += widths4thRow[i];
            }

            double toPanelHeightCm = 3.0;
            double toPanelHeightPoints = toPanelHeightCm * 72 / 2.54;
            double toPanelYStart = currentYOffset + 5 + (1.5 * 72 / 2.54);
            XRect toPanelRect = new XRect(margin, toPanelYStart, pageWidth - (2 * margin), toPanelHeightPoints);
            gfx.DrawRectangle(thinPen, toPanelRect);

            XPoint toLabelPoint = new XPoint(toPanelRect.X + cellPadding, toPanelRect.Y + cellPadding + panelFont.Height);
            gfx.DrawString("To:", panelFont, XBrushes.Black, toLabelPoint);

            XRect toValueRect = new XRect(toLabelPoint.X + gfx.MeasureString("To:", panelFont).Width + 5,
                                          toLabelPoint.Y - panelFont.Height + 3,
                                          toPanelRect.Width - (toLabelPoint.X + gfx.MeasureString("To:", panelFont).Width + 5) + toPanelRect.X - cellPadding,
                                          toPanelHeightPoints - (2 * cellPadding));
            tf.DrawString(textToValue, panelFont, XBrushes.Black, toValueRect, XStringFormats.TopLeft);

            gfx.DrawString("Client Tax Center:", panelFont, XBrushes.Black,
                new XPoint(toPanelRect.X + cellPadding, toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));

            double clientTaxIdX = toPanelRect.Right - 5 * 72 / 2.54;
            gfx.DrawString("Client Tax ID:", panelFont, XBrushes.Black,
                new XPoint(clientTaxIdX, toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));

            double newPanelsYStart = toPanelRect.Bottom + 5;

            double ratioDesc = 5;
            double ratioQty = 1.5;
            double ratioUnitPrice = 3;
            double ratioTotalPrice = 3;
            double totalRatio = ratioDesc + ratioQty + ratioUnitPrice + ratioTotalPrice;

            double panelSpacing = 5;
            double availableWidth = pageWidth - (2 * margin) - (3 * panelSpacing);

            double panelDescWidth = (availableWidth / totalRatio) * ratioDesc;
            double panelQtyWidth = (availableWidth / totalRatio) * ratioQty;
            double panelUnitPriceWidth = (availableWidth / totalRatio) * ratioUnitPrice;
            double panelTotalPriceWidth = (availableWidth / totalRatio) * ratioTotalPrice;

            double defaultMainPanelHeightCm = 10;
            double defaultMainPanelHeightPoints = defaultMainPanelHeightCm * 72 / 2.54;

            double headerHeight = 0.6 * 72 / 2.54;
            double subPanelHeightPoints = headerHeight;
            double subPanelVerticalSpacing = 5;

            double currentPanelX_local = margin;
            double currentSubPanelY_local;

            Action<string, XRect, XFont, XBrush> DrawCenteredHeaderText = (text, rect, font, brush) =>
            {
                XSize textSize = gfx.MeasureString(text, font);
                double textX = rect.X + (rect.Width - textSize.Width) / 2;
                double textY = rect.Y + (rect.Height - textSize.Height) / 2 + textSize.Height;
                gfx.DrawString(text, font, brush, textX, textY);
            };

            XRect panel1Rect = new XRect(currentPanelX_local, newPanelsYStart, panelDescWidth, defaultMainPanelHeightPoints);
            gfx.DrawRectangle(thinPen, panel1Rect);

            XRect panel1HeaderRect = new XRect(panel1Rect.X, panel1Rect.Y, panel1Rect.Width, headerHeight);
            gfx.DrawRectangle(XBrushes.Black, panel1HeaderRect);
            DrawCenteredHeaderText("DESCRIPTION - SERIAL NUMBER", panel1HeaderRect, boldHeaderFont, XBrushes.White);

            // Açıklama metnini ForceWordWrap ile sarmala
            string wrappedDescriptionData = ForceWordWrap(gfx, textDescriptionData, panelFont, panel1Rect.Width - (2 * cellPadding));
            double descriptionHeight = MeasureTextHeightWithFormatter(gfx, wrappedDescriptionData, panelFont, panel1Rect.Width - (2 * cellPadding));
            XRect descriptionDataRect = new XRect(panel1Rect.X + cellPadding, panel1HeaderRect.Bottom + cellPadding,
                                                  panel1Rect.Width - (2 * cellPadding), descriptionHeight);
            tf.DrawString(wrappedDescriptionData, panelFont, XBrushes.Black, descriptionDataRect, XStringFormats.TopLeft);

            double notesWidth = panel1Rect.Width;
            double totalNotesHeight = 0;
            var processedNotes = new List<(string text, double height)>();

            foreach (var (richTextBox, index) in notRichTextBoxes.Select((rtb, i) => (rtb, i + 1)))
            {
                if (!string.IsNullOrEmpty(richTextBox.Text))
                {
                    string originalNotText = $"Not {index}: {richTextBox.Text}";
                    string notText = ForceWordWrap(gfx, originalNotText, panelFont, notesWidth - (2 * cellPadding));
                    double notHeight = MeasureTextHeightWithFormatter(gfx, notText, panelFont, notesWidth - (2 * cellPadding));
                    processedNotes.Add((notText, notHeight));
                    totalNotesHeight += notHeight + (cellPadding / 2);
                }
            }

            double availableDrawingHeightForNotes = panel1Rect.Bottom - descriptionDataRect.Bottom - cellPadding;
            double currentNotesYPosition;

            if (totalNotesHeight < availableDrawingHeightForNotes)
            {
                currentNotesYPosition = panel1Rect.Bottom - totalNotesHeight - cellPadding;
            }
            else
            {
                currentNotesYPosition = descriptionDataRect.Bottom + cellPadding;
            }

            foreach (var (notText, notHeight) in processedNotes)
            {
                if (currentNotesYPosition + notHeight <= panel1Rect.Bottom - cellPadding)
                {
                    XRect notRect = new XRect(panel1Rect.X + cellPadding, currentNotesYPosition, notesWidth - (2 * cellPadding), notHeight);
                    tf.DrawString(notText, panelFont, XBrushes.Black, notRect, XStringFormats.TopLeft);
                    currentNotesYPosition += notHeight + (cellPadding / 2);
                }
                else
                {
                    XRect notRect = new XRect(panel1Rect.X + cellPadding, currentNotesYPosition, notesWidth - (2 * cellPadding), panel1Rect.Bottom - currentNotesYPosition - cellPadding);
                    if (notRect.Height > 0)
                    {
                        tf.DrawString(notText, panelFont, XBrushes.Black, notRect, XStringFormats.TopLeft);
                    }
                    break;
                }
            }

            double labelIndent = 0;
            double spaceAfterPanel1 = 5;
            double currentLabelY = panel1Rect.Bottom + spaceAfterPanel1 + (0.25 * 72 / 2.54);
            double labelTextSpacing = 5;
            double largeLabelLineHeight = largeLabelFont.Height + 2;

            gfx.DrawString("Only:", largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent, currentLabelY));
            gfx.DrawString(textOnly, largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent + gfx.MeasureString("Only:", largeLabelFont).Width + labelTextSpacing, currentLabelY));

            currentLabelY += largeLabelLineHeight;

            gfx.DrawString("Shipping No:", largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent, currentLabelY));
            gfx.DrawString(textShippingNo, largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent + gfx.MeasureString("Shipping No:", largeLabelFont).Width + labelTextSpacing, currentLabelY));

            currentLabelY += largeLabelLineHeight;

            gfx.DrawString("Shipping Date:", largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent, currentLabelY));
            gfx.DrawString(textShippingDate, largeLabelFont, XBrushes.Black,
                new XPoint(margin + labelIndent + gfx.MeasureString("Shipping Date:", largeLabelFont).Width + labelTextSpacing, currentLabelY));

            currentPanelX_local += panelDescWidth + panelSpacing;

            XRect panel2Rect = new XRect(currentPanelX_local, newPanelsYStart, panelQtyWidth, defaultMainPanelHeightPoints);
            gfx.DrawRectangle(thinPen, panel2Rect);

            XRect panel2HeaderRect = new XRect(panel2Rect.X, panel2Rect.Y, panel2Rect.Width, headerHeight);
            gfx.DrawRectangle(XBrushes.Black, panel2HeaderRect);
            DrawCenteredHeaderText("QUANTITY", panel2HeaderRect, boldHeaderFont, XBrushes.White);
            double panel2BottomY = panel2Rect.Bottom;
            currentPanelX_local += panelQtyWidth + panelSpacing;

            double panel3StartX = currentPanelX_local;
            double panel3Width = panelUnitPriceWidth;

            double panel4StartX_Calculated = panel3StartX + panel3Width + panelSpacing;
            double panel4Width = panelTotalPriceWidth;

            double targetBottomYForSubPanels = panel2BottomY;

            double totalSubPanelsHeightWithSpacing = (3 * subPanelHeightPoints) + (2 * subPanelVerticalSpacing);
            double subPanelYStartOffset = targetBottomYForSubPanels - totalSubPanelsHeightWithSpacing;
            double panelToSubPanelSpacing = 5;

            double panel3CalculatedHeightPoints = subPanelYStartOffset - newPanelsYStart - panelToSubPanelSpacing;

            XRect panel3Rect = new XRect(panel3StartX, newPanelsYStart, panel3Width, panel3CalculatedHeightPoints);
            gfx.DrawRectangle(thinPen, panel3Rect);

            XRect panel3HeaderRect = new XRect(panel3Rect.X, panel3Rect.Y, panel3Rect.Width, headerHeight);
            gfx.DrawRectangle(XBrushes.Black, panel3HeaderRect);
            DrawCenteredHeaderText("UNIT PRICE", panel3HeaderRect, boldHeaderFont, XBrushes.White);

            XRect unitPriceValueRect = new XRect(panel3Rect.X + cellPadding, panel3HeaderRect.Bottom + cellPadding,
                                                 panel3Rect.Width - (2 * cellPadding), panel3Rect.Height - headerHeight - (2 * cellPadding));
            tf.DrawString(singleAmountValue, panelFont, XBrushes.Black, unitPriceValueRect, XStringFormats.TopLeft);

            currentSubPanelY_local = subPanelYStartOffset;

            XRect subPanel3_1Rect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, subPanel3_1Rect);
            gfx.DrawRectangle(XBrushes.Black, subPanel3_1Rect);
            DrawCenteredHeaderText("TOTAL", subPanel3_1Rect, boldHeaderFont, XBrushes.White);

            currentSubPanelY_local += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel3_2Rect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, subPanel3_2Rect);
            gfx.DrawRectangle(XBrushes.Black, subPanel3_2Rect);
            DrawCenteredHeaderText("VAT", subPanel3_2Rect, boldHeaderFont, XBrushes.White);

            currentSubPanelY_local += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel3_3Rect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, subPanel3_3Rect);
            gfx.DrawRectangle(XBrushes.Black, subPanel3_3Rect);
            DrawCenteredHeaderText("GRAND TOTAL", subPanel3_3Rect, boldHeaderFont, XBrushes.White);

            XRect panel4Rect = new XRect(panel4StartX_Calculated, newPanelsYStart, panel4Width, panel3CalculatedHeightPoints);
            gfx.DrawRectangle(thinPen, panel4Rect);

            XRect panel4HeaderRect = new XRect(panel4Rect.X, panel4Rect.Y, panel4Rect.Width, headerHeight);
            gfx.DrawRectangle(XBrushes.Black, panel4HeaderRect);
            DrawCenteredHeaderText("TOTAL PRICE", panel4HeaderRect, boldHeaderFont, XBrushes.White);

            XRect totalPriceValueRect = new XRect(panel4Rect.X + cellPadding, panel4HeaderRect.Bottom + cellPadding,
                                                  panel4Rect.Width - (2 * cellPadding), panel4Rect.Height - headerHeight - (2 * cellPadding));
            tf.DrawString(singleAmountValue, panelFont, XBrushes.Black, totalPriceValueRect, XStringFormats.TopLeft);

            double currentSubPanelY_Panel4 = subPanelYStartOffset;

            XRect subPanel4_1Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_1Rect);
            gfx.DrawRectangle(thinPen, subPanel4_1Rect);
            DrawCenteredHeaderText(textTotalValue, subPanel4_1Rect, boldHeaderFont, XBrushes.Black);

            currentSubPanelY_Panel4 += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel4_2Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_2Rect);
            gfx.DrawRectangle(thinPen, subPanel4_2Rect);

            currentSubPanelY_Panel4 += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel4_3Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_3Rect);
            gfx.DrawRectangle(thinPen, subPanel4_3Rect);
            DrawCenteredHeaderText(textGrandTotalValue, subPanel4_3Rect, boldHeaderFont, XBrushes.Black);

            double miniRowHeight = labelFont.Height + 2;
            double eachInnerCellTotalHeight = (3 * miniRowHeight);
            double mainFooterBlockHeight = 3 * eachInnerCellTotalHeight;
            double mainFooterBlockYStart = pageHeight - margin - mainFooterBlockHeight;
            double mainFooterBlockWidth = pageWidth - (2 * margin);

            XPen blackPen = new XPen(XColors.Black, 0.8);

            XRect mainFooterBlockRect = new XRect(margin, mainFooterBlockYStart, mainFooterBlockWidth, mainFooterBlockHeight);
            gfx.DrawRectangle(blackPen, mainFooterBlockRect);

            double eachFooterRowHeight = mainFooterBlockHeight / 3;
            double eachFooterColumnWidth = mainFooterBlockWidth / 2;

            for (int row = 0; row < 3; row++)
            {
                double currentRowY = mainFooterBlockYStart + (row * eachFooterRowHeight);
                gfx.DrawLine(blackPen, margin + eachFooterColumnWidth, currentRowY, margin + eachFooterColumnWidth, currentRowY + eachFooterRowHeight);

                if (row < 2)
                {
                    gfx.DrawLine(blackPen, margin, currentRowY + eachFooterRowHeight, margin + mainFooterBlockWidth, currentRowY + eachFooterRowHeight);
                }

                for (int col = 0; col < 2; col++)
                {
                    double currentColumnX_Loop = margin + (col * eachFooterColumnWidth);

                    for (int innerRow = 0; innerRow < 3; innerRow++)
                    {
                        double currentMiniRowY = currentRowY + (innerRow * miniRowHeight);
                        string textToDraw = footerTexts[row, col, innerRow];
                        double offsetY = (miniRowHeight - labelFont.Height) / 2;

                        XRect textDrawRect = new XRect(currentColumnX_Loop + cellPadding,
                                                       currentMiniRowY + offsetY,
                                                       eachFooterColumnWidth - (2 * cellPadding),
                                                       labelFont.Height);
                        tf.DrawString(textToDraw, labelFont, XBrushes.Black, textDrawRect, XStringFormats.TopLeft);
                    }
                }
            }

            double infoLabelYPosition = mainFooterBlockYStart - (infoLabelFont.Height + 5);
            XSize leftInfoSize = gfx.MeasureString(leftInfoText, infoLabelFont);
            gfx.DrawString(leftInfoText, infoLabelFont, XBrushes.Black,
                           new XPoint(margin, infoLabelYPosition));

            XSize rightInfoSize = gfx.MeasureString(rightInfoText, infoLabelFont);
            gfx.DrawString(rightInfoText, infoLabelFont, XBrushes.Black,
                           new XPoint(pageWidth - margin - rightInfoSize.Width, infoLabelYPosition));

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string invoiceFolderPath = Path.Combine(desktopPath, "Invoice");
            if (!Directory.Exists(invoiceFolderPath))
            {
                Directory.CreateDirectory(invoiceFolderPath);
            }
            string filename = Path.Combine(invoiceFolderPath, $"{textInvoiceNumber}.pdf");
            try
            {
                document.Save(filename);
                MessageBox.Show($"PDF '{textInvoiceNumber}.pdf' Invoice klasörüne başarıyla oluşturuldu.", "PDF Oluşturuldu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (IOException)
            {
                MessageBox.Show("İşlem, başka bir işlem tarafından kullanıldığından dosyaya erişemiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}