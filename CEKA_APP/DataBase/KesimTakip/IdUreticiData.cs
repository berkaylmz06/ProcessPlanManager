using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase
{
    public class IdUreticiData
    {
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
