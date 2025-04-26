using KesimTakip.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

        public (List<MalzemeBilgisi> ParsedData, List<string> InvalidData) AjanOku(string pdfText)
        {
            if (string.IsNullOrWhiteSpace(pdfText))
            {
                throw new ArgumentException("PDF metni boş veya null olamaz.", nameof(pdfText));
            }

            var parsedDataList = new List<MalzemeBilgisi>();
            var invalidDataSet = new HashSet<string>();

            try
            {
                // PDF satırlarını ayır
                string[] lines = pdfText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                StringBuilder currentLine = new StringBuilder();
                int i = 0;

                while (i < lines.Length)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrEmpty(line) || line.StartsWith("-------- Sayfa"))
                    {
                        i++;
                        continue;
                    }

                    if (line.StartsWith("ST"))
                    {
                        currentLine.Clear();
                        currentLine.Append(line);

                        // Sonraki satırları kontrol et, 24193.01 veya 24193.01BKM bulana kadar
                        i++;
                        bool foundNumber = false;
                        while (i < lines.Length && !foundNumber)
                        {
                            string nextLine = lines[i].Trim();
                            if (string.IsNullOrEmpty(nextLine) || nextLine.StartsWith("-------- Sayfa"))
                            {
                                i++;
                                continue;
                            }

                            if (Regex.IsMatch(nextLine, @"^\d{5}\.\d{2}$"))
                            {
                                currentLine.Append("-").Append(nextLine);
                                foundNumber = true;
                            }
                            i++;
                        }

                        string processedLine = currentLine.ToString();

                        // generalPattern ile kontrol
                        string generalPattern = @"ST.*?AD\s*-?\s*(\d{5})\.(\d{2})?";
                        if (Regex.IsMatch(processedLine, generalPattern, RegexOptions.IgnoreCase))
                        {
                            // AD'den sonraki 12 karakter kontrolü
                            int adIndex = processedLine.IndexOf("AD", StringComparison.OrdinalIgnoreCase);
                            if (adIndex != -1)
                            {
                                string afterAd = processedLine.Substring(adIndex + 2);
                                if (afterAd.Length >= 12)
                                {
                                    string twelveChars = afterAd.Substring(0, 12);
                                    if (!Regex.IsMatch(twelveChars, @"^\s*-?\s*\d{5}\.\d{2}"))
                                    {
                                        continue; // 12 karakter içinde XXXXX.XX veya XXXXX.XXBKM yoksa satırı yok say
                                    }
                                }
                            }

                            // Katı desen kontrolü
                            if (Regex.IsMatch(processedLine, @"^ST\d{2}\s*-?\s*\d{1,2}MM\s*-?\s*\d{3}\s*-?\s*\d{2}\s*-?\s*P\d{2,3}\s*-?\s*\d{1,3}AD\s*-?\s*\d{5}\.\d{2}$", RegexOptions.IgnoreCase))
                            {
                                var parsed = ParseLine(processedLine);
                                if (parsed != null)
                                {
                                    parsedDataList.Add(parsed);
                                }
                                else
                                {
                                    invalidDataSet.Add(processedLine);
                                }
                            }
                            else
                            {
                                invalidDataSet.Add(processedLine);
                            }
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ajan veri ayrıştırma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (parsedDataList, invalidDataSet.ToList());
        }

        public (List<MalzemeBilgisi> ParsedData, List<string> InvalidData) BaykalOku(string pdfText)
        {
            if (string.IsNullOrWhiteSpace(pdfText))
            {
                throw new ArgumentException("PDF metni boş veya null olamaz.", nameof(pdfText));
            }

            var parsedDataList = new List<MalzemeBilgisi>();
            var invalidDataSet = new HashSet<string>();

            try
            {
                // PDF satırlarını ayır
                string[] lines = pdfText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                StringBuilder currentLine = new StringBuilder();
                int i = 0;

                while (i < lines.Length)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrEmpty(line) || line.StartsWith("-------- Sayfa"))
                    {
                        i++;
                        continue;
                    }

                    if (line.StartsWith("ST"))
                    {
                        currentLine.Clear();
                        currentLine.Append(line);

                        // Sonraki satırları kontrol et, 24193.01 veya 24193.01BKM bulana kadar
                        i++;
                        bool foundNumber = false;
                        while (i < lines.Length && !foundNumber)
                        {
                            string nextLine = lines[i].Trim();
                            if (string.IsNullOrEmpty(nextLine) || nextLine.StartsWith("-------- Sayfa"))
                            {
                                i++;
                                continue;
                            }

                            if (Regex.IsMatch(nextLine, @"^\d{5}\.\d{2}$"))
                            {
                                currentLine.Append("-").Append(nextLine);
                                foundNumber = true;
                            }
                            i++;
                        }

                        string processedLine = currentLine.ToString();

                        // General pattern kontrolü: ST ile başlar ve 5 sayı . 2 sayı ile biter
                        if (Regex.IsMatch(processedLine, @"^ST.*\d{5}\.\d{2}$", RegexOptions.IgnoreCase))
                        {
                            // AD sonrası 12 karakterde XXXXX.XX kontrolü
                            int adIndex = processedLine.IndexOf("AD", StringComparison.OrdinalIgnoreCase);
                            if (adIndex != -1 && processedLine.Length >= adIndex + 2 + 12)
                            {
                                string afterAd = processedLine.Substring(adIndex + 2, 12);
                                if (!Regex.IsMatch(afterAd, @"\d{5}\.\d{2}"))
                                {
                                    continue; // 12 karakter içinde XXXXX.XX yoksa tamamen atla
                                }
                            }
                            else
                            {
                                continue; // AD bulunamazsa veya 12 karakter yoksa da atla
                            }

                            // StrictPattern ile detaylı kontrol: PXX-XXXAD-XXXXX.XX formatı
                            string strictPattern = @"^ST\d{2}-\d{1,2}MM-\d{3}-\d{2}-P\d{2,3}-\d{1,3}AD-\d{5}\.\d{2}$";
                            if (Regex.IsMatch(processedLine, strictPattern, RegexOptions.IgnoreCase))
                            {
                                var parsed = ParseLine(processedLine);
                                if (parsed != null)
                                {
                                    parsedDataList.Add(parsed);
                                }
                                else
                                {
                                    invalidDataSet.Add(processedLine);
                                }
                            }
                            else
                            {
                                invalidDataSet.Add(processedLine);
                            }
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Baykal veri ayrıştırma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (parsedDataList, invalidDataSet.ToList());
        }



        // ParseLine metodu, veriyi doğru şekilde parse eder
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
                    Kalınlık = match.Groups["Kalınlık"].Value,
                    Kalıp = match.Groups["Kalıp"].Value,
                    Poz = match.Groups["Poz"].Value,
                    Adet = match.Groups["Adet"].Value,
                    Proje = match.Groups["Proje"].Value,
                };
            }

            return null;
        }
    }
}