using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using KesimTakip.Entitys;

namespace KesimTakip.DataBase
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

                    // 1. ProjeId getir (yoksa ekle)
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

                    // 2. GrupId getir (yoksa ekle)
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

                    // 3. Malzeme ekle
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

        public static void ProjeEkle(string projeAdi, string aciklama = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                conn.Open();
                string sorgu = @"
            INSERT INTO Projeler (projeAdi, olusturmaTarihi, aciklama)
            VALUES (@projeAdi, GETDATE(), @aciklama)";

                SqlCommand komut = new SqlCommand(sorgu, conn);
                komut.Parameters.AddWithValue("@projeAdi", projeAdi);
                komut.Parameters.AddWithValue("@aciklama", aciklama != null ? (object)aciklama : DBNull.Value);
                komut.ExecuteNonQuery();
            }
        }
        public static void GrupEkleGuncelle(string projeAdi, string grupAdi, string eskiGrupAdi = null)
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int projeId;
                            string getProjeIdQuery = "SELECT projeId FROM Projeler WHERE projeAdi = @projeAdi";
                            using (var cmd = new SqlCommand(getProjeIdQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                                var result = cmd.ExecuteScalar();
                                if (result == null)
                                {
                                    throw new Exception($"Proje bulunamadı: {projeAdi}");
                                }
                                projeId = Convert.ToInt32(result);
                            }

                            if (!string.IsNullOrEmpty(eskiGrupAdi) && eskiGrupAdi != grupAdi)
                            {
                                string deleteQuery = @"
                                DELETE m
                                FROM Malzemeler m
                                JOIN Gruplar g ON m.grupId = g.grupId
                                JOIN Projeler p ON g.projeId = p.projeId
                                WHERE p.projeAdi = @projeAdi AND g.grupAdi = @eskiGrupAdi;

                                DELETE g
                                FROM Gruplar g
                                JOIN Projeler p ON g.projeId = p.projeId
                                WHERE p.projeAdi = @projeAdi AND g.grupAdi = @eskiGrupAdi";
                                using (var deleteCmd = new SqlCommand(deleteQuery, conn, transaction))
                                {
                                    deleteCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                                    deleteCmd.Parameters.AddWithValue("@eskiGrupAdi", eskiGrupAdi);
                                    int rowsAffected = deleteCmd.ExecuteNonQuery();
                                }
                            }

                            string checkQuery = @"
                            SELECT grupId 
                            FROM Gruplar g
                            JOIN Projeler p ON g.projeId = p.projeId
                            WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";
                            int grupId;
                            using (var checkCmd = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                                checkCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                var result = checkCmd.ExecuteScalar();
                                if (result != null)
                                {
                                    grupId = Convert.ToInt32(result);
                                    string updateQuery = @"
                                    UPDATE Gruplar
                                    SET grupAdi = @grupAdi
                                    WHERE grupId = @grupId";
                                    using (var updateCmd = new SqlCommand(updateQuery, conn, transaction))
                                    {
                                        updateCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                        updateCmd.Parameters.AddWithValue("@grupId", grupId);
                                        int rowsAffected = updateCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Yeni grup ekle
                                    string insertQuery = @"
                                    INSERT INTO Gruplar (projeId, grupAdi)
                                    VALUES (@projeId, @grupAdi);
                                    SELECT SCOPE_IDENTITY();";
                                    using (var insertCmd = new SqlCommand(insertQuery, conn, transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue("@projeId", projeId);
                                        insertCmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                        grupId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Grup ekleme/güncelleme hatası: {ex.Message}", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Veritabanı bağlantı hatası: {ex.Message}", ex);
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
        public static void MalzemeEkleGuncelle(string projeAdi, string grupAdi, string malzemeKod, int adet, string malzemeAd, string kalite)
        {
            if (string.IsNullOrEmpty(malzemeKod))
                throw new ArgumentException("Malzeme kodu boş olamaz.");

            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // GrupId'yi getir
                            string getGrupIdQuery = @"
                        SELECT g.grupId
                        FROM Gruplar g
                        JOIN Projeler p ON g.projeId = p.projeId
                        WHERE p.projeAdi = @projeAdi AND g.grupAdi = @grupAdi";
                            int grupId;
                            using (var cmd = new SqlCommand(getGrupIdQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@projeAdi", projeAdi);
                                cmd.Parameters.AddWithValue("@grupAdi", grupAdi);
                                var result = cmd.ExecuteScalar();
                                if (result == null)
                                {
                                    throw new Exception($"Grup bulunamadı: projeAdi={projeAdi}, grupAdi={grupAdi}");
                                }
                                grupId = Convert.ToInt32(result);
                            }

                            string checkQuery = @"
                        SELECT malzemeId
                        FROM Malzemeler
                        WHERE grupId = @grupId AND malzemeKod = @malzemeKod";
                            using (var checkCmd = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@grupId", grupId);
                                checkCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                using (var reader = checkCmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int malzemeId = reader.GetInt32(0);
                                        reader.Close();

                                        string updateQuery = @"
                                    UPDATE Malzemeler
                                    SET adet = @adet, malzemeAd = @malzemeAd, kalite = @kalite
                                    WHERE malzemeId = @malzemeId";
                                        using (var updateCmd = new SqlCommand(updateQuery, conn, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@adet", adet);
                                            updateCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                                            updateCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                                            updateCmd.Parameters.AddWithValue("@malzemeId", malzemeId);
                                            int rowsAffected = updateCmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        reader.Close();

                                        string insertQuery = @"
                                    INSERT INTO Malzemeler (grupId, malzemeKod, adet, malzemeAd, kalite)
                                    VALUES (@grupId, @malzemeKod, @adet, @malzemeAd, @kalite);
                                    SELECT SCOPE_IDENTITY();";
                                        using (var insertCmd = new SqlCommand(insertQuery, conn, transaction))
                                        {
                                            insertCmd.Parameters.AddWithValue("@grupId", grupId);
                                            insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                            insertCmd.Parameters.AddWithValue("@adet", adet);
                                            insertCmd.Parameters.AddWithValue("@malzemeAd", malzemeAd ?? (object)DBNull.Value);
                                            insertCmd.Parameters.AddWithValue("@kalite", kalite ?? (object)DBNull.Value);
                                            int malzemeId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                        }
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Malzeme ekleme/güncelleme hatası: {ex.Message}", ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Veritabanı bağlantı hatası: {ex.Message}", ex);
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
                    command.Parameters.AddWithValue("@malzemeKod", kalip); // malzemeAd ile kalip birleştirilmiş kabul ediliyor
                    command.Parameters.AddWithValue("@ProjeAdi", proje);

                    var result = command.ExecuteScalar();
                    int toplamAdet = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    return (girilenAdet <= toplamAdet, toplamAdet);
                }
            }
        }
    }
}
