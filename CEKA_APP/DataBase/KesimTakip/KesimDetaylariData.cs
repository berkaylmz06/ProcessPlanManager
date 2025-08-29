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
    class KesimDetaylariData
    {
        public static void SaveKesimDetaylariData(string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet, bool ekBilgi)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string checkQuery = @"SELECT COUNT(*) FROM KesimDetaylari 
                                  WHERE kalite = @kalite AND malzeme = @malzeme 
                                  AND malzemeKod = @malzemeKod AND proje = @proje
                                  AND ekBilgi = @ekBilgi";

                    using (var checkCmd = new SqlCommand(checkQuery, conn))
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

                            using (var insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@kalite", kalite);
                                insertCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                insertCmd.Parameters.AddWithValue("@proje", proje);
                                insertCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                                insertCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);
                                insertCmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                                insertCmd.ExecuteNonQuery();
                                Console.WriteLine($"Yeni kayıt eklendi: kalite={kalite}, malzeme={malzeme}, malzemeKod={malzemeKod}, proje={proje}, kesilecekAdet={kesilecekAdet}, toplamAdet={toplamAdet}, ekVar={ekBilgi}");
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

                            using (var updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@kalite", kalite);
                                updateCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                updateCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                updateCmd.Parameters.AddWithValue("@proje", proje);
                                updateCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                                updateCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);
                                updateCmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                                int rowsAffected = updateCmd.ExecuteNonQuery();
                                Console.WriteLine($"Kayıt güncellendi: kalite={kalite}, malzeme={malzeme}, malzemeKod={malzemeKod}, proje={proje}, kesilecekAdet+={kesilecekAdet}, toplamAdet+={toplamAdet}, EkVar={ekBilgi}, etkilenen satır={rowsAffected}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                Console.WriteLine($"Hata oluştu: {ex.Message}");
            }
        }

        public static DataRow GetKesimDetaylariData(string kalite, string malzeme, string malzemeKod, string proje)
        {
            DataTable dt = GetKesimDetaylari();
            foreach (DataRow row in dt.Rows)
            {
                if (row["kalite"].ToString() == kalite &&
                    row["malzeme"].ToString() == malzeme &&
                    row["malzemeKod"].ToString() == malzemeKod &&
                    row["proje"].ToString() == proje)
                {
                    return row;
                }
            }
            return null;
        }

        public static DataTable GetKesimDetaylari()
        {
            string query = "SELECT kalite, malzeme, malzemeKod, proje ,kesilmisAdet, kesilecekAdet, toplamAdet FROM KesimDetaylari";
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static DataTable GetKesimDetaylariBilgi()
        {
            string query = "SELECT kalite, malzeme, malzemeKod, proje, kesilmisAdet, kesilecekAdet, toplamAdet, ekBilgi FROM KesimDetaylari";
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
        public static bool PozExists(string kalite, string malzeme, string malzemekod, string proje)
        {
            using (SqlConnection connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM KesimDetaylari WHERE kalite = @kalite AND malzeme = @malzeme AND malzemekod = @malzemekod AND proje = @proje";
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
                catch (Exception ex)
                {
                    Console.WriteLine($"PozExists Hata: {ex.Message}");
                    return false;
                }
            }
        }

        public static bool UpdateKesilmisAdet(string kalite, string malzeme, string malzemekod, string proje, int sondurum)
        {
            using (SqlConnection connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                UPDATE KesimDetaylari 
                SET kesilmisAdet = kesilmisAdet + @sondurum, 
                    kesilecekAdet = kesilecekAdet - @sondurum 
                WHERE kalite = @kalite 
                AND malzeme = @malzeme 
                AND malzemekod = @malzemekod 
                AND proje = @proje 
                AND kesilecekAdet >= @sondurum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kalite", kalite);
                        command.Parameters.AddWithValue("@malzeme", malzeme);
                        command.Parameters.AddWithValue("@malzemekod", malzemekod);
                        command.Parameters.AddWithValue("@proje", proje);
                        command.Parameters.AddWithValue("@sondurum", sondurum);
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Kalite: {kalite}, Malzeme: {malzeme}, Malzemekod: {malzemekod}, Proje: {proje}, Sondurum: {sondurum}, RowsAffected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UpdateKesilmisAdet Hata: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool KayitVarMi(string kalite, string malzeme, string malzemekod, string proje)
        {
            using (SqlConnection connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM KesimDetaylari WHERE kalite = @kalite AND malzeme = @malzeme AND malzemekod = @malzemekod AND proje = @proje";
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
                catch (Exception ex)
                {
                    Console.WriteLine($"KayitVarMi Hata: {ex.Message}");
                    return false;
                }
            }
        }
        public static List<KesimDetaylari> GetKesimDetaylariBilgileri()
        {
            List<KesimDetaylari> detaylar = new List<KesimDetaylari>();

            string query = @"
    SELECT
        kd.kalite,
        kd.malzeme,
        kd.malzemeKod,
        kd.proje AS projeAdi,
        SUM(kd.kesilmisAdet) AS toplamKesilmisAdet,
        ISNULL(SUM(kd.toplamAdet), 0) AS toplamAdet,
        CAST(kd.ekBilgi AS BIT) AS ekBilgi,
        donusturulmusMalzemeler.grupAdi
    FROM
        KesimDetaylari AS kd
    INNER JOIN
        (
            SELECT DISTINCT
                REPLACE(m.malzemeKod, SUBSTRING(m.malzemeKod, 1, 6), g.grupAdi) AS yeniMalzemeKod,
                g.grupAdi
            FROM
                Malzemeler AS m
            INNER JOIN
                Gruplar AS g ON m.grupId = g.grupId
        ) AS donusturulmusMalzemeler ON kd.malzemeKod = donusturulmusMalzemeler.yeniMalzemeKod
    GROUP BY
        kd.kalite,
        kd.malzeme,
        kd.malzemeKod,
        kd.proje,
        kd.ekBilgi,
        donusturulmusMalzemeler.grupAdi;
    ";

            using (SqlConnection conn = DataBaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string kalite = reader["kalite"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            string malzeme = reader["malzeme"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            string malzemeKod = reader["malzemeKod"]?.ToString()?.Trim().ToLowerInvariant() ?? "";
                            string projeAdi = reader["projeAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
                            int toplamKesilmisAdet = reader["toplamKesilmisAdet"] != DBNull.Value ? Convert.ToInt32(reader["toplamKesilmisAdet"]) : 0;
                            int toplamAdet = reader["toplamAdet"] != DBNull.Value ? Convert.ToInt32(reader["toplamAdet"]) : 0;
                            bool ekBilgi = reader["ekBilgi"] != DBNull.Value && Convert.ToBoolean(reader["ekBilgi"]);
                            string grupAdi = reader["grupAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmeyen_grup";


                            string[] parts = malzemeKod.Split('-');
                            if (parts.Length >= 3)
                            {
                                string kalipKodu = $"{parts[0]}-{parts[1]}";
                                if (!AutoCadAktarimData.GetirStandartGruplar(kalipKodu))
                                {
                                    parts[1] = "00";
                                    malzemeKod = string.Join("-", parts);
                                }
                            }
                            malzemeKod = malzemeKod.ToLowerInvariant();

                            string key = $"{kalite}_{malzeme}_{malzemeKod}_{projeAdi}_{grupAdi}";

                            detaylar.Add(new KesimDetaylari
                            {
                                Key = key,
                                kesilmisAdet = toplamKesilmisAdet,
                                toplamAdet = toplamAdet,
                                ekBilgi = ekBilgi
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata: {ex.Message}", "Veri Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return detaylar;
        }
        //    public static List<KesimDetaylari> GetKesimDetaylariBilgileri()
        //    {
        //        List<KesimDetaylari> detaylar = new List<KesimDetaylari>();

        //        string query = @"
        //    SELECT
        //        kd.kalite,
        //        kd.malzeme,
        //        kd.malzemeKod,
        //        kd.proje AS projeAdi,
        //        kd.kesilmisAdet,
        //        ISNULL(SUM(kd.toplamAdet), 0) AS toplamAdet,
        //        CAST(kd.ekBilgi AS BIT) AS ekBilgi,
        //        donusturulmusMalzemeler.grupAdi
        //    FROM
        //        KesimDetaylari AS kd
        //    INNER JOIN
        //        (
        //            SELECT
        //                m.malzemeId,
        //                REPLACE(m.malzemeKod, SUBSTRING(m.malzemeKod, 1, 6), g.grupAdi) AS yeniMalzemeKod,
        //                m.grupId,
        //                g.grupAdi
        //            FROM
        //                Malzemeler AS m
        //            INNER JOIN
        //                Gruplar AS g ON m.grupId = g.grupId
        //        ) AS donusturulmusMalzemeler ON kd.malzemeKod = donusturulmusMalzemeler.yeniMalzemeKod
        //    GROUP BY
        //        kd.kalite,
        //        kd.malzeme,
        //        kd.malzemeKod,
        //        kd.proje,
        //        kd.kesilmisAdet,
        //        kd.ekBilgi,
        //        donusturulmusMalzemeler.grupAdi
        //";

        //        using (SqlConnection conn = DataBaseHelper.GetConnection())
        //        {
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            try
        //            {
        //                conn.Open();
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        string kalite = reader["kalite"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                        string malzeme = reader["malzeme"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                        string malzemeKod = reader["malzemeKod"]?.ToString()?.Trim().ToLowerInvariant() ?? "";
        //                        string projeAdi = reader["projeAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmiyor";
        //                        int kesilmisAdet = reader["kesilmisAdet"] != DBNull.Value ? Convert.ToInt32(reader["kesilmisAdet"]) : 0;
        //                        int toplamAdet = reader["toplamAdet"] != DBNull.Value ? Convert.ToInt32(reader["toplamAdet"]) : 0;
        //                        bool ekBilgi = reader["ekBilgi"] != DBNull.Value && Convert.ToBoolean(reader["ekBilgi"]);
        //                        string grupAdi = reader["grupAdi"]?.ToString()?.Trim().ToLowerInvariant() ?? "bilinmeyen_grup";

        //                        // Hata ayıklama için log
        //                        Console.WriteLine($"Satır: MalzemeKod={malzemeKod}, ProjeAdi={projeAdi}, GrupAdi={grupAdi}, ToplamAdet={toplamAdet}, KesilmisAdet={kesilmisAdet}, EkBilgi={ekBilgi}");

        //                        string[] parts = malzemeKod.Split('-');
        //                        if (parts.Length >= 3)
        //                        {
        //                            string kalipKodu = $"{parts[0]}-{parts[1]}";
        //                            if (!AutoCadAktarimData.GetirStandartGruplar(kalipKodu))
        //                            {
        //                                parts[1] = "00";
        //                                malzemeKod = string.Join("-", parts);
        //                            }
        //                        }
        //                        malzemeKod = malzemeKod.ToLowerInvariant();

        //                        string key = $"{kalite}_{malzeme}_{malzemeKod}_{projeAdi}_{grupAdi}";

        //                        detaylar.Add(new KesimDetaylari
        //                        {
        //                            Key = key,
        //                            kesilmisAdet = kesilmisAdet,
        //                            toplamAdet = toplamAdet,
        //                            ekBilgi = ekBilgi
        //                        });
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Hata: {ex.Message}", "Veri Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                Console.WriteLine($"SQL Hatası: {ex.Message}");
        //            }
        //        }

        //        return detaylar;
        //    }


        public static bool SilKesimDetaylari(string kalite, string malzeme, string malzemeKod, string proje, int silinecekAdet = 0, bool tamSilme = false)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string selectQuery = @"SELECT kesilecekAdet, toplamAdet FROM KesimDetaylari 
                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                AND malzemeKod = @malzemeKod AND proje = @proje";

                    using (var selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@kalite", kalite);
                        selectCmd.Parameters.AddWithValue("@malzeme", malzeme);
                        selectCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        selectCmd.Parameters.AddWithValue("@proje", proje);

                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int mevcutKesilecekAdet = reader.GetInt32(0);
                                int mevcutToplamAdet = reader.GetInt32(1);

                                if (tamSilme)
                                {
                                    string deleteQuery = @"DELETE FROM KesimDetaylari 
                                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                                AND malzemeKod = @malzemeKod AND proje = @proje";
                                    using (var deleteCmd = new SqlCommand(deleteQuery, conn))
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
                                    {
                                        MessageBox.Show("Silinecek adet, mevcut değerlerden fazla olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }

                                    string updateQuery = @"UPDATE KesimDetaylari 
                                                SET kesilecekAdet = kesilecekAdet - @silinecekAdet, 
                                                    toplamAdet = toplamAdet - @silinecekAdet
                                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                                AND malzemeKod = @malzemeKod AND proje = @proje";

                                    using (var updateCmd = new SqlCommand(updateQuery, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@kalite", kalite);
                                        updateCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                        updateCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                        updateCmd.Parameters.AddWithValue("@proje", proje);
                                        updateCmd.Parameters.AddWithValue("@silinecekAdet", silinecekAdet);

                                        int rowsAffected = updateCmd.ExecuteNonQuery();
                                        if (rowsAffected > 0)
                                        {
                                            Console.WriteLine($"Kayıt güncellendi: kalite={kalite}, malzeme={malzeme}, malzemeKod={malzemeKod}, proje={proje}, silinenAdet={silinecekAdet}");
                                        }
                                        return rowsAffected > 0;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Silinecek adet belirtilmedi veya tam silme istenmedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Belirtilen kritere göre kayıt bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return false;
            }
        }
        public static bool GuncelleKesimDetaylari(string kalite, string malzeme, string kalipNo, string kesilecekPozlar, string proje, int silinecekAdet = 0, bool tamSilme = false)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string malzemeKod = $"{kalipNo}-{kesilecekPozlar}";

                    string selectQuery = @"SELECT kesilecekAdet, toplamAdet FROM KesimDetaylari 
                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                AND malzemeKod = @malzemeKod AND proje = @proje";

                    using (var selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@kalite", kalite);
                        selectCmd.Parameters.AddWithValue("@malzeme", malzeme);
                        selectCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        selectCmd.Parameters.AddWithValue("@proje", proje);

                        using (var reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int mevcutKesilecekAdet = reader.GetInt32(0);
                                int mevcutToplamAdet = reader.GetInt32(1);

                                reader.Close();

                                if (tamSilme)
                                {
                                    string deleteQuery = @"DELETE FROM KesimDetaylari 
                                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                                AND malzemeKod = @malzemeKod AND proje = @proje";
                                    using (var deleteCmd = new SqlCommand(deleteQuery, conn))
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
                                    {
                                        MessageBox.Show("Silinecek adet, mevcut değerlerden fazla olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }

                                    string updateQuery = @"UPDATE KesimDetaylari 
                                                SET kesilecekAdet = kesilecekAdet - @silinecekAdet, 
                                                    toplamAdet = toplamAdet - @silinecekAdet
                                                WHERE kalite = @kalite AND malzeme = @malzeme 
                                                AND malzemeKod = @malzemeKod AND proje = @proje";

                                    using (var updateCmd = new SqlCommand(updateQuery, conn))
                                    {
                                        updateCmd.Parameters.AddWithValue("@kalite", kalite);
                                        updateCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                        updateCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                        updateCmd.Parameters.AddWithValue("@proje", proje);
                                        updateCmd.Parameters.AddWithValue("@silinecekAdet", silinecekAdet);

                                        int rowsAffected = updateCmd.ExecuteNonQuery();
                                        if (rowsAffected > 0)
                                        {
                                            Console.WriteLine($"Kayıt güncellendi: kalite={kalite}, malzeme={malzeme}, malzemeKod={malzemeKod}, proje={proje}, silinenAdet={silinecekAdet}");
                                        }
                                        return rowsAffected > 0;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Silinecek adet belirtilmedi veya tam silme istenmedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                            else
                            {
                                reader.Close();
                                if (tamSilme)
                                {
                                    MessageBox.Show($"Belirtilen kritere göre kayıt bulunamadı: kalite={kalite}, malzeme={malzeme}, malzemeKod={malzemeKod}, proje={proje}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme işlemi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return false;
            }
        }
        public static (int toplamAdet, int kesilmisAdet, int kesilecekAdet, List<string> eslesenPozlar) GetAdetlerVeEslesenPozlar(
               string kalite, string malzeme, string proje, string malzemeKodIlkKisim, string malzemeKodUcuncuKisim)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT malzemeKod, toplamAdet, kesilmisAdet, kesilecekAdet
                        FROM KesimDetaylari
                        WHERE kalite = @kalite
                        AND malzeme = @malzeme
                        AND proje = @proje
                        AND malzemeKod LIKE @malzemeKodIlkKisim + '-[0-9][0-9]-' + @malzemeKodUcuncuKisim
                        AND ekBilgi = 1"; 

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kalite", kalite);
                        cmd.Parameters.AddWithValue("@malzeme", malzeme);
                        cmd.Parameters.AddWithValue("@proje", proje);
                        cmd.Parameters.AddWithValue("@malzemeKodIlkKisim", malzemeKodIlkKisim);
                        cmd.Parameters.AddWithValue("@malzemeKodUcuncuKisim", malzemeKodUcuncuKisim);

                        int toplamAdet = 0;
                        int kesilmisAdet = 0;
                        int kesilecekAdetSum = 0;
                        List<string> eslesenPozlar = new List<string>();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                toplamAdet += reader["toplamAdet"] != DBNull.Value ? Convert.ToInt32(reader["toplamAdet"]) : 0;
                                kesilmisAdet += reader["kesilmisAdet"] != DBNull.Value ? Convert.ToInt32(reader["kesilmisAdet"]) : 0;
                                kesilecekAdetSum += reader["kesilecekAdet"] != DBNull.Value ? Convert.ToInt32(reader["kesilecekAdet"]) : 0;
                                string malzemeKod = reader["malzemeKod"]?.ToString() ?? "";
                                eslesenPozlar.Add($"{kalite}-{malzeme}-{malzemeKod}-{proje}");
                            }
                        }

                        return (toplamAdet, kesilmisAdet, kesilecekAdetSum, eslesenPozlar);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Veri Sorgulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"GetAdetlerVeEslesenPozlar Hata: {ex.Message}");
                return (0, 0, 0, new List<string>());
            }
        }

    }
}
