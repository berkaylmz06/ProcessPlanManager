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
        public List<(int Id, string Adi,string Birim, DateTime Tarih)> GetFiyatlandirmaKalemleri()
        {
            var fiyatlandirmaKalemleri = new List<(int Id, string Adi, string Birim, DateTime Tarih)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT fiyatlandirmaKalemId, kalemAdi, kalemBirim, olusturmaTarihi FROM ProjeFinans_FiyatlandirmaKalemleri";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fiyatlandirmaKalemleri.Add((reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                    }
                }
                connection.Close();
            }
            return fiyatlandirmaKalemleri;
        }

        public int FiyatlandirmaKalemleriEkle(string kalemAdi, string kalemBirim)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_FiyatlandirmaKalemleri (kalemAdi, kalemBirim, olusturmaTarihi) VALUES (@kalemAdi, @kalemBirim, @olusturmaTarihi)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@kalemAdi", kalemAdi);
                    cmd.Parameters.AddWithValue("@kalemBirim", kalemBirim);
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
