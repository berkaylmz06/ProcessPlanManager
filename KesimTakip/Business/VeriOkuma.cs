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
            var parsedDataList = new List<MalzemeBilgisi>();
            var invalidDataSet = new HashSet<string>(); // Benzersiz geçersiz veriler için HashSet

            try
            {
                string generalPattern = @"ST\d{2}[^\n\r]+";
                Regex generalRegex = new Regex(generalPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection allMatches = generalRegex.Matches(pdfText);

                HashSet<string> extractedData = new HashSet<string>();

                foreach (Match match in allMatches)
                {
                    string line = match.Value.Trim();

                    int dashCount = line.Count(c => c == '-');

                    if (dashCount < 4)
                    {
                        invalidDataSet.Add(line); // Benzersiz şekilde ekle
                        continue;
                    }

                    var parsed = ParseLine(line);
                    if (parsed != null)
                    {
                        if (extractedData.Add(line))
                        {
                            parsedDataList.Add(parsed);
                        }
                    }
                    else
                    {
                        invalidDataSet.Add(line); // Benzersiz şekilde ekle
                    }
                }

                if (!parsedDataList.Any() && !invalidDataSet.Any())
                {
                    string debugInfo = $"Yakalanan satır yok. PDF içeriğinin bir kısmını kontrol edin:\n" +
                                      pdfText.Substring(0, Math.Min(500, pdfText.Length)).Replace("\n", "\\n");
                    throw new Exception(debugInfo);
                }
                else if (!parsedDataList.Any() && invalidDataSet.Any())
                {
                    string debugInfo = $"Hiçbir veri formata uymadı. Yakalanan {invalidDataSet.Count} satırdan bir örnek: " +
                                      $"{invalidDataSet.FirstOrDefault() ?? "Yok"}\n" +
                                      $"PDF içeriğinin bir kısmını kontrol edin:\n" +
                                      pdfText.Substring(0, Math.Min(500, pdfText.Length)).Replace("\n", "\\n");
                    throw new Exception(debugInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF ayrıştırma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (parsedDataList, invalidDataSet.ToList()); // HashSet'i List'e çevir
        }

        public (List<MalzemeBilgisi> ParsedData, List<string> InvalidData) BaykalOku(string pdfText)
        {
            var parsedDataList = new List<MalzemeBilgisi>();
            var invalidDataSet = new HashSet<string>(); // Benzersiz geçersiz veriler için HashSet

            try
            {
                string generalPattern = @"ST\d{2}[-\s][^\n\r]+";
                Regex generalRegex = new Regex(generalPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection allMatches = generalRegex.Matches(pdfText);

                HashSet<string> extractedData = new HashSet<string>();

                foreach (Match match in allMatches)
                {
                    string line = match.Value.Trim();

                    int dashCount = line.Count(c => c == '-');

                    if (dashCount < 4)
                    {
                        invalidDataSet.Add(line); // Benzersiz şekilde ekle
                        continue;
                    }

                    if (dashCount >= 4 && dashCount <= 5)
                    {
                        var parsed = ParseLine(line);
                        if (parsed != null)
                        {
                            if (extractedData.Add(line))
                            {
                                parsedDataList.Add(parsed);
                            }
                        }
                        else
                        {
                            invalidDataSet.Add(line); // Benzersiz şekilde ekle
                        }
                    }
                    else if (dashCount >= 6)
                    {
                        var parsed = ParseLine(line);
                        if (parsed != null)
                        {
                            if (extractedData.Add(line))
                            {
                                parsedDataList.Add(parsed);
                            }
                        }
                        else
                        {
                            invalidDataSet.Add(line); // Benzersiz şekilde ekle
                        }
                    }
                }

                if (!parsedDataList.Any() && !invalidDataSet.Any())
                {
                    string debugInfo = $"Yakalanan satır yok. PDF içeriğinin bir kısmını kontrol edin:\n" +
                                      pdfText.Substring(0, Math.Min(500, pdfText.Length)).Replace("\n", "\\n");
                    throw new Exception(debugInfo);
                }
                else if (!parsedDataList.Any() && invalidDataSet.Any())
                {
                    string debugInfo = $"Hiçbir veri formata uymadı. Yakalanan {invalidDataSet.Count} satırdan bir örnek: " +
                                      $"{invalidDataSet.FirstOrDefault() ?? "Yok"}\n" +
                                      $"PDF içeriğinin bir kısmını kontrol edin:\n" +
                                      pdfText.Substring(0, Math.Min(500, pdfText.Length)).Replace("\n", "\\n");
                    throw new Exception(debugInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Baykal veri ayrıştırma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (parsedDataList, invalidDataSet.ToList()); // HashSet'i List'e çevir
        }

        private MalzemeBilgisi ParseLine(string line)
        {
            var match = Regex.Match(line,
                @"ST(?<Kalite>\d{2})[-\s]*(?<Kalınlık>\d+(?:MM|mm))[-\s]*(?<Kalıp>\d{3}-\d{2})[-\s]*(?<Poz>P\d{1,3})[-\s]*(?<Adet>\d+\s?[A-Za-z]{2})(?:[-\s]*(?<Proje>\d{5}.\d{1,2}))?",
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