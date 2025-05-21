using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.DataBase
{
    public static class KullaniciHareketLog
    {
        public static void LogEkle(string kullaniciAdi, string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            using (var conn = DataBaseHelper.GetConnection())
            {
                string query = @"
                INSERT INTO KullaniciHareketLog 
                (kullaniciAdi, islemTuru, tarihSaat, sayfaAdi, ekBilgi) 
                VALUES 
                (@kullaniciAdi, @islemTuru, @tarihSaat, @sayfaAdi, @ekBilgi)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@islemTuru", islemTuru);
                    cmd.Parameters.AddWithValue("@tarihSaat", DateTime.Now);
                    cmd.Parameters.AddWithValue("@sayfaAdi", sayfaAdi);
                    cmd.Parameters.AddWithValue("@ekBilgi", ekBilgi);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
