using KesimTakip.Entitys;
using System;
using System.Data.SqlClient;

namespace KesimTakip.DataBase
{
    class YoneticiData
    {
        public Yoneticiler GirisYapYonetici(string kullaniciAdi, string sifre)
        {
            Yoneticiler yonetici = null;

            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();

                string sql = "SELECT adSoyad FROM [Yoneticiler] WHERE kullaniciAdi = @kullaniciAdi AND sifre = @sifre";
                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@sifre", sifre);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            yonetici = new Yoneticiler()
                            {
                                kullaniciAdi = kullaniciAdi,
                                sifre = sifre,
                                adSoyad = reader.GetString(0)
                            };
                        }
                    }
                }
            }

            return yonetici;
        }
    }
}
