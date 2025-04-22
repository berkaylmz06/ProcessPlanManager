using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


                string sql = "SELECT adsoyad FROM \"Yoneticiler\" WHERE kullaniciadi = @kullaniciadi AND sifre = @sifre";
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("kullaniciadi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("sifre", sifre);

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
