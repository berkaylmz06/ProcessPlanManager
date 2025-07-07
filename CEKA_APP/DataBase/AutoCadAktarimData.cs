using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CEKA_APP.Entitys;

namespace CEKA_APP.DataBase
{
    public class AutoCadAktarimData
    {
        public static void SaveAutoCadData(string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    int projeId;
                    string projeQuery = "SELECT projeId FROM Projeler WHERE projeAdi = @projeAdi";
                    using (var cmd = new SqlCommand(projeQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            string insertProje = "INSERT INTO Projeler (projeAdi, olusturmaTarihi) VALUES (@projeAdi, @tarih); SELECT SCOPE_IDENTITY();";
                            using (var insertCmd = new SqlCommand(insertProje, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                                insertCmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                                projeId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            projeId = Convert.ToInt32(result);
                        }
                    }

                    int grupId;
                    string grupQuery = "SELECT grupId FROM Gruplar WHERE projeId = @projeId AND grupAdi = @grupAdi";
                    using (var cmd = new SqlCommand(grupQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@projeId", projeId);
                        cmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            string insertGrup = "INSERT INTO Gruplar (projeId, grupAdi) VALUES (@projeId, @grupAdi); SELECT SCOPE_IDENTITY();";
                            using (var insertCmd = new SqlCommand(insertGrup, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@projeId", projeId);
                                insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                grupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            grupId = Convert.ToInt32(result);
                        }
                    }

                    string insertMalzeme = @"
                INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite)
                VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite)";
                    using (var insertCmd = new SqlCommand(insertMalzeme, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@grupId", grupId);
                        insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        insertCmd.Parameters.AddWithValue("@adet", adet);
                        insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public static List<AutoCadAktarimDetay> GetAutoCadKayitlari(string projeAdi)
        {
            List<AutoCadAktarimDetay> list = new List<AutoCadAktarimDetay>();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"
            -- Malzemeli kayıtlar
            SELECT 
                p.projeAdi AS proje,
                g.grupAdi AS grup,
                m.malzemeKod
            FROM Malzemeler m
            JOIN Gruplar g ON m.grupId = g.grupId
            JOIN Projeler p ON g.projeId = p.projeId
            WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi)
            UNION
            -- Malzemesiz gruplar
            SELECT 
                p.projeAdi AS proje,
                g.grupAdi AS grup,
                NULL AS malzemeKod
            FROM Gruplar g
            JOIN Projeler p ON g.projeId = p.projeId
            LEFT JOIN Malzemeler m ON m.grupId = g.grupId
            WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND m.malzemeId IS NULL
            UNION
            -- Grup veya malzemesiz projeler
            SELECT 
                p.projeAdi AS proje,
                NULL AS grup,
                NULL AS malzemeKod
            FROM Projeler p
            LEFT JOIN Gruplar g ON g.projeId = p.projeId
            WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND g.grupId IS NULL
            ORDER BY proje, grup, malzemeKod";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@projeAdi", string.IsNullOrEmpty(projeAdi) ? (object)DBNull.Value : projeAdi);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new AutoCadAktarimDetay
                    {
                        Proje = reader["proje"].ToString(),
                        Grup = reader["grup"] != DBNull.Value ? reader["grup"].ToString() : null,
                        MalzemeKod = reader["malzemeKod"] != DBNull.Value ? reader["malzemeKod"].ToString() : null
                    });
                }
                reader.Close();
            }
            return list;
        }

        public static DataTable GruplariGetir(string projeAdi)
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
            SELECT DISTINCT p.projeAdi, g.grupAdi 
            FROM Gruplar g
            JOIN Projeler p ON g.projeId = p.projeId
            WHERE p.projeAdi = @projeAdi
            ORDER BY g.grupAdi";

                SqlCommand komut = new SqlCommand(sorgu, conn);
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                SqlDataAdapter adaptor = new SqlDataAdapter(komut);
                adaptor.Fill(tablo);
            }
            return tablo;
        }

        public static DataTable MalzemeleriGetir(string projeAdi, string grupAdi)
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
            SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite
            FROM Malzemeler m
            JOIN Gruplar g ON m.grupId = g.grupId
            JOIN Projeler p ON g.projeId = p.projeId
            WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi
            ORDER BY m.malzemeKod";

                SqlCommand komut = new SqlCommand(sorgu, conn);
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                SqlDataAdapter adaptor = new SqlDataAdapter(komut);
                adaptor.Fill(tablo);
            }
            return tablo;
        }

        public static DataTable MalzemeDetaylariniGetir(string projeAdi, string grupAdi, string malzemeKod)
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
            SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite
            FROM Malzemeler m
            JOIN Gruplar g ON m.grupId = g.grupId
            JOIN Projeler p ON g.projeId = p.projeId
            WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi AND m.malzemeKod = @malzemeKod";

                SqlCommand komut = new SqlCommand(sorgu, conn);
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                komut.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                SqlDataAdapter adaptor = new SqlDataAdapter(komut);
                adaptor.Fill(tablo);
            }
            return tablo;
        }

        public static void ProjeEkle(string projeAdi, string aciklama, DateTime olusturmaTarihi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
            INSERT INTO Projeler (projeAdi, olusturmaTarihi, aciklama)
            VALUES (@projeAdi, @olusturmaTarihi, @aciklama)";

                using (SqlCommand komut = new SqlCommand(sorgu, conn))
                {
                    komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                    komut.Parameters.AddWithValue("@olusturmaTarihi", olusturmaTarihi);
                    komut.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(aciklama) ? DBNull.Value : (object)aciklama);

                    komut.ExecuteNonQuery();
                }
            }
        }
        public static void StandartGrupEkle(string grupNo)
        {
            string sorgu = @"
        INSERT INTO StandartGruplar (grupNo, olusturmaTarihi)
        VALUES (@grupNo, GETDATE())";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var komut = new SqlCommand(sorgu, conn))
                {
                    komut.Parameters.AddWithValue("@grupNo", grupNo);
                    komut.ExecuteNonQuery();
                }
            }
        }
        public static List<string> GetirStandartGruplarListe()
        {
            var gruplar = new List<string>();

            string query = "SELECT grupNo FROM StandartGruplar ORDER BY grupNo";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var command = new SqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string grupNo = reader["grupNo"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(grupNo))
                            gruplar.Add(grupNo);
                    }
                }
            }

            return gruplar;
        }
        public static bool GetirStandartGruplar(string kalip)
        {
          
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM StandartGruplar WHERE grupNo = @grupNo";
                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@grupNo", kalip);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public static void StandartGrupSil(string grupNo)
        {
            string sorgu = "DELETE FROM StandartGruplar WHERE grupNo = @grupNo";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var komut = new SqlCommand(sorgu, conn))
                {
                    komut.Parameters.AddWithValue("@grupNo", grupNo);
                    komut.ExecuteNonQuery();
                }
            }
        }

        public static void GrupEkleGuncelle(string projeNo, string grupAdi, string eskiGrupAdi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                int projeId = GetProjeId(conn, projeNo);

                string checkQuery = "SELECT COUNT(*) FROM Gruplar WHERE projeId = @projeId AND grupAdi = @grupAdi";
                using (var checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@projeId", projeId);
                    checkCmd.Parameters.AddWithValue("@grupAdi", eskiGrupAdi ?? grupAdi);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0 && !string.IsNullOrEmpty(eskiGrupAdi))
                    {
                        string updateQuery = "UPDATE Gruplar SET grupAdi = @yeniGrupAdi WHERE projeId = @projeId AND grupAdi = @eskiGrupAdi";
                        using (var updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@yeniGrupAdi", grupAdi);
                            updateCmd.Parameters.AddWithValue("@projeId", projeId);
                            updateCmd.Parameters.AddWithValue("@eskiGrupAdi", eskiGrupAdi);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Gruplar (projeId, grupAdi) VALUES (@projeId, @grupAdi)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@projeId", projeId);
                            insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public static void GrupSil(string projeAdi, string grupAdi)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    string checkQuery = @"
                SELECT g.grupId
                FROM Gruplar g
                JOIN Projeler p ON g.projeId = p.projeId
                WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                        checkCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                        var result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception($"Grup bulunamadı: projeAdi={projeAdi}, grupAdi={grupAdi}");
                        }
                    }

                    string sorgu = @"
                DELETE m
                FROM Malzemeler m
                JOIN Gruplar g ON m.grupId = g.grupId
                JOIN Projeler p ON g.projeId = p.projeId
                WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi;

                DELETE g
                FROM Gruplar g
                JOIN Projeler p ON g.projeId = p.projeId
                WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";

                    using (var komut = new SqlCommand(sorgu, conn))
                    {
                        komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                        komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                        int rowsAffected = komut.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Grup silme hatası: {ex.Message}", ex);
                }
            }
        }
        public static void MalzemeEkleGuncelle(string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, string eskiMalzemeKod = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                // ProjeId ve GrupId'yi al
                int projeId = GetProjeId(conn, projeNo);
                int grupId = GetGrupId(conn, projeId, grupAdi);

                string checkQuery = "SELECT COUNT(*) FROM Malzemeler WHERE grupId = @grupId AND malzemeKod = @malzemeKod";
                using (var checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@grupId", grupId);
                    checkCmd.Parameters.AddWithValue("@malzemeKod", eskiMalzemeKod ?? malzemeKod);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0 && !string.IsNullOrEmpty(eskiMalzemeKod))
                    {
                        string updateQuery = @"UPDATE Malzemeler 
                                   SET malzemeKod = @yeniMalzemeKod, adet = @adet, malzemeAd = @malzemeAd, kalite = @kalite 
                                   WHERE grupId = @grupId AND malzemeKod = @eskiMalzemeKod";
                        using (var updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@yeniMalzemeKod", malzemeKod);
                            updateCmd.Parameters.AddWithValue("@adet", adet);
                            updateCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@grupId", grupId);
                            updateCmd.Parameters.AddWithValue("@eskiMalzemeKod", eskiMalzemeKod);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = @"INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite) 
                                   VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@grupId", grupId);
                            insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                            insertCmd.Parameters.AddWithValue("@adet", adet);
                            insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private static int GetProjeId(SqlConnection conn, string projeNo)
        {
            string query = "SELECT projeId FROM Projeler WHERE projeAdi = @projeAdi";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@projeAdi", projeNo);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception($"Proje bulunamadı: {projeNo}");
                }
                return Convert.ToInt32(result);
            }
        }

        private static int GetGrupId(SqlConnection conn, int projeId, string grupAdi)
        {
            string query = "SELECT grupId FROM Gruplar WHERE projeId = @projeId AND grupAdi = @grupAdi";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@projeId", projeId);
                cmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception($"Grup bulunamadı: projeId={projeId}, grupAdi={grupAdi}");
                }
                return Convert.ToInt32(result);
            }
        }
        public static void MalzemeSil(string projeAdi, string grupAdi, string malzemeKod)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string sorgu = @"
                    DELETE m
                    FROM Malzemeler m
                    JOIN Gruplar g ON m.grupId = g.grupId
                    JOIN Projeler p ON g.projeId = p.projeId
                    WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi AND m.malzemeKod = @malzemeKod";

                    using (var komut = new SqlCommand(sorgu, conn))
                    {
                        komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                        komut.Parameters.AddWithValue("@grupAdi", grupAdi);
                        komut.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        int rowsAffected = komut.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Malzeme silme hatası: {ex.Message}", ex);
                }
            }
        }
        public static (bool, int) KontrolAdet(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        {
            string query = @"
        SELECT SUM(m.adet) AS ToplamAdet
        FROM Malzemeler m
        JOIN Gruplar g ON m.grupID = g.grupID
        JOIN Projeler p ON g.projeID = p.projeID
        WHERE m.kalite = @Kalite 
          AND m.malzemeAd = @malzemeAd 
          AND m.malzemeKod = @malzemeKod 
          AND p.projeAdi = @ProjeAdi";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Kalite", kalite);
                    command.Parameters.AddWithValue("@malzemeAd", malzeme);
                    command.Parameters.AddWithValue("@malzemeKod", kalip);
                    command.Parameters.AddWithValue("@ProjeAdi", proje);

                    var result = command.ExecuteScalar();
                    int toplamAdet = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    return (girilenAdet <= toplamAdet, toplamAdet);
                }
            }
        }
        public static (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        {
            string[] kalipParcalari = kalip.Split('-');
            string originalKalip = kalip;

            if (kalipParcalari.Length >= 3 && GetirStandartGruplar(kalip))
            {
                kalip = $"{kalipParcalari[0]}-00-{kalipParcalari[2]}";
            }

            string query = @"
SELECT 
    SUM(m.adet) AS ToplamAdetIfs
FROM Malzemeler m
JOIN Gruplar g ON m.grupID = g.grupID
JOIN Projeler p ON g.projeID = p.projeID
WHERE m.kalite = @Kalite
  AND m.malzemeAd = @malzemeAd
  AND m.malzemeKod = @malzemeKod
  AND p.projeAdi = @ProjeAdi;

SELECT SUM(KD.toplamAdet) AS ToplamAdetYuklenen
FROM KesimDetaylari KD
WHERE KD.kalite = @Kalite
  AND KD.malzemeKod LIKE @malzemeKodLike
  AND KD.proje = @ProjeAdi";

            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                using (var command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Kalite", kalite);
                    command.Parameters.AddWithValue("@malzemeAd", malzeme);
                    command.Parameters.AddWithValue("@malzemeKod", kalip);
                    command.Parameters.AddWithValue("@ProjeAdi", proje);
                    command.Parameters.AddWithValue("@malzemeKodLike", $"{kalipParcalari[0]}-[0-9][0-9]-{kalipParcalari[2]}");
                    using (var reader = command.ExecuteReader())
                    {
                        int toplamAdetIfs = 0;
                        int toplamAdetYuklenen = 0;

                        if (reader.Read())
                        {
                            toplamAdetIfs = reader["ToplamAdetIfs"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetIfs"]) : 0;
                        }

                        reader.NextResult();

                        if (reader.Read())
                        {
                            toplamAdetYuklenen = reader["ToplamAdetYuklenen"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetYuklenen"]) : 0;
                        }

                        bool isValid = girilenAdet + toplamAdetYuklenen <= toplamAdetIfs;
                        return (isValid, toplamAdetIfs, toplamAdetYuklenen);
                    }
                }
            }
        }
        //        public static (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        //        {
        //            string[] kalipParcalari = kalip.Split('-');
        //            if (kalipParcalari.Length >= 3)
        //            {
        //                kalip = $"{kalipParcalari[0]}-00-{kalipParcalari[2]}";
        //            }

        //            string query = @"
        //SELECT 
        //    SUM(m.adet) AS ToplamAdetIfs
        //FROM Malzemeler m
        //JOIN Gruplar g ON m.grupID = g.grupID
        //JOIN Projeler p ON g.projeID = p.projeID
        //WHERE m.kalite = @Kalite
        //  AND m.malzemeAd = @malzemeAd
        //  AND m.malzemeKod = @malzemeKod
        //  AND p.projeAdi = @ProjeAdi;

        //SELECT SUM(KD.toplamAdet) AS ToplamAdetYuklenen
        //FROM KesimDetaylari KD
        //WHERE KD.kalite = @Kalite
        //  AND KD.malzemeKod LIKE @malzemeKodLike
        //  AND KD.proje = @ProjeAdi";

        //            using (var conn = DataBaseHelper.GetConnection())
        //            {
        //                conn.Open();
        //                using (var command = new SqlCommand(query, conn))
        //                {
        //                    command.Parameters.AddWithValue("@Kalite", kalite);
        //                    command.Parameters.AddWithValue("@malzemeAd", malzeme);
        //                    command.Parameters.AddWithValue("@malzemeKod", kalip);
        //                    command.Parameters.AddWithValue("@ProjeAdi", proje);
        //                    command.Parameters.AddWithValue("@malzemeKodLike", $"{kalipParcalari[0]}-[0-9][0-9]-{kalipParcalari[2]}"); 
        //                    using (var reader = command.ExecuteReader())
        //                    {
        //                        int toplamAdetIfs = 0;
        //                        int toplamAdetYuklenen = 0;

        //                        if (reader.Read())
        //                        {
        //                            toplamAdetIfs = reader["ToplamAdetIfs"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetIfs"]) : 0;
        //                        }

        //                        reader.NextResult();

        //                        if (reader.Read())
        //                        {
        //                            toplamAdetYuklenen = reader["ToplamAdetYuklenen"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetYuklenen"]) : 0;
        //                        }

        //                        bool isValid = girilenAdet + toplamAdetYuklenen <= toplamAdetIfs;
        //                        return (isValid, toplamAdetIfs, toplamAdetYuklenen);
        //                    }
        //                }
        //            }
        //        }
        //        public static (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        //        {
        //            string query = @"
        //SELECT 
        //    SUM(m.adet) AS ToplamAdetIfs,
        //    (
        //        SELECT SUM(KD.toplamAdet)
        //        FROM KesimDetaylari KD
        //        WHERE KD.malzemeKod = @malzemeKod
        //          AND KD.kalite = @Kalite
        //          AND KD.proje = @ProjeAdi
        //    ) AS ToplamAdetYuklenen
        //FROM Malzemeler m
        //JOIN Gruplar g ON m.grupID = g.grupID
        //JOIN Projeler p ON g.projeID = p.projeID
        //WHERE m.kalite = @Kalite
        //  AND m.malzemeAd = @malzemeAd
        //  AND m.malzemeKod = @malzemeKod
        //  AND p.projeAdi = @ProjeAdi";

        //            using (var conn = DataBaseHelper.GetConnection())
        //            {
        //                conn.Open();
        //                using (var command = new SqlCommand(query, conn))
        //                {
        //                    command.Parameters.AddWithValue("@Kalite", kalite);
        //                    command.Parameters.AddWithValue("@malzemeAd", malzeme);
        //                    command.Parameters.AddWithValue("@malzemeKod", kalip);
        //                    command.Parameters.AddWithValue("@ProjeAdi", proje);

        //                    using (var reader = command.ExecuteReader())
        //                    {
        //                        int toplamAdetIfs = 0;
        //                        int toplamAdetYuklenen = 0;

        //                        if (reader.Read())
        //                        {
        //                            toplamAdetIfs = reader["ToplamAdetIfs"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetIfs"]) : 0;
        //                            toplamAdetYuklenen = reader["ToplamAdetYuklenen"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetYuklenen"]) : 0;
        //                        }

        //                        bool isValid = girilenAdet + toplamAdetYuklenen <= toplamAdetIfs;
        //                        return (isValid, toplamAdetIfs, toplamAdetYuklenen);
        //                    }
        //                }
        //            }
        //        }

        //        public static (bool isValid, int toplamAdetIfs, int toplamAdetYuklenen) KontrolAdeta(string kalite, string malzeme, string kalip, string proje, int girilenAdet)
        //        {
        //            string query = @"
        //SELECT 
        //    SUM(m.adet) AS ToplamAdetIfs, 
        //    COALESCE(SUM(KD.toplamAdet), 0) AS ToplamAdetYuklenen 
        //FROM Malzemeler m 
        //JOIN Gruplar g ON m.grupID = g.grupID 
        //JOIN Projeler p ON g.projeID = p.projeID 
        //LEFT JOIN KesimDetaylari KD ON KD.malzemeKod = m.malzemeKod 
        //    AND KD.kalite = m.kalite 
        //    AND KD.proje = p.projeAdi 
        //WHERE m.kalite = @Kalite 
        //  AND m.malzemeAd = @malzemeAd 
        //  AND m.malzemeKod = @malzemeKod 
        //  AND p.projeAdi = @ProjeAdi";

        //            using (var conn = DataBaseHelper.GetConnection())
        //            {
        //                conn.Open();
        //                using (var command = new SqlCommand(query, conn))
        //                {
        //                    command.Parameters.AddWithValue("@Kalite", kalite);
        //                    command.Parameters.AddWithValue("@malzemeAd", malzeme);
        //                    command.Parameters.AddWithValue("@malzemeKod", kalip);
        //                    command.Parameters.AddWithValue("@ProjeAdi", proje);

        //                    using (var reader = command.ExecuteReader())
        //                    {
        //                        int toplamAdetIfs = 0;
        //                        int toplamAdetYuklenen = 0;
        //                        if (reader.Read())
        //                        {
        //                            toplamAdetIfs = reader["ToplamAdetIfs"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetIfs"]) : 0;
        //                            toplamAdetYuklenen = reader["ToplamAdetYuklenen"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdetYuklenen"]) : 0;
        //                        }
        //                        return (girilenAdet + toplamAdetYuklenen <= toplamAdetIfs, toplamAdetIfs, toplamAdetYuklenen);
        //                    }
        //                }
        //            }
        //        }
        //    }
    }
}
