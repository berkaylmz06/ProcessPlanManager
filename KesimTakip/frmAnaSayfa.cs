using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KesimTakip.Abstracts;
using KesimTakip.Concretes;
using KesimTakip.DataBase;
using KesimTakip.Helper;

namespace KesimTakip
{
    public partial class frmAnaSayfa : Form
    {
        private Timer timer;
        private int timerCounter = 0;
        public IFormArayuzu FormArayuzuInterface { get; private set; }
        public frmAnaSayfa(string adSoyad)
        {
            InitializeComponent();

            FormArayuzuInterface = new FormArayuzu(this);
            ctlKesimPlaniEkle1.FormArayuzuAyarla(FormArayuzuInterface);

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

            panelAraYuz.BackColor = ColorTranslator.FromHtml("#2C3E50");
            panelYardimCubugu.BackColor = ColorTranslator.FromHtml("#E67E22");
          
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
            
            ButonGenelHelper.StilUygula(btnKesimPlaniEkle);  
            ButonGenelHelper.StilUygula(btnKesimYap);
            ButonGenelHelper.StilUygula(btnYapilanKesimleriGor);
            ButonGenelHelper.StilUygula(btnOturumuKapat);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnSistem);
            ButonGenelHelper.TuruncuZeminButonStilUygula(btnYardim);
            MenuStripGenelHelper.StilUygula(menuStrip1);

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
            string kesimYapanKullanici = lblSistemKullanici?.Text;
            foreach (Form form in Application.OpenForms)
            {
                if (form is frmKesimYap)
                {
                    form.Focus();
                    return;
                }
            }

            frmKesimYap kesimYapForm = new frmKesimYap(kesimYapanKullanici);
            kesimYapForm.Show();
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
        private void btnKesimPlaniEkle_Click(object sender, EventArgs e)
        {
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
        

        //---------------------------------------------------------------------------------------BAYKALPDF İÇİN KULLANILABİLİR---------------------------------------------
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
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------



        //private void HesaplaEkAgirlikYuzdeleri()
        //{
        //    try
        //    {
        //        var logLines = new List<string> {
        //    "=== EkAgirlikYuzdeLog.txt ===",
        //    $"İşlem Tarihi: {DateTime.Now}",
        //    "\nDataGridView2 Verileri Analizi:"
        //};
        //        var pozGroups = new Dictionary<string, List<(string Poz, string SayfaID, int Adet, double Agirlik)>>();
        //        Regex ekRegex = new Regex(@"-EK\d+$", RegexOptions.IgnoreCase);

        //        foreach (DataGridViewRow row in dataGridView2.Rows)
        //        {
        //            if (row.IsNewRow) continue;

        //            string poz = row.Cells[0].Value?.ToString() ?? "";
        //            string sayfaID = row.Cells[1].Value?.ToString() ?? "";
        //            string adetStr = row.Cells[2].Value?.ToString() ?? "0";
        //            string agirlikStr = row.Cells[3].Value?.ToString() ?? "0";

        //            // Virgül varsa nokta ile değiştir
        //            agirlikStr = agirlikStr.Replace(",", ".");

        //            if (!int.TryParse(adetStr, out int adet) ||
        //                !double.TryParse(agirlikStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double agirlik))
        //            {
        //                logLines.Add($"Hata: Geçersiz adet veya ağırlık değeri - Poz: {poz}");
        //                continue;
        //            }

        //            string basePoz = ekRegex.Replace(poz, "");

        //            if (!pozGroups.ContainsKey(basePoz))
        //                pozGroups[basePoz] = new List<(string, string, int, double)>();

        //            pozGroups[basePoz].Add((poz, sayfaID, adet, agirlik));
        //        }

        //        logLines.Add("\nEK İbaresi İçeren Pozlar ve Yüzde Hesaplamaları:");
        //        foreach (var group in pozGroups)
        //        {
        //            string basePoz = group.Key;
        //            var items = group.Value;

        //            if (items.Any(item => ekRegex.IsMatch(item.Poz)) && items.Count > 1)
        //            {
        //                double toplamAgirlik = items.Sum(item => item.Agirlik);
        //                logLines.Add($"\nPoz: {basePoz}, Toplam Ağırlık: {toplamAgirlik}");

        //                foreach (var item in items)
        //                {
        //                    double oran = item.Agirlik / toplamAgirlik;
        //                    logLines.Add($"  - Poz: {item.Poz}, Sayfa ID: {item.SayfaID}, Adet: {item.Adet}, Ağırlık: {item.Agirlik}, Parçanın Toplam Ağırlığa Oranı {oran}");
        //                }
        //            }
        //        }

        //        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //        string logPath = Path.Combine(desktopPath, "EkAgirlikYuzdeLog.txt");
        //        File.WriteAllLines(logPath, logLines);

        //        MessageBox.Show("EK ibaresi içeren pozların ağırlık yüzdeleri hesaplandı ve log dosyası masaüstüne kaydedildi!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Hata oluştu: {ex.Message}");
        //    }
        //}
    }
}
