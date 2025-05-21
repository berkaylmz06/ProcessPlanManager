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
                            Malzeme = validMatch.Groups[2].Value,
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
                            Malzeme = validMatch.Groups[2].Value,
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
                @"ST(?<Kalite>\d{2})[-\s]*(?<Malzeme>\d+(?:MM|mm))[-\s]*(?<Kalıp>\d{3}-\d{2})[-\s]*(?<Poz>P\d{1,3})[-\s]*(?<Adet>\d+[A-Za-z]{2})[-\s]*(?<Proje>\d{5}\.\d{2})",
                RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return new MalzemeBilgisi
                {
                    Kalite = $"ST{match.Groups["Kalite"].Value}",
                    Malzeme = match.Groups["Malzeme"].Value,
                    Kalip = match.Groups["Kalıp"].Value,
                    Poz = match.Groups["Poz"].Value,
                    Adet = match.Groups["Adet"].Value,
                    Proje = match.Groups["Proje"].Value,
                };
            }

            return null;
        }
    }
}