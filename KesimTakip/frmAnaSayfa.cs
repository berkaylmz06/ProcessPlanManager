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
            panelContainer.Size = new System.Drawing.Size(1924, 150);
            buttonGroup = new List<Button> { btnAjan, btnAdm, btnBaykal };

            btnAjan.Click += ExclusiveButton_Click;
            btnAdm.Click += ExclusiveButton_Click;
            btnBaykal.Click += ExclusiveButton_Click;


            ButonMakinaSecHelper.ButonSekli(btnAdm, buttonGroup);
            ButonMakinaSecHelper.ButonSekli(btnBaykal, buttonGroup);
            ButonMakinaSecHelper.ButonSekli(btnAjan, buttonGroup);
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

        private async void btnSec_Click(object sender, EventArgs e)
        {
            if (_seciliButon == null)
            {
                MessageBox.Show("Lütfen önce bir makine seçin (Ajan, Adm, Baykal).");
                return;
            }

            // Ortak UI temizleme işlemleri
            richTextBox1.Clear();
            richTextBox2.Clear();
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

                        // PDF'yi yükle (varsa, mevcut metodunuz)
                        await PdfYukle(filePath);

                        // PDF metnini oku
                        string pdfText = await PdfOku(filePath);

                        if (string.IsNullOrEmpty(pdfText))
                        {
                            MessageBox.Show("PDF metni okunamadı.");
                            progressBar1.Visible = false;
                            return;
                        }

                        // Seçili butona göre ilgili metodu çalıştır
                        List<MalzemeBilgisi> parsedData = await Task.Run(() =>
                        {
                            try
                            {
                                if (_seciliButon == btnAjan)
                                    return _veriOkuma.AjanOku(pdfText); // Ajan için metod
                                else if (_seciliButon == btnAdm)
                                    return _veriOkuma.LantekOku(pdfText); // Adm için LantekOku
                                else if (_seciliButon == btnBaykal)
                                    return _veriOkuma.BaykalOku(pdfText); // Baykal için metod
                                else
                                return null;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                return null;
                            }
                        });

                        if (parsedData == null || parsedData.Count == 0)
                        {
                            progressBar1.Visible = false;
                            return;
                        }

                        // UI'yı güncelle
                        richTextBox2.Clear();
                        dataGridView1.Rows.Clear();

                        int totalItems = parsedData.Count;
                        int currentItem = 0;

                        foreach (var data in parsedData)
                        {
                            if (data != null)
                            {
                                richTextBox2.AppendText($"{data.Kalite} - {data.Kalınlık} - {data.Kalıp} - {data.Poz} - {data.Adet}\n");
                                dataGridView1.Rows.Add(data.Kalite, data.Kalınlık, data.Kalıp, data.Poz, data.Adet);

                                // ProgressBar'ı güncelle
                                currentItem++;
                                int progressValue = (int)((currentItem / (float)totalItems) * 100);
                                progressBar1.Value = progressValue;
                            }
                        }

                        // Yerleştirme tekrar sayısını hesapla
                        YerlestirmeTekrarSayisi(pdfText);

                        // ProgressBar'ı gizle
                        progressBar1.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata oluştu: " + ex.Message);
                        progressBar1.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Dosya bulunamadı.");
                    progressBar1.Visible = false;
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
