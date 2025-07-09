using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class FiyatlandirmaKalemleriData
    {
        public List<(int Id, string Adi, DateTime Tarih)> GetFiyatlandirmaKalemleri()
        {
            var fiyatlandirmaKalemleri = new List<(int Id, string Adi, DateTime Tarih)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT fiyatlandirmaKalemId, kalemAdi, olusturmaTarihi FROM ProjeFinans_FiyatlandirmaKalemleri";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fiyatlandirmaKalemleri.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                    }
                }
                connection.Close();
            }
            return fiyatlandirmaKalemleri;
        }

        public int FiyatlandirmaKalemleriEkle(string kalemAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_FiyatlandirmaKalemleri (kalemAdi, olusturmaTarihi) VALUES (@kalemAdi, @olusturmaTarihi)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kalemAdi", kalemAdi);
                    cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public int GetFiyatlandirmaKalemIdByAdi(string kalemAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT fiyatlandirmaKalemId FROM ProjeFinans_FiyatlandirmaKalemleri WHERE kalemAdi = @kalemAdi";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kalemAdi", kalemAdi);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}
