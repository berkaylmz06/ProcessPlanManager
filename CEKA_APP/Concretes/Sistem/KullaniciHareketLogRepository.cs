using CEKA_APP.Abstracts.Sistem;
using CEKA_APP.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.Concretes.Sistem
{
    public class KullaniciHareketLogRepository : IKullaniciHareketLogRepository
    {
        public void LogEkle(SqlConnection connection, SqlTransaction transaction, int kullaniciId, string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            string query = @"
        INSERT INTO KullaniciHareketLog 
        (kullaniciId, islemTuru, sayfaAdi, tarihSaat, ekBilgi) 
        VALUES 
        (@kullaniciId, @islemTuru, @sayfaAdi, @tarihSaat, @ekBilgi)";

            using (var cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                cmd.Parameters.AddWithValue("@islemTuru", (object)islemTuru ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sayfaAdi", (object)sayfaAdi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tarihSaat", DateTime.Now);
                cmd.Parameters.AddWithValue("@ekBilgi", string.IsNullOrEmpty(ekBilgi) ? (object)DBNull.Value : ekBilgi);

                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetKullaniciLog(SqlConnection connection)
        {
            string query = @"
        SELECT 
            k.kullaniciAdi,
            l.islemTuru,
            l.sayfaAdi,
            l.tarihSaat,
            l.ekBilgi
        FROM KullaniciHareketLog l
        INNER JOIN Kullanicilar k ON l.kullaniciId = k.id
        ORDER BY l.tarihSaat DESC";

            using (var adapter = new SqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
