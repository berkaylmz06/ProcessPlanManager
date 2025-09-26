using CEKA_APP.Abstracts.ERP;
using CEKA_APP.Abstracts.Genel;
using CEKA_APP.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEKA_APP.Concretes.KesimTakip
{
    public class KesimDetaylariRepository : IKesimDetaylariRepository
    {
        private readonly IAutoCadAktarimRepository _autoCadAktarimRepository;

        public KesimDetaylariRepository(IAutoCadAktarimRepository autoCadAktarimRepository)
        {
            _autoCadAktarimRepository = autoCadAktarimRepository ?? throw new ArgumentNullException(nameof(autoCadAktarimRepository));
        }
        public void SaveKesimDetaylariData(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet, bool ekBilgi)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string checkQuery = @"SELECT COUNT(*) FROM KesimDetaylari 
                  WHERE kalite = @kalite AND malzeme = @malzeme 
                  AND malzemeKod = @malzemeKod AND proje = @proje
                  AND ekBilgi = @ekBilgi";

            using (var checkCmd = new SqlCommand(checkQuery, connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@kalite", kalite);
                checkCmd.Parameters.AddWithValue("@malzeme", malzeme);
                checkCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                checkCmd.Parameters.AddWithValue("@proje", proje);
                checkCmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    string insertQuery = @"INSERT INTO KesimDetaylari 
                           (kalite, malzeme, malzemeKod, proje, kesilecekAdet, toplamAdet, ekBilgi) 
                           VALUES 
                           (@kalite, @malzeme, @malzemeKod, @proje, @kesilecekAdet, @toplamAdet, @ekBilgi)";

                    using (var insertCmd = new SqlCommand(insertQuery, connection, transaction))
                    {
                        insertCmd.Parameters.AddWithValue("@kalite", kalite);
                        insertCmd.Parameters.AddWithValue("@malzeme", malzeme);
                        insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        insertCmd.Parameters.AddWithValue("@proje", proje);
                        insertCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                        insertCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);
                        insertCmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                        insertCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string updateQuery = @"UPDATE KesimDetaylari 
                           SET kesilecekAdet = kesilecekAdet + @kesilecekAdet, 
                               toplamAdet = toplamAdet + @toplamAdet
                           WHERE kalite = @kalite AND malzeme = @malzeme 
                           AND malzemeKod = @malzemeKod AND proje = @proje
                           AND ekBilgi = @ekBilgi";

                    using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@kalite", kalite);
                        updateCmd.Parameters.AddWithValue("@malzeme", malzeme);
                        updateCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        updateCmd.Parameters.AddWithValue("@proje", proje);
                        updateCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                        updateCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);
                        updateCmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public DataTable GetKesimDetaylariBilgi(SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = "SELECT kalite, malzeme, malzemeKod, proje, kesilmisAdet, kesilecekAdet, toplamAdet, ekBilgi FROM KesimDetaylari";

            using (var adapter = new SqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public bool PozExists(SqlConnection connection, string kalite, string malzeme, string malzemekod, string proje)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            string query = @"
        SELECT COUNT(*) 
        FROM KesimDetaylari 
        WHERE kalite = @kalite 
          AND malzeme = @malzeme 
          AND malzemekod = @malzemekod 
          AND proje = @proje";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@kalite", kalite);
                command.Parameters.AddWithValue("@malzeme", malzeme);
                command.Parameters.AddWithValue("@malzemekod", malzemekod);
                command.Parameters.AddWithValue("@proje", proje);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public bool UpdateKesilmisAdet(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string malzemekod, string proje, decimal sondurum)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string query = @"
        UPDATE KesimDetaylari 
        SET kesilmisAdet = kesilmisAdet + @sondurum, 
            kesilecekAdet = kesilecekAdet - @sondurum 
        WHERE kalite = @kalite 
          AND malzeme = @malzeme 
          AND malzemekod = @malzemekod 
          AND proje = @proje 
          AND kesilecekAdet >= @sondurum";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@kalite", kalite);
                command.Parameters.AddWithValue("@malzeme", malzeme);
                command.Parameters.AddWithValue("@malzemekod", malzemekod);
                command.Parameters.AddWithValue("@proje", proje);
                command.Parameters.AddWithValue("@sondurum", sondurum);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public List<KesimDetaylari> GetKesimDetaylariBilgileri(SqlConnection connection, string projeAdi = null, string grupAdi = null)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            List<KesimDetaylari> detaylar = new List<KesimDetaylari>();
            List<(string kalite, string malzeme, string malzemeKod, string projeAdi, decimal toplamKesilmisAdet, decimal toplamAdet, bool ekBilgi, string grupAdi)> tempList = new List<(string, string, string, string, decimal, decimal, bool, string)>();

            string query = @"
SELECT
    kd.kalite,
    kd.malzeme,
    kd.malzemeKod,
    kd.proje AS projeAdi,
    SUM(kd.kesilmisAdet) AS toplamKesilmisAdet,
    ISNULL((
        SELECT SUM(m2.adet * COALESCE(g2.takimCarpani, 1) * COALESCE(ug2.takimCarpani, 1))
        FROM Malzemeler m2
        INNER JOIN Gruplar g2 ON m2.grupId = g2.grupId
        INNER JOIN Projeler p2 ON g2.projeID = p2.projeID
        LEFT JOIN UstGruplar ug2 ON g2.ustGrupID = ug2.ustGrupID
        WHERE p2.projeAdi = kd.proje 
          AND m2.malzemeKod = kd.malzemeKod
    ), 0) AS ToplamAdet,
    CAST(kd.ekBilgi AS BIT) AS ekBilgi
FROM
    KesimDetaylari AS kd
WHERE
    1=1
    AND (@projeAdi IS NULL OR kd.proje = @projeAdi)
    AND EXISTS (
        SELECT 1 
        FROM Malzemeler m3
        INNER JOIN Gruplar g3 ON m3.grupId = g3.grupId
        WHERE m3.malzemeKod = kd.malzemeKod 
          AND (@grupAdi IS NULL OR g3.grupAdi = @grupAdi)
    )
GROUP BY
    kd.kalite,
    kd.malzeme,
    kd.malzemeKod,
    kd.proje,
    kd.ekBilgi;";

            bool shouldCloseConnection = connection.State != ConnectionState.Open;
            if (shouldCloseConnection)
            {
                connection.Open();
            }

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@projeAdi", projeAdi?.Trim().ToLowerInvariant() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@grupAdi", grupAdi?.Trim().ToLowerInvariant() ?? (object)DBNull.Value);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string kalite = reader["kalite"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            string malzeme = reader["malzeme"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            string malzemeKod = reader["malzemeKod"]?.ToString()?.Trim().ToLowerInvariant() ?? "";
                            string projeAdiFromDb = reader["projeAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            decimal toplamKesilmisAdet = reader["toplamKesilmisAdet"] != DBNull.Value ? Convert.ToDecimal(reader["toplamKesilmisAdet"]) : 0m;
                            decimal toplamAdet = reader["ToplamAdet"] != DBNull.Value ? Convert.ToDecimal(reader["ToplamAdet"]) : 0m;
                            bool ekBilgi = reader["ekBilgi"] != DBNull.Value && Convert.ToBoolean(reader["ekBilgi"]);

                            string grupAdiFromDb = grupAdi?.Trim().ToLowerInvariant() ?? "bilinmeyen_grup";

                            if (toplamKesilmisAdet > toplamAdet && toplamAdet > 0)
                            {
                                toplamKesilmisAdet = toplamAdet;
                            }
                            tempList.Add((kalite, malzeme, malzemeKod, projeAdiFromDb, toplamKesilmisAdet, toplamAdet, ekBilgi, grupAdiFromDb));
                        }
                    }
                }

                var uniqueKalipKodlari = new HashSet<string>();
                foreach (var item in tempList)
                {
                    string[] parts = item.malzemeKod.Split('-');
                    if (parts.Length >= 3)
                    {
                        uniqueKalipKodlari.Add($"{parts[0]}-{parts[1]}");
                    }
                }

                var standartGruplarCache = new Dictionary<string, bool>();
                foreach (string kalipKodu in uniqueKalipKodlari)
                {
                    try
                    {
                        bool isStandart = _autoCadAktarimRepository.GetirStandartGruplar(connection, kalipKodu);
                        standartGruplarCache[kalipKodu] = isStandart;
                    }
                    catch
                    {
                        standartGruplarCache[kalipKodu] = false;
                    }
                }

                foreach (var item in tempList)
                {
                    string malzemeKod = item.malzemeKod;
                    string[] parts = malzemeKod.Split('-');

                    if (parts.Length >= 3)
                    {
                        string kalipKodu = $"{parts[0]}-{parts[1]}";
                        if (standartGruplarCache.TryGetValue(kalipKodu, out bool isStandart) && !isStandart)
                        {
                            parts[1] = "00";
                            malzemeKod = string.Join("-", parts);
                        }
                    }
                    malzemeKod = malzemeKod.ToLowerInvariant();

                    string key = $"{item.kalite}_{item.malzeme}_{malzemeKod}_{item.projeAdi}_bilinmeyen_grup";

                    detaylar.Add(new KesimDetaylari
                    {
                        Key = key,
                        kesilmisAdet = item.toplamKesilmisAdet,
                        toplamAdet = item.toplamAdet,
                        ekBilgi = item.ekBilgi
                    });
                }
            stopwatch.Stop();
            return detaylar;
        }

        //public List<KesimDetaylari> GetKesimDetaylariBilgileri(SqlConnection connection, string projeAdi = null, string grupAdi = null)
        //{
        //    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        //    if (connection == null)
        //        throw new ArgumentNullException(nameof(connection));

        //    List<KesimDetaylari> detaylar = new List<KesimDetaylari>();
        //    List<(string kalite, string malzeme, string malzemeKod, string projeAdi, decimal toplamKesilmisAdet, decimal toplamAdet, bool ekBilgi, string grupAdi)> tempList = new List<(string, string, string, string, decimal, decimal, bool, string)>();

        //    // *** YENİ BASİT SORGUNUZ ***
        //    string query = @"
        //SELECT
        //    kd.kalite,
        //    kd.malzeme,
        //    kd.malzemeKod,
        //    kd.proje AS projeAdi,
        //    SUM(kd.kesilmisAdet) AS toplamKesilmisAdet,
        //    ISNULL((
        //        SELECT SUM(m2.adet * COALESCE(g2.takimCarpani, 1) * COALESCE(ug2.takimCarpani, 1))
        //        FROM Malzemeler m2
        //        INNER JOIN Gruplar g2 ON m2.grupId = g2.grupId
        //        INNER JOIN Projeler p2 ON g2.projeID = p2.projeID
        //        LEFT JOIN UstGruplar ug2 ON g2.ustGrupID = ug2.ustGrupID
        //        WHERE p2.projeAdi = kd.proje 
        //          AND m2.malzemeKod = kd.malzemeKod
        //    ), 0) AS ToplamAdet,
        //    CAST(kd.ekBilgi AS BIT) AS ekBilgi,
        //    'Bilinmeyen' AS grupAdi
        //FROM
        //    KesimDetaylari AS kd
        //WHERE
        //    1=1
        //    AND (@projeAdi IS NULL OR kd.proje = @projeAdi)
        //GROUP BY
        //    kd.kalite,
        //    kd.malzeme,
        //    kd.malzemeKod,
        //    kd.proje,
        //    kd.ekBilgi;";

        //    bool shouldCloseConnection = connection.State != ConnectionState.Open;
        //    if (shouldCloseConnection)
        //    {
        //        connection.Open();
        //    }

        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@projeAdi", projeAdi?.Trim().ToLowerInvariant() ?? (object)DBNull.Value);

        //            System.Diagnostics.Debug.WriteLine($"[DEBUG] SQL Parametresi - Proje: '{projeAdi?.Trim().ToLowerInvariant()}'");

        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string kalite = reader["kalite"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                    string malzeme = reader["malzeme"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                    string malzemeKod = reader["malzemeKod"]?.ToString()?.Trim().ToLowerInvariant() ?? "";
        //                    string projeAdiFromDb = reader["projeAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                    decimal toplamKesilmisAdet = reader["toplamKesilmisAdet"] != DBNull.Value ? Convert.ToDecimal(reader["toplamKesilmisAdet"]) : 0m;
        //                    decimal toplamAdet = reader["ToplamAdet"] != DBNull.Value ? Convert.ToDecimal(reader["ToplamAdet"]) : 0m;
        //                    bool ekBilgi = reader["ekBilgi"] != DBNull.Value && Convert.ToBoolean(reader["ekBilgi"]);
        //                    string grupAdiFromDb = reader["grupAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmeyen_grup";

        //                    // *** MANTIK KONTROLÜ ***
        //                    if (toplamKesilmisAdet > toplamAdet && toplamAdet > 0)
        //                    {
        //                        System.Diagnostics.Debug.WriteLine($"[UYARI] MANTIK HATASI: {malzemeKod} Kesim({toplamKesilmisAdet}) > Toplam({toplamAdet})");
        //                        toplamKesilmisAdet = toplamAdet; // Maksimum toplam kadar kesilmiş kabul et
        //                    }

        //                    System.Diagnostics.Debug.WriteLine($"[DEBUG] SONUÇ: {kalite} {malzeme} {malzemeKod} -> Kesim: {toplamKesilmisAdet}, Toplam: {toplamAdet}");

        //                    tempList.Add((kalite, malzeme, malzemeKod, projeAdiFromDb, toplamKesilmisAdet, toplamAdet, ekBilgi, grupAdiFromDb));
        //                }
        //            }

        //            System.Diagnostics.Debug.WriteLine($"[DEBUG] {tempList.Count} kayıt bulundu");
        //        }

        //        // *** NORMALİZASYON VE KEY OLUŞTURMA (MEVCUT MANTIK) ***
        //        var uniqueKalipKodlari = new HashSet<string>();
        //        foreach (var item in tempList)
        //        {
        //            string[] parts = item.malzemeKod.Split('-');
        //            if (parts.Length >= 3)
        //            {
        //                uniqueKalipKodlari.Add($"{parts[0]}-{parts[1]}");
        //            }
        //        }

        //        var standartGruplarCache = new Dictionary<string, bool>();
        //        foreach (string kalipKodu in uniqueKalipKodlari)
        //        {
        //            try
        //            {
        //                bool isStandart = _autoCadAktarimRepository.GetirStandartGruplar(connection, kalipKodu);
        //                standartGruplarCache[kalipKodu] = isStandart;
        //            }
        //            catch
        //            {
        //                standartGruplarCache[kalipKodu] = false;
        //            }
        //        }

        //        foreach (var item in tempList)
        //        {
        //            string malzemeKod = item.malzemeKod;
        //            string[] parts = malzemeKod.Split('-');

        //            if (parts.Length >= 3)
        //            {
        //                string kalipKodu = $"{parts[0]}-{parts[1]}";
        //                if (standartGruplarCache.TryGetValue(kalipKodu, out bool isStandart) && !isStandart)
        //                {
        //                    parts[1] = "00";
        //                    malzemeKod = string.Join("-", parts);
        //                    System.Diagnostics.Debug.WriteLine($"[DEBUG] NORMALİZASYON: {item.malzemeKod} -> {malzemeKod}");
        //                }
        //            }
        //            malzemeKod = malzemeKod.ToLowerInvariant();

        //            string key = $"{item.kalite}_{item.malzeme}_{malzemeKod}_{item.projeAdi}_bilinmeyen_grup";

        //            detaylar.Add(new KesimDetaylari
        //            {
        //                Key = key,
        //                kesilmisAdet = item.toplamKesilmisAdet,
        //                toplamAdet = item.toplamAdet,
        //                ekBilgi = item.ekBilgi
        //            });
        //        }

        //        System.Diagnostics.Debug.WriteLine($"[DEBUG] Toplam {detaylar.Count} adet KesimDetayi oluşturuldu");
        //    }
        //    finally
        //    {
        //        if (shouldCloseConnection && connection.State == ConnectionState.Open)
        //        {
        //            connection.Close();
        //        }
        //    }

        //    stopwatch.Stop();
        //    System.Diagnostics.Debug.WriteLine($"[PERF] GetKesimDetaylariBilgileri tamamlandı: {stopwatch.ElapsedMilliseconds}ms");
        //    return detaylar;
        //}
        public bool GuncelleKesimDetaylari(SqlConnection connection, SqlTransaction transaction, string kalite, string malzeme, string kalipNo, string kesilecekPozlar, string proje, decimal silinecekAdet = 0, bool tamSilme = false)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            string malzemeKod = $"{kalipNo}-{kesilecekPozlar}";

            string selectQuery = @"SELECT kesilecekAdet, toplamAdet FROM KesimDetaylari 
                           WHERE kalite = @kalite AND malzeme = @malzeme 
                           AND malzemeKod = @malzemeKod AND proje = @proje";

            using (var selectCmd = new SqlCommand(selectQuery, connection, transaction))
            {
                selectCmd.Parameters.AddWithValue("@kalite", kalite);
                selectCmd.Parameters.AddWithValue("@malzeme", malzeme);
                selectCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                selectCmd.Parameters.AddWithValue("@proje", proje);

                using (var reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal mevcutKesilecekAdet = reader.GetDecimal(0);
                        decimal mevcutToplamAdet = reader.GetDecimal(1);
                        reader.Close();

                        if (tamSilme)
                        {
                            string deleteQuery = @"DELETE FROM KesimDetaylari 
                                           WHERE kalite = @kalite AND malzeme = @malzeme 
                                           AND malzemeKod = @malzemeKod AND proje = @proje";
                            using (var deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCmd.Parameters.AddWithValue("@kalite", kalite);
                                deleteCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                deleteCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                deleteCmd.Parameters.AddWithValue("@proje", proje);

                                int rowsAffected = deleteCmd.ExecuteNonQuery();
                                return rowsAffected > 0;
                            }
                        }
                        else if (silinecekAdet > 0)
                        {
                            if (mevcutKesilecekAdet < silinecekAdet || mevcutToplamAdet < silinecekAdet)
                                return false;

                            string updateQuery = @"UPDATE KesimDetaylari 
                                           SET kesilecekAdet = kesilecekAdet - @silinecekAdet, 
                                               toplamAdet = toplamAdet - @silinecekAdet
                                           WHERE kalite = @kalite AND malzeme = @malzeme 
                                           AND malzemeKod = @malzemeKod AND proje = @proje";

                            using (var updateCmd = new SqlCommand(updateQuery, connection, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@kalite", kalite);
                                updateCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                updateCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                updateCmd.Parameters.AddWithValue("@proje", proje);
                                var p = updateCmd.Parameters.Add("@silinecekAdet", SqlDbType.Decimal);
                                p.Precision = 18;
                                p.Scale = 2;
                                p.Value = silinecekAdet;

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                return rowsAffected > 0;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }
                }
            }
        }
        public (decimal toplamAdet, decimal kesilmisAdet, decimal kesilecekAdet, List<string> eslesenPozlar) GetAdetlerVeEslesenPozlar(SqlConnection connection, string kalite, string malzeme, string proje, string malzemeKodIlkKisim, string malzemeKodUcuncuKisim)
        {
            string query = @"
        SELECT malzemeKod, toplamAdet, kesilmisAdet, kesilecekAdet
        FROM KesimDetaylari
        WHERE kalite = @kalite
          AND malzeme = @malzeme
          AND proje = @proje
          AND malzemeKod LIKE @malzemeKodIlkKisim + '-[0-9][0-9]-' + @malzemeKodUcuncuKisim
          AND ekBilgi = 1";

            using (var cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@kalite", kalite);
                cmd.Parameters.AddWithValue("@malzeme", malzeme);
                cmd.Parameters.AddWithValue("@proje", proje);
                cmd.Parameters.AddWithValue("@malzemeKodIlkKisim", malzemeKodIlkKisim);
                cmd.Parameters.AddWithValue("@malzemeKodUcuncuKisim", malzemeKodUcuncuKisim);

                decimal toplamAdet = 0, kesilmisAdet = 0, kesilecekAdetSum = 0;
                List<string> eslesenPozlar = new List<string>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toplamAdet += reader["toplamAdet"] != DBNull.Value ? Convert.ToDecimal(reader["toplamAdet"]) : 0;
                        kesilmisAdet += reader["kesilmisAdet"] != DBNull.Value ? Convert.ToDecimal(reader["kesilmisAdet"]) : 0;
                        kesilecekAdetSum += reader["kesilecekAdet"] != DBNull.Value ? Convert.ToDecimal(reader["kesilecekAdet"]) : 0;
                        string malzemeKod = reader["malzemeKod"]?.ToString() ?? "";
                        eslesenPozlar.Add($"{kalite}-{malzeme}-{malzemeKod}-{proje}");
                    }
                }

                return (toplamAdet, kesilmisAdet, kesilecekAdetSum, eslesenPozlar);
            }
        }

    }
}
