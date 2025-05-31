using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesimTakip.DataBase
{
    public class KullaniciHareketLogData
    {
        public static void LogEkle(int kullaniciId, string islemTuru, string sayfaAdi, string ekBilgi = "")
        {
            string query = @"
        INSERT INTO KullaniciHareketLog 
        (kullaniciId, islemTuru, sayfaAdi, tarihSaat, ekBilgi) 
        VALUES 
        (@kullaniciId, @islemTuru, @sayfaAdi, @tarihSaat, @ekBilgi)";

            using (var conn = DataBaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                        cmd.Parameters.AddWithValue("@islemTuru", islemTuru ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@sayfaAdi", sayfaAdi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tarihSaat", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ekBilgi", string.IsNullOrEmpty(ekBilgi) ? (object)DBNull.Value : ekBilgi);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Log ekleme sırasında bir hata oluştu.", ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
        public DataTable GetKullaniciLog()
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

            using (var connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    using (var adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Veritabanı sorgusu sırasında bir hata oluştu.", ex);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
        }
    }
}
