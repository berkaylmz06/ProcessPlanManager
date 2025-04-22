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
                // Parça verilerini çekme
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

                    // Sayısal değerlerin doğruluğunu kontrol et
                    if (!ValidateNumericValues(partQuantity, partLength, partWeight))
                    {
                        throw new FormatException($"Geçersiz sayısal veri bulundu (Parça: {partNumber}).");
                    }

                    // Parça verilerini MalzemeBilgisi nesnesine dönüştür
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

        public List<MalzemeBilgisi> BaykalOku(string pdfText)
        {
            var parsedDataList = new List<MalzemeBilgisi>();
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

                        HashSet<string> seenMaterials = new HashSet<string>();

                        foreach (Match match in materialMatches)
                        {
                            string malzeme = match.Groups[3].Value;
                            string fullMaterial = $"{malzeme} AD";

                            if (!seenMaterials.Contains(fullMaterial))
                            {
                                seenMaterials.Add(fullMaterial);
                                parsedDataList.Add(ParseLine(fullMaterial));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Pdf okunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Baykal veri ayrıştırma hatası: {ex.Message}", ex);

                }
            }
            else
            {
                MessageBox.Show("PDF okunamadı. Lütfen seçilen makinayı gözden geçiriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return parsedDataList;
        }
        public List<MalzemeBilgisi> AjanOku(string pdfText)
        {

            var parsedDataList = new List<MalzemeBilgisi>();
            string[] firstFourLines = pdfText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Take(4).ToArray();
            string firstFourLinesText = string.Join("\n", firstFourLines);


            if (firstFourLinesText.Contains("FirmaAdı"))
            {
                try
                {
                    string pattern = @"(\d+)\s(ST\d+)-(\d+mm)-(\d+-\d+)-(\w+)-(\w+)\s([\dX]+)\s([\d\.]+)\s([\d\:\.]+)\s(\d+)\s([\d\.]+)|(\d+)\s(ST\d+)-(\d+mm)-(\d+-\d+)-(\w+)-(\w+)\s([\dX]+)\s([\d\.]+)\s([\d\:\.]+)\s(\d+)\s([\d\.]+)\s(\d+-\d+)\s+([\d\.]+)";
                    Regex regex = new Regex(pattern);
                    MatchCollection matches = regex.Matches(pdfText);

                    HashSet<string> extractedData = new HashSet<string>();

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
                                parsedDataList.Add(ParseLine(result));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"PDF ayrıştırma hatası (FirmaAdı formatı): {ex.Message}");
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
                            parsedDataList.Add(ParseLine(matchValue));
                        }
                    }
                    else
                    {
                        throw new Exception("Pdf okunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"PDF ayrıştırma hatası (ToplamParçaKesmeListesi formatı): {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("PDF okunamadı. Lütfen seçilen makinayı gözden geçiriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return parsedDataList;
        }

        private MalzemeBilgisi ParseLine(string line)
        {
            var match = Regex.Match(line, @"(?<Kalite>[A-Za-z0-9]+)\s*-\s*(?<Kalınlık>[A-Za-z0-9]+)\s*-\s*(?<Kalıp>[A-Za-z0-9-]+)\s*-\s*(?<Poz>[A-Za-z0-9]+)\s*-\s*(?<Adet>[0-9]+)");

            if (match.Success)
            {
                return new MalzemeBilgisi
                {
                    Kalite = match.Groups["Kalite"].Value,
                    Kalınlık = match.Groups["Kalınlık"].Value,
                    Kalıp = match.Groups["Kalıp"].Value,
                    Poz = match.Groups["Poz"].Value,
                    Adet = match.Groups["Adet"].Value
                };
            }

            return null;
        }
    }
}