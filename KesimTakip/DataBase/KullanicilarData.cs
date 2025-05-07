using System;
using System.Data;
using System.Data.SqlClient;

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
    }
}
