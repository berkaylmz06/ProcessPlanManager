using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEKA_APP.DataBase.ProjeFinans
{
    public class ProjeFinans_SevkiyatPaketleriData
    {
        public List<(int Id, string Adi, DateTime Tarih)> GetPaketler()
        {
            var paketler = new List<(int Id, string Adi, DateTime Tarih)>();
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT paketId, paketAdi, olusturmaTarihi FROM ProjeFinans_SevkiyatPaketleri";
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paketler.Add((reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
                    }
                }
                connection.Close();
            }
            return paketler;
        }

        public int PaketEkle(string paketAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProjeFinans_SevkiyatPaketleri (paketAdi, olusturmaTarihi) VALUES (@paketAdi, @olusturmaTarihi)";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@paketAdi", paketAdi);
                    cmd.Parameters.AddWithValue("@olusturmaTarihi", DateTime.Now);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public int GetPaketIdByAdi(string paketAdi)
        {
            using (var connection = DataBaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT paketId FROM ProjeFinans_SevkiyatPaketleri WHERE paketAdi = @paketAdi";
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@paketAdi", paketAdi);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return 0; 
        }
    }
}
