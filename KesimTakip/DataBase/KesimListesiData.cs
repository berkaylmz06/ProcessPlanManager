using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using KesimTakip.Entitys;

namespace KesimTakip.DataBase
{
    class KesimListesiData
    {
        public static void SaveKesimData(int id, string olusturan, string kesimId, string projeno, string malzeme, string kalite, string[] kaliplar, string[] pozlar, string[] adetler, string eklemeTarihi)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    if (pozlar.Length != adetler.Length || pozlar.Length != kaliplar.Length)
                    {
                        throw new ArgumentException("Pozlar, kalıp numaraları ve adet sayıları eşleşmiyor!");
                    }

                    for (int i = 0; i < pozlar.Length; i++)
                    {
                        string temizPoz = pozlar[i].Trim();
                        string temizKalip = kaliplar[i].Trim();
                        string temizAdet = adetler[i].Trim();

                        if (!string.IsNullOrEmpty(temizPoz) && !string.IsNullOrEmpty(temizKalip) && !string.IsNullOrEmpty(temizAdet))
                        {
                            string query = "INSERT INTO KesimListesi (id, olusturan, kesimId, projeNo, malzeme, kalite, kalipNo, kesilecekPozlar, kpAdetSayilari, eklemeTarihi) " +
                                           "VALUES (@id, @olusturan, @kesimId, @projeNo, @malzeme, @kalite, @kalipNo, @kesilecekPozlar, @kpAdetSayilari, @eklemeTarihi)";

                            using (var cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@olusturan", olusturan);
                                cmd.Parameters.AddWithValue("@kesimId", kesimId);
                                cmd.Parameters.AddWithValue("@projeNo", projeno);
                                cmd.Parameters.AddWithValue("@malzeme", malzeme);
                                cmd.Parameters.AddWithValue("@kalite", kalite);
                                cmd.Parameters.AddWithValue("@kalipNo", temizKalip);
                                cmd.Parameters.AddWithValue("@kesilecekPozlar", temizPoz);
                                cmd.Parameters.AddWithValue("@kpAdetSayilari", temizAdet);
                                cmd.Parameters.AddWithValue("@eklemeTarihi", eklemeTarihi);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        public static List<KesimListesi> GetKesimListesi()
        {
            try
            {
                var veriler = new List<KesimListesi>();
                string query = "SELECT olusturan, kesimId, projeNo, kalite, malzeme, kalipNo, kesilecekPozlar, kpAdetSayilari, eklemeTarihi FROM KesimListesi";

                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                veriler.Add(new KesimListesi
                                {
                                    olusturan = reader.GetString(0),
                                    kesimId = reader.GetString(1),
                                    projeNo = reader.GetString(2),
                                    kalite = reader.GetString(3),
                                    malzeme = reader.GetString(4),
                                    kalipNo = reader.GetString(5),
                                    kesilecekPozlar = reader.GetString(6),
                                    kpAdetSayilari = reader.GetString(7),
                                    eklemeTarihi = reader.GetString(8)
                                });
                            }
                        }
                    }
                }
                return veriler;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetirKesimListesi(string kesimId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM KesimListesi WHERE kesimId = @kesimId";

                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@kesimId", kesimId);
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Veri yüklenirken hata oluştu: " + ex.Message);
            }

            return dt;
        }

        public static int GetSiradakiId()
        {
            try
            {
                using (SqlConnection conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT COALESCE(SonId, 0) + 1 
                        FROM IdUretici 
                        WHERE TabloAdi = 'KesimListesi'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"GetSiradakiId hatası: {ex.Message}", ex);
            }
        }

        public static void SiradakiIdKaydet(int id)
        {
            try
            {
                using (SqlConnection conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                IF EXISTS (SELECT 1 FROM IdUretici WHERE TabloAdi = 'KesimListesi')
                BEGIN
                    UPDATE IdUretici SET SonId = @id WHERE TabloAdi = 'KesimListesi'
                END
                ELSE
                BEGIN
                    INSERT INTO IdUretici (TabloAdi, SonId) VALUES ('KesimListesi', @id)
                END";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SiradakiIdKaydet hatası: {ex.Message}", ex);
            }
        }

    }
}
