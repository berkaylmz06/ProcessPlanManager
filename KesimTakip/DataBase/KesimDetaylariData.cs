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
        public static void SaveKesimDetaylariData(string poz, string kesimId, int kesilecekAdet, int toplamAdet)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM KesimDetaylari WHERE poz = @poz";

                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@poz", poz);
                        checkCmd.Parameters.AddWithValue("@kesimId", kesimId);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            string insertQuery = "INSERT INTO KesimDetaylari (poz, kesimId, kesilecekAdet, toplamAdet) " +
                                                 "VALUES (@poz, @kesimId, @kesilecekAdet, @toplamAdet)";

                            using (var insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@poz", poz);
                                insertCmd.Parameters.AddWithValue("@kesimId", kesimId);
                                insertCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                                insertCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
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
