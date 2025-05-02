using KesimTakip.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static KesimTakip.frmAnaSayfa;

namespace KesimTakip.Business
{
    public class VeriOkuma
    {
        public List<MalzemeBilgisi> LantekOku(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Giriş metni boş veya null olamaz.");
            }

            var parsedDataList = new List<MalzemeBilgisi>();
            try
            {
                const string partPattern = @"(\d+-\d+-\d+-P-\d+)(?:\s([A-Za-z0-9\-\/]+))?\s(\d+)\s([\d,]+)\s([\d,]+)\s([\d,]+)";
                var partRegex = new Regex(partPattern, RegexOptions.Compiled);
                var partMatches = partRegex.Matches(input);

                var seenMaterials = new HashSet<string>();
                foreach (Match match in partMatches)
                {
                    string partNumber = match.Groups[1].Value;
                    string partDescription = string.IsNullOrEmpty(match.Groups[2].Value) ? "Yok" : match.Groups[2].Value;
                    string partQuantity = match.Groups[3].Value;
                    string partLength = match.Groups[4].Value;
                    string partWeight = match.Groups[5].Value;

                    if (!ValidateNumericValues(partQuantity, partLength, partWeight))
                    {
                        throw new FormatException($"Geçersiz sayısal veri bulundu (Parça: {partNumber}).");
                    }

                    string partData = $"{partNumber}-{partQuantity}";
                    if (seenMaterials.Add(partData))
                    {
                        var parsed = ParseLine($"Part-{partDescription}-{partNumber}-{partQuantity}");
                        if (parsed != null)
                        {
                            parsedDataList.Add(parsed);
                        }
                    }
                }

                if (parsedDataList.Count == 0)
                {
                    throw new Exception("Hiçbir veri eşleşmedi. Giriş formatını kontrol edin.");
                }
            }
            catch
            {
                MessageBox.Show("PDF okunamadı. Lütfen seçilen makinayı gözden geçiriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return parsedDataList;
        }

        private bool ValidateNumericValues(params string[] values)
        {
            foreach (var value in values)
            {
                if (!decimal.TryParse(value.Replace(",", "."), out decimal parsedValue) || parsedValue < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) AjanOku(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Metin kutusu boş olamaz");
            }

            List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
            List<string> invalidData = new List<string>();
            HashSet<string> validDataSet = new HashSet<string>();
            HashSet<string> invalidDataSet = new HashSet<string>();

            string genelPattern = @"(?i)ST\d+.*?AD\s*-\s*\d{5}\.\d{2}";
            string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

            MatchCollection matches = Regex.Matches(text, genelPattern);

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();
                if (invalidDataSet.Contains(value))
                    continue;

                Match validMatch = Regex.Match(value, validPattern);
                if (validMatch.Success)
                {
                    string uniqueKey = $"{validMatch.Groups[1].Value}|{validMatch.Groups[2].Value}|{validMatch.Groups[3].Value}|{validMatch.Groups[4].Value}|{validMatch.Groups[5].Value}|{validMatch.Groups[6].Value}";

                    if (validDataSet.Add(uniqueKey))
                    {
                        var malzeme = new MalzemeBilgisi
                        {
                            Kalite = validMatch.Groups[1].Value,
                            Kalinlik = validMatch.Groups[2].Value,
                            Kalip = validMatch.Groups[3].Value,
                            Poz = validMatch.Groups[4].Value,
                            Adet = validMatch.Groups[5].Value,
                            Proje = validMatch.Groups[6].Value
                        };
                        validData.Add(malzeme);
                    }
                }
                else if (invalidDataSet.Add(value))
                {
                    invalidData.Add(value);
                }
            }

            return (validData, invalidData);
        }

        public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) BaykalOku(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Okunacak veri yok!");
            }

            List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
            List<string> invalidData = new List<string>();
            HashSet<string> validDataSet = new HashSet<string>();
            HashSet<string> invalidDataSet = new HashSet<string>();

            string genelPattern = @"(?i)ST\d+.*?AD\s*-\s*\d{5}\.\d{2}";
            string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

            MatchCollection matches = Regex.Matches(text, genelPattern);

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();
                if (invalidDataSet.Contains(value))
                    continue;

                Match validMatch = Regex.Match(value, validPattern);
                if (validMatch.Success)
                {
                    string uniqueKey = $"{validMatch.Groups[1].Value}|{validMatch.Groups[2].Value}|{validMatch.Groups[3].Value}|{validMatch.Groups[4].Value}|{validMatch.Groups[5].Value}|{validMatch.Groups[6].Value}";

                    if (validDataSet.Add(uniqueKey))
                    {
                        var malzeme = new MalzemeBilgisi
                        {
                            Kalite = validMatch.Groups[1].Value,
                            Kalinlik = validMatch.Groups[2].Value,
                            Kalip = validMatch.Groups[3].Value,
                            Poz = validMatch.Groups[4].Value,
                            Adet = validMatch.Groups[5].Value,
                            Proje = validMatch.Groups[6].Value
                        };
                        validData.Add(malzeme);
                    }
                }
                else if (invalidDataSet.Add(value))
                {
                    invalidData.Add(value);
                }
            }

            return (validData, invalidData);
        }

        private MalzemeBilgisi ParseLine(string line)
        {
            var match = Regex.Match(line,
                @"ST(?<Kalite>\d{2})[-\s]*(?<Kalınlık>\d+(?:MM|mm))[-\s]*(?<Kalıp>\d{3}-\d{2})[-\s]*(?<Poz>P\d{1,3})[-\s]*(?<Adet>\d+[A-Za-z]{2})[-\s]*(?<Proje>\d{5}\.\d{2}",
                RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return new MalzemeBilgisi
                {
                    Kalite = $"ST{match.Groups["Kalite"].Value}",
                    Kalinlik = match.Groups["Kalınlık"].Value,
                    Kalip = match.Groups["Kalıp"].Value,
                    Poz = match.Groups["Poz"].Value,
                    Adet = match.Groups["Adet"].Value,
                    Proje = match.Groups["Proje"].Value,
                };
            }

            return null;
        }
        public class PageInfo0
        {
            public string Id { get; set; }
            public string YerlesimSayisi { get; set; }
            public string TekrarSayisi { get; set; }
        }

        public class BaykalPartInfo
        {
            public string Poz { get; set; }
            public string Layout { get; set; }
            public int Count { get; set; }
        }
        public class PageInfo
        {
            public string Id { get; set; }
            public string Sayfa { get; set; }
            public string ToplamSayfaTekrari { get; set; }
        }

        public class SayfaBilgisi
        {
            public string SayfaNumarasi { get; set; }
            public string TekrarSayisi { get; set; }
        }
        public class ProNestParser
        {
            public (List<BaykalPartInfo> PartInfoList, List<PageInfo0> PageInfoList, string LogFilePath) ParseProNestOutput(string text, string baseId)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Metin boş olamaz.");
                }

                if (string.IsNullOrWhiteSpace(baseId))
                {
                    throw new ArgumentException("BaseId boş olamaz.");
                }

                string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProNest_Log.txt");
                File.WriteAllText(logFilePath, "ProNest Parser Log\n=================\n");

                List<BaykalPartInfo> partInfoList = new List<BaykalPartInfo>();
                List<PageInfo0> pageInfoList = new List<PageInfo0>();

                string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                File.AppendAllText(logFilePath, $"Toplam satır sayısı: {lines.Length}\n");

                string currentLayout = "";
                string baseLayout = "";
                int continuationCount = 0;
                bool isPartSection = false;
                string currentPage = "";
                string tekrarSayisi = "0";

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (line.Contains("Yerleşim:"))
                    {
                        var match = Regex.Match(line, @"Yerleşim: (\d+) / \d+");
                        if (match.Success)
                        {
                            currentPage = match.Groups[1].Value;
                            currentLayout = currentPage;
                            baseLayout = currentPage;
                            continuationCount = 0;
                            isPartSection = false;
                            File.AppendAllText(logFilePath, $"Sayfa tespit edildi: {currentPage}, ID: {baseId}-{currentPage}\n");
                        }
                        continue;
                    }

                    if (line.Contains("Kesme sayısı:"))
                    {
                        var kesmeMatch = Regex.Match(line, @"Kesme sayısı: (\d+)");
                        if (kesmeMatch.Success)
                        {
                            tekrarSayisi = kesmeMatch.Groups[1].Value;
                            File.AppendAllText(logFilePath, $"Kesme sayısı bulundu: {tekrarSayisi}\n");
                        }
                        continue;
                    }

                    if (line.Trim().StartsWith("Sıralama"))
                    {
                        isPartSection = true;
                        string pageId = $"{baseId}-{currentPage}";
                        pageInfoList.Add(new PageInfo0
                        {
                            Id = pageId,
                            YerlesimSayisi = currentPage,
                            TekrarSayisi = tekrarSayisi
                        });
                        File.AppendAllText(logFilePath, $"Sıralama bölümü başladı, sayfa eklendi: {currentPage}, ID: {pageId}, Tekrar: {tekrarSayisi}\n");
                        continue;
                    }

                    if (isPartSection && Regex.IsMatch(line, @"^\d+(?: - \d+)?\s+.*?-P\d+"))
                    {
                        var match = Regex.Match(line, @"(\d+)(?: - (\d+))?\s+.*?(\d+-\d+)-P(\d+)(?:-\d+AD)?");
                        if (match.Success)
                        {
                            string startNum = match.Groups[1].Value;
                            string endNum = match.Groups[2].Value;
                            string partCode = match.Groups[3].Value;
                            string pozNum = match.Groups[4].Value;
                            string poz = $"{partCode}-P{pozNum}";
                            int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                            partInfoList.Add(new BaykalPartInfo
                            {
                                Poz = poz,
                                Layout = currentLayout,
                                Count = count
                            });

                            File.AppendAllText(logFilePath, $"Poz bulundu: {poz}, Yerleşim: {currentLayout}, Adet: {count}, Satır: {line}\n");
                        }
                        else
                        {
                            File.AppendAllText(logFilePath, $"Parça satırı eşleşmedi: {line}\n");
                        }
                    }
                    else if (isPartSection && !line.Trim().StartsWith("Sıralama"))
                    {
                        if (Regex.IsMatch(line, @"^\d+(?: - \d+)?\s+.*?-P\d+"))
                        {
                            continuationCount++;
                            currentLayout = $"{baseLayout}-{continuationCount}";
                            File.AppendAllText(logFilePath, $"Devam eden yerleşim: {currentLayout}\n");

                            var match = Regex.Match(line, @"(\d+)(?: - (\d+))?\s+.*?(\d+-\d+)-P(\d+)(?:-\d+AD)?");
                            if (match.Success)
                            {
                                string startNum = match.Groups[1].Value;
                                string endNum = match.Groups[2].Value;
                                string partCode = match.Groups[3].Value;
                                string pozNum = match.Groups[4].Value;
                                string poz = $"{partCode}-P{pozNum}";
                                int count = string.IsNullOrEmpty(endNum) ? 1 : int.Parse(endNum) - int.Parse(startNum) + 1;

                                partInfoList.Add(new BaykalPartInfo
                                {
                                    Poz = poz,
                                    Layout = currentLayout,
                                    Count = count
                                });

                                File.AppendAllText(logFilePath, $"Poz bulundu: {poz}, Yerleşim: {currentLayout}, Adet: {count}, Satır: {line}\n");
                            }
                            else
                            {
                                File.AppendAllText(logFilePath, $"Parça satırı eşleşmedi: {line}\n");
                            }
                        }
                    }
                }

                var uniquePozs = partInfoList.Select(p => p.Poz).Distinct().OrderBy(p => p).ToList();
                File.AppendAllText(logFilePath, $"Benzersiz pozlar: {string.Join(", ", uniquePozs)}\n");

                if (!uniquePozs.Any())
                {
                    throw new InvalidOperationException("Hiçbir poz bulunamadı. Lütfen veri formatını kontrol edin.");
                }

                return (partInfoList, pageInfoList, logFilePath);
            }
        }

        public class AjanParser
        {
            // Parça bilgilerini tutacak sınıf
            public class PartInfo
            {
                public string Id { get; set; } // Örn: 13-1-1
                public string PartName { get; set; } // Örn: 110-00-P3-61AD-24193.01
                public int Count { get; set; } // Örn: 61
                public string Layout { get; set; } // Örn: 1
                public string Source { get; set; } // Örn: richTextBox1
            }

            // Sayfa bilgilerini tutacak sınıf
            public class PageInfo
            {
                public string Id { get; set; } // Örn: 13-1
                public string PageNumber { get; set; } // Örn: 1
                public string RepeatCount { get; set; } // Örn: 1
            }

            // Ajan çıktısını işleyen ana metod
            public (List<PartInfo> PartInfoList, List<PageInfo> PageInfoList, string LogFilePath) ParseAjanOutput(string text1, string text4, string baseId)
            {
                if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text4))
                {
                    throw new ArgumentException("Metin boş olamaz.");
                }

                if (string.IsNullOrWhiteSpace(baseId))
                {
                    throw new ArgumentException("BaseId boş olamaz.");
                }

                string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Ajan_Log.txt");
                File.WriteAllText(logFilePath, "Ajan Parser Log\n=================\n");

                List<PartInfo> partInfoList = new List<PartInfo>();
                List<PageInfo> pageInfoList = new List<PageInfo>();
                Dictionary<string, int> partCountMap = new Dictionary<string, int>(); // Parça adı -> Adet

                // richTextBox1'i işle (sayfa ve parça bilgileri)
                File.AppendAllText(logFilePath, $"richTextBox1 işleniyor...\n");
                ProcessText(text1, baseId, partInfoList, pageInfoList, partCountMap, logFilePath, "richTextBox1", true);

                // richTextBox4'ü işle (sadece parça bilgileri)
                File.AppendAllText(logFilePath, $"richTextBox4 işleniyor...\n");
                ProcessText(text4, baseId, partInfoList, pageInfoList, partCountMap, logFilePath, "richTextBox4", false);

                File.AppendAllText(logFilePath, $"Toplam parça sayısı: {partInfoList.Count}\n");
                File.AppendAllText(logFilePath, $"Toplam sayfa sayısı: {pageInfoList.Count}\n");

                if (!partInfoList.Any())
                {
                    throw new InvalidOperationException("Hiçbir parça bulunamadı. Lütfen veri formatını kontrol edin.");
                }

                return (partInfoList, pageInfoList, logFilePath);
            }

            // Metni satır satır işleyen yardımcı metod
            private void ProcessText(string text, string baseId, List<PartInfo> partInfoList, List<PageInfo> pageInfoList, Dictionary<string, int> partCountMap, string logFilePath, string source, bool processPageInfo)
            {
                string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                File.AppendAllText(logFilePath, $"{source} için toplam satır sayısı: {lines.Length}\n");

                string currentPageId = "";
                string currentPageNumber = "";
                string repeatCount = "";
                bool isPartSection = false;
                bool skipUntilNextPage = false;
                int partIndex = 0;
                int pageCounter = 0;
                Dictionary<string, string> partIdMap = new Dictionary<string, string>(); // Parça adı -> ID

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // ToplamParçaKesmeListesi ile başlayan sayfayı atla
                    if (line.StartsWith("ToplamParçaKesmeListesi"))
                    {
                        skipUntilNextPage = true;
                        File.AppendAllText(logFilePath, $"ToplamParçaKesmeListesi sayfası atlanıyor: {line} ({source})\n");
                        continue;
                    }

                    // Sayfa sonunu tespit et ve atlama bayrağını sıfırla
                    if (skipUntilNextPage && line.StartsWith("Sayfa:"))
                    {
                        skipUntilNextPage = false;
                        File.AppendAllText(logFilePath, $"ToplamParçaKesmeListesi sayfası atlama bitti: {line} ({source})\n");
                        continue;
                    }

                    if (skipUntilNextPage) continue;

                    // FirmaAdı ile başlayan sayfayı işle
                    if (line.StartsWith("FirmaAdı"))
                    {
                        pageCounter++;
                        currentPageNumber = pageCounter.ToString();
                        currentPageId = $"{baseId}-{currentPageNumber}";
                        partIndex = 0;
                        File.AppendAllText(logFilePath, $"Sayfa tespit edildi: {currentPageNumber}, ID: {currentPageId} ({source})\n");

                        // Tekrar sayısını bul (sadece richTextBox1 için)
                        if (processPageInfo)
                        {
                            for (int j = i; j < lines.Length; j++)
                            {
                                if (lines[j].Contains("LevhaÖlçüleri"))
                                {
                                    var repeatMatch = Regex.Match(lines[j], @"LevhaÖlçüleri\s+\d+X\d+mm\s+(\d+)\s+Kalınlık", RegexOptions.IgnoreCase);
                                    if (repeatMatch.Success)
                                    {
                                        repeatCount = repeatMatch.Groups[1].Value;
                                        pageInfoList.Add(new PageInfo
                                        {
                                            Id = currentPageId,
                                            PageNumber = currentPageNumber,
                                            RepeatCount = repeatCount
                                        });
                                        File.AppendAllText(logFilePath, $"Sayfa eklendi: ID: {currentPageId}, Sayfa: {currentPageNumber}, Tekrar: {repeatCount} ({source})\n");
                                    }
                                    break;
                                }
                            }
                        }
                        continue;
                    }

                    // Parça bölümünü başlat
                    if (line.StartsWith("Parça") && !line.Contains("ParçaAdı") && !isPartSection)
                    {
                        isPartSection = true;
                        File.AppendAllText(logFilePath, $"Parça bölümü başladı: {currentPageId} ({source})\n");
                        continue;
                    }

                    // Parça verilerini işle
                    if (isPartSection && line.StartsWith("ST", StringComparison.OrdinalIgnoreCase))
                    {
                        // richTextBox1 için son ek opsiyonel, richTextBox4 için zorunlu
                        string regexPattern = source == "richTextBox1"
                            ? @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(?:\d{5}\.\d{2})?"
                            : @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2}BKM?)";
                        var partMatch = Regex.Match(line, regexPattern);
                        if (partMatch.Success)
                        {
                            string partName = partMatch.Groups[0].Value; // Tam eşleşen satır
                            string cleanPartName = $"{partMatch.Groups[3].Value}-{partMatch.Groups[4].Value}-{partMatch.Groups[5].Value}";
                            if (partMatch.Groups.Count > 6 && !string.IsNullOrEmpty(partMatch.Groups[6].Value))
                            {
                                cleanPartName += $"-{partMatch.Groups[6].Value.Replace("BKM", "")}";
                            }
                            string partId = $"{currentPageId}-{++partIndex}";
                            int count = 0;

                            // richTextBox1 için adet bilgisi
                            if (source == "richTextBox1")
                            {
                                if (i + 1 < lines.Length)
                                {
                                    var countMatch = Regex.Match(lines[i + 1], @"\d+\s+\d+X\d+\s+\d+\.\d+\s+\d+:\d+:\d+\s+(\d+)");
                                    if (countMatch.Success)
                                    {
                                        count = int.Parse(countMatch.Groups[1].Value);
                                        partCountMap[cleanPartName] = count;
                                        File.AppendAllText(logFilePath, $"Adet bilgisi alındı: {count} için {partName} ({source})\n");
                                    }
                                    else
                                    {
                                        File.AppendAllText(logFilePath, $"Adet bilgisi eşleşmedi: {lines[i + 1]} ({source})\n");
                                    }
                                }
                            }

                            // Parça ID'sini kaydet
                            if (!partIdMap.ContainsKey(cleanPartName))
                            {
                                partIdMap[cleanPartName] = partId;
                            }

                            // richTextBox4 için parça eklerken richTextBox1'den adet al
                            if (source == "richTextBox4")
                            {
                                count = partCountMap.ContainsKey(cleanPartName) ? partCountMap[cleanPartName] : 0;
                            }

                            partInfoList.Add(new PartInfo
                            {
                                Id = partIdMap[cleanPartName],
                                PartName = cleanPartName,
                                Count = count,
                                Layout = currentPageNumber,
                                Source = source
                            });

                            File.AppendAllText(logFilePath, $"Parça eklendi: ID: {partIdMap[cleanPartName]}, Ad: {cleanPartName}, Adet: {count}, Yerleşim: {currentPageNumber} ({source})\n");
                        }
                        else
                        {
                            File.AppendAllText(logFilePath, $"Parça satırı eşleşmedi: {line} ({source})\n");
                        }
                    }
                }
            }

            // DataGridView'leri dolduran metod
            public void AjanTekSayfaDetay(RichTextBox richTextBox1, RichTextBox richTextBox4, TextBox txtId, DataGridView dataGridView2, DataGridView dataGridView3)
            {
                try
                {
                    var parser = new AjanParser();
                    var (partInfoList, pageInfoList, logFilePath) = parser.ParseAjanOutput(richTextBox1.Text, richTextBox4.Text, txtId.Text.Trim());

                    // DataGridView2: Parça Adı, Yerleşim, Adet
                    var groupedParts = partInfoList
                        .Where(p => !string.IsNullOrEmpty(p.PartName) && p.Count > 0)
                        .GroupBy(p => new { p.PartName, p.Layout })
                        .OrderBy(g => g.Key.PartName)
                        .ThenBy(g => g.Key.Layout);

                    dataGridView2.Rows.Clear();
                    foreach (var group in groupedParts)
                    {
                        string partName = group.Key.PartName;
                        string layout = group.Key.Layout;
                        int totalCount = group.Sum(p => p.Count);
                        dataGridView2.Rows.Add(partName, layout, totalCount);
                    }

                    // DataGridView3: ID, Sayfa, Tekrar Sayısı
                    dataGridView3.Rows.Clear();
                    foreach (var page in pageInfoList.OrderBy(p => p.Id))
                    {
                        dataGridView3.Rows.Add(page.Id, page.PageNumber, page.RepeatCount);
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

        }
    }
}