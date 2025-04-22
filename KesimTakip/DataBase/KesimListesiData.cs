using KesimTakip.Entitys;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace KesimTakip.DataBase
{
    class KesimListesiData
    {
        public static void SaveKesimData(string olusturan, int kesimId, string projeno, string kalinlik, string kalite, string[] kaliplar, string[] pozlar, string[] adetler, string eklemeTarihi)
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
                        string query = "INSERT INTO \"KesimListesi\" (\"olusturan\", \"kesimId\", \"projeNo\", \"kalinlik\", \"kalite\", \"kalipNo\", \"kesilecekPozlar\", \"kpAdetSayilari\", \"eklemeTarihi\") " +
                                       "VALUES (@olusturan, @kesimId, @projeNo, @kalinlik, @kalite, @kalipNo, @kesilecekPozlar, @kpAdetSayilari, @eklemeTarihi)";

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@olusturan", olusturan);
                            cmd.Parameters.AddWithValue("@kesimId", kesimId);
                            cmd.Parameters.AddWithValue("@projeNo", projeno);
                            cmd.Parameters.AddWithValue("@kalinlik", kalinlik);
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
        public static List<KesimListesi> GetKesimListesi()
        {
            var veriler = new List<KesimListesi>();
            string query = "SELECT olusturan, \"kesimId\", \"projeNo\", kalite, kalinlik, \"kalipNo\", \"kesilecekPozlar\", \"kpAdetSayilari\", \"eklemeTarihi\" FROM \"KesimListesi\"";

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            veriler.Add(new KesimListesi
                            {
                                olusturan = reader.GetString(0),
                                kesimId = reader.GetInt32(1),
                                projeNo = reader.GetString(2),
                                kalite = reader.GetString(3),
                                kalinlik = reader.GetString(4),
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

        public DataTable GetKesimVerileri()
        {
            string query = "SELECT \"kesimId\", olusturan, \"kesimPlaniTekrarSayisi\", \"toplamKesimSayisi\", \"eklemeTarihi\" FROM \"KesimListesi\"";
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                using (var adapter = new NpgsqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

 
       
        public static DataTable GetirKesimListesi(int kesimId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM \"KesimListesi\" WHERE \"kesimId\" = @kesimId";

                    using (var da = new NpgsqlDataAdapter(query, conn))
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
    }
}



