using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase
{
    class KesimTamamlanmisHareket
    {
        public static bool TablodanKesimTamamlanmisHareketEkleme(string kesimYapan, string kesimId, int kesilenAdet, DateTime kesimTarihi, TimeSpan kesimSaati)
        {
            try
            {
                using (var connection = DataBaseHelper.GetConnection())
                {
                    connection.Open();

                    string query = @"
            INSERT INTO [KesimTamamlanmisHareket] 
                ([kesimYapan], [kesimId], [kesilenAdet], [kesimTarihi], [kesimSaati])
            VALUES 
                (@kesimYapan, @kesimId, @kesilenAdet, @kesimTarihi, @kesimSaati)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kesimYapan", kesimYapan);
                        command.Parameters.AddWithValue("@kesimId", kesimId);
                        command.Parameters.AddWithValue("@kesilenAdet", kesilenAdet);
                        command.Parameters.AddWithValue("@kesimTarihi", kesimTarihi);
                        command.Parameters.AddWithValue("@kesimSaati", kesimSaati);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static DataTable GetirKesimTamamlanmisHareket(string kesimId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM KesimTamamlanmisHareket WHERE kesimId = @kesimId";

                    using (var da = new SqlDataAdapter(query, conn))
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
