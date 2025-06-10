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
        public static void SaveKesimDetaylariData(string kalite, string malzeme, string malzemeKod, string proje, int kesilecekAdet, int toplamAdet)
        {
            try
            {
                using (var conn = DataBaseHelper.GetConnection())
                {
                    conn.Open();

                    string checkQuery = @"SELECT COUNT(*) FROM KesimDetaylari 
                                  WHERE kalite = @kalite AND malzeme = @malzeme 
                                  AND malzemeKod = @malzemeKod AND proje = @proje";

                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@kalite", kalite);
                        checkCmd.Parameters.AddWithValue("@malzeme", malzeme);
                        checkCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                        checkCmd.Parameters.AddWithValue("@proje", proje);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            string insertQuery = @"INSERT INTO KesimDetaylari 
                                           (kalite, malzeme, malzemeKod, proje, kesilecekAdet, toplamAdet) 
                                           VALUES 
                                           (@kalite, @malzeme, @malzemeKod, @proje, @kesilecekAdet, @toplamAdet)";

                            using (var insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@kalite", kalite);
                                insertCmd.Parameters.AddWithValue("@malzeme", malzeme);
                                insertCmd.Parameters.AddWithValue("@malzemeKod", malzemeKod);
                                insertCmd.Parameters.AddWithValue("@proje", proje);
                                insertCmd.Parameters.AddWithValue("@kesilecekAdet", kesilecekAdet);
                                insertCmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        // Not: Eğer varsa else ile update işlemi de eklenebilir.
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
            string query = "SELECT kalite, malzeme, malzemeKod, proje ,kesilmisAdet, kesilecekAdet, toplamAdet FROM KesimDetaylari";
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
        public static bool PozExists(string poz)
        {
            using (SqlConnection connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM KesimDetaylari WHERE poz = @poz";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@poz", poz);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"PozExists Hata: {ex.Message}");
                    return false;
                }
            }
        }

        public static bool UpdateKesilmisAdet(string poz, int sondurum)
        {
            using (SqlConnection connection = DataBaseHelper.GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = @"
                    UPDATE KesimDetaylari 
                    SET kesilmisAdet = kesilmisAdet + @sondurum, 
                        kesilecekAdet = kesilecekAdet - @sondurum 
                    WHERE poz = @poz AND kesilecekAdet >= @sondurum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@sondurum", sondurum);
                        command.Parameters.AddWithValue("@poz", poz);
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Poz: {poz}, Sondurum: {sondurum}, RowsAffected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UpdateKesilmisAdet Hata: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool KayitVarMi(string poz)
        {
            // Veritabanı bağlantınızı ve sorgunuzu buraya ekleyin
            // Örnek bir SQL sorgusu
            string query = "SELECT COUNT(*) FROM KesimDetaylari WHERE poz = @poz";
            using (SqlConnection connection =DataBaseHelper.GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@poz", poz);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0; // Kayıt varsa true döner
            }
        }
    }
}
