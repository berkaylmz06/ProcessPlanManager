using KesimTakip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ProNestParser
{
    public class ProNestParserService
    {
        //public (List<PartInfo> Parts, List<PageInfo> Pages) ParseProNestData(string text1, string text4, string baseId, List<SayfaBilgisi> sayfaBilgileri, out string logFilePath)
        //{
        //    logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProNest_Log.txt"); File.WriteAllText(logFilePath, "ProNest Parser Log\n=================\n");

        //    List<PartInfo> partInfoList = new List<PartInfo>();
        //    List<PageInfo> pageInfoList = new List<PageInfo>();
        //    string[] lines1 = text1.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        //    string[] lines4 = text4.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        //    File.AppendAllText(logFilePath, $"RichTextBox1 satır: {lines1.Length}, RichTextBox4 satır: {lines4.Length}\n");

        //    // Pozları sayfa ID'si ve poz sıra numarasıyla sakla
        //    Dictionary<string, (string Poz, string Page)> pozWithIds = new Dictionary<string, (string, string)>();
        //    string currentPage = "";
        //    bool isValidPage = false;
        //    int pozCounter = 1;

        //    // richTextBox4 için pozları ve sayfa bilgilerini al
        //    foreach (string line in lines4)
        //    {
        //        if (string.IsNullOrWhiteSpace(line)) continue;

        //        if (line.Contains("Sayfa:"))
        //        {
        //            var pageMatch = Regex.Match(line, @"Sayfa:(\d+)");
        //            if (pageMatch.Success)
        //            {
        //                currentPage = pageMatch.Groups[1].Value;
        //                pozCounter = 1; // Yeni sayfa için sıfırla

        //                // Sayfa bilgisini ekle
        //                string pageId = $"{baseId}-{currentPage}";
        //                var sayfaBilgisi = sayfaBilgileri.Find(s => s.SayfaNumarasi == currentPage);
        //                string tekrarSayisi = sayfaBilgisi != null ? sayfaBilgisi.TekrarSayisi : "Bilinmiyor";
        //                pageInfoList.Add(new PageInfo
        //                {
        //                    Id = pageId,
        //                    Sayfa = currentPage,
        //                    ToplamSayfaTekrari = tekrarSayisi
        //                });

        //                File.AppendAllText(logFilePath, $"Sayfa tespit edildi (richTextBox4): {currentPage}, ID: {pageId}, Tekrar: {tekrarSayisi}\n");
        //            }
        //            continue;
        //        }

        //        if (line.Trim().StartsWith("ToplamParçaKesmeListesi"))
        //        {
        //            isValidPage = false;
        //            File.AppendAllText(logFilePath, "ToplamParçaKesmeListesi atlandı (richTextBox4).\n");
        //            continue;
        //        }

        //        if (line.Trim().StartsWith("FirmaAdı"))
        //        {
        //            isValidPage = true;
        //            File.AppendAllText(logFilePath, "FirmaAdı sayfası başladı (richTextBox4).\n");
        //            continue;
        //        }

        //        if (isValidPage)
        //        {
        //            var match = Regex.Match(line, @"(?i)(ST\d+)\s*-\s*(\d+(?:MM|mm))\s*-\s*(\d+-\d+)\s*-\s*(P\d+)\s*-\s*(\d+AD)\s*-\s*(\d{5}\.\d{2})(?:[A-Z]*)?");
        //            if (match.Success)
        //            {
        //                string partCode = match.Groups[3].Value;
        //                string pozNum = match.Groups[4].Value;
        //                string ad = match.Groups[5].Value;
        //                string not1 = match.Groups[6].Value;
        //                string poz = $"{partCode}-{pozNum}-{ad}-{not1}";
        //                string pozId = $"{baseId}-{currentPage}-{pozCounter}";

        //                pozWithIds[pozId] = (poz, currentPage);
        //                File.AppendAllText(logFilePath, $"Poz bulundu (richTextBox4): {poz}, Sayfa: {currentPage}, Satır: {line}, ID: {pozId}\n");
        //                pozCounter++;
        //            }
        //            else
        //            {
        //                File.AppendAllText(logFilePath, $"Eşleşmeyen satır (richTextBox4): {line}\n");
        //            }
        //        }
        //    }

        //    // richTextBox1 için adetleri al
        //    currentPage = "";
        //    isValidPage = false;
        //    pozCounter = 1;
        //    foreach (string line in lines1)
        //    {
        //        if (string.IsNullOrWhiteSpace(line)) continue;

        //        if (line.Contains("Sayfa:"))
        //        {
        //            var pageMatch = Regex.Match(line, @"Sayfa:(\d+)");
        //            if (pageMatch.Success)
        //            {
        //                currentPage = pageMatch.Groups[1].Value;
        //                pozCounter = 1;
        //                File.AppendAllText(logFilePath, $"Sayfa tespit edildi (richTextBox1): {currentPage}, ID: {baseId}-{currentPage}\n");
        //            }
        //            continue;
        //        }

        //        if (line.Trim().StartsWith("ToplamParçaKesmeListesi"))
        //        {
        //            isValidPage = false;
        //            File.AppendAllText(logFilePath, "ToplamParçaKesmeListesi atlandı (richTextBox1).\n");
        //            continue;
        //        }

        //        if (line.Trim().StartsWith("FirmaAdı"))
        //        {
        //            isValidPage = true;
        //            File.AppendAllText(logFilePath, "FirmaAdı sayfası başladı (richTextBox1).\n");
        //            continue;
        //        }

        //        if (isValidPage)
        //        {
        //            var match = Regex.Match(line, @"^\s*\d+\s+.*?0:00:\d{2}\s+(\d+)\s+.*?$");
        //            if (match.Success)
        //            {
        //                string pozId = $"{baseId}-{currentPage}-{pozCounter}";
        //                if (pozWithIds.ContainsKey(pozId))
        //                {
        //                    int count = int.Parse(match.Groups[1].Value);
        //                    var (poz, page) = pozWithIds[pozId];
        //                    partInfoList.Add(new PartInfo
        //                    {
        //                        Poz = poz,
        //                        Page = page,
        //                        Count = count
        //                    });
        //                    File.AppendAllText(logFilePath, $"Adet bulundu (richTextBox1): Poz: {poz}, Sayfa: {page}, Adet: {count}, Satır: {line}, ID: {pozId}\n");
        //                    pozCounter++;
        //                }
        //                else
        //                {
        //                    File.AppendAllText(logFilePath, $"Adet bulundu ama poz ID eşleşmedi (richTextBox1): Satır: {line}, ID: {pozId}\n");
        //                }
        //            }
        //            else
        //            {
        //                File.AppendAllText(logFilePath, $"Eşleşmeyen satır (richTextBox1): {line}\n");
        //            }
        //        }
        //    }

        //    return (partInfoList, pageInfoList);
        //}
    }

}