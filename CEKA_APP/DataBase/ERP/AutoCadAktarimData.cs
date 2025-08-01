using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CEKA_APP.Entitys;

namespace CEKA_APP.DataBase
{
    public class AutoCadAktarimData
    {
        public static void SaveAutoCadData(string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid yuklemeId)
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

                    string ustGrup = grupAdi.Length >= 3 ? grupAdi.Substring(0, 3) : grupAdi;
                    string ustGrupQuery = "SELECT ustGrupId FROM UstGruplar WHERE projeId = @projeId AND ustGrup = @ustGrup AND yuklemeId = @yuklemeId";
                    int ustGrupId;
                    using (var cmd = new SqlCommand(ustGrupQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@projeId", projeId);
                        cmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                        cmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        var result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            string insertUstGrup = "INSERT INTO UstGruplar (projeId, ustGrup, yuklemeId, takimCarpani) VALUES (@projeId, @ustGrup, @yuklemeId, 1); SELECT SCOPE_IDENTITY();";
                            using (var insertCmd = new SqlCommand(insertUstGrup, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@projeId", projeId);
                                insertCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                                insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                                ustGrupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            ustGrupId = Convert.ToInt32(result);
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
                            string insertGrup = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani) VALUES (@projeId, @grupAdi, @yuklemeId, 1); SELECT SCOPE_IDENTITY();";
                            using (var insertCmd = new SqlCommand(insertGrup, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@projeId", projeId);
                                insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                                grupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            grupId = Convert.ToInt32(result);
                        }
                    }

                    string insertMalzeme = @"
                INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite, yuklemeId, orjinalAdet)
                VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite, @yuklemeId, @orjinalAdet)";
                    using (var insertCmd = new SqlCommand(insertMalzeme, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@grupId", grupId);
                        insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        insertCmd.Parameters.AddWithValue("@adet", adet);
                        insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        insertCmd.Parameters.AddWithValue("@orjinalAdet", adet); 
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
                    -- Malzeme kayıtları
                    SELECT
                        p.projeAdi AS proje,
                        g.grupAdi AS grup,
                        m.malzemeKod,
                        m.yuklemeId
                    FROM Malzemeler m
                    JOIN Gruplar g ON m.grupId = g.grupId
                    JOIN Projeler p ON g.projeId = p.projeId
                    WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi)
                    UNION
                    -- Grup kayıtları (malzemesiz)
                    SELECT
                        p.projeAdi AS proje,
                        g.grupAdi AS grup,
                        NULL AS malzemeKod,
                        g.yuklemeId
                    FROM Gruplar g
                    JOIN Projeler p ON g.projeId = p.projeId
                    LEFT JOIN Malzemeler m ON m.grupId = g.grupId
                    WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi) AND m.malzemeId IS NULL
                    UNION
                    -- Üst Grup kayıtları
                    SELECT
                        p.projeAdi AS proje,
                        ug.ustGrup AS grup,
                        NULL AS malzemeKod,
                        ug.yuklemeId
                    FROM UstGruplar ug
                    JOIN Projeler p ON ug.projeId = p.projeId
                    WHERE (@projeAdi IS NULL OR p.projeAdi = @projeAdi)
                    UNION
                    -- Proje kayıtları (grupsuz ve malzemesiz)
                    SELECT
                        p.projeAdi AS proje,
                        NULL AS grup,
                        NULL AS malzemeKod,
                        NULL AS yuklemeId
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
                        MalzemeKod = reader["malzemeKod"] != DBNull.Value ? reader["malzemeKod"].ToString() : null,
                        YuklemeId = reader["yuklemeId"] != DBNull.Value ? Guid.Parse(reader["yuklemeId"].ToString()) : (Guid?)null
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
            SELECT DISTINCT p.projeAdi, g.grupAdi, g.yuklemeId, g.takimCarpani
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

        public static DataTable UstGruplariGetir(string projeAdi)
        {
            DataTable tablo = new DataTable();
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
                    SELECT DISTINCT
                        P.projeAdi,
                        UG.ustGrup AS ustGrupAdi,
                        UG.takimCarpani,
                        UG.yuklemeId
                    FROM UstGruplar UG
                    JOIN Projeler P ON UG.projeId = P.projeId
                    WHERE P.projeAdi = @projeAdi
                    ORDER BY UG.ustGrup";

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
            SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite, m.yuklemeId, m.orjinalAdet
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
            SELECT p.projeAdi, g.grupAdi, m.malzemeKod, m.malzemeAd, m.adet, m.kalite, m.yuklemeId, m.orjinalAdet
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

        public static void GrupEkleGuncelle(string projeNo, string grupAdi, string eskiGrupAdi, Guid? yuklemeId = null)
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
                        string updateQuery = "UPDATE Gruplar SET grupAdi = @yeniGrupAdi, yuklemeId = @yuklemeId WHERE projeId = @projeId AND grupAdi = @eskiGrupAdi";
                        using (var updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@yeniGrupAdi", grupAdi);
                            updateCmd.Parameters.AddWithValue("@projeId", projeId);
                            updateCmd.Parameters.AddWithValue("@eskiGrupAdi", eskiGrupAdi);
                            updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani) VALUES (@projeId, @grupAdi, @yuklemeId, 1)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@projeId", projeId);
                            insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                            insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void UstGrupEkleGuncelle(string projeNo, string ustGrupAdi, Guid? yuklemeId, int takimCarpani, string eskiUstGrupAdi = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                int projeId = GetProjeId(conn, projeNo);

                string checkQuery = "SELECT COUNT(*) FROM UstGruplar WHERE projeId = @projeId AND ustGrup = @ustGrupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
                using (var checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@projeId", projeId);
                    checkCmd.Parameters.AddWithValue("@ustGrupAdi", eskiUstGrupAdi ?? ustGrupAdi);
                    checkCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0 && !string.IsNullOrEmpty(eskiUstGrupAdi))
                    {
                        string updateQuery = @"UPDATE UstGruplar
                                              SET ustGrup = @yeniUstGrupAdi, takimCarpani = @takimCarpani
                                              WHERE projeId = @projeId AND ustGrup = @eskiUstGrupAdi AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)";
                        using (var updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@yeniUstGrupAdi", ustGrupAdi);
                            updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                            updateCmd.Parameters.AddWithValue("@projeId", projeId);
                            updateCmd.Parameters.AddWithValue("@eskiUstGrupAdi", eskiUstGrupAdi);
                            updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO UstGruplar (projeId, ustGrup, yuklemeId, takimCarpani) VALUES (@projeId, @ustGrupAdi, @yuklemeId, @takimCarpani)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@projeId", projeId);
                            insertCmd.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                            insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                            insertCmd.ExecuteNonQuery();
                        }

                        string insertGrupQuery = "INSERT INTO Gruplar (projeId, grupAdi, yuklemeId, takimCarpani) VALUES (@projeId, @grupAdi, @yuklemeId, @takimCarpani)";
                        using (var insertGrupCmd = new SqlCommand(insertGrupQuery, conn))
                        {
                            insertGrupCmd.Parameters.AddWithValue("@projeId", projeId);
                            insertGrupCmd.Parameters.AddWithValue("@grupAdi", ustGrupAdi); 
                            insertGrupCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            insertGrupCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                            insertGrupCmd.ExecuteNonQuery();
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

        public static void UstGrupSil(string projeAdi, string ustGrupAdi, Guid yuklemeId)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string checkQuery = @"
                        SELECT UG.ustGrupId
                        FROM UstGruplar UG
                        JOIN Projeler P ON UG.projeId = P.projeId
                        WHERE P.projeAdi = @projeAdi AND UG.ustGrup = @ustGrupAdi AND UG.yuklemeId = @yuklemeId";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                        checkCmd.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                        checkCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        var result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception($"Üst Grup bulunamadı: projeAdi={projeAdi}, ustGrupAdi={ustGrupAdi}, yuklemeId={yuklemeId}");
                        }
                    }

                    string deleteRelatedDataQuery = @"
                        DELETE M
                        FROM Malzemeler M
                        JOIN Gruplar G ON M.grupId = G.grupId
                        JOIN Projeler P ON G.projeId = P.projeId
                        WHERE P.projeAdi = @projeAdi AND G.yuklemeId = @yuklemeId;

                        DELETE G
                        FROM Gruplar G
                        JOIN Projeler P ON G.projeId = P.projeId
                        WHERE P.projeAdi = @projeAdi AND G.yuklemeId = @yuklemeId;

                        DELETE UG
                        FROM UstGruplar UG
                        JOIN Projeler P ON UG.projeId = P.projeId
                        WHERE P.projeAdi = @projeAdi AND UG.ustGrup = @ustGrupAdi AND UG.yuklemeId = @yuklemeId;
                    ";

                    using (var komut = new SqlCommand(deleteRelatedDataQuery, conn))
                    {
                        komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                        komut.Parameters.AddWithValue("@ustGrupAdi", ustGrupAdi);
                        komut.Parameters.AddWithValue("@yuklemeId", yuklemeId);
                        komut.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Üst Grup silme hatası: {ex.Message}", ex);
                }
            }
        }

        public static void MalzemeEkleGuncelle(string projeNo, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite, Guid? yuklemeId = null, string eskiMalzemeKod = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
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
                                   SET malzemeKod = @yeniMalzemeKod, adet = @adet, malzemeAd = @malzemeAd, kalite = @kalite, yuklemeId = @yuklemeId
                                   WHERE grupId = @grupId AND malzemeKod = @eskiMalzemeKod";
                        using (var updateCmd = new SqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@yeniMalzemeKod", malzemeKod);
                            updateCmd.Parameters.AddWithValue("@adet", adet);
                            updateCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@grupId", grupId);
                            updateCmd.Parameters.AddWithValue("@eskiMalzemeKod", eskiMalzemeKod);
                            updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = @"INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite, yuklemeId, orjinalAdet)
                                   VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite, @yuklemeId, @orjinalAdet)";
                        using (var insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@grupId", grupId);
                            insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                            insertCmd.Parameters.AddWithValue("@adet", adet);
                            insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@orjinalAdet", adet); 
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
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

        public static void UpdateTakimCarpaniVeAltGruplar(string projeAdi, string ustGrup, Guid? yuklemeId, int takimCarpani)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                int projeId = GetProjeId(conn, projeAdi);

                string ustGrupUpdateQuery = @"
                    UPDATE UstGruplar
                    SET takimCarpani = @takimCarpani
                    WHERE projeId = @projeId
                    AND ustGrup = @ustGrup
                    AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)"; 
                using (var updateCmd = new SqlCommand(ustGrupUpdateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@projeId", projeId);
                    updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                    updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    updateCmd.ExecuteNonQuery();
                }

                string grupUpdateQuery = @"
                    UPDATE Gruplar
                    SET takimCarpani = @takimCarpani
                    WHERE projeId = @projeId
                    AND grupAdi LIKE @ustGrup + '-%'
                    AND (@yuklemeId IS NULL OR yuklemeId = @yuklemeId)"; 
                using (var updateCmd = new SqlCommand(grupUpdateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@projeId", projeId);
                    updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                    updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    updateCmd.ExecuteNonQuery();
                }

                string malzemeUpdateQuery = @"
                    UPDATE Malzemeler
                    SET adet = m.orjinalAdet * @takimCarpani
                    FROM Malzemeler m
                    JOIN Gruplar g ON m.grupId = g.grupId
                    JOIN Projeler p ON g.projeId = p.projeId
                    WHERE p.projeAdi = @projeAdi
                    AND g.grupAdi LIKE @ustGrup + '-%'
                    AND (@yuklemeId IS NULL OR m.yuklemeId = @yuklemeId)"; 
                using (var updateCmd = new SqlCommand(malzemeUpdateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                    updateCmd.Parameters.AddWithValue("@ustGrup", ustGrup);
                    updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    updateCmd.Parameters.AddWithValue("@yuklemeId", yuklemeId ?? (object)DBNull.Value);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateTakimCarpani(string projeAdi, string grupAdi, int takimCarpani)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                int projeId = GetProjeId(conn, projeAdi);
                int grupId = GetGrupId(conn, projeId, grupAdi);

                string updateQuery = @"
                    UPDATE Gruplar
                    SET takimCarpani = @takimCarpani
                    WHERE projeId = @projeId AND grupId = @grupId;

                    UPDATE Malzemeler
                    SET adet = m.orjinalAdet * @takimCarpani
                    FROM Malzemeler m
                    JOIN Gruplar g ON m.grupId = g.grupId
                    JOIN Projeler p ON g.projeId = p.projeId
                    WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";
                using (var updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@takimCarpani", takimCarpani);
                    updateCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                    updateCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                    updateCmd.Parameters.AddWithValue("@projeId", projeId);
                    updateCmd.Parameters.AddWithValue("@grupId", grupId);
                    updateCmd.ExecuteNonQuery();
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
    }
}