using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KesimTakip.Entitys;

namespace KesimTakip.DataBase
{
    class KesimDetaylariData
    {
        public static void SaveKesimDetaylariData(string poz,string kesimId, int kesilecekAdet, int toplamAdet)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO KesimDetaylari (poz, kesimId, kesilecekAdet, toplamAdet) " +
                                   "VALUES (@poz, @kesimId, @kesilecekAdet, @toplamAdet)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@poz", poz);
                        cmd.Parameters.AddWithValue("@kesimId", kesimId);
                        cmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                        cmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        public static DataTable GetKesimDetaylari()
        {
            string query = "SELECT poz, kesimId, kesilmisAdet, kesilecekAdet, toplamAdet FROM KesimDetaylari";
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
    }
}
