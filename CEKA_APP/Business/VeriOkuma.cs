using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static CEKA_APP.frmAnaSayfa;

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

            string validPattern = @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

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

            string validPattern = @"(?i)(ST\d+)\s*-\s*([A-Za-z0-9]+(?:[X-][A-Za-z0-9]+)*)\s*-\s*(\d+-\d+(?:-\d+)*)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})";

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