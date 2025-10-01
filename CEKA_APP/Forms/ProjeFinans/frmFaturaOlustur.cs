using CEKA_APP.DataBase;
using CEKA_APP.DataBase.ProjeFinans;
using CEKA_APP.Helper;
using CEKA_APP.Interfaces.ProjeFinans;
using CEKA_APP.Services.ProjeFinans;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string proje;
        private int notSayisi = 0;
        private List<RichTextBox> notRichTextBoxes = new List<RichTextBox>();
        private List<Label> notLabels = new List<Label>();
        private List<Button> notKapatButonlari = new List<Button>();
        private Panel notPanel;
        private readonly string projeNo;
        private readonly List<int> kilometreTasiIds;
        private readonly List<int?> odemeIds;

        private readonly IOdemeSartlariService _odemeSartlariService;
        private readonly IMusterilerService _musterilerService;
        private readonly IFinansProjelerService _finansProjelerService;
        private readonly IProjeIliskiService _projeIliskiService;
        private readonly IProjeKutukService _projeKutukService;

        public frmFaturaOlustur(IOdemeSartlariService odemeSartlariService, IMusterilerService musterilerService, IFinansProjelerService finansProjelerService, IProjeIliskiService projeIliskiService, IProjeKutukService projeKutukService, string tutar, string aciklama, string tarih, string proje, List<int> kilometreTasiIds = null, List<int?> odemeIds = null)
        {
            InitializeComponent();

            _odemeSartlariService = odemeSartlariService ?? throw new ArgumentNullException(nameof(odemeSartlariService));
            _musterilerService = musterilerService ?? throw new ArgumentNullException(nameof(musterilerService));
            _finansProjelerService = finansProjelerService ?? throw new ArgumentNullException(nameof(finansProjelerService));
            _projeIliskiService = projeIliskiService ?? throw new ArgumentNullException(nameof(projeIliskiService));
            _projeKutukService = projeKutukService ?? throw new ArgumentNullException(nameof(projeKutukService));

            this.tutar = tutar ?? "0";
            this.aciklama = aciklama ?? "";
            this.tarih = tarih ?? "Belirtilmemiş";
            this.proje = proje ?? "Bilinmiyor";
            this.projeNo = proje?.Trim();
            this.kilometreTasiIds = kilometreTasiIds ?? new List<int>();
            this.odemeIds = odemeIds ?? new List<int?>();

            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new PlatformFontResolver();

            ReadConfigFile();
        }
        public frmFaturaOlustur(IOdemeSartlariService odemeSartlariService, IMusterilerService musterilerService, IFinansProjelerService finansProjelerService, IProjeIliskiService projeIliskiService, IProjeKutukService projeKutukService, string tutar, string aciklama, string tarih, string proje, int kilometreTasiId = 0, int? odemeId = null)
      : this(odemeSartlariService, musterilerService, finansProjelerService, projeIliskiService, projeKutukService, tutar, aciklama, tarih, proje,
             new List<int> { kilometreTasiId },
             odemeId.HasValue ? new List<int?> { odemeId } : new List<int?>())
        {
        }

        private void frmFaturaOlustur_Load(object sender, EventArgs e)
        {
            txtTutar.Text = tutar;
            txtAciklama.Text = aciklama;
            txtTarih.Text = tarih;
            txtProjeNo.Text = proje;
            var projeId = _finansProjelerService.GetProjeIdByNo(projeNo);
            this.Icon = Properties.Resources.cekalogokirmizi;

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
            if (!string.IsNullOrEmpty(projeNo))
            {
                int? effectiveProjeNo = projeId;
                int? ustProjeId = null;

                // Eğer alt proje ise, üst proje ID'sini al
                if (_projeIliskiService.CheckAltProje(projeId.Value))
                {
                    ustProjeId = _projeIliskiService.GetUstProjeId(projeId.Value);
                    effectiveProjeNo = ustProjeId ?? projeId;
                }

                // Proje kutuğu önce mevcut proje, yoksa üst proje ID ile ara
                var projeKutuk = _projeKutukService.ProjeKutukAra(effectiveProjeNo.Value);

                if (projeKutuk != null && !string.IsNullOrEmpty(projeKutuk.musteriNo))
                {
                    var musteriBilgi = _musterilerService.GetMusteriByMusteriNo(projeKutuk.musteriNo);

                    if (musteriBilgi != null)
                    {
                        txtMusteri.Text = musteriBilgi.musteriAdi ?? "";
                        txtMusteriAdresi.Text = musteriBilgi.adres ?? "";
                        txtVergiMerkezi.Text = musteriBilgi.vergiDairesi ?? "";
                        txtVergiNo.Text = musteriBilgi.vergiNo ?? "";
                        txtKdv.Text = "";
                        txtKdv.Enabled = !string.IsNullOrEmpty(musteriBilgi.musteriMensei) &&
                                        musteriBilgi.musteriMensei.Trim().ToLower() == "türkiye";
                    }
                    else
                    {
                        MessageBox.Show($"Müşteri numarası {projeKutuk.musteriNo} için bilgi bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Eğer üst proje varsa mesajda onu da göster
                    string message = ustProjeId.HasValue
                        ? $"Proje numarasına {projeNo} ve üst proje numarasına {ustProjeId} göre müşteri numarası bulunamadı."
                        : $"Proje numarasına {projeNo} göre müşteri numarası bulunamadı.";

                    MessageBox.Show(message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            var faturaNoSet = new HashSet<string>();
            bool hasNullFaturaNo = false;
            foreach (var kmId in kilometreTasiIds)
            {
                if (kmId > 0)
                {
                    var odemeBilgi = _odemeSartlariService.GetOdemeBilgi(projeNo, kmId);
                    if (odemeBilgi != null)
                    {
                        if (string.IsNullOrEmpty(odemeBilgi.faturaNo))
                        {
                            hasNullFaturaNo = true;
                        }
                        else
                        {
                            faturaNoSet.Add(odemeBilgi.faturaNo);
                        }
                    }
                }
            }

            if (faturaNoSet.Count == 1 && !hasNullFaturaNo)
            {
                string faturaNo = faturaNoSet.First();
                lblFaturaBilgi.Text = $"Seçili kilometre taşları için {faturaNo} fatura no ile daha önceden fatura oluşturulmuş!";
                btnGoruntule.Enabled = true;
                btnSil.Enabled = true;
                btnPdfOlustur.Enabled = false;
            }
            else if (faturaNoSet.Count > 1 || hasNullFaturaNo)
            {
                lblFaturaBilgi.Text = "";
                btnGoruntule.Enabled = false;
                btnSil.Enabled = false;
                btnPdfOlustur.Enabled = true;
            }
            else
            {
                lblFaturaBilgi.Text = "";
                btnGoruntule.Enabled = false;
                btnSil.Enabled = false;
                btnPdfOlustur.Enabled = true;
            }
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
            string configFilePath = ConfigurationManager.AppSettings["ConfigYolu"];

            if (string.IsNullOrEmpty(configFilePath))
            {
                MessageBox.Show("Config dosyası yolu AppSettings'de tanımlı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new InvalidOperationException("Config dosyası yolu AppSettings'de tanımlı değil.");
            }

            if (!File.Exists(configFilePath))
            {
                MessageBox.Show($"config.txt belirtilen yolda bulunamadı: {configFilePath}.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new FileNotFoundException($"Config dosyası bulunamadı: {configFilePath}");
            }

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
                throw;
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

            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    result.Append(" ");
                    continue;
                }

                if (gfx.MeasureString(word, font).Width > maxWidth)
                {
                    var currentSegment = new System.Text.StringBuilder();
                    foreach (char c in word)
                    {
                        currentSegment.Append(c);
                        if (gfx.MeasureString(currentSegment.ToString(), font).Width > maxWidth)
                        {
                            result.Append(currentSegment.ToString(0, currentSegment.Length - 1));
                            result.Append(" ");
                            currentSegment.Clear();
                            currentSegment.Append(c);
                        }
                    }
                    result.Append(currentSegment.ToString());
                }
                else
                {
                    result.Append(word);
                }
                result.Append(" ");
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

            decimal parsedTutar;
            if (dil == "tr")
            {
                if (!decimal.TryParse(tutar, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("tr-TR"), out parsedTutar))
                {
                    MessageBox.Show(dil == "tr" ? "Geçersiz sayı formatı." : "Invalid number format.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "HATA: Geçersiz Sayı!";
                }
            }
            else
            {
                if (!decimal.TryParse(tutar, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsedTutar))
                {
                    MessageBox.Show(dil == "tr" ? "Geçersiz sayı formatı." : "Invalid number format.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "HATA: Geçersiz Sayı!";
                }
            }

            long tamKisim = (long)Math.Truncate(parsedTutar);
            int ondalikKisim = (int)((parsedTutar - tamKisim) * 100);

            string tamKisimStr = tamKisim.ToString();
            string ondalikKisimStr = ondalikKisim.ToString("D2");

            if (tamKisimStr.Length > 18)
            {
                MessageBox.Show(dil == "tr" ? "Sayı katrilyondan büyük olamaz." : "Number cannot be larger than quadrillion.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "HATA: Sayı çok büyük!";
            }

            string sonuc = "";

            if (tamKisim == 0)
            {
                sonuc = zeroWord;
            }
            else
            {
                sonuc = ConvertNumberGroup(tamKisimStr, dil, birler, onlar, binler, hundredWord);
            }

            sonuc += " " + currencyMain;

            if (ondalikKisim > 0)
            {
                if (dil == "en" && tamKisim != 0)
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
            {
                MessageBox.Show("Proje numarası boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException("Proje numarası boş olamaz.");
            }

            string yearLastTwo = DateTime.Now.Year.ToString().Substring(2);
            string baseInvoiceNumber = $"{yearLastTwo}P{projeNo}_";

            int lastCounter = 0;
            string configFilePath = ConfigurationManager.AppSettings["ConfigYolu"];
            if (string.IsNullOrEmpty(configFilePath))
            {
                MessageBox.Show("Config dosyası yolu AppSettings'de tanımlı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new InvalidOperationException("Config dosyası yolu AppSettings'de tanımlı değil.");
            }

            if (configValues.TryGetValue("invoiceCounter", out string counterValue) && int.TryParse(counterValue, out int parsedCounter))
            {
                lastCounter = parsedCounter;
            }

            int counter = lastCounter + 1;
            string finalInvoiceNumber = $"{baseInvoiceNumber}{counter:D3}";

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
            catch (IOException ex)
            {
                MessageBox.Show($"Config dosyasına erişim sırasında hata oluştu: {ex.Message}.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Config dosyasına yazma izni yok: {ex.Message}. Lütfen ağ izinlerini kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            string invoiceFolderPath = ConfigurationManager.AppSettings["InvoiceFolderPath"];
            if (string.IsNullOrEmpty(invoiceFolderPath))
            {
                MessageBox.Show("Fatura klasör yolu AppSettings'de tanımlı değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new InvalidOperationException("Fatura klasör yolu AppSettings'de tanımlı değil.");
            }

            if (!Directory.Exists(invoiceFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(invoiceFolderPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fatura klasörü oluşturulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }

            string fullPath = Path.Combine(invoiceFolderPath, $"{finalInvoiceNumber}.pdf");
            while (File.Exists(fullPath))
            {
                counter++;
                finalInvoiceNumber = $"{baseInvoiceNumber}{counter:D3}";
                fullPath = Path.Combine(invoiceFolderPath, $"{finalInvoiceNumber}.pdf");
            }

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
                    throw;
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

            if (string.IsNullOrEmpty(txtMusteri.Text))
            {
                MessageBox.Show("Lütfen bir Proje Numarası giriniz. Fatura numarası oluşturulamaz.", "Proje Numarası Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            string textDescriptionData = (txtAciklama?.Text ?? "").Replace("\\n", "\n");
            string singleAmountValue = txtTutar?.Text ?? "0,00";
            string textKdvValue = txtKdv?.Text ?? "0";

            double tutar = 0;
            double kdvOrani = 0;
            double kdvTutari = 0;
            double grandTotal = 0;

            var culture = new System.Globalization.CultureInfo("tr-TR");

            string[] amountLines = singleAmountValue.Split(new[] { "\\n" }, StringSplitOptions.None);
            List<double> validAmounts = new List<double>();
            foreach (var line in amountLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (double.TryParse(line, System.Globalization.NumberStyles.Any, culture, out double amount))
                {
                    validAmounts.Add(amount);
                }
                else
                {
                    MessageBox.Show($"Tutar alanında geçersiz bir değer bulundu: '{line}'. Lütfen yalnızca sayılar ve \\n kullanın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            tutar = validAmounts.Sum();

            string cleanKdvValue = txtKdv?.Text.Replace("%", "").Trim() ?? "0";
            bool isKdvValid = double.TryParse(cleanKdvValue, System.Globalization.NumberStyles.Any, culture, out kdvOrani);

            if (isKdvValid && kdvOrani > 0)
            {
                kdvTutari = tutar * (kdvOrani / 100);
                grandTotal = tutar + kdvTutari;
            }
            else
            {
                kdvTutari = 0;
                grandTotal = tutar;
            }

            string formattedTutar = tutar.ToString("N2", culture);
            string formattedKdvTutari = kdvTutari.ToString("N2", culture);
            string formattedGrandTotal = grandTotal.ToString("N2", culture);

            string formattedKdvOrani = kdvOrani > 0 ? $"%{kdvOrani.ToString(culture)}" : "";

            string textOnly;
            if (tutar == 0)
            {
                MessageBox.Show("Lütfen geçerli bir tutar girin.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textOnly = "N/A";
            }
            else
            {
                string lang = chkEnglish.Checked ? "en" : "tr";
                textOnly = SayiyiYaziyaCevir(grandTotal.ToString(new System.Globalization.CultureInfo(lang == "en" ? "en-US" : "tr-TR")), lang);
            }

            string textTotalValue = formattedTutar;
            string textGrandTotalValue = formattedGrandTotal;


            string textShippingNo = configValues.TryGetValue("textBoxShippingNo", out string shippingNo) ? shippingNo : "";
            string textShippingDate = configValues.TryGetValue("textBoxShippingDate", out string shippingDate) ? shippingDate : "";
            string textInvoiceDateValue = txtTarih?.Text ?? "N/A";
            string textToValue = txtMusteri?.Text ?? "N/A";
            string textInvoiceNumber = GenerateInvoiceNumber(txtProjeNo.Text);


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
                                          panelFont.Height);
            tf.DrawString(textToValue, panelFont, XBrushes.Black, toValueRect, XStringFormats.TopLeft);

            string textAddressValue = txtMusteriAdresi?.Text ?? "N/A";
            textAddressValue = textAddressValue.Replace("\r\n", " ").Replace("\n", " ");
            string wrappedAddressValue = ForceWordWrap(gfx, textAddressValue, panelFont, toPanelRect.Width - (toLabelPoint.X + gfx.MeasureString("To:", panelFont).Width + 5) + toPanelRect.X - cellPadding);
            double addressY = toValueRect.Y + panelFont.Height + 5;
            XRect addressRect = new XRect(toLabelPoint.X + gfx.MeasureString("To:", panelFont).Width + 5,
                                          addressY,
                                          toPanelRect.Width - (toLabelPoint.X + gfx.MeasureString("To:", panelFont).Width + 5) + toPanelRect.X - cellPadding,
                                          toPanelHeightPoints - addressY + toPanelRect.Y - (2 * cellPadding));
            tf.DrawString(wrappedAddressValue, panelFont, XBrushes.Black, addressRect, XStringFormats.TopLeft);

            string textVergiMerkeziValue = txtVergiMerkezi?.Text ?? "N/A";
            gfx.DrawString("Client Tax Center:", panelFont, XBrushes.Black,
                new XPoint(toPanelRect.X + cellPadding, toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));
            gfx.DrawString(textVergiMerkeziValue, panelFont, XBrushes.Black,
                new XPoint(toPanelRect.X + cellPadding + gfx.MeasureString("Client Tax Center:", panelFont).Width + 5,
                           toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));

            string textVergiNoValue = txtVergiNo?.Text ?? "N/A";
            double clientTaxIdX = toPanelRect.Right - 5 * 72 / 2.54;
            gfx.DrawString("Client Tax ID:", panelFont, XBrushes.Black,
                new XPoint(clientTaxIdX, toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));
            gfx.DrawString(textVergiNoValue, panelFont, XBrushes.Black,
                new XPoint(clientTaxIdX + gfx.MeasureString("Client Tax ID:", panelFont).Width + 5,
                           toPanelRect.Y + toPanelHeightPoints - cellPadding - panelFont.Height));

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
            double subPanelHeightPoints = headerHeight * 1.5;
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

            XRect descriptionDataRect = new XRect(panel1Rect.X + cellPadding, panel1HeaderRect.Bottom + cellPadding,
                                      panel1Rect.Width - (2 * cellPadding), panel1Rect.Height - headerHeight - (2 * cellPadding));
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(textDescriptionData, panelFont, XBrushes.Black, descriptionDataRect, XStringFormats.TopLeft);

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


            double descriptionHeight = MeasureTextHeightWithFormatter(gfx, textDescriptionData, panelFont, panel1Rect.Width - (2 * cellPadding));

            descriptionDataRect.Height = descriptionHeight;
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(textDescriptionData, panelFont, XBrushes.Black, descriptionDataRect, XStringFormats.TopLeft);

            double availableDrawingHeightForNotes = panel1Rect.Bottom - descriptionDataRect.Bottom - cellPadding;

            double currentNotesYPosition;

            if (availableDrawingHeightForNotes <= 0 || totalNotesHeight > availableDrawingHeightForNotes)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                tf = new XTextFormatter(gfx);
                notesWidth = pageWidth - (2 * margin);
                currentNotesYPosition = margin;
                gfx.DrawString("Notlar (Devam)", boldHeaderFont, XBrushes.Black, new XPoint(margin, margin), XStringFormats.TopLeft);
                currentNotesYPosition += boldHeaderFont.Height + 10;
            }
            else
            {
                currentNotesYPosition = totalNotesHeight < availableDrawingHeightForNotes
                    ? panel1Rect.Bottom - totalNotesHeight - cellPadding
                    : descriptionDataRect.Bottom + cellPadding;
            }

            foreach (var (notText, notHeight) in processedNotes)
            {
                double remainingHeight = availableDrawingHeightForNotes <= 0 ? pageHeight - margin - currentNotesYPosition : panel1Rect.Bottom - currentNotesYPosition - cellPadding;
                if (remainingHeight <= 0)
                {
                    MessageBox.Show("Notlar için yeterli alan kalmadı, bazı notlar görüntülenemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }

                double rectHeight = Math.Min(notHeight, remainingHeight);
                XRect notRect = new XRect(margin + cellPadding, currentNotesYPosition, notesWidth - (2 * cellPadding), rectHeight);
                tf.DrawString(notText, panelFont, XBrushes.Black, notRect, XStringFormats.TopLeft);
                currentNotesYPosition += notHeight + (cellPadding / 2);
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
            tf.Alignment = XParagraphAlignment.Left;
            double unitPriceCurrentY = unitPriceValueRect.Y;
            foreach (var line in amountLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (double.TryParse(line, System.Globalization.NumberStyles.Any, culture, out double amount))
                {
                    string formattedAmount = amount.ToString("N2", culture);
                    tf.DrawString(formattedAmount, panelFont, XBrushes.Black, new XRect(unitPriceValueRect.X, unitPriceCurrentY, unitPriceValueRect.Width, panelFont.Height), XStringFormats.TopLeft);
                    unitPriceCurrentY += panelFont.Height + 2;
                }
            }
            currentSubPanelY_local = subPanelYStartOffset;

            XRect subPanel3_1Rect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, subPanel3_1Rect);
            gfx.DrawRectangle(XBrushes.Black, subPanel3_1Rect);
            DrawCenteredHeaderText("TOTAL", subPanel3_1Rect, boldHeaderFont, XBrushes.White);

            currentSubPanelY_local += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect vatPanelRect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, vatPanelRect);
            gfx.DrawRectangle(XBrushes.Black, vatPanelRect);
            DrawCenteredHeaderText("VAT", vatPanelRect, boldHeaderFont, XBrushes.White);

            currentSubPanelY_local += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect grandTotalPanelRect = new XRect(panel3StartX, currentSubPanelY_local, panel3Width, subPanelHeightPoints);
            gfx.DrawRectangle(thinPen, grandTotalPanelRect);
            gfx.DrawRectangle(XBrushes.Black, grandTotalPanelRect);
            DrawCenteredHeaderText("GRAND TOTAL", grandTotalPanelRect, boldHeaderFont, XBrushes.White);

            XRect panel4Rect = new XRect(panel4StartX_Calculated, newPanelsYStart, panel4Width, panel3CalculatedHeightPoints);
            gfx.DrawRectangle(thinPen, panel4Rect);
            XRect panel4HeaderRect = new XRect(panel4Rect.X, panel4Rect.Y, panel4Rect.Width, headerHeight);
            gfx.DrawRectangle(XBrushes.Black, panel4HeaderRect);
            DrawCenteredHeaderText("TOTAL PRICE", panel4HeaderRect, boldHeaderFont, XBrushes.White);

            XRect totalPriceValueRect = new XRect(panel4Rect.X + cellPadding, panel4HeaderRect.Bottom + cellPadding,
                                                panel4Rect.Width - (2 * cellPadding), panel4Rect.Height - headerHeight - (2 * cellPadding));
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(formattedTutar, panelFont, XBrushes.Black, totalPriceValueRect, XStringFormats.TopLeft);

            double currentSubPanelY_Panel4 = subPanelYStartOffset;

            XRect subPanel4_1Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_1Rect);
            gfx.DrawRectangle(thinPen, subPanel4_1Rect);
            DrawCenteredHeaderText(formattedTutar, subPanel4_1Rect, boldHeaderFont, XBrushes.Black);

            currentSubPanelY_Panel4 += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel4_2Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_2Rect);
            gfx.DrawRectangle(thinPen, subPanel4_2Rect);

            double desiredShiftX = 28.039;
            double minWidth = 20;
            double vatWidth = panel4Width - (2 * cellPadding - 1) - desiredShiftX;
            double vatValueWidth = Math.Max(minWidth, vatWidth);
            XRect vatValueRect = new XRect(
                subPanel4_2Rect.X + cellPadding + desiredShiftX,
                subPanel4_2Rect.Y + cellPadding + 4,
                vatValueWidth,
                subPanelHeightPoints - cellPadding
            );
            string kdvDisplay = !string.IsNullOrEmpty(formattedKdvOrani) ? $"{formattedKdvOrani} ({formattedKdvTutari})" : ""; string wrappedKdvOrani = ForceWordWrap(gfx, kdvDisplay, panelFont, vatValueWidth - (2 * cellPadding));
            tf.DrawString(wrappedKdvOrani, panelFont, XBrushes.Black, vatValueRect, XStringFormats.TopLeft);

            currentSubPanelY_Panel4 += subPanelHeightPoints + subPanelVerticalSpacing;

            XRect subPanel4_3Rect = new XRect(panel4StartX_Calculated, currentSubPanelY_Panel4, panel4Width, subPanelHeightPoints);
            gfx.DrawRectangle(XBrushes.White, subPanel4_3Rect);
            gfx.DrawRectangle(thinPen, subPanel4_3Rect);
            DrawCenteredHeaderText(formattedGrandTotal, subPanel4_3Rect, boldHeaderFont, XBrushes.Black);

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

            string invoiceFolderPath = ConfigurationManager.AppSettings["InvoiceFolderPath"];
            if (!string.IsNullOrEmpty(invoiceFolderPath) && !Directory.Exists(invoiceFolderPath))
            {
                Directory.CreateDirectory(invoiceFolderPath);
            }
            string filename = Path.Combine(invoiceFolderPath, $"{textInvoiceNumber}.pdf");

            try
            {
                document.Save(filename);
                bool allUpdatesSuccessful = true;

                foreach (var odemeId in odemeIds.Where(id => id.HasValue))
                {
                    bool updateSuccess = _odemeSartlariService.UpdateFaturaNo(odemeId.Value, textInvoiceNumber);
                    if (!updateSuccess)
                    {
                        allUpdatesSuccessful = false;
                    }
                }

                if (odemeIds.Any(id => id.HasValue))
                {
                    if (allUpdatesSuccessful)
                    {
                        btnGoruntule.Enabled = true;
                        btnSil.Enabled = true;
                        btnPdfOlustur.Enabled = false;
                        lblFaturaBilgi.Text = $"Seçili kilometre taşları için {textInvoiceNumber} numaralı fatura oluşturulmuştur.";
                        MessageBox.Show($"PDF '{textInvoiceNumber}.pdf' Invoice klasörüne başarıyla oluşturuldu ve fatura numaraları veritabanına kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"PDF '{textInvoiceNumber}.pdf' oluşturuldu, ancak bazı fatura numaraları veritabanına kaydedilemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"PDF '{textInvoiceNumber}.pdf' Invoice klasörüne başarıyla oluşturuldu, ancak ödeme ID'leri geçersiz olduğu için fatura numaraları kaydedilemedi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (IOException ex)
            {
                MessageBox.Show($"PDF dosyası oluşturulurken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnGoruntule_Click(object sender, EventArgs e)
        {
            var faturaNoSet = new HashSet<string>();

            foreach (var kmId in kilometreTasiIds)
            {
                if (kmId > 0)
                {
                    var odemeBilgi = _odemeSartlariService.GetOdemeBilgi(projeNo, kmId);
                    if (odemeBilgi != null && !string.IsNullOrEmpty(odemeBilgi.faturaNo?.ToString()))
                    {
                        faturaNoSet.Add(odemeBilgi.faturaNo.ToString());
                    }
                }
            }

            if (faturaNoSet.Count == 1)
            {
                string faturaNo = faturaNoSet.First();
                string invoiceFolderPath = ConfigurationManager.AppSettings["InvoiceFolderPath"];
                string fullPath = Path.Combine(invoiceFolderPath, $"{faturaNo}.pdf");
                if (File.Exists(fullPath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fatura görüntülenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Fatura dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (faturaNoSet.Count > 1)
            {
                MessageBox.Show("Seçili satırların fatura numaraları farklı. Lütfen tek bir fatura numarasıyla ilişkili satırları seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Bu ödeme için fatura bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            var faturaNoSet = new HashSet<string>();

            foreach (var kmId in kilometreTasiIds)
            {
                if (kmId > 0)
                {
                    var odemeBilgi = _odemeSartlariService.GetOdemeBilgi(projeNo, kmId);
                    if (odemeBilgi != null && !string.IsNullOrEmpty(odemeBilgi.faturaNo))
                    {
                        faturaNoSet.Add(odemeBilgi.faturaNo);
                    }
                }
            }

            if (faturaNoSet.Count == 1)
            {
                string faturaNo = faturaNoSet.First();
                string invoiceFolderPath = ConfigurationManager.AppSettings["InvoiceFolderPath"];
                string fullPath = Path.Combine(invoiceFolderPath, $"{faturaNo}.pdf");

                try
                {
                    bool fileExistsBeforeDelete = File.Exists(fullPath);
                    if (fileExistsBeforeDelete)
                    {
                        File.Delete(fullPath);
                    }

                    bool allUpdatesSuccessful = true;
                    foreach (var odemeId in odemeIds.Where(id => id.HasValue))
                    {
                        bool updateSuccess = _odemeSartlariService.UpdateFaturaNo(odemeId.Value, null);
                        if (!updateSuccess)
                        {
                            allUpdatesSuccessful = false;
                        }
                    }

                    if (allUpdatesSuccessful)
                    {
                        lblFaturaBilgi.Text = "";
                        btnGoruntule.Enabled = false;
                        btnSil.Enabled = false;
                        btnPdfOlustur.Enabled = true;

                        if (fileExistsBeforeDelete)
                        {
                            MessageBox.Show("Fatura başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Fatura dosyası bulunamadı, ancak fatura numaraları veritabanından kaldırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bazı fatura numaraları veritabanından kaldırılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Fatura dosyası silinirken hata oluştu: {ex.Message}. Dosya başka bir işlem tarafından kullanılıyor olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (faturaNoSet.Count > 1)
            {
                MessageBox.Show("Seçili satırların fatura numaraları farklı. Lütfen tek bir fatura numarasıyla ilişkili satırları seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Bu ödeme için fatura bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblFaturaBilgi.Text = "";
                btnGoruntule.Enabled = false;
                btnSil.Enabled = false;
                btnPdfOlustur.Enabled = true;
            }
        }
        private void txtKdv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if ((e.KeyChar == ',' || e.KeyChar == '.') && txtKdv.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}