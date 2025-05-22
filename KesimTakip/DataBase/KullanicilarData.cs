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

                string sql = "SELECT adSoyad FROM [Kullanicilar] WHERE kullaniciAdi = @kullaniciAdi AND sifre = @sifre";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("sifre", sifre);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kullanici = new Kullanicilar()
                            {
                                kullaniciAdi = kullaniciAdi,
                                sifre = sifre,
                                adSoyad = reader.GetString(0)
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

                    string query = @"
                INSERT INTO [Kullanicilar] 
                    ([adSoyad], [kullaniciAdi], [sifre], [kullaniciRol], [email])
                VALUES 
                    (@adSoyad, @kullaniciAdi, @sifre, @kullaniciRol, @email)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@adSoyad", kullanici.adSoyad);
                        command.Parameters.AddWithValue("@kullaniciAdi", kullanici.kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", kullanici.sifre);
                        command.Parameters.AddWithValue("@kullaniciRol", kullanici.kullaniciRol);
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

    }
}
