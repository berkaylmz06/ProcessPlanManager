using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using KesimTakip.Entitys;

namespace KesimTakip.DataBase
{
    class KullanicilarData
    {
        public Kullanicilar GirisYap(string kullaniciAdi, string sifre)
        {
            Kullanicilar kullanici = null;

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                string sql = "SELECT adSoyad, kullaniciAdi, sifre, kullaniciRol, email FROM [Kullanicilar] WHERE kullaniciAdi = @kullaniciAdi AND sifre = @sifre";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@sifre", sifre);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kullanici = new Kullanicilar()
                            {
                                adSoyad = reader.GetString(0),
                                kullaniciAdi = reader.GetString(1),
                                sifre = reader.GetString(2),
                                kullaniciRol = reader.GetString(3),
                                email = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return kullanici;
        }
        public static DataTable GetKullaniciListesi()
        {
            string query = "SELECT adsoyad, kullaniciAdi, sifre, kullaniciRol, email FROM Kullanicilar";
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
        public static void KullaniciEkle(Kullanicilar kullanici)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    string kontrolQuery = "SELECT COUNT(*) FROM [Kullanicilar] WHERE kullaniciAdi = @kullaniciAdi";
                    using (var kontrolCmd = new SqlCommand(kontrolQuery, connection))
                    {
                        kontrolCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                        int mevcutKullanici = (int)kontrolCmd.ExecuteScalar();

                        if (mevcutKullanici > 0)
                        {
                            return;
                        }
                    }

                    string query = @"
                INSERT INTO [Kullanicilar] 
                    ([adSoyad], [kullaniciAdi], [sifre], [email])
                VALUES 
                    (@adSoyad, @kullaniciAdi, @sifre, @email)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad);
                        command.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", kullanici.sifre);
                        command.Parameters.AddWithValue("@email", kullanici.email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool KullaniciAdiVarMi(string kullaniciAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM [Kullanicilar] WHERE kullaniciAdi = @kullaniciAdi";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public static bool KullaniciSil(string kullaniciAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM [Kullanicilar] WHERE kullaniciAdi = @kullaniciAdi";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        }
        public static bool KullaniciGuncelle(Kullanicilar kullanici)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                string selectQuery = "SELECT adSoyad, sifre, kullaniciRol, email FROM Kullanicilar WHERE kullaniciAdi = @kullaniciAdi";
                using (var selectCmd = new SqlCommand(selectQuery, connection))
                {
                    selectCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);

                    using (var reader = selectCmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return false;
                        }

                        string mevcutAdSoyad = reader["adSoyad"].ToString();
                        string mevcutSifre = reader["sifre"].ToString();
                        string mevcutRol = reader["kullaniciRol"].ToString();
                        string mevcutEmail = reader["email"].ToString();

                        if (mevcutAdSoyad == kullanici.adSoyad &&
                            mevcutSifre == kullanici.sifre &&
                            mevcutRol == kullanici.kullaniciRol &&
                            mevcutEmail == kullanici.email)
                        {
                            return false; 
                        }
                    }
                }

                string updateQuery = @"
            UPDATE Kullanicilar 
            SET adSoyad = @adSoyad, sifre = @sifre, kullaniciRol = @kullaniciRol, email = @email
            WHERE kullaniciAdi = @kullaniciAdi";

                using (var updateCmd = new SqlCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad);
                    updateCmd.Parameters.AddWithValue("@sifre", kullanici.sifre);
                    updateCmd.Parameters.AddWithValue("@kullaniciRol", kullanici.kullaniciRol);
                    updateCmd.Parameters.AddWithValue("@email", kullanici.email);
                    updateCmd.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);

                    updateCmd.ExecuteNonQuery();
                }

                return true;
            }
        }


    }
}
