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

namespace KesimTakip
{
    public partial class frmAnaSayfa : Form
    {
        private Timer timer;
        private int timerCounter = 0;
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

        private async void btnSec_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            lblKesimId.Enabled = false;
            txtKesimId.Enabled = false;
            dataGridView1.Rows.Clear();
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

                        await PdfYukle(filePath);

                        string pdfText = await PdfOku(filePath);

                        progressBar1.Visible = false;
                        ProcessPdfData(pdfText);
                        YerlestirmeTekrarSayisi(pdfText);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata oluştu: " + ex.Message);
                        progressBar1.Visible = false;
                    }
                }
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
                    for (int i = 1; i <= pageNumbers; i++)
                    {
                        ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                        string text = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
                        pageText.AppendLine(text);

                        int progressValue = (int)((i / (float)pageNumbers) * 100);
                        Invoke((Action)(() => progressBar1.Value = progressValue));

                        await Task.Delay(100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Metin okuma hatası: " + ex.Message);
            }

            Invoke((Action)(() => richTextBox1.Text = pageText.ToString()));

            return pageText.ToString();
        }

        private void ProcessPdfData(string pdfText)
        {
            string[] firstFourLines = pdfText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Take(4).ToArray();
            string firstFourLinesText = string.Join("\n", firstFourLines);

            if (firstFourLinesText.Contains("ProNest"))
            {
                try
                {
                    string startPattern = @"Numarası\s+Numara";
                    Regex startRegex = new Regex(startPattern);
                    Match startMatch = startRegex.Match(pdfText);

                    if (startMatch.Success)
                    {
                        string materialPattern = @"(\d+)\s+(\d+)\s+(ST\d+-\d+mm-\d+-\d+-P\d+-\d+)\s+AD";
                        Regex materialRegex = new Regex(materialPattern);
                        MatchCollection materialMatches = materialRegex.Matches(pdfText);

                        var extractedData = new StringBuilder();

                        HashSet<string> seenMaterials = new HashSet<string>();

                        foreach (Match match in materialMatches)
                        {
                            string numara1 = match.Groups[1].Value;
                            string numara2 = match.Groups[2].Value;
                            string malzeme = match.Groups[3].Value;

                            string fullMaterial = $"{malzeme} AD";

                            if (!seenMaterials.Contains(fullMaterial))
                            {
                                seenMaterials.Add(fullMaterial);
                                extractedData.AppendLine(fullMaterial);
                            }
                        }

                        Invoke((Action)(() => richTextBox2.AppendText(extractedData.ToString())));

                        ParseAndDisplayData();
                    }
                    else
                    {
                        MessageBox.Show("Pdf okunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata 1" + ex.Message);
                }

            }
            else if (firstFourLinesText.Contains("FirmaAdı"))
            {
                try
                {
                    string pattern = @"(\d+)\s(ST\d+)-(\d+mm)-(\d+-\d+)-(\w+)-(\w+)\s([\dX]+)\s([\d\.]+)\s([\d\:\.]+)\s(\d+)\s([\d\.]+)|(\d+)\s(ST\d+)-(\d+mm)-(\d+-\d+)-(\w+)-(\w+)\s([\dX]+)\s([\d\.]+)\s([\d\:\.]+)\s(\d+)\s([\d\.]+)\s(\d+-\d+)\s+([\d\.]+)";
                    Regex regex = new Regex(pattern);
                    MatchCollection matches = regex.Matches(pdfText);

                    List<string> extractedData = new List<string>();

                    foreach (Match match in matches)
                    {
                        if (match.Groups.Count >= 12)
                        {
                            string kalite = match.Groups[2].Value;
                            string mm = match.Groups[3].Value;
                            string numara = match.Groups[4].Value;
                            string p1 = match.Groups[5].Value;
                            string p2 = match.Groups[6].Value;

                            string result = $"{kalite} - {mm} - {numara} - {p1} - {p2}".Trim();
                            if (!extractedData.Contains(result))
                            {
                                extractedData.Add(result);
                            }
                        }
                    }

                    string finalResult = string.Join("\n", extractedData);
                    richTextBox2.Text = finalResult;
                    ParseAndDisplayData();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata 2" + ex.Message);
                }
            }
            else if (firstFourLinesText.Contains("ToplamParçaKesmeListesi"))
            {
                try
                {
                    string desen = @"ToplamParçaKesmeListesi[\s\S]*?Buverilerstandartkesimparametrelerinegörehesaplanmaktadır";
                    Match aralikMatch = Regex.Match(pdfText, desen, RegexOptions.Singleline);

                    if (aralikMatch.Success)
                    {
                        string sadeceKesimListesi = aralikMatch.Value;

                        string pattern = @"ST\d{2}-\d+mm-\d+-\d+-P\d+-\d+";
                        Regex regex = new Regex(pattern);
                        MatchCollection matches = regex.Matches(sadeceKesimListesi);

                        HashSet<string> extractedData = new HashSet<string>();
                        foreach (Match match in matches)
                        {
                            string matchValue = match.Value;
                            if (!matchValue.EndsWith(" AD"))
                            {
                                matchValue += " AD";
                            }
                            extractedData.Add(matchValue);
                        }

                        string finalResult = string.Join("\n", extractedData);
                        richTextBox2.Text = finalResult;
                        ParseAndDisplayData();
                    }
                    else
                    {
                        MessageBox.Show("Pdf okunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata 3" + ex.Message);
                }
            }

            else
            {
                MessageBox.Show("PDF Okunamadı.");
            }
        }

        private void ParseAndDisplayData()
        {
            dataGridView1.Rows.Clear();

            string[] lines = richTextBox2.Lines;

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"(?<Kalite>[A-Za-z0-9]+)\s*-\s*(?<Kalınlık>[A-Za-z0-9]+)\s*-\s*(?<Kalıp>[A-Za-z0-9-]+)\s*-\s*(?<Poz>[A-Za-z0-9]+)\s*-\s*(?<Adet>[0-9]+)");

                if (match.Success)
                {
                    string kalite = match.Groups["Kalite"].Value;
                    string kalınlık = match.Groups["Kalınlık"].Value;
                    string kalıp = match.Groups["Kalıp"].Value;
                    string poz = match.Groups["Poz"].Value;
                    string adet = match.Groups["Adet"].Value;

                    dataGridView1.Rows.Add(kalite, kalınlık, kalıp, poz, adet);
                }
            }
        }
        private void lantekYukleme(string pdfText)
        {
            string[] lines = pdfText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();

            // Checkbox kontrolü yapılacak
            if (checkBox1.Checked) // Eğer checkbox işaretliyse
            {
                MessageBox.Show("Checkbox işaretli!"); // Debugging için

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("ID"))
                    {
                        if (i + 2 < lines.Length)
                        {
                            string cncLine = lines[i + 2];
                            string[] cncParts = cncLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (cncParts.Length >= 3 &&
                                int.TryParse(cncParts[1], out int cncNo) &&        // CNC numarasını al
                                int.TryParse(cncParts[2], out int quantityUst))   // Üst Quantity'yi al
                            {
                                // CNC programı bulundu, devam ediyoruz
                                for (int j = i + 3; j < lines.Length; j++)
                                {
                                    if (lines[j].StartsWith("ID")) break; // Yeni bir CNC programına geçersek çık

                                    if (lines[j].StartsWith("Part"))
                                    {
                                        j++; // "Part Quantity..." satırını atla
                                        while (j < lines.Length && !lines[j].StartsWith("ID") && !string.IsNullOrWhiteSpace(lines[j]))
                                        {
                                            string[] partParts = lines[j].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                            if (partParts.Length >= 3)
                                            {
                                                // Burada malzeme ve quantity değerlerini alıyoruz
                                                string malzeme = partParts[0];   // Malzeme (örneğin ST1000 gibi)
                                                if (int.TryParse(partParts[1], out int quantityAlt)) // Alt quantity (part quantity)
                                                {
                                                    int carpim = quantityUst * quantityAlt; // Çarpım işlemi

                                                    sb.AppendLine($"{quantityUst} - {malzeme} - {quantityAlt} - {carpim}");
                                                }
                                            }
                                            j++; // Bir sonraki satıra geç
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Checkbox işaretlenmemiş."); // Debugging için
            }

            richTextBox2.Text = sb.ToString();
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
            if (panelYardim.Visible = true)
            {
                panelYardim.Visible = false;
            }

            panelSistem.Visible = !panelSistem.Visible;
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
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
            if (panelSistem.Visible = true)
            {
                panelSistem.Visible = false;
            }

            panelYardim.Visible = !panelYardim.Visible;
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
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

        private void btnKesimYap_Click(object sender, EventArgs e)
        {
            string kesimYapanKullanici = lblSistemKullanici.Text;
            frmKesimYap kesyap = new frmKesimYap(kesimYapanKullanici);
            kesyap.Show();
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Size = new System.Drawing.Size(1924, 150);
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

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            MessageBox.Show("XML başarıyla oluşturuldu.");
        }

        private void btnKesimPlaniEkle_Click(object sender, EventArgs e)
        {
            PanelGoster(panelKesimPlaniEkle);
        }
        private void PanelGoster(Panel target)
        {
            panelKesimPlaniEkle.Visible = false;

            target.Visible = true;
        }

        private void btnAjan_Click(object sender, EventArgs e)
        {
            // Buton tıklandığında seçili durum
            if (btnAjan.BackColor == System.Drawing.Color.LightBlue)
            {
                // Seçili değilse orijinal font boyutu ve renk
                btnAjan.BackColor = System.Drawing.Color.Gray;
                btnAjan.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 8); // Orijinal font boyutu
            }
            else
            {
                // Seçili yap ve fontu küçült
                btnAjan.BackColor = System.Drawing.Color.LightBlue;
                btnAjan.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 10); // Küçültülmüş font boyutu
                btnAjan.FlatStyle = FlatStyle.Flat;
                btnAjan.FlatAppearance.BorderSize = 0;
            }
        }

        private void btnAdm_Click(object sender, EventArgs e)
        {
            // Buton tıklandığında seçili durum
            if (btnAdm.BackColor == System.Drawing.Color.LightBlue)
            {
                // Seçili değilse orijinal font boyutu ve renk
                btnAdm.BackColor = System.Drawing.Color.Gray;
                btnAdm.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 8); // Orijinal font boyutu
            }
            else
            {
                // Seçili yap ve fontu küçült
                btnAdm.BackColor = System.Drawing.Color.LightBlue;
                btnAdm.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 10); // Küçültülmüş font boyutu
                btnAdm.FlatStyle = FlatStyle.Flat;
                btnAdm.FlatAppearance.BorderSize = 0;
            }
        }

        private void btnBaykal_Click(object sender, EventArgs e)
        {
            // Buton tıklandığında seçili durum
            if (btnBaykal.BackColor == System.Drawing.Color.LightBlue)
            {
                // Seçili değilse orijinal font boyutu ve renk
                btnBaykal.BackColor = System.Drawing.Color.Gray;
                btnBaykal.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 8); // Orijinal font boyutu
            }
            else
            {
                // Seçili yap ve fontu küçült
                btnBaykal.BackColor = System.Drawing.Color.LightBlue;
                btnBaykal.Font = new System.Drawing.Font(btnAjan.Font.FontFamily, 10); // Küçültülmüş font boyutu
                btnBaykal.FlatStyle = FlatStyle.Flat;
                btnBaykal.FlatAppearance.BorderSize = 0;
            }
        }
    }
}
