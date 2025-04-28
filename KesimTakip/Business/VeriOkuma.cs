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
        public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) AjanOku(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Textbox cannot be empty");
            }

            List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
            List<string> invalidData = new List<string>();

            string genelPattern = @"(?i)ST\d+.*?AD\s*-\s*\d{5}\.\d{2}";

            string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

            MatchCollection matches = Regex.Matches(text, genelPattern);

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();
                Match validMatch = Regex.Match(value, validPattern);
                if (validMatch.Success)
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
                else
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

            string genelPattern = @"(?i)ST\d+.*?AD\s*-\s*\d{5}\.\d{2}";

            string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

            MatchCollection matches = Regex.Matches(text, genelPattern);

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();
                Match validMatch = Regex.Match(value, validPattern);
                if (validMatch.Success)
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
                else
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
        public class SayfaBilgisi
        {
            public string SayfaNumarasi { get; set; }
            public string TekrarSayisi { get; set; }
        }

        public List<SayfaBilgisi> ProcessPdfPages(string pdfText)
        {
            List<SayfaBilgisi> sayfaBilgileri = new List<SayfaBilgisi>();

            List<string> pdfPages = SplitPages(pdfText);

            foreach (string pageText in pdfPages)
            {
                if (pageText.Contains("FirmaAdı"))
                {
                    string sayfaPattern = @"Sayfa:(\d+)";
                    Regex sayfaRegex = new Regex(sayfaPattern);
                    Match sayfaMatch = sayfaRegex.Match(pageText);
                    string sayfaNumarasi = sayfaMatch.Success ? sayfaMatch.Groups[1].Value : "Bilinmiyor";

                    string tekrarSayisi = "Yok";
                    string tekrarPattern = @"LevhaÖlçüleri\s+\S+\s+(\d+)\s+";
                    Regex tekrarRegex = new Regex(tekrarPattern);
                    Match tekrarMatch = tekrarRegex.Match(pageText);

                    if (tekrarMatch.Success)
                    {
                        tekrarSayisi = tekrarMatch.Groups[1].Value;
                    }

                    sayfaBilgileri.Add(new SayfaBilgisi
                    {
                        SayfaNumarasi = sayfaNumarasi,
                        TekrarSayisi = tekrarSayisi
                    });
                }
            }

            return sayfaBilgileri;
        }

        private List<string> SplitPages(string pdfText)
        {
            List<string> pdfPages = new List<string>();

            string[] pageSeparators = new string[] { "-------- Sayfa" };
            string[] pages = pdfText.Split(pageSeparators, StringSplitOptions.None);

            foreach (string pageContent in pages)
            {
                string trimmedContent = pageContent.Trim();
                if (!string.IsNullOrEmpty(trimmedContent) && !trimmedContent.StartsWith("Toplam sayfa sayısı"))
                {
                    pdfPages.Add(trimmedContent);
                }
            }

            return pdfPages;
        }
    }
}