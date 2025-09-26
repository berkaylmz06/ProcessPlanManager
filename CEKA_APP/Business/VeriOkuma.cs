using CEKA_APP.Entitys;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CEKA_APP.Business
{
    public class VeriOkuma
    {
        public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) PlazmaOku(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Okunacak veri yok!");
            }

            List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
            List<string> invalidData = new List<string>();
            HashSet<string> validDataSet = new HashSet<string>();
            HashSet<string> invalidDataSet = new HashSet<string>();

            // Regex ifadesi daha esnek hale getirildi. Parantez içindeki verileri görmezden gelecek.
            // Aynı zamanda -EK ve -EK1 gibi ifadeleri de doğru bir şekilde yakalayacak.
            string validPattern = @"(?i)(ST\d+)\s*-\s*([^-]+)\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2,})\s*(-EK)?(\d*).*";

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (invalidDataSet.Contains(trimmedLine) || validDataSet.Contains(trimmedLine))
                    continue;

                Match validMatch = Regex.Match(trimmedLine, validPattern);
                if (validMatch.Success)
                {
                    string proje = validMatch.Groups[6].Value;
                    string ekDurumu = "";

                    // Ek durumunu kontrol etme mantığı yenilendi.
                    // Regex'teki 7. grup "-EK" kısmını, 8. grup ise sayı kısmını yakalar.
                    string ekGrup = validMatch.Groups[7].Value;
                    string sayiGrup = validMatch.Groups[8].Value;

                    if (!string.IsNullOrEmpty(ekGrup))
                    {
                        if (string.IsNullOrEmpty(sayiGrup))
                        {
                            ekDurumu = "EK";
                        }
                        else
                        {
                            ekDurumu = "Ek Parça";
                        }
                    }

                    // Mükerrer veriyi kontrol eden anahtara ek durumu bilgisini de ekledim.
                    string uniqueKey = $"{validMatch.Groups[1].Value}|{validMatch.Groups[2].Value}|{validMatch.Groups[3].Value}|{validMatch.Groups[4].Value}|{validMatch.Groups[5].Value}|{proje}|{ekDurumu}";

                    if (validDataSet.Add(uniqueKey))
                    {
                        var malzeme = new MalzemeBilgisi
                        {
                            Kalite = validMatch.Groups[1].Value,
                            Malzeme = validMatch.Groups[2].Value,
                            Kalip = validMatch.Groups[3].Value,
                            Poz = validMatch.Groups[4].Value,
                            Adet = validMatch.Groups[5].Value,
                            Proje = proje,
                            EkDurumu = ekDurumu
                        };
                        validData.Add(malzeme);
                    }
                }
                else
                {
                    invalidDataSet.Add(trimmedLine);
                    invalidData.Add(trimmedLine);
                }
            }

            return (validData, invalidData);
        }

        //public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) PlazmaOku(string text)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        throw new ArgumentException("Okunacak veri yok!");
        //    }

        //    List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
        //    List<string> invalidData = new List<string>();
        //    HashSet<string> validDataSet = new HashSet<string>();
        //    HashSet<string> invalidDataSet = new HashSet<string>();

        //    string validPattern = @"(?i)(ST\d+)\s*-\s*([^-]+)\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2,}(?:-EK)?)";

        //    var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //    foreach (var line in lines)
        //    {
        //        string trimmedLine = line.Trim();
        //        if (invalidDataSet.Contains(trimmedLine) || validDataSet.Contains(trimmedLine))
        //            continue;

        //        Match validMatch = Regex.Match(trimmedLine, validPattern);
        //        if (validMatch.Success)
        //        {

        //            string proje = validMatch.Groups[6].Value;
        //            string ekDurumu = "";

        //            if (proje.EndsWith("-EK", StringComparison.OrdinalIgnoreCase))
        //            {
        //                ekDurumu = "EK";
        //                proje = proje.Replace("-EK", "");
        //            }

        //            string uniqueKey = $"{validMatch.Groups[1].Value}|{validMatch.Groups[2].Value}|{validMatch.Groups[3].Value}|{validMatch.Groups[4].Value}|{validMatch.Groups[5].Value}|{proje}";

        //            if (validDataSet.Add(uniqueKey))
        //            {
        //                var malzeme = new MalzemeBilgisi
        //                {
        //                    Kalite = validMatch.Groups[1].Value,
        //                    Malzeme = validMatch.Groups[2].Value,
        //                    Kalip = validMatch.Groups[3].Value,
        //                    Poz = validMatch.Groups[4].Value,
        //                    Adet = validMatch.Groups[5].Value,
        //                    Proje = proje,
        //                    EkDurumu = ekDurumu
        //                };
        //                validData.Add(malzeme);
        //            }
        //        }
        //        else
        //        {
        //            invalidDataSet.Add(trimmedLine);
        //            invalidData.Add(trimmedLine);
        //        }
        //    }

        //    return (validData, invalidData);
        //}



        //public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) PlazmaOku(string text)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //    {
        //        throw new ArgumentException("Okunacak veri yok!");
        //    }

        //    List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
        //    List<string> invalidData = new List<string>();
        //    HashSet<string> validDataSet = new HashSet<string>();
        //    HashSet<string> invalidDataSet = new HashSet<string>();

        //    //string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";
        //    //string validPattern = @"(?i)(ST\d+)\s*-\s*([^-]+)\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";
        //    string validPattern = @"(?i)(ST\d+)\s*-\s*([^-]+)\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2,})";

        //    var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //    foreach (var line in lines)
        //    {
        //        string trimmedLine = line.Trim();
        //        if (invalidDataSet.Contains(trimmedLine) || validDataSet.Contains(trimmedLine))
        //            continue;

        //        Match validMatch = Regex.Match(trimmedLine, validPattern);
        //        if (validMatch.Success)
        //        {
        //            string uniqueKey = $"{validMatch.Groups[1].Value}|{validMatch.Groups[2].Value}|{validMatch.Groups[3].Value}|{validMatch.Groups[4].Value}|{validMatch.Groups[5].Value}|{validMatch.Groups[6].Value}";

        //            if (validDataSet.Add(uniqueKey))
        //            {
        //                var malzeme = new MalzemeBilgisi
        //                {
        //                    Kalite = validMatch.Groups[1].Value,
        //                    Malzeme = validMatch.Groups[2].Value,
        //                    Kalip = validMatch.Groups[3].Value,
        //                    Poz = validMatch.Groups[4].Value,
        //                    Adet = validMatch.Groups[5].Value,
        //                    Proje = validMatch.Groups[6].Value
        //                };
        //                validData.Add(malzeme);
        //            }
        //        }
        //        else
        //        {
        //            invalidDataSet.Add(trimmedLine);
        //            invalidData.Add(trimmedLine);
        //        }
        //    }

        //    return (validData, invalidData);
        //}
        public (List<MalzemeBilgisi> ValidData, List<string> InvalidData) AdmOku(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Okunacak veri yok!");
            }

            List<MalzemeBilgisi> validData = new List<MalzemeBilgisi>();
            List<string> invalidData = new List<string>();
            HashSet<string> validDataSet = new HashSet<string>();
            HashSet<string> invalidDataSet = new HashSet<string>();

            string validPattern = @"(?i)(ST\d+)\s*-\s*([^-]+)\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2,})\s*(-EK)?(\d*).*";

            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (invalidDataSet.Contains(trimmedLine) || validDataSet.Contains(trimmedLine))
                    continue;

                Match validMatch = Regex.Match(trimmedLine, validPattern);
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
                else
                {
                    invalidDataSet.Add(trimmedLine);
                    invalidData.Add(trimmedLine);
                }
            }

            return (validData, invalidData);
        }
    }
}